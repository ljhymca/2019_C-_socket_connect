using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.IO;

namespace captcha_client
{
    public partial class ChatForm : Form
    {
        delegate void AppendTextDelegate(Control ctrl, string s);
        AppendTextDelegate _textAppender;
        Socket mainSock;
        IPAddress thisAddress;
        string key = "01234567891234560123456789123456";
        string nameID;

        public ChatForm()
        {
            InitializeComponent();
            mainSock = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);
            _textAppender = new AppendTextDelegate
                (AppendText);

        }
        void OnFormLoaded(object sender, EventArgs e)
        {
            if (thisAddress == null)
            {
                // 로컬호스트 주소를 사용한다.
                thisAddress = IPAddress.Loopback;
                txtAddress.Text = thisAddress.ToString();
            }
            else
            {
                thisAddress = IPAddress.Parse(txtAddress.Text);
            }
        }

        public class AsyncObject
        {
            public byte[] Buffer;
            public Socket WorkingSocket;
            public readonly int BufferSize;
            public AsyncObject(int bufferSize)
            {
                BufferSize = bufferSize;
                Buffer = new byte[BufferSize];
            }

            public void ClearBuffer()
            {
                Array.Clear(Buffer, 0, BufferSize);
            }
        }

        private void ChatForm_FormClosing(object sender, FormClosedEventArgs e)
        {
            try
            {
                mainSock.Close();
            }
            catch { }
        }

