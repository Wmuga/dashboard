namespace Dashboard
{
    partial class Form1
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
            this.IrcMsgBox = new System.Windows.Forms.RichTextBox();
            this.currentViewersBox = new System.Windows.Forms.RichTextBox();
            this.eventTextBox = new System.Windows.Forms.RichTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.sendMsgButton = new System.Windows.Forms.Button();
            this.msgTextBox = new System.Windows.Forms.RichTextBox();
            this.channelTextBox = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // IrcMsgBox
            // 
            this.IrcMsgBox.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.IrcMsgBox.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
            this.IrcMsgBox.Location = new System.Drawing.Point(12, 37);
            this.IrcMsgBox.Name = "IrcMsgBox";
            this.IrcMsgBox.ReadOnly = true;
            this.IrcMsgBox.Size = new System.Drawing.Size(278, 426);
            this.IrcMsgBox.TabIndex = 0;
            this.IrcMsgBox.Text = "";
            // 
            // currentViewersBox
            // 
            this.currentViewersBox.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte) (204)));
            this.currentViewersBox.Location = new System.Drawing.Point(309, 41);
            this.currentViewersBox.Name = "currentViewersBox";
            this.currentViewersBox.Size = new System.Drawing.Size(172, 169);
            this.currentViewersBox.TabIndex = 1;
            this.currentViewersBox.Text = "";
            // 
            // eventTextBox
            // 
            this.eventTextBox.Location = new System.Drawing.Point(309, 257);
            this.eventTextBox.Name = "eventTextBox";
            this.eventTextBox.Size = new System.Drawing.Size(171, 205);
            this.eventTextBox.TabIndex = 2;
            this.eventTextBox.Text = "";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(342, 221);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(101, 25);
            this.button1.TabIndex = 3;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // sendMsgButton
            // 
            this.sendMsgButton.Location = new System.Drawing.Point(506, 415);
            this.sendMsgButton.Name = "sendMsgButton";
            this.sendMsgButton.Size = new System.Drawing.Size(100, 32);
            this.sendMsgButton.TabIndex = 4;
            this.sendMsgButton.Text = "send";
            this.sendMsgButton.UseVisualStyleBackColor = true;
            this.sendMsgButton.Click += new System.EventHandler(this.sendMsgButton_Click);
            // 
            // msgTextBox
            // 
            this.msgTextBox.Location = new System.Drawing.Point(506, 98);
            this.msgTextBox.Name = "msgTextBox";
            this.msgTextBox.Size = new System.Drawing.Size(99, 286);
            this.msgTextBox.TabIndex = 5;
            this.msgTextBox.Text = "";
            // 
            // channelTextBox
            // 
            this.channelTextBox.Location = new System.Drawing.Point(506, 37);
            this.channelTextBox.Name = "channelTextBox";
            this.channelTextBox.Size = new System.Drawing.Size(99, 41);
            this.channelTextBox.TabIndex = 6;
            this.channelTextBox.Text = "";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(645, 475);
            this.Controls.Add(this.channelTextBox);
            this.Controls.Add(this.msgTextBox);
            this.Controls.Add(this.sendMsgButton);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.eventTextBox);
            this.Controls.Add(this.currentViewersBox);
            this.Controls.Add(this.IrcMsgBox);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Button sendMsgButton;
        private System.Windows.Forms.RichTextBox msgTextBox;
        private System.Windows.Forms.RichTextBox channelTextBox;

        private System.Windows.Forms.Button button1;

        private System.Windows.Forms.RichTextBox eventTextBox;

        private System.Windows.Forms.RichTextBox currentViewersBox;
        private System.Windows.Forms.RichTextBox IrcMsgBox;

        #endregion
    }
}