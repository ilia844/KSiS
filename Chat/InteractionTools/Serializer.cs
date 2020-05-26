using System;
using System.Xml.Serialization;
using System.IO;

namespace InteractionTools
{
    public class Serializer
    {
        public byte[] Serialize(LANMessage message)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(LANMessage));
            MemoryStream messageStorage = new MemoryStream();
            serializer.Serialize(messageStorage, message);
            return messageStorage.GetBuffer();
        }

        public LANMessage Deserialize(byte[] data)
        {
            MemoryStream messageStorage = new MemoryStream();
            messageStorage.Write(data, 0, data.Length);  // 0 - смещение
            XmlSerializer serializer = new XmlSerializer(typeof(LANMessage));
            messageStorage.Position = 0;
            LANMessage message = (LANMessage)serializer.Deserialize(messageStorage);
            return message;
        }
    }
}
