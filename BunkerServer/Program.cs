using System;
using System.Threading;
using System.Net.Sockets;
using System.Text;
using System.Net;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace BunkerServer
{
    /* 
     * Клиент -> Сервер
     * 
     *  1234567890123 
     *  LOGIN__CLIENT -> LOGIN = PASSWORD=
     *  REGIST_CLIENT -> LOGIN = PASSWORD= NAME=
     *  LOGIN_DISCONN -> 
     *  
     *  CONNECT__ROOM -> ID_ROOM=   ID_CLIENT=    ID_NAME=
     *  DISCONN__ROOM -> ID_ROOM=   ID_CLIENT=     ID_NAME=
     *   
     *  ONLINE___ROOM -> 
     * 
     *  START____ROOM -> ID_ROOM= ID_CLIENT=  ID_NAME= PERMISSION=
     *  
     *  OPEN_CHARACTE -> ID_ROOM= ID_CLIENT= ID_NAME= CHARATER=
     * 
     *  NEXT_____MOVE -> ID_ROOM= ID_CLIENT= ID_NAME= 
     * 
     *  VOTING___KICK->ID_ROOM = ID_CLIENT =  ID_NAME= VOTE =
     * 
     *  CHAT_SEND_MSG -> ID_ROOM = ID_CLIENT =  ID_NAME=
     *  
     *  CARD____ACTIV -> ID_ROOM = ID_CLIENT =  ID_NAME= CARD=
     * 
     *  Сервер  -> Клиент
     *  1234567890123 
     *  LOGIN_CONNECT -> ID_CLIENT= ID_NAME=
     *  LOGIN_DISCONN 
     *  
     *  CONNECT__ROOM -> ID_ROOM=  ID_CLIENT= ID_NAME=
     *  DISCONN__ROOM -> ID_ROOM=  ID_CLIENT= ID_NAME=
     *  
     *  ONLINE___ROOM -> ID_ROOM= ID_CLIENT(n)=  ID_NAME(n)=
     *  
     *  START____ROOM -> ID_ROOM=
     *  
     *  ALL_INFO_GAME
     *  
     *  OPEN_CHARACTE -> ID_ROOM= ID_CLIENT= ID_NAME= CHARATER=
     *  
     *  NEXT_____MOVE -> ID_ROOM= ID_CLIENT= ID_NAME=
     *  
     *  VOTING___KICK -> ID_ROOM= KICK=
     *  
     *  COUNT____VOTE -> ID_ROOM= ID_NAME= VOTE=
     *  
     *  VOTING_N_KICK
     *  
     *  END__THE_GAME
     *  
     *  ONE_MORE_KICK
     * 
     *  START__VOTING
     *  
     *  NEXT_NEW_MOVE
     *  
     *  CHAT_GET__MSG
     *  
     *  UPDATE___INFO
     *  
     *  START__VOTING
     */

    public class Program
    {
        static infoPerson iPerson = new infoPerson();
        public static Hashtable clientsList = new Hashtable();
        public static Hashtable clientsListAutorisation = new Hashtable();
        static NetworkStream networkStream;
        public static ICollection keys;
        public static ICollection keysAutorisation;

        public static string dataFromClient;
        public static string dataFromClientsize;

        static handleClinet client;
        static string LocationMaps;
        static string[] allinfo = new string[12];
        public static SqlConnection sqlcon = null;

        public static object locker = new object();

        static void Main(string[] args)
        {
            /* String ip;
             int port;

             Console.Write("IP:");
             ip = Console.ReadLine();

             Console.Write("Port:");
             port = Convert.ToInt32(Console.ReadLine());
             TcpListener serverSocket = new TcpListener(IPAddress.Parse(ip), port);
            НУЖНО!
             */

            TcpListener serverSocket = new TcpListener(IPAddress.Parse("192.168.0.66"), 368);
            TcpClient clientSocket = default(TcpClient);
          
            bool NameCheck = true;
            serverSocket.Start();
            string cs = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=D:\\программ\\Bunker\\BunkerServer\\Bunker.mdf;Integrated Security=True;";
            sqlcon = new SqlConnection(cs);
            sqlcon.Open();
           
            Console.WriteLine("Bunker Server Started");
            if (sqlcon.State == ConnectionState.Open) Console.WriteLine("Bunker BD Started ");
            try
            {
                while (true)
                {
                    if (sqlcon.State == ConnectionState.Open)
                    {
                        NameCheck = true;
                        clientSocket = serverSocket.AcceptTcpClient();
                        int ReceiveBufferSize = 6000;

                        
                        dataFromClient = null;
                           
                        byte[] bytesFrom = new byte[ReceiveBufferSize];
                        networkStream = clientSocket.GetStream();
                        networkStream.Read(bytesFrom, 0, 4);
                        string size_get = Encoding.UTF8.GetString(bytesFrom);

                        int size_masssange;
                        bool success = Int32.TryParse(size_get, out size_masssange);
                        if (success)
                        {
                            string massange = "";

                            while (Encoding.UTF8.GetByteCount(massange) != size_masssange)
                            {
                                byte[] massange_byte = new byte[size_masssange];
                                networkStream.Read(massange_byte, 0, size_masssange);
                                massange = massange + Encoding.UTF8.GetString(massange_byte);
                                massange = massange.Trim('\0');
                                if (Encoding.UTF8.GetByteCount(massange) == size_masssange) break;
                            }
                            dataFromClient = massange;
                        }

                            keys = clientsList.Keys;
                            keysAutorisation = clientsListAutorisation.Keys;

                            int sizefromclient = dataFromClient.Length;

                        if (dataFromClient.IndexOf("CONNECT__ROOM", 0, 13) > -1)
                        {
                             SqlDataReader readSql;
                            int first = 0;
                            int next = 0;
                            string[] Get_Info = new string[4];

                            for (int i = 0; i < 4; i++)
                            {
                                first = dataFromClient.IndexOf("{", next);
                                next = dataFromClient.IndexOf("}", first);
                                Get_Info[i] = dataFromClient.Substring(first + 1, next - first - 1);
                                first = first + 1;
                                next = next + 1;
                            }

                            if (Get_Info[3] == "new_room_id") 
                                {

                                    SqlCommand command = new SqlCommand("SELECT * FROM games WHERE room_id =@id_room ", sqlcon);
                                    command.Parameters.AddWithValue("@id_room", Get_Info[0]);
                                    readSql = command.ExecuteReader();

                                    if (readSql.Read() != true)
                                    {
                                        readSql.Close();
                                        SqlCommand insert = new SqlCommand("INSERT INTO games (room_id,person_id,name,status,permission) VALUES(@room_id,@person_id,@name,@status,@permission) ", sqlcon);

                                        insert.Parameters.AddWithValue("room_id", Get_Info[0]);
                                        insert.Parameters.AddWithValue("person_id", Get_Info[1]);
                                        insert.Parameters.AddWithValue("name", Get_Info[2]);
                                        insert.Parameters.AddWithValue("status", "waiting");
                                        insert.Parameters.AddWithValue("permission", "admin");
                                        insert.ExecuteNonQuery();

                                        SqlCommand info_game = new SqlCommand("SELECT * FROM info_game WHERE room =@id_room ", sqlcon);
                                        info_game.Parameters.AddWithValue("@id_room", Get_Info[0]);
                                        readSql = info_game.ExecuteReader();
                                        if (readSql.Read() != true)
                                        {
                                        readSql.Close();
                                        SqlCommand insert_info_game = new SqlCommand("INSERT INTO info_game (room,Id_person,person_name) VALUES(@room_id,@person_id,@name) ", sqlcon);
                                        insert_info_game.Parameters.AddWithValue("room_id", Get_Info[0]);
                                        insert_info_game.Parameters.AddWithValue("person_id", Get_Info[1]);
                                        insert_info_game.Parameters.AddWithValue("name", Get_Info[2]);
                                        insert_info_game.ExecuteNonQuery();
                                        }   

                                    }
                                    else
                                    {
                                        string massage = "LOGIN_DISCONN {#101}{Такая комната уже существует}";
                                        int size = Encoding.UTF8.GetByteCount(massage);

                                        string size_str = "0";
                                        if (size.ToString().Length == 1) size_str = "000" + size;
                                        if (size.ToString().Length == 2) size_str = "00" + size;
                                        if (size.ToString().Length == 3) size_str = "0" + size;
                                        if (size.ToString().Length == 4) size_str = size.ToString();

                                        byte[] broadcastBytes = new byte[4];
                                        broadcastBytes = Encoding.UTF8.GetBytes(size_str);
                                        networkStream.Write(broadcastBytes, 0, broadcastBytes.Length);

                                        broadcastBytes = new byte[size];
                                        broadcastBytes = Encoding.UTF8.GetBytes(massage);
                                        networkStream.Write(broadcastBytes, 0, broadcastBytes.Length);

                                        networkStream.Close();
                                        clientSocket.Close();
                                        readSql.Close();
                                      
                                        NameCheck = false;
                                    }

                                   
                                   

                            }
                            if (Get_Info[3] == "connect_room_id") 
                            {
                                SqlCommand command = new SqlCommand("SELECT * FROM games WHERE room_id =@id_room AND status =@status_game AND person_id!=@person_id", sqlcon);
                                command.Parameters.AddWithValue("@id_room", Get_Info[0]);
                                command.Parameters.AddWithValue("@status_game", "waiting");
                                command.Parameters.AddWithValue("@person_id", Get_Info[1]);
                                readSql = command.ExecuteReader();
                                if (readSql.Read() == true)
                                {
                                        readSql.Close();
                                        SqlCommand insert = new SqlCommand("INSERT INTO games (room_id,person_id,name,status,permission) VALUES(@room_id,@person_id,@name,@status,@permission) ", sqlcon);

                                        insert.Parameters.AddWithValue("room_id", Get_Info[0]);
                                        insert.Parameters.AddWithValue("person_id", Get_Info[1]);
                                        insert.Parameters.AddWithValue("name", Get_Info[2]);
                                        insert.Parameters.AddWithValue("status", "waiting");
                                        insert.Parameters.AddWithValue("permission", "player");
                                        insert.ExecuteNonQuery();

                                    SqlCommand info_game = new SqlCommand("SELECT * FROM info_game WHERE room =@id_room ", sqlcon);
                                    info_game.Parameters.AddWithValue("@id_room", Get_Info[0]);
                                    readSql = info_game.ExecuteReader();
                                    if (readSql.Read() == true)
                                    {
                                        readSql.Close();
                                        SqlCommand insert_info = new SqlCommand("INSERT INTO info_game (room,Id_person,person_name) VALUES(@room_id,@person_id,@name) ", sqlcon);
                                        insert_info.Parameters.AddWithValue("room_id", Get_Info[0]);
                                        insert_info.Parameters.AddWithValue("person_id", Get_Info[1]);
                                        insert_info.Parameters.AddWithValue("name", Get_Info[2]);
                                        insert_info.ExecuteNonQuery();
                                    }
                                    
                                }
                                else
                                {
                                    string massage = "LOGIN_DISCONN {#101}{Игра уже идет}";
                                    int size = Encoding.UTF8.GetByteCount(massage);

                                    string size_str = "0";
                                    if (size.ToString().Length == 1) size_str = "000" + size;
                                    if (size.ToString().Length == 2) size_str = "00" + size;
                                    if (size.ToString().Length == 3) size_str = "0" + size;
                                    if (size.ToString().Length == 4) size_str = size.ToString();

                                    byte[] broadcastBytes = new byte[4];
                                    broadcastBytes = Encoding.UTF8.GetBytes(size_str);
                                    networkStream.Write(broadcastBytes, 0, broadcastBytes.Length);

                                    broadcastBytes = new byte[size];
                                    broadcastBytes = Encoding.UTF8.GetBytes(massage);
                                    networkStream.Write(broadcastBytes, 0, broadcastBytes.Length);

                                    networkStream.Close();
                                    clientSocket.Close();
                                    readSql.Close();

                                    NameCheck = false;
                                }

                                    


                            }
                            if (NameCheck == true)
                                {
                                    string room_player = "{" + Get_Info[0] + "}{" + Get_Info[1] + "}{" + Get_Info[2] + "}";
                                    lock (Program.locker)
                                    {
                                    clientsList.Add(room_player, clientSocket);
                                    Console.WriteLine(room_player + " Joined game room ");
                                    client = new handleClinet();
                                    client.startClient(clientSocket, room_player, clientsList);
                                    clientsList = client.clientsList;
                                    broadcast("", Get_Info[0], "Online");
                                    }
                                }

                        }

                        else if (dataFromClient.IndexOf("LOGIN__CLIENT", 0, 13) > -1)
                            {

                                SqlDataReader readSql;
                                int  first = 0;
                                int next =0;
                                string[] Get_Info = new string[2];

                            for (int i = 0; i < 2; i++)
                            {
                                first = dataFromClient.IndexOf("{", next);
                                next = dataFromClient.IndexOf("}", first);
                                Get_Info[i] = dataFromClient.Substring(first + 1, next - first - 1);
                                first = first + 1;
                                next = next + 1;
                            }
                             
                                SqlCommand command = new SqlCommand("SELECT * FROM person WHERE login =@login AND  password =@password", sqlcon);
                                command.Parameters.AddWithValue("@login", Get_Info[0]);
                                command.Parameters.AddWithValue("@password", Get_Info[1]);
                                readSql = command.ExecuteReader();

                                if (readSql.Read() == true)
                                {
                                    int id_person = (int)readSql["Id"];
                                    string mame = (string)readSql["name"];
                                    readSql.Close();
                                    string dataClient = "{"+ id_person + "}";
                                    string dataClientinfo = "LOGIN_CONNECT " + "{" + id_person + "}" + "{" + mame + "}";

                                    foreach (string s in keysAutorisation)
                                    {                                                                          
                                        if (s.Length >0)
                                        {
                                            string y = s.Substring(1, s.IndexOf("}")-1);
                                            string x = id_person.ToString();

                                            if (y == x)
                                            {
                                                string massage = "LOGIN_DISCONN {#102}{Такой пользователь уже в сети}";
                                                int size = Encoding.UTF8.GetByteCount(massage);

                                                string size_str = "0";
                                                if (size.ToString().Length == 1) size_str = "000" + size;
                                                if (size.ToString().Length == 2) size_str = "00" + size;
                                                if (size.ToString().Length == 3) size_str = "0" + size;
                                                if (size.ToString().Length == 4) size_str = size.ToString();

                                                byte[] broadcastBytes = new byte[4];
                                                broadcastBytes = Encoding.UTF8.GetBytes(size_str);
                                                networkStream.Write(broadcastBytes, 0, broadcastBytes.Length);

                                                broadcastBytes = new byte[size];
                                                broadcastBytes = Encoding.UTF8.GetBytes(massage);
                                                networkStream.Write(broadcastBytes, 0, broadcastBytes.Length);

                                                networkStream.Close();
                                                clientSocket.Close();
                                                readSql.Close();
                                                NameCheck = false;                                         
                                                break;
                                            }
                                        }
                                    }

                                    if (NameCheck == true)
                                    {
                                        lock (Program.locker)
                                        {
                                        clientsListAutorisation.Add(dataClient, clientSocket);
                                        broadcast(dataClientinfo, dataClient, "auth");
                                        Console.WriteLine(dataClient + " Joined menu room ");
                                        client = new handleClinet();
                                        client.startClientAutorisation(clientSocket, dataClient, clientsListAutorisation);
                                        clientsListAutorisation = client.clientsListAutorisation;
                                        keysAutorisation = clientsListAutorisation.Keys;
                                        }
                                    }
                                }
                                else
                                {
                                    string massage = "LOGIN_DISCONN {#101}{Неправильный логин или пороль }";
                                    int size = Encoding.UTF8.GetByteCount(massage);

                                    string size_str = "0";
                                    if (size.ToString().Length == 1) size_str = "000" + size;
                                    if (size.ToString().Length == 2) size_str = "00" + size;
                                    if (size.ToString().Length == 3) size_str = "0" + size;
                                    if (size.ToString().Length == 4) size_str = size.ToString();

                                    byte[] broadcastBytes = new byte[4];
                                    broadcastBytes = Encoding.UTF8.GetBytes(size_str);
                                    networkStream.Write(broadcastBytes, 0, broadcastBytes.Length);

                                    broadcastBytes = new byte[size];
                                    broadcastBytes = Encoding.UTF8.GetBytes(massage);
                                    networkStream.Write(broadcastBytes, 0, broadcastBytes.Length);

                                    networkStream.Close();
                                    clientSocket.Close();
                                    readSql.Close();
                                }

                           }

                        else if (dataFromClient.IndexOf("REGIST_CLIENT", 0, 13) > -1)
                            {
                                SqlDataReader readSql;
                          
                            int first = 0;
                            int next = 0;
                            string[] Get_Info = new string[3];

                            for (int i = 0; i < 3; i++)
                            {
                                first = dataFromClient.IndexOf("{", next);
                                next = dataFromClient.IndexOf("}", first);
                                Get_Info[i] = dataFromClient.Substring(first + 1, next - first - 1);
                                first = first + 1;
                                next = next + 1;
                            }

                                SqlCommand command = new SqlCommand($"SELECT * FROM person WHERE login =@login ", sqlcon);
                                command.Parameters.AddWithValue("@login", Get_Info[0]);
                                readSql = command.ExecuteReader();

                                if (readSql.Read() != true)
                                {
                                    readSql.Close();
                                    SqlDataReader readSqlreg;

                                    SqlCommand commandreg = new SqlCommand("INSERT INTO person (login,password,name) VALUES  (@login,@password,@name)", sqlcon);
                                    commandreg.Parameters.AddWithValue("@login", Get_Info[0]);
                                    commandreg.Parameters.AddWithValue("@password", Get_Info[1]);
                                    commandreg.Parameters.AddWithValue("@name", Get_Info[2]);
                                    commandreg.ExecuteNonQuery();

                                    SqlCommand commandget = new SqlCommand("SELECT * FROM person WHERE login =@login AND  password =@password", sqlcon);
                                    commandget.Parameters.AddWithValue("@login", Get_Info[0]);
                                    commandget.Parameters.AddWithValue("@password", Get_Info[1]);
                                    readSqlreg = commandget.ExecuteReader();

                                    if (readSqlreg.Read() == true)
                                    {
                                    int id_person = (int)readSql["Id"];
                                    string mame = (string)readSql["name"];

                                    string dataClient ="{"+ id_person + "}";
                                    string dataClientinfo = "LOGIN_CONNECT " + " " + "{" + id_person + "}{" + mame + "}";
                                    readSqlreg.Close();
                                    foreach (string s in keysAutorisation)
                                    {

                                        if (s.Length > 0)
                                        {
                                            string y = s.Substring(1, s.IndexOf("}") - 1);
                                            string x = id_person.ToString();

                                            if (y == x)
                                            {
                                                networkStream.Close();
                                                clientSocket.Close();
                                                Console.WriteLine("error");
                                                NameCheck = false;
                                            }
                                        }
                                    }
                                    if (NameCheck == true)
                                        {

                                            lock (Program.locker)
                                            {
                                                clientsListAutorisation.Add(dataClient, clientSocket);
                                                broadcast(dataClientinfo, dataClient, "auth");
                                                Console.WriteLine(dataClient + " Joined menu room ");
                                                client = new handleClinet();
                                                client.startClientAutorisation(clientSocket, dataClient, clientsListAutorisation);
                                                clientsListAutorisation = client.clientsListAutorisation;
                                                keysAutorisation = clientsListAutorisation.Keys;
                                            }
                                       
                                        }
                                    }
                                    else
                                    {
                                        int size = Encoding.UTF8.GetByteCount("LOGIN_DISCONN");
                                        Byte[] Bytes_DISCONN = new byte[size];

         
                                        Bytes_DISCONN = Encoding.UTF8.GetBytes("LOGIN_DISCONN");
                                        networkStream.Write(Bytes_DISCONN, 0, Bytes_DISCONN.Length);

                                        networkStream.Close();
                                        clientSocket.Close();
                                        readSqlreg.Close();

                                    }
                                }
                                else
                                {

                                    string massage = "LOGIN_DISCONN {#100}{Такой пользователь существует}";
                                    int size = Encoding.UTF8.GetByteCount(massage);

                                    string size_str = "0";
                                    if (size.ToString().Length == 1) size_str = "000" + size;
                                    if (size.ToString().Length == 2) size_str = "00" + size;
                                    if (size.ToString().Length == 3) size_str = "0" + size;
                                    if (size.ToString().Length == 4) size_str = size.ToString();

                                    byte[] broadcastBytes = new byte[4];
                                    broadcastBytes = Encoding.UTF8.GetBytes(size_str);
                                    networkStream.Write(broadcastBytes, 0, broadcastBytes.Length);

                                    broadcastBytes = new byte[size];
                                    broadcastBytes = Encoding.UTF8.GetBytes(massage);
                                    networkStream.Write(broadcastBytes, 0, broadcastBytes.Length);

                                    networkStream.Close();
                                    clientSocket.Close();
                                    readSql.Close();
                                }
                            }
                                              

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                networkStream.Close();
                clientSocket.Close();
            }         
        }     
        public static void broadcast(string msg, string room,string case_str)
        {
            lock (locker)
            {
                switch (case_str)
                {
                    case "send":                      
                        int size = Encoding.UTF8.GetByteCount(msg);                  
                        foreach (DictionaryEntry Item in clientsList)
                        {
                            Object obj = new Object();
                            obj = Item.Key;
                            string s = obj.ToString();
                            string sName = room.Substring(0, room.Length);

                            if (s.IndexOf(sName, 1) > -1)
                            {

                                TcpClient broadcastSocket;
                                broadcastSocket = (TcpClient)Item.Value;
                                NetworkStream broadcastStream = broadcastSocket.GetStream();

                                string size_str="0";
                                if (size.ToString().Length == 1) size_str = "000" + size;
                                if (size.ToString().Length == 2) size_str = "00" + size;
                                if (size.ToString().Length == 3) size_str = "0" + size;
                                if (size.ToString().Length == 4) size_str = size.ToString();

                                byte[] broadcastBytes = new byte[4];
                                
                                broadcastBytes = Encoding.UTF8.GetBytes(size_str);
                                broadcastStream.Write(broadcastBytes, 0, broadcastBytes.Length);

                                broadcastBytes = new byte[size];
                                
                                broadcastBytes = Encoding.UTF8.GetBytes(msg);
                                broadcastStream.Write(broadcastBytes, 0, broadcastBytes.Length);

                            }
                        }
                        break;

                    case "auth":
                        int size_auth = Encoding.UTF8.GetByteCount(msg);
                        foreach (DictionaryEntry Item in clientsListAutorisation)
                        {
                            Object obj = new Object();
                            obj = Item.Key;
                            string s = obj.ToString();
                            if (s == room)
                            {

                                TcpClient broadcastSocket;
                                broadcastSocket = (TcpClient)Item.Value;
                                NetworkStream broadcastStream = broadcastSocket.GetStream();

                                string size_str = "0";
                                if (size_auth.ToString().Length == 1) size_str = "000" + size_auth;
                                if (size_auth.ToString().Length == 2) size_str = "00" + size_auth;
                                if (size_auth.ToString().Length == 3) size_str = "0" + size_auth;
                                if (size_auth.ToString().Length == 4) size_str = size_auth.ToString();

                                byte[] broadcastBytes = new byte[4];

                                broadcastBytes = Encoding.UTF8.GetBytes(size_str);
                                broadcastStream.Write(broadcastBytes, 0, broadcastBytes.Length);
                                broadcastBytes = new byte[size_auth];
                                broadcastBytes = Encoding.UTF8.GetBytes(msg);
                                broadcastStream.Write(broadcastBytes, 0, broadcastBytes.Length);

                            }
                        }
                        break;

                    case "Online":
                        string online_names = null;
                        foreach (DictionaryEntry Item in clientsList)
                        {
                            Object obj = new Object();
                            obj = Item.Key;
                            string s = obj.ToString();
                            if (s.IndexOf(room, 1) > -1) online_names = online_names + "(" + s + ") ";
                        }
                        online_names = "ONLINE___ROOM " + online_names;
                        int size_ONLINE = Encoding.UTF8.GetByteCount(online_names);
                        foreach (DictionaryEntry Item in clientsList)
                        {

                            Object obj = new Object();
                            obj = Item.Key;
                            string s = obj.ToString();
                            //Console.WriteLine("Test2" + s);

                            if (s.IndexOf(room, 1) > -1)
                            {
                                //Console.WriteLine("Test2" + s+"  "+ room);
                                TcpClient broadcastSocket;
                                broadcastSocket = (TcpClient)Item.Value;
                                NetworkStream broadcastStream = broadcastSocket.GetStream();
                             

                                string size_str = "0";
                                if (size_ONLINE.ToString().Length == 1) size_str = "000" + size_ONLINE;
                                if (size_ONLINE.ToString().Length == 2) size_str = "00" + size_ONLINE;
                                if (size_ONLINE.ToString().Length == 3) size_str = "0" + size_ONLINE;
                                if (size_ONLINE.ToString().Length == 4 ) size_str = size_ONLINE.ToString();

                                byte[] broadcastBytes = new byte[4];
                               

                                broadcastBytes = Encoding.UTF8.GetBytes(size_str);
                                broadcastStream.Write(broadcastBytes, 0, broadcastBytes.Length);

                                broadcastBytes = new byte[size_ONLINE];
                               

                                broadcastBytes = Encoding.UTF8.GetBytes(online_names);
                                broadcastStream.Write(broadcastBytes, 0, broadcastBytes.Length);
                            }
                        }
                        break;

                    default:
                        Console.WriteLine("Error command");
                        break;
                }
            }
        
        }      
        public static void start_game(string room, string permission)
        {                    
            SqlConnection sql_con_game = null;
            string str_con = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=D:\\программ\\Bunker\\BunkerServer\\Bunker.mdf;Integrated Security=True;";
            sql_con_game = new SqlConnection(str_con);
            sql_con_game.Open();
            if (sql_con_game.State == ConnectionState.Open)
            {

                Console.WriteLine("Bunker BD2 Started ");
                SqlDataReader readSql;

                SqlCommand command = new SqlCommand("SELECT * FROM games WHERE room_id =@id_room AND status=@status_client AND permission=@permission_client", sql_con_game);
                command.Parameters.AddWithValue("@id_room", room);
                command.Parameters.AddWithValue("@status_client", "waiting");
                command.Parameters.AddWithValue("@permission_client", permission);

                readSql = command.ExecuteReader();
                if (readSql.Read() == true)
                {
                    readSql.Close();

                    SqlCommand update = new SqlCommand("UPDATE games SET status=@play WHERE room_id =@room  ", sql_con_game);

                    update.Parameters.AddWithValue("@play", "play");
                    update.Parameters.AddWithValue("@room", room);
                    update.ExecuteNonQuery();

                    SqlCommand empty_voted = new SqlCommand("UPDATE games SET voted_for=@emptyvoted WHERE room_id =@room  ", sql_con_game);

                    empty_voted.Parameters.AddWithValue("@emptyvoted", "empty_voted");
                    empty_voted.Parameters.AddWithValue("@room", room);
                    empty_voted.ExecuteNonQuery();

                    SqlCommand round_set = new SqlCommand("UPDATE games SET round=@round_set WHERE room_id=@room  ", sql_con_game);

                    round_set.Parameters.AddWithValue("@round_set", 0);
                    round_set.Parameters.AddWithValue("@room", room);
                    round_set.ExecuteNonQuery();

                    PlayerInfo(room);
                }
                else
                {
                    readSql.Close();
                }
                sql_con_game.Close();

            }
         
        }                     
        public static void PlayerInfo(string room)
        {
            Location();
            int i = 0;
            int players = 0;
            string allinfoFull = null;
            string NameInfo = null;
            lock (locker)
            {
                foreach (DictionaryEntry Item in clientsList)
                {

                    Object obj = new Object();
                    obj = Item.Key;
                    string s = obj.ToString();

                    if (s.IndexOf(room, 1) > -1)
                    {
                        players++;
                        allinfo[i] = iPerson.AllInfo(s);

                        if (players == 1)
                        {
                            NameInfo = "(" + "{" + players + "}" + s + allinfo[i] + "{yes}" + ")";
                            int first = 0;
                            int next = 0;
                            string[] Get_Info = new string[3];

                            for (int j = 0; j < 3; j++)
                            {
                                first = s.IndexOf("{", next);
                                next = s.IndexOf("}", first);
                                Get_Info[j] = s.Substring(first + 1, next - first - 1);
                                first = first + 1;
                                next = next + 1;
                            }

                            SqlConnection sql_move = null;

                            string str_con = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=D:\\программ\\Bunker\\BunkerServer\\Bunker.mdf;Integrated Security=True;";
                            sql_move = new SqlConnection(str_con);
                            sql_move.Open();
                            if (sql_move.State == ConnectionState.Open)
                            {
                                Console.WriteLine("Bunker BD3 Started ");

                                SqlCommand update = new SqlCommand("UPDATE games SET move=@move_player,game_number=@number,player_status=@p_status WHERE room_id =@room AND person_id=@id_player", sql_move);

                                update.Parameters.AddWithValue("@move_player", "yes");
                                update.Parameters.AddWithValue("@room", room);
                                update.Parameters.AddWithValue("@number", players);
                                update.Parameters.AddWithValue("@id_player", Get_Info[1]);
                                update.Parameters.AddWithValue("@p_status", "play");



                                update.ExecuteReader();
                                sql_move.Close();
                            }


                        }
                        else
                        {
                            NameInfo = "(" + "{" + players + "}" + s + allinfo[i] + "{no}" + ")";


                            int first = 0;
                            int next = 0;
                            string[] Get_Info = new string[3];

                            for (int j = 0; j < 3; j++)
                            {
                                first = s.IndexOf("{", next);
                                next = s.IndexOf("}", first);
                                Get_Info[j] = s.Substring(first + 1, next - first - 1);
                                first = first + 1;
                                next = next + 1;
                            }
                            SqlConnection sql_move = null;

                            string str_con = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=D:\\программ\\Bunker\\BunkerServer\\Bunker.mdf;Integrated Security=True;";
                            sql_move = new SqlConnection(str_con);
                            sql_move.Open();
                            if (sql_move.State == ConnectionState.Open)
                            {
                                Console.WriteLine("Bunker BD3 Started ");

                                SqlCommand update = new SqlCommand("UPDATE games SET move=@move_player,game_number=@number,player_status=@p_status WHERE room_id =@room AND person_id=@id_player", sql_move);

                                update.Parameters.AddWithValue("@move_player", "no");
                                update.Parameters.AddWithValue("@room", room);
                                update.Parameters.AddWithValue("@number", players);
                                update.Parameters.AddWithValue("@id_player", Get_Info[1]);
                                update.Parameters.AddWithValue("@p_status", "play");

                                update.ExecuteReader();
                                sql_move.Close();
                            }

                        }

                        allinfoFull = allinfoFull + NameInfo;
                        i++;

                    }
                }
            }
            allinfoFull = "ALL_INFO_GAME " +"{"+ players + "}" + allinfoFull + "(" + LocationMaps + ")";
      

            broadcast(allinfoFull, room, "send");
            allinfoFull = null;
        } 
        public static void Location()
        {
            string Maps = null;
            string Room = null;
            string Thing = null;

            string PeopleLiveText = "Процент живых людей ";

            string[] LocationMapsBox = { "Наводнение", "Извержение вулканов", "Массовое потепление", "Массовое похолодание", "Вторжение инопланетян" };
            string[] RoomBox = { "столовая", "местерская", "Гидропоника", "Склад оружия", "Мед Блок", "кухня", "водоочистительная станция", "бильярдная", "бассейн", "комната отдыха", "массажный кабинет" };
            string[] ThingBox = { "Набор для шитья", "Набор книг по выживанию", "Набор книг по медицине", "Журналы", "Шахматы", "Набор для очистки воды" };            
            Random rnd = new Random();
            int LocationMapsNumber = rnd.Next(0, 5);
            int Rooms= rnd.Next(1, 5);
            int Things = rnd.Next(1, 4);

            int PeopleLive = rnd.Next(0, 100);

            for (int i =0; i< Rooms; i++)
            {
                int Room_next = rnd.Next(0, RoomBox.Length);
                Room = Room+ "{"+RoomBox[Room_next] + "}";
            }
            for (int i = 0; i < Things; i++)
            {
                int Things_next = rnd.Next(0, ThingBox.Length);
                Thing = Thing + "{" + ThingBox[Things_next] + "}";
            }

            Maps = LocationMapsBox[LocationMapsNumber];
            PeopleLiveText = PeopleLiveText + PeopleLive;

            LocationMaps ="{"+ Rooms + "}"+ "{" + Things + "}" + Room + Thing + "{" + Maps +"}"  +  "{"+ PeopleLiveText + "}";
           
 
        }
        public static void Next_move(string room, bool flag)
        {
            // Console.WriteLine("Next_move");
            bool next_round = false;
            bool error_check = true;
            SqlConnection sql_con_game = null;
            string str_con = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=D:\\программ\\Bunker\\BunkerServer\\Bunker.mdf;Integrated Security=True;";
            sql_con_game = new SqlConnection(str_con);
            sql_con_game.Open();
            if (sql_con_game.State == ConnectionState.Open)
            {


                if (flag == true)
                {
                    SqlDataReader readSql;

                    SqlCommand new_move = new SqlCommand("SELECT MIN(game_number) FROM games WHERE room_id=@room AND player_status=@status_client", sql_con_game);
                    new_move.Parameters.AddWithValue("@room", room);
                    new_move.Parameters.AddWithValue("@status_client", "play");
                    string new_move_player = (string)new_move.ExecuteScalar();

                    SqlCommand check_new_move_player = new SqlCommand("SELECT * FROM games WHERE room_id=@room AND game_number=@number", sql_con_game);
                    check_new_move_player.Parameters.AddWithValue("@room", room);
                    check_new_move_player.Parameters.AddWithValue("@number", new_move_player);
                    readSql = check_new_move_player.ExecuteReader();
                    if (readSql.Read() == true)
                    {
                        readSql.Close();

                        SqlCommand update = new SqlCommand("UPDATE games SET move=@move_last WHERE room_id=@room  AND game_number=@number ", sql_con_game);
                        update.Parameters.AddWithValue("@move_last", "yes");
                        update.Parameters.AddWithValue("@room", room);
                        update.Parameters.AddWithValue("@number", new_move_player);
                        update.ExecuteNonQuery();

                        SqlCommand get_new_move_player = new SqlCommand("SELECT * FROM games WHERE room_id=@room AND game_number=@number", sql_con_game);
                        get_new_move_player.Parameters.AddWithValue("@room", room);
                        get_new_move_player.Parameters.AddWithValue("@number", new_move_player);
                        readSql = get_new_move_player.ExecuteReader();
                        if (readSql.Read() == true)
                        {
                            string dataClientinfo;

                            dataClientinfo = "NEXT_____MOVE " + "{" + room + "}{" + (string)readSql["person_id"] + "}{" + (string)readSql["name"] + "}";
                            readSql.Close();
                            broadcast(dataClientinfo, room, "send");
                        }
                        else
                        {
                            readSql.Close();
                        }
                    }
                    else
                    {
                        readSql.Close();
                    }
                }
                else
                {
                    #region next_move
                    Console.WriteLine("Bunker BD4 Started ");
                    SqlDataReader readSql;
                    SqlCommand command = new SqlCommand("SELECT * FROM games WHERE room_id =@id_room AND status=@status_client AND move=@move_last", sql_con_game);
                    command.Parameters.AddWithValue("@id_room", room);
                    command.Parameters.AddWithValue("@status_client", "play");
                    command.Parameters.AddWithValue("@move_last", "yes");
                    readSql = command.ExecuteReader();
                    if (readSql.Read() == true)
                    {

                        string game_str = readSql["game_number"] + "";
                        int game_namber_act = Int32.Parse(game_str);
                        readSql.Close();

                        SqlCommand update_last = new SqlCommand("UPDATE games SET move=@move_last WHERE game_number=@number AND room_id=@room", sql_con_game);
                        update_last.Parameters.AddWithValue("@move_last", "no");
                        update_last.Parameters.AddWithValue("@number", game_namber_act.ToString());
                        update_last.Parameters.AddWithValue("@room", room);
                        update_last.ExecuteNonQuery();

                        SqlCommand count_player_sql = new SqlCommand("SELECT COUNT(*) FROM games WHERE room_id=@id_room", sql_con_game);
                        count_player_sql.Parameters.AddWithValue("@id_room", room);
                        Int32 count = (Int32)count_player_sql.ExecuteScalar();
                        game_namber_act++;
                        if (game_namber_act > count)
                        {
                            game_namber_act = 1;
                            next_round = true;
                            broadcast("START__VOTING", room, "send");
                        }
                        if (next_round != true)
                        {
                            for (int i = 0; i <= count; i++)
                            {
                                SqlCommand Check_the_playing_players = new SqlCommand("SELECT * FROM games WHERE room_id=@room AND game_number=@number AND player_status=@p_status", sql_con_game);
                                Check_the_playing_players.Parameters.AddWithValue("@room", room);
                                Check_the_playing_players.Parameters.AddWithValue("@number", game_namber_act.ToString());
                                Check_the_playing_players.Parameters.AddWithValue("@p_status", "play");
                                readSql = Check_the_playing_players.ExecuteReader();
                                if (readSql.Read() == true)
                                {
                                    readSql.Close();
                                    error_check = false;
                                    break;
                                }
                                else
                                {
                                    readSql.Close();
                                    game_namber_act++;
                                    if (game_namber_act > count)
                                    {
                                        game_namber_act = 1;
                                    }
                                }
                            }

                            if (error_check == false)
                            {
                                SqlCommand check_new_move_player = new SqlCommand("SELECT * FROM games WHERE room_id=@room AND game_number=@number", sql_con_game);
                                check_new_move_player.Parameters.AddWithValue("@room", room);
                                check_new_move_player.Parameters.AddWithValue("@number", game_namber_act.ToString());
                                readSql = check_new_move_player.ExecuteReader();
                                if (readSql.Read() == true)
                                {
                                    readSql.Close();

                                    SqlCommand update = new SqlCommand("UPDATE games SET move=@move_last WHERE room_id=@room  AND game_number=@number ", sql_con_game);
                                    update.Parameters.AddWithValue("@move_last", "yes");
                                    update.Parameters.AddWithValue("@room", room);
                                    update.Parameters.AddWithValue("@number", game_namber_act.ToString());
                                    update.ExecuteNonQuery();

                                    SqlCommand get_new_move_player = new SqlCommand("SELECT * FROM games WHERE room_id=@room AND game_number=@number", sql_con_game);
                                    get_new_move_player.Parameters.AddWithValue("@room", room);
                                    get_new_move_player.Parameters.AddWithValue("@number", game_namber_act.ToString());
                                    readSql = get_new_move_player.ExecuteReader();
                                    if (readSql.Read() == true)
                                    {
                                        string dataClientinfo;

                                        dataClientinfo = "NEXT_____MOVE " + "{" + room + "}{" + (string)readSql["person_id"] + "}{" + (string)readSql["name"] + "}";
                                        readSql.Close();
                                        broadcast(dataClientinfo, room, "send");
                                    }
                                    else
                                    {
                                        readSql.Close();
                                    }
                                }
                                else
                                {
                                    readSql.Close();
                                }

                            }
                        }
                    }
                    else
                    {
                        readSql.Close();
                    }
                    sql_con_game.Close();

                    #endregion
                }
            }


        }    
        public static void count_vote(string room)
        {
            SqlConnection vote_sql = null;
            string str_con = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=D:\\программ\\Bunker\\BunkerServer\\Bunker.mdf;Integrated Security=True;";
            vote_sql = new SqlConnection(str_con);
            vote_sql.Open();
            if (vote_sql.State == ConnectionState.Open)
            {
                SqlDataReader readSql;
                SqlCommand count_player_sql = new SqlCommand("SELECT COUNT(*) FROM games WHERE room_id=@id_room", vote_sql);
                count_player_sql.Parameters.AddWithValue("@id_room", room);
                Int32 count = (Int32)count_player_sql.ExecuteScalar();
                string new_msg = null;
                for (int i = 1; i <= count; i++)
                {
                    SqlCommand check = new SqlCommand("SELECT * FROM games WHERE game_number=@number AND room_id=@room", vote_sql);
                    check.Parameters.AddWithValue("@room", room);
                    check.Parameters.AddWithValue("@number", i.ToString());
                    readSql = check.ExecuteReader();

                    if (readSql.Read() == true)
                    {
                        string room_id = (string)readSql["room_id"];
                        string player_id = (string)readSql["person_id"];
                        string player_name = (string)readSql["name"];
                        int vote_kick_new = (int)readSql["vote_kick"];
                        string full_name = "{" + room_id + "}{" + player_id + "}{" + player_name + "}";
                        new_msg = new_msg+"(" + full_name + "{" + vote_kick_new + "})";
                        

                        readSql.Close();

                    }
                }
                broadcast("COUNT____VOTE " + new_msg, room, "send");
                vote_sql.Close();
            }
        }
        public static void Voting_kick(string msg, string room, string name, string status)
        {

            int first = 0;
            int next = 0;
            string[] Get_Info = new string[2];

            for (int i = 0; i < 2; i++)
            {
                first = name.IndexOf("{", next);
                next = name.IndexOf("}", first);
                Get_Info[i] = name.Substring(first + 1, next - first - 1);
                first = first + 1;
                next = next + 1;
            }
            string id = msg;

            if (status.IndexOf("skip") > -1)
            {
                SqlConnection vote_sql = null;
                string str_con = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=D:\\программ\\Bunker\\BunkerServer\\Bunker.mdf;Integrated Security=True;";
                vote_sql = new SqlConnection(str_con);
                vote_sql.Open();
                if (vote_sql.State == ConnectionState.Open)
                {
                    SqlDataReader readSql;
                    SqlCommand select_action = new SqlCommand("SELECT * FROM games WHERE person_id=@id AND room_id=@room", vote_sql);
                    select_action.Parameters.AddWithValue("@room", room);
                    select_action.Parameters.AddWithValue("@id", Get_Info[1]);

                    readSql = select_action.ExecuteReader();
                    if (readSql.Read() == true)
                    {
                        int action = (int)readSql["action"];
                        action++;
                        readSql.Close();

                        SqlCommand action_sql = new SqlCommand("UPDATE games SET action=@action_p WHERE room_id=@room and person_id=@id", vote_sql);
                        action_sql.Parameters.AddWithValue("@action_p", action);
                        action_sql.Parameters.AddWithValue("@room", room);
                        action_sql.Parameters.AddWithValue("@id", Get_Info[1]);
                        action_sql.ExecuteNonQuery();
                    }
                    else
                    {
                        readSql.Close();
                    }

                    SqlCommand count_player_sql = new SqlCommand("SELECT COUNT(*) FROM games WHERE room_id=@id_room AND player_status=@status", vote_sql);
                    count_player_sql.Parameters.AddWithValue("@id_room", room);
                    count_player_sql.Parameters.AddWithValue("@status", "play");
                    Int32 count_player = (Int32)count_player_sql.ExecuteScalar();

                    SqlCommand count_action_sql = new SqlCommand("SELECT COUNT(*) FROM games WHERE room_id=@id_room AND player_status=@status AND action=@action_p", vote_sql);
                    count_action_sql.Parameters.AddWithValue("@id_room", room);
                    count_action_sql.Parameters.AddWithValue("@status", "play");
                    count_action_sql.Parameters.AddWithValue("@action_p", 2);
                    Int32 count_action = (Int32)count_action_sql.ExecuteScalar();

                    SqlCommand check = new SqlCommand("SELECT * FROM games WHERE person_id=@id AND room_id=@room", vote_sql);
                    check.Parameters.AddWithValue("@room", room);
                    check.Parameters.AddWithValue("@id", id);
                    readSql = check.ExecuteReader();

                    if (readSql.Read() == true)
                    {
                        string room_id = (string)readSql["room_id"];
                        string player_id = (string)readSql["person_id"];
                        string player_name = (string)readSql["name"];
                        int vote_kick_new = (int)readSql["vote_kick"];
                        string full_name = "{" + room_id + "}{" + player_id + "}{" + player_name + "}";

                        int voted_new = (int)readSql["voted"];
                        readSql.Close();
                        count_vote(room);

                        if (count_action == count_player)
                        {

                            SqlCommand max_vote = new SqlCommand("SELECT MAX(vote_kick) FROM games WHERE room_id=@room", vote_sql);

                            max_vote.Parameters.AddWithValue("@room", room);
                            int max_vote_count = (int)max_vote.ExecuteScalar();

                            SqlCommand vote_kick_count_sql = new SqlCommand("SELECT COUNT(*) FROM games WHERE room_id=@id_room AND vote_kick=@vote_kick_count", vote_sql);
                            vote_kick_count_sql.Parameters.AddWithValue("@id_room", room);
                            vote_kick_count_sql.Parameters.AddWithValue("@vote_kick_count", max_vote_count);

                            Int32 vote_kick_count = (Int32)vote_kick_count_sql.ExecuteScalar();


                            SqlCommand get_player_for_kick = new SqlCommand("SELECT * FROM games WHERE room_id=@room AND vote_kick=@vote_k", vote_sql);
                            get_player_for_kick.Parameters.AddWithValue("@room", room);
                            get_player_for_kick.Parameters.AddWithValue("@vote_k", max_vote_count);

                            readSql = get_player_for_kick.ExecuteReader();
                            if (readSql.Read() == true)
                            {
                                room_id = (string)readSql["room_id"];
                                player_id = (string)readSql["person_id"];
                                player_name = (string)readSql["name"];
                                //VOTING___KICK->ID_ROOM = KICK =
                                readSql.Close();
                                SqlCommand update_info = new SqlCommand("UPDATE games SET vote_kick=@vote_k,voted=@vote WHERE room_id=@room", vote_sql);
                                update_info.Parameters.AddWithValue("@vote_k", (int)0);
                                update_info.Parameters.AddWithValue("@vote", (int)0);
                                update_info.Parameters.AddWithValue("@room", room_id);
                                update_info.ExecuteNonQuery();

                                SqlCommand update_action_sql = new SqlCommand("UPDATE games SET action=@action_p WHERE room_id=@room", vote_sql);
                                update_action_sql.Parameters.AddWithValue("@action_p", (int)0);
                                update_action_sql.Parameters.AddWithValue("@room", room_id);
                                update_action_sql.ExecuteNonQuery();




                                SqlCommand update_voted_for = new SqlCommand("UPDATE games SET voted_for=@voted WHERE room_id=@room", vote_sql);
                                update_voted_for.Parameters.AddWithValue("@voted", "empty_voted");
                                update_voted_for.Parameters.AddWithValue("@room", room);
                                update_voted_for.ExecuteNonQuery();


                                if (vote_kick_count == 1)
                                {
                                    int round = 0;
                                    SqlCommand select_round = new SqlCommand("SELECT * FROM games WHERE room_id=@room ", vote_sql);
                                    select_round.Parameters.AddWithValue("@room", room);
                                    readSql = select_round.ExecuteReader();
                                    if (readSql.Read() == true)
                                    {
                                        round = (int)readSql["round"];
                                        readSql.Close();
                                        round++;
                                    }
                                    else
                                    {
                                        readSql.Close();
                                    }

                                    SqlCommand round_set = new SqlCommand("UPDATE games SET round=@round_set WHERE room_id=@room  ", vote_sql);
                                    round_set.Parameters.AddWithValue("@round_set", round);
                                    round_set.Parameters.AddWithValue("@room", room);
                                    round_set.ExecuteNonQuery();

                                    SqlCommand update_info_player = new SqlCommand("UPDATE games SET player_status=@status WHERE person_id=@id AND room_id=@room", vote_sql);
                                    update_info_player.Parameters.AddWithValue("@status", "kick");
                                    update_info_player.Parameters.AddWithValue("@room", room_id);
                                    update_info_player.Parameters.AddWithValue("@id", player_id);
                                    update_info_player.ExecuteNonQuery();

                                    full_name = "{" + room_id + "}{" + player_id + "}{" + player_name + "}";

                                    SqlCommand playing_players_count_sql = new SqlCommand("SELECT COUNT(*) FROM games WHERE room_id=@id_room AND player_status=@p_status", vote_sql);
                                    playing_players_count_sql.Parameters.AddWithValue("@id_room", room);
                                    playing_players_count_sql.Parameters.AddWithValue("@p_status", "play");
                                    Int32 playing_players_count = (Int32)playing_players_count_sql.ExecuteScalar();

                                    SqlCommand players_count_sql = new SqlCommand("SELECT COUNT(*) FROM games WHERE room_id=@id_room", vote_sql);
                                    players_count_sql.Parameters.AddWithValue("@id_room", room);
                                    Int32 players_count = (Int32)players_count_sql.ExecuteScalar();

                                    SqlCommand players_count_kick_sql = new SqlCommand("SELECT COUNT(*) FROM games WHERE room_id=@id_room AND player_status=@p_status", vote_sql);
                                    players_count_kick_sql.Parameters.AddWithValue("@id_room", room);
                                    players_count_kick_sql.Parameters.AddWithValue("@p_status", "kick");
                                    Int32 players_count_kick = (Int32)players_count_kick_sql.ExecuteScalar();



                                    if (playing_players_count == Math.Ceiling(players_count / 2.0))
                                    {
                                        SqlCommand update_status = new SqlCommand("UPDATE games SET status=@status_game WHERE room_id=@room", vote_sql);
                                        update_status.Parameters.AddWithValue("@status_game", "end");
                                        update_status.Parameters.AddWithValue("@room", room_id);
                                        update_status.ExecuteNonQuery();

                                        SqlCommand update_player_status = new SqlCommand("UPDATE games SET player_status=@status WHERE player_status=@status_play AND room_id=@room", vote_sql);
                                        update_player_status.Parameters.AddWithValue("@status", "win");
                                        update_player_status.Parameters.AddWithValue("@room", room_id);
                                        update_player_status.Parameters.AddWithValue("@status_play", "play");
                                        update_player_status.ExecuteNonQuery();

                                        string send_end_game_ = "END__THE_GAME " + full_name;
                                        broadcast(send_end_game_, room_id, "send");
                                    }
                                    else
                                    {
                                        string send_result_ = "VOTING___KICK " + full_name;

                                        broadcast(send_result_, room_id, "send");
                                        Thread.Sleep(300);

                                        if (players_count_kick != round)
                                        {
                                            string one_more_kick = "ONE_MORE_KICK ";

                                            broadcast(one_more_kick, room_id, "send");

                                        }
                                        else
                                        {
                                            Next_move(room, true);
                                            
                                            /*SqlCommand check_move = new SqlCommand("SELECT * FROM games WHERE person_id=@id AND room_id=@room", vote_sql);
                                            check_move.Parameters.AddWithValue("@room", room);
                                            check_move.Parameters.AddWithValue("@id", player_id);
                                            readSql = check_move.ExecuteReader();

                                            if (readSql.Read() == true)
                                            {
                                                string move = (string)readSql["move"];

                                                if (move.IndexOf("yes") > -1) Next_move(room, true);
                                                readSql.Close();
                                            }
                                            else
                                            {
                                                readSql.Close();
                                            }*/

                                        }

                                    }


                                }
                                else
                                {
                                    int round = 0;
                                    SqlCommand select_round = new SqlCommand("SELECT * FROM games WHERE room_id=@room ", vote_sql);
                                    select_round.Parameters.AddWithValue("@room", room);
                                    readSql = select_round.ExecuteReader();
                                    if (readSql.Read() == true)
                                    {
                                        round = (int)readSql["round"];
                                        readSql.Close();
                                        round++;
                                    }
                                    else
                                    {
                                        readSql.Close();
                                    }

                                    SqlCommand round_set = new SqlCommand("UPDATE games SET round=@round_set WHERE room_id=@room  ", vote_sql);
                                    round_set.Parameters.AddWithValue("@round_set", round);
                                    round_set.Parameters.AddWithValue("@room", room);
                                    round_set.ExecuteNonQuery();

                                    string send_result_ = "VOTING_N_KICK ";
                                    broadcast(send_result_, room_id, "send");
                                }

                            }
                            else
                            {
                                readSql.Close();
                            }
                        }
                    }
                    else
                    {
                        readSql.Close();
                    }

                    vote_sql.Close();
                }
                else 
                {
                    vote_sql.Close();
                }
            }
            else
            {
 
                SqlConnection vote_sql = null;
                string str_con = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=D:\\программ\\Bunker\\BunkerServer\\Bunker.mdf;Integrated Security=True;";
                vote_sql = new SqlConnection(str_con);
                vote_sql.Open();
                if (vote_sql.State == ConnectionState.Open)
                {
                    Console.WriteLine("Bunker BD5 Started ");

                    SqlDataReader readSql;

                    SqlCommand select_p = new SqlCommand("SELECT * FROM games WHERE person_id=@id AND room_id=@room", vote_sql);
                    select_p.Parameters.AddWithValue("@room", room);
                    select_p.Parameters.AddWithValue("@id", id);

                    readSql = select_p.ExecuteReader();
                    if (readSql.Read() == true)
                    {
                        string player_status = (string)readSql["player_status"];
                        int vote_kick = (int)readSql["vote_kick"];
                        int voted = (int)readSql["voted"];
                        readSql.Close();

                        SqlCommand select_voted_for = new SqlCommand("SELECT * FROM games WHERE person_id=@id AND room_id=@room", vote_sql);
                        select_voted_for.Parameters.AddWithValue("@room", room);
                        select_voted_for.Parameters.AddWithValue("@id", Get_Info[1]);
                        readSql = select_voted_for.ExecuteReader();

                        if (readSql.Read() == true)
                        {

                            string voted_for_last = (string)readSql["voted_for"];
                            readSql.Close();

                            if (player_status == "play")
                            {
                                bool check_vote = false;

                                SqlCommand select_action = new SqlCommand("SELECT * FROM games WHERE person_id=@id AND room_id=@room", vote_sql);
                                select_action.Parameters.AddWithValue("@room", room);
                                select_action.Parameters.AddWithValue("@id", Get_Info[1]);

                                readSql = select_action.ExecuteReader();
                                if (readSql.Read() == true)
                                {
                                    int action = (int)readSql["action"];
                                    action++;
                                    readSql.Close();

                                    SqlCommand action_sql = new SqlCommand("UPDATE games SET action=@action_p WHERE room_id=@room and person_id=@id", vote_sql);
                                    action_sql.Parameters.AddWithValue("@action_p", action);
                                    action_sql.Parameters.AddWithValue("@room", room);
                                    action_sql.Parameters.AddWithValue("@id", Get_Info[1]);
                                    action_sql.ExecuteNonQuery();
                                }
                                else
                                {
                                    readSql.Close();
                                }

                                if (voted_for_last != id && voted_for_last != "empty_voted")
                                {
                                   
                                    check_vote = true;

                                    SqlCommand select_voted = new SqlCommand("SELECT * FROM games WHERE person_id=@id AND room_id=@room", vote_sql);
                                    select_voted.Parameters.AddWithValue("@room", room);
                                    select_voted.Parameters.AddWithValue("@id", voted_for_last);
                                    readSql = select_voted.ExecuteReader();

                                    if (readSql.Read() == true)
                                    {
                                        int vote_kick_last = (int)readSql["vote_kick"];

                                        vote_kick_last--;

                                        readSql.Close();

                                        SqlCommand update_vote = new SqlCommand("UPDATE games SET vote_kick=@vote_k WHERE person_id=@id AND room_id=@room", vote_sql);
                                        update_vote.Parameters.AddWithValue("@vote_k", vote_kick_last);
                                        update_vote.Parameters.AddWithValue("@room", room);
                                        update_vote.Parameters.AddWithValue("@id", voted_for_last);
                                        update_vote.ExecuteNonQuery();

                                    }
                                    else
                                    {
                                        readSql.Close();
                                    }
                                }
                              
                                if (voted_for_last != id)
                                {
                                   
                                    vote_kick++;
                                    if (check_vote == false) voted++;
                                    SqlCommand write_vote = new SqlCommand("UPDATE games SET vote_kick=@vote_k,voted=@vote WHERE person_id=@id AND room_id=@room", vote_sql);
                                    write_vote.Parameters.AddWithValue("@vote_k", vote_kick);
                                    write_vote.Parameters.AddWithValue("@vote", voted);
                                    write_vote.Parameters.AddWithValue("@room", room);
                                    write_vote.Parameters.AddWithValue("@id", id);
                                    write_vote.ExecuteNonQuery();

                                    SqlCommand write_voted = new SqlCommand("UPDATE games SET voted=@vote WHERE room_id=@room", vote_sql);
                                    write_voted.Parameters.AddWithValue("@vote", voted);
                                    write_voted.Parameters.AddWithValue("@room", room);
                                    write_voted.ExecuteNonQuery();

                                    SqlCommand voted_for = new SqlCommand("UPDATE games SET voted_for=@voted WHERE room_id=@room and person_id=@id", vote_sql);
                                    voted_for.Parameters.AddWithValue("@voted", id);
                                    voted_for.Parameters.AddWithValue("@room", room);
                                    voted_for.Parameters.AddWithValue("@id", Get_Info[1]);

                                    voted_for.ExecuteNonQuery();


                                    SqlCommand count_player_sql = new SqlCommand("SELECT COUNT(*) FROM games WHERE room_id=@id_room AND player_status=@status", vote_sql);
                                    count_player_sql.Parameters.AddWithValue("@id_room", room);
                                    count_player_sql.Parameters.AddWithValue("@status", "play");
                                    Int32 count_player = (Int32)count_player_sql.ExecuteScalar();

                                    SqlCommand count_action_sql = new SqlCommand("SELECT COUNT(*) FROM games WHERE room_id=@id_room AND player_status=@status AND action=@action_p", vote_sql);
                                    count_action_sql.Parameters.AddWithValue("@id_room", room);
                                    count_action_sql.Parameters.AddWithValue("@status", "play");
                                    count_action_sql.Parameters.AddWithValue("@action_p", 2);
                                    Int32 count_action = (Int32)count_action_sql.ExecuteScalar();


                                    SqlCommand check = new SqlCommand("SELECT * FROM games WHERE person_id=@id AND room_id=@room", vote_sql);
                                    check.Parameters.AddWithValue("@room", room);
                                    check.Parameters.AddWithValue("@id", id);
                                    readSql = check.ExecuteReader();

                                    if (readSql.Read() == true)
                                    {
                                        string room_id = (string)readSql["room_id"];
                                        string player_id = (string)readSql["person_id"];
                                        string player_name = (string)readSql["name"];
                                        int vote_kick_new = (int)readSql["vote_kick"];
                                        string full_name = "{" + room_id + "}{" + player_id + "}{" + player_name + "}";
                                      

                                        int voted_new = (int)readSql["voted"];
                                        readSql.Close();
                                        count_vote(room);
                                        if (count_action == count_player)
                                        {

                                            SqlCommand max_vote = new SqlCommand("SELECT MAX(vote_kick) FROM games WHERE room_id=@room", vote_sql);

                                            max_vote.Parameters.AddWithValue("@room", room);
                                            int max_vote_count = (int)max_vote.ExecuteScalar();

                                            SqlCommand vote_kick_count_sql = new SqlCommand("SELECT COUNT(*) FROM games WHERE room_id=@id_room AND vote_kick=@vote_kick_count", vote_sql);
                                            vote_kick_count_sql.Parameters.AddWithValue("@id_room", room);
                                            vote_kick_count_sql.Parameters.AddWithValue("@vote_kick_count", max_vote_count);

                                            Int32 vote_kick_count = (Int32)vote_kick_count_sql.ExecuteScalar();


                                            SqlCommand get_player_for_kick = new SqlCommand("SELECT * FROM games WHERE room_id=@room AND vote_kick=@vote_k", vote_sql);
                                            get_player_for_kick.Parameters.AddWithValue("@room", room);
                                            get_player_for_kick.Parameters.AddWithValue("@vote_k", max_vote_count);

                                            readSql = get_player_for_kick.ExecuteReader();
                                            if (readSql.Read() == true)
                                            {
                                                room_id = (string)readSql["room_id"];
                                                player_id = (string)readSql["person_id"];
                                                player_name = (string)readSql["name"];
                                                //VOTING___KICK->ID_ROOM = KICK =
                                                readSql.Close();
                                                SqlCommand update_info = new SqlCommand("UPDATE games SET vote_kick=@vote_k,voted=@vote WHERE room_id=@room", vote_sql);
                                                update_info.Parameters.AddWithValue("@vote_k", (int)0);
                                                update_info.Parameters.AddWithValue("@vote", (int)0);
                                                update_info.Parameters.AddWithValue("@room", room_id);
                                                update_info.ExecuteNonQuery();

                                                SqlCommand update_action_sql = new SqlCommand("UPDATE games SET action=@action_p WHERE room_id=@room", vote_sql);
                                                update_action_sql.Parameters.AddWithValue("@action_p", (int)0);
                                                update_action_sql.Parameters.AddWithValue("@room", room_id);
                                                update_action_sql.ExecuteNonQuery();

                                                SqlCommand update_voted_for = new SqlCommand("UPDATE games SET voted_for=@voted WHERE room_id=@room", vote_sql);
                                                update_voted_for.Parameters.AddWithValue("@voted", "empty_voted");
                                                update_voted_for.Parameters.AddWithValue("@room", room);
                                                update_voted_for.ExecuteNonQuery();


                                                if (vote_kick_count == 1)
                                                {
                                                    int round = 0;
                                                    SqlCommand select_round = new SqlCommand("SELECT * FROM games WHERE room_id=@room ", vote_sql);
                                                    select_round.Parameters.AddWithValue("@room", room);
                                                    readSql = select_round.ExecuteReader();
                                                    if (readSql.Read() == true)
                                                    {
                                                        round = (int)readSql["round"];
                                                        readSql.Close();
                                                        round++;
                                                    }
                                                    else
                                                    {
                                                        readSql.Close();
                                                    }

                                                    SqlCommand round_set = new SqlCommand("UPDATE games SET round=@round_set WHERE room_id=@room  ", vote_sql);
                                                    round_set.Parameters.AddWithValue("@round_set", round);
                                                    round_set.Parameters.AddWithValue("@room", room);
                                                    round_set.ExecuteNonQuery();

                                                    SqlCommand update_info_player = new SqlCommand("UPDATE games SET player_status=@status WHERE person_id=@id AND room_id=@room", vote_sql);
                                                    update_info_player.Parameters.AddWithValue("@status", "kick");
                                                    update_info_player.Parameters.AddWithValue("@room", room_id);
                                                    update_info_player.Parameters.AddWithValue("@id", player_id);
                                                    update_info_player.ExecuteNonQuery();

                                                    full_name = "{" + room_id + "}{" + player_id + "}{" + player_name + "}";

                                                    SqlCommand playing_players_count_sql = new SqlCommand("SELECT COUNT(*) FROM games WHERE room_id=@id_room AND player_status=@p_status", vote_sql);
                                                    playing_players_count_sql.Parameters.AddWithValue("@id_room", room);
                                                    playing_players_count_sql.Parameters.AddWithValue("@p_status", "play");
                                                    Int32 playing_players_count = (Int32)playing_players_count_sql.ExecuteScalar();

                                                    SqlCommand players_count_sql = new SqlCommand("SELECT COUNT(*) FROM games WHERE room_id=@id_room", vote_sql);
                                                    players_count_sql.Parameters.AddWithValue("@id_room", room);
                                                    Int32 players_count = (Int32)players_count_sql.ExecuteScalar();

                                                    SqlCommand players_count_kick_sql = new SqlCommand("SELECT COUNT(*) FROM games WHERE room_id=@id_room AND player_status=@p_status", vote_sql);
                                                    players_count_kick_sql.Parameters.AddWithValue("@id_room", room);
                                                    players_count_kick_sql.Parameters.AddWithValue("@p_status", "kick");
                                                    Int32 players_count_kick = (Int32)players_count_kick_sql.ExecuteScalar();



                                                    if (playing_players_count == Math.Ceiling(players_count / 2.0))
                                                    {
                                                        SqlCommand update_status = new SqlCommand("UPDATE games SET status=@status_game WHERE room_id=@room", vote_sql);
                                                        update_status.Parameters.AddWithValue("@status_game", "end");
                                                        update_status.Parameters.AddWithValue("@room", room_id);
                                                        update_status.ExecuteNonQuery();

                                                        SqlCommand update_player_status = new SqlCommand("UPDATE games SET player_status=@status WHERE player_status=@status_play AND room_id=@room", vote_sql);
                                                        update_player_status.Parameters.AddWithValue("@status", "win");
                                                        update_player_status.Parameters.AddWithValue("@room", room_id);
                                                        update_player_status.Parameters.AddWithValue("@status_play", "play");
                                                        update_player_status.ExecuteNonQuery();

                                                        string send_end_game_ = "END__THE_GAME " + full_name;
                                                        broadcast(send_end_game_, room_id, "send");
                                                    }
                                                    else
                                                    {
                                                        string send_result_ = "VOTING___KICK " + full_name;

                                                        broadcast(send_result_, room_id, "send");
                                                        Thread.Sleep(300);

                                                        if (players_count_kick != round)
                                                        {
                                                            string one_more_kick = "ONE_MORE_KICK ";

                                                            broadcast(one_more_kick, room_id, "send");

                                                        }
                                                        else
                                                        {
                                                            Next_move(room, true);
                                                            /*
                                                            SqlCommand check_move = new SqlCommand("SELECT * FROM games WHERE person_id=@id AND room_id=@room", vote_sql);
                                                            check_move.Parameters.AddWithValue("@room", room);
                                                            check_move.Parameters.AddWithValue("@id", player_id);
                                                            readSql = check_move.ExecuteReader();

                                                            if (readSql.Read() == true)
                                                            {
                                                                string move = (string)readSql["move"];

                                                                if (move.IndexOf("yes") > -1) Next_move(room, true);
                                                                readSql.Close();
                                                            }
                                                            else
                                                            {
                                                                readSql.Close();
                                                            }
                                                            */
                                                        }

                                                    }


                                                }
                                                else
                                                {
                                                    int round = 0;
                                                    SqlCommand select_round = new SqlCommand("SELECT * FROM games WHERE room_id=@room ", vote_sql);
                                                    select_round.Parameters.AddWithValue("@room", room);
                                                    readSql = select_round.ExecuteReader();
                                                    if (readSql.Read() == true)
                                                    {
                                                        round = (int)readSql["round"];
                                                        readSql.Close();
                                                        round++;
                                                    }
                                                    else
                                                    {
                                                        readSql.Close();
                                                    }

                                                    SqlCommand round_set = new SqlCommand("UPDATE games SET round=@round_set WHERE room_id=@room  ", vote_sql);
                                                    round_set.Parameters.AddWithValue("@round_set", round);
                                                    round_set.Parameters.AddWithValue("@room", room);
                                                    round_set.ExecuteNonQuery();

                                                    string send_result_ = "VOTING_N_KICK ";
                                                    broadcast(send_result_, room_id, "send");
                                                }

                                            }
                                            else
                                            {
                                                readSql.Close();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        readSql.Close();
                                    }
                                }
                                else
                                {
                                    SqlCommand count_player_sql = new SqlCommand("SELECT COUNT(*) FROM games WHERE room_id=@id_room AND player_status=@status", vote_sql);
                                    count_player_sql.Parameters.AddWithValue("@id_room", room);
                                    count_player_sql.Parameters.AddWithValue("@status", "play");
                                    Int32 count_player = (Int32)count_player_sql.ExecuteScalar();

                                    SqlCommand count_action_sql = new SqlCommand("SELECT COUNT(*) FROM games WHERE room_id=@id_room AND player_status=@status AND action=@action_p", vote_sql);
                                    count_action_sql.Parameters.AddWithValue("@id_room", room);
                                    count_action_sql.Parameters.AddWithValue("@status", "play");
                                    count_action_sql.Parameters.AddWithValue("@action_p", 2);
                                    Int32 count_action = (Int32)count_action_sql.ExecuteScalar();


                                    SqlCommand check = new SqlCommand("SELECT * FROM games WHERE person_id=@id AND room_id=@room", vote_sql);
                                    check.Parameters.AddWithValue("@room", room);
                                    check.Parameters.AddWithValue("@id", id);
                                    readSql = check.ExecuteReader();

                                    if (readSql.Read() == true)
                                    {
                                        string room_id = (string)readSql["room_id"];
                                        string player_id = (string)readSql["person_id"];
                                        string player_name = (string)readSql["name"];
                                        int vote_kick_new = (int)readSql["vote_kick"];
                                        string full_name = "{" + room_id + "}{" + player_id + "}{" + player_name + "}";
                                        

                                        int voted_new = (int)readSql["voted"];
                                        readSql.Close();
                                        count_vote(room);
                                        if (count_action == count_player)
                                        {

                                            SqlCommand max_vote = new SqlCommand("SELECT MAX(vote_kick) FROM games WHERE room_id=@room", vote_sql);

                                            max_vote.Parameters.AddWithValue("@room", room);
                                            int max_vote_count = (int)max_vote.ExecuteScalar();

                                            SqlCommand vote_kick_count_sql = new SqlCommand("SELECT COUNT(*) FROM games WHERE room_id=@id_room AND vote_kick=@vote_kick_count", vote_sql);
                                            vote_kick_count_sql.Parameters.AddWithValue("@id_room", room);
                                            vote_kick_count_sql.Parameters.AddWithValue("@vote_kick_count", max_vote_count);

                                            Int32 vote_kick_count = (Int32)vote_kick_count_sql.ExecuteScalar();


                                            SqlCommand get_player_for_kick = new SqlCommand("SELECT * FROM games WHERE room_id=@room AND vote_kick=@vote_k", vote_sql);
                                            get_player_for_kick.Parameters.AddWithValue("@room", room);
                                            get_player_for_kick.Parameters.AddWithValue("@vote_k", max_vote_count);

                                            readSql = get_player_for_kick.ExecuteReader();
                                            if (readSql.Read() == true)
                                            {
                                                room_id = (string)readSql["room_id"];
                                                player_id = (string)readSql["person_id"];
                                                player_name = (string)readSql["name"];
                                                //VOTING___KICK->ID_ROOM = KICK =
                                                readSql.Close();
                                                SqlCommand update_info = new SqlCommand("UPDATE games SET vote_kick=@vote_k,voted=@vote WHERE room_id=@room", vote_sql);
                                                update_info.Parameters.AddWithValue("@vote_k", (int)0);
                                                update_info.Parameters.AddWithValue("@vote", (int)0);
                                                update_info.Parameters.AddWithValue("@room", room_id);
                                                update_info.ExecuteNonQuery();

                                                SqlCommand update_action_sql = new SqlCommand("UPDATE games SET action=@action_p WHERE room_id=@room", vote_sql);
                                                update_action_sql.Parameters.AddWithValue("@action_p", (int)0);
                                                update_action_sql.Parameters.AddWithValue("@room", room_id);
                                                update_action_sql.ExecuteNonQuery();

                                                SqlCommand update_voted_for = new SqlCommand("UPDATE games SET voted_for=@voted WHERE room_id=@room", vote_sql);
                                                update_voted_for.Parameters.AddWithValue("@voted", "empty_voted");
                                                update_voted_for.Parameters.AddWithValue("@room", room);
                                                update_voted_for.ExecuteNonQuery();


                                                if (vote_kick_count == 1)
                                                {
                                                    int round = 0;
                                                    SqlCommand select_round = new SqlCommand("SELECT * FROM games WHERE room_id=@room ", vote_sql);
                                                    select_round.Parameters.AddWithValue("@room", room);
                                                    readSql = select_round.ExecuteReader();
                                                    if (readSql.Read() == true)
                                                    {
                                                        round = (int)readSql["round"];
                                                        readSql.Close();
                                                        round++;
                                                    }
                                                    else
                                                    {
                                                        readSql.Close();
                                                    }

                                                    SqlCommand round_set = new SqlCommand("UPDATE games SET round=@round_set WHERE room_id=@room  ", vote_sql);
                                                    round_set.Parameters.AddWithValue("@round_set", round);
                                                    round_set.Parameters.AddWithValue("@room", room);
                                                    round_set.ExecuteNonQuery();

                                                    SqlCommand update_info_player = new SqlCommand("UPDATE games SET player_status=@status WHERE person_id=@id AND room_id=@room", vote_sql);
                                                    update_info_player.Parameters.AddWithValue("@status", "kick");
                                                    update_info_player.Parameters.AddWithValue("@room", room_id);
                                                    update_info_player.Parameters.AddWithValue("@id", player_id);
                                                    update_info_player.ExecuteNonQuery();

                                                    full_name = "{" + room_id + "}{" + player_id + "}{" + player_name + "}";

                                                    SqlCommand playing_players_count_sql = new SqlCommand("SELECT COUNT(*) FROM games WHERE room_id=@id_room AND player_status=@p_status", vote_sql);
                                                    playing_players_count_sql.Parameters.AddWithValue("@id_room", room);
                                                    playing_players_count_sql.Parameters.AddWithValue("@p_status", "play");
                                                    Int32 playing_players_count = (Int32)playing_players_count_sql.ExecuteScalar();

                                                    SqlCommand players_count_sql = new SqlCommand("SELECT COUNT(*) FROM games WHERE room_id=@id_room", vote_sql);
                                                    players_count_sql.Parameters.AddWithValue("@id_room", room);
                                                    Int32 players_count = (Int32)players_count_sql.ExecuteScalar();

                                                    SqlCommand players_count_kick_sql = new SqlCommand("SELECT COUNT(*) FROM games WHERE room_id=@id_room AND player_status=@p_status", vote_sql);
                                                    players_count_kick_sql.Parameters.AddWithValue("@id_room", room);
                                                    players_count_kick_sql.Parameters.AddWithValue("@p_status", "kick");
                                                    Int32 players_count_kick = (Int32)players_count_kick_sql.ExecuteScalar();



                                                    if (playing_players_count == Math.Ceiling(players_count / 2.0))
                                                    {
                                                        SqlCommand update_status = new SqlCommand("UPDATE games SET status=@status_game WHERE room_id=@room", vote_sql);
                                                        update_status.Parameters.AddWithValue("@status_game", "end");
                                                        update_status.Parameters.AddWithValue("@room", room_id);
                                                        update_status.ExecuteNonQuery();

                                                        SqlCommand update_player_status = new SqlCommand("UPDATE games SET player_status=@status WHERE player_status=@status_play AND room_id=@room", vote_sql);
                                                        update_player_status.Parameters.AddWithValue("@status", "win");
                                                        update_player_status.Parameters.AddWithValue("@room", room_id);
                                                        update_player_status.Parameters.AddWithValue("@status_play", "play");
                                                        update_player_status.ExecuteNonQuery();

                                                        string send_end_game_ = "END__THE_GAME " + full_name;
                                                        broadcast(send_end_game_, room_id, "send");
                                                    }
                                                    else
                                                    {
                                                        string send_result_ = "VOTING___KICK " + full_name;

                                                        broadcast(send_result_, room_id, "send");
                                                        Thread.Sleep(300);

                                                        if (players_count_kick != round)
                                                        {
                                                            string one_more_kick = "ONE_MORE_KICK ";

                                                            broadcast(one_more_kick, room_id, "send");

                                                        }
                                                        else
                                                        {
                                                            Next_move(room, true);
                                                            /*
                                                            SqlCommand check_move = new SqlCommand("SELECT * FROM games WHERE person_id=@id AND room_id=@room", vote_sql);
                                                            check_move.Parameters.AddWithValue("@room", room);
                                                            check_move.Parameters.AddWithValue("@id", player_id);
                                                            readSql = check_move.ExecuteReader();

                                                            if (readSql.Read() == true)
                                                            {
                                                                string move = (string)readSql["move"];

                                                                if (move.IndexOf("yes") > -1) Next_move(room, true);
                                                                readSql.Close();
                                                            }
                                                            else
                                                            {
                                                                readSql.Close();
                                                            }*/

                                                        }

                                                    }


                                                }
                                                else
                                                {
                                                    int round = 0;
                                                    SqlCommand select_round = new SqlCommand("SELECT * FROM games WHERE room_id=@room ", vote_sql);
                                                    select_round.Parameters.AddWithValue("@room", room);
                                                    readSql = select_round.ExecuteReader();
                                                    if (readSql.Read() == true)
                                                    {
                                                        round = (int)readSql["round"];
                                                        readSql.Close();
                                                        round++;
                                                    }
                                                    else
                                                    {
                                                        readSql.Close();
                                                    }

                                                    SqlCommand round_set = new SqlCommand("UPDATE games SET round=@round_set WHERE room_id=@room  ", vote_sql);
                                                    round_set.Parameters.AddWithValue("@round_set", round);
                                                    round_set.Parameters.AddWithValue("@room", room);
                                                    round_set.ExecuteNonQuery();

                                                    string send_result_ = "VOTING_N_KICK ";
                                                    broadcast(send_result_, room_id, "send");
                                                }

                                            }
                                            else
                                            {
                                                readSql.Close();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        readSql.Close();
                                    }
                                }
                            }
                            else
                            {
                                readSql.Close();
                            }
                        }
                        else
                        {
                            readSql.Close();
                        }
                    }
                }
                else
                {
                    vote_sql.Close();

                }

                vote_sql.Close();
            }
        }
        public static void Voting_kick(string room, string name)
        {
            int first = 0;
            int next = 0;
            string[] Get_Info = new string[2];

            for (int i = 0; i < 2; i++)
            {
                first = name.IndexOf("{", next);
                next = name.IndexOf("}", first);
                Get_Info[i] = name.Substring(first + 1, next - first - 1);
                first = first + 1;
                next = next + 1;
            }
            string room_id = Get_Info[0];
            string player_id = Get_Info[1];
            
            SqlConnection vote_sql = null;
            string str_con = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=D:\\программ\\Bunker\\BunkerServer\\Bunker.mdf;Integrated Security=True;";
            vote_sql = new SqlConnection(str_con);
            vote_sql.Open();
            if (vote_sql.State == ConnectionState.Open)
            {
                    Console.WriteLine("Bunker BD6 Started ");

                SqlDataReader readSql;

                SqlCommand select_p = new SqlCommand("SELECT * FROM games WHERE person_id=@id AND room_id=@room", vote_sql);
                select_p.Parameters.AddWithValue("@room", room);
                select_p.Parameters.AddWithValue("@id", player_id);
               // Console.WriteLine(room + " " + player_id );

                readSql = select_p.ExecuteReader();
                if (readSql.Read() == true)
                {
                    string player_status = (string)readSql["player_status"];
                    string player_name = (string)readSql["name"];

                    readSql.Close();
                    //Console.WriteLine(room_id + " " + player_id + " " + player_name);
                    //Console.WriteLine(player_status + " ");

                    if (player_status == "play")
                    {

                        #region kick
                        int round = 0;
                        SqlCommand select_round = new SqlCommand("SELECT * FROM games WHERE room_id=@room ", vote_sql);
                        select_round.Parameters.AddWithValue("@room", room);
                        readSql = select_round.ExecuteReader();
                        if (readSql.Read() == true)
                        {
                            round = (int)readSql["round"];
                            readSql.Close();
                            round++;
                        }
                        else
                        {
                            readSql.Close();
                        }

                        SqlCommand round_set = new SqlCommand("UPDATE games SET round=@round_set WHERE room_id=@room  ", vote_sql);
                        round_set.Parameters.AddWithValue("@round_set", round);
                        round_set.Parameters.AddWithValue("@room", room);
                        round_set.ExecuteNonQuery();

                        SqlCommand update_info_player = new SqlCommand("UPDATE games SET player_status=@status WHERE person_id=@id AND room_id=@room", vote_sql);
                        update_info_player.Parameters.AddWithValue("@status", "kick");
                        update_info_player.Parameters.AddWithValue("@room", room_id);
                        update_info_player.Parameters.AddWithValue("@id", player_id);
                        update_info_player.ExecuteNonQuery();

                        string full_name = "{" + room_id + "}{" + player_id + "}{" + player_name + "}";

                        SqlCommand playing_players_count_sql = new SqlCommand("SELECT COUNT(*) FROM games WHERE room_id=@id_room AND player_status=@p_status", vote_sql);
                        playing_players_count_sql.Parameters.AddWithValue("@id_room", room);
                        playing_players_count_sql.Parameters.AddWithValue("@p_status", "play");
                        Int32 playing_players_count = (Int32)playing_players_count_sql.ExecuteScalar();

                        SqlCommand players_count_sql = new SqlCommand("SELECT COUNT(*) FROM games WHERE room_id=@id_room", vote_sql);
                        players_count_sql.Parameters.AddWithValue("@id_room", room);
                        Int32 players_count = (Int32)players_count_sql.ExecuteScalar();

                        SqlCommand players_count_kick_sql = new SqlCommand("SELECT COUNT(*) FROM games WHERE room_id=@id_room AND player_status=@p_status", vote_sql);
                        players_count_kick_sql.Parameters.AddWithValue("@id_room", room);
                        players_count_kick_sql.Parameters.AddWithValue("@p_status", "kick");
                        Int32 players_count_kick = (Int32)players_count_kick_sql.ExecuteScalar();



                        if (playing_players_count == Math.Ceiling(players_count / 2.0))
                        {
                            string send_result_ = "VOTING___KICK " + full_name;

                            broadcast(send_result_, room_id, "send");
                            Thread.Sleep(300);

                            SqlCommand update_status = new SqlCommand("UPDATE games SET status=@status_game WHERE room_id=@room", vote_sql);
                            update_status.Parameters.AddWithValue("@status_game", "end");
                            update_status.Parameters.AddWithValue("@room", room_id);
                            update_status.ExecuteNonQuery();

                            SqlCommand update_player_status = new SqlCommand("UPDATE games SET player_status=@status WHERE player_status=@status_play AND room_id=@room", vote_sql);
                            update_player_status.Parameters.AddWithValue("@status", "win");
                            update_player_status.Parameters.AddWithValue("@room", room_id);
                            update_player_status.Parameters.AddWithValue("@status_play", "play");
                            update_player_status.ExecuteNonQuery();

                            string send_end_game_ = "END__THE_GAME " + full_name;
                            broadcast(send_end_game_, room_id, "send");
                        }
                        else
                        {
                            string send_result_ = "VOTING___KICK " + full_name;

                            broadcast(send_result_, room_id, "send");
                            Thread.Sleep(300);

                            if (players_count_kick != round)
                            {
                                string one_more_kick = "ONE_MORE_KICK ";

                                broadcast(one_more_kick, room_id, "send");

                            }
                            else
                            {
                                Next_move(room, true);
                                /*
                                SqlCommand check_move = new SqlCommand("SELECT * FROM games WHERE person_id=@id AND room_id=@room", vote_sql);
                                check_move.Parameters.AddWithValue("@room", room);
                                check_move.Parameters.AddWithValue("@id", player_id);
                                readSql = check_move.ExecuteReader();

                                if (readSql.Read() == true)
                                {
                                    string move = (string)readSql["move"];

                                    if (move.IndexOf("yes") > -1) Next_move(room, true);
                                    readSql.Close();
                                }
                                else
                                {
                                    readSql.Close();
                                }*/

                            }




                        }
                        #endregion
                    }
                }
                else
                {
                    readSql.Close();
                }
            }
            vote_sql.Close();
        }
        public static void update_info(string room,string id,string person)
        {     

            int i = 0;
            string NameInfo = null;
            string allinfoFull = null;
            lock (locker)
            {
                foreach (DictionaryEntry Item in clientsList)
                {
                    Object obj = new Object();
                    obj = Item.Key;
                    string s = obj.ToString();
                    string sName = room.Substring(0, room.Length);

                    if (s.IndexOf(sName, 1) > -1)
                    {

                        allinfo[i] = iPerson.update_game_info(s, id, person);



                        NameInfo = "(" + s + allinfo[i] + ")";
                        i++;
                        allinfoFull = allinfoFull + NameInfo;
                    }
                }
            }
            allinfoFull = "UPDATE___INFO " + allinfoFull;
            broadcast(allinfoFull, room, "send");
        }

    }


    public class handleClinet
    {
        TcpClient clientSocket;
        string clNo;
        public Hashtable clientsList;
        public  Hashtable clientsListAutorisation;
        public static ICollection keys;
        string status;
        public void startClient(TcpClient inClientSocket, string clineNo, Hashtable cList)
        {
            status = "play";
            this.clientSocket = inClientSocket;
            this.clNo = clineNo;
            this.clientsList = cList;
            Thread ctThread = new Thread(doChat);
            ctThread.Start();
        }
        public void startClientAutorisation(TcpClient inClientSocket, string clineNo, Hashtable cList)
        {
            status = "auth";
            this.clientSocket = inClientSocket;
            this.clNo = clineNo;
            this.clientsListAutorisation = cList;
            Thread ctThread = new Thread(doChat);
            ctThread.Start();
        }

        public void doChat()
        {
            string dataFromClient = "";
            NetworkStream networkStream = null;
            try
            {
                while (true)
                {
                    dataFromClient = null;            
                    byte[] bytesFrom = new byte[4];
                    networkStream = clientSocket.GetStream();
                    networkStream.Read(bytesFrom, 0,4);
                    string size_get = Encoding.UTF8.GetString(bytesFrom);

                    int size_masssange;
                    bool success = Int32.TryParse(size_get, out size_masssange);
                    if (success)
                    {
                        string massange = "";

                        while (Encoding.UTF8.GetByteCount(massange) != size_masssange)
                        {
                            byte[] massange_byte = new byte[size_masssange];
                            networkStream.Read(massange_byte, 0, size_masssange);
                            massange = massange + Encoding.UTF8.GetString(massange_byte);
                            massange = massange.Trim('\0');
                            if (Encoding.UTF8.GetByteCount(massange) == size_masssange) break;

                        }
                        dataFromClient = massange;
                    }
                    int  size = dataFromClient.Length;
                    Console.WriteLine(dataFromClient);
                    if (dataFromClient.Length ==0)
                    {

                        Console.WriteLine("epmty" + dataFromClient + clNo);
                        networkStream.Close();
                        clientSocket.Close();
                        dataFromClient = " ";
                        lock (Program.locker)
                        {
                          if(status == "play")  Program.clientsList.Remove(clNo);
                          else  Program.clientsListAutorisation.Remove(clNo);
                        }
                        break;
                    }                   
                    else if (dataFromClient.IndexOf("NEXT_____MOVE", 0, 13) > -1)
                    {
                        

                        int first = 0;
                        int next = 0;
                        string[] Get_Info = new string[2];

                        for (int i = 0; i < Get_Info.Length; i++)
                        {
                            first = dataFromClient.IndexOf("{", next);
                            next = dataFromClient.IndexOf("}", first);
                            Get_Info[i] = dataFromClient.Substring(first + 1, next - first - 1);
                            first = first + 1;
                            next = next + 1;
                        }

                        Program.Next_move(Get_Info[0],false);

                    }
                    else if (dataFromClient.IndexOf("OPEN_CHARACTE", 0, 13) > -1)
                    {
                      

                        int first = 0;
                        int next = 0;
                        string[] Get_Info = new string[2];

                        for (int i = 0; i < Get_Info.Length; i++)
                        {
                            first = dataFromClient.IndexOf("{", next);
                            next = dataFromClient.IndexOf("}", first);
                            Get_Info[i] = dataFromClient.Substring(first + 1, next - first - 1);
                            first = first + 1;
                            next = next + 1;
                        }

                        Program.broadcast(dataFromClient,Get_Info[0], "send");

                    }
                    else if (dataFromClient.IndexOf("START____ROOM", 0, 13) > -1)
                    {                     

                        int first = 0;
                        int next = 0;
                        string[] Get_Info = new string[3];

                        for (int i = 0; i < 3; i++)
                        {
                            first = dataFromClient.IndexOf("{", next);
                            next = dataFromClient.IndexOf("}", first);
                            Get_Info[i] = dataFromClient.Substring(first + 1, next - first - 1);
                            first = first + 1;
                            next = next + 1;
                        }

                        Program.start_game( Get_Info[0], Get_Info[2]);
                        Console.WriteLine(clNo + " : " + dataFromClient);

                    }                     
                    else if (dataFromClient.IndexOf("DISCONN__ROOM", 0, 13) > -1)
                        {

                            dataFromClient = dataFromClient.Substring(13, size - 13);
                            Console.WriteLine("От клиента - " + clNo + " : " + dataFromClient);
                        int first = 0;
                        int next = 0;
                        string[] Get_Info = new string[3];

                        for (int i = 0; i < 3; i++)
                        {
                            first = clNo.IndexOf("{", next);
                            next = clNo.IndexOf("}", first);
                            Get_Info[i] = clNo.Substring(first + 1, next - first - 1);
                            first = first + 1;
                            next = next + 1;
                        }

                        lock (Program.locker)
                        {
                            Program.clientsList.Remove(clNo);
                        }
                        Program.broadcast(null,Get_Info[0], "Online");
                       
                        SqlConnection vote_sql = null;
                        string str_con = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=D:\\программ\\Bunker\\BunkerServer\\Bunker.mdf;Integrated Security=True;";
                        vote_sql = new SqlConnection(str_con);
                        vote_sql.Open();
                        if (vote_sql.State == ConnectionState.Open)
                        {
                            SqlDataReader readSql;

                            SqlCommand command = new SqlCommand("SELECT * FROM games WHERE room_id =@id_room AND status=@status_client AND  person_id=@id", vote_sql);
                            command.Parameters.AddWithValue("@id_room", Get_Info[0]);
                            command.Parameters.AddWithValue("@status_client", "play");
                            command.Parameters.AddWithValue("@id", Get_Info[1]);

                            readSql = command.ExecuteReader();
                            if (readSql.Read() == true)
                            {
                                readSql.Close();
                                Program.Voting_kick(Get_Info[0], clNo);
                            }
                            readSql.Close();
                            SqlCommand command_for_delete = new SqlCommand("SELECT * FROM games WHERE room_id =@id_room AND status=@status_client AND  person_id=@id", vote_sql);
                            command_for_delete.Parameters.AddWithValue("@id_room", Get_Info[0]);
                            command_for_delete.Parameters.AddWithValue("@status_client", "waiting");
                            command_for_delete.Parameters.AddWithValue("@id", Get_Info[1]);

                            readSql = command_for_delete.ExecuteReader();
                            if (readSql.Read() == true)
                            {
                                readSql.Close();
                                SqlCommand dalete_games = new SqlCommand("DELETE FROM games WHERE room_id =@id_room AND status=@status_client AND  person_id=@id", vote_sql);
                                dalete_games.Parameters.AddWithValue("@id_room", Get_Info[0]);
                                dalete_games.Parameters.AddWithValue("@status_client", "waiting");
                                dalete_games.Parameters.AddWithValue("@id", Get_Info[1]);
                                dalete_games.ExecuteNonQuery();

                                SqlCommand dalete_info_game = new SqlCommand("DELETE FROM info_game WHERE room =@id_room  AND id_person=@id", vote_sql);
                                dalete_info_game.Parameters.AddWithValue("@id_room", Get_Info[0]);
                                dalete_info_game.Parameters.AddWithValue("@id", Get_Info[1]);
                                dalete_info_game.ExecuteNonQuery();
                            }
                            readSql.Close();
                        }
                        if (networkStream.CanWrite == true) networkStream.Close();
                        if (clientSocket.Connected == true) clientSocket.Close();
                        break;
                    }
                    else if (dataFromClient.IndexOf("LOGIN_DISCONN", 0, 13)> -1)
                    {
                            dataFromClient = dataFromClient.Substring(13, size -13);
                            Console.WriteLine("От клиента - " + clNo + " : " + dataFromClient);
                            networkStream.Close();
                            lock (Program.locker)
                            {
                            Program.clientsListAutorisation.Remove(clNo);
                            }
                             clientSocket.Close();
                            break;
                    }
                    else if (dataFromClient.IndexOf("VOTING___KICK", 0, 13) > -1)
                    {
                   

                        int first = 0;
                        int next = 0;
                        string[] Get_Info = new string[4];

                        for (int i = 0; i < Get_Info.Length; i++)
                        {
                            first = dataFromClient.IndexOf("{", next);
                            next = dataFromClient.IndexOf("}", first);
                            Get_Info[i] = dataFromClient.Substring(first + 1, next - first - 1);
                            first = first + 1;
                            next = next + 1;
                        }


                        Program.Voting_kick(Get_Info[1],  Get_Info[0], clNo, Get_Info[3]);
                    }
                    else if (dataFromClient.IndexOf("CHAT_SEND_MSG", 0, 13) > -1)
                    {


                        int first = 0;
                        int next = 0;
                        string[] Get_Info = new string[4];

                        for (int i = 0; i < Get_Info.Length; i++)
                        {
                            first = dataFromClient.IndexOf("{", next);
                            next = dataFromClient.IndexOf("}", first);
                            Get_Info[i] = dataFromClient.Substring(first + 1, next - first - 1);
                            first = first + 1;
                            next = next + 1;
                        }
                        Program.broadcast("CHAT_GET__MSG " + Get_Info[2] + ": " + Get_Info[3], Get_Info[0],"send");

                    }
                    else if (dataFromClient.IndexOf("CARD____ACTIV", 0, 13) > -1)
                    {


                        int first = 0;
                        int next = 0;
                        string[] Get_Info = new string[4];

                        for (int i = 0; i < Get_Info.Length; i++)
                        {
                            first = dataFromClient.IndexOf("{", next);
                            next = dataFromClient.IndexOf("}", first);
                            Get_Info[i] = dataFromClient.Substring(first + 1, next - first - 1);
                            first = first + 1;
                            next = next + 1;
                        }
                        Program.update_info(Get_Info[0],Get_Info[3], clNo);
                    }                    
                    Array.Clear(bytesFrom, 0, bytesFrom.Length);
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                networkStream.Close();
                clientSocket.Close();
            }                 
        }    
    }
    public  class infoPerson
    {

        string AllInfoString;
        int Age { get; set; }
        string Sex { get; set; }
        string Job { get; set; }
        string Hobby { get; set; }
        string Health { get; set; }
        string Baggage { get; set; }
        string Phobia { get; set; }
        string Character { get; set; }
        string action_cards_one { get; set; }
        string action_cards_two{ get; set; }
        string work_experience { get; set; }
        string hobby_level { get; set; }
        string children { get; set; }
        public string AllInfo (string data)
        {
            int first = 0;
            int next = 0;
            string[] Get_Info = new string[3];

            for (int i = 0; i < 3; i++)
            {
                first = data.IndexOf("{", next);
                next = data.IndexOf("}", first);
                Get_Info[i] = data.Substring(first + 1, next - first - 1);
                first = first + 1;
                next = next + 1;
            }

            AgeRand();
            SexRand();
            hobby_level_Rand(Age);
            children_Rand();
            JobRand();
            HobbyRand();
            HealthRand();
            BaggageRand();
            PhobiaRand();
            CharacterRand();
            action_cards_Rand();
            work_experience_Rand(Age);
           
            // AllInfoString = " Возраст" + Age + " Пол" + Sex + " Работа" + Job + " Хобби" + Hobby + " Здоровье" + Health + " Багаж" + Baggage + " Фобия" + Phobia + " Характер" + Character ;
            AllInfoString = "{" + Age + "}" + "{" + Sex + " " + children + "}" + "{" + Job + " " + work_experience + "}" + "{" + Hobby + " " + hobby_level + "}" + "{" + Health + "}" + "{" + Baggage + "}" + "{" + Phobia + "}" + "{" + Character + "}" + "{" + action_cards_one + "}" + "{" + action_cards_two + "}";

            SqlConnection sql = null;
            string str_con = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=D:\\программ\\Bunker\\BunkerServer\\Bunker.mdf;Integrated Security=True;";
            sql = new SqlConnection(str_con);
            sql.Open();
            if (sql.State == ConnectionState.Open)
            {
                Console.WriteLine("Bunker infoPerson Started ");    
                SqlCommand update = new SqlCommand("UPDATE info_game SET Age=@Age_new,Sex=@Sex_new,Job=@Job_new,Hobby=@Hobby_new,Health=@Health_new,Baggage=@Baggage_new,Phobia=@Phobia_new,Character=@Character_new,action_cards_one=@action_cards_one_new,action_cards_two=@action_cards_two_new,work_experience=@work_experience_new,hobby_level=@hobby_level_new,children=@children_new WHERE room=@room_new AND Id_person=@Id_person_new AND person_name=@person_name_new", sql);

                update.Parameters.AddWithValue("@room_new", Get_Info[0]);
                update.Parameters.AddWithValue("@Id_person_new", Get_Info[1]);
                update.Parameters.AddWithValue("@person_name_new", Get_Info[2]);
                update.Parameters.AddWithValue("@Age_new", Age);
                update.Parameters.AddWithValue("@Sex_new", Sex);
                update.Parameters.AddWithValue("@Job_new", Job);
                update.Parameters.AddWithValue("@Hobby_new", Hobby);
                update.Parameters.AddWithValue("@Health_new", Health);
                update.Parameters.AddWithValue("@Baggage_new", Baggage);
                update.Parameters.AddWithValue("@Phobia_new", Phobia);
                update.Parameters.AddWithValue("@Character_new", Character);
                update.Parameters.AddWithValue("@action_cards_one_new", action_cards_one);
                update.Parameters.AddWithValue("@action_cards_two_new", action_cards_two);
                update.Parameters.AddWithValue("@work_experience_new", work_experience);
                update.Parameters.AddWithValue("@hobby_level_new", hobby_level);
                update.Parameters.AddWithValue("@children_new", children);
                update.ExecuteReader();
                sql.Close();
            }
                return AllInfoString;

        }

        public string update_game_info(string name, string id, string alone)
        {
            int first = 0;
            int next = 0;
            string[] Get_Info = new string[3];
            for (int i = 0; i < 3; i++)
            {
                first = name.IndexOf("{", next);
                next = name.IndexOf("}", first);
                Get_Info[i] = name.Substring(first + 1, next - first - 1);
                first = first + 1;
                next = next + 1;
            }
            int get_age =0;
            string new_info = null;
            SqlConnection sql = null;
            string str_con = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=D:\\программ\\Bunker\\BunkerServer\\Bunker.mdf;Integrated Security=True;";
            sql = new SqlConnection(str_con);
            sql.Open();
            if (sql.State == ConnectionState.Open)
            {
                SqlDataReader readSql;

                SqlCommand get = new SqlCommand("SELECT * FROM info_game WHERE room=@room_new AND Id_person=@Id_person_new AND person_name=@person_name_new", sql);
                get.Parameters.AddWithValue("@room_new", Get_Info[0]);
                get.Parameters.AddWithValue("@Id_person_new", Get_Info[1]);
                get.Parameters.AddWithValue("@person_name_new", Get_Info[2]);

                readSql = get.ExecuteReader();
                if (readSql.Read() == true)
                {
                     get_age =Int32.Parse( (string)readSql["Age"]);
                    readSql.Close();
                }
                if (id == "#100")
                {

                    string info = JobRand();
                    string info2 = work_experience_Rand(get_age);
                    SqlCommand update = new SqlCommand("UPDATE info_game SET Job=@Job_new,work_experience=@work_experience_new WHERE room=@room_new AND Id_person=@Id_person_new AND person_name=@person_name_new", sql);
                    update.Parameters.AddWithValue("@room_new", Get_Info[0]);
                    update.Parameters.AddWithValue("@Id_person_new", Get_Info[1]);
                    update.Parameters.AddWithValue("@person_name_new", Get_Info[2]);
                    update.Parameters.AddWithValue("@Job_new", info);
                    update.Parameters.AddWithValue("@work_experience_new", info2);

                    update.ExecuteReader();
                    sql.Close();
                    new_info = "{" + "Job" + "}{" + info +" " +info2+ "}";

                }
                else if (id == "#101")
                {
                    int info2 = AgeRand();
                    string info = SexRand();

                    SqlCommand update = new SqlCommand("UPDATE info_game SET Age=@Age_new, Sex=@Sex_new WHERE room=@room_new AND Id_person=@Id_person_new AND person_name=@person_name_new", sql);
                    update.Parameters.AddWithValue("@room_new", Get_Info[0]);
                    update.Parameters.AddWithValue("@Id_person_new", Get_Info[1]);
                    update.Parameters.AddWithValue("@person_name_new", Get_Info[2]);
                    update.Parameters.AddWithValue("@Age_new", info2);
                    update.Parameters.AddWithValue("@Sex_new", info);
                    update.ExecuteReader();
                    sql.Close();
                    new_info = "{" + "Age" + "}{" + info2 + "}{" + "Sex" + "}{" + info + "}"; 
                }

                else if(id == "#102") 
                {
                    string info = HobbyRand();
                    SqlCommand update = new SqlCommand("UPDATE info_game SET Hobby=@Hobby_new WHERE room=@room_new AND Id_person=@Id_person_new AND person_name=@person_name_new", sql);
                    update.Parameters.AddWithValue("@room_new", Get_Info[0]);
                    update.Parameters.AddWithValue("@Id_person_new", Get_Info[1]);
                    update.Parameters.AddWithValue("@person_name_new", Get_Info[2]);
                    update.Parameters.AddWithValue("@Hobby_new", info);
                    update.ExecuteReader();
                    sql.Close();
                    new_info = "{" + "Hobby" + "}{" + info + "}";
                   
                }
               
                else if(id == "#103") 
                {                  
                    string info = HealthRand();
                    SqlCommand update = new SqlCommand("UPDATE info_game SET Health=@Health_new WHERE room=@room_new AND Id_person=@Id_person_new AND person_name=@person_name_new", sql);
                    update.Parameters.AddWithValue("@room_new", Get_Info[0]);
                    update.Parameters.AddWithValue("@Id_person_new", Get_Info[1]);
                    update.Parameters.AddWithValue("@person_name_new", Get_Info[2]);
                    update.Parameters.AddWithValue("@Health_new", info);
                    update.ExecuteReader();
                    sql.Close();
                    new_info = "{" + "Health" + "}{" + info + "}";
                }
              
                else if (id == "#104")
                {
                    string info = BaggageRand();
                    SqlCommand update = new SqlCommand("UPDATE info_game SET Baggage=@Baggage_new WHERE room=@room_new AND Id_person=@Id_person_new AND person_name=@person_name_new", sql);
                    update.Parameters.AddWithValue("@room_new", Get_Info[0]);
                    update.Parameters.AddWithValue("@Id_person_new", Get_Info[1]);
                    update.Parameters.AddWithValue("@person_name_new", Get_Info[2]);
                    update.Parameters.AddWithValue("@Baggage_new", info);
                    update.ExecuteReader();
                    sql.Close();
                    new_info = "{" + "Baggage" + "}{" + info + "}"; 
                }
                
                else if (id == "#105")
                {
                    string info = PhobiaRand();
                    SqlCommand update = new SqlCommand("UPDATE info_game SET Phobia=@Phobia_new WHERE room=@room_new AND Id_person=@Id_person_new AND person_name=@person_name_new", sql);
                    update.Parameters.AddWithValue("@room_new", Get_Info[0]);
                    update.Parameters.AddWithValue("@Id_person_new", Get_Info[1]);
                    update.Parameters.AddWithValue("@person_name_new", Get_Info[2]);
                    update.Parameters.AddWithValue("@Phobia_new", info);
                    update.ExecuteReader();
                    sql.Close();
                    new_info = "{" + "Phobia" + "}{" + info + "}"; 
                }
             
                else if (id == "#106") 
                {
                    string info = CharacterRand();
                    SqlCommand update = new SqlCommand("UPDATE info_game SET Character=@Character_new WHERE room=@room_new AND Id_person=@Id_person_new AND person_name=@person_name_new", sql);
                    update.Parameters.AddWithValue("@room_new", Get_Info[0]);
                    update.Parameters.AddWithValue("@Id_person_new", Get_Info[1]);
                    update.Parameters.AddWithValue("@person_name_new", Get_Info[2]);
                    update.Parameters.AddWithValue("@Character_new", info);
                    update.ExecuteReader();
                    sql.Close();
                    new_info = "{" + "Character" + "}{" + info + "}"; 
                }
           
                else if (id == "#107" && name == alone)
                {

                    string info = JobRand();
                    string info2 = work_experience_Rand(get_age);
                    SqlCommand update = new SqlCommand("UPDATE info_game SET Job=@Job_new,work_experience=@work_experience_new WHERE room=@room_new AND Id_person=@Id_person_new AND person_name=@person_name_new", sql);
                    update.Parameters.AddWithValue("@room_new", Get_Info[0]);
                    update.Parameters.AddWithValue("@Id_person_new", Get_Info[1]);
                    update.Parameters.AddWithValue("@person_name_new", Get_Info[2]);
                    update.Parameters.AddWithValue("@Job_new", info);
                    update.Parameters.AddWithValue("@work_experience_new", info2);

                    update.ExecuteReader();
                    sql.Close();
                    new_info = "{" + "Job" + "}{" + info + " " + info2 + "}";

                }

                else if (id == "#108" && name == alone)
                {
                    int info2 = AgeRand();
                    string info = SexRand();

                    SqlCommand update = new SqlCommand("UPDATE info_game SET Age=@Age_new, Sex=@Sex_new WHERE room=@room_new AND Id_person=@Id_person_new AND person_name=@person_name_new", sql);
                    update.Parameters.AddWithValue("@room_new", Get_Info[0]);
                    update.Parameters.AddWithValue("@Id_person_new", Get_Info[1]);
                    update.Parameters.AddWithValue("@person_name_new", Get_Info[2]);
                    update.Parameters.AddWithValue("@Age_new", info2);
                    update.Parameters.AddWithValue("@Sex_new", info);
                    update.ExecuteReader();
                    sql.Close();
                    new_info = "{" + "Age" + "}{" + info2 + "}{" + "Sex" + "}{" + info + "}";
                }

                else if (id == "#109" && name == alone)
                {
                    string info = HobbyRand();
                    SqlCommand update = new SqlCommand("UPDATE info_game SET Hobby=@Hobby_new WHERE room=@room_new AND Id_person=@Id_person_new AND person_name=@person_name_new", sql);
                    update.Parameters.AddWithValue("@room_new", Get_Info[0]);
                    update.Parameters.AddWithValue("@Id_person_new", Get_Info[1]);
                    update.Parameters.AddWithValue("@person_name_new", Get_Info[2]);
                    update.Parameters.AddWithValue("@Hobby_new", info);
                    update.ExecuteReader();
                    sql.Close();
                    new_info = "{" + "Hobby" + "}{" + info + "}";

                }

                else if (id == "#110" && name == alone)
                {
                    string info = HealthRand();
                    SqlCommand update = new SqlCommand("UPDATE info_game SET Health=@Health_new WHERE room=@room_new AND Id_person=@Id_person_new AND person_name=@person_name_new", sql);
                    update.Parameters.AddWithValue("@room_new", Get_Info[0]);
                    update.Parameters.AddWithValue("@Id_person_new", Get_Info[1]);
                    update.Parameters.AddWithValue("@person_name_new", Get_Info[2]);
                    update.Parameters.AddWithValue("@Health_new", info);
                    update.ExecuteReader();
                    sql.Close();
                    new_info = "{" + "Health" + "}{" + info + "}";
                }

                else if (id == "#111" && name == alone)
                {
                    string info = BaggageRand();
                    SqlCommand update = new SqlCommand("UPDATE info_game SET Baggage=@Baggage_new WHERE room=@room_new AND Id_person=@Id_person_new AND person_name=@person_name_new", sql);
                    update.Parameters.AddWithValue("@room_new", Get_Info[0]);
                    update.Parameters.AddWithValue("@Id_person_new", Get_Info[1]);
                    update.Parameters.AddWithValue("@person_name_new", Get_Info[2]);
                    update.Parameters.AddWithValue("@Baggage_new", info);
                    update.ExecuteReader();
                    sql.Close();
                    new_info = "{" + "Baggage" + "}{" + info + "}";
                }


                else if (id == "#112" && name == alone)
                {
                    string info = PhobiaRand();
                    SqlCommand update = new SqlCommand("UPDATE info_game SET Phobia=@Phobia_new WHERE room=@room_new AND Id_person=@Id_person_new AND person_name=@person_name_new", sql);
                    update.Parameters.AddWithValue("@room_new", Get_Info[0]);
                    update.Parameters.AddWithValue("@Id_person_new", Get_Info[1]);
                    update.Parameters.AddWithValue("@person_name_new", Get_Info[2]);
                    update.Parameters.AddWithValue("@Phobia_new", info);
                    update.ExecuteReader();
                    sql.Close();
                    new_info = "{" + "Phobia" + "}{" + info + "}";
                }


                else if (id == "#113" && name == alone)
                {
                    string info = CharacterRand();
                    SqlCommand update = new SqlCommand("UPDATE info_game SET Character=@Character_new WHERE room=@room_new AND Id_person=@Id_person_new AND person_name=@person_name_new", sql);
                    update.Parameters.AddWithValue("@room_new", Get_Info[0]);
                    update.Parameters.AddWithValue("@Id_person_new", Get_Info[1]);
                    update.Parameters.AddWithValue("@person_name_new", Get_Info[2]);
                    update.Parameters.AddWithValue("@Character_new", info);
                    update.ExecuteReader();
                    sql.Close();
                    new_info = "{" + "Character"+"}{" + info + "}";
                }

                else new_info =  "{" +"none" + "}";
            }
            return new_info;
        }

        public int AgeRand()
        {
            Random rnd = new Random();
            Age = rnd.Next(18, 70);
            return Age;
        }
        public string children_Rand()
        {
            



            string[] children_box = { "Чайлдфри", "", "Бесплодие" };
            Random rnd = new Random();
            int rndNumber = rnd.Next(0, 100);
            if(rndNumber < 60) children = children_box[1];
            if (rndNumber >= 61 && rndNumber <= 80) children = children_box[0];
            if (rndNumber >= 81 && rndNumber <= 100) children = children_box[2];

            return children;
        }

        public string SexRand()
        {
            string[] SexBox = { "Муж", "Жен" };
            Random rnd = new Random();
            int rndNumber = rnd.Next(0, SexBox.Length);
            Sex = SexBox[rndNumber];

            return  Sex;
        }
        public string JobRand()
        {
            string[] JobBox = { "разнорабочий", "Безработный", "Пожарный","Спасатель", "Военный", "Психолог", "Иммунолог", "Терапевт", "Стоматолог", "Генетик" };
            Random rnd = new Random();
            int rndNumber = rnd.Next(0, JobBox.Length);
            Job = JobBox[rndNumber] ;

            return Job;
        }
        public string HobbyRand()
        {
            string[] HobbBox = { "Туриз","Охота","Рыбалка","Танцы", "Садоводство", "Чтение", "Кулинария" };
            Random rnd = new Random();
            int rndNumber = rnd.Next(0, HobbBox.Length);
            Hobby = HobbBox[rndNumber];

            return   Hobby;
        }
        public string HealthRand()
        {
            string[] HealthBox = { "Здоров", "Алкоголизм", "Аллергия", "астма", "ВИЧ", "СПИД", "Наркомания", "Деменция " };
            Random rnd = new Random();
            int rndNumber = rnd.Next(0, 100);
            if (rndNumber <= 50) Health = HealthBox[0];
            if (rndNumber >= 51 && rndNumber <= 60) Health = HealthBox[1];
            if (rndNumber >= 61 && rndNumber <= 80) Health = HealthBox[2];
            if (rndNumber >= 81 && rndNumber <= 88) Health = HealthBox[3];
            if (rndNumber >= 89 && rndNumber <= 91) Health = HealthBox[4];
            if (rndNumber >= 92 && rndNumber <= 93) Health = HealthBox[5];
            if (rndNumber >= 94 && rndNumber <= 98) Health = HealthBox[6];
            if (rndNumber >= 99 && rndNumber <= 100) Health = HealthBox[7];

            return Health;
        }
        public string BaggageRand()
        {
            string[] BaggageBox = { "Колонка", "Алкоголь", "Еда", "Наркотики", "Вода", "Палатка" };
            Random rnd = new Random();
            int rndNumber = rnd.Next(0, BaggageBox.Length);
            Baggage = BaggageBox[rndNumber];

            return Baggage;
        }
        public string PhobiaRand()
        {
 
            string[] PhobiaBox = {"Нет", "нозофобия", "акрофобия", "клаустрофобия", "социофобия", "арахнофобия", "танатофобия" };
            Random rnd = new Random();
            int rndNumber = rnd.Next(0, 100);
            if (rndNumber <= 51) Phobia = PhobiaBox[0];
            if (rndNumber >= 52 && rndNumber <= 60) Phobia = PhobiaBox[1];
            if (rndNumber >= 61 && rndNumber <= 69) Phobia = PhobiaBox[2];
            if (rndNumber >= 70 && rndNumber <= 78) Phobia = PhobiaBox[3];
            if (rndNumber >= 79 && rndNumber <= 87) Phobia = PhobiaBox[4];
            if (rndNumber >= 88 && rndNumber <= 95) Phobia = PhobiaBox[5];
            if (rndNumber >= 96 && rndNumber <= 100) Phobia = PhobiaBox[6];
          
            return  Phobia;
        }

        public string CharacterRand()
        {
            string[] CharacterBox = { "Аккуратность", "активность", "альтруизм", "артистичность", "бескорыстие", "вежливость" };
            Random rnd = new Random();
            int rndNumber = rnd.Next(0, CharacterBox.Length);
            Character = CharacterBox[rndNumber];

            return  Character;
        }

        public void action_cards_Rand()
        {
            string[] action_cards_Box = { "#100", "#101", "#102", "#103", "#104", "#105", "#106", "#107", "#108", "#109", "#110", "#111", "#112", "#113", "#200", "#201", "#202" };
            Random rnd = new Random();
            int rndNumber = rnd.Next(0, action_cards_Box.Length);
            action_cards_one = action_cards_Box[rndNumber];
            rndNumber = rnd.Next(0, action_cards_Box.Length);
            action_cards_two = action_cards_Box[rndNumber];

        }

        public string work_experience_Rand(int age_check)
        {
            Random rnd = new Random();
            work_experience = rnd.Next(0, 52).ToString();

            if (age_check - int.Parse(work_experience) < 18)
            {
                work_experience_Rand(age_check);
            }
           
            return work_experience;

        }
        public string hobby_level_Rand(int age_check)
        {
            Random rnd = new Random();
            hobby_level = rnd.Next(0, 52).ToString();

            if (age_check - int.Parse(hobby_level) < 18)
            {
                hobby_level_Rand(age_check);
            }
            return hobby_level;
        }

    }

   

}
