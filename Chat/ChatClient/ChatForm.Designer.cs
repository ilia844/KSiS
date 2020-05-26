namespace ChatClient
{
    partial class ChatForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.lbName = new System.Windows.Forms.Label();
            this.lbPassword = new System.Windows.Forms.Label();
            this.tbName = new System.Windows.Forms.TextBox();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.cbIsConnected = new System.Windows.Forms.CheckBox();
            this.btConnect = new System.Windows.Forms.Button();
            this.btDisconnect = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tbIPAdress = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lbParticipants = new System.Windows.Forms.ListBox();
            this.lbCurrentDialog = new System.Windows.Forms.Label();
            this.tbChatContent = new System.Windows.Forms.RichTextBox();
            this.tbMessageContent = new System.Windows.Forms.RichTextBox();
            this.btSendMessage = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbName
            // 
            this.lbName.AutoSize = true;
            this.lbName.Location = new System.Drawing.Point(24, 29);
            this.lbName.Name = "lbName";
            this.lbName.Size = new System.Drawing.Size(45, 17);
            this.lbName.TabIndex = 0;
            this.lbName.Text = "Name";
            // 
            // lbPassword
            // 
            this.lbPassword.AutoSize = true;
            this.lbPassword.Location = new System.Drawing.Point(24, 67);
            this.lbPassword.Name = "lbPassword";
            this.lbPassword.Size = new System.Drawing.Size(69, 17);
            this.lbPassword.TabIndex = 1;
            this.lbPassword.Text = "Password";
            // 
            // tbName
            // 
            this.tbName.Location = new System.Drawing.Point(126, 29);
            this.tbName.Name = "tbName";
            this.tbName.Size = new System.Drawing.Size(155, 22);
            this.tbName.TabIndex = 2;
            // 
            // tbPassword
            // 
            this.tbPassword.Location = new System.Drawing.Point(126, 67);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.Size = new System.Drawing.Size(155, 22);
            this.tbPassword.TabIndex = 3;
            // 
            // cbIsConnected
            // 
            this.cbIsConnected.AutoSize = true;
            this.cbIsConnected.Location = new System.Drawing.Point(27, 111);
            this.cbIsConnected.Name = "cbIsConnected";
            this.cbIsConnected.Size = new System.Drawing.Size(110, 21);
            this.cbIsConnected.TabIndex = 4;
            this.cbIsConnected.Text = "Is connected";
            this.cbIsConnected.UseVisualStyleBackColor = true;
            // 
            // btConnect
            // 
            this.btConnect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btConnect.Location = new System.Drawing.Point(27, 155);
            this.btConnect.Name = "btConnect";
            this.btConnect.Size = new System.Drawing.Size(119, 33);
            this.btConnect.TabIndex = 5;
            this.btConnect.Text = "Connect";
            this.btConnect.UseVisualStyleBackColor = false;
            // 
            // btDisconnect
            // 
            this.btDisconnect.Location = new System.Drawing.Point(162, 155);
            this.btDisconnect.Name = "btDisconnect";
            this.btDisconnect.Size = new System.Drawing.Size(119, 33);
            this.btDisconnect.TabIndex = 6;
            this.btDisconnect.Text = "Disconnect";
            this.btDisconnect.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 218);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 17);
            this.label1.TabIndex = 7;
            this.label1.Text = "Server IP";
            // 
            // tbIPAdress
            // 
            this.tbIPAdress.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.tbIPAdress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbIPAdress.Location = new System.Drawing.Point(115, 216);
            this.tbIPAdress.Name = "tbIPAdress";
            this.tbIPAdress.Size = new System.Drawing.Size(166, 22);
            this.tbIPAdress.TabIndex = 8;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(324, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(254, 23);
            this.label2.TabIndex = 9;
            this.label2.Text = "Participants";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lbParticipants
            // 
            this.lbParticipants.FormattingEnabled = true;
            this.lbParticipants.ItemHeight = 16;
            this.lbParticipants.Location = new System.Drawing.Point(327, 32);
            this.lbParticipants.Name = "lbParticipants";
            this.lbParticipants.Size = new System.Drawing.Size(251, 228);
            this.lbParticipants.TabIndex = 10;
            // 
            // lbCurrentDialog
            // 
            this.lbCurrentDialog.Location = new System.Drawing.Point(27, 263);
            this.lbCurrentDialog.Name = "lbCurrentDialog";
            this.lbCurrentDialog.Size = new System.Drawing.Size(551, 23);
            this.lbCurrentDialog.TabIndex = 11;
            this.lbCurrentDialog.Text = "Not connected";
            this.lbCurrentDialog.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // tbChatContent
            // 
            this.tbChatContent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.tbChatContent.Location = new System.Drawing.Point(27, 289);
            this.tbChatContent.Name = "tbChatContent";
            this.tbChatContent.Size = new System.Drawing.Size(551, 205);
            this.tbChatContent.TabIndex = 12;
            this.tbChatContent.Text = "";
            // 
            // tbMessageContent
            // 
            this.tbMessageContent.Location = new System.Drawing.Point(27, 493);
            this.tbMessageContent.Name = "tbMessageContent";
            this.tbMessageContent.Size = new System.Drawing.Size(435, 36);
            this.tbMessageContent.TabIndex = 13;
            this.tbMessageContent.Text = "";
            // 
            // btSendMessage
            // 
            this.btSendMessage.Location = new System.Drawing.Point(459, 493);
            this.btSendMessage.Name = "btSendMessage";
            this.btSendMessage.Size = new System.Drawing.Size(119, 36);
            this.btSendMessage.TabIndex = 14;
            this.btSendMessage.Text = "Send";
            this.btSendMessage.UseVisualStyleBackColor = true;
            // 
            // ChatForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.ClientSize = new System.Drawing.Size(605, 541);
            this.Controls.Add(this.btSendMessage);
            this.Controls.Add(this.tbMessageContent);
            this.Controls.Add(this.tbChatContent);
            this.Controls.Add(this.lbCurrentDialog);
            this.Controls.Add(this.lbParticipants);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbIPAdress);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btDisconnect);
            this.Controls.Add(this.btConnect);
            this.Controls.Add(this.cbIsConnected);
            this.Controls.Add(this.tbPassword);
            this.Controls.Add(this.tbName);
            this.Controls.Add(this.lbPassword);
            this.Controls.Add(this.lbName);
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Name = "ChatForm";
            this.Text = "Chat";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbName;
        private System.Windows.Forms.Label lbPassword;
        private System.Windows.Forms.TextBox tbName;
        private System.Windows.Forms.TextBox tbPassword;
        private System.Windows.Forms.CheckBox cbIsConnected;
        private System.Windows.Forms.Button btConnect;
        private System.Windows.Forms.Button btDisconnect;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbIPAdress;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox lbParticipants;
        private System.Windows.Forms.Label lbCurrentDialog;
        private System.Windows.Forms.RichTextBox tbChatContent;
        private System.Windows.Forms.RichTextBox tbMessageContent;
        private System.Windows.Forms.Button btSendMessage;
    }
}

