using System;
using System.Collections.Generic;

namespace InteractionTools
{
    public enum MessageType { UDPRequest, UDPResponse, PrivateMess, CommonMess, Identification, ClientJoin, ClientExit, ClientHistory, MessageSize, BufferSize };

    [Serializable]
    public class LANMessage
    {
        public MessageType messageType;
        public int SenderID;
        public string SenderName;
        public int Port;
        public string IP;
        public string content;
        public List<DialogInfo> Dialogs;
        public List<int> AttachedFiles;
        public LANMessage(MessageType messType, int senderId, int receiverId, string ip, string cont, List<int> files)//Приватное сообщение
        {
            messageType = messType;
            SenderID = senderId;
            Port = receiverId;
            IP = ip;
            content = cont;
            AttachedFiles = files;
        }
        public LANMessage(MessageType messType, string ip, int port)
        {
            messageType = messType;
            Port = port;
            IP = ip;
        }
        public LANMessage(MessageType messType, string cont, int senderId, string ip, List<int> files)//Общее сообщение
        {
            messageType = messType;
            SenderName = cont;
            content = cont;
            SenderID = senderId;
            IP = ip;
            AttachedFiles = files;
        }
        public LANMessage(MessageType messType, string cont, int senderId, string ip)
        {
            messageType = messType;
            SenderName = cont;
            content = cont;
            SenderID = senderId;
            IP = ip;
        }
        public LANMessage(MessageType messType, List<DialogInfo> dialogs)
        {
            messageType = messType;
            Dialogs = dialogs;
        }
        public LANMessage(MessageType messType, int newClientId, string name)
        {
            messageType = messType;
            SenderID = newClientId;
            SenderName = name;
        }
        public LANMessage(MessageType messType, int newClientId, DialogInfo dialog)
        {
            messageType = messType;
            SenderID = newClientId;
            Dialogs = new List<DialogInfo>
            {
                dialog
            };
        }

        public LANMessage(MessageType messType, int size)
        {
            messageType = messType;
            Port = size;
        }

        public LANMessage()
        {
        }
    }
}
