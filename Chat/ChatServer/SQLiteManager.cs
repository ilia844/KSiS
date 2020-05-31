using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InteractionTools;
using System.Data.SQLite;

namespace ChatServer
{
    public class SQLiteManager : IDisposable
    {
        private const string DBFileName = "Data Source=ChatDB.db";
        private const string CreateUsersTable = "CREATE TABLE IF NOT EXISTS users (password_hash INTEGER  PRIMARY KEY,name VARCHAR(20) NOT NULL);";
        private const string CreateMessagesTable = "CREATE TABLE IF NOT EXISTS messages (id INTEGER PRIMARY KEY AUTOINCREMENT, type VARCHAR(20) NOT NULL, id_from INTEGER REFERENCES users(password_hash) ON DELETE CASCADE ON UPDATE CASCADE NOT NULL, id_to INTEGER REFERENCES users(password_hash) ON DELETE CASCADE ON UPDATE CASCADE, date DATETIME NOT NULL, content STRING);";
        private const string LoadClients = "SELECT * FROM users;";
        private const string LoadCommonMessages = "SELECT * FROM messages WHERE type = 'common';";
        private const string FindName = "SELECT name FROM users WHERE password_hash = @password_hash;";
        private const string InsertNewClient = "INSERT INTO users VALUES(@password_hash, @name);";
        private const string InsertNewMessage = "INSERT INTO messages VALUES(NULL, @type, @id_from, @id_to, @date, @content);";
        private const string FindReceivers = "SELECT * FROM messages WHERE type = 'private' and id_from = @id_from;";
        private const string FindPrivateMessages = "SELECT * FROM messages WHERE type = 'private' and id_from = @id_from and id_to = @id_to;";

        private const string IDParametrName = "@password_hash";
        private const string NameParametrName = "@name";
        private const string TypeParametrName = "@type";
        private const string IDFromParametrName = "@id_from";
        private const string IDToParametrName = "@id_to";
        private const string DateParametrName = "@date";
        private const string ContentParametrName = "@content";


        private const int IDColumnIndex = 0;
        private const int NameColumnIndex = 1;
        private const int FromIDColumnIndex = 2;
        private const int ToIDColumnIndex = 3;
        private const int DateColumnIndex = 4;
        private const int ContentColumnIndex = 5;

        private SQLiteConnection dbConnection;
        private SQLiteCommand sqlCommand;

        public SQLiteManager()
        {
            dbConnection = new SQLiteConnection(DBFileName);
            dbConnection.Open();
            sqlCommand = dbConnection.CreateCommand();
            using (var createUsersTable = new SQLiteCommand(CreateUsersTable, dbConnection))
            {
                using (var createMessagesTable = new SQLiteCommand(CreateMessagesTable, dbConnection))
                {
                    createUsersTable.ExecuteNonQuery();
                    createMessagesTable.ExecuteNonQuery();
                }
            }

        }

        public Dictionary<int, Client> GetClients()
        {
            Dictionary<int, Client> clients = new Dictionary<int, Client>();
            sqlCommand.CommandText = LoadClients;

            using (SQLiteDataReader reader = sqlCommand.ExecuteReader())
            {
                int connectionID = 1;
                while (reader.Read())
                {
                    Client client = new Client(reader.GetString(NameColumnIndex), connectionID);
                    client.IsConnected = false;
                    clients.Add(reader.GetInt32(IDColumnIndex), client);
                    connectionID++;
                }
            }
            return clients;
        }

        public List<ChatMessage> GetCommonDialogHistory()
        {
            List<ChatMessage> messageHistory = new List<ChatMessage>();
            sqlCommand.CommandText = LoadCommonMessages;

            using (SQLiteDataReader reader = sqlCommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    ChatMessage message = new ChatMessage(reader.GetInt32(FromIDColumnIndex), GetName(reader.GetInt32(FromIDColumnIndex)), reader.GetString(ContentColumnIndex), reader.GetDateTime(DateColumnIndex));
                    messageHistory.Add(message);
                }
            }
            return messageHistory;
        }

        public string GetName(int id)
        {
            string result;
            SQLiteCommand getName = dbConnection.CreateCommand();
            getName.CommandText = FindName;
            getName.Parameters.AddWithValue(IDParametrName, id);

            result = (string)getName.ExecuteScalar();
            return result;
        }

        public Dictionary<int, DialogInfo> GetPrivateDialogs(int clientID, string clientName)
        {
            Dictionary<int, DialogInfo> privateDialogs = new Dictionary<int, DialogInfo>();
            List<int> receiversId = GetReceiversID(clientID);
            sqlCommand.CommandText = FindPrivateMessages;
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue(IDFromParametrName, clientID);

            foreach (int receiverId in receiversId)
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.Parameters.AddWithValue(IDFromParametrName, clientID);
                privateDialogs.Add(receiverId, new DialogInfo(clientName, receiverId));
                sqlCommand.Parameters.AddWithValue(IDToParametrName, receiverId);
                using (SQLiteDataReader reader = sqlCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        privateDialogs[receiverId].MessagesHistory.Add(new ChatMessage(clientID, clientName, reader.GetString(ContentColumnIndex), reader.GetDateTime(DateColumnIndex)));
                    }
                }
            }
            return privateDialogs;
        }

        private List<int> GetReceiversID(int senderID)
        {
            List<int> receiversId = new List<int>();
            sqlCommand.CommandText = FindReceivers;
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue(IDFromParametrName, senderID);

            using (SQLiteDataReader reader = sqlCommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    int receiverId = reader.GetInt32(ToIDColumnIndex);
                    if (!IsReceiverExist(receiverId, receiversId))
                        receiversId.Add(receiverId);
                }
            }
            return receiversId;
        }

        private bool IsReceiverExist(int id, List<int> receivers)
        {
            bool result = false;
            foreach (int i in receivers)
            {
                if (id == i)
                    result = true;
            }
            return result;
        }

        public void AddClient(int passwordHash, string name)
        {
            sqlCommand.CommandText = InsertNewClient;
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue(IDParametrName, passwordHash);
            sqlCommand.Parameters.AddWithValue(NameParametrName, name);

            sqlCommand.ExecuteNonQuery();
        }

        public void AddMessage(string type, int idFrom, int idTo, DateTime date, string content)
        {
            sqlCommand.CommandText = InsertNewMessage;
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue(TypeParametrName, type);
            sqlCommand.Parameters.AddWithValue(IDFromParametrName, idFrom);
            sqlCommand.Parameters.AddWithValue(IDToParametrName, idTo);
            sqlCommand.Parameters.AddWithValue(DateParametrName, date);
            sqlCommand.Parameters.AddWithValue(ContentParametrName, content);

            sqlCommand.ExecuteNonQuery();
        }
        
        public void Dispose()
        {
            dbConnection.Dispose();
        }
    }
}

