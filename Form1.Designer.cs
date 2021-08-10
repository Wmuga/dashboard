using System.Windows.Forms;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.IrcMsgBox = new System.Windows.Forms.RichTextBox();
            this.currentViewersBox = new System.Windows.Forms.RichTextBox();
            this.eventTextBox = new System.Windows.Forms.RichTextBox();
            this.buttonSetSubs = new System.Windows.Forms.Button();
            this.sendMsgButton = new System.Windows.Forms.Button();
            this.msgTextBox = new System.Windows.Forms.RichTextBox();
            this.channelTextBox = new System.Windows.Forms.RichTextBox();
            this.topPanel = new System.Windows.Forms.Panel();
            this.labelDashboard = new System.Windows.Forms.Label();
            this.closeButton = new System.Windows.Forms.Button();
            this.labelChat = new System.Windows.Forms.Label();
            this.labelViewers = new System.Windows.Forms.Label();
            this.labelMessage = new System.Windows.Forms.Label();
            this.startReqButton = new System.Windows.Forms.Button();
            this.endReqButton = new System.Windows.Forms.Button();
            this.skipReqButton = new System.Windows.Forms.Button();
            this.commandLabel = new System.Windows.Forms.Label();
            this.topPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // IrcMsgBox
            // 
            this.IrcMsgBox.BackColor = System.Drawing.Color.FromArgb(((int) (((byte) (0)))), ((int) (((byte) (51)))), ((int) (((byte) (153)))));
            this.IrcMsgBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.IrcMsgBox, "IrcMsgBox");
            this.IrcMsgBox.ForeColor = System.Drawing.SystemColors.Control;
            this.IrcMsgBox.Name = "IrcMsgBox";
            this.IrcMsgBox.ReadOnly = true;
            // 
            // currentViewersBox
            // 
            this.currentViewersBox.BackColor = System.Drawing.Color.FromArgb(((int) (((byte) (0)))), ((int) (((byte) (51)))), ((int) (((byte) (153)))));
            this.currentViewersBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.currentViewersBox, "currentViewersBox");
            this.currentViewersBox.ForeColor = System.Drawing.SystemColors.Control;
            this.currentViewersBox.Name = "currentViewersBox";
            // 
            // eventTextBox
            // 
            this.eventTextBox.BackColor = System.Drawing.Color.FromArgb(((int) (((byte) (0)))), ((int) (((byte) (51)))), ((int) (((byte) (153)))));
            this.eventTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.eventTextBox, "eventTextBox");
            this.eventTextBox.ForeColor = System.Drawing.SystemColors.Control;
            this.eventTextBox.Name = "eventTextBox";
            // 
            // buttonSetSubs
            // 
            this.buttonSetSubs.BackColor = System.Drawing.Color.FromArgb(((int) (((byte) (32)))), ((int) (((byte) (32)))), ((int) (((byte) (32)))));
            this.buttonSetSubs.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int) (((byte) (24)))), ((int) (((byte) (136)))), ((int) (((byte) (156)))));
            resources.ApplyResources(this.buttonSetSubs, "buttonSetSubs");
            this.buttonSetSubs.ForeColor = System.Drawing.Color.FromArgb(((int) (((byte) (24)))), ((int) (((byte) (136)))), ((int) (((byte) (156)))));
            this.buttonSetSubs.Name = "buttonSetSubs";
            this.buttonSetSubs.UseVisualStyleBackColor = false;
            this.buttonSetSubs.Click += new System.EventHandler(this.button1_Click);
            // 
            // sendMsgButton
            // 
            this.sendMsgButton.BackColor = System.Drawing.Color.FromArgb(((int) (((byte) (32)))), ((int) (((byte) (32)))), ((int) (((byte) (32)))));
            this.sendMsgButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int) (((byte) (24)))), ((int) (((byte) (136)))), ((int) (((byte) (156)))));
            resources.ApplyResources(this.sendMsgButton, "sendMsgButton");
            this.sendMsgButton.ForeColor = System.Drawing.Color.FromArgb(((int) (((byte) (24)))), ((int) (((byte) (136)))), ((int) (((byte) (156)))));
            this.sendMsgButton.Name = "sendMsgButton";
            this.sendMsgButton.UseVisualStyleBackColor = false;
            this.sendMsgButton.Click += new System.EventHandler(this.sendMsgButton_Click);
            // 
            // msgTextBox
            // 
            this.msgTextBox.BackColor = System.Drawing.Color.FromArgb(((int) (((byte) (0)))), ((int) (((byte) (51)))), ((int) (((byte) (153)))));
            this.msgTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.msgTextBox, "msgTextBox");
            this.msgTextBox.ForeColor = System.Drawing.SystemColors.Control;
            this.msgTextBox.Name = "msgTextBox";
            // 
            // channelTextBox
            // 
            this.channelTextBox.BackColor = System.Drawing.Color.FromArgb(((int) (((byte) (0)))), ((int) (((byte) (51)))), ((int) (((byte) (153)))));
            this.channelTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.channelTextBox, "channelTextBox");
            this.channelTextBox.ForeColor = System.Drawing.SystemColors.Control;
            this.channelTextBox.Name = "channelTextBox";
            // 
            // topPanel
            // 
            this.topPanel.BackColor = System.Drawing.Color.FromArgb(((int) (((byte) (50)))), ((int) (((byte) (50)))), ((int) (((byte) (50)))));
            this.topPanel.Controls.Add(this.labelDashboard);
            this.topPanel.Controls.Add(this.closeButton);
            resources.ApplyResources(this.topPanel, "topPanel");
            this.topPanel.Name = "topPanel";
            // 
            // labelDashboard
            // 
            resources.ApplyResources(this.labelDashboard, "labelDashboard");
            this.labelDashboard.ForeColor = System.Drawing.Color.FromArgb(((int) (((byte) (24)))), ((int) (((byte) (136)))), ((int) (((byte) (156)))));
            this.labelDashboard.Name = "labelDashboard";
            // 
            // closeButton
            // 
            this.closeButton.BackColor = System.Drawing.Color.FromArgb(((int) (((byte) (50)))), ((int) (((byte) (50)))), ((int) (((byte) (50)))));
            this.closeButton.FlatAppearance.BorderSize = 0;
            resources.ApplyResources(this.closeButton, "closeButton");
            this.closeButton.ForeColor = System.Drawing.Color.FromArgb(((int) (((byte) (129)))), ((int) (((byte) (7)))), ((int) (((byte) (11)))));
            this.closeButton.Name = "closeButton";
            this.closeButton.UseVisualStyleBackColor = false;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // labelChat
            // 
            resources.ApplyResources(this.labelChat, "labelChat");
            this.labelChat.ForeColor = System.Drawing.Color.FromArgb(((int) (((byte) (24)))), ((int) (((byte) (136)))), ((int) (((byte) (156)))));
            this.labelChat.Name = "labelChat";
            // 
            // labelViewers
            // 
            resources.ApplyResources(this.labelViewers, "labelViewers");
            this.labelViewers.ForeColor = System.Drawing.Color.FromArgb(((int) (((byte) (24)))), ((int) (((byte) (136)))), ((int) (((byte) (156)))));
            this.labelViewers.Name = "labelViewers";
            // 
            // labelMessage
            // 
            resources.ApplyResources(this.labelMessage, "labelMessage");
            this.labelMessage.ForeColor = System.Drawing.Color.FromArgb(((int) (((byte) (24)))), ((int) (((byte) (136)))), ((int) (((byte) (156)))));
            this.labelMessage.Name = "labelMessage";
            // 
            // startReqButton
            // 
            this.startReqButton.BackColor = System.Drawing.Color.FromArgb(((int) (((byte) (32)))), ((int) (((byte) (32)))), ((int) (((byte) (32)))));
            this.startReqButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int) (((byte) (24)))), ((int) (((byte) (136)))), ((int) (((byte) (156)))));
            resources.ApplyResources(this.startReqButton, "startReqButton");
            this.startReqButton.ForeColor = System.Drawing.Color.FromArgb(((int) (((byte) (24)))), ((int) (((byte) (136)))), ((int) (((byte) (156)))));
            this.startReqButton.Name = "startReqButton";
            this.startReqButton.UseVisualStyleBackColor = false;
            this.startReqButton.Click += new System.EventHandler(this.startReqButton_Click);
            // 
            // endReqButton
            // 
            this.endReqButton.BackColor = System.Drawing.Color.FromArgb(((int) (((byte) (32)))), ((int) (((byte) (32)))), ((int) (((byte) (32)))));
            this.endReqButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int) (((byte) (24)))), ((int) (((byte) (136)))), ((int) (((byte) (156)))));
            resources.ApplyResources(this.endReqButton, "endReqButton");
            this.endReqButton.ForeColor = System.Drawing.Color.FromArgb(((int) (((byte) (24)))), ((int) (((byte) (136)))), ((int) (((byte) (156)))));
            this.endReqButton.Name = "endReqButton";
            this.endReqButton.UseVisualStyleBackColor = false;
            this.endReqButton.Click += new System.EventHandler(this.endReqButton_Click);
            // 
            // skipReqButton
            // 
            this.skipReqButton.BackColor = System.Drawing.Color.FromArgb(((int) (((byte) (32)))), ((int) (((byte) (32)))), ((int) (((byte) (32)))));
            this.skipReqButton.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int) (((byte) (24)))), ((int) (((byte) (136)))), ((int) (((byte) (156)))));
            resources.ApplyResources(this.skipReqButton, "skipReqButton");
            this.skipReqButton.ForeColor = System.Drawing.Color.FromArgb(((int) (((byte) (24)))), ((int) (((byte) (136)))), ((int) (((byte) (156)))));
            this.skipReqButton.Name = "skipReqButton";
            this.skipReqButton.UseVisualStyleBackColor = false;
            this.skipReqButton.Click += new System.EventHandler(this.skipReqButton_Click);
            // 
            // commandLabel
            // 
            resources.ApplyResources(this.commandLabel, "commandLabel");
            this.commandLabel.ForeColor = System.Drawing.Color.FromArgb(((int) (((byte) (24)))), ((int) (((byte) (136)))), ((int) (((byte) (156)))));
            this.commandLabel.Name = "commandLabel";
            // 
            // Form1
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int) (((byte) (32)))), ((int) (((byte) (32)))), ((int) (((byte) (32)))));
            this.Controls.Add(this.commandLabel);
            this.Controls.Add(this.labelMessage);
            this.Controls.Add(this.labelViewers);
            this.Controls.Add(this.labelChat);
            this.Controls.Add(this.topPanel);
            this.Controls.Add(this.channelTextBox);
            this.Controls.Add(this.msgTextBox);
            this.Controls.Add(this.skipReqButton);
            this.Controls.Add(this.endReqButton);
            this.Controls.Add(this.startReqButton);
            this.Controls.Add(this.sendMsgButton);
            this.Controls.Add(this.buttonSetSubs);
            this.Controls.Add(this.eventTextBox);
            this.Controls.Add(this.currentViewersBox);
            this.Controls.Add(this.IrcMsgBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form1";
            this.ShowIcon = false;
            this.topPanel.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Label commandLabel;

        private System.Windows.Forms.Button skipReqButton;

        private System.Windows.Forms.Button endReqButton;

        private System.Windows.Forms.Button startReqButton;

        private System.Windows.Forms.Label labelViewers;
        private System.Windows.Forms.Label labelMessage;

        private System.Windows.Forms.Label labelChat;

        private System.Windows.Forms.Label labelDashboard;
        

        private System.Windows.Forms.Button closeButton;

        private System.Windows.Forms.Button buttonSetSubs;

        private System.Windows.Forms.Panel topPanel;

        private System.Windows.Forms.Button sendMsgButton;
        private System.Windows.Forms.RichTextBox msgTextBox;
        private System.Windows.Forms.RichTextBox channelTextBox;
        

        private System.Windows.Forms.RichTextBox eventTextBox;

        private System.Windows.Forms.RichTextBox currentViewersBox;
        private System.Windows.Forms.RichTextBox IrcMsgBox;

        #endregion
    }
}