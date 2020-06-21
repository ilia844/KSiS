using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using InteractionTools;
using System.Net;
using FileStoringService;

namespace ChatClient
{
    public partial class ChatForm : Form
    {
        private const string FileSharingServerUrl = "http://localhost:8888/";

        Client client;
        FileSharingClient fileSharingClient;
        CommunityData communityData;
        Dictionary<int, int> MatchingDialogs;
        const int CommonDialogID = 0;
        int CurrentDialog;
        int CurrentMessageId;

        public ChatForm()
        {
            InitializeComponent();
        }

        public void HandleMess(LANMessage message)
        {
            switch (message.messageType)
            {
                case MessageType.ClientHistory:
                    {
                        Action action = delegate
                        {
                            communityData = new CommunityData(DialogInfoMethods.ListIntoDictionary(message.Dialogs));
                            CurrentDialog = CommonDialogID;
                            MatchingDialogs = new Dictionary<int, int>();
                            UpdateParticipants();
                            if (CommonDialogID == CurrentDialog)
                                UpdateContent();
                            lbCurrentDialog.Text = communityData.Dialogs[CommonDialogID].Name;
                        };
                        if (InvokeRequired)
                            Invoke(action);
                        else
                            action();
                    }
                    break;
                case MessageType.CommonMess:
                    DialogInfoMethods.AddMessage(communityData.Dialogs[CommonDialogID].MessagesHistory,
                        ref communityData.Dialogs[CommonDialogID].UnreadMessCount,
                        new ChatMessage(message.SenderID, message.SenderName, /*message.IP + "  " +*/ message.content, message.AttachedFiles, DateTime.Now));
                    if (CommonDialogID == CurrentDialog)
                    {
                        UpdateContent();
                        communityData.Dialogs[CommonDialogID].UnreadMessCount--;
                    }
                    else
                    {
                        UpdateParticipants();
                    }
                    break;
                case MessageType.ClientJoin:
                    {
                        Action action = delegate
                        {
                            DialogInfoMethods.AddMessage(communityData.Dialogs[CommonDialogID].MessagesHistory,
                                ref communityData.Dialogs[CommonDialogID].UnreadMessCount,
                                new ChatMessage(message.SenderID, message.SenderName, "  joined", DateTime.Now));
                            if (DialogExists(message.SenderID))
                            {
                                communityData.Dialogs[message.SenderID].IsActive = true;
                            }
                            else
                            {
                                communityData.Dialogs[message.SenderID] = new DialogInfo(message.SenderName, message.SenderID);
                            }

                            if (CommonDialogID == CurrentDialog)
                            {
                                UpdateContent();
                                communityData.Dialogs[CommonDialogID].UnreadMessCount--;
                            }
                            UpdateParticipants();
                        };
                        if (InvokeRequired)
                            Invoke(action);
                        else
                            action();
                    }
                    break;
                case MessageType.ClientExit:
                    communityData.Dialogs[message.SenderID].IsActive = false;
                    DialogInfoMethods.AddMessage(communityData.Dialogs[CommonDialogID].MessagesHistory,
                        ref communityData.Dialogs[CommonDialogID].UnreadMessCount,
                        new ChatMessage(message.SenderID, message.SenderName, "  exit", DateTime.Now));
                    if (CommonDialogID == CurrentDialog)
                    {
                        UpdateContent();
                        communityData.Dialogs[CommonDialogID].UnreadMessCount--;
                    }
                    UpdateParticipants();
                    break;
                case MessageType.PrivateMess:
                    DialogInfoMethods.AddMessage(communityData.Dialogs[message.SenderID].MessagesHistory,
                        ref communityData.Dialogs[message.SenderID].UnreadMessCount,
                        new ChatMessage(message.SenderID, message.SenderName, /*message.IP + "  " +*/ message.content, message.AttachedFiles, DateTime.Now));
                    if (message.SenderID == CurrentDialog)
                    {
                        UpdateContent();
                        communityData.Dialogs[message.SenderID].UnreadMessCount--;
                    }
                    else
                    {
                        UpdateParticipants();
                    }
                    break;
            }
        }

        private void UpdateParticipants()
        {
            Action action = delegate
            {
                lbParticipants.Items.Clear();
                int index = 0;
                MatchingDialogs.Clear();
                foreach (DialogInfo dialog in communityData.Dialogs.Values)
                {
                    MatchingDialogs.Add(index, dialog.Id);
                    lbParticipants.Items.Insert(index, ((dialog.IsActive) ?
                        "[on]" : "[off]") + "  " + dialog.Name + "  " + ((dialog.UnreadMessCount != 0) ?
                        dialog.UnreadMessCount.ToString() : ""));
                    index++;
                }
            };
            if (InvokeRequired)
                Invoke(action);
            else
                action();
        }

        private void UpdateContent()
        {
            Action action = delegate
            {                
                lbChatContent.Items.Clear();

                ListBox[] messages = new ListBox[communityData.Dialogs[MatchingDialogs[CurrentDialog]].MessagesHistory.Count];
                int index = 0;

                foreach (ChatMessage message in communityData.Dialogs[MatchingDialogs[CurrentDialog]].MessagesHistory)
                {
                    AddChatMessage(message);
                    // messages[index] = new ListBox();
                    //messages[index].Items.Add();
                    //string 
                    //lbChatContent.Items. += message.SenderName + ": " + message.Content + "  " + message.Time.ToString() + "\r\n";
                    //messages[index] = new ListBox();
                    //messages[index].BackColor = BackColor;
                    ////messages[index].Items.Add(message.SenderName + ": " + message.Content + "  " + message.Time.ToString() + "\r\n");
                    //messages[index].Items.Add(new Button());
                    //messages[index].Items.Add(new Button());

                    //lbChatContent.Items.Add(messages[index]);
                    //index++;
                }
            };
            if (InvokeRequired)
                Invoke(action);
            else
                action();
        }

        private void AddChatMessage(ChatMessage chatMessage)
        {
            Action action = delegate
            {
                string messageContent = chatMessage.SenderName + ": " + chatMessage.Content + "  " + chatMessage.Time.ToString() + "\r\n";
                lbChatContent.Items.Add(messageContent);
            };
            if (InvokeRequired)
                Invoke(action);
            else
                action();
        }

        private bool DialogExists(int newClientId)
        {
            bool IsExists = false;
            foreach (KeyValuePair<int, DialogInfo> dialog in communityData.Dialogs)
            {
                if (dialog.Key == newClientId)
                {
                    return IsExists = true;
                }
            }
            return IsExists;
        }

        private void btConnect_Click(object sender, EventArgs e)
        {
            client = new Client();
            fileSharingClient = new FileSharingClient();
            fileSharingClient.UpdateFilesListEvent += UpdateAttachedFiles;
            client.MessageReceieved += HandleMess;
            client.SendUDPRequest();
            if (client.IsConnected)
            {
                cbIsConnected.Checked = true;
                tbIPAdress.Text = ((IPEndPoint)client.TCPsocket.RemoteEndPoint).Address.ToString();
                tbName.ReadOnly = tbPassword.ReadOnly = true;
                btConnect.Enabled = false;
                btDisconnect.Enabled = true;
                int hash = Math.Abs(tbPassword.Text.GetHashCode());
                client.JoinChat(tbName.Text, Math.Abs(tbPassword.Text.GetHashCode()));
            }
        }

        private void btDisconnect_Click(object sender, EventArgs e)
        {
            client?.SendMessage(new LANMessage(MessageType.ClientExit, client.ClientId, tbName.Text));
            client?.Disconnect();
            client = null;
            btDisconnect.Enabled = false;
            btSendMessage.Enabled = false;
            foreach (DialogInfo dialog in communityData.Dialogs.Values)
            {
                dialog.IsActive = false;
            }
            UpdateParticipants();
            btConnect.Enabled = true;
            cbIsConnected.Checked = false;
            tbName.ReadOnly = false;
            tbPassword.ReadOnly = false;
        }

        private void ChatForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            client?.SendMessage(new LANMessage(MessageType.ClientExit, client.ClientId, tbName.Text));
            client?.Disconnect();
            client = null;
            communityData = null;
        }

        private void btSendMessage_Click(object sender, EventArgs e)
        {
            if ((tbMessageContent.Text.Length > 0))
            {
                string content = tbMessageContent.Text;
                if (fileSharingClient.filesToSend.Count > 0)
                {
                    content += " [Вложения]";   
                }
                if (CurrentDialog == CommonDialogID)
                {
                    client.SendMessage(new LANMessage(MessageType.CommonMess, content, client.ClientId, ((IPEndPoint)client.TCPsocket.LocalEndPoint).Address.ToString(), GetAttachedFilesId(fileSharingClient.filesToSend)));
                }
                else
                {
                    client.SendMessage(new LANMessage(MessageType.PrivateMess, client.ClientId, MatchingDialogs[CurrentDialog], ((IPEndPoint)client.TCPsocket.LocalEndPoint).Address.ToString(), content, GetAttachedFilesId(fileSharingClient.filesToSend)));
                }
                DialogInfoMethods.AddMessage(communityData.Dialogs[MatchingDialogs[CurrentDialog]].MessagesHistory,
                    ref communityData.Dialogs[MatchingDialogs[CurrentDialog]].UnreadMessCount,
                    new ChatMessage(client.ClientId, "me", content, GetAttachedFilesId(fileSharingClient.filesToSend), DateTime.Now));
                UpdateContent();
                communityData.Dialogs[MatchingDialogs[CurrentDialog]].UnreadMessCount--;
                tbMessageContent.Clear();
                lbMessageFiles.Items.Clear();
                lbAttachedFiles.Items.Clear();
            }
        }

