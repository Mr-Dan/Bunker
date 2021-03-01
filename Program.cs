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
     *  REGIST_CLIENT -> LOGIN = PASSWORD=
     *  LOGIN_DISCONN -> LOGIN = PASSWORD=
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

                        byte[] bytessize = new byte[ReceiveBufferSize];
                        dataFromClient = null;

                        networkStream = clientSocket.GetStream();
                       /* networkStream.Read(bytessize, 0, 8192);
                        dataFromClientsize = Encoding.UTF8.GetString(bytessize);


                        int sizebyte;

                        bool isNum = int.TryParse(dataFromClientsize, out sizebyte);

                        if (isNum)
                        {*/
                            byte[] bytesFrom = new byte[ReceiveBufferSize];
                            networkStream.Read(bytesFrom, 0, ReceiveBufferSize);
                            dataFromClient = Encoding.UTF8.GetString(bytesFrom);
                            keys = clientsList.Keys;
                            keysAutorisation = clientsListAutorisation.Keys;

                            dataFromClient= dataFromClient.Trim('\0');
                            Console.WriteLine(dataFromClient);

                            int sizefromclient = dataFromClient.Length;

                            if (dataFromClient.IndexOf("CONNECT__ROOM", 0, 13) > -1)
                            {
                                //CONNECT__ROOM->ID_ROOM = ID_CLIENT = ID_NAME = STATUS=
                                SqlDataReader readSql;


                                int id_room_pos = dataFromClient.IndexOf("ID_ROOM", 13);
                                int id_client_pos = dataFromClient.IndexOf("ID_CLIENT", 13);
                                int id_name_pos = dataFromClient.IndexOf("ID_NAME", 13);
                                int status_game_pos = dataFromClient.IndexOf("STATUS",13);


                                string id_room = dataFromClient.Substring(id_room_pos+8, id_client_pos-1-8- id_room_pos);
                                string id_client = dataFromClient.Substring(id_client_pos + 10, id_name_pos - 1 - 10 - id_client_pos);
                                string id_name = dataFromClient.Substring(id_name_pos + 8, status_game_pos - 8 - id_name_pos);
                                string status_game = dataFromClient.Substring(status_game_pos + 7, sizefromclient - 7 - status_game_pos);

                                Console.WriteLine(status_game);

                                if (status_game == "new_room_id") 
                                {

                                    SqlCommand command = new SqlCommand("SELECT * FROM games WHERE room_id =@id_room ", sqlcon);

                                    command.Parameters.AddWithValue("@id_room", id_room);


                                    readSql = command.ExecuteReader();

                                    if (readSql.Read() != true)
                                    {
                                        readSql.Close();
                                        SqlCommand insert = new SqlCommand("INSERT INTO games (room_id,person_id,name,status,permission) VALUES(@room_id,@person_id,@name,@status,@permission) ", sqlcon);

                                        insert.Parameters.AddWithValue("room_id", id_room);
                                        insert.Parameters.AddWithValue("person_id", id_client);
                                        insert.Parameters.AddWithValue("name", id_name);
                                        insert.Parameters.AddWithValue("status", "waiting");
                                        insert.Parameters.AddWithValue("permission", "admin");
                                        insert.ExecuteNonQuery();


                                    }
                                    else
                                    {
                                        readSql.Close();
                                        NameCheck = false;
                                    }


                                }
                                if (status_game == "connect_room_id") 
                                {
                                    SqlCommand command = new SqlCommand("SELECT * FROM games WHERE room_id =@id_room AND  status =@status_game", sqlcon);

                                    command.Parameters.AddWithValue("@id_room", id_room);
                                    command.Parameters.AddWithValue("@status_game", "waiting");


                                    readSql = command.ExecuteReader();

                                    if (readSql.Read() == true)
                                    {
                                        readSql.Close();
                                        SqlCommand insert = new SqlCommand("INSERT INTO games (room_id,person_id,name,status,permission) VALUES(@room_id,@person_id,@name,@status,@permission) ", sqlcon);

                                        insert.Parameters.AddWithValue("room_id", id_room);
                                        insert.Parameters.AddWithValue("person_id", id_client);
                                        insert.Parameters.AddWithValue("name", id_name);
                                        insert.Parameters.AddWithValue("status", "waiting");
                                        insert.Parameters.AddWithValue("permission", "player");
                                        insert.ExecuteNonQuery();

                                    }
                                    else
                                    {
                                    int size = Encoding.UTF8.GetByteCount("LOGIN_DISCONN");
                                    Byte[] Bytes_DISCONN = new byte[size];

                                    /* Bytes_DISCONN = Encoding.UTF8.GetBytes(size.ToString());
                                     networkStream.Write(Bytes_DISCONN, 0, Bytes_DISCONN.Length);*/

                                    Bytes_DISCONN = Encoding.UTF8.GetBytes("LOGIN_DISCONN");
                                    networkStream.Write(Bytes_DISCONN, 0, Bytes_DISCONN.Length);

                                    networkStream.Close();
                                    clientSocket.Close();

                                    readSql.Close();
                                        NameCheck = false;
                                    }
                                   

                                }



                                if (NameCheck == true)
                                {
                                    string room_player = id_room + " " + id_client + " " +id_name;

                                    clientsList.Add(room_player, clientSocket);
                                   // SendConnectRoom("CONNECT__ROOM " + room_player + " Joined ", room_player);
                                    
                                    Console.WriteLine(room_player + " Joined chat room ");

                                    client = new handleClinet();
                                    client.startClient(clientSocket, room_player, clientsList);


                                   
                                    clientsList = client.clientsList;
                                    Player_Online_Room(id_room);
                                }

                            }

                            else if (dataFromClient.IndexOf("LOGIN__CLIENT", 0, 13) > -1)
                            {


                                SqlDataReader readSql;

                                int login_pos = dataFromClient.IndexOf("LOGIN", 13);
                                int pass_pos = dataFromClient.IndexOf("PASSWORD");
                                string login = null;
                                string password = null;

                                login = dataFromClient.Substring(login_pos + 6, pass_pos - login_pos - 1 - 6);
                                password = dataFromClient.Substring(pass_pos + 9, sizefromclient - 9 - pass_pos);

                                SqlCommand command = new SqlCommand("SELECT * FROM person WHERE login =@login AND  password =@password", sqlcon);

                                command.Parameters.AddWithValue("@login", login);
                                command.Parameters.AddWithValue("@password", password);


                                readSql = command.ExecuteReader();

                                if (readSql.Read() == true)
                                {

                                    string dataClient = (int)readSql["Id"] + "";
                                    string dataClientinfo = "LOGIN_CONNECT" + " " + "ID_CLIENT=" + (int)readSql["Id"] + " " + "ID_NAME=" + (string)readSql["name"];

                                    foreach (string s in keysAutorisation)
                                    {
                                                                            
                                        if (s.IndexOf(" ") > -1)
                                        {
                                            string y = s.Substring(0, s.IndexOf(" "));
                                            string x = (int)readSql["Id"] + "";


                                            if (y == x)
                                            {
                                            networkStream.Close();
                                            clientSocket.Close();
                                            Console.WriteLine("error");
                                            NameCheck = false;
                                            readSql.Close();
                                            }
                                        }
                                    }

                                    if (NameCheck == true)
                                    {
                                        clientsListAutorisation.Add(dataClient, clientSocket);
                                        SendtoAutorisation(dataClientinfo, dataClient);
                                        Console.WriteLine(dataClient + " Joined chat room ");
                                        client = new handleClinet();
                                        client.startClientAutorisation(clientSocket, dataClient, clientsListAutorisation);

                                        clientsListAutorisation = client.clientsListAutorisation;
                                        keysAutorisation = clientsListAutorisation.Keys;

                                    readSql.Close();
                                    }
                                }
                                else
                                {
                                    int size = Encoding.UTF8.GetByteCount("LOGIN_DISCONN");
                                    Byte[] Bytes_DISCONN = new byte[size];

                                    /*Bytes_DISCONN = Encoding.UTF8.GetBytes(size.ToString());
                                    networkStream.Write(Bytes_DISCONN, 0, Bytes_DISCONN.Length);*/

                                    Bytes_DISCONN = Encoding.UTF8.GetBytes("LOGIN_DISCONN");
                                    networkStream.Write(Bytes_DISCONN, 0, Bytes_DISCONN.Length);

                                    networkStream.Close();
                                    clientSocket.Close();
                                    readSql.Close();
                                }

                           }

                            else if (dataFromClient.IndexOf("REGIST_CLIENT", 0, 13) > -1)
                            {
                                SqlDataReader readSql;

                                int login_pos = dataFromClient.IndexOf("LOGIN", 13);
                                int pass_pos = dataFromClient.IndexOf("PASSWORD");
                                int name_pos = dataFromClient.IndexOf("NAME");

                                string login = null;
                                string password = null;
                                string name = null;

                                login = dataFromClient.Substring(login_pos + 6, pass_pos - login_pos - 1 - 6);
                                password = dataFromClient.Substring(pass_pos + 9, name_pos - 9 - pass_pos);
                                name = dataFromClient.Substring(name_pos + 5, sizefromclient - 5 - name_pos);

                                SqlCommand command = new SqlCommand($"SELECT * FROM person WHERE login =@login ", sqlcon);
                                command.Parameters.AddWithValue("@login", login);
                                readSql = command.ExecuteReader();

                                if (readSql.Read() != true)
                                {


                                    readSql.Close();
                                    SqlDataReader readSqlreg;

                                    SqlCommand commandreg = new SqlCommand("INSERT INTO person (login,password,name) VALUES  (@login,@password,@name)", sqlcon);
                                    commandreg.Parameters.AddWithValue("@login",login);
                                    commandreg.Parameters.AddWithValue("@password", password);
                                    commandreg.Parameters.AddWithValue("@name", name);

                                    commandreg.ExecuteNonQuery();

                                    SqlCommand commandget = new SqlCommand("SELECT * FROM person WHERE login =@login AND  password =@password", sqlcon);
                                    commandget.Parameters.AddWithValue("@login", login);
                                    commandget.Parameters.AddWithValue("@password", password);
                                    readSqlreg = commandget.ExecuteReader();

                                    if (readSqlreg.Read() == true)
                                    {
                                        string dataClient = (int)readSqlreg["Id"] + "";
                                        string dataClientinfo = "LOGIN_CONNECT" + " " + "ID_CLIENT=" + (int)readSqlreg["Id"] + " " + "ID_NAME=" + (string)readSqlreg["name"];

                                        foreach (string s in keysAutorisation)
                                        {

                                            string y = s.Substring(0, s.IndexOf(" "));
                                            string x = (int)readSqlreg["Id"] + "";

                                            if (y == x)
                                            {
                                                networkStream.Close();
                                                clientSocket.Close();
                                                Console.WriteLine("error");
                                                NameCheck = false;
                                            }
                                        }

                                        if (NameCheck == true)
                                        {
                                            clientsListAutorisation.Add(dataClient, clientSocket);
                                            SendtoAutorisation(dataClientinfo, dataClient);
                                            Console.WriteLine(dataClient + " Joined chat room ");

                                            client = new handleClinet();
                                            client.startClientAutorisation(clientSocket, dataClient, clientsListAutorisation);

                                            clientsListAutorisation = client.clientsListAutorisation;
                                            keysAutorisation = clientsListAutorisation.Keys;

                                        readSqlreg.Close();
                                        }
                                    }
                                    else
                                    {
                                        int size = Encoding.UTF8.GetByteCount("LOGIN_DISCONN");
                                        Byte[] Bytes_DISCONN = new byte[size];

                                        /*Bytes_DISCONN = Encoding.UTF8.GetBytes(size.ToString());
                                        networkStream.Write(Bytes_DISCONN, 0, Bytes_DISCONN.Length);
                                        */

                                        Bytes_DISCONN = Encoding.UTF8.GetBytes("LOGIN_DISCONN");
                                        networkStream.Write(Bytes_DISCONN, 0, Bytes_DISCONN.Length);

                                        networkStream.Close();
                                        clientSocket.Close();
                                        readSqlreg.Close();

                                    }
                                }
                                else
                                {
                                    int size = Encoding.UTF8.GetByteCount("LOGIN_DISCONN");
                                    Byte[] Bytes_DISCONN = new byte[size];

                                   /* Bytes_DISCONN = Encoding.UTF8.GetBytes(size.ToString());
                                    networkStream.Write(Bytes_DISCONN, 0, Bytes_DISCONN.Length);*/

                                    Bytes_DISCONN = Encoding.UTF8.GetBytes("LOGIN_DISCONN");
                                    networkStream.Write(Bytes_DISCONN, 0, Bytes_DISCONN.Length);

                                    networkStream.Close();
                                    clientSocket.Close();
                                    readSql.Close();
                                }
                            }

                        // }

                        else
                        {
                            int size = Encoding.UTF8.GetByteCount("LOGIN_DISCONN");
                            Byte[] Bytes_DISCONN = new byte[size];

                           /* Bytes_DISCONN = Encoding.UTF8.GetBytes(size.ToString());
                            networkStream.Write(Bytes_DISCONN, 0, Bytes_DISCONN.Length);*/

                            Bytes_DISCONN = Encoding.UTF8.GetBytes("LOGIN_DISCONN");
                            networkStream.Write(Bytes_DISCONN, 0, Bytes_DISCONN.Length);

                            networkStream.Close();
                            clientSocket.Close();

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

        public static void broadcast(string msg, string uName, bool flag)
        {
            
            foreach (DictionaryEntry Item in clientsList)
            {

                Object obj = new Object();
                obj = Item.Key;
                string s = obj.ToString();
                string sName = uName.Substring(0, 1);

                if (s.IndexOf(sName) > -1)
                {
                    TcpClient broadcastSocket;
                    broadcastSocket = (TcpClient)Item.Value;
                    NetworkStream broadcastStream = broadcastSocket.GetStream();
                    Byte[] broadcastBytes = null;

                    if (flag == true)
                    {
                        broadcastBytes = Encoding.UTF8.GetBytes(uName + " says : " + msg);
                    }
                    else
                    {
                        broadcastBytes = Encoding.UTF8.GetBytes(msg);
                    }

                    broadcastStream.Write(broadcastBytes, 0, broadcastBytes.Length);
  
                    Array.Clear(broadcastBytes, 0, broadcastBytes.Length);

                }
            }       
        }

        public static void SendConnectRoom(string msg, string uName)
        {
                int size = Encoding.UTF8.GetByteCount(msg);

            foreach (DictionaryEntry Item in clientsList)
            {

                Object obj = new Object();
                obj = Item.Key;
                string s = obj.ToString();
                string sName = uName.Substring(0, uName.IndexOf(" "));

                if (s.IndexOf(sName) > -1 && s != uName)
                {
                    TcpClient broadcastSocket;
                    broadcastSocket = (TcpClient)Item.Value;
                    NetworkStream broadcastStream = broadcastSocket.GetStream();

                   /* byte[] broadcastBytessize = new byte[size];
                    broadcastBytessize = Encoding.UTF8.GetBytes(size.ToString());
                    broadcastStream.Write(broadcastBytessize, 0, broadcastBytessize.Length);*/

                    byte[] broadcastBytes = new byte[size];
                    broadcastBytes = Encoding.UTF8.GetBytes(msg);
                    broadcastStream.Write(broadcastBytes, 0, broadcastBytes.Length);

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

                Console.WriteLine(room);
                Console.WriteLine(permission);


                readSql = command.ExecuteReader();
                if (readSql.Read() == true)
                {
                    readSql.Close();

                    SqlCommand update = new SqlCommand("UPDATE games SET status=@play WHERE room_id =@room  ", sql_con_game);

                    update.Parameters.AddWithValue("@play", "play");
                    update.Parameters.AddWithValue("@room", room);
                    update.ExecuteReader();
                    PlayerInfo(room);
                }
                else
                {
                    readSql.Close();
                }
                sql_con_game.Close();

            }
         
        }

        public static void SendtoAutorisation(string msg, string uName)
        {

            foreach (DictionaryEntry Item in clientsListAutorisation)
            {

                Object obj = new Object();
                obj = Item.Key;
                string s = obj.ToString();

                int size = Encoding.UTF8.GetByteCount(msg);
               

                if (s == uName)
                {
                 
                    TcpClient broadcastSocket;
                    broadcastSocket = (TcpClient)Item.Value;
                    NetworkStream broadcastStream = broadcastSocket.GetStream();

                   /* byte[] broadcastBytessize = new byte[size];
                    broadcastBytessize = Encoding.UTF8.GetBytes(size.ToString());
                    broadcastStream.Write(broadcastBytessize, 0, broadcastBytessize.Length);*/

                    byte[] broadcastBytes = new byte[size];
                    broadcastBytes = Encoding.UTF8.GetBytes(msg);           
                    broadcastStream.Write(broadcastBytes, 0, broadcastBytes.Length);              

                }
            }
        }
       
        public static void Player_Online_Room(string room)
        {
            string online_names= null;
            foreach (DictionaryEntry Item in clientsList)
            {
                Object obj = new Object();
                obj = Item.Key;
                string s = obj.ToString();

                if (s.IndexOf(room, 0, room.Length) > -1) online_names = online_names +" {" + s + "} ";
              
            }

            online_names = "ONLINE___ROOM " + online_names;
            int size = Encoding.UTF8.GetByteCount(online_names);

            foreach (DictionaryEntry Item in clientsList)
            {

                Object obj = new Object();
                obj = Item.Key;
                string s = obj.ToString();              

                if (s.IndexOf(room, 0, room.Length) > -1)
                {
                    Console.WriteLine(s);
                    Console.WriteLine(online_names);

                    TcpClient broadcastSocket;
                    broadcastSocket = (TcpClient)Item.Value;
                    NetworkStream broadcastStream = broadcastSocket.GetStream();

                   /* byte[] broadcastBytessize = new byte[size];
                    broadcastBytessize = Encoding.UTF8.GetBytes(size.ToString());
                    broadcastStream.Write(broadcastBytessize, 0, broadcastBytessize.Length);*/

                    byte[] broadcastBytes = new byte[size];
                    broadcastBytes = Encoding.UTF8.GetBytes(online_names);
                    broadcastStream.Write(broadcastBytes, 0, broadcastBytes.Length);
                }
            }

        }

        public static void Send_player_info(string msg, string room)
        {
            int size = Encoding.UTF8.GetByteCount(msg);
           
            foreach (DictionaryEntry Item in clientsList)
            {

                Object obj = new Object();
                obj = Item.Key;
                string s = obj.ToString();
                string sName = room.Substring(0, room.Length);

                if (s.IndexOf(sName) > -1 )
                { Console.WriteLine(msg);
                    TcpClient broadcastSocket;
                    broadcastSocket = (TcpClient)Item.Value;
                    NetworkStream broadcastStream = broadcastSocket.GetStream();

                    /* byte[] broadcastBytessize = new byte[size];
                     broadcastBytessize = Encoding.UTF8.GetBytes(size.ToString());
                     broadcastStream.Write(broadcastBytessize, 0, broadcastBytessize.Length);*/

                    byte[] broadcastBytes = new byte[size];
                    broadcastBytes = Encoding.UTF8.GetBytes(msg);
                    broadcastStream.Write(broadcastBytes, 0, broadcastBytes.Length);

                }
            }

        }
        public static void Send_open_characteristic(string msg, string room)
        {
          
            int size = Encoding.UTF8.GetByteCount(msg);
            Console.WriteLine(msg);
            foreach (DictionaryEntry Item in clientsList)
            {

                Object obj = new Object();
                obj = Item.Key;
                string s = obj.ToString();
                string sName = room.Substring(0, room.Length);

                if (s.IndexOf(sName) > -1)
                {
                    
                    TcpClient broadcastSocket;
                    broadcastSocket = (TcpClient)Item.Value;
                    NetworkStream broadcastStream = broadcastSocket.GetStream();

                    /* byte[] broadcastBytessize = new byte[size];
                     broadcastBytessize = Encoding.UTF8.GetBytes(size.ToString());
                     broadcastStream.Write(broadcastBytessize, 0, broadcastBytessize.Length);*/

                    byte[] broadcastBytes = new byte[size];
                    broadcastBytes = Encoding.UTF8.GetBytes(msg);
                    broadcastStream.Write(broadcastBytes, 0, broadcastBytes.Length);

                }
            }

        }

        public static void PlayerInfo( string room)
        {
            Location();
            int i = 0;
            int players = 0;
            string allinfoFull = null;
            string NameInfo = null;

            foreach (DictionaryEntry Item in clientsList)
            {

                Object obj = new Object();
                obj = Item.Key;
                string s = obj.ToString();

                if (s.IndexOf(room, 0, room.Length) > -1)
                {
                    players++;
                    allinfo[i] = iPerson.AllInfo();

                    if (players == 1)
                    {
                        NameInfo = " ID_PLAYER=" + players + "(ID_NAME=" + s + "info=" + allinfo[i] + " move=" + "yes" + ")";

                        int id_player_pos = s.IndexOf(" ");
                        int id_player_pos_next = s.IndexOf(" ", id_player_pos + 1);

                        string id_player = s.Substring(id_player_pos + 1, id_player_pos_next - id_player_pos - 1);

                        Console.WriteLine("id_player " + id_player);
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
                            update.Parameters.AddWithValue("@id_player", id_player);
                            update.Parameters.AddWithValue("@p_status", "play");



                            update.ExecuteReader();
                            sql_move.Close();
                        }


                    }
                    else
                    {
                        NameInfo = " ID_PLAYER=" + players + "(ID_NAME=" + s + "info=" + allinfo[i] + " move=" + "no" + ")";

                        int id_player_pos = s.IndexOf(" ");
                        int id_player_pos_next = s.IndexOf(" ", id_player_pos+1);
                       

                        string id_player = s.Substring(id_player_pos+1, id_player_pos_next - id_player_pos-1);

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
                            update.Parameters.AddWithValue("@id_player", id_player);
                            update.Parameters.AddWithValue("@p_status", "play");

                            update.ExecuteReader();
                            sql_move.Close();
                        }

                    }

                    allinfoFull = allinfoFull + NameInfo;
                    i++;
                    
                }
            }
           allinfoFull = "ALL_INFO_GAME=" +"players="+ players + allinfoFull + "Location=" + LocationMaps;
            Send_player_info(allinfoFull, room);
            allinfoFull = null;
        } 

        public static void Location()
        {
            string Maps;
            string Room; 
            string PeopleLiveText = "Процент живых людей";

            string[] LocationMapsBox = { "Наводнение", "Извержение вулканов", "Массовое потепление", "Массовое похолодание", "Вторжение инопланетян" };
            string[] RoomBox = { "столовая", "местерская", "Гидропоника", "Склад оружия", "Мед Блок" };
            Random rnd = new Random();
            int LocationMapsNumber = rnd.Next(0, 5);
            int RoomNumber = rnd.Next(0, 5);
            int PeopleLive = rnd.Next(0, 100);


            Maps = LocationMapsBox[LocationMapsNumber];
            Room = RoomBox[RoomNumber];
            PeopleLiveText = PeopleLiveText + PeopleLive;

            LocationMaps = "{"+ Maps +"}" + "{" + Room + "}" +"{"+ PeopleLiveText + "}";


        }


        public static void Next_move(string room)
        {
            bool error_check = true;
            SqlConnection sql_con_game = null;
            string str_con = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=D:\\программ\\Bunker\\BunkerServer\\Bunker.mdf;Integrated Security=True;";
            sql_con_game = new SqlConnection(str_con);
            sql_con_game.Open();
            if (sql_con_game.State == ConnectionState.Open)
            {

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

                    SqlCommand update_last = new SqlCommand("UPDATE games SET move=@move_last WHERE game_number=@number", sql_con_game);

                    update_last.Parameters.AddWithValue("@move_last", "no");
                    update_last.Parameters.AddWithValue("@number", game_namber_act.ToString());
                    update_last.ExecuteNonQuery();

               

                    SqlCommand count_player_sql = new SqlCommand("SELECT COUNT(*) FROM games WHERE room_id=@id_room", sql_con_game);
                    count_player_sql.Parameters.AddWithValue("@id_room", room);

                    Int32 count = (Int32)count_player_sql.ExecuteScalar();

                    Console.WriteLine("ola_game_namber_act " + game_namber_act);

                    Console.WriteLine("count " +count);
                    game_namber_act++;
                    if (game_namber_act > count)
                    {
                        game_namber_act = 1;
                    }
                    Console.WriteLine("game_namber_act " + game_namber_act);


                  
                    for (int i= 0; i<= count;i++)
                    {
                        Console.WriteLine("game_namber_act " + game_namber_act);

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
                                string dataClientinfo = "NEXT_____MOVE" + " ID_ROOM=" + room + " ID_CLIENT=" + (string)readSql["person_id"] + " ID_NAME=" + (string)readSql["name"];
                                readSql.Close();
                                Send_Next_Move(dataClientinfo, room);
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
                    readSql.Close();
                }
                sql_con_game.Close();


            }


        }


        public static void Send_Next_Move(string msg, string room)
        {

            int size = Encoding.UTF8.GetByteCount(msg);
            Console.WriteLine(msg);
            foreach (DictionaryEntry Item in clientsList)
            {

                Object obj = new Object();
                obj = Item.Key;
                string s = obj.ToString();
                string sName = room.Substring(0, room.Length);

                if (s.IndexOf(sName) > -1)
                {

                    TcpClient broadcastSocket;
                    broadcastSocket = (TcpClient)Item.Value;
                    NetworkStream broadcastStream = broadcastSocket.GetStream();

                    /* byte[] broadcastBytessize = new byte[size];
                     broadcastBytessize = Encoding.UTF8.GetBytes(size.ToString());
                     broadcastStream.Write(broadcastBytessize, 0, broadcastBytessize.Length);*/

                    byte[] broadcastBytes = new byte[size];
                    broadcastBytes = Encoding.UTF8.GetBytes(msg);
                    broadcastStream.Write(broadcastBytes, 0, broadcastBytes.Length);

                }
            }

        }
        
        public static void Voting_kick(string msg, string room)
        {
            int room_pos = msg.IndexOf(room);
            int name_pos = msg.IndexOf(" ",room_pos+1);
            string id = msg.Substring(room_pos +1, name_pos- room_pos-1);

            SqlConnection vote_sql = null;
            string str_con = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=D:\\программ\\Bunker\\BunkerServer\\Bunker.mdf;Integrated Security=True;";
            vote_sql= new SqlConnection(str_con);
            vote_sql.Open();
            if(vote_sql.State == ConnectionState.Open)
            {
                Console.WriteLine("Bunker BD5 Started ");

                SqlDataReader readSql;



                SqlCommand select_p = new SqlCommand("SELECT * FROM games WHERE person_id=@id AND room_id=@room", vote_sql);
                select_p.Parameters.AddWithValue("@room", room);
                select_p.Parameters.AddWithValue("@person_id", id);

                readSql = select_p.ExecuteReader();
                if (readSql.Read() == true)
                {
                    string player_status = (string)readSql["player_status"];
                    int vote_kick = (int)readSql["vote_kick"];
                    int voted = (int)readSql["voted"];
                    readSql.Close();
                    if (player_status.IndexOf("play") >1)
                    {
                        vote_kick++;
                        voted++;
                        SqlCommand write_vote = new SqlCommand("UPDATE games SET vote_kick=@vote_k,voted=@vote WHERE person_id=@id AND room_id=@room", vote_sql);
                        write_vote.Parameters.AddWithValue("@vote_k", vote_kick);
                        write_vote.Parameters.AddWithValue("@vote", voted);
                        write_vote.Parameters.AddWithValue("@room", room);
                        write_vote.Parameters.AddWithValue("@person_id", id);
                        write_vote.ExecuteNonQuery();


                        SqlCommand count_player_sql = new SqlCommand("SELECT COUNT(*) FROM games WHERE room_id=@id_room AND player_status=@status", vote_sql);
                        count_player_sql.Parameters.AddWithValue("@id_room", room);
                        count_player_sql.Parameters.AddWithValue("@status", "play");

                        Int32 count_player = (Int32)count_player_sql.ExecuteScalar();

                        SqlCommand check = new SqlCommand("SELECT * FROM games WHERE person_id=@id AND room_id=@room", vote_sql);
                        check.Parameters.AddWithValue("@room", room);
                        check.Parameters.AddWithValue("@person_id", id);

                        readSql = check.ExecuteReader();
                        if(readSql.Read() == true)
                        {
                            int vote_kick_new = (int)readSql["vote_kick"];
                            int voted_new = (int)readSql["voted"];
                            readSql.Close();

                            if(voted_new == count_player)
                            {

                                SqlCommand max_vote = new SqlCommand("SELECT MAX(vote_kick) FROM games WHERE person_id=@id AND room_id=@room", vote_sql);
                                max_vote.Parameters.AddWithValue("@id",id);
                                max_vote.Parameters.AddWithValue("@room_id", room);
                                int max_vote_count = (int)max_vote.ExecuteScalar();


                                SqlCommand get_player_for_kick = new SqlCommand("SELECT * FROM games WHERE person_id=@id AND room_id=@room AND vote_kick=@vote_k", vote_sql);
                                get_player_for_kick.Parameters.AddWithValue("@id", id);
                                get_player_for_kick.Parameters.AddWithValue("@room_id", room);
                                get_player_for_kick.Parameters.AddWithValue("@vote_k", max_vote_count);

                                readSql = get_player_for_kick.ExecuteReader();
                                if(readSql.Read()== true)
                                {
                                    string room_id = (string)readSql["room_id"];
                                    string player_id = (string)readSql["person_id"];
                                    string player_name = (string)readSql["name"];
                                    //VOTING___KICK->ID_ROOM = KICK =
                                    readSql.Close();

                                    string full_name = room_id + " " + player_id + " " + player_name;

                                    string send_result_ = "VOTING___KICK " + "ID_ROOM=" + room_id + "KICK=" + full_name;



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

        }

    }


    public class handleClinet
    {
        TcpClient clientSocket;
        string clNo;
        public Hashtable clientsList;
        public  Hashtable clientsListAutorisation;

        public static ICollection keys;

        public void startClient(TcpClient inClientSocket, string clineNo, Hashtable cList)
        {
            this.clientSocket = inClientSocket;
            this.clNo = clineNo;
            this.clientsList = cList;
            Thread ctThread = new Thread(doChat);
            ctThread.Start();
        }
        public void startClientAutorisation(TcpClient inClientSocket, string clineNo, Hashtable cList)
        {
            this.clientSocket = inClientSocket;
            this.clNo = clineNo;
            this.clientsListAutorisation = cList;
            Thread ctThread = new Thread(doChat);
            ctThread.Start();
        }

        public void doChat()
        {
            byte[] bytessize = new byte[8192];

            string dataFromClient = null;
            int ReceiveBufferSize = 6000;
            NetworkStream networkStream = null;
            try
             {

                while (true)
                {
                    dataFromClient = null;

                  /*  networkStream = clientSocket.GetStream();
                    networkStream.Read(bytessize, 0, 8192);
                    dataFromClientsize = Encoding.UTF8.GetString(bytessize);

                    int sizebyte;
                    bool isNum = int.TryParse(dataFromClientsize, out sizebyte);

                    if (isNum)
                    {*/
                        byte[] bytesFrom = new byte[ReceiveBufferSize];

                        networkStream = clientSocket.GetStream();
                        networkStream.Read(bytesFrom, 0,ReceiveBufferSize);
                        dataFromClient = Encoding.UTF8.GetString(bytesFrom);
                        dataFromClient = dataFromClient.Trim('\0');
                        int size = dataFromClient.Length;

                   


                    //NEXT_____MOVE->ID_ROOM = ID_CLIENT =
                    if (dataFromClient.IndexOf("NEXT_____MOVE", 0, 13) > -1)
                    {
                        int id_room_pos = dataFromClient.IndexOf("ID_ROOM", 13);
                        int id_client_pos = dataFromClient.IndexOf("ID_CLIENT", 13);
                        string id_room = dataFromClient.Substring(id_room_pos + 8, id_client_pos - 1 - 8 - id_room_pos);
                        string id_client = dataFromClient.Substring(id_client_pos + 10, dataFromClient.Length - 1 - 10 - id_client_pos);

                        Program.Next_move(id_room);

                    }
                    //OPEN_CHARACTE->ID_ROOM= ID_CLIENT= CHARATER=
                    else if(dataFromClient.IndexOf("OPEN_CHARACTE",0,13) > -1)
                    {
                       int id_room_pos = dataFromClient.IndexOf("ID_ROOM", 13);
                       int id_client_pos = dataFromClient.IndexOf("ID_CLIENT", 13);
                       string id_room = dataFromClient.Substring(id_room_pos + 8, id_client_pos - 1 - 8 - id_room_pos);  
                        
                       Program.Send_open_characteristic(dataFromClient, id_room);

                    }
                    //START____ROOM->ID_ROOM = ID_CLIENT = PERMISSION =
                    else if (dataFromClient.IndexOf("START____ROOM",0,13) > -1)
                    {
                        int id_room_pos = dataFromClient.IndexOf("ID_ROOM", 13);
                        int id_client_pos = dataFromClient.IndexOf("ID_CLIENT", 13);
                        int permission_pos = dataFromClient.IndexOf("PERMISSION", 13);


                        string id_room = dataFromClient.Substring(id_room_pos + 8, id_client_pos - 1 - 8 - id_room_pos);
                        string id_client = dataFromClient.Substring(id_client_pos + 10, permission_pos - 1 - 10 - id_client_pos);
                        string permission = dataFromClient.Substring(permission_pos + 11, size - 11 - permission_pos);

                        Program.start_game(id_room, permission);
                        Console.WriteLine(clNo + " : " + dataFromClient);

                    }
                       
                        // DISCONN__ROOM - Удаление и отключение клиента из игры
                    else if (dataFromClient.IndexOf("DISCONN__ROOM",0,13) > -1)
                        {
                            dataFromClient = dataFromClient.Substring(13, size - 13);
                            Console.WriteLine("От клиента - " + clNo + " : " + dataFromClient);
                            networkStream.Close();
                            Program.clientsList.Remove(clNo);
                            this.clientSocket.Close();
                            string room = clNo.Substring(0, clNo.IndexOf(" ",0));
                            Program.Player_Online_Room(room);
                         
                            break;
                    }
                        // LOGIN_DISCONN - Удаление и отключение клиента из меню
                    else if ( dataFromClient.IndexOf("LOGIN_DISCONN", 0, 13 )> -1)
                    {
                            dataFromClient = dataFromClient.Substring(13, size -13);
                            Console.WriteLine("От клиента - " + clNo + " : " + dataFromClient);
                            networkStream.Close();
                            Program.clientsListAutorisation.Remove(clNo);
                            this.clientSocket.Close();
                            break;
                    }
                    //VOTING___KICK->ID_ROOM =  VOTE =
                    else if (dataFromClient.IndexOf("VOTING___KICK", 0, 13) > -1)
                    {
                        dataFromClient = dataFromClient.Substring(13, size - 13);

                        int id_room_pos = dataFromClient.IndexOf("ID_ROOM", 13);
                        int pos_vote = dataFromClient.IndexOf("VOTE");
                        string id_room = dataFromClient.Substring(id_room_pos + 8, pos_vote - 1 - 8 - id_room_pos);
                        string vote = dataFromClient.Substring(pos_vote+5, dataFromClient.Length - pos_vote-5);

                        Program.Voting_kick(vote, id_room);
                    }
                    Array.Clear(bytesFrom, 0, bytesFrom.Length);
                }
                //}
            }
             catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

                //clientsListAutorisation.Remove(clNo);
               // clientsList.Remove(clNo);

                networkStream.Close();
                clientSocket.Close();
            }                 
        }

       
    }

    public  class infoPerson
    {

        public string AllInfoString;
        public int Age;
        public string Sex;
        public string Job;
        public string Hobby;
        public string Health;
        public string Baggage;
        public string Phobia;
        public string Character;
        public string action_cards_one;
        public string action_cards_two;

        public string AllInfo ()
        {
            AgeRand();
            SexRand();
            JobRand();
            HobbyRand();
            HealthRand();
            BaggageRand();
            PhobiaRand();
            CharacterRand();
            action_cards_Rand();
            // AllInfoString = " Возраст" + Age + " Пол" + Sex + " Работа" + Job + " Хобби" + Hobby + " Здоровье" + Health + " Багаж" + Baggage + " Фобия" + Phobia + " Характер" + Character ;
            AllInfoString = "{" + Age + "}" + "{" + Sex + "}" + "{" + Job + "}" + "{" + Hobby + "}" + "{" + Health + "}" + "{" + Baggage + "}" + "{" + Phobia + "}" + "{" + Character + "}" + "{" + action_cards_one + "}" + "{" + action_cards_two + "}";

            return AllInfoString;

        }

        private  void AgeRand()
        {
            Random rnd = new Random();
            Age = rnd.Next(16, 70);

        }
        private void SexRand()
        {
            string[] SexBox = { "Мужской", "Женский" };
            Random rnd = new Random();
            int rndNumber = rnd.Next(0, 2);
            Sex = SexBox[rndNumber];
        }
        private  void JobRand()
        {
            string[] JobBox = { "Пожарный","Спасатель", "Военный", "Психолог", "Иммунолог", "Терапевт", "Стоматолог", "Генетик" };
            Random rnd = new Random();
            int rndNumber = rnd.Next(0, 8);
            Job = JobBox[rndNumber];
        }
        private  void HobbyRand()
        {
            string[] HobbBox = { "Туриз","Охота","Рыбалка","Танцы", "Садоводство", "Чтение", "Кулинария" };

            Random rnd = new Random();
            int rndNumber = rnd.Next(0, 7);
            Hobby = HobbBox[rndNumber];
        }
        private  void HealthRand()
        {
            string[] HealthBox = { "Алкоголизм", "Аллергия", "астма", "ВИЧ", "СПИД", "Наркомания", "Деменция " };
            Random rnd = new Random();
            int rndNumber = rnd.Next(0, 7);
            Health = HealthBox[rndNumber];
        }
        private  void BaggageRand()
        {
            string[] BaggageBox = { "Колонка", "Алкоголь", "Еда", "Наркотики", "Вода", "Палатка" };
            Random rnd = new Random();
            int rndNumber = rnd.Next(0, 6);
            Baggage = BaggageBox[rndNumber];
        }
        private  void PhobiaRand()
        {
 
            string[] PhobiaBox = { "нозофобия", "акрофобия", "клаустрофобия", "социофобия", "арахнофобия", "танатофобия" };
            Random rnd = new Random();
            int rndNumber = rnd.Next(0, 6);
            Phobia = PhobiaBox[rndNumber];
        }
  
        private  void CharacterRand()
        {
            string[] CharacterBox = { "Аккуратность", "активность", "альтруизм", "артистичность", "бескорыстие", "вежливость" };
            Random rnd = new Random();
            int rndNumber = rnd.Next(0, 6);
            Character = CharacterBox[rndNumber];
        }
      
        private void action_cards_Rand()
        {
            string[] CharacterBox = { "#100", "#101", "#102", "#103", "#104", "#105" };
            Random rnd = new Random();
            int rndNumber = rnd.Next(0, 6);
            action_cards_one = CharacterBox[rndNumber];
            rndNumber = rnd.Next(0, 6);
            action_cards_two = CharacterBox[rndNumber];
        }
    }
}
