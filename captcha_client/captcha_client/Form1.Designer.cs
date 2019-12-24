namespace captcha_client
{
    partial class Form1
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.chapcha_show = new System.Windows.Forms.PictureBox();
            this.AnswerBox = new System.Windows.Forms.TextBox();
            this.Check = new System.Windows.Forms.Button();
            this.Reset = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.chapcha_show)).BeginInit();
            this.SuspendLayout();
            // 
            // chapcha_show
            // 
            this.chapcha_show.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chapcha_show.BackColor = System.Drawing.Color.Transparent;
            this.chapcha_show.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("chapcha_show.BackgroundImage")));
            this.chapcha_show.Location = new System.Drawing.Point(23, 58);
            this.chapcha_show.Name = "chapcha_show";
            this.chapcha_show.Padding = new System.Windows.Forms.Padding(60, 80, 80, 80);
            this.chapcha_show.Size = new System.Drawing.Size(299, 212);
            this.chapcha_show.TabIndex = 0;
            this.chapcha_show.TabStop = false;
            // 
            // AnswerBox
            // 
            this.AnswerBox.Location = new System.Drawing.Point(38, 276);
            this.AnswerBox.Name = "AnswerBox";
            this.AnswerBox.Size = new System.Drawing.Size(240, 21);
            this.AnswerBox.TabIndex = 1;
            // 
            // Check
            // 
            this.Check.Location = new System.Drawing.Point(373, 199);
            this.Check.Name = "Check";
            this.Check.Size = new System.Drawing.Size(149, 98);
            this.Check.TabIndex = 2;
            this.Check.Text = "확인";
            this.Check.UseVisualStyleBackColor = true;
            this.Check.Click += new System.EventHandler(this.Check_Click);
            // 
            // Reset
            // 
            this.Reset.Location = new System.Drawing.Point(373, 58);
            this.Reset.Name = "Reset";
            this.Reset.Size = new System.Drawing.Size(148, 98);
            this.Reset.TabIndex = 3;
            this.Reset.Text = "새로고침";
            this.Reset.UseVisualStyleBackColor = true;
            this.Reset.Click += new System.EventHandler(this.Reset_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 441);
            this.Controls.Add(this.Reset);
            this.Controls.Add(this.Check);
            this.Controls.Add(this.AnswerBox);
            this.Controls.Add(this.chapcha_show);
            this.Name = "Form1";
            this.Text = " ";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_Closing);
            ((System.ComponentModel.ISupportInitialize)(this.chapcha_show)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox chapcha_show;
        private System.Windows.Forms.TextBox AnswerBox;
        private System.Windows.Forms.Button Check;
        private System.Windows.Forms.Button Reset;
    }
}