        private List<int> GetAttachedFilesId(Dictionary<int, string> filesToSend)
        {
            List<int> attachedFilesId = new List<int>();
            foreach (KeyValuePair<int, string> file in filesToSend)
            {
                attachedFilesId.Add(file.Key);
            }
            return attachedFilesId;
        }

        private void lbParticipants_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbParticipants.SelectedIndex == -1)
                CurrentDialog = CommonDialogID;
            if (CurrentDialog != lbParticipants.SelectedIndex)
            {
                CurrentDialog = lbParticipants.SelectedIndex;
                lbCurrentDialog.Text = communityData.Dialogs[MatchingDialogs[CurrentDialog]].Name;
                UpdateContent();
                btSendMessage.Enabled = communityData.Dialogs[MatchingDialogs[CurrentDialog]].IsActive;
                communityData.Dialogs[MatchingDialogs[CurrentDialog]].UnreadMessCount = 0;
                UpdateParticipants();
            }
        }

        private async void btLoadFile_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    await fileSharingClient.SendFile(filePath, FileSharingServerUrl);
                    //UpdateAttachedFiles(fileSharingClient.filesToSend);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception: " + ex.Message);
            }
        }

        public void UpdateAttachedFiles(Dictionary<int, string> filesToSend)
        {
            lbAttachedFiles.Items.Clear();
            foreach (KeyValuePair<int, string> file in filesToSend)
            {
                lbAttachedFiles.Items.Add(file.Value);
            }
        }

        private void lbMessageFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            btDownloadFile.Enabled = true;
        }

        private async void lbChatContent_SelectedIndexChanged(object sender, EventArgs e)
        {
            lbMessageFiles.Items.Clear();
            int selectedIndex = lbChatContent.SelectedIndex;
            if (selectedIndex > -1 && selectedIndex < lbChatContent.Items.Count)
            {
                CurrentMessageId = lbChatContent.SelectedIndex;
                ChatMessage message = communityData.Dialogs[MatchingDialogs[CurrentDialog]].MessagesHistory[CurrentMessageId];
                foreach (int fileId in message.AttachedFiles)
                {
                    FileStoringService.FileInfo fileInfo = await fileSharingClient.GetFileInfo(fileId, FileSharingServerUrl);
                    lbMessageFiles.Items.Add(GetStringByFileInfo(fileInfo));
                }
            }
            btDownloadFile.Enabled = false;
        }

        private string GetStringByFileInfo(FileStoringService.FileInfo fileInfo)
        {
            string fileSize = string.Format("{0:F2}", ((double)fileInfo.FileSize / 1024));
            return fileInfo.FileName + " " + fileSize + "KB";
        }

        private async void btRemoveFile_Click(object sender, EventArgs e)
        {
            int selectedFileIndex = lbAttachedFiles.SelectedIndex;
            if (selectedFileIndex > -1 && selectedFileIndex < lbAttachedFiles.Items.Count)
            {
                string fileInfo = lbAttachedFiles.Items[selectedFileIndex].ToString();
                int fileId = fileSharingClient.GetFileIdByFilesToSend(fileInfo);
                if (fileId != -1)
                {
                    await fileSharingClient.DeleteFile(fileId, FileSharingServerUrl);
                }
            }
        }

        private async void btDownloadFile_Click(object sender, EventArgs e)
        {
            int selectedFileIndex = lbMessageFiles.SelectedIndex;
            if (selectedFileIndex > -1 && selectedFileIndex < lbMessageFiles.Items.Count)
            {
                int fileId = communityData.Dialogs[MatchingDialogs[CurrentDialog]].MessagesHistory[CurrentMessageId].AttachedFiles[selectedFileIndex];
                FileForDownload fileForDownload = await fileSharingClient.DownloadFile(fileId, FileSharingServerUrl);
                if (fileForDownload != null)
                {
                    SaveFileDialog saveDialog = new SaveFileDialog();
                    saveDialog.FileName = fileForDownload.FileName;
                    if (saveDialog.ShowDialog() == DialogResult.OK)
                    {
                        string filePath = saveDialog.FileName;
                        using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            fileStream.Write(fileForDownload.FileBytes, 0, fileForDownload.FileBytes.Length);
                        }
                    }
                }
            }
        }
    }
}
