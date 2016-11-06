namespace UiControls
{
    partial class CaptchaEnterForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.wbCaptcha = new System.Windows.Forms.WebBrowser();
            this.tbValue = new System.Windows.Forms.TextBox();
            this.btEnter = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // wbCaptcha
            // 
            this.wbCaptcha.Location = new System.Drawing.Point(12, 12);
            this.wbCaptcha.MinimumSize = new System.Drawing.Size(20, 20);
            this.wbCaptcha.Name = "wbCaptcha";
            this.wbCaptcha.Size = new System.Drawing.Size(457, 250);
            this.wbCaptcha.TabIndex = 2;
            // 
            // tbValue
            // 
            this.tbValue.Location = new System.Drawing.Point(12, 280);
            this.tbValue.Name = "tbValue";
            this.tbValue.Size = new System.Drawing.Size(376, 20);
            this.tbValue.TabIndex = 0;
            // 
            // btEnter
            // 
            this.btEnter.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btEnter.Location = new System.Drawing.Point(394, 278);
            this.btEnter.Name = "btEnter";
            this.btEnter.Size = new System.Drawing.Size(75, 23);
            this.btEnter.TabIndex = 1;
            this.btEnter.Text = "ВВОД";
            this.btEnter.UseVisualStyleBackColor = true;
            // 
            // CaptchaEnterForm
            // 
            this.AcceptButton = this.btEnter;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(481, 315);
            this.Controls.Add(this.btEnter);
            this.Controls.Add(this.tbValue);
            this.Controls.Add(this.wbCaptcha);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "CaptchaEnterForm";
            this.Text = "Ввод капчи";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.WebBrowser wbCaptcha;
        private System.Windows.Forms.TextBox tbValue;
        private System.Windows.Forms.Button btEnter;
    }
}