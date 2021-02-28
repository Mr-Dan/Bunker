using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.Collections;
using System.IO;
using System.Timers;


    
namespace BunkerClient
{
    public partial class Form1 : Form 
    {
               
        TcpClient clientSocket = new TcpClient();
        NetworkStream serverStream = null;

        public string id_client;
        public string id_name;
        public string login_client;
        public string password_client;
        public string ip;
        public string port;
        public string status_game;
        public string permission;
        static string readData = null;
        public Form1()
        {
            InitializeComponent();
        }
        public void PassValue(string code, string id, string name,string login, string password, string ip_copy, string port_copy,string status,string permission_copy)
        {
            CodeRoom.Text = code;
            id_client = id;
            id_name = name;
            login_client = login;
            password_client = password;
            ip = ip_copy;
            port = port_copy;
            status_game = status;
            permission = permission_copy;
        }
     
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                readData = "Conected to Chat Server ...";
                msg();
                clientSocket.Connect(IPAddress.Parse(ip), Int32.Parse(port));

                serverStream = clientSocket.GetStream();
                //CONNECT__ROOM->ID_ROOM = ID_CLIENT = ID_NAME =

                string Message = "CONNECT__ROOM " + "ID_ROOM=" +CodeRoom.Text + " ID_CLIENT="+ id_client + "ID_NAME=" + id_name + " STATUS="+status_game;
               
                int Messagesize = Encoding.UTF8.GetByteCount(Message);

                /*byte[] outStreamsize = new byte[Messagesize];
                outStreamsize = Encoding.UTF8.GetBytes(Messagesize.ToString());
                serverStream.Write(outStreamsize, 0, outStreamsize.Length);*/

                byte[] outStream = new byte[Messagesize];
                outStream = Encoding.UTF8.GetBytes(Message);
                serverStream.Write(outStream, 0, outStream.Length);

                textBox1.Text = Messagesize.ToString();

                Thread ctThread = new Thread(getMessage);
                ctThread.Start();

                button2.Enabled = false;
              
                PersonComponents1OFF();
                PersonComponents2OFF();
                PersonComponents3OFF();
                PersonComponents4OFF();
                PersonComponents5OFF();
                PersonComponents6OFF();
                PersonComponents7OFF();
                PersonComponents8OFF();
                PersonComponents9OFF();
                PersonComponents10OFF();
                PersonComponents11OFF();
                PersonComponents12OFF();

                StartGames.BackColor = Color.Lime;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                if (clientSocket == null)
                    clientSocket.Close();

