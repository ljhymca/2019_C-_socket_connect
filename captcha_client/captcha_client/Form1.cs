using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace captcha_client
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            LoadCaptcha();
        }
        int OutCount = 0;
        string randomString = "";
        private void LoadCaptcha()
        {
            char[] letters = "abcdefghijklmnopqrstuvwxyz1234567890".ToCharArray();

            Random r1 = new Random();
            for (int i = 0; i < 10; i++)
            {

                randomString += letters[r1.Next(0, 34)].ToString();
                var img = new Bitmap(this.chapcha_show.Width, this.chapcha_show.Height);
                var font = new Font("맑은 고딕", 35, FontStyle.Strikeout, GraphicsUnit.Pixel);
                var graphics = Graphics.FromImage(img);
                graphics.DrawString(randomString.ToString(), font, Brushes.Blue, new Point(0, 0));
                chapcha_show.Image = img;
            }

            //num = r1.next(1000, 9999);
            //var img = new bitmap(this.chapcha_show.width,this.chapcha_show.height);
            //var font = new font("맑은 고딕", 30, fontstyle.bold, graphicsunit.pixel);
            //var graphics = graphics.fromimage(img);
            //graphics.drawstring(num.tostring(), font, brushes.blue, new point(0, 0));
            //chapcha_show.image = img;
        }



        private void Reset_Click(object sender, EventArgs e)
        {
            randomString = null;//출력 이미지 값 초기화
            LoadCaptcha();
        }

        private void Check_Click(object sender, EventArgs e)
        {
            if (AnswerBox.Text == randomString)
            {
                MessageBox.Show("휴먼입니다.");
                this.Visible = false;
                ChatForm frm = new ChatForm();
                frm.Owner = this;
                frm.Show();
            }
            else
            {
                MessageBox.Show("다시 해주세요.");
                OutCount++;
                if (OutCount > 4)
                {
                    MessageBox.Show("5회이상 오류입니다. 강제종료 됩니다.");
                    Application.Exit();
                }
            }
        }

        private void Form1_Closing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

    }
}