        void OnConnectToServer(object sender, EventArgs e)
        {
            if (mainSock.Connected)
            {
                MsgBoxHelper.Error("이미 연결되어 있습니다!");
                return;
            }
            int port = 15000; //고정
            nameID = txtID.Text; //ID
            AppendText(txtHistory, string.Format("서버: @{0},port: 15000, ID: @{1}",txtAddress.Text, nameID));
            try
            {
                mainSock.Connect(txtAddress.Text, port);
            }
            catch (Exception ex)
            {
                MsgBoxHelper.Error("연결에 실패했습니다!\n오류 내용: {0}",
                MessageBoxButtons.OK, ex.Message);
                return;
            }
            // 연결 완료되었다는 메세지를 띄워준다.
            AppendText(txtHistory, "서버와 연결되었습니다.");
            // 연결 완료, 서버에서 데이터가 올 수 있으므로 수신 대기한다.
            AsyncObject obj = new AsyncObject(4096);
            obj.WorkingSocket = mainSock;
            mainSock.BeginReceive(obj.Buffer, 0, obj.BufferSize, 0,
            DataReceived, obj);
        }
        void DataReceived(IAsyncResult ar)
        {
            AsyncObject obj = (AsyncObject)ar.AsyncState;
            try
            {
                int received = obj.WorkingSocket.EndReceive(ar);//오류 해결

                if (received <= 0)
                {
                    obj.WorkingSocket.Close();
                    return;
                }

                string text = Encoding.UTF8.GetString(obj.Buffer);

                string[] tokens = text.Split(':');//기준
                string id = Decrypt256(tokens[0], key);//보낸사람 ID

                string[] message;
                message = tokens[1].Split('=');
                message[0] += "==";
                string msg = Decrypt256(message[0], key);//메세지
                // 텍스트박스에 추가해준다.
                // 비동기식으로 작업하기 때문에 폼의 UI 스레드에서 작업을 해줘야 한다.
                // 따라서 대리자를 통해 처리한다.
                //전송 시간 표시 추가.
                AppendText(txtHistory, string.Format(DateTime.Now.ToString("HH:mm:ss") + "[받음]{0}: {1}", id, msg));

                // 클라이언트에선 데이터를 전달해줄 필요가 없으므로 바로 수신 대기한다.
                // 데이터를 받은 후엔 다시 버퍼를 비워주고 같은 방법으로 수신을 대기한다.
                obj.ClearBuffer();
                // 수신 대기
                obj.WorkingSocket.BeginReceive(obj.Buffer, 0, 4096, 0,
                DataReceived, obj);
            }
            catch//오류시 폼 종료
            {
                //Application.Exit();
            }
           
        }
        void OnSendData(object sender, EventArgs e)
        {
            // 서버가 대기중인지 확인한다.
            if (!mainSock.IsBound)
            {
                MsgBoxHelper.Warn("서버가 실행되고 있지 않습니다!");
                return;
            }
            // 보낼 텍스트
            string tts = txtTTS.Text.Trim();
            if (string.IsNullOrEmpty(tts))
            {
                MsgBoxHelper.Warn("텍스트가 입력되지 않았습니다!");
                txtTTS.Focus();
                return;
            }
            // ID 와 메세지를 담도록 만든다.
            // 문자열을 utf8 형식의 바이트로 변환한다.
            byte[] bDts = Encoding.UTF8.GetBytes(AESEncrypt256(nameID, key) + ':' + AESEncrypt256(tts,key));
            //이 부분 암호화 필요
            
            // 서버에 전송한다.
            mainSock.Send(bDts);
            // 전송 완료 후 텍스트박스에 추가하고, 원래의 내용은 지운다.
            
            AppendText(txtHistory, string.Format(DateTime.Now.ToString("HH:mm:ss") + "[보냄]{0}: {1}", nameID, tts));
            txtTTS.Clear();
        }
        void AppendText(Control ctrl, string s)
        {
            if (ctrl.InvokeRequired) ctrl.Invoke(_textAppender, ctrl, s);
            else
            {
                string source = ctrl.Text;
                ctrl.Text = source + Environment.NewLine + s;
            }
        }
        public static class MsgBoxHelper
        {
            public static DialogResult Warn(string s,
            MessageBoxButtons buttons =
            MessageBoxButtons.OK,
            params object[] args)
            {
                return MessageBox.Show(f(s, args), "경고", buttons,
                MessageBoxIcon.Exclamation);
            }
            public static DialogResult Error(string s,
            MessageBoxButtons buttons =
            MessageBoxButtons.OK,
            params object[] args)
            {
                return MessageBox.Show(f(s, args), "오류", buttons,
                MessageBoxIcon.Error);
            }
            public static DialogResult Info(string s,
                MessageBoxButtons buttons = MessageBoxButtons.OK,
                params object[] args)
            {
                return MessageBox.Show(f(s, args), "알림", buttons,
                MessageBoxIcon.Information);
            }
            public static DialogResult Show(string s,
            MessageBoxButtons buttons = MessageBoxButtons.OK,
            params object[] args)
            {
                return MessageBox.Show(f(s, args), "알림", buttons, 0);
            }
            static string f(string s, params object[] args)
            {
                if (args == null) return s;
                return string.Format(s, args);
            }
        }
        private void ChatForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //종료
            Application.Exit();
        }
        private string Decrypt256(String Input, String key)

        {

            RijndaelManaged aes = new RijndaelManaged();
            aes.KeySize = 256;
            aes.BlockSize = 128;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            var decrypt = aes.CreateDecryptor();
            byte[] xBuff = null;
            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, decrypt, CryptoStreamMode.Write))
                {
                    byte[] xXml = Convert.FromBase64String(Input);
                    cs.Write(xXml, 0, xXml.Length);
                }
                xBuff = ms.ToArray();
            }
            String Output = Encoding.UTF8.GetString(xBuff);
            return Output;
        }



        private String AESEncrypt256(String Input, String key)
        {
            RijndaelManaged aes = new RijndaelManaged();

            aes.KeySize = 256;
            aes.BlockSize = 128;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            var encrypt = aes.CreateEncryptor(aes.Key, aes.IV);
            byte[] xBuff = null;
            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, encrypt, CryptoStreamMode.Write))
                {
                    byte[] xXml = Encoding.UTF8.GetBytes(Input);
                    cs.Write(xXml, 0, xXml.Length);
                }
                xBuff = ms.ToArray();
            }
            String Output = Convert.ToBase64String(xBuff);
            return Output;

        }
    }
}