                if (clientSocket.Connected == false)  // Port is in use and connection is successful
                    MessageBox.Show("Сервер не работает ", "Ошибка", MessageBoxButtons.OK);
                //if (serverStream == null) serverStream.Close();
            }
        }
        private void msg()
        {

            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(msg));
                //textBox1.Text =  Environment.NewLine + readData;
            }
            else
            {

              
                if (readData.IndexOf("CONNECT__ROOM",0,13) > -1)
                {
                    StartGames.Enabled = true;
                    StartGames.BackColor = Color.Lime;
                    textBox1.Text = textBox1.Text + permission;
                    My_Name.Text = id_client + " " + id_name;

                    //player();
                }
                else if(readData.IndexOf("NEXT_____MOVE", 0, 13) > -1)
                {
                    if (listBox1.BackColor == Color.Green)
                    {
                        #region SET_vote_button
                        if (Int32.Parse(online_p.Text) == 1)
                        {
                            vote_button1.Visible = true;

                        }
                        else if (Int32.Parse(online_p.Text) == 2)
                        {
                            vote_button1.Visible = true;
                            vote_button2.Visible = true;

                        }
                        else if (Int32.Parse(online_p.Text) == 3)
                        {
                            vote_button1.Visible = true;
                            vote_button2.Visible = true;
                            vote_button3.Visible = true;

                        }
                        else if (Int32.Parse(online_p.Text) == 4)
                        {
                            vote_button1.Visible = true;
                            vote_button2.Visible = true;
                            vote_button3.Visible = true;
                            vote_button4.Visible = true;
                        }
                        else if (Int32.Parse(online_p.Text) == 5)
                        {
                            vote_button1.Visible = true;
                            vote_button2.Visible = true;
                            vote_button3.Visible = true;
                            vote_button4.Visible = true;
                            vote_button5.Visible = true;

                        }
                        else if (Int32.Parse(online_p.Text) == 6)
                        {
                            vote_button1.Visible = true;
                            vote_button2.Visible = true;
                            vote_button3.Visible = true;
                            vote_button4.Visible = true;
                            vote_button5.Visible = true;
                            vote_button6.Visible = true;

                        }
                        else if (Int32.Parse(online_p.Text) == 7)
                        {
                            vote_button1.Visible = true;
                            vote_button2.Visible = true;
                            vote_button3.Visible = true;
                            vote_button4.Visible = true;
                            vote_button5.Visible = true;
                            vote_button6.Visible = true;
                            vote_button7.Visible = true;

                        }
                        else if (Int32.Parse(online_p.Text) == 8)
                        {
                            vote_button1.Visible = true;
                            vote_button2.Visible = true;
                            vote_button3.Visible = true;
                            vote_button4.Visible = true;
                            vote_button5.Visible = true;
                            vote_button6.Visible = true;
                            vote_button7.Visible = true;
                            vote_button8.Visible = true;

                        }
                        else if (Int32.Parse(online_p.Text) == 9)
                        {
                            vote_button1.Visible = true;
                            vote_button2.Visible = true;
                            vote_button3.Visible = true;
                            vote_button4.Visible = true;
                            vote_button5.Visible = true;
                            vote_button6.Visible = true;
                            vote_button7.Visible = true;
                            vote_button8.Visible = true;
                            vote_button9.Visible = true;

                        }
                        else if (Int32.Parse(online_p.Text) == 10)
                        {
                            vote_button1.Visible = true;
                            vote_button2.Visible = true;
                            vote_button3.Visible = true;
                            vote_button4.Visible = true;
                            vote_button5.Visible = true;
                            vote_button6.Visible = true;
                            vote_button7.Visible = true;
                            vote_button8.Visible = true;
                            vote_button9.Visible = true;
                            vote_button10.Visible = true;


                        }
                        else if (Int32.Parse(online_p.Text) == 11)
                        {
                            vote_button1.Visible = true;
                            vote_button2.Visible = true;
                            vote_button3.Visible = true;
                            vote_button4.Visible = true;
                            vote_button5.Visible = true;
                            vote_button6.Visible = true;
                            vote_button7.Visible = true;
                            vote_button8.Visible = true;
                            vote_button9.Visible = true;
                            vote_button10.Visible = true;
                            vote_button11.Visible = true;

                        }
                        else if (Int32.Parse(online_p.Text) == 1)
                        {
                            vote_button1.Visible = true;
                            vote_button2.Visible = true;
                            vote_button3.Visible = true;
                            vote_button4.Visible = true;
                            vote_button5.Visible = true;
                            vote_button6.Visible = true;
                            vote_button7.Visible = true;
                            vote_button8.Visible = true;
                            vote_button9.Visible = true;
                            vote_button10.Visible = true;
                            vote_button11.Visible = true;
                            vote_button12.Visible = true;
                        }
                        #endregion
                    }
                    else
                    {
                        textBox1.Text = textBox1.Text;
                        string move = readData.Substring(13, readData.Length - 13);
                        next_move(move);
                    }
                    
                }

                else if (readData.IndexOf("ONLINE___ROOM",0,13) > -1)
                {

                    if (permission.IndexOf("admin") > -1)
                    {
                        StartGames.Visible = true;
                    }
                    textBox2.Text = readData;
                }
                else if (readData.IndexOf("ALL_INFO_GAME", 0, 13) > -1)
                {

                    Set_info_players(readData);
                    textBox1.Text = textBox1.Text + readData;
                }
                else if (readData.IndexOf("OPEN_CHARACTE", 0, 13) > -1)
                {
                    
                    textBox1.Text = textBox1.Text + readData;
                    string characteristic = readData.Substring(13,readData.Length - 13);
                    open_characteristic(characteristic);
                }


            }
        }
        private void getMessage()
       
        { int  ReceiveBufferSize = 6000;

            try
            {
                while (true)
                {
                    /*serverStreamConn = clientSocket.GetStream();
                    byte[] inStreamsize = new byte[8192];
                    serverStreamConn.Read(inStreamsize, 0, 8192);
                    string returnsize = Encoding.UTF8.GetString(inStreamsize);

                    int size;
                    bool isNum = int.TryParse(returnsize, out size);

                    if (isNum)
                    {*/

                        byte[] inStream = new byte[ReceiveBufferSize];
                        serverStream = clientSocket.GetStream();

                        serverStream.Read(inStream, 0, ReceiveBufferSize);
                        string returndata = Encoding.UTF8.GetString(inStream);
                        readData = "" + returndata;
                        readData = readData.Trim('\0');

                         msg();
                    //}


                }
            }
            catch(Exception ex)
            {
                //MessageBox.Show(ex.ToString(), "Ошибка", MessageBoxButtons.OK);
                clientSocket.Close();
                serverStream.Close();

            }
        }   
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {

            if (serverStream != null && clientSocket.Connected == true)
            {
                int Messagesize = Encoding.UTF8.GetByteCount("DISCONN__ROOM вышел из игры");

                /*byte[] outStreamsize = new byte[Messagesize];
                outStreamsize = Encoding.UTF8.GetBytes(Messagesize.ToString());
                serverStream.Write(outStreamsize, 0, outStreamsize.Length);*/

                byte[] outStream = new byte[Messagesize];
                outStream = Encoding.UTF8.GetBytes("DISCONN__ROOM вышел из игры");
                serverStream.Write(outStream, 0, outStream.Length);         
            }
                
            if (clientSocket.Connected != false) clientSocket.Close();
            if (serverStream != null) serverStream.Close();
            Form3 F3 = new Form3();
            F3.menu_auth(login_client,password_client,true);
            F3.Show();
        }
       
        private void Start_Game_Click(object sender, EventArgs e)
        {
            try
            {
                //START____ROOM->ID_ROOM = ID_CLIENT = PERMISSION =
                string massage = "START____ROOM" + " ID_ROOM=" + CodeRoom.Text + " ID_CLIENT="+id_client + " PERMISSION="+permission;
                int Messagesize = Encoding.UTF8.GetByteCount(massage);

               
                byte[] outStream = new byte[Messagesize];
                outStream = Encoding.UTF8.GetBytes(massage);
                serverStream.Write(outStream, 0, outStream.Length);

                StartGames.BackColor = Color.Red;
                StartGames.Enabled = false;
            }
            catch 
            {
                if (clientSocket.Connected == false)  // Port is in use and connection is successful
                    MessageBox.Show("Сервер не работает ", "Ошибка", MessageBoxButtons.OK);

                if (clientSocket == null)
                    clientSocket.Close();
                if (serverStream == null) serverStream.Close();
            }
         }

        private void Form1_Load(object sender, EventArgs e)
        {
                StartGames.Visible = false;
                Next_Move_button.Visible = false;

            vote_button1.Visible = false;
            vote_button2.Visible = false;
            vote_button3.Visible = false;
            vote_button4.Visible = false;
            vote_button5.Visible = false;
            vote_button6.Visible = false;
            vote_button7.Visible = false;
            vote_button8.Visible = false;
            vote_button9.Visible = false;
            vote_button10.Visible = false;
            vote_button11.Visible = false;
            vote_button12.Visible = false;

        }

        private void Set_info_players(string read_data)
        {
            /*
            ALL_INFO_GAME=players=2
            ID_PLAYER=1(ID_NAME=nmi 13 fox info={30}{Женский}{Терапевт}{Садоводство}{СПИД}{Вода}{акрофобия}{артистичность} move=yes)
            ID_PLAYER=2(ID_NAME=nmi 1 dan info={66}{Мужской}{Стоматолог}{Рыбалка}{Аллергия}{Вода}{арахнофобия}{активность} move=no)
            Location={Массовое похолодание}{Гидропоника}{Процент живых людей4}*/

            int pos_players = read_data.IndexOf("players",13); 
            int pos_ID_PLAYER = read_data.IndexOf("ID_PLAYER", 13);
            int pos_Location = read_data.IndexOf("Location", 13);
            string players = read_data.Substring(pos_players+8, pos_ID_PLAYER-8- pos_players-1);
            string Location = read_data.Substring(pos_Location+10,  read_data.Length - 10 - 1- pos_Location);

            online_p.Text =players;
            location.Text = Location;
            string [] all_characteristic  = new string[Int32.Parse(players)];

            for (int i =0; i< Int32.Parse(players);i++)
            {

                int pos_ID_PLAYER_next = read_data.IndexOf("ID_PLAYER", pos_ID_PLAYER+1);
             
                if (pos_ID_PLAYER_next > -1)
                {
                    all_characteristic [i] = read_data.Substring(pos_ID_PLAYER+10, pos_ID_PLAYER_next - pos_ID_PLAYER -10);

                }
                else
                {
                    all_characteristic [i] = read_data.Substring(pos_ID_PLAYER+10, pos_Location - pos_ID_PLAYER-10);
                }

                pos_ID_PLAYER = pos_ID_PLAYER_next;
            }

            for(int i=0; i < Int32.Parse(players); i++)
            {
                
                if (all_characteristic [i].IndexOf("1",0,1)>-1 && all_characteristic [i].IndexOf("1", 1, 2) < 0 && all_characteristic [i].IndexOf("0", 1, 2) < 0 && (all_characteristic [i].IndexOf(id_name)>-1 && all_characteristic [i].IndexOf(id_client) > -1))
                {
                    int pos_move = all_characteristic[i].IndexOf("move");
                    int pos_move_end = all_characteristic[i].IndexOf(")");
                    string move = all_characteristic[i].Substring(pos_move+5, pos_move_end- pos_move-5);

                    PersonComponents1OFF();
                    PersonComponents2OFF();
                    PersonComponents3OFF();
                    PersonComponents4OFF();
                    PersonComponents5OFF();
                    PersonComponents6OFF();
                    PersonComponents7OFF();
                    PersonComponents8OFF();
                    PersonComponents9OFF();
                    PersonComponents10OFF();
                    PersonComponents11OFF();
                    PersonComponents12OFF();

                    if (move.IndexOf("yes") > -1)
                    {
                        PersonComponents1ON();
                        Next_Move_button.Visible = true;

                    }
                   

                    string[]  individual_characteristic = new string[10];
                    textBox2.Text = all_characteristic [i]+ "|" + textBox2.Text;
                    int first = 0; 
                    int next = 0;
                    for (int j = 0; j < 10;j++)
                    {                    
                        first = all_characteristic [i].IndexOf("{", next);
                        next = all_characteristic [i].IndexOf("}", first);
                         individual_characteristic[j] = all_characteristic [i].Substring(first+1, next - first-1);
                        first = first + 1;
                        next = next + 1;
                    }
                    AgeText1.Text =  individual_characteristic[0];
                    SexText1.Text =  individual_characteristic[1];
                    JobText1.Text =  individual_characteristic[2];
                    HobbyText1.Text =  individual_characteristic[3];
                    HealthText1.Text =  individual_characteristic[4];
                    BaggageText1.Text =  individual_characteristic[5];
                    PhobiaText1.Text =  individual_characteristic[6];
                    CharacterText1.Text =  individual_characteristic[7];
                    card_1_label.Text = individual_characteristic[8];
                    card_2_label.Text = individual_characteristic[9];


                }
                if (all_characteristic [i].IndexOf("2", 0, 1) > -1 && (all_characteristic [i].IndexOf(id_name) > -1 && all_characteristic [i].IndexOf(id_client) > -1))
                {
                    int pos_move = all_characteristic[i].IndexOf("move");
                    int pos_move_end = all_characteristic[i].IndexOf(")");
                    string move = all_characteristic[i].Substring(pos_move + 5, pos_move_end - pos_move - 5);

                    PersonComponents1OFF();
                    PersonComponents2OFF();
                    PersonComponents3OFF();
                    PersonComponents4OFF();
                    PersonComponents5OFF();
                    PersonComponents6OFF();
                    PersonComponents7OFF();
                    PersonComponents8OFF();
                    PersonComponents9OFF();
                    PersonComponents10OFF();
                    PersonComponents11OFF();
                    PersonComponents12OFF();

                    if (move.IndexOf("yes") > -1)
                    {
                        PersonComponents2ON();
                        Next_Move_button.Visible = true;

                    }

                    string[]  individual_characteristic = new string[10];
                    textBox2.Text = all_characteristic [i] + "|" + textBox2.Text;
                    int first = 0;
                    int next = 0;
                    for (int j = 0; j < 10; j++)
                    {
                        first = all_characteristic [i].IndexOf("{", next);
                        next = all_characteristic [i].IndexOf("}", first);
                         individual_characteristic[j] = all_characteristic [i].Substring(first + 1, next - first - 1);
                        first = first + 1;
                        next = next + 1;
                    }
                    AgeText2.Text =  individual_characteristic[0];
                    SexText2.Text =  individual_characteristic[1];
                    JobText2.Text =  individual_characteristic[2];
                    HobbyText2.Text =  individual_characteristic[3];
                    HealthText2.Text =  individual_characteristic[4];
                    BaggageText2.Text =  individual_characteristic[5];
                    PhobiaText2.Text =  individual_characteristic[6];
                    CharacterText2.Text =  individual_characteristic[7];
                    card_1_label.Text = individual_characteristic[8];
                    card_2_label.Text = individual_characteristic[9];
                }
                if (all_characteristic [i].IndexOf("3", 0, 2) > -1 && (all_characteristic [i].IndexOf(id_name) > -1 && all_characteristic [i].IndexOf(id_client) > -1))
                {
                    int pos_move = all_characteristic[i].IndexOf("move");
                    int pos_move_end = all_characteristic[i].IndexOf(")");
                    string move = all_characteristic[i].Substring(pos_move + 5, pos_move_end - pos_move - 5);

                    PersonComponents1OFF();
                    PersonComponents2OFF();
                    PersonComponents3OFF();
                    PersonComponents4OFF();
                    PersonComponents5OFF();
                    PersonComponents6OFF();
                    PersonComponents7OFF();
                    PersonComponents8OFF();
                    PersonComponents9OFF();
                    PersonComponents10OFF();
                    PersonComponents11OFF();
                    PersonComponents12OFF();

                    if (move.IndexOf("yes") > -1)
                    {
                        PersonComponents3ON();
                        Next_Move_button.Visible = true;

                    }

                    string[]  individual_characteristic = new string[10];
                    textBox2.Text = all_characteristic [i] + "|" + textBox2.Text;
                    int first = 0;
                    int next = 0;
                    for (int j = 0; j < 10; j++)
                    {
                        first = all_characteristic [i].IndexOf("{", next);
                        next = all_characteristic [i].IndexOf("}", first);
                         individual_characteristic[j] = all_characteristic [i].Substring(first + 1, next - first - 1);
                        first = first + 1;
                        next = next + 1;
                    }
                    AgeText3.Text =  individual_characteristic[0];
                    SexText3.Text =  individual_characteristic[1];
                    JobText3.Text =  individual_characteristic[2];
                    HobbyText3.Text =  individual_characteristic[3];
                    HealthText3.Text =  individual_characteristic[4];
                    BaggageText3.Text =  individual_characteristic[5];
                    PhobiaText3.Text =  individual_characteristic[6];
                    CharacterText3.Text =  individual_characteristic[7];
                    card_1_label.Text = individual_characteristic[8];
                    card_2_label.Text = individual_characteristic[9];

                }
                if (all_characteristic [i].IndexOf("4", 0, 2) > -1 && (all_characteristic [i].IndexOf(id_name) > -1 && all_characteristic [i].IndexOf(id_client) > -1))
                {
                    int pos_move = all_characteristic[i].IndexOf("move");
                    int pos_move_end = all_characteristic[i].IndexOf(")");
                    string move = all_characteristic[i].Substring(pos_move + 5, pos_move_end - pos_move - 5);

                    PersonComponents1OFF();
                    PersonComponents2OFF();
                    PersonComponents3OFF();
                    PersonComponents4OFF();
                    PersonComponents5OFF();
                    PersonComponents6OFF();
                    PersonComponents7OFF();
                    PersonComponents8OFF();
                    PersonComponents9OFF();
                    PersonComponents10OFF();
                    PersonComponents11OFF();
                    PersonComponents12OFF();

                    if (move.IndexOf("yes") > -1)
                    {
                        PersonComponents4ON();
                        Next_Move_button.Visible = true;

                    }

                    string[]  individual_characteristic = new string[10];
                    textBox2.Text = all_characteristic [i] + "|" + textBox2.Text;
                    int first = 0;
                    int next = 0;
                    for (int j = 0; j < 10; j++)
                    {
                        first = all_characteristic [i].IndexOf("{", next);
                        next = all_characteristic [i].IndexOf("}", first);
                         individual_characteristic[j] = all_characteristic [i].Substring(first + 1, next - first - 1);
                        first = first + 1;
                        next = next + 1;
                    }
                    AgeText4.Text =  individual_characteristic[0];
                    SexText4.Text =  individual_characteristic[1];
                    JobText4.Text =  individual_characteristic[2];
                    HobbyText4.Text =  individual_characteristic[3];
                    HealthText4.Text =  individual_characteristic[4];
                    BaggageText4.Text =  individual_characteristic[5];
                    PhobiaText4.Text =  individual_characteristic[6];
                    CharacterText4.Text =  individual_characteristic[7];
                    card_1_label.Text = individual_characteristic[8];
                    card_2_label.Text = individual_characteristic[9];

                }
                if (all_characteristic [i].IndexOf("5", 0, 2) > -1 && (all_characteristic [i].IndexOf(id_name) > -1 && all_characteristic [i].IndexOf(id_client) > -1))
                {
                    int pos_move = all_characteristic[i].IndexOf("move");
                    int pos_move_end = all_characteristic[i].IndexOf(")");
                    string move = all_characteristic[i].Substring(pos_move + 5, pos_move_end - pos_move - 5);

                    PersonComponents1OFF();
                    PersonComponents2OFF();
                    PersonComponents3OFF();
                    PersonComponents4OFF();
                    PersonComponents5OFF();
                    PersonComponents6OFF();
                    PersonComponents7OFF();
                    PersonComponents8OFF();
                    PersonComponents9OFF();
                    PersonComponents10OFF();
                    PersonComponents11OFF();
                    PersonComponents12OFF();

                    if (move.IndexOf("yes") > -1)
                    {
                        PersonComponents5ON();
                        Next_Move_button.Visible = true;

                    }

                    string[]  individual_characteristic = new string[10];
                    textBox2.Text = all_characteristic [i] + "|" + textBox2.Text;
                    int first = 0;
                    int next = 0;
                    for (int j = 0; j < 10; j++)
                    {
                        first = all_characteristic [i].IndexOf("{", next);
                        next = all_characteristic [i].IndexOf("}", first);
                         individual_characteristic[j] = all_characteristic [i].Substring(first + 1, next - first - 1);
                        first = first + 1;
                        next = next + 1;
                    }
                    AgeText5.Text =  individual_characteristic[0];
                    SexText5.Text =  individual_characteristic[1];
                    JobText5.Text =  individual_characteristic[2];
                    HobbyText5.Text =  individual_characteristic[3];
                    HealthText5.Text =  individual_characteristic[4];
                    BaggageText5.Text =  individual_characteristic[5];
                    PhobiaText5.Text =  individual_characteristic[6];
                    CharacterText5.Text =  individual_characteristic[7];
                    card_1_label.Text = individual_characteristic[8];
                    card_2_label.Text = individual_characteristic[9];

                }
                if (all_characteristic [i].IndexOf("6", 0, 2) > -1 && (all_characteristic [i].IndexOf(id_name) > -1 && all_characteristic [i].IndexOf(id_client) > -1))
                {
                    int pos_move = all_characteristic[i].IndexOf("move");
                    int pos_move_end = all_characteristic[i].IndexOf(")");
                    string move = all_characteristic[i].Substring(pos_move + 5, pos_move_end - pos_move - 5);

                    PersonComponents1OFF();
                    PersonComponents2OFF();
                    PersonComponents3OFF();
                    PersonComponents4OFF();
                    PersonComponents5OFF();
                    PersonComponents6OFF();
                    PersonComponents7OFF();
                    PersonComponents8OFF();
                    PersonComponents9OFF();
                    PersonComponents10OFF();
                    PersonComponents11OFF();
                    PersonComponents12OFF();

                    if (move.IndexOf("yes") > -1)
                    {
                        PersonComponents6ON();
                        Next_Move_button.Visible = true;

                    }

                    string[]  individual_characteristic = new string[10];
                    textBox2.Text = all_characteristic [i] + "|" + textBox2.Text;
                    int first = 0;
                    int next = 0;
                    for (int j = 0; j < 10; j++)
                    {
                        first = all_characteristic [i].IndexOf("{", next);
                        next = all_characteristic [i].IndexOf("}", first);
                         individual_characteristic[j] = all_characteristic [i].Substring(first + 1, next - first - 1);
                        first = first + 1;
                        next = next + 1;
                    }
                    AgeText6.Text =  individual_characteristic[0];
                    SexText6.Text =  individual_characteristic[1];
                    JobText6.Text =  individual_characteristic[2];
                    HobbyText6.Text =  individual_characteristic[3];
                    HealthText6.Text =  individual_characteristic[4];
                    BaggageText6.Text =  individual_characteristic[5];
                    PhobiaText6.Text =  individual_characteristic[6];
                    CharacterText6.Text =  individual_characteristic[7];
                    card_1_label.Text = individual_characteristic[8];
                    card_2_label.Text = individual_characteristic[9];


                }
                if (all_characteristic [i].IndexOf("7", 0, 2) > -1 && (all_characteristic [i].IndexOf(id_name) > -1 && all_characteristic [i].IndexOf(id_client) > -1))
                {
                    int pos_move = all_characteristic[i].IndexOf("move");
                    int pos_move_end = all_characteristic[i].IndexOf(")");
                    string move = all_characteristic[i].Substring(pos_move + 5, pos_move_end - pos_move - 5);

                    PersonComponents1OFF();
                    PersonComponents2OFF();
                    PersonComponents3OFF();
                    PersonComponents4OFF();
                    PersonComponents5OFF();
                    PersonComponents6OFF();
                    PersonComponents7OFF();
                    PersonComponents8OFF();
                    PersonComponents9OFF();
                    PersonComponents10OFF();
                    PersonComponents11OFF();
                    PersonComponents12OFF();

                    if (move.IndexOf("yes") > -1)
                    {
                        PersonComponents7ON();
                        Next_Move_button.Visible = true;

                    }

                    string[]  individual_characteristic = new string[10];
                    textBox2.Text = all_characteristic [i] + "|" + textBox2.Text;
                    int first = 0;
                    int next = 0;
                    for (int j = 0; j < 10; j++)
                    {
                        first = all_characteristic [i].IndexOf("{", next);
                        next = all_characteristic [i].IndexOf("}", first);
                         individual_characteristic[j] = all_characteristic [i].Substring(first + 1, next - first - 1);
                        first = first + 1;
                        next = next + 1;
                    }
                    AgeText7.Text =  individual_characteristic[0];
                    SexText7.Text =  individual_characteristic[1];
                    JobText7.Text =  individual_characteristic[2];
                    HobbyText7.Text =  individual_characteristic[3];
                    HealthText7.Text =  individual_characteristic[4];
                    BaggageText7.Text =  individual_characteristic[5];
                    PhobiaText7.Text =  individual_characteristic[6];
                    CharacterText7.Text =  individual_characteristic[7];
                    card_1_label.Text = individual_characteristic[8];
                    card_2_label.Text = individual_characteristic[9];


                }
                if (all_characteristic [i].IndexOf("8", 0, 2) > -1 && (all_characteristic [i].IndexOf(id_name) > -1 && all_characteristic [i].IndexOf(id_client) > -1))
                {
                    int pos_move = all_characteristic[i].IndexOf("move");
                    int pos_move_end = all_characteristic[i].IndexOf(")");
                    string move = all_characteristic[i].Substring(pos_move + 5, pos_move_end - pos_move - 5);

                    PersonComponents1OFF();
                    PersonComponents2OFF();
                    PersonComponents3OFF();
                    PersonComponents4OFF();
                    PersonComponents5OFF();
                    PersonComponents6OFF();
                    PersonComponents7OFF();
                    PersonComponents8OFF();
                    PersonComponents9OFF();
                    PersonComponents10OFF();
                    PersonComponents11OFF();
                    PersonComponents12OFF();

                    if (move.IndexOf("yes") > -1)
                    {
                        PersonComponents8ON();
                        Next_Move_button.Visible = true;

                    }

                    string[]  individual_characteristic = new string[10];
                    textBox2.Text = all_characteristic [i] + "|" + textBox2.Text;
                    int first = 0;
                    int next = 0;
                    for (int j = 0; j < 10; j++)
                    {
                        first = all_characteristic [i].IndexOf("{", next);
                        next = all_characteristic [i].IndexOf("}", first);
                         individual_characteristic[j] = all_characteristic [i].Substring(first + 1, next - first - 1);
                        first = first + 1;
                        next = next + 1;
                    }
                    AgeText8.Text =  individual_characteristic[0];
                    SexText8.Text =  individual_characteristic[1];
                    JobText8.Text =  individual_characteristic[2];
                    HobbyText8.Text =  individual_characteristic[3];
                    HealthText8.Text =  individual_characteristic[4];
                    BaggageText8.Text =  individual_characteristic[5];
                    PhobiaText8.Text =  individual_characteristic[6];
                    CharacterText8.Text =  individual_characteristic[7];
                    card_1_label.Text = individual_characteristic[8];
                    card_2_label.Text = individual_characteristic[9];


                }
                if (all_characteristic [i].IndexOf("9", 0, 2) > -1 && (all_characteristic [i].IndexOf(id_name) > -1 && all_characteristic [i].IndexOf(id_client) > -1))
                {
                    int pos_move = all_characteristic[i].IndexOf("move");
                    int pos_move_end = all_characteristic[i].IndexOf(")");
                    string move = all_characteristic[i].Substring(pos_move + 5, pos_move_end - pos_move - 5);

                    PersonComponents1OFF();
                    PersonComponents2OFF();
                    PersonComponents3OFF();
                    PersonComponents4OFF();
                    PersonComponents5OFF();
                    PersonComponents6OFF();
                    PersonComponents7OFF();
                    PersonComponents8OFF();
                    PersonComponents9OFF();
                    PersonComponents10OFF();
                    PersonComponents11OFF();
                    PersonComponents12OFF();

                    if (move.IndexOf("yes") > -1)
                    {
                        PersonComponents9ON();
                        Next_Move_button.Visible = true;

                    }

                    string[]  individual_characteristic = new string[10];
                    textBox2.Text = all_characteristic [i] + "|" + textBox2.Text;
                    int first = 0;
                    int next = 0;
                    for (int j = 0; j < 10; j++)
                    {
                        first = all_characteristic [i].IndexOf("{", next);
                        next = all_characteristic [i].IndexOf("}", first);
                         individual_characteristic[j] = all_characteristic [i].Substring(first + 1, next - first - 1);
                        first = first + 1;
                        next = next + 1;
                    }
                    AgeText9.Text =  individual_characteristic[0];
                    SexText9.Text =  individual_characteristic[1];
                    JobText9.Text =  individual_characteristic[2];
                    HobbyText9.Text =  individual_characteristic[3];
                    HealthText9.Text =  individual_characteristic[4];
                    BaggageText9.Text =  individual_characteristic[5];
                    PhobiaText9.Text =  individual_characteristic[6];
                    CharacterText9.Text =  individual_characteristic[7];
                    card_1_label.Text = individual_characteristic[8];
                    card_2_label.Text = individual_characteristic[9];


                }
                if (all_characteristic [i].IndexOf("10", 0, 2) > -1 && (all_characteristic [i].IndexOf(id_name) > -1 && all_characteristic [i].IndexOf(id_client) > -1))
                {
                    int pos_move = all_characteristic[i].IndexOf("move");
                    int pos_move_end = all_characteristic[i].IndexOf(")");
                    string move = all_characteristic[i].Substring(pos_move + 5, pos_move_end - pos_move - 5);

                    PersonComponents1OFF();
                    PersonComponents2OFF();
                    PersonComponents3OFF();
                    PersonComponents4OFF();
                    PersonComponents5OFF();
                    PersonComponents6OFF();
                    PersonComponents7OFF();
                    PersonComponents8OFF();
                    PersonComponents9OFF();
                    PersonComponents10OFF();
                    PersonComponents11OFF();
                    PersonComponents12OFF();

                    if (move.IndexOf("yes") > -1)
                    {
                        PersonComponents10ON();
                        Next_Move_button.Visible = true;

                    }

                    string[]  individual_characteristic = new string[10];
                    textBox2.Text = all_characteristic [i] + "|" + textBox2.Text;
                    int first = 0;
                    int next = 0;
                    for (int j = 0; j < 10; j++)
                    {
                        first = all_characteristic [i].IndexOf("{", next);
                        next = all_characteristic [i].IndexOf("}", first);
                         individual_characteristic[j] = all_characteristic [i].Substring(first + 1, next - first - 1);
                        first = first + 1;
                        next = next + 1;
                    }
                    AgeText10.Text =  individual_characteristic[0];
                    SexText10.Text =  individual_characteristic[1];
                    JobText10.Text =  individual_characteristic[2];
                    HobbyText10.Text =  individual_characteristic[3];
                    HealthText10.Text =  individual_characteristic[4];
                    BaggageText10.Text =  individual_characteristic[5];
                    PhobiaText10.Text =  individual_characteristic[6];
                    CharacterText10.Text =  individual_characteristic[7];
                    card_1_label.Text = individual_characteristic[8];
                    card_2_label.Text = individual_characteristic[9];


                }
                if (all_characteristic [i].IndexOf("11", 0, 2) > -1 && (all_characteristic [i].IndexOf(id_name) > -1 && all_characteristic [i].IndexOf(id_client) > -1))
                {
                    int pos_move = all_characteristic[i].IndexOf("move");
                    int pos_move_end = all_characteristic[i].IndexOf(")");
                    string move = all_characteristic[i].Substring(pos_move + 5, pos_move_end - pos_move - 5);

                    PersonComponents1OFF();
                    PersonComponents2OFF();
                    PersonComponents3OFF();
                    PersonComponents4OFF();
                    PersonComponents5OFF();
                    PersonComponents6OFF();
                    PersonComponents7OFF();
                    PersonComponents8OFF();
                    PersonComponents9OFF();
                    PersonComponents10OFF();
                    PersonComponents11OFF();
                    PersonComponents12OFF();

                    if (move.IndexOf("yes") > -1)
                    {
                        PersonComponents11ON();
                        Next_Move_button.Visible = true;

                    }

                    string[]  individual_characteristic = new string[10];
                    textBox2.Text = all_characteristic [i] + "|" + textBox2.Text;
                    int first = 0;
                    int next = 0;
                    for (int j = 0; j < 10; j++)
                    {
                        first = all_characteristic [i].IndexOf("{", next);
                        next = all_characteristic [i].IndexOf("}", first);
                         individual_characteristic[j] = all_characteristic [i].Substring(first + 1, next - first - 1);
                        first = first + 1;
                        next = next + 1;
                    }
                    AgeText11.Text =  individual_characteristic[0];
                    SexText11.Text =  individual_characteristic[1];
                    JobText11.Text =  individual_characteristic[2];
                    HobbyText11.Text =  individual_characteristic[3];
                    HealthText11.Text =  individual_characteristic[4];
                    BaggageText11.Text =  individual_characteristic[5];
                    PhobiaText11.Text =  individual_characteristic[6];
                    CharacterText11.Text =  individual_characteristic[7];
                    card_1_label.Text = individual_characteristic[8];
                    card_2_label.Text = individual_characteristic[9];


                }
                if (all_characteristic [i].IndexOf("12", 0, 2) > -1 && (all_characteristic [i].IndexOf(id_name) > -1 && all_characteristic [i].IndexOf(id_client) > -1))
                {
                    int pos_move = all_characteristic[i].IndexOf("move");
                    int pos_move_end = all_characteristic[i].IndexOf(")");
                    string move = all_characteristic[i].Substring(pos_move + 5, pos_move_end - pos_move - 5);

                    PersonComponents1OFF();
                    PersonComponents2OFF();
                    PersonComponents3OFF();
                    PersonComponents4OFF();
                    PersonComponents5OFF();
                    PersonComponents6OFF();
                    PersonComponents7OFF();
                    PersonComponents8OFF();
                    PersonComponents9OFF();
                    PersonComponents10OFF();
                    PersonComponents11OFF();
                    PersonComponents12OFF();

                    if (move.IndexOf("yes") > -1)
                    {
                        PersonComponents12ON();
                        Next_Move_button.Visible = true;

                    }

                    string[]  individual_characteristic = new string[10];
                    textBox2.Text = all_characteristic [i] + "|" + textBox2.Text;
                    int first = 0;
                    int next = 0;
                    for (int j = 0; j < 10; j++)
                    {
                        first = all_characteristic [i].IndexOf("{", next);
                        next = all_characteristic [i].IndexOf("}", first);
                         individual_characteristic[j] = all_characteristic [i].Substring(first + 1, next - first - 1);
                        first = first + 1;
                        next = next + 1;
                    }
                    AgeText12.Text =  individual_characteristic[0];
                    SexText12.Text =  individual_characteristic[1];
                    JobText12.Text =  individual_characteristic[2];
                    HobbyText12.Text =  individual_characteristic[3];
                    HealthText12.Text =  individual_characteristic[4];
                    BaggageText12.Text =  individual_characteristic[5];
                    PhobiaText12.Text =  individual_characteristic[6];
                    CharacterText12.Text =  individual_characteristic[7];
                    card_1_label.Text = individual_characteristic[8];
                    card_2_label.Text = individual_characteristic[9];


                }

            }

            for (int i = 0; i < Int32.Parse(players); i++)
            {

                if (all_characteristic [i].IndexOf("1", 0, 1) > -1 && all_characteristic [i].IndexOf("1", 1, 2) < 0 && all_characteristic [i].IndexOf("0", 1, 2) < 0)
                {

                    if (all_characteristic [i].IndexOf("ID_NAME") >-1)
                    {
                        int name_pos = all_characteristic [i].IndexOf("ID_NAME");
                        int name_pos_end = all_characteristic [i].IndexOf("info");

                        player_identifier1.Text = all_characteristic[i].Substring(name_pos + 7 + 1, name_pos_end - name_pos - 7 - 1);
                        string name = all_characteristic [i].Substring(name_pos + 7 +1, name_pos_end - name_pos -7-1);
                        name = name.Replace(CodeRoom.Text, "");
                        name = name.Trim(' ');
                        name = name.Substring(name.IndexOf(" ")+1, name.Length - name.IndexOf(" ") -1);
                        name1.Text = name;

                    }                 
                }

                if (all_characteristic [i].IndexOf("2", 0, 1) > -1 )
                {

                    if (all_characteristic [i].IndexOf("ID_NAME") > -1)
                    {
                        int name_pos = all_characteristic [i].IndexOf("ID_NAME");
                        int name_pos_end = all_characteristic [i].IndexOf("info");

                        player_identifier2.Text = all_characteristic[i].Substring(name_pos + 7 + 1, name_pos_end - name_pos - 7 - 1);
                        string name = all_characteristic [i].Substring(name_pos + 7 + 1, name_pos_end - name_pos - 7 - 1);
                        name = name.Replace(CodeRoom.Text, "");
                        name = name.Trim(' ');
                        name = name.Substring(name.IndexOf(" ") + 1, name.Length - name.IndexOf(" ") - 1);
                        name2.Text = name;

                    }
                }
                if (all_characteristic [i].IndexOf("3", 0, 2) > -1)
                {

                    if (all_characteristic [i].IndexOf("ID_NAME") > -1)
                    {
                        int name_pos = all_characteristic [i].IndexOf("ID_NAME");
                        int name_pos_end = all_characteristic [i].IndexOf("info");

                        player_identifier3.Text = all_characteristic[i].Substring(name_pos + 7 + 1, name_pos_end - name_pos - 7 - 1);
                        string name = all_characteristic [i].Substring(name_pos + 7 + 1, name_pos_end - name_pos - 7 - 1);
                        name = name.Replace(CodeRoom.Text, "");
                        name = name.Trim(' ');
                        name = name.Substring(name.IndexOf(" ") + 1, name.Length - name.IndexOf(" ") - 1);
                        name3.Text = name;

                    }
                }

                if (all_characteristic [i].IndexOf("4", 0, 2) > -1)
                {

                    if (all_characteristic [i].IndexOf("ID_NAME") > -1)
                    {
                        int name_pos = all_characteristic [i].IndexOf("ID_NAME");
                        int name_pos_end = all_characteristic [i].IndexOf("info");

                        player_identifier4.Text = all_characteristic[i].Substring(name_pos + 7 + 1, name_pos_end - name_pos - 7 - 1);
                        string name = all_characteristic [i].Substring(name_pos + 7 + 1, name_pos_end - name_pos - 7 - 1);
                        name = name.Replace(CodeRoom.Text, "");
                        name = name.Trim(' ');
                        name = name.Substring(name.IndexOf(" ") + 1, name.Length - name.IndexOf(" ") - 1);
                        name4.Text = name;

                    }
                }

                if (all_characteristic [i].IndexOf("5", 0, 2) > -1)
                {

                    if (all_characteristic [i].IndexOf("ID_NAME") > -1)
                    {
                        int name_pos = all_characteristic [i].IndexOf("ID_NAME");
                        int name_pos_end = all_characteristic [i].IndexOf("info");

                        player_identifier5.Text = all_characteristic[i].Substring(name_pos + 7 + 1, name_pos_end - name_pos - 7 - 1);
                        string name = all_characteristic [i].Substring(name_pos + 7 + 1, name_pos_end - name_pos - 7 - 1);
                        name = name.Replace(CodeRoom.Text, "");
                        name = name.Trim(' ');
                        name = name.Substring(name.IndexOf(" ") + 1, name.Length - name.IndexOf(" ") - 1);
                        name5.Text = name;

                    }
                }

                if (all_characteristic [i].IndexOf("6", 0, 2) > -1)
                {

                    if (all_characteristic [i].IndexOf("ID_NAME") > -1)
                    {
                        int name_pos = all_characteristic [i].IndexOf("ID_NAME");
                        int name_pos_end = all_characteristic [i].IndexOf("info");

                        player_identifier6.Text = all_characteristic[i].Substring(name_pos + 7 + 1, name_pos_end - name_pos - 7 - 1);
                        string name = all_characteristic [i].Substring(name_pos + 7 + 1, name_pos_end - name_pos - 7 - 1);
                        name = name.Replace(CodeRoom.Text, "");
                        name = name.Trim(' ');
                        name = name.Substring(name.IndexOf(" ") + 1, name.Length - name.IndexOf(" ") - 1);
                        name6.Text = name;

                    }
                }

                if (all_characteristic [i].IndexOf("7", 0, 2) > -1)
                {

                    if (all_characteristic [i].IndexOf("ID_NAME") > -1)
                    {
                        int name_pos = all_characteristic [i].IndexOf("ID_NAME");
                        int name_pos_end = all_characteristic [i].IndexOf("info");

                        player_identifier7.Text = all_characteristic[i].Substring(name_pos + 7 + 1, name_pos_end - name_pos - 7 - 1);
                        string name = all_characteristic [i].Substring(name_pos + 7 + 1, name_pos_end - name_pos - 7 - 1);
                        name = name.Replace(CodeRoom.Text, "");
                        name = name.Trim(' ');
                        name = name.Substring(name.IndexOf(" ") + 1, name.Length - name.IndexOf(" ") - 1);
                        name7.Text = name;

                    }
                }

                if (all_characteristic [i].IndexOf("8", 0, 2) > -1)
                {

                    if (all_characteristic [i].IndexOf("ID_NAME") > -1)
                    {
                        int name_pos = all_characteristic [i].IndexOf("ID_NAME");
                        int name_pos_end = all_characteristic [i].IndexOf("info");

                        player_identifier8.Text = all_characteristic[i].Substring(name_pos + 7 + 1, name_pos_end - name_pos - 7 - 1);
                        string name = all_characteristic [i].Substring(name_pos + 7 + 1, name_pos_end - name_pos - 7 - 1);
                        name = name.Replace(CodeRoom.Text, "");
                        name = name.Trim(' ');
                        name = name.Substring(name.IndexOf(" ") + 1, name.Length - name.IndexOf(" ") - 1);
                        name8.Text = name;

                    }
                }

                if (all_characteristic [i].IndexOf("9", 0, 2) > -1)
                {

                    if (all_characteristic [i].IndexOf("ID_NAME") > -1)
                    {
                        int name_pos = all_characteristic [i].IndexOf("ID_NAME");
                        int name_pos_end = all_characteristic [i].IndexOf("info");

                        player_identifier9.Text = all_characteristic[i].Substring(name_pos + 7 + 1, name_pos_end - name_pos - 7 - 1);
                        string name = all_characteristic [i].Substring(name_pos + 7 + 1, name_pos_end - name_pos - 7 - 1);
                        name = name.Replace(CodeRoom.Text, "");
                        name = name.Trim(' ');
                        name = name.Substring(name.IndexOf(" ") + 1, name.Length - name.IndexOf(" ") - 1);
                        name9.Text = name;

                    }
                }
                if (all_characteristic [i].IndexOf("10", 0, 2) > -1)
                {

                    if (all_characteristic [i].IndexOf("ID_NAME") > -1)
                    {
                        int name_pos = all_characteristic [i].IndexOf("ID_NAME");
                        int name_pos_end = all_characteristic [i].IndexOf("info");

                        player_identifier10.Text = all_characteristic[i].Substring(name_pos + 7 + 1, name_pos_end - name_pos - 7 - 1);
                        string name = all_characteristic [i].Substring(name_pos + 7 + 1, name_pos_end - name_pos - 7 - 1);
                        name = name.Replace(CodeRoom.Text, "");
                        name = name.Trim(' ');
                        name = name.Substring(name.IndexOf(" ") + 1, name.Length - name.IndexOf(" ") - 1);
                        name10.Text = name;

                    }
                }
                if (all_characteristic [i].IndexOf("11", 0, 2) > -1)
                {

                    if (all_characteristic [i].IndexOf("ID_NAME") > -1)
                    {
                        int name_pos = all_characteristic [i].IndexOf("ID_NAME");
                        int name_pos_end = all_characteristic [i].IndexOf("info");

                        player_identifier11.Text = all_characteristic[i].Substring(name_pos + 7 + 1, name_pos_end - name_pos - 7 - 1);
                        string name = all_characteristic [i].Substring(name_pos + 7 + 1, name_pos_end - name_pos - 7 - 1);
                        name = name.Replace(CodeRoom.Text, "");
                        name = name.Trim(' ');
                        name = name.Substring(name.IndexOf(" ") + 1, name.Length - name.IndexOf(" ") - 1);
                        name11.Text = name;

                    }
                }
                if (all_characteristic [i].IndexOf("12", 0, 2) > -1)
                {

                    if (all_characteristic [i].IndexOf("ID_NAME") > -1)
                    {
                        int name_pos = all_characteristic [i].IndexOf("ID_NAME");
                        int name_pos_end = all_characteristic [i].IndexOf("info");

                        player_identifier12.Text = all_characteristic[i].Substring(name_pos + 7 + 1, name_pos_end - name_pos - 7 - 1);
                        string name = all_characteristic [i].Substring(name_pos + 7 + 1, name_pos_end - name_pos - 7 - 1);
                        name = name.Replace(CodeRoom.Text, "");
                        name = name.Trim(' ');
                        name = name.Substring(name.IndexOf(" ") + 1, name.Length - name.IndexOf(" ") - 1);
                        name12.Text = name;

                    }
                }

            }

            listBox1.BackColor = Color.Green;
        }

        private void open_characteristic(string characteristic_buff)
        {
            //OPEN_CHARACTE ID_ROOM=wmb ID_CLIENT=1 dan CHARATER=Age=58

            int id_room_pos = characteristic_buff.IndexOf("ID_ROOM",0);
            int id_client_pos = characteristic_buff.IndexOf("ID_CLIENT", 0);
            int characteristic_pos = characteristic_buff.IndexOf("CHARATER", 0);

            string id_room = characteristic_buff.Substring(id_room_pos+8, id_client_pos - 8- id_room_pos-1);
            string id_client = characteristic_buff.Substring(id_client_pos+10, characteristic_pos-10 - id_client_pos-1);
            string characteristic = characteristic_buff.Substring(characteristic_pos+9, characteristic_buff.Length - 9 -characteristic_pos );

            string player_name = id_room + " " + id_client;
            //Age Sex Baggage Job Hobby Health Phobia Character
            #region open_characteristic
            if (player_identifier1.Text.IndexOf(player_name) > -1)
            {
                int characteristic_id_pos = characteristic.IndexOf("=");
                string characteristic_id = characteristic.Substring(0, characteristic_id_pos);
                string characteristic_data = characteristic.Substring(characteristic_id_pos+1, characteristic.Length - characteristic_id_pos-1);

                if (characteristic_id.IndexOf("Age") > -1)
                {
                    AgeText1.Text = characteristic_data;
                }
                if (characteristic_id.IndexOf("Sex") > -1 )
                {
                    SexText1.Text = characteristic_data;
                }
                if (characteristic_id.IndexOf("Baggage") >-1)
                {
                    BaggageText1.Text = characteristic_data;
                }
                if (characteristic_id.IndexOf("Job") > -1 )
                {
                    JobText1.Text = characteristic_data;
                }
                if (characteristic_id.IndexOf("Hobby") > -1)
                {
                    HobbyText1.Text = characteristic_data;
                }
                if (characteristic_id.IndexOf("Health") > -1 )
                {
                    HealthText1.Text = characteristic_data;
                }
                if (characteristic_id.IndexOf("Phobia") > -1 )
                {
                    PhobiaText1.Text = characteristic_data;
                }
                if (characteristic_id.IndexOf("Character") > -1)
                {
                    CharacterText1.Text = characteristic_data;
                }
            }
            if (player_identifier2.Text.IndexOf(player_name) > -1)
            {
                int characteristic_id_pos = characteristic.IndexOf("=");
                string characteristic_id = characteristic.Substring(0, characteristic_id_pos);
                string characteristic_data = characteristic.Substring(characteristic_id_pos + 1, characteristic.Length - characteristic_id_pos - 1);

                if (characteristic_id.IndexOf("Age") > -1)
                {
                    AgeText2.Text = characteristic_data;
                }
                if (characteristic_id.IndexOf("Sex") > -1)
                {
                    SexText2.Text = characteristic_data;
                }
                if (characteristic_id.IndexOf("Baggage") > -1)
                {
                    BaggageText2.Text = characteristic_data;
                }
                if (characteristic_id.IndexOf("Job") > -1)
                {
                    JobText2.Text = characteristic_data;
                }
                if (characteristic_id.IndexOf("Hobby") > -1)
                {
                    HobbyText2.Text = characteristic_data;
                }
                if (characteristic_id.IndexOf("Health") > -1)
                {
                    HealthText2.Text = characteristic_data;
                }
                if (characteristic_id.IndexOf("Phobia") > -1)
                {
                    PhobiaText2.Text = characteristic_data;
                }
                if (characteristic_id.IndexOf("Character") > -1)
                {
                    CharacterText2.Text = characteristic_data;
                }
            }
            if (player_identifier3.Text.IndexOf(player_name) > -1)
            {
                int characteristic_id_pos = characteristic.IndexOf("=");
                string characteristic_id = characteristic.Substring(0, characteristic_id_pos);
                string characteristic_data = characteristic.Substring(characteristic_id_pos + 1, characteristic.Length - characteristic_id_pos - 1);

                if (characteristic_id.IndexOf("Age") > -1)
                {
                    AgeText3.Text = characteristic_data;
                }
                if (characteristic_id.IndexOf("Sex") > -1)
                {
                    SexText3.Text = characteristic_data;
                }
                if (characteristic_id.IndexOf("Baggage") > -1)
                {
                    BaggageText3.Text = characteristic_data;
                }
                if (characteristic_id.IndexOf("Job") > -1)
                {
                    JobText3.Text = characteristic_data;
                }
                if (characteristic_id.IndexOf("Hobby") > -1)
                {
                    HobbyText3.Text = characteristic_data;
                }
                if (characteristic_id.IndexOf("Health") > -1)
                {
                    HealthText3.Text = characteristic_data;
                }
                if (characteristic_id.IndexOf("Phobia") > -1)
                {
                    PhobiaText3.Text = characteristic_data;
                }
                if (characteristic_id.IndexOf("Character") > -1)
                {
                    CharacterText3.Text = characteristic_data;
                }
            }
            if (player_identifier4.Text.IndexOf(player_name) > -1)
            {
                int characteristic_id_pos = characteristic.IndexOf("=");
                string characteristic_id = characteristic.Substring(0, characteristic_id_pos);
                string characteristic_data = characteristic.Substring(characteristic_id_pos + 1, characteristic.Length - characteristic_id_pos - 1);

                if (characteristic_id.IndexOf("Age") > -1)
                {
                    AgeText4.Text = characteristic_data;
                }
                if (characteristic_id.IndexOf("Sex") > -1)
                {
                    SexText4.Text = characteristic_data;
                }
                if (characteristic_id.IndexOf("Baggage") > -1)
                {
                    BaggageText4.Text = characteristic_data;
                }
                if (characteristic_id.IndexOf("Job") > -1)
                {
                    JobText4.Text = characteristic_data;
                }
                if (characteristic_id.IndexOf("Hobby") > -1)
                {
                    HobbyText4.Text = characteristic_data;
                }
                if (characteristic_id.IndexOf("Health") > -1)
                {
                    HealthText4.Text = characteristic_data;
                }
                if (characteristic_id.IndexOf("Phobia") > -1)
                {
                    PhobiaText4.Text = characteristic_data;
                }
                if (characteristic_id.IndexOf("Character") > -1)
                {
                    CharacterText4.Text = characteristic_data;
                }
            }
            if (player_identifier5.Text.IndexOf(player_name) > -1)
            {
                int characteristic_id_pos = characteristic.IndexOf("=");
                string characteristic_id = characteristic.Substring(0, characteristic_id_pos);
                string characteristic_data = characteristic.Substring(characteristic_id_pos + 1, characteristic.Length - characteristic_id_pos - 1);

                if (characteristic_id.IndexOf("Age") > -1)
                {
                    AgeText5.Text = characteristic_data;
                }
                if (characteristic_id.IndexOf("Sex") > -1)
                {
                    SexText5.Text = characteristic_data;
                }
                if (characteristic_id.IndexOf("Baggage") > -1)
                {
                    BaggageText5.Text = characteristic_data;
                }
                if (characteristic_id.IndexOf("Job") > -1)
                {
                    JobText5.Text = characteristic_data;
                }
                if (characteristic_id.IndexOf("Hobby") > -1)
                {
                    HobbyText5.Text = characteristic_data;
                }
                if (characteristic_id.IndexOf("Health") > -1)
                {
                    HealthText5.Text = characteristic_data;
                }
                if (characteristic_id.IndexOf("Phobia") > -1)
                {
                    PhobiaText5.Text = characteristic_data;
                }
                if (characteristic_id.IndexOf("Character") > -1)
                {
                    CharacterText5.Text = characteristic_data;
                }
            }
            if (player_identifier6.Text.IndexOf(player_name) > -1)
            {
                int characteristic_id_pos = characteristic.IndexOf("=");
                string characteristic_id = characteristic.Substring(0, characteristic_id_pos);
                string characteristic_data = characteristic.Substring(characteristic_id_pos + 1, characteristic.Length - characteristic_id_pos - 1);

                if (characteristic_id.IndexOf("Age") > -1)
                {
                    AgeText6.Text = characteristic_data;
                }
                if (characteristic_id.IndexOf("Sex") > -1)
                {
                    SexText6.Text = characteristic_data;
                }
                if (characteristic_id.IndexOf("Baggage") > -1)
                {
                    BaggageText6.Text = characteristic_data;
                }
                if (characteristic_id.IndexOf("Job") > -1)
                {
                    JobText6.Text = characteristic_data;
                }
                if (characteristic_id.IndexOf("Hobby") > -1)
                {
                    HobbyText6.Text = characteristic_data;
                }
                if (characteristic_id.IndexOf("Health") > -1)
                {
                    HealthText6.Text = characteristic_data;
                }
                if (characteristic_id.IndexOf("Phobia") > -1)
                {
                    PhobiaText6.Text = characteristic_data;
                }
                if (characteristic_id.IndexOf("Character") > -1)
                {
                    CharacterText6.Text = characteristic_data;
                }
            }
            if (player_identifier7.Text.IndexOf(player_name) > -1)
            {
                int characteristic_id_pos = characteristic.IndexOf("=");
                string characteristic_id = characteristic.Substring(0, characteristic_id_pos);
                string characteristic_data = characteristic.Substring(characteristic_id_pos + 1, characteristic.Length - characteristic_id_pos - 1);

                if (characteristic_id.IndexOf("Age") > -1)
                {
                    AgeText7.Text = characteristic_data;
                }
                if (characteristic_id.IndexOf("Sex") > -1)
                {
                    SexText7.Text = characteristic_data;
                }
                if (characteristic_id.IndexOf("Baggage") > -1)
                {
                    BaggageText7.Text = characteristic_data;
                }
                if (characteristic_id.IndexOf("Job") > -1)
                {
                    JobText7.Text = characteristic_data;
                }
                if (characteristic_id.IndexOf("Hobby") > -1)
                {
                    HobbyText7.Text = characteristic_data;
                }
                if (characteristic_id.IndexOf("Health") > -1)
                {
                    HealthText7.Text = characteristic_data;
                }
                if (characteristic_id.IndexOf("Phobia") > -1)
                {
                    PhobiaText7.Text = characteristic_data;
                }
                if (characteristic_id.IndexOf("Character") > -1)
                {
                    CharacterText7.Text = characteristic_data;
                }
            }
            if (player_identifier8.Text.IndexOf(player_name) > -1)
            {
                int characteristic_id_pos = characteristic.IndexOf("=");
                string characteristic_id = characteristic.Substring(0, characteristic_id_pos);
                string characteristic_data = characteristic.Substring(characteristic_id_pos + 1, characteristic.Length - characteristic_id_pos - 1);

                if (characteristic_id.IndexOf("Age") > -1)
                {
                    AgeText8.Text = characteristic_data;
                }
                if (characteristic_id.IndexOf("Sex") > -1)
                {
                    SexText8.Text = characteristic_data;
                }
                if (characteristic_id.IndexOf("Baggage") > -1)
                {
                    BaggageText8.Text = characteristic_data;
                }
                if (characteristic_id.IndexOf("Job") > -1)
                {
                    JobText8.Text = characteristic_data;
                }
                if (characteristic_id.IndexOf("Hobby") > -1)
                {
                    HobbyText8.Text = characteristic_data;
                }
                if (characteristic_id.IndexOf("Health") > -1)
                {
                    HealthText8.Text = characteristic_data;
                }
                if (characteristic_id.IndexOf("Phobia") > -1)
                {
                    PhobiaText8.Text = characteristic_data;
                }
                if (characteristic_id.IndexOf("Character") > -1)
                {
                    CharacterText8.Text = characteristic_data;
                }
            }
            if (player_identifier9.Text.IndexOf(player_name) > -1)
            {
                int characteristic_id_pos = characteristic.IndexOf("=");
                string characteristic_id = characteristic.Substring(0, characteristic_id_pos);
                string characteristic_data = characteristic.Substring(characteristic_id_pos + 1, characteristic.Length - characteristic_id_pos - 1);

                if (characteristic_id.IndexOf("Age") > -1)
                {
                    AgeText9.Text = characteristic_data;
                }
                if (characteristic_id.IndexOf("Sex") > -1)
                {
                    SexText9.Text = characteristic_data;
                }
                if (characteristic_id.IndexOf("Baggage") > -1)
                {
                    BaggageText9.Text = characteristic_data;
                }
                if (characteristic_id.IndexOf("Job") > -1)
                {
                    JobText9.Text = characteristic_data;
                }
                if (characteristic_id.IndexOf("Hobby") > -1)
                {
                    HobbyText9.Text = characteristic_data;
                }
                if (characteristic_id.IndexOf("Health") > -1)
                {
                    HealthText9.Text = characteristic_data;
                }
                if (characteristic_id.IndexOf("Phobia") > -1)
                {
                    PhobiaText9.Text = characteristic_data;
                }
                if (characteristic_id.IndexOf("Character") > -1)
                {
                    CharacterText9.Text = characteristic_data;
                }
            }
            if (player_identifier10.Text.IndexOf(player_name) > -1)
            {
                int characteristic_id_pos = characteristic.IndexOf("=");
                string characteristic_id = characteristic.Substring(0, characteristic_id_pos);
                string characteristic_data = characteristic.Substring(characteristic_id_pos + 1, characteristic.Length - characteristic_id_pos - 1);

                if (characteristic_id.IndexOf("Age") > -1)
                {
                    AgeText10.Text = characteristic_data;
                }
                if (characteristic_id.IndexOf("Sex") > -1)
                {
                    SexText10.Text = characteristic_data;
                }
                if (characteristic_id.IndexOf("Baggage") > -1)
                {
                    BaggageText10.Text = characteristic_data;
                }
                if (characteristic_id.IndexOf("Job") > -1)
                {
                    JobText10.Text = characteristic_data;
                }
                if (characteristic_id.IndexOf("Hobby") > -1)
                {
                    HobbyText10.Text = characteristic_data;
                }
                if (characteristic_id.IndexOf("Health") > -1)
                {
                    HealthText10.Text = characteristic_data;
                }
                if (characteristic_id.IndexOf("Phobia") > -1)
                {
                    PhobiaText10.Text = characteristic_data;
                }
                if (characteristic_id.IndexOf("Character") > -1)
                {
                    CharacterText10.Text = characteristic_data;
                }
            }
            if (player_identifier11.Text.IndexOf(player_name) > -1)
            {
                int characteristic_id_pos = characteristic.IndexOf("=");
                string characteristic_id = characteristic.Substring(0, characteristic_id_pos);
                string characteristic_data = characteristic.Substring(characteristic_id_pos + 1, characteristic.Length - characteristic_id_pos - 1);

                if (characteristic_id.IndexOf("Age") > -1)
                {
                    AgeText11.Text = characteristic_data;
                }
                if (characteristic_id.IndexOf("Sex") > -1)
                {
                    SexText11.Text = characteristic_data;
                }
                if (characteristic_id.IndexOf("Baggage") > -1)
                {
                    BaggageText11.Text = characteristic_data;
                }
                if (characteristic_id.IndexOf("Job") > -1)
                {
                    JobText11.Text = characteristic_data;
                }
                if (characteristic_id.IndexOf("Hobby") > -1)
                {
                    HobbyText11.Text = characteristic_data;
                }
                if (characteristic_id.IndexOf("Health") > -1)
                {
                    HealthText11.Text = characteristic_data;
                }
                if (characteristic_id.IndexOf("Phobia") > -1)
                {
                    PhobiaText11.Text = characteristic_data;
                }
                if (characteristic_id.IndexOf("Character") > -1)
                {
                    CharacterText11.Text = characteristic_data;
                }
            }
            if (player_identifier12.Text.IndexOf(player_name) > -1)
            {
                int characteristic_id_pos = characteristic.IndexOf("=");
                string characteristic_id = characteristic.Substring(0, characteristic_id_pos);
                string characteristic_data = characteristic.Substring(characteristic_id_pos + 1, characteristic.Length - characteristic_id_pos - 1);

                if (characteristic_id.IndexOf("Age") > -1)
                {
                    AgeText12.Text = characteristic_data;
                }
                if (characteristic_id.IndexOf("Sex") > -1)
                {
                    SexText12.Text = characteristic_data;
                }
                if (characteristic_id.IndexOf("Baggage") > -1)
                {
                    BaggageText12.Text = characteristic_data;
                }
                if (characteristic_id.IndexOf("Job") > -1)
                {
                    JobText12.Text = characteristic_data;
                }
                if (characteristic_id.IndexOf("Hobby") > -1)
                {
                    HobbyText12.Text = characteristic_data;
                }
                if (characteristic_id.IndexOf("Health") > -1)
                {
                    HealthText12.Text = characteristic_data;
                }
                if (characteristic_id.IndexOf("Phobia") > -1)
                {
                    PhobiaText12.Text = characteristic_data;
                }
                if (characteristic_id.IndexOf("Character") > -1)
                {
                    CharacterText12.Text = characteristic_data;
                }
            }
            #endregion
        }

        private void next_move(string data)
        {
            //NEXT_____MOVE->ID_ROOM= ID_CLIENT= ID_NAME=

            int id_room_pos = data.IndexOf("ID_ROOM");
            int id_client_pos = data.IndexOf("ID_CLIENT");
            int id_name_pos = data.IndexOf("ID_NAME");

            string id_room_move = data.Substring(id_room_pos + 8, id_client_pos - id_room_pos - 8-1);
            string id_client_move = data.Substring(id_client_pos + 10, id_name_pos - id_client_pos - 10-1);
            string id_name_move = data.Substring(id_name_pos + 8, data.Length - id_name_pos - 8-1);

            string all_name = id_room_move + " " + id_client_move + " " + id_name_move;
            string all_name_save = id_client +  id_name;

            if (id_client.IndexOf(id_client_move) > -1 && id_name.IndexOf(id_name_move) > -1)
            {
                //istBox1.BackColor = Color.Green; 
                Next_Move_button.Visible = true;

            }
            if (player_identifier1.Text.IndexOf(all_name) > -1)
            {
                listBox1.BackColor = Color.Green;

                listBox2.BackColor = Color.LightGray;
                listBox9.BackColor = Color.LightGray;
                listBox8.BackColor = Color.LightGray;
                listBox7.BackColor = Color.LightGray;
                listBox6.BackColor = Color.LightGray;
                listBox5.BackColor = Color.LightGray;
                listBox4.BackColor = Color.LightGray;
                listBox3.BackColor = Color.LightGray;
                listBox10.BackColor = Color.LightGray;
                listBox11.BackColor = Color.LightGray;
                listBox12.BackColor = Color.LightGray;

               
                

            }
            else if (player_identifier2.Text.IndexOf(all_name) > -1)
            {
                listBox2.BackColor = Color.Green;

                listBox1.BackColor = Color.LightGray;
                listBox9.BackColor = Color.LightGray;
                listBox8.BackColor = Color.LightGray;
                listBox7.BackColor = Color.LightGray;
                listBox6.BackColor = Color.LightGray;
                listBox5.BackColor = Color.LightGray;
                listBox4.BackColor = Color.LightGray;
                listBox3.BackColor = Color.LightGray;
                listBox10.BackColor = Color.LightGray;
                listBox11.BackColor = Color.LightGray;
                listBox12.BackColor = Color.LightGray;

               
               
            }
            else if (player_identifier3.Text.IndexOf(all_name) > -1)
            {
                listBox9.BackColor = Color.Green;

                listBox1.BackColor = Color.LightGray;
                listBox2.BackColor = Color.LightGray;
                listBox8.BackColor = Color.LightGray;
                listBox7.BackColor = Color.LightGray;
                listBox6.BackColor = Color.LightGray;
                listBox5.BackColor = Color.LightGray;
                listBox4.BackColor = Color.LightGray;
                listBox3.BackColor = Color.LightGray;
                listBox10.BackColor = Color.LightGray;
                listBox11.BackColor = Color.LightGray;
                listBox12.BackColor = Color.LightGray;

               
                
            }        
            else if (player_identifier4.Text.IndexOf(all_name) > -1)
            {
                listBox8.BackColor = Color.Green;

                listBox1.BackColor = Color.LightGray;
                listBox2.BackColor = Color.LightGray;
                listBox9.BackColor = Color.LightGray;
                listBox7.BackColor = Color.LightGray;
                listBox6.BackColor = Color.LightGray;
                listBox5.BackColor = Color.LightGray;
                listBox4.BackColor = Color.LightGray;
                listBox3.BackColor = Color.LightGray;
                listBox10.BackColor = Color.LightGray;
                listBox11.BackColor = Color.LightGray;
                listBox12.BackColor = Color.LightGray;

               
             

            }
            else if (player_identifier5.Text.IndexOf(all_name) > -1)
            {
                listBox7.BackColor = Color.Green;

                listBox1.BackColor = Color.LightGray;
                listBox2.BackColor = Color.LightGray;
                listBox9.BackColor = Color.LightGray;
                listBox8.BackColor = Color.LightGray;
                listBox6.BackColor = Color.LightGray;
                listBox5.BackColor = Color.LightGray;
                listBox4.BackColor = Color.LightGray;
                listBox3.BackColor = Color.LightGray;
                listBox10.BackColor = Color.LightGray;
                listBox11.BackColor = Color.LightGray;
                listBox12.BackColor = Color.LightGray;

               
        

            }
            else if (player_identifier6.Text.IndexOf(all_name) > -1)
            {
                listBox6.BackColor = Color.Green;

                listBox1.BackColor = Color.LightGray;
                listBox2.BackColor = Color.LightGray;
                listBox9.BackColor = Color.LightGray;
                listBox8.BackColor = Color.LightGray;
                listBox7.BackColor = Color.LightGray;
                listBox5.BackColor = Color.LightGray;
                listBox4.BackColor = Color.LightGray;
                listBox3.BackColor = Color.LightGray;
                listBox10.BackColor = Color.LightGray;
                listBox11.BackColor = Color.LightGray;
                listBox12.BackColor = Color.LightGray;

                
             
            }
            else if (player_identifier7.Text.IndexOf(all_name) > -1)
            {
                listBox5.BackColor = Color.Green;

                listBox1.BackColor = Color.LightGray;
                listBox2.BackColor = Color.LightGray;
                listBox9.BackColor = Color.LightGray;
                listBox8.BackColor = Color.LightGray;
                listBox7.BackColor = Color.LightGray;
                listBox6.BackColor = Color.LightGray;
                listBox4.BackColor = Color.LightGray;
                listBox3.BackColor = Color.LightGray;
                listBox10.BackColor = Color.LightGray;
                listBox11.BackColor = Color.LightGray;
                listBox12.BackColor = Color.LightGray;

               
               
            }
            else if (player_identifier8.Text.IndexOf(all_name) > -1)
            {
                listBox4.BackColor = Color.Green;


                listBox1.BackColor = Color.LightGray;
                listBox2.BackColor = Color.LightGray;
                listBox9.BackColor = Color.LightGray;
                listBox8.BackColor = Color.LightGray;
                listBox7.BackColor = Color.LightGray;
                listBox6.BackColor = Color.LightGray;
                listBox5.BackColor = Color.LightGray;
                listBox3.BackColor = Color.LightGray;
                listBox10.BackColor = Color.LightGray;
                listBox11.BackColor = Color.LightGray;
                listBox12.BackColor = Color.LightGray;

                
              
            }
            else if (player_identifier9.Text.IndexOf(all_name) > -1)
            {
                listBox3.BackColor = Color.Green;


                listBox1.BackColor = Color.LightGray;
                listBox2.BackColor = Color.LightGray;
                listBox9.BackColor = Color.LightGray;
                listBox8.BackColor = Color.LightGray;
                listBox7.BackColor = Color.LightGray;
                listBox6.BackColor = Color.LightGray;
                listBox5.BackColor = Color.LightGray;
                listBox4.BackColor = Color.LightGray;
                listBox10.BackColor = Color.LightGray;
                listBox11.BackColor = Color.LightGray;
                listBox12.BackColor = Color.LightGray;

               
                
            }
            else if (player_identifier10.Text.IndexOf(all_name) > -1)
            {
                listBox10.BackColor = Color.Green;

                listBox1.BackColor = Color.LightGray;
                listBox2.BackColor = Color.LightGray;
                listBox9.BackColor = Color.LightGray;
                listBox8.BackColor = Color.LightGray;
                listBox7.BackColor = Color.LightGray;
                listBox6.BackColor = Color.LightGray;
                listBox5.BackColor = Color.LightGray;
                listBox4.BackColor = Color.LightGray;
                listBox3.BackColor = Color.LightGray;
                listBox11.BackColor = Color.LightGray;
                listBox12.BackColor = Color.LightGray;

                
               
            }
            else if (player_identifier11.Text.IndexOf(all_name) > -1)
            {
                listBox11.BackColor = Color.Green;

                listBox1.BackColor = Color.LightGray;
                listBox2.BackColor = Color.LightGray;
                listBox9.BackColor = Color.LightGray;
                listBox8.BackColor = Color.LightGray;
                listBox7.BackColor = Color.LightGray;
                listBox6.BackColor = Color.LightGray;
                listBox5.BackColor = Color.LightGray;
                listBox4.BackColor = Color.LightGray;
                listBox3.BackColor = Color.LightGray;
                listBox10.BackColor = Color.LightGray;
                listBox12.BackColor = Color.LightGray;

                
                
            }
            else if (player_identifier12.Text.IndexOf(all_name) > -1)
            {
                listBox12.BackColor = Color.Green;

                listBox1.BackColor = Color.LightGray;
                listBox2.BackColor = Color.LightGray;
                listBox9.BackColor = Color.LightGray;
                listBox8.BackColor = Color.LightGray;
                listBox7.BackColor = Color.LightGray;
                listBox6.BackColor = Color.LightGray;
                listBox5.BackColor = Color.LightGray;
                listBox4.BackColor = Color.LightGray;
                listBox3.BackColor = Color.LightGray;
                listBox10.BackColor = Color.LightGray;
                listBox11.BackColor = Color.LightGray;

                
            }

            if(all_name.IndexOf(all_name_save) >-1 && player_identifier1.Text.IndexOf(all_name_save) > -1)
            {
                PersonComponents1ON();
            }
            else if (all_name.IndexOf(all_name_save) > -1 && player_identifier2.Text.IndexOf(all_name_save) > -1)
            {
                PersonComponents2ON();
            }     
            else if (all_name.IndexOf(all_name_save) > -1 && player_identifier3.Text.IndexOf(all_name_save) > -1)
            {
                PersonComponents3ON();
            }
            else if (all_name.IndexOf(all_name_save) > -1 && player_identifier4.Text.IndexOf(all_name_save) > -1)
            {
                PersonComponents4ON();
            }
            else if (all_name.IndexOf(all_name_save) > -1 && player_identifier5.Text.IndexOf(all_name_save) > -1)
            {
                PersonComponents5ON();
            }
            else if (all_name.IndexOf(all_name_save) > -1 && player_identifier6.Text.IndexOf(all_name_save) > -1)
            {
                PersonComponents6ON();
            }
            else if (all_name.IndexOf(all_name_save) > -1 && player_identifier7.Text.IndexOf(all_name_save) > -1)
            {
                PersonComponents7ON();
            }
            else if (all_name.IndexOf(all_name_save) > -1 && player_identifier8.Text.IndexOf(all_name_save) > -1)
            {
                PersonComponents8ON();
            }
            else if (all_name.IndexOf(all_name_save) > -1 && player_identifier9.Text.IndexOf(all_name_save) > -1)
            {
                PersonComponents9ON();
            }
            else if (all_name.IndexOf(all_name_save) > -1 && player_identifier10.Text.IndexOf(all_name_save) > -1)
            {
                PersonComponents10ON();
            }
            else if (all_name.IndexOf(all_name_save) > -1 && player_identifier11.Text.IndexOf(all_name_save) > -1)
            {
                PersonComponents11ON();
            }
            else if (all_name.IndexOf(all_name_save) > -1 && player_identifier12.Text.IndexOf(all_name_save) > -1)
            {
                PersonComponents12ON();
            }
            else
            {
                PersonComponents1OFF();
                PersonComponents2OFF();
                PersonComponents3OFF();
                PersonComponents4OFF();
                PersonComponents5OFF();
                PersonComponents6OFF();
                PersonComponents7OFF();
                PersonComponents8OFF();
                PersonComponents9OFF();
                PersonComponents10OFF();
                PersonComponents11OFF();
                PersonComponents12OFF();
            }
        }

        private void SendDataCompinents(string SendData)
        {
            int Messagesize = Encoding.UTF8.GetByteCount(SendData);

            string Message = "OPEN_CHARACTE" + " ID_ROOM=" + CodeRoom.Text + " ID_CLIENT=" + id_client + id_name + " CHARATER=" + SendData;
            byte[] outStream = new byte[Messagesize];

            outStream = Encoding.UTF8.GetBytes(Message);
            serverStream.Write(outStream, 0, outStream.Length);
            Array.Clear(outStream, 0, outStream.Length);
        }
        #region Components
        public void PersonComponents1OFF()
        {

            Age1.Visible = false;
            Sex1.Visible = false;
            Job1.Visible = false;
            Hobby1.Visible = false;
            Health1.Visible = false;
            Baggage1.Visible = false;
            Phobia1.Visible = false;
            Character1.Visible = false;

            /* AgeText1
             SexText1
             JobText1
             HobbyText1
             HealthText1
             BaggageText1
             PhobiaText1
             CharacterText1*/
        }
        public void PersonComponents2OFF()
        {
            Age2.Visible = false;
            Sex2.Visible = false;
            Job2.Visible = false;
            Hobby2.Visible = false;
            Health2.Visible = false;
            Baggage2.Visible = false;
            Phobia2.Visible = false;
            Character2.Visible = false;
        }
        public void PersonComponents3OFF()
        {
            Age3.Visible = false;
            Sex3.Visible = false;
            Job3.Visible = false;
            Hobby3.Visible = false;
            Health3.Visible = false;
            Baggage3.Visible = false;
            Phobia3.Visible = false;
            Character3.Visible = false;
        }
        public void PersonComponents4OFF()
        {
            Age4.Visible = false;
            Sex4.Visible = false;
            Job4.Visible = false;
            Hobby4.Visible = false;
            Health4.Visible = false;
            Baggage4.Visible = false;
            Phobia4.Visible = false;
            Character4.Visible = false;
        }
        public void PersonComponents5OFF()
        {
            Age5.Visible = false;
            Sex5.Visible = false;
            Job5.Visible = false;
            Hobby5.Visible = false;
            Health5.Visible = false;
            Baggage5.Visible = false;
            Phobia5.Visible = false;
            Character5.Visible = false;
        }
        public void PersonComponents6OFF()
        {
            Age6.Visible = false;
            Sex6.Visible = false;
            Job6.Visible = false;
            Hobby6.Visible = false;
            Health6.Visible = false;
            Baggage6.Visible = false;
            Phobia6.Visible = false;
            Character6.Visible = false;
        }
        public void PersonComponents7OFF()
        {
            Age7.Visible = false;
            Sex7.Visible = false;
            Job7.Visible = false;
            Hobby7.Visible = false;
            Health7.Visible = false;
            Baggage7.Visible = false;
            Phobia7.Visible = false;
            Character7.Visible = false;
        }
        public void PersonComponents8OFF()
        {
            Age8.Visible = false;
            Sex8.Visible = false;
            Job8.Visible = false;
            Hobby8.Visible = false;
            Health8.Visible = false;
            Baggage8.Visible = false;
            Phobia8.Visible = false;
            Character8.Visible = false;
        }
        public void PersonComponents9OFF()
        {
            Age9.Visible = false;
            Sex9.Visible = false;
            Job9.Visible = false;
            Hobby9.Visible = false;
            Health9.Visible = false;
            Baggage9.Visible = false;
            Phobia9.Visible = false;
            Character9.Visible = false;
        }
        public void PersonComponents10OFF()
        {
            Age10.Visible = false;
            Sex10.Visible = false;
            Job10.Visible = false;
            Hobby10.Visible = false;
            Health10.Visible = false;
            Baggage10.Visible = false;
            Phobia10.Visible = false;
            Character10.Visible = false;
        }
        public void PersonComponents11OFF()
        {
            Age11.Visible = false;
            Sex11.Visible = false;
            Job11.Visible = false;
            Hobby11.Visible = false;
            Health11.Visible = false;
            Baggage11.Visible = false;
            Phobia11.Visible = false;
            Character11.Visible = false;
        }
        public void PersonComponents12OFF()
        {
            Age12.Visible = false;
            Sex12.Visible = false;
            Job12.Visible = false;
            Hobby12.Visible = false;
            Health12.Visible = false;
            Baggage12.Visible = false;
            Phobia12.Visible = false;
            Character12.Visible = false;
        }


        public void PersonComponents1ON()
        {

            Age1.Visible = true;
            Sex1.Visible = true;
            Job1.Visible = true;
            Hobby1.Visible = true;
            Health1.Visible = true;
            Baggage1.Visible = true;
            Phobia1.Visible = true;
            Character1.Visible = true;

            /* AgeText1
             SexText1
             JobText1
             HobbyText1
             HealthText1
             BaggageText1
             PhobiaText1
             CharacterText1*/
        }
        public void PersonComponents2ON()
        {
            Age2.Visible = true;
            Sex2.Visible = true;
            Job2.Visible = true;
            Hobby2.Visible = true;
            Health2.Visible = true;
            Baggage2.Visible = true;
            Phobia2.Visible = true;
            Character2.Visible = true;
        }
        public void PersonComponents3ON()
        {
            Age3.Visible = true;
            Sex3.Visible = true;
            Job3.Visible = true;
            Hobby3.Visible = true;
            Health3.Visible = true;
            Baggage3.Visible = true;
            Phobia3.Visible = true;
            Character3.Visible = true;
        }
        public void PersonComponents4ON()
        {
            Age4.Visible = true;
            Sex4.Visible = true;
            Job4.Visible = true;
            Hobby4.Visible = true;
            Health4.Visible = true;
            Baggage4.Visible = true;
            Phobia4.Visible = true;
            Character4.Visible = true;
        }
        public void PersonComponents5ON()
        {
            Age5.Visible = true;
            Sex5.Visible = true;
            Job5.Visible = true;
            Hobby5.Visible = true;
            Health5.Visible = true;
            Baggage5.Visible = true;
            Phobia5.Visible = true;
            Character5.Visible = true;
        }
        public void PersonComponents6ON()
        {
            Age6.Visible = true;
            Sex6.Visible = true;
            Job6.Visible = true;
            Hobby6.Visible = true;
            Health6.Visible = true;
            Baggage6.Visible = true;
            Phobia6.Visible = true;
            Character6.Visible = true;
        }
        public void PersonComponents7ON()
        {
            Age7.Visible = true;
            Sex7.Visible = true;
            Job7.Visible = true;
            Hobby7.Visible = true;
            Health7.Visible = true;
            Baggage7.Visible = true;
            Phobia7.Visible = true;
            Character7.Visible = true;
        }
        public void PersonComponents8ON()
        {
            Age8.Visible = true;
            Sex8.Visible = true;
            Job8.Visible = true;
            Hobby8.Visible = true;
            Health8.Visible = true;
            Baggage8.Visible = true;
            Phobia8.Visible = true;
            Character8.Visible = true;
        }
        public void PersonComponents9ON()
        {
            Age9.Visible = true;
            Sex9.Visible = true;
            Job9.Visible = true;
            Hobby9.Visible = true;
            Health9.Visible = true;
            Baggage9.Visible = true;
            Phobia9.Visible = true;
            Character9.Visible = true;
        }
        public void PersonComponents10ON()
        {
            Age10.Visible = true;
            Sex10.Visible = true;
            Job10.Visible = true;
            Hobby10.Visible = true;
            Health10.Visible = true;
            Baggage10.Visible = true;
            Phobia10.Visible = true;
            Character10.Visible = true;
        }
        public void PersonComponents11ON()
        {
            Age11.Visible = true;
            Sex11.Visible = true;
            Job11.Visible = true;
            Hobby11.Visible = true;
            Health11.Visible = true;
            Baggage11.Visible = true;
            Phobia11.Visible = true;
            Character11.Visible = true;
        }
        public void PersonComponents12ON()
        {
            Age12.Visible = true;
            Sex12.Visible = true;
            Job12.Visible = true;
            Hobby12.Visible = true;
            Health12.Visible = true;
            Baggage12.Visible = true;
            Phobia12.Visible = true;
            Character12.Visible = true;
        }




        #endregion
        #region SendData
        #region 1
        private void Age1_CheckedChanged(object sender, EventArgs e)
        {
            Age1.Enabled = false;
            SendDataCompinents("Age="+ AgeText1.Text);             
        
        }

        private void Sex1_CheckedChanged(object sender, EventArgs e)
        {
            Sex1.Enabled = false;
            SendDataCompinents("Sex=" + SexText1.Text);
        }

        private void Baggage1_CheckedChanged(object sender, EventArgs e)
        {
            Baggage1.Enabled = false;
            SendDataCompinents("Baggage=" + BaggageText1.Text);
        }

        private void Job1_CheckedChanged(object sender, EventArgs e)
        {
            Job1.Enabled = false;
            SendDataCompinents("Job=" + JobText1.Text);
        }

        private void Hobby1_CheckedChanged(object sender, EventArgs e)
        {
            Hobby1.Enabled = false;
            SendDataCompinents("Hobby=" + HobbyText1.Text);
        }

        private void Health1_CheckedChanged(object sender, EventArgs e)
        {
            Health1.Enabled = false;
            SendDataCompinents("Health=" + HealthText1.Text);
        }

        private void Phobia1_CheckedChanged(object sender, EventArgs e)
        {
            Phobia1.Enabled = false;
            SendDataCompinents("Phobia=" + PhobiaText1.Text);
        }

        private void Character1_CheckedChanged(object sender, EventArgs e)
        {
            Character1.Enabled = false;
            SendDataCompinents("Character=" + CharacterText1.Text);
        }
        #endregion
        #region 2
       
        private void Character2_CheckedChanged(object sender, EventArgs e)
        {
            Character2.Enabled = false;
            SendDataCompinents("Character=" + CharacterText2.Text);
        }
        private void Job2_CheckedChanged(object sender, EventArgs e)
        {
            Job2.Enabled = false;
            SendDataCompinents("Job=" + JobText2.Text);
        }
        private void Hobby2_CheckedChanged(object sender, EventArgs e)
        {
            Hobby2.Enabled = false;
            SendDataCompinents("Hobby=" + HobbyText2.Text);
        }
        private void Phobia2_CheckedChanged(object sender, EventArgs e)
        {
            Phobia2.Enabled = false;
            SendDataCompinents("Phobia=" + PhobiaText2.Text);
        }
        private void Health2_CheckedChanged(object sender, EventArgs e)
        {
            Health2.Enabled = false;
            SendDataCompinents("Health=" + HealthText2.Text);
        }
        private void Baggage2_CheckedChanged(object sender, EventArgs e)
        {
            Baggage2.Enabled = false;
            SendDataCompinents("Baggage=" + BaggageText2.Text);
        }
        private void Sex2_CheckedChanged(object sender, EventArgs e)
        {
            Sex2.Enabled = false;
            SendDataCompinents("Sex=" + SexText2.Text);
        }
        private void Age2_CheckedChanged(object sender, EventArgs e)
        {
            Age2.Enabled = false;
            SendDataCompinents("Age=" + AgeText2.Text);
        }
        #endregion
        #region 3
        private void Health3_CheckedChanged(object sender, EventArgs e)
        {
            Health3.Enabled = false;
            SendDataCompinents("Health=" + HealthText3.Text);
        }
        private void Baggage3_CheckedChanged(object sender, EventArgs e)
        {
            Baggage3.Enabled = false;
            SendDataCompinents("Baggage=" + BaggageText3.Text);
        }
        private void Sex3_CheckedChanged(object sender, EventArgs e)
        {
            Sex3.Enabled = false;
            SendDataCompinents("Sex=" + SexText3.Text);
        }
        private void Age3_CheckedChanged(object sender, EventArgs e)
        {
            Age3.Enabled = false;
            SendDataCompinents("Age=" + AgeText3.Text);
        }
        private void Job3_CheckedChanged(object sender, EventArgs e)
        {
            Job3.Enabled = false;
            SendDataCompinents("Job=" + JobText3.Text);
        }
        private void Character3_CheckedChanged(object sender, EventArgs e)
        {
            Character3.Enabled = false;
            SendDataCompinents("Character=" + CharacterText3.Text);
        }
        private void Hobby3_CheckedChanged(object sender, EventArgs e)
        {
            Hobby3.Enabled = false;
            SendDataCompinents("Hobby=" + HobbyText3.Text);
        }
        private void Phobia3_CheckedChanged(object sender, EventArgs e)
        {
            Phobia3.Enabled = false;
            SendDataCompinents("Phobia=" + PhobiaText3.Text);
        }
        #endregion
        #region 4
        private void Age4_CheckedChanged(object sender, EventArgs e)
            {
            Age4.Enabled = false;
            SendDataCompinents("Age=" + AgeText4.Text);
        }
        private void Job4_CheckedChanged(object sender, EventArgs e)
            {
            Job4.Enabled = false;
            SendDataCompinents("Job=" + JobText4.Text);
        }
        private void Hobby4_CheckedChanged(object sender, EventArgs e)
        {
            Hobby4.Enabled = false;
            SendDataCompinents("Hobby=" + HobbyText4.Text);
        }
        private void Health4_CheckedChanged(object sender, EventArgs e)
        {
            Health4.Enabled = false;
            SendDataCompinents("Health=" + HealthText4.Text);
        }
        private void Baggage4_CheckedChanged(object sender, EventArgs e)
            {
            Baggage4.Enabled = false;
            SendDataCompinents("Baggage=" + BaggageText4.Text);
        }
        private void Sex4_CheckedChanged(object sender, EventArgs e)
        {
            Sex4.Enabled = false;
            SendDataCompinents("Sex=" + SexText4.Text);
        }
        private void Phobia4_CheckedChanged(object sender, EventArgs e)
        {
            Phobia4.Enabled = false;
            SendDataCompinents("Phobia=" + PhobiaText4.Text);
        }
        private void Character4_CheckedChanged(object sender, EventArgs e)
        {
            Character4.Enabled = false;
            SendDataCompinents("Character=" + CharacterText4.Text);
        }
            #endregion
        #region 5
        private void Phobia5_CheckedChanged(object sender, EventArgs e)
        {
            Phobia5.Enabled = false;
            SendDataCompinents("Phobia=" + PhobiaText5.Text);
        }
        private void Health5_CheckedChanged(object sender, EventArgs e)
        {
            Health5.Enabled = false;
            SendDataCompinents("Health=" + HealthText5.Text);
        }
        private void Hobby5_CheckedChanged(object sender, EventArgs e)
        {
            Hobby5.Enabled = false;
            SendDataCompinents("Hobby=" + HobbyText5.Text);
        }
        private void Baggage5_CheckedChanged(object sender, EventArgs e)
        {
            Baggage5.Enabled = false;
            SendDataCompinents("Baggage=" + BaggageText5.Text);
        }
        private void Sex5_CheckedChanged(object sender, EventArgs e)
        {
            Sex5.Enabled = false;
            SendDataCompinents("Sex=" + SexText5.Text);
        }
        private void Age5_CheckedChanged(object sender, EventArgs e)
        {
            Age5.Enabled = false;
            SendDataCompinents("Age=" + AgeText5.Text);
        }
        private void Job5_CheckedChanged(object sender, EventArgs e)
        {
            Job5.Enabled = false;
            SendDataCompinents("Job=" + JobText5.Text);
        }
        private void Character5_CheckedChanged(object sender, EventArgs e)
        {
            Character5.Enabled = false;
            SendDataCompinents("Character=" + CharacterText5.Text);
        }
        #endregion
        #region 6
        private void Job6_CheckedChanged(object sender, EventArgs e)
        {
            Job6.Enabled = false;
            SendDataCompinents("Job=" + JobText6.Text);
        }
        private void Baggage6_CheckedChanged(object sender, EventArgs e)
        {
            Baggage6.Enabled = false;
            SendDataCompinents("Baggage=" + BaggageText6.Text);
        }
        private void Character6_CheckedChanged(object sender, EventArgs e)
        {
            Character6.Enabled = false;
            SendDataCompinents("Character=" + CharacterText6.Text);
        }
        private void Phobia6_CheckedChanged(object sender, EventArgs e)
        {
            Phobia6.Enabled = false;
            SendDataCompinents("Phobia=" + PhobiaText6.Text);
        }
        private void Sex6_CheckedChanged(object sender, EventArgs e)
        {
            Sex6.Enabled = false;
            SendDataCompinents("Sex=" + SexText6.Text);
        }
        private void Health6_CheckedChanged(object sender, EventArgs e)
        {
            Health6.Enabled = false;
            SendDataCompinents("Health=" + HealthText6.Text);
        }
        private void Hobby6_CheckedChanged(object sender, EventArgs e)
        {
            Hobby6.Enabled = false;
            SendDataCompinents("Hobby=" + HobbyText6.Text);
        }
        private void Age6_CheckedChanged(object sender, EventArgs e)
        {
            Age6.Enabled = false;
            SendDataCompinents("Age=" + AgeText6.Text);
        }
        #endregion
        #region 7
        private void Sex7_CheckedChanged(object sender, EventArgs e)
        {
            Sex7.Enabled = false;
            SendDataCompinents("Sex=" + SexText7.Text);
        }      
        private void Health7_CheckedChanged(object sender, EventArgs e)
        {
            Health7.Enabled = false;
            SendDataCompinents("Health=" + HealthText7.Text);
        }
        private void Baggage7_CheckedChanged(object sender, EventArgs e)
        {
            Baggage7.Enabled = false;
            SendDataCompinents("Baggage=" + BaggageText7.Text);
        }
        private void Phobia7_CheckedChanged(object sender, EventArgs e)
        {
            Phobia7.Enabled = false;
            SendDataCompinents("Phobia=" + PhobiaText7.Text);
        }
        private void Job7_CheckedChanged(object sender, EventArgs e)
        {
            Job7.Enabled = false;
            SendDataCompinents("Job=" + JobText7.Text);
        }
        private void Hobby7_CheckedChanged(object sender, EventArgs e)
        {
            Hobby7.Enabled = false;
            SendDataCompinents("Hobby=" + HobbyText7.Text);
        }
        private void Age7_CheckedChanged(object sender, EventArgs e)
        {
            Age7.Enabled = false;
            SendDataCompinents("Age=" + AgeText7.Text);
        }
        private void Character7_CheckedChanged(object sender, EventArgs e)
        {
            Character7.Enabled = false;
            SendDataCompinents("Character=" + CharacterText7.Text);
        }
        #endregion
        #region 8
        private void Hobby8_CheckedChanged(object sender, EventArgs e)
        {
            Hobby8.Enabled = false;
            SendDataCompinents("Hobby=" + HobbyText8.Text);
        }
        private void Character8_CheckedChanged(object sender, EventArgs e)
        {
            Character8.Enabled = false;
            SendDataCompinents("Character=" + CharacterText8.Text);
        }
        private void Baggage8_CheckedChanged(object sender, EventArgs e)
        {
            Baggage8.Enabled = false;
            SendDataCompinents("Baggage=" + BaggageText8.Text);
        }
        private void Phobia8_CheckedChanged(object sender, EventArgs e)
        {
            Phobia8.Enabled = false;
            SendDataCompinents("Phobia=" + PhobiaText8.Text);
        }
        private void Sex8_CheckedChanged(object sender, EventArgs e)
        {
            Sex8.Enabled = false;
            SendDataCompinents("Sex=" + SexText8.Text);
        }
        private void Health8_CheckedChanged(object sender, EventArgs e)
        {
            Health8.Enabled = false;
            SendDataCompinents("Health=" + HealthText8.Text);
        }
        private void Age8_CheckedChanged(object sender, EventArgs e)
        {
            Age8.Enabled = false;
            SendDataCompinents("Age=" + AgeText8.Text);
        }
        private void Job8_CheckedChanged(object sender, EventArgs e)
        {
            Job8.Enabled = false;
            SendDataCompinents("Job=" + JobText8.Text);
        }
        #endregion
        #region 9
          private void Phobia9_CheckedChanged(object sender, EventArgs e)
          {
            Phobia9.Enabled = false;
            SendDataCompinents("Phobia=" + PhobiaText9.Text);
        }     
          private void Sex9_CheckedChanged(object sender, EventArgs e)
          {
            Sex9.Enabled = false;
            SendDataCompinents("Sex=" + SexText9.Text);
        }
          private void Health9_CheckedChanged(object sender, EventArgs e)
           {
            Health9.Enabled = false;
            SendDataCompinents("Health=" + HealthText9.Text);
        }
          private void Hobby9_CheckedChanged(object sender, EventArgs e)
             {
            Hobby9.Enabled = false;
            SendDataCompinents("Hobby=" + HobbyText9.Text);
        }
          private void Baggage9_CheckedChanged(object sender, EventArgs e)
             {
            Baggage9.Enabled = false;
            SendDataCompinents("Baggage=" + BaggageText9.Text);
        }
          private void Job9_CheckedChanged(object sender, EventArgs e)
           {
            Job9.Enabled = false;
            SendDataCompinents("Job=" + JobText9.Text);
        }
          private void Age9_CheckedChanged(object sender, EventArgs e)
        {
            Age9.Enabled = false;
            SendDataCompinents("Age=" + AgeText9.Text);
        }
          private void Character9_CheckedChanged(object sender, EventArgs e)
        {
            Character9.Enabled = false;
            SendDataCompinents("Character=" + CharacterText9.Text);
        }

         #endregion
        #region 10

        private void Health10_CheckedChanged(object sender, EventArgs e)
        {
            Health10.Enabled = false;
            SendDataCompinents("Health=" + HealthText10.Text);
        }
        private void Sex10_CheckedChanged(object sender, EventArgs e)
        {
            Sex10.Enabled = false;
            SendDataCompinents("Sex=" + SexText10.Text);
        }
        private void Age10_CheckedChanged(object sender, EventArgs e)
        {
            Age10.Enabled = false;
            SendDataCompinents("Age=" + AgeText10.Text);
        }
        private void Baggage10_CheckedChanged(object sender, EventArgs e)
        {
            Baggage10.Enabled = false;
            SendDataCompinents("Baggage=" + BaggageText10.Text);
        }
        private void Hobby10_CheckedChanged(object sender, EventArgs e)
        {
            Hobby10.Enabled = false;
            SendDataCompinents("Hobby=" + HobbyText10.Text);
        }
        private void Phobia10_CheckedChanged(object sender, EventArgs e)
        {
            Phobia10.Enabled = false;
            SendDataCompinents("Phobia=" + PhobiaText10.Text);
        }
        private void Job10_CheckedChanged(object sender, EventArgs e)
        {
            Job10.Enabled = false;
            SendDataCompinents("Job=" + JobText10.Text);
        }
        private void Character10_CheckedChanged(object sender, EventArgs e)
        {
            Character10.Enabled = false;
            SendDataCompinents("Character=" + CharacterText10.Text);
        }
        #endregion
        #region 11
        private void Baggage11_CheckedChanged(object sender, EventArgs e)
        {
            Baggage11.Enabled = false;
            SendDataCompinents("Baggage=" + BaggageText11.Text);
        }    
        private void Hobby11_CheckedChanged(object sender, EventArgs e)
        {
            Hobby11.Enabled = false;
            SendDataCompinents("Hobby=" + HobbyText11.Text);
        }
        private void Health11_CheckedChanged(object sender, EventArgs e)
        {
            Health11.Enabled = false;
            SendDataCompinents("Health=" + HealthText11.Text);
        }
        private void Sex11_CheckedChanged(object sender, EventArgs e)
        {
            Sex11.Enabled = false;
            SendDataCompinents("Sex=" + SexText11.Text);
        }
        private void Job11_CheckedChanged(object sender, EventArgs e)
        {
            Job11.Enabled = false;
            SendDataCompinents("Job=" + JobText11.Text);
        }
        private void Age11_CheckedChanged(object sender, EventArgs e)
        {
            Age11.Enabled = false;
            SendDataCompinents("Age=" + AgeText11.Text);
        }
        private void Phobia11_CheckedChanged(object sender, EventArgs e)
        {
            Phobia11.Enabled = false;
            SendDataCompinents("Phobia=" + PhobiaText11.Text);
        }
        private void Character11_CheckedChanged(object sender, EventArgs e)
        {
            Character11.Enabled = false;
            SendDataCompinents("Character=" + CharacterText11.Text);
        }
        #endregion
        #region 12
        private void Sex12_CheckedChanged(object sender, EventArgs e)
        {
            Sex12.Enabled = false;
            SendDataCompinents("Sex=" + SexText12.Text);
        }
        private void Age12_CheckedChanged(object sender, EventArgs e)
        {
            Age12.Enabled = false;
            SendDataCompinents("Age=" + AgeText12.Text);
        }
        private void Health12_CheckedChanged(object sender, EventArgs e)
        {
            Health12.Enabled = false;
            SendDataCompinents("Health=" + HealthText12.Text);
        }
        private void Job12_CheckedChanged(object sender, EventArgs e)
        {
            Job12.Enabled = false;
            SendDataCompinents("Job=" + JobText12.Text);
        }
        private void Hobby12_CheckedChanged(object sender, EventArgs e)
        {
            Hobby12.Enabled = false;
            SendDataCompinents("Hobby=" + HobbyText12.Text);
        }

        private void Baggage12_CheckedChanged(object sender, EventArgs e)
        {
            Baggage12.Enabled = false;
            SendDataCompinents("Baggage=" + BaggageText12.Text);
        }

        private void Phobia12_CheckedChanged(object sender, EventArgs e)
        {
            Phobia12.Enabled = false;
            SendDataCompinents("Phobia=" + PhobiaText12.Text);
        }

        private void Character12_CheckedChanged(object sender, EventArgs e)
        {
            Character12.Enabled = false;
            SendDataCompinents("Character=" + CharacterText12.Text);
        }




        #endregion

        #endregion SendData

        private void Next_Move_button_Click(object sender, EventArgs e)
        {

            Next_Move_button.Visible = false;
            string Message = "NEXT_____MOVE" + " ID_ROOM=" + CodeRoom.Text + " ID_CLIENT=" + id_client;
            int Messagesize = Encoding.UTF8.GetByteCount(Message);
            byte[] outStream = new byte[Messagesize];
            outStream = Encoding.UTF8.GetBytes(Message);
            serverStream.Write(outStream, 0, outStream.Length);


        }

        #region vote_button_Click
        private void vote_button1_Click(object sender, EventArgs e)
        {
            Send_vote(player_identifier1.Text);
            
        }

        private void vote_button2_Click(object sender, EventArgs e)
        {
            Send_vote(player_identifier2.Text);
        }

        private void vote_button3_Click(object sender, EventArgs e)
        {
            Send_vote(player_identifier3.Text);
        }

        private void vote_button4_Click(object sender, EventArgs e)
        {
            Send_vote(player_identifier4.Text);
        }

        private void vote_button5_Click(object sender, EventArgs e)
        {
            Send_vote(player_identifier5.Text);
        }

        private void vote_button6_Click(object sender, EventArgs e)
        {
            Send_vote(player_identifier6.Text);
        }

        private void vote_button7_Click(object sender, EventArgs e)
        {
            Send_vote(player_identifier7.Text);
        }

        private void vote_button8_Click(object sender, EventArgs e)
        {
            Send_vote(player_identifier8.Text);
        }

        private void vote_button9_Click(object sender, EventArgs e)
        {
            Send_vote(player_identifier9.Text);
        }

        private void vote_button10_Click(object sender, EventArgs e)
        {
            Send_vote(player_identifier10.Text);
        }

        private void vote_button11_Click(object sender, EventArgs e)
        {
            Send_vote(player_identifier11.Text);
        }

        private void vote_button12_Click(object sender, EventArgs e)
        {
            Send_vote(player_identifier12.Text);
        }
        #endregion
        private void Send_vote(string name)
        {
            vote_button1.Visible = false;
            vote_button2.Visible = false;
            vote_button3.Visible = false;
            vote_button4.Visible = false;
            vote_button5.Visible = false;
            vote_button6.Visible = false;
            vote_button7.Visible = false;
            vote_button8.Visible = false;
            vote_button9.Visible = false;
            vote_button10.Visible = false;
            vote_button11.Visible = false;
            vote_button12.Visible = false;
            //VOTING___KICK->ID_ROOM = VOTE =
            string Message = "VOTING___KICK " + "ID_ROOM=" + CodeRoom.Text +  " VOTE=" + name;

            int Messagesize = Encoding.UTF8.GetByteCount(Message);

            byte[] outStream = new byte[Messagesize];
            outStream = Encoding.UTF8.GetBytes(Message);
            serverStream.Write(outStream, 0, outStream.Length);

        }
       


        


    }


}
