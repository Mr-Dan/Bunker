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
    public partial class Game_menu : Form
    {

        TcpClient clientSocket = new TcpClient();
        NetworkStream serverStream = null;
        info_Characteristic[] info = new info_Characteristic[12];
        public string id_client;
        public string id_name;
        public string login_client;
        public string password_client;
        public string ip;
        public string port;
        public string status_game;
        public string permission;
        static string readData = null;
        static int all_playing = 0;

        int first_walker = 1;
        bool kick = false;
        public Cards[] cards;
        public int vote_click = 0;
        public Game_menu()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
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

            My_Name.Text = name;
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
               // Chat_Box.Text = Chat_Box.Text+readData+"\n";
                if (readData.Length<13)
                {
                    //Chat_Box.Text = Chat_Box.Text + "No command" + "\n";
                }
                else if (readData.IndexOf("CONNECT__ROOM",0,13) > -1)
                {
                    //StartGames.Enabled = true;
                   // textBox1.Text = textBox1.Text + permission;
                    My_Name.Text = id_client + " " + id_name;

                   
                }
                else if (readData.IndexOf("NEXT_____MOVE", 0, 13) > -1)
                {
                    string move = readData.Substring(13, readData.Length - 13);
                   // textBox1.Text = textBox1.Text + readData;
                    next_move(move);
                  
                 
                }
                else if (readData.IndexOf("ONLINE___ROOM", 0, 13) > -1)
                {

                    if (permission.IndexOf("admin") > -1)
                    {
                        StartGames.Visible = true;
                    }
                    send_button.Visible = true;

                    int first = 0;
                    int next = 0;
                    string[] Get_Info = new string[12];
                    int count = 0;
                    for (int i = 0; i < 12; i++)
                    {
                        first = readData.IndexOf("(", next);
                        if (first == -1) break;
                        next = readData.IndexOf(")", first);
                        Get_Info[i] = readData.Substring(first + 1, next - first - 1);
                        first = first + 1;
                        next = next + 1;
                        count++;
                    }
                    online_p.Text = count.ToString();

                    if (Int32.Parse(online_p.Text) >= 4) StartGames.Enabled = true;
                    // textBox2.Text = readData;
                }
                else if (readData.IndexOf("ALL_INFO_GAME", 0, 13) > -1)
                {
                   
                    Set_info_players(readData);
                    //textBox1.Text = textBox1.Text + readData;
                }
                else if (readData.IndexOf("OPEN_CHARACTE", 0, 13) > -1)
                {
                    
                   // textBox1.Text = textBox1.Text + readData;
                    string characteristic = readData.Substring(13,readData.Length - 13);
                    open_characteristic(characteristic);
                }
                else if (readData.IndexOf("VOTING___KICK", 0, 13) > -1)
                {
                    
                    string move = readData.Substring(13, readData.Length - 13);
                    move = move.Trim();
                    string full_name = "{" + CodeRoom.Text + "}" + "{" + id_client + "}" + "{" + id_name + "}";
                    if (move.IndexOf(full_name) > -1) kick = true;

                    if (player_identifier1.Text.IndexOf(move) > -1) listBox1.BackColor = Color.FromArgb(112, 0, 0);
                    if (player_identifier2.Text.IndexOf(move) > -1) listBox2.BackColor = Color.FromArgb(112, 0, 0);
                    if (player_identifier3.Text.IndexOf(move) > -1) listBox3.BackColor = Color.FromArgb(112, 0, 0);
                    if (player_identifier4.Text.IndexOf(move) > -1) listBox4.BackColor = Color.FromArgb(112, 0, 0);
                    if (player_identifier5.Text.IndexOf(move) > -1) listBox5.BackColor = Color.FromArgb(112, 0, 0);
                    if (player_identifier6.Text.IndexOf(move) > -1) listBox6.BackColor = Color.FromArgb(112, 0, 0);
                    if (player_identifier7.Text.IndexOf(move) > -1) listBox7.BackColor = Color.FromArgb(112, 0, 0);
                    if (player_identifier8.Text.IndexOf(move) > -1) listBox8.BackColor = Color.FromArgb(112, 0, 0);
                    if (player_identifier9.Text.IndexOf(move) > -1) listBox9.BackColor = Color.FromArgb(112, 0, 0);
                    if (player_identifier10.Text.IndexOf(move) > -1) listBox10.BackColor = Color.FromArgb(112, 0, 0);
                    if (player_identifier11.Text.IndexOf(move) > -1) listBox11.BackColor = Color.FromArgb(112, 0, 0);
                    if (player_identifier12.Text.IndexOf(move) > -1) listBox12.BackColor = Color.FromArgb(112, 0, 0);



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
                else if (readData.IndexOf("END__THE_GAME", 0, 13) > -1)
                {
                   // textBox1.Text = textBox1.Text + readData;
                    string move = readData.Substring(13, readData.Length - 13);
                   
                    move = move.Trim(' ');
                   // textBox2.Text ="new "+ move;
                  
                    if (player_identifier1.Text.IndexOf(move) > -1)
                    {
                        listBox1.BackColor = Color.FromArgb(112, 0, 0);
                      
                    }
                    if (player_identifier2.Text.IndexOf(move) > -1)
                    {
                        listBox2.BackColor = Color.FromArgb(112, 0, 0);

                       
                    }
                    if (player_identifier3.Text.IndexOf(move) > -1)
                    {
                        listBox3.BackColor = Color.FromArgb(112, 0, 0);

                      
                    }
                    if (player_identifier4.Text.IndexOf(move) > -1)
                    {
                        listBox4.BackColor = Color.FromArgb(112, 0, 0);

                        
                    }
                    if (player_identifier5.Text.IndexOf(move) > -1)
                    {
                        listBox5.BackColor = Color.FromArgb(112, 0, 0);

                       
                    }
                    if (player_identifier6.Text.IndexOf(move) > -1)
                    {
                        listBox6.BackColor = Color.FromArgb(112, 0, 0);

                        
                    }
                    if (player_identifier7.Text.IndexOf(move) > -1)
                    {
                        listBox7.BackColor = Color.FromArgb(112, 0, 0);

                       
                    }
                    if (player_identifier8.Text.IndexOf(move) > -1)
                    {
                        listBox8.BackColor = Color.FromArgb(112, 0, 0);

                        
                    }
                    if (player_identifier9.Text.IndexOf(move) > -1)
                    {
                        listBox9.BackColor = Color.FromArgb(112, 0, 0);

                        
                    }
                    if (player_identifier10.Text.IndexOf(move) > -1)
                    {
                        listBox10.BackColor = Color.FromArgb(112, 0, 0);

                        
                    }
                    if (player_identifier11.Text.IndexOf(move) > -1)
                    {
                        listBox11.BackColor = Color.FromArgb(112, 0, 0);

                        
                    }
                    if (player_identifier12.Text.IndexOf(move) > -1)
                    {
                        listBox12.BackColor = Color.FromArgb(112, 0, 0);


                    }

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
                    Next_Move_button.Visible = false;
                    skip_button.Visible = false;
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

                    card_1_button.Visible = false;
                    card_2_button.Visible = false;
                    // MessageBox.Show("Конец игры ", "Ок", MessageBoxButtons.OK);

                }
                else if (readData.IndexOf("ONE_MORE_KICK", 0, 13) > -1)
                {
                    Next_Move_button.Visible = false;
                
                    #region SET_vote_button
                    if (Int32.Parse(online_p.Text) == 1)
                    {
                        if (listBox1.BackColor != Color.FromArgb(112, 0, 0)) vote_button1.Visible = true;

                    }
                    else if (Int32.Parse(online_p.Text) == 2)
                    {
                        if (listBox1.BackColor != Color.FromArgb(112, 0, 0)) vote_button1.Visible = true;
                        if (listBox2.BackColor != Color.FromArgb(112, 0, 0)) vote_button2.Visible = true;

                    }
                    else if (Int32.Parse(online_p.Text) == 3)
                    {
                        if (listBox1.BackColor != Color.FromArgb(112, 0, 0)) vote_button1.Visible = true;
                        if (listBox2.BackColor != Color.FromArgb(112, 0, 0)) vote_button2.Visible = true;
                        if (listBox3.BackColor != Color.FromArgb(112, 0, 0)) vote_button3.Visible = true;

                    }
                    else if (Int32.Parse(online_p.Text) == 4)
                    {
                        if (listBox1.BackColor != Color.FromArgb(112, 0, 0)) vote_button1.Visible = true;
                        if (listBox2.BackColor != Color.FromArgb(112, 0, 0)) vote_button2.Visible = true;
                        if (listBox3.BackColor != Color.FromArgb(112, 0, 0)) vote_button3.Visible = true;
                        if (listBox4.BackColor != Color.FromArgb(112, 0, 0)) vote_button4.Visible = true;
                    }
                    else if (Int32.Parse(online_p.Text) == 5)
                    {
                        if (listBox1.BackColor != Color.FromArgb(112, 0, 0)) vote_button1.Visible = true;
                        if (listBox2.BackColor != Color.FromArgb(112, 0, 0)) vote_button2.Visible = true;
                        if (listBox3.BackColor != Color.FromArgb(112, 0, 0)) vote_button3.Visible = true;
                        if (listBox4.BackColor != Color.FromArgb(112, 0, 0)) vote_button4.Visible = true;
                        if (listBox5.BackColor != Color.FromArgb(112, 0, 0)) vote_button5.Visible = true;

                    }
                    else if (Int32.Parse(online_p.Text) == 6)
                    {
                        if (listBox1.BackColor != Color.FromArgb(112, 0, 0)) vote_button1.Visible = true;
                        if (listBox2.BackColor != Color.FromArgb(112, 0, 0)) vote_button2.Visible = true;
                        if (listBox3.BackColor != Color.FromArgb(112, 0, 0)) vote_button3.Visible = true;
                        if (listBox4.BackColor != Color.FromArgb(112, 0, 0)) vote_button4.Visible = true;
                        if (listBox5.BackColor != Color.FromArgb(112, 0, 0)) vote_button5.Visible = true;
                        if (listBox6.BackColor != Color.FromArgb(112, 0, 0)) vote_button6.Visible = true;

                    }
                    else if (Int32.Parse(online_p.Text) == 7)
                    {
                        if (listBox1.BackColor != Color.FromArgb(112, 0, 0)) vote_button1.Visible = true;
                        if (listBox2.BackColor != Color.FromArgb(112, 0, 0)) vote_button2.Visible = true;
                        if (listBox3.BackColor != Color.FromArgb(112, 0, 0)) vote_button3.Visible = true;
                        if (listBox4.BackColor != Color.FromArgb(112, 0, 0)) vote_button4.Visible = true;
                        if (listBox5.BackColor != Color.FromArgb(112, 0, 0)) vote_button5.Visible = true;
                        if (listBox6.BackColor != Color.FromArgb(112, 0, 0)) vote_button6.Visible = true;
                        if (listBox7.BackColor != Color.FromArgb(112, 0, 0)) vote_button7.Visible = true;

                    }
                    else if (Int32.Parse(online_p.Text) == 8)
                    {
                        if (listBox1.BackColor != Color.FromArgb(112, 0, 0)) vote_button1.Visible = true;
                        if (listBox2.BackColor != Color.FromArgb(112, 0, 0)) vote_button2.Visible = true;
                        if (listBox3.BackColor != Color.FromArgb(112, 0, 0)) vote_button3.Visible = true;
                        if (listBox4.BackColor != Color.FromArgb(112, 0, 0)) vote_button4.Visible = true;
                        if (listBox5.BackColor != Color.FromArgb(112, 0, 0)) vote_button5.Visible = true;
                        if (listBox6.BackColor != Color.FromArgb(112, 0, 0)) vote_button6.Visible = true;
                        if (listBox7.BackColor != Color.FromArgb(112, 0, 0)) vote_button7.Visible = true;
                        if (listBox8.BackColor != Color.FromArgb(112, 0, 0)) vote_button8.Visible = true;

                    }
                    else if (Int32.Parse(online_p.Text) == 9)
                    {
                        if (listBox1.BackColor != Color.FromArgb(112, 0, 0)) vote_button1.Visible = true;
                        if (listBox2.BackColor != Color.FromArgb(112, 0, 0)) vote_button2.Visible = true;
                        if (listBox3.BackColor != Color.FromArgb(112, 0, 0)) vote_button3.Visible = true;
                        if (listBox4.BackColor != Color.FromArgb(112, 0, 0)) vote_button4.Visible = true;
                        if (listBox5.BackColor != Color.FromArgb(112, 0, 0)) vote_button5.Visible = true;
                        if (listBox6.BackColor != Color.FromArgb(112, 0, 0)) vote_button6.Visible = true;
                        if (listBox7.BackColor != Color.FromArgb(112, 0, 0)) vote_button7.Visible = true;
                        if (listBox8.BackColor != Color.FromArgb(112, 0, 0)) vote_button8.Visible = true;
                        if (listBox9.BackColor != Color.FromArgb(112, 0, 0)) vote_button9.Visible = true;

                    }
                    else if (Int32.Parse(online_p.Text) == 10)
                    {
                        if (listBox1.BackColor != Color.FromArgb(112, 0, 0)) vote_button1.Visible = true;
                        if (listBox2.BackColor != Color.FromArgb(112, 0, 0)) vote_button2.Visible = true;
                        if (listBox3.BackColor != Color.FromArgb(112, 0, 0)) vote_button3.Visible = true;
                        if (listBox4.BackColor != Color.FromArgb(112, 0, 0)) vote_button4.Visible = true;
                        if (listBox5.BackColor != Color.FromArgb(112, 0, 0)) vote_button5.Visible = true;
                        if (listBox6.BackColor != Color.FromArgb(112, 0, 0)) vote_button6.Visible = true;
                        if (listBox7.BackColor != Color.FromArgb(112, 0, 0)) vote_button7.Visible = true;
                        if (listBox8.BackColor != Color.FromArgb(112, 0, 0)) vote_button8.Visible = true;
                        if (listBox9.BackColor != Color.FromArgb(112, 0, 0)) vote_button9.Visible = true;
                        if (listBox10.BackColor != Color.FromArgb(112, 0, 0)) vote_button10.Visible = true;


                    }
                    else if (Int32.Parse(online_p.Text) == 11)
                    {
                        if (listBox1.BackColor != Color.FromArgb(112, 0, 0)) vote_button1.Visible = true;
                        if (listBox2.BackColor != Color.FromArgb(112, 0, 0)) vote_button2.Visible = true;
                        if (listBox3.BackColor != Color.FromArgb(112, 0, 0)) vote_button3.Visible = true;
                        if (listBox4.BackColor != Color.FromArgb(112, 0, 0)) vote_button4.Visible = true;
                        if (listBox5.BackColor != Color.FromArgb(112, 0, 0)) vote_button5.Visible = true;
                        if (listBox6.BackColor != Color.FromArgb(112, 0, 0)) vote_button6.Visible = true;
                        if (listBox7.BackColor != Color.FromArgb(112, 0, 0)) vote_button7.Visible = true;
                        if (listBox8.BackColor != Color.FromArgb(112, 0, 0)) vote_button8.Visible = true;
                        if (listBox9.BackColor != Color.FromArgb(112, 0, 0)) vote_button9.Visible = true;
                        if (listBox10.BackColor != Color.FromArgb(112, 0, 0)) vote_button10.Visible = true;
                        if (listBox11.BackColor != Color.FromArgb(112, 0, 0)) vote_button11.Visible = true;

                    }
                    else if (Int32.Parse(online_p.Text) == 12)
                    {
                        if (listBox1.BackColor != Color.FromArgb(112, 0, 0)) vote_button1.Visible = true;
                        if (listBox2.BackColor != Color.FromArgb(112, 0, 0)) vote_button2.Visible = true;
                        if (listBox3.BackColor != Color.FromArgb(112, 0, 0)) vote_button3.Visible = true;
                        if (listBox4.BackColor != Color.FromArgb(112, 0, 0)) vote_button4.Visible = true;
                        if (listBox5.BackColor != Color.FromArgb(112, 0, 0)) vote_button5.Visible = true;
                        if (listBox6.BackColor != Color.FromArgb(112, 0, 0)) vote_button6.Visible = true;
                        if (listBox7.BackColor != Color.FromArgb(112, 0, 0)) vote_button7.Visible = true;
                        if (listBox8.BackColor != Color.FromArgb(112, 0, 0)) vote_button8.Visible = true;
                        if (listBox9.BackColor != Color.FromArgb(112, 0, 0)) vote_button9.Visible = true;
                        if (listBox10.BackColor != Color.FromArgb(112, 0, 0)) vote_button10.Visible = true;
                        if (listBox11.BackColor != Color.FromArgb(112, 0, 0)) vote_button11.Visible = true;
                        if (listBox12.BackColor != Color.FromArgb(112, 0, 0)) vote_button12.Visible = true;
                    }
                    #endregion
                }
                else if (readData.IndexOf("VOTING_N_KICK", 0, 13) > -1)
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
                    skip_button.Visible = false;
                }
                else if (readData.IndexOf("NEXT_NEW_MOVE", 0, 13) > -1)
                {
                    string move = readData.Substring(13, readData.Length - 13);
                    //textBox1.Text = textBox1.Text + readData;

                    next_move(move);
                    skip_button.Visible = false;
                }
                else if (readData.IndexOf("COUNT____VOTE", 0, 13) > -1)
                {
                    string vote_line = readData.Substring(13, readData.Length - 13);                  
                    set_vote(vote_line);
                }
                else if (readData.IndexOf("CHAT_GET__MSG", 0, 13) > -1)
                {
                    string massange_get = readData.Substring(13, readData.Length - 13);

                    Chat_Box.Text = Chat_Box.Text + Environment.NewLine + massange_get;
                    Chat_Box.SelectionStart = Chat_Box.Text.Length;
                    Chat_Box.ScrollToCaret();
                }
                else if (readData.IndexOf("UPDATE___INFO", 0, 13) > -1)
                {
                    string massange_get = readData.Substring(13, readData.Length - 13);

                    //Chat_Box.Text = readData;

                    update_info(massange_get);
                }
                else if (readData.IndexOf("START__VOTING", 0, 13) > -1)
                {
                    vote_click = 0;
                    Next_Move_button.Visible = false;

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

                    #region SET_vote_button
                    if (Int32.Parse(online_p.Text) == 1)
                    {
                        if (listBox1.BackColor != Color.FromArgb(112, 0, 0)) vote_button1.Visible = true;

                    }
                    else if (Int32.Parse(online_p.Text) == 2)
                    {
                        if (listBox1.BackColor != Color.FromArgb(112, 0, 0)) vote_button1.Visible = true;
                        if (listBox2.BackColor != Color.FromArgb(112, 0, 0)) vote_button2.Visible = true;

                    }
                    else if (Int32.Parse(online_p.Text) == 3)
                    {
                        if (listBox1.BackColor != Color.FromArgb(112, 0, 0)) vote_button1.Visible = true;
                        if (listBox2.BackColor != Color.FromArgb(112, 0, 0)) vote_button2.Visible = true;
                        if (listBox3.BackColor != Color.FromArgb(112, 0, 0)) vote_button3.Visible = true;

                    }
                    else if (Int32.Parse(online_p.Text) == 4)
                    {
                        if (listBox1.BackColor != Color.FromArgb(112, 0, 0)) vote_button1.Visible = true;
                        if (listBox2.BackColor != Color.FromArgb(112, 0, 0)) vote_button2.Visible = true;
                        if (listBox3.BackColor != Color.FromArgb(112, 0, 0)) vote_button3.Visible = true;
                        if (listBox4.BackColor != Color.FromArgb(112, 0, 0)) vote_button4.Visible = true;
                    }
                    else if (Int32.Parse(online_p.Text) == 5)
                    {
                        if (listBox1.BackColor != Color.FromArgb(112, 0, 0)) vote_button1.Visible = true;
                        if (listBox2.BackColor != Color.FromArgb(112, 0, 0)) vote_button2.Visible = true;
                        if (listBox3.BackColor != Color.FromArgb(112, 0, 0)) vote_button3.Visible = true;
                        if (listBox4.BackColor != Color.FromArgb(112, 0, 0)) vote_button4.Visible = true;
                        if (listBox5.BackColor != Color.FromArgb(112, 0, 0)) vote_button5.Visible = true;

                    }
                    else if (Int32.Parse(online_p.Text) == 6)
                    {
                        if (listBox1.BackColor != Color.FromArgb(112, 0, 0)) vote_button1.Visible = true;
                        if (listBox2.BackColor != Color.FromArgb(112, 0, 0)) vote_button2.Visible = true;
                        if (listBox3.BackColor != Color.FromArgb(112, 0, 0)) vote_button3.Visible = true;
                        if (listBox4.BackColor != Color.FromArgb(112, 0, 0)) vote_button4.Visible = true;
                        if (listBox5.BackColor != Color.FromArgb(112, 0, 0)) vote_button5.Visible = true;
                        if (listBox6.BackColor != Color.FromArgb(112, 0, 0)) vote_button6.Visible = true;

                    }
                    else if (Int32.Parse(online_p.Text) == 7)
                    {
                        if (listBox1.BackColor != Color.FromArgb(112, 0, 0)) vote_button1.Visible = true;
                        if (listBox2.BackColor != Color.FromArgb(112, 0, 0)) vote_button2.Visible = true;
                        if (listBox3.BackColor != Color.FromArgb(112, 0, 0)) vote_button3.Visible = true;
                        if (listBox4.BackColor != Color.FromArgb(112, 0, 0)) vote_button4.Visible = true;
                        if (listBox5.BackColor != Color.FromArgb(112, 0, 0)) vote_button5.Visible = true;
                        if (listBox6.BackColor != Color.FromArgb(112, 0, 0)) vote_button6.Visible = true;
                        if (listBox7.BackColor != Color.FromArgb(112, 0, 0)) vote_button7.Visible = true;

                    }
                    else if (Int32.Parse(online_p.Text) == 8)
                    {
                        if (listBox1.BackColor != Color.FromArgb(112, 0, 0)) vote_button1.Visible = true;
                        if (listBox2.BackColor != Color.FromArgb(112, 0, 0)) vote_button2.Visible = true;
                        if (listBox3.BackColor != Color.FromArgb(112, 0, 0)) vote_button3.Visible = true;
                        if (listBox4.BackColor != Color.FromArgb(112, 0, 0)) vote_button4.Visible = true;
                        if (listBox5.BackColor != Color.FromArgb(112, 0, 0)) vote_button5.Visible = true;
                        if (listBox6.BackColor != Color.FromArgb(112, 0, 0)) vote_button6.Visible = true;
                        if (listBox7.BackColor != Color.FromArgb(112, 0, 0)) vote_button7.Visible = true;
                        if (listBox8.BackColor != Color.FromArgb(112, 0, 0)) vote_button8.Visible = true;

                    }
                    else if (Int32.Parse(online_p.Text) == 9)
                    {
                        if (listBox1.BackColor != Color.FromArgb(112, 0, 0)) vote_button1.Visible = true;
                        if (listBox2.BackColor != Color.FromArgb(112, 0, 0)) vote_button2.Visible = true;
                        if (listBox3.BackColor != Color.FromArgb(112, 0, 0)) vote_button3.Visible = true;
                        if (listBox4.BackColor != Color.FromArgb(112, 0, 0)) vote_button4.Visible = true;
                        if (listBox5.BackColor != Color.FromArgb(112, 0, 0)) vote_button5.Visible = true;
                        if (listBox6.BackColor != Color.FromArgb(112, 0, 0)) vote_button6.Visible = true;
                        if (listBox7.BackColor != Color.FromArgb(112, 0, 0)) vote_button7.Visible = true;
                        if (listBox8.BackColor != Color.FromArgb(112, 0, 0)) vote_button8.Visible = true;
                        if (listBox9.BackColor != Color.FromArgb(112, 0, 0)) vote_button9.Visible = true;

                    }
                    else if (Int32.Parse(online_p.Text) == 10)
                    {
                        if (listBox1.BackColor != Color.FromArgb(112, 0, 0)) vote_button1.Visible = true;
                        if (listBox2.BackColor != Color.FromArgb(112, 0, 0)) vote_button2.Visible = true;
                        if (listBox3.BackColor != Color.FromArgb(112, 0, 0)) vote_button3.Visible = true;
                        if (listBox4.BackColor != Color.FromArgb(112, 0, 0)) vote_button4.Visible = true;
                        if (listBox5.BackColor != Color.FromArgb(112, 0, 0)) vote_button5.Visible = true;
                        if (listBox6.BackColor != Color.FromArgb(112, 0, 0)) vote_button6.Visible = true;
                        if (listBox7.BackColor != Color.FromArgb(112, 0, 0)) vote_button7.Visible = true;
                        if (listBox8.BackColor != Color.FromArgb(112, 0, 0)) vote_button8.Visible = true;
                        if (listBox9.BackColor != Color.FromArgb(112, 0, 0)) vote_button9.Visible = true;
                        if (listBox10.BackColor != Color.FromArgb(112, 0, 0)) vote_button10.Visible = true;


                    }
                    else if (Int32.Parse(online_p.Text) == 11)
                    {
                        if (listBox1.BackColor != Color.FromArgb(112, 0, 0)) vote_button1.Visible = true;
                        if (listBox2.BackColor != Color.FromArgb(112, 0, 0)) vote_button2.Visible = true;
                        if (listBox3.BackColor != Color.FromArgb(112, 0, 0)) vote_button3.Visible = true;
                        if (listBox4.BackColor != Color.FromArgb(112, 0, 0)) vote_button4.Visible = true;
                        if (listBox5.BackColor != Color.FromArgb(112, 0, 0)) vote_button5.Visible = true;
                        if (listBox6.BackColor != Color.FromArgb(112, 0, 0)) vote_button6.Visible = true;
                        if (listBox7.BackColor != Color.FromArgb(112, 0, 0)) vote_button7.Visible = true;
                        if (listBox8.BackColor != Color.FromArgb(112, 0, 0)) vote_button8.Visible = true;
                        if (listBox9.BackColor != Color.FromArgb(112, 0, 0)) vote_button9.Visible = true;
                        if (listBox10.BackColor != Color.FromArgb(112, 0, 0)) vote_button10.Visible = true;
                        if (listBox11.BackColor != Color.FromArgb(112, 0, 0)) vote_button11.Visible = true;

                    }
                    else if (Int32.Parse(online_p.Text) == 12)
                    {
                        if (listBox1.BackColor != Color.FromArgb(112, 0, 0)) vote_button1.Visible = true;
                        if (listBox2.BackColor != Color.FromArgb(112, 0, 0)) vote_button2.Visible = true;
                        if (listBox3.BackColor != Color.FromArgb(112, 0, 0)) vote_button3.Visible = true;
                        if (listBox4.BackColor != Color.FromArgb(112, 0, 0)) vote_button4.Visible = true;
                        if (listBox5.BackColor != Color.FromArgb(112, 0, 0)) vote_button5.Visible = true;
                        if (listBox6.BackColor != Color.FromArgb(112, 0, 0)) vote_button6.Visible = true;
                        if (listBox7.BackColor != Color.FromArgb(112, 0, 0)) vote_button7.Visible = true;
                        if (listBox8.BackColor != Color.FromArgb(112, 0, 0)) vote_button8.Visible = true;
                        if (listBox9.BackColor != Color.FromArgb(112, 0, 0)) vote_button9.Visible = true;
                        if (listBox10.BackColor != Color.FromArgb(112, 0, 0)) vote_button10.Visible = true;
                        if (listBox11.BackColor != Color.FromArgb(112, 0, 0)) vote_button11.Visible = true;
                        if (listBox12.BackColor != Color.FromArgb(112, 0, 0)) vote_button12.Visible = true;
                    }
                    #endregion
                }

            }
        }
        private void getMessage()
       
        { int  ReceiveBufferSize = 4;

            try
            {
                while (true)             
                {                
                        byte[] inStream = new byte[ReceiveBufferSize];
                        serverStream = clientSocket.GetStream();
                        serverStream.Read(inStream, 0, 4);
                        string returndata = Encoding.UTF8.GetString(inStream);

                    int size;
                    bool success = Int32.TryParse(returndata, out size);
                    if (success)
                    {
                        string massange = "";
                        while (Encoding.UTF8.GetByteCount(massange) != size)
                        {
                           
                            byte[] massange_byte = new byte[size];
                            serverStream.Read(massange_byte, 0, size);
                            massange = massange + Encoding.UTF8.GetString(massange_byte);
                            massange = massange.Trim('\0');
                            if (Encoding.UTF8.GetByteCount(massange) == size) break;
                        }
                        readData = "" + massange;
                        msg(); 
                    }
                                  
                }
            }
            catch(Exception ex)
            {
                //MessageBox.Show(ex.ToString(), "Ошибка", MessageBoxButtons.OK);
                clientSocket.Close();
                serverStream.Close();

            }
        }
        private void send_Messege(string message, bool flag)
        {

            if (flag == true)
            {
                int Messagesize = Encoding.UTF8.GetByteCount(message);

                string size_str = "0";
                if (Messagesize.ToString().Length == 1) size_str = "000" + Messagesize;
                if (Messagesize.ToString().Length == 2) size_str = "00" + Messagesize;
                if (Messagesize.ToString().Length == 3) size_str = "0" + Messagesize;
                if (Messagesize.ToString().Length == 4) size_str = Messagesize.ToString();

                byte[] outStream = new byte[4];
                outStream = Encoding.UTF8.GetBytes(size_str);
                serverStream.Write(outStream, 0, outStream.Length);

                outStream = new byte[Messagesize];
                outStream = Encoding.UTF8.GetBytes(message);
                serverStream.Write(outStream, 0, outStream.Length);

                Thread ctThread = new Thread(getMessage);
                ctThread.Start();
            }
            else
            {
                int Messagesize = Encoding.UTF8.GetByteCount(message);

                string size_str = "0";
                if (Messagesize.ToString().Length == 1) size_str = "000" + Messagesize;
                if (Messagesize.ToString().Length == 2) size_str = "00" + Messagesize;
                if (Messagesize.ToString().Length == 3) size_str = "0" + Messagesize;
                if (Messagesize.ToString().Length == 4) size_str = Messagesize.ToString();

                byte[] outStream = new byte[4];
                outStream = Encoding.UTF8.GetBytes(size_str);
                serverStream.Write(outStream, 0, outStream.Length);

                outStream = new byte[Messagesize];
                outStream = Encoding.UTF8.GetBytes(message);
                serverStream.Write(outStream, 0, outStream.Length);
            }

        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {

            if (serverStream != null && clientSocket.Connected == true)
            {
               
                send_Messege("DISCONN__ROOM вышел из игры", false);

            }

            if (clientSocket.Connected != false) clientSocket.Close();
            if (serverStream != null) serverStream.Close();
            Main_menu F3 = new Main_menu();
            F3.menu_auth(login_client,password_client,true);
            F3.Show();
        }     
        private void Start_Game_Click(object sender, EventArgs e)
        {
            
                string massage = "START____ROOM " + "{" + CodeRoom.Text + "}{" + id_client + "}{" + permission + "}";             
                send_Messege(massage, false);
                StartGames.Enabled = false;
                StartGames.Visible = false;
          
         }
        private void Form1_Load(object sender, EventArgs e)
        {
            connect_timer.Start();
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
            skip_button.Visible = false;

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

            send_button.Visible = false;

            
        }

        private void Set_info_players(string read_data)
        { 
           
            int first_pos = readData.IndexOf("{", 0);
            int next_pos = readData.IndexOf("}", first_pos);
            string players = read_data.Substring(first_pos + 1, next_pos - first_pos-1);
            online_p.Text = players;
            int players_for_game = Int32.Parse(players);
            all_playing = players_for_game;

            string[] all_characteristic  = new string[players_for_game + 1];

            int first_pos_characteristic = 0;
            int next_pos_characteristic_ = 0;

            for (int i = 0; i < players_for_game + 1; i++)
            {
                first_pos_characteristic = readData.IndexOf("(", next_pos_characteristic_);
                next_pos_characteristic_ = readData.IndexOf(")", first_pos_characteristic);
                all_characteristic[i] = readData.Substring(first_pos_characteristic + 1, next_pos_characteristic_ - first_pos_characteristic - 1);
                first_pos_characteristic = first_pos_characteristic + 1;
                next_pos_characteristic_ = next_pos_characteristic_ + 1;
            }
            Set_info_location(all_characteristic[players_for_game]);

           
            for (int i = 0; i < players_for_game ; i++)
            {
                info[i] = new info_Characteristic(all_characteristic[i]);
            }

            if (players_for_game >= 1) if (info[0].Id == id_client )
            {
                if (info[0].Move == "yes")
                {
                    PersonComponents1ON();
                    Next_Move_button.Visible = true;
                }
                AgeText1.Text = info[0].Age;
                SexText1.Text = info[0].Sex;
                JobText1.Text = info[0].Job;
                HobbyText1.Text = info[0].Hobby;
                HealthText1.Text = info[0].Health;
                BaggageText1.Text = info[0].Baggage;
                PhobiaText1.Text = info[0].Phobia;
                CharacterText1.Text = info[0].Character;
                card_1_label.Text = info[0].Action_cards_one;
                card_2_label.Text = info[0].Action_cards_two;
            }
            if (players_for_game >= 2) if (info[1].Id == id_client )
            {
                if (info[1].Move == "yes")
                {
                    PersonComponents2ON();
                    Next_Move_button.Visible = true;
                }
                AgeText2.Text = info[1].Age;
                SexText2.Text = info[1].Sex;
                JobText2.Text = info[1].Job;
                HobbyText2.Text = info[1].Hobby;
                HealthText2.Text = info[1].Health;
                BaggageText2.Text = info[1].Baggage;
                PhobiaText2.Text = info[1].Phobia;
                CharacterText2.Text = info[1].Character;
                card_1_label.Text = info[1].Action_cards_one;
                card_2_label.Text = info[1].Action_cards_two;
            }
            if (players_for_game >= 3) if (info[2].Id == id_client )
            {
                if (info[2].Move == "yes")
                {
                    PersonComponents3ON();
                    Next_Move_button.Visible = true;
                }
                AgeText3.Text = info[2].Age;
                SexText3.Text = info[2].Sex;
                JobText3.Text = info[2].Job;
                HobbyText3.Text = info[2].Hobby;
                HealthText3.Text = info[2].Health;
                BaggageText3.Text = info[2].Baggage;
                PhobiaText3.Text = info[2].Phobia;
                CharacterText3.Text = info[2].Character;
                card_1_label.Text = info[2].Action_cards_one;
                card_2_label.Text = info[2].Action_cards_two;
            }
            if (players_for_game >= 4) if (info[3].Id == id_client )
            {
                if (info[3].Move == "yes")
                {
                    PersonComponents4ON();
                    Next_Move_button.Visible = true;
                }
                AgeText4.Text = info[3].Age;
                SexText4.Text = info[3].Sex;
                JobText4.Text = info[3].Job;
                HobbyText4.Text = info[3].Hobby;
                HealthText4.Text = info[3].Health;
                BaggageText4.Text = info[3].Baggage;
                PhobiaText4.Text = info[3].Phobia;
                CharacterText4.Text = info[3].Character;
                card_1_label.Text = info[3].Action_cards_one;
                card_2_label.Text = info[3].Action_cards_two;
            }
            if (players_for_game >= 5) if (info[4].Id == id_client )
            {
                if (info[4].Move == "yes")
                {
                    PersonComponents5ON();
                    Next_Move_button.Visible = true;
                }
                AgeText5.Text = info[4].Age;
                SexText5.Text = info[4].Sex;
                JobText5.Text = info[4].Job;
                HobbyText5.Text = info[4].Hobby;
                HealthText5.Text = info[4].Health;
                BaggageText5.Text = info[4].Baggage;
                PhobiaText5.Text = info[4].Phobia;
                CharacterText5.Text = info[4].Character;
                card_1_label.Text = info[4].Action_cards_one;
                card_2_label.Text = info[4].Action_cards_two;
            }
            if (players_for_game >= 6) if (info[5].Id == id_client )
            {
                if (info[5].Move == "yes")
                {
                    PersonComponents6ON();
                    Next_Move_button.Visible = true;
                }
                AgeText6.Text = info[5].Age;
                SexText6.Text = info[5].Sex;
                JobText6.Text = info[5].Job;
                HobbyText6.Text = info[5].Hobby;
                HealthText6.Text = info[5].Health;
                BaggageText6.Text = info[5].Baggage;
                PhobiaText6.Text = info[5].Phobia;
                CharacterText6.Text = info[5].Character;
                card_1_label.Text = info[5].Action_cards_one;
                card_2_label.Text = info[5].Action_cards_two;
            }
            if (players_for_game >= 7) if (info[6].Id == id_client )
            {
                if (info[6].Move == "yes")
                {
                    PersonComponents7ON();
                    Next_Move_button.Visible = true;
                }
                AgeText7.Text = info[6].Age;
                SexText7.Text = info[6].Sex;
                JobText7.Text = info[6].Job;
                HobbyText7.Text = info[6].Hobby;
                HealthText7.Text = info[6].Health;
                BaggageText7.Text = info[6].Baggage;
                PhobiaText7.Text = info[6].Phobia;
                CharacterText7.Text = info[6].Character;
                card_1_label.Text = info[6].Action_cards_one;
                card_2_label.Text = info[6].Action_cards_two;
            }
            if (players_for_game >= 8) if (info[7].Id == id_client )
            {
                if (info[7].Move == "yes")
                {
                    PersonComponents8ON();
                    Next_Move_button.Visible = true;
                }
                AgeText8.Text = info[7].Age;
                SexText8.Text = info[7].Sex;
                JobText8.Text = info[7].Job;
                HobbyText8.Text = info[7].Hobby;
                HealthText8.Text = info[7].Health;
                BaggageText8.Text = info[7].Baggage;
                PhobiaText8.Text = info[7].Phobia;
                CharacterText8.Text = info[7].Character;
                card_1_label.Text = info[7].Action_cards_one;
                card_2_label.Text = info[7].Action_cards_two;
            }
            if (players_for_game >= 9) if (info[8].Id == id_client )
            {
                if (info[8].Move == "yes")
                {
                    PersonComponents9ON();
                    Next_Move_button.Visible = true;
                }
                AgeText9.Text = info[8].Age;
                SexText9.Text = info[8].Sex;
                JobText9.Text = info[8].Job;
                HobbyText9.Text = info[8].Hobby;
                HealthText9.Text = info[8].Health;
                BaggageText9.Text = info[8].Baggage;
                PhobiaText9.Text = info[8].Phobia;
                CharacterText9.Text = info[8].Character;
                card_1_label.Text = info[8].Action_cards_one;
                card_2_label.Text = info[8].Action_cards_two;
            }
            if (players_for_game >= 10) if (info[9].Id == id_client )
            {
                if (info[9].Move == "yes")
                {
                    PersonComponents10ON();
                    Next_Move_button.Visible = true;
                }
                AgeText10.Text = info[9].Age;
                SexText10.Text = info[9].Sex;
                JobText10.Text = info[9].Job;
                HobbyText10.Text = info[9].Hobby;
                HealthText10.Text = info[9].Health;
                BaggageText10.Text = info[9].Baggage;
                PhobiaText10.Text = info[9].Phobia;
                CharacterText10.Text = info[9].Character;
                card_1_label.Text = info[9].Action_cards_one;
                card_2_label.Text = info[9].Action_cards_two;
            }
            if (players_for_game >= 11) if (info[10].Id == id_client )
            {
                if (info[10].Move == "yes")
                {
                    PersonComponents11ON();
                    Next_Move_button.Visible = true;
                }
                AgeText11.Text = info[10].Age;
                SexText11.Text = info[10].Sex;
                JobText11.Text = info[10].Job;
                HobbyText11.Text = info[10].Hobby;
                HealthText11.Text = info[10].Health;
                BaggageText11.Text = info[10].Baggage;
                PhobiaText11.Text = info[10].Phobia;
                CharacterText11.Text = info[10].Character;
                card_1_label.Text = info[10].Action_cards_one;
                card_2_label.Text = info[10].Action_cards_two;
            }
            if (players_for_game >= 12) if (info[11].Id == id_client )
            {
                if (info[11].Move == "yes")
                {
                    PersonComponents12ON();
                    Next_Move_button.Visible = true;
                }
                AgeText12.Text = info[11].Age;
                SexText12.Text = info[11].Sex;
                JobText12.Text = info[11].Job;
                HobbyText12.Text = info[11].Hobby;
                HealthText12.Text = info[11].Health;
                BaggageText12.Text = info[11].Baggage;
                PhobiaText12.Text = info[11].Phobia;
                CharacterText12.Text = info[11].Character;
                card_1_label.Text = info[11].Action_cards_one;
                card_2_label.Text = info[11].Action_cards_two;
            }


            if (players_for_game >= 1) if (info[0].Id_game == 1 )
            {
                player_identifier1.Text = "{" + info[0].room + "}{" + info[0].Id + "}{" + info[0].Person_name + "}";
                name1.Text = info[0].Person_name;
            }
            if (players_for_game >= 2) if (info[1].Id_game == 2 )
            {
                player_identifier2.Text = "{" + info[1].room + "}{" + info[1].Id + "}{" + info[1].Person_name + "}";
                name2.Text = info[1].Person_name;
            }
            if (players_for_game >= 3) if (info[2].Id_game == 3 )
            {
                player_identifier3.Text = "{" + info[2].room + "}{" + info[2].Id + "}{" + info[2].Person_name + "}";
                name3.Text = info[2].Person_name;
            }
            if (players_for_game >= 4) if (info[3].Id_game == 4 )
            {
                player_identifier4.Text = "{" + info[3].room + "}{" + info[3].Id + "}{" + info[3].Person_name + "}";
                name4.Text = info[3].Person_name;
            }
            if (players_for_game >= 5) if (info[4].Id_game == 5 )
            {
                player_identifier5.Text = "{" + info[4].room + "}{" + info[4].Id + "}{" + info[4].Person_name + "}";
                name5.Text = info[4].Person_name;
            }
            if (players_for_game >= 6) if (info[5].Id_game == 6 )
            {
                player_identifier6.Text = "{" + info[5].room + "}{" + info[5].Id + "}{" + info[5].Person_name + "}";
                name6.Text = info[5].Person_name;
            }
            if (players_for_game >= 7) if (info[6].Id_game == 7 )
            {
                player_identifier7.Text = "{" + info[6].room + "}{" + info[6].Id + "}{" + info[6].Person_name + "}";
                name7.Text = info[6].Person_name;
            }
            if (players_for_game >= 8) if (info[7].Id_game == 8 )
            {
                player_identifier8.Text = "{" + info[7].room + "}{" + info[7].Id + "}{" + info[7].Person_name + "}";
                name8.Text = info[7].Person_name;
            }
            if (players_for_game >= 9) if (info[8].Id_game == 9 )
            {
                player_identifier9.Text = "{" + info[8].room + "}{" + info[8].Id + "}{" + info[8].Person_name + "}";
                name9.Text = info[8].Person_name;
            }
            if (players_for_game >= 10) if (info[9].Id_game == 10 )
            {
                player_identifier10.Text = "{" + info[9].room + "}{" + info[9].Id + "}{" + info[9].Person_name + "}";
                name10.Text = info[9].Person_name;
            }
            if (players_for_game >= 11) if (info[10].Id_game == 11 )
            {
                player_identifier11.Text = "{" + info[10].room + "}{" + info[10].Id + "}{" + info[10].Person_name + "}";
                name11.Text = info[10].Person_name;
            }
            if (players_for_game >= 12) if (info[11].Id_game == 12 )
            {
                player_identifier12.Text = "{" + info[11].room + "}{" + info[11].Id + "}{" + info[11].Person_name + "}";
                name12.Text = info[11].Person_name;
            }


            listBox1.BackColor = Color.Green;
            create_cards(card_1_label.Text, card_2_label.Text);
        }
        private void Set_info_location(string read_data)
        {
            //Location={Вторжение инопланетян}{Гидропоника}{Процент живых людей90}
            string[] info = new string[2];

            int first = 0;
            int next = 0;
            for (int j = 0; j < 2; j++)
            {
                first = read_data.IndexOf("{", next);
                next = read_data.IndexOf("}", first);
                info[j] = read_data.Substring(first + 1, next - first - 1);
                first = first + 1;
                next = next + 1;

            }

            int size = 4 +Int32.Parse( info[0]) + Int32.Parse(info[1]);
            string[] location = new string[size+1];
            first = 0;
            next = 0;
            for (int j = 0; j < size; j++)
            {
                first = read_data.IndexOf("{", next);
                next = read_data.IndexOf("}", first);
                location[j] = read_data.Substring(first + 1, next - first - 1);
                first = first + 1;
                next = next + 1;

            }

            for(int i=0;i< Int32.Parse(info[0]);i++ )
            {
                string room = "img/"+ location[i + 2]+ ".png";
                if (Bunker_room_1.BackgroundImage == null && i == 0){
                    Bunker_room_1.Visible = true;
                    Bunker_room_1_text.Visible = true;
                    Bunker_room_1.BackgroundImage = Image.FromFile(room);
                    Bunker_room_1_text.Text = location[i + 2];
                }
                if (Bunker_room_2.BackgroundImage == null && i == 1){
                    Bunker_room_2.Visible = true;
                    Bunker_room_2_text.Visible = true;
                    Bunker_room_2.BackgroundImage = Image.FromFile(room);
                    Bunker_room_2_text.Text = location[i + 2];
                }
                if (Bunker_room_3.BackgroundImage == null && i == 2){
                    Bunker_room_3.Visible = true;
                    Bunker_room_3_text.Visible = true;
                    Bunker_room_3.BackgroundImage = Image.FromFile(room);
                    Bunker_room_3_text.Text = location[i + 2];
                }
                if (Bunker_room_4.BackgroundImage == null && i == 3){
                    Bunker_room_4.Visible = true;
                    Bunker_room_4_text.Visible = true;
                    Bunker_room_4.BackgroundImage = Image.FromFile(room);
                    Bunker_room_4_text.Text = location[i + 2];
                }
                if (Bunker_room_5.BackgroundImage == null && i == 4){
                    Bunker_room_5.Visible = true;
                    Bunker_room_5_text.Visible = true;
                    Bunker_room_5.BackgroundImage = Image.FromFile(room);
                    Bunker_room_5_text.Text = location[i + 2];
                }

            }
            for (int i = 0; i < Int32.Parse(info[1]); i++)
            {
                string room = "img/" + location[i + 2 + Int32.Parse(info[0])] + ".png";
                if (Bunker_Thing_1.BackgroundImage == null && i == 0)
                {
                    Bunker_Thing_1.Visible = true;
                    Bunker_Thing_1_text.Visible = true;
                    Bunker_Thing_1.BackgroundImage = Image.FromFile(room);
                    Bunker_Thing_1_text.Text = location[i + Int32.Parse(info[0]) + 2];
                }
                if (Bunker_Thing_2.BackgroundImage == null && i == 1)
                {
                    Bunker_Thing_2.Visible = true;
                    Bunker_Thing_2_text.Visible = true;
                    Bunker_Thing_2.BackgroundImage = Image.FromFile(room);
                    Bunker_Thing_2_text.Text = location[i + Int32.Parse(info[0]) + 2];
                }
                if (Bunker_Thing_3.BackgroundImage == null && i == 2)
                {
                    Bunker_Thing_3.Visible = true;
                    Bunker_Thing_3_text.Visible = true;
                    Bunker_Thing_3.BackgroundImage = Image.FromFile(room);
                    Bunker_Thing_3_text.Text = location[i + Int32.Parse(info[0]) + 2];
                }
                if (Bunker_Thing_4.BackgroundImage == null && i == 3)
                {
                    Bunker_Thing_4.Visible = true;
                    Bunker_Thing_4_text.Visible = true;
                    Bunker_Thing_4.BackgroundImage = Image.FromFile(room);
                    Bunker_Thing_4_text.Text = location[i + Int32.Parse(info[0]) + 2];
                }
                

            }
            location_img.Visible = true;
            location_img_text.Visible = true;
            People_Live_Text.Visible = true;
            location_img.BackgroundImage = Image.FromFile("img/" + location[location.Length - 3] + ".png");
            location_img_text.Text = location[location.Length - 3];
            People_Live_Text.Text = location[location.Length - 2]+"%";

            //  "столовая", "местерская", "Гидропоника", "Склад оружия", "Мед Блок", "кухня", 
            //   "водоочистительная станция", "бильярдная", "бассейн", "комната отдыха", "массажный кабинет" };

            Console.WriteLine();
        }
        private void open_characteristic(string characteristic_buff)
        {
            //OPEN_CHARACTE ID_ROOM=wmb ID_CLIENT=1 dan CHARATER=Age=58

            string[] characteristic_data_array = new string[4];
     
            int first = 0;
            int next = 0;
            for (int j = 0; j < 4; j++)
            {
                first = characteristic_buff.IndexOf("{", next);
                next = characteristic_buff.IndexOf("}", first);
                characteristic_data_array[j] = characteristic_buff.Substring(first + 1, next - first - 1);
                first = first + 1;
                next = next + 1;
            }

            string characteristic = characteristic_data_array[3];

            string player_name = "{" + characteristic_data_array[0] + "}{" + characteristic_data_array[1] + "}{" + characteristic_data_array[2] + "}";
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
            string all_name = "{"+Get_Info[0] + "}{" + Get_Info[1] + "}{" + Get_Info[2]+"}";
            string all_name_save ="{" + CodeRoom.Text + "}{" + id_client + "}{" + id_name + "}";

            if (id_client.IndexOf(Get_Info[1]) > -1 && id_name.IndexOf(Get_Info[2]) > -1)
            {
                //istBox1.BackColor = Color.Green; 
                Next_Move_button.Visible = true;

            }
            if (player_identifier1.Text.IndexOf(all_name) > -1)
            {
                listBox1.BackColor = Color.Green;

                if (listBox2.BackColor != Color.FromArgb(112, 0, 0)) listBox2.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox9.BackColor != Color.FromArgb(112, 0, 0)) listBox9.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox8.BackColor != Color.FromArgb(112, 0, 0)) listBox8.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox7.BackColor != Color.FromArgb(112, 0, 0)) listBox7.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox6.BackColor != Color.FromArgb(112, 0, 0)) listBox6.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox5.BackColor != Color.FromArgb(112, 0, 0)) listBox5.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox4.BackColor != Color.FromArgb(112, 0, 0)) listBox4.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox3.BackColor != Color.FromArgb(112, 0, 0)) listBox3.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox10.BackColor != Color.FromArgb(112, 0, 0)) listBox10.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox11.BackColor != Color.FromArgb(112, 0, 0)) listBox11.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox12.BackColor != Color.FromArgb(112, 0, 0)) listBox12.BackColor = Color.FromArgb(64, 64, 66);



            }
            else if (player_identifier2.Text.IndexOf(all_name) > -1)
            {
                listBox2.BackColor = Color.Green;

                if (listBox1.BackColor != Color.FromArgb(112, 0, 0)) listBox1.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox9.BackColor != Color.FromArgb(112, 0, 0)) listBox9.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox8.BackColor != Color.FromArgb(112, 0, 0)) listBox8.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox7.BackColor != Color.FromArgb(112, 0, 0)) listBox7.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox6.BackColor != Color.FromArgb(112, 0, 0)) listBox6.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox5.BackColor != Color.FromArgb(112, 0, 0)) listBox5.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox4.BackColor != Color.FromArgb(112, 0, 0)) listBox4.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox3.BackColor != Color.FromArgb(112, 0, 0)) listBox3.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox10.BackColor != Color.FromArgb(112, 0, 0)) listBox10.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox11.BackColor != Color.FromArgb(112, 0, 0)) listBox11.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox12.BackColor != Color.FromArgb(112, 0, 0)) listBox12.BackColor = Color.FromArgb(64, 64, 66);

               
               
            }
            else if (player_identifier3.Text.IndexOf(all_name) > -1)
            {
                listBox3.BackColor = Color.Green;

                if (listBox1.BackColor != Color.FromArgb(112, 0, 0)) listBox1.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox2.BackColor != Color.FromArgb(112, 0, 0)) listBox2.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox8.BackColor != Color.FromArgb(112, 0, 0)) listBox8.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox7.BackColor != Color.FromArgb(112, 0, 0)) listBox7.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox6.BackColor != Color.FromArgb(112, 0, 0)) listBox6.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox5.BackColor != Color.FromArgb(112, 0, 0)) listBox5.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox4.BackColor != Color.FromArgb(112, 0, 0)) listBox4.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox9.BackColor != Color.FromArgb(112, 0, 0)) listBox9.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox10.BackColor != Color.FromArgb(112, 0, 0)) listBox10.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox11.BackColor != Color.FromArgb(112, 0, 0)) listBox11.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox12.BackColor != Color.FromArgb(112, 0, 0)) listBox12.BackColor = Color.FromArgb(64, 64, 66);

               
                
            }        
            else if (player_identifier4.Text.IndexOf(all_name) > -1)
            {
                listBox4.BackColor = Color.Green;

                if (listBox1.BackColor != Color.FromArgb(112, 0, 0)) listBox1.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox2.BackColor != Color.FromArgb(112, 0, 0)) listBox2.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox9.BackColor != Color.FromArgb(112, 0, 0)) listBox9.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox7.BackColor != Color.FromArgb(112, 0, 0)) listBox7.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox6.BackColor != Color.FromArgb(112, 0, 0)) listBox6.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox5.BackColor != Color.FromArgb(112, 0, 0)) listBox5.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox8.BackColor != Color.FromArgb(112, 0, 0)) listBox8.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox3.BackColor != Color.FromArgb(112, 0, 0)) listBox3.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox10.BackColor != Color.FromArgb(112, 0, 0)) listBox10.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox11.BackColor != Color.FromArgb(112, 0, 0)) listBox11.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox12.BackColor != Color.FromArgb(112, 0, 0)) listBox12.BackColor = Color.FromArgb(64, 64, 66);

               
             

            }
            else if (player_identifier5.Text.IndexOf(all_name) > -1)
            {
                listBox5.BackColor = Color.Green;

                if (listBox1.BackColor != Color.FromArgb(112, 0, 0)) listBox1.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox2.BackColor != Color.FromArgb(112, 0, 0)) listBox2.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox9.BackColor != Color.FromArgb(112, 0, 0)) listBox9.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox8.BackColor != Color.FromArgb(112, 0, 0)) listBox8.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox6.BackColor != Color.FromArgb(112, 0, 0)) listBox6.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox7.BackColor != Color.FromArgb(112, 0, 0)) listBox7.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox4.BackColor != Color.FromArgb(112, 0, 0)) listBox4.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox3.BackColor != Color.FromArgb(112, 0, 0)) listBox3.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox10.BackColor != Color.FromArgb(112, 0, 0)) listBox10.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox11.BackColor != Color.FromArgb(112, 0, 0)) listBox11.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox12.BackColor != Color.FromArgb(112, 0, 0)) listBox12.BackColor = Color.FromArgb(64, 64, 66);

               
        

            }
            else if (player_identifier6.Text.IndexOf(all_name) > -1)
            {
                listBox6.BackColor = Color.Green;

                if (listBox1.BackColor != Color.FromArgb(112, 0, 0)) listBox1.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox2.BackColor != Color.FromArgb(112, 0, 0)) listBox2.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox9.BackColor != Color.FromArgb(112, 0, 0)) listBox9.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox8.BackColor != Color.FromArgb(112, 0, 0)) listBox8.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox7.BackColor != Color.FromArgb(112, 0, 0)) listBox7.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox5.BackColor != Color.FromArgb(112, 0, 0)) listBox5.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox4.BackColor != Color.FromArgb(112, 0, 0)) listBox4.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox3.BackColor != Color.FromArgb(112, 0, 0)) listBox3.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox10.BackColor != Color.FromArgb(112, 0, 0)) listBox10.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox11.BackColor != Color.FromArgb(112, 0, 0)) listBox11.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox12.BackColor != Color.FromArgb(112, 0, 0)) listBox12.BackColor = Color.FromArgb(64, 64, 66);

                
             
            }
            else if (player_identifier7.Text.IndexOf(all_name) > -1)
            {
                listBox7.BackColor = Color.Green;

                if (listBox1.BackColor != Color.FromArgb(112, 0, 0)) listBox1.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox2.BackColor != Color.FromArgb(112, 0, 0)) listBox2.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox9.BackColor != Color.FromArgb(112, 0, 0)) listBox9.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox8.BackColor != Color.FromArgb(112, 0, 0)) listBox8.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox5.BackColor != Color.FromArgb(112, 0, 0)) listBox5.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox6.BackColor != Color.FromArgb(112, 0, 0)) listBox6.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox4.BackColor != Color.FromArgb(112, 0, 0)) listBox4.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox3.BackColor != Color.FromArgb(112, 0, 0)) listBox3.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox10.BackColor != Color.FromArgb(112, 0, 0)) listBox10.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox11.BackColor != Color.FromArgb(112, 0, 0)) listBox11.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox12.BackColor != Color.FromArgb(112, 0, 0)) listBox12.BackColor = Color.FromArgb(64, 64, 66);

               
               
            }
            else if (player_identifier8.Text.IndexOf(all_name) > -1)
            {
                listBox8.BackColor = Color.Green;


                if (listBox1.BackColor != Color.FromArgb(112, 0, 0)) listBox1.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox2.BackColor != Color.FromArgb(112, 0, 0)) listBox2.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox9.BackColor != Color.FromArgb(112, 0, 0)) listBox9.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox4.BackColor != Color.FromArgb(112, 0, 0)) listBox4.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox7.BackColor != Color.FromArgb(112, 0, 0)) listBox7.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox6.BackColor != Color.FromArgb(112, 0, 0)) listBox6.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox5.BackColor != Color.FromArgb(112, 0, 0)) listBox5.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox3.BackColor != Color.FromArgb(112, 0, 0)) listBox3.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox10.BackColor != Color.FromArgb(112, 0, 0)) listBox10.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox11.BackColor != Color.FromArgb(112, 0, 0)) listBox11.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox12.BackColor != Color.FromArgb(112, 0, 0)) listBox12.BackColor = Color.FromArgb(64, 64, 66);

                
              
            }
            else if (player_identifier9.Text.IndexOf(all_name) > -1)
            {
                listBox9.BackColor = Color.Green;


                if (listBox1.BackColor != Color.FromArgb(112, 0, 0)) listBox1.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox2.BackColor != Color.FromArgb(112, 0, 0)) listBox2.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox3.BackColor != Color.FromArgb(112, 0, 0)) listBox3.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox8.BackColor != Color.FromArgb(112, 0, 0)) listBox8.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox7.BackColor != Color.FromArgb(112, 0, 0)) listBox7.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox6.BackColor != Color.FromArgb(112, 0, 0)) listBox6.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox5.BackColor != Color.FromArgb(112, 0, 0)) listBox5.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox4.BackColor != Color.FromArgb(112, 0, 0)) listBox4.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox10.BackColor != Color.FromArgb(112, 0, 0)) listBox10.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox11.BackColor != Color.FromArgb(112, 0, 0)) listBox11.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox12.BackColor != Color.FromArgb(112, 0, 0)) listBox12.BackColor = Color.FromArgb(64, 64, 66);

               
                
            }
            else if (player_identifier10.Text.IndexOf(all_name) > -1)
            {
                listBox10.BackColor = Color.Green;

                if (listBox1.BackColor != Color.FromArgb(112, 0, 0)) listBox1.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox2.BackColor != Color.FromArgb(112, 0, 0)) listBox2.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox9.BackColor != Color.FromArgb(112, 0, 0)) listBox9.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox8.BackColor != Color.FromArgb(112, 0, 0)) listBox8.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox7.BackColor != Color.FromArgb(112, 0, 0)) listBox7.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox6.BackColor != Color.FromArgb(112, 0, 0)) listBox6.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox5.BackColor != Color.FromArgb(112, 0, 0)) listBox5.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox4.BackColor != Color.FromArgb(112, 0, 0)) listBox4.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox3.BackColor != Color.FromArgb(112, 0, 0)) listBox3.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox11.BackColor != Color.FromArgb(112, 0, 0)) listBox11.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox12.BackColor != Color.FromArgb(112, 0, 0)) listBox12.BackColor = Color.FromArgb(64, 64, 66);

                
               
            }
            else if (player_identifier11.Text.IndexOf(all_name) > -1)
            {
                listBox11.BackColor = Color.Green;

                if (listBox1.BackColor != Color.FromArgb(112, 0, 0)) listBox1.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox2.BackColor != Color.FromArgb(112, 0, 0)) listBox2.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox9.BackColor != Color.FromArgb(112, 0, 0)) listBox9.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox8.BackColor != Color.FromArgb(112, 0, 0)) listBox8.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox7.BackColor != Color.FromArgb(112, 0, 0)) listBox7.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox6.BackColor != Color.FromArgb(112, 0, 0)) listBox6.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox5.BackColor != Color.FromArgb(112, 0, 0)) listBox5.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox4.BackColor != Color.FromArgb(112, 0, 0)) listBox4.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox3.BackColor != Color.FromArgb(112, 0, 0)) listBox3.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox10.BackColor != Color.FromArgb(112, 0, 0)) listBox10.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox12.BackColor != Color.FromArgb(112, 0, 0)) listBox12.BackColor = Color.FromArgb(64, 64, 66);

                
                
            }
            else if (player_identifier12.Text.IndexOf(all_name) > -1)
            {
                listBox12.BackColor = Color.Green;

                if (listBox1.BackColor != Color.FromArgb(112, 0, 0)) listBox1.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox2.BackColor != Color.FromArgb(112, 0, 0)) listBox2.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox9.BackColor != Color.FromArgb(112, 0, 0)) listBox9.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox8.BackColor != Color.FromArgb(112, 0, 0)) listBox8.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox7.BackColor != Color.FromArgb(112, 0, 0)) listBox7.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox6.BackColor != Color.FromArgb(112, 0, 0)) listBox6.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox5.BackColor != Color.FromArgb(112, 0, 0)) listBox5.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox4.BackColor != Color.FromArgb(112, 0, 0)) listBox4.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox3.BackColor != Color.FromArgb(112, 0, 0)) listBox3.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox10.BackColor != Color.FromArgb(112, 0, 0)) listBox10.BackColor = Color.FromArgb(64, 64, 66);
                if (listBox11.BackColor != Color.FromArgb(112, 0, 0)) listBox11.BackColor = Color.FromArgb(64, 64, 66);

                
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

            string Message = "OPEN_CHARACTE" + " {" + CodeRoom.Text + "}{" + id_client+"}{"+ id_name + "}{" + SendData + "}";
           
            send_Messege(Message, false);

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
            string Message = "NEXT_____MOVE " + "{" + CodeRoom.Text + "}{" + id_client+"}";
         
            send_Messege(Message, false);


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
       
            skip_button.Visible = true;
            vote_click++;
                string Message = "VOTING___KICK " + name +"{"+ "no" + "}";
               

            send_Messege(Message, false);

            if (vote_click==2) {
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
                   skip_button.Visible = false;
            }
        }

        private void skip_button_Click(object sender, EventArgs e)
        {
            string Message = "VOTING___KICK " + "{" + CodeRoom.Text + "}{" + id_client + "}{" + id_name + "}{" + "skip" + "}";
          

            send_Messege(Message, false);

            skip_button.Visible = false;
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

        private void Bunker_img_Click(object sender, EventArgs e)
        {
            if (Bunker_Box.Visible == false) Bunker_Box.Visible = true;
            else Bunker_Box.Visible = false;

           
        }

        private void exit_button_Click(object sender, EventArgs e)
        {
            if (serverStream != null && clientSocket.Connected == true)
            {
              
                send_Messege("DISCONN__ROOM вышел из игры", false);
            }

            if (clientSocket.Connected != false) clientSocket.Close();
            if (serverStream != null) serverStream.Close();
                     
            this.Close();
        }

        private void send_button_Click(object sender, EventArgs e)
        {

           string Message = "CHAT_SEND_MSG " + "{" + CodeRoom.Text + "}{" + id_client + "}{" + id_name + "}"+"{" +Chat_send_box.Text + "}";
            
            send_Messege(Message, false);

            Chat_send_box.Clear();
        }

        private void map_pictureBox_Click(object sender, EventArgs e)
        {
            if (Map_Box.Visible == false) Map_Box.Visible = true;
            else Map_Box.Visible = false;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (action_cards_Box.Visible == false) action_cards_Box.Visible = true;
            else action_cards_Box.Visible = false;
        }
     
        private void set_vote(string line)
        {
            int count = 0;
            int first_line = 0;
            int next_line = 0;
            string[] Get_Info_line = new string[12];

            for (int i = 0; i < 12; i++)
            {
                first_line = line.IndexOf("(", next_line);
                if (first_line == -1) break;
                next_line = line.IndexOf(")", first_line);
                Get_Info_line[i] = line.Substring(first_line + 1, next_line - first_line - 1);
                first_line = first_line + 1;
                next_line = next_line + 1;
                count++;
            }

            for (int j = 0; j < count; j++)
            {
                int first = 0;
                int next = 0;
                string[] Get_Info = new string[4];

                for (int i = 0; i < 4; i++)
                {
                    first = Get_Info_line[j].IndexOf("{", next);
                    next = Get_Info_line[j].IndexOf("}", first);
                    Get_Info[i] = Get_Info_line[j].Substring(first + 1, next - first - 1);
                    first = first + 1;
                    next = next + 1;
                }
                string full_name = "{" + Get_Info[0] + "}{" + Get_Info[1] + "}{" + Get_Info[2] + "}";
                if (player_identifier1.Text.IndexOf(full_name) > -1)vote_label1.Text = Get_Info[3];
                if (player_identifier2.Text.IndexOf(full_name) > -1) vote_label2.Text = Get_Info[3];
                if (player_identifier3.Text.IndexOf(full_name) > -1) vote_label3.Text = Get_Info[3];
                if (player_identifier4.Text.IndexOf(full_name) > -1) vote_label4.Text = Get_Info[3];
                if (player_identifier5.Text.IndexOf(full_name) > -1) vote_label5.Text = Get_Info[3];
                if (player_identifier6.Text.IndexOf(full_name) > -1) vote_label6.Text = Get_Info[3];
                if (player_identifier7.Text.IndexOf(full_name) > -1) vote_label7.Text = Get_Info[3];
                if (player_identifier8.Text.IndexOf(full_name) > -1) vote_label8.Text = Get_Info[3];
                if (player_identifier9.Text.IndexOf(full_name) > -1) vote_label9.Text = Get_Info[3];
                if (player_identifier10.Text.IndexOf(full_name) > -1) vote_label10.Text = Get_Info[3];
                if (player_identifier11.Text.IndexOf(full_name) > -1) vote_label11.Text = Get_Info[3];
                if (player_identifier12.Text.IndexOf(full_name) > -1) vote_label12.Text = Get_Info[3];

            }

        }

        private void create_cards(string card_one, string card_two)
        {
            cards = new Cards[17];
            cards[0] = new Cards("#100", "Новая всем работа", "Данная карта дает способность всем перераздать характеристику 'работа'",true);
            cards[1] = new Cards("#101", "Новая всем биология","Данная карта дает способность всем перераздать характеристики 'Пол' и 'Возраст' ", true);
            cards[2] = new Cards("#102", "Новое всем Хобби", "Данная карта дает способность всем перераздать характеристики 'Хобби' ", true);
            cards[3] = new Cards("#103", "Новая всем Здоровье", "Данная карта дает способность всем перераздать характеристики 'Здоровье'", true);
            cards[4] = new Cards("#104", "Новая всем Багаж"," Данная карта дает способность всем перераздать характеристики 'Багаж' ", true);
            cards[5] = new Cards("#105", "Новая всем Фобия", "Данная карта дает способность всем перераздать характеристики 'Фобия' ", true);
            cards[6] = new Cards("#106", "Новый всем Характер", "Данная карта дает способность всем перераздать характеристики 'Характер'", true);
            cards[7] = new Cards("#107", "Новая себе работа", "Данная карта дает способность себе перераздать характеристику 'работа'", true);
            cards[8] = new Cards("#108", "Новая себе биология", "Данная карта дает способность себе перераздать характеристики 'Пол' и 'Возраст' ", true);
            cards[9] = new Cards("#109", "Новое себе Хобби", "Данная карта дает способность себе перераздать характеристики 'Хобби' ", true);
            cards[10] = new Cards("#110", "Новая себе Здоровье", "Данная карта дает способность себе перераздать характеристики 'Здоровье'", true);
            cards[11] = new Cards("#111", "Новая себе Багаж", " Данная карта дает способность себе перераздать характеристики 'Багаж' ", true);
            cards[12] = new Cards("#112", "Новая себе Фобия", "Данная карта дает способность себе перераздать характеристики 'Фобия' ", true);
            cards[13] = new Cards("#113", "Новый себе Характер", "Данная карта дает способность себе перераздать характеристики 'Характер'", true);
            cards[14] = new Cards("#200", "Твой друг", "Выбери себе друга , ты должен помочь ему зайти в бункер", false);
            cards[15] = new Cards("#201", "Твой Враг", "Выбери себе врага, ты должен не позволить ему зайти в бункер", false);
            cards[16] = new Cards("#202", "Предатель", "Ты должен предать одного игрока", false);

            for(int i =0;i<cards.Length; i++)
            {
                if(card_one == cards[i].ID)
                {
                    card_1_name_label.Text = cards[i].Name;
                    card_1_info_label.Text = cards[i].Info;

                    if (cards[i].Used == true) card_1_button.Visible = true;
                }
            }
            for (int i = 0; i < cards.Length; i++)
            {
                if (card_two == cards[i].ID)
                {
                    card_2_name_label.Text = cards[i].Name;
                    card_2_info_label.Text = cards[i].Info;
                    if (cards[i].Used == true) card_2_button.Visible = true;
                }
            }
        }

        private void card_1_button_Click(object sender, EventArgs e)
        {
            string Message = "CARD____ACTIV " + "{" + CodeRoom.Text + "}{" + id_client + "}{" + id_name + "}" + "{" + card_1_label.Text + "}";
            
            send_Messege(Message, false);

            card_1_button.Visible = false;
        }

        private void card_2_button_Click(object sender, EventArgs e)
        {
            string Message = "CARD____ACTIV " + "{" + CodeRoom.Text + "}{" + id_client + "}{" + id_name + "}" + "{" + card_2_label.Text + "}";           
            send_Messege(Message, false);

            card_2_button.Visible = false;
        }

        private void update_info(string data)
        {
           
            string[] all_characteristic = new string[all_playing];

            int first_pos_characteristic = 0;
            int next_pos_characteristic_ = 0;

            for (int i = 0; i < all_playing ; i++)
            {
                first_pos_characteristic = data.IndexOf("(", next_pos_characteristic_);
                if (first_pos_characteristic == -1) break;
                next_pos_characteristic_ = data.IndexOf(")", first_pos_characteristic);
                all_characteristic[i] = data.Substring(first_pos_characteristic + 1, next_pos_characteristic_ - first_pos_characteristic - 1);
                first_pos_characteristic = first_pos_characteristic + 1;
                next_pos_characteristic_ = next_pos_characteristic_ + 1;
            }

            for (int j = 0; j < all_characteristic.Length; j++)
            {
                int first = 0;
                int next = 0;
                string[] Get_Info = new string[7];

                for (int i = 0; i < Get_Info.Length; i++)
                {
                  
                    first = all_characteristic[j].IndexOf("{", next);
                    if (first == -1) break;
                    next = all_characteristic[j].IndexOf("}", first);
                    Get_Info[i] = all_characteristic[j].Substring(first + 1, next - first - 1);
                    first = first + 1;
                    next = next + 1;
                }

                string player_name = "{" + Get_Info[0] + "}{" + Get_Info[1] + "}{" + Get_Info[2] + "}";
                if (player_identifier1.Text.IndexOf(player_name) > -1)
                {

                    if (Get_Info[3].IndexOf("Age") > -1)
                    {
                       if (AgeText1.Text.Length > 0 && Get_Info[3].IndexOf("none")==-1) AgeText1.Text = Get_Info[4];
                       if (SexText1.Text.Length > 0 && Get_Info[3].IndexOf("none") == -1) SexText1.Text = Get_Info[6];
                    }
                    
                    if (Get_Info[3].IndexOf("Baggage") > -1)
                    {
                        if (BaggageText1.Text.Length > 0 && Get_Info[3].IndexOf("none") == -1) BaggageText1.Text = Get_Info[4];
                    }
                    if (Get_Info[3].IndexOf("Job") > -1)
                    {
                        if (JobText1.Text.Length > 0 && Get_Info[3].IndexOf("none") == -1) JobText1.Text = Get_Info[4];
                    }
                    if (Get_Info[3].IndexOf("Hobby") > -1)
                    {
                        if (HobbyText1.Text.Length > 0 && Get_Info[3].IndexOf("none") == -1)  HobbyText1.Text = Get_Info[4];
                    }
                    if (Get_Info[3].IndexOf("Health") > -1)
                    {
                        if (HealthText1.Text.Length > 0 && Get_Info[3].IndexOf("none") == -1) HealthText1.Text = Get_Info[4];
                    }
                    if (Get_Info[3].IndexOf("Phobia") > -1)
                    {
                        if (PhobiaText1.Text.Length > 0 && Get_Info[3].IndexOf("none") == -1)  PhobiaText1.Text = Get_Info[4];
                    }
                    if (Get_Info[3].IndexOf("Character") > -1)
                    {
                        if (CharacterText1.Text.Length > 0 && Get_Info[3].IndexOf("none") == -1)  CharacterText1.Text = Get_Info[4];
                    }
                }
                if (player_identifier2.Text.IndexOf(player_name) > -1)
                {

                    if (Get_Info[3].IndexOf("Age") > -1)
                    {
                        if (AgeText2.Text.Length > 0 && Get_Info[3].IndexOf("none") == -1) AgeText2.Text = Get_Info[4];
                        if (SexText2.Text.Length > 0 && Get_Info[3].IndexOf("none") == -1) SexText2.Text = Get_Info[6];
                    }

                    if (Get_Info[3].IndexOf("Baggage") > -1)
                    {
                        if (BaggageText2.Text.Length > 0 && Get_Info[3].IndexOf("none") == -1) BaggageText2.Text = Get_Info[4];
                    }
                    if (Get_Info[3].IndexOf("Job") > -1)
                    {
                        if (JobText2.Text.Length > 0 && Get_Info[3].IndexOf("none") == -1) JobText2.Text = Get_Info[4];
                    }
                    if (Get_Info[3].IndexOf("Hobby") > -1)
                    {
                        if (HobbyText2.Text.Length > 0 && Get_Info[3].IndexOf("none") == -1) HobbyText2.Text = Get_Info[4];
                    }
                    if (Get_Info[3].IndexOf("Health") > -1)
                    {
                        if (HealthText2.Text.Length > 0 && Get_Info[3].IndexOf("none") == -1) HealthText2.Text = Get_Info[4];
                    }
                    if (Get_Info[3].IndexOf("Phobia") > -1)
                    {
                        if (PhobiaText2.Text.Length > 0 && Get_Info[3].IndexOf("none") == -1) PhobiaText2.Text = Get_Info[4];
                    }
                    if (Get_Info[3].IndexOf("Character") > -1)
                    {
                        if (CharacterText2.Text.Length > 0 && Get_Info[3].IndexOf("none") == -1) CharacterText2.Text = Get_Info[4];
                    }
                }
                if (player_identifier3.Text.IndexOf(player_name) > -1)
                {

                    if (Get_Info[3].IndexOf("Age") > -1)
                    {
                        if (AgeText3.Text.Length > 0 && Get_Info[3].IndexOf("none") == -1) AgeText3.Text = Get_Info[4];
                        if (SexText3.Text.Length > 0 && Get_Info[3].IndexOf("none") == -1) SexText3.Text = Get_Info[6];
                    }

                    if (Get_Info[3].IndexOf("Baggage") > -1)
                    {
                        if (BaggageText3.Text.Length > 0 && Get_Info[3].IndexOf("none") == -1) BaggageText3.Text = Get_Info[4];
                    }
                    if (Get_Info[3].IndexOf("Job") > -1)
                    {
                        if (JobText3.Text.Length > 0 && Get_Info[3].IndexOf("none") == -1) JobText3.Text = Get_Info[4];
                    }
                    if (Get_Info[3].IndexOf("Hobby") > -1)
                    {
                        if (HobbyText3.Text.Length > 0 && Get_Info[3].IndexOf("none") == -1) HobbyText3.Text = Get_Info[4];
                    }
                    if (Get_Info[3].IndexOf("Health") > -1)
                    {
                        if (HealthText3.Text.Length > 0 && Get_Info[3].IndexOf("none") == -1) HealthText3.Text = Get_Info[4];
                    }
                    if (Get_Info[3].IndexOf("Phobia") > -1)
                    {
                        if (PhobiaText3.Text.Length > 0 && Get_Info[3].IndexOf("none") == -1) PhobiaText3.Text = Get_Info[4];
                    }
                    if (Get_Info[3].IndexOf("Character") > -1)
                    {
                        if (CharacterText3.Text.Length > 0 && Get_Info[3].IndexOf("none") == -1) CharacterText3.Text = Get_Info[4];
                    }
                }
                if (player_identifier4.Text.IndexOf(player_name) > -1)
                {

                    if (Get_Info[3].IndexOf("Age") > -1)
                    {
                        if (AgeText4.Text.Length > 0 && Get_Info[3].IndexOf("none") == -1) AgeText4.Text = Get_Info[4];
                        if (SexText4.Text.Length > 0 && Get_Info[3].IndexOf("none") == -1) SexText4.Text = Get_Info[6];
                    }

                    if (Get_Info[3].IndexOf("Baggage") > -1)
                    {
                        if (BaggageText4.Text.Length > 0 && Get_Info[3].IndexOf("none") == -1) BaggageText4.Text = Get_Info[4];
                    }
                    if (Get_Info[3].IndexOf("Job") > -1)
                    {
                        if (JobText4.Text.Length > 0 && Get_Info[3].IndexOf("none") == -1) JobText4.Text = Get_Info[4];
                    }
                    if (Get_Info[3].IndexOf("Hobby") > -1)
                    {
                        if (HobbyText4.Text.Length > 0 && Get_Info[3].IndexOf("none") == -1) HobbyText4.Text = Get_Info[4];
                    }
                    if (Get_Info[3].IndexOf("Health") > -1)
                    {
                        if (HealthText4.Text.Length > 0 && Get_Info[3].IndexOf("none") == -1) HealthText4.Text = Get_Info[4];
                    }
                    if (Get_Info[3].IndexOf("Phobia") > -1)
                    {
                        if (PhobiaText4.Text.Length > 0 && Get_Info[3].IndexOf("none") == -1) PhobiaText4.Text = Get_Info[4];
                    }
                    if (Get_Info[3].IndexOf("Character") > -1)
                    {
                        if (CharacterText4.Text.Length > 0 && Get_Info[3].IndexOf("none") == -1) CharacterText4.Text = Get_Info[4];
                    }
                }
                if (player_identifier5.Text.IndexOf(player_name) > -1)
                {

                    if (Get_Info[3].IndexOf("Age") > -1)
                    {
                        if (AgeText5.Text.Length > 0 && Get_Info[3].IndexOf("none") == -1) AgeText5.Text = Get_Info[4];
                        if (SexText5.Text.Length > 0 && Get_Info[3].IndexOf("none") == -1) SexText5.Text = Get_Info[6];
                    }

                    if (Get_Info[3].IndexOf("Baggage") > -1)
                    {
                        if (BaggageText5.Text.Length > 0 && Get_Info[3].IndexOf("none") == -1) BaggageText5.Text = Get_Info[4];
                    }
                    if (Get_Info[3].IndexOf("Job") > -1)
                    {
                        if (JobText5.Text.Length > 0 && Get_Info[3].IndexOf("none") == -1) JobText5.Text = Get_Info[4];
                    }
                    if (Get_Info[3].IndexOf("Hobby") > -1)
                    {
                        if (HobbyText5.Text.Length > 0 && Get_Info[3].IndexOf("none") == -1) HobbyText5.Text = Get_Info[4];
                    }
                    if (Get_Info[3].IndexOf("Health") > -1)
                    {
                        if (HealthText5.Text.Length > 0 && Get_Info[3].IndexOf("none") == -1) HealthText5.Text = Get_Info[4];
                    }
                    if (Get_Info[3].IndexOf("Phobia") > -1)
                    {
                        if (PhobiaText5.Text.Length > 0 && Get_Info[3].IndexOf("none") == -1) PhobiaText5.Text = Get_Info[4];
                    }
                    if (Get_Info[3].IndexOf("Character") > -1)
                    {
                        if (CharacterText5.Text.Length > 0 && Get_Info[3].IndexOf("none") == -1) CharacterText5.Text = Get_Info[4];
                    }
                }
                if (player_identifier6.Text.IndexOf(player_name) > -1)
                {

                    if (Get_Info[3].IndexOf("Age") > -1)
                    {
                        if (AgeText6.Text.Length > 0 && Get_Info[3].IndexOf("none") == -1) AgeText6.Text = Get_Info[4];
                        if (SexText6.Text.Length > 0 && Get_Info[3].IndexOf("none") == -1) SexText6.Text = Get_Info[6];
                    }

                    if (Get_Info[3].IndexOf("Baggage") > -1)
                    {
                        if (BaggageText6.Text.Length > 0 && Get_Info[3].IndexOf("none") == -1) BaggageText6.Text = Get_Info[4];
                    }
                    if (Get_Info[3].IndexOf("Job") > -1)
                    {
                        if (JobText6.Text.Length > 0 && Get_Info[3].IndexOf("none") == -1) JobText6.Text = Get_Info[4];
                    }
                    if (Get_Info[3].IndexOf("Hobby") > -1)
                    {
                        if (HobbyText6.Text.Length > 0 && Get_Info[3].IndexOf("none") == -1) HobbyText6.Text = Get_Info[4];
                    }
                    if (Get_Info[3].IndexOf("Health") > -1)
                    {
                        if (HealthText6.Text.Length > 0 && Get_Info[3].IndexOf("none") == -1) HealthText6.Text = Get_Info[4];
                    }
                    if (Get_Info[3].IndexOf("Phobia") > -1)
                    {
                        if (PhobiaText6.Text.Length > 0 && Get_Info[3].IndexOf("none") == -1) PhobiaText6.Text = Get_Info[4];
                    }
                    if (Get_Info[3].IndexOf("Character") > -1)
                    {
                        if (CharacterText6.Text.Length > 0 && Get_Info[3].IndexOf("none") == -1) CharacterText6.Text = Get_Info[4];
                    }
                }
                if (player_identifier7.Text.IndexOf(player_name) > -1)
                {

                    if (Get_Info[3].IndexOf("Age") > -1)
                    {
                        if (AgeText7.Text.Length > 0 && Get_Info[3].IndexOf("none") == -1) AgeText7.Text = Get_Info[4];
                        if (SexText7.Text.Length > 0 && Get_Info[3].IndexOf("none") == -1) SexText7.Text = Get_Info[6];
                    }

                    if (Get_Info[3].IndexOf("Baggage") > -1)
                    {
                        if (BaggageText7.Text.Length > 0 && Get_Info[3].IndexOf("none") == -1) BaggageText7.Text = Get_Info[4];
                    }
                    if (Get_Info[3].IndexOf("Job") > -1)
                    {
                        if (JobText7.Text.Length > 0 && Get_Info[3].IndexOf("none") == -1) JobText7.Text = Get_Info[4];
                    }
                    if (Get_Info[3].IndexOf("Hobby") > -1)
                    {
                        if (HobbyText7.Text.Length > 0 && Get_Info[3].IndexOf("none") == -1) HobbyText7.Text = Get_Info[4];
                    }
                    if (Get_Info[3].IndexOf("Health") > -1)
                    {
                        if (HealthText7.Text.Length > 0 && Get_Info[3].IndexOf("none") == -1) HealthText7.Text = Get_Info[4];
                    }
                    if (Get_Info[3].IndexOf("Phobia") > -1)
                    {
                        if (PhobiaText7.Text.Length > 0 && Get_Info[3].IndexOf("none") == -1) PhobiaText7.Text = Get_Info[4];
                    }
                    if (Get_Info[3].IndexOf("Character") > -1)
                    {
                        if (CharacterText7.Text.Length > 0 && Get_Info[3].IndexOf("none") == -1) CharacterText7.Text = Get_Info[4];
                    }
                }
                if (player_identifier8.Text.IndexOf(player_name) > -1)
                {

                    if (Get_Info[3].IndexOf("Age") > -1)
                    {
                        if (AgeText8.Text.Length > 0 && Get_Info[3].IndexOf("none") == -1) AgeText8.Text = Get_Info[4];
                        if (SexText8.Text.Length > 0 && Get_Info[3].IndexOf("none") == -1) SexText8.Text = Get_Info[6];
                    }

                    if (Get_Info[3].IndexOf("Baggage") > -1)
                    {
                        if (BaggageText8.Text.Length > 0 && Get_Info[3].IndexOf("none") == -1) BaggageText8.Text = Get_Info[4];
                    }
                    if (Get_Info[3].IndexOf("Job") > -1)
                    {
                        if (JobText8.Text.Length > 0 && Get_Info[3].IndexOf("none") == -1) JobText8.Text = Get_Info[4];
                    }
                    if (Get_Info[3].IndexOf("Hobby") > -1)
                    {
                        if (HobbyText8.Text.Length > 0 && Get_Info[3].IndexOf("none") == -1) HobbyText8.Text = Get_Info[4];
                    }
                    if (Get_Info[3].IndexOf("Health") > -1)
                    {
                        if (HealthText8.Text.Length > 0 && Get_Info[3].IndexOf("none") == -1) HealthText8.Text = Get_Info[4];
                    }
                    if (Get_Info[3].IndexOf("Phobia") > -1)
                    {
                        if (PhobiaText8.Text.Length > 0 && Get_Info[3].IndexOf("none") == -1) PhobiaText8.Text = Get_Info[4];
                    }
                    if (Get_Info[3].IndexOf("Character") > -1)
                    {
                        if (CharacterText8.Text.Length > 0 && Get_Info[3].IndexOf("none") == -1) CharacterText8.Text = Get_Info[4];
                    }
                }
                if (player_identifier9.Text.IndexOf(player_name) > -1)
                {

                    if (Get_Info[3].IndexOf("Age") > -1)
                    {
                        if (AgeText9.Text.Length > 0 && Get_Info[3].IndexOf("none") == -1) AgeText9.Text = Get_Info[4];
                        if (SexText9.Text.Length > 0 && Get_Info[3].IndexOf("none") == -1) SexText9.Text = Get_Info[6];
                    }

                    if (Get_Info[3].IndexOf("Baggage") > -1)
                    {
                        if (BaggageText9.Text.Length > 0 && Get_Info[3].IndexOf("none") == -1) BaggageText9.Text = Get_Info[4];
                    }
                    if (Get_Info[3].IndexOf("Job") > -1)
                    {
                        if (JobText9.Text.Length > 0 && Get_Info[3].IndexOf("none") == -1) JobText9.Text = Get_Info[4];
                    }
                    if (Get_Info[3].IndexOf("Hobby") > -1)
                    {
                        if (HobbyText9.Text.Length > 0 && Get_Info[3].IndexOf("none") == -1) HobbyText9.Text = Get_Info[4];
                    }
                    if (Get_Info[3].IndexOf("Health") > -1)
                    {
                        if (HealthText9.Text.Length > 0 && Get_Info[3].IndexOf("none") == -1) HealthText9.Text = Get_Info[4];
                    }
                    if (Get_Info[3].IndexOf("Phobia") > -1)
                    {
                        if (PhobiaText9.Text.Length > 0 && Get_Info[3].IndexOf("none") == -1) PhobiaText9.Text = Get_Info[4];
                    }
                    if (Get_Info[3].IndexOf("Character") > -1)
                    {
                        if (CharacterText9.Text.Length > 0 && Get_Info[3].IndexOf("none") == -1) CharacterText9.Text = Get_Info[4];
                    }
                }
                if (player_identifier10.Text.IndexOf(player_name) > -1)
                {

                    if (Get_Info[3].IndexOf("Age") > -1)
                    {
                        if (AgeText10.Text.Length > 0 && Get_Info[3].IndexOf("none") == -1) AgeText10.Text = Get_Info[4];
                        if (SexText10.Text.Length > 0 && Get_Info[3].IndexOf("none") == -1) SexText10.Text = Get_Info[6];
                    }

                    if (Get_Info[3].IndexOf("Baggage") > -1)
                    {
                        if (BaggageText10.Text.Length > 0 && Get_Info[3].IndexOf("none") == -1) BaggageText10.Text = Get_Info[4];
                    }
                    if (Get_Info[3].IndexOf("Job") > -1)
                    {
                        if (JobText10.Text.Length > 0 && Get_Info[3].IndexOf("none") == -1) JobText10.Text = Get_Info[4];
                    }
                    if (Get_Info[3].IndexOf("Hobby") > -1)
                    {
                        if (HobbyText10.Text.Length > 0 && Get_Info[3].IndexOf("none") == -1) HobbyText10.Text = Get_Info[4];
                    }
                    if (Get_Info[3].IndexOf("Health") > -1)
                    {
                        if (HealthText10.Text.Length > 0 && Get_Info[3].IndexOf("none") == -1) HealthText10.Text = Get_Info[4];
                    }
                    if (Get_Info[3].IndexOf("Phobia") > -1)
                    {
                        if (PhobiaText10.Text.Length > 0 && Get_Info[3].IndexOf("none") == -1) PhobiaText10.Text = Get_Info[4];
                    }
                    if (Get_Info[3].IndexOf("Character") > -1)
                    {
                        if (CharacterText10.Text.Length > 0 && Get_Info[3].IndexOf("none") == -1) CharacterText10.Text = Get_Info[4];
                    }
                }
                if (player_identifier11.Text.IndexOf(player_name) > -1)
                {

                    if (Get_Info[3].IndexOf("Age") > -1)
                    {
                        if (AgeText11.Text.Length > 0 && Get_Info[3].IndexOf("none") == -1) AgeText11.Text = Get_Info[4];
                        if (SexText11.Text.Length > 0 && Get_Info[3].IndexOf("none") == -1) SexText11.Text = Get_Info[6];
                    }

                    if (Get_Info[3].IndexOf("Baggage") > -1)
                    {
                        if (BaggageText11.Text.Length > 0 && Get_Info[3].IndexOf("none") == -1) BaggageText11.Text = Get_Info[4];
                    }
                    if (Get_Info[3].IndexOf("Job") > -1)
                    {
                        if (JobText11.Text.Length > 0 && Get_Info[3].IndexOf("none") == -1) JobText11.Text = Get_Info[4];
                    }
                    if (Get_Info[3].IndexOf("Hobby") > -1)
                    {
                        if (HobbyText11.Text.Length > 0 && Get_Info[3].IndexOf("none") == -1) HobbyText11.Text = Get_Info[4];
                    }
                    if (Get_Info[3].IndexOf("Health") > -1)
                    {
                        if (HealthText11.Text.Length > 0 && Get_Info[3].IndexOf("none") == -1) HealthText11.Text = Get_Info[4];
                    }
                    if (Get_Info[3].IndexOf("Phobia") > -1)
                    {
                        if (PhobiaText11.Text.Length > 0 && Get_Info[3].IndexOf("none") == -1) PhobiaText11.Text = Get_Info[4];
                    }
                    if (Get_Info[3].IndexOf("Character") > -1)
                    {
                        if (CharacterText11.Text.Length > 0 && Get_Info[3].IndexOf("none") == -1) CharacterText11.Text = Get_Info[4];
                    }
                }
                if (player_identifier12.Text.IndexOf(player_name) > -1)
                {

                    if (Get_Info[3].IndexOf("Age") > -1)
                    {
                        if (AgeText12.Text.Length > 0 && Get_Info[3].IndexOf("none") == -1) AgeText12.Text = Get_Info[4];
                        if (SexText12.Text.Length > 0 && Get_Info[3].IndexOf("none") == -1) SexText12.Text = Get_Info[6];
                    }

                    if (Get_Info[3].IndexOf("Baggage") > -1)
                    {
                        if (BaggageText12.Text.Length > 0 && Get_Info[3].IndexOf("none") == -1) BaggageText12.Text = Get_Info[4];
                    }
                    if (Get_Info[3].IndexOf("Job") > -1)
                    {
                        if (JobText12.Text.Length > 0 && Get_Info[3].IndexOf("none") == -1) JobText12.Text = Get_Info[4];
                    }
                    if (Get_Info[3].IndexOf("Hobby") > -1)
                    {
                        if (HobbyText12.Text.Length > 0 && Get_Info[3].IndexOf("none") == -1) HobbyText12.Text = Get_Info[4];
                    }
                    if (Get_Info[3].IndexOf("Health") > -1)
                    {
                        if (HealthText12.Text.Length > 0 && Get_Info[3].IndexOf("none") == -1) HealthText12.Text = Get_Info[4];
                    }
                    if (Get_Info[3].IndexOf("Phobia") > -1)
                    {
                        if (PhobiaText12.Text.Length > 0 && Get_Info[3].IndexOf("none") == -1) PhobiaText12.Text = Get_Info[4];
                    }
                    if (Get_Info[3].IndexOf("Character") > -1)
                    {
                        if (CharacterText12.Text.Length > 0 && Get_Info[3].IndexOf("none") == -1) CharacterText12.Text = Get_Info[4];
                    }
                }
            }
        }
        private void connect_timer_Tick(object sender, EventArgs e)
        {
            try
            {
                clientSocket = new TcpClient();
                readData = "Conected to room Server ...";
                msg();
                clientSocket.Connect(IPAddress.Parse(ip), Int32.Parse(port));

                serverStream = clientSocket.GetStream();
                //CONNECT__ROOM->ID_ROOM = ID_CLIENT = ID_NAME = STATUS=

                string Message = "CONNECT__ROOM " + "{" + CodeRoom.Text + "}" + "{" + id_client + "}" + "{" + id_name + "}" + "{" + status_game + "}";

                send_Messege(Message, true);


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
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                if (clientSocket == null)
                    clientSocket.Close();

                if (clientSocket.Connected == false)
                    MessageBox.Show("Сервер не работает ", "Ошибка", MessageBoxButtons.OK);

            }
            connect_timer.Stop();
        }

   
    }
    public class Cards
    {
        public  Cards(string id,string name, string info, bool used)
        {
            ID = id;
            Name = name;
            Info = info;
            Used = used;
        }

        public string ID { get; set; }
        public string Name { get; set; }
        public string Info { get; set; }
        public bool Used { get; set; }

    }

    public class info_Characteristic
    {
        public int Id_game { get; set; }
        public string room { get; set; }
        public string Id { get; set;  }
        public string Person_name { get; set; }
        public string Age { get; set; }
        public string Sex { get; set; }
        public string Job { get; set; }
        public string Hobby { get; set; }
        public string Health { get; set; }
        public string Baggage { get; set; }
        public string Phobia { get; set; }
        public string Character { get; set; }
        public string Action_cards_one { get; set; }
        public string Action_cards_two { get; set; }
        public string Move { get; set; }



        public info_Characteristic(string data)
        {
            string[] individual_characteristic = new string[15];
            //textBox2.Text = all_characteristic [i]+ "|" + textBox2.Text;
            int first = 0;
            int next = 0;
            for (int j = 0; j < 15; j++)
            {
                first = data.IndexOf("{", next);
                next = data.IndexOf("}", first);
                individual_characteristic[j] = data.Substring(first + 1, next - first - 1);
                first = first + 1;
                next = next + 1;
            }
            Id_game = Int32.Parse( individual_characteristic[0]);
            room = individual_characteristic[1];
            Id = individual_characteristic[2];
            Person_name = individual_characteristic[3];
            Age = individual_characteristic[4];
            Sex= individual_characteristic[5];
            Job = individual_characteristic[6];
            Hobby = individual_characteristic[7];
            Health = individual_characteristic[8];
            Baggage = individual_characteristic[9];
            Phobia = individual_characteristic[10];
            Character = individual_characteristic[11];
            Action_cards_one = individual_characteristic[12];
            Action_cards_two = individual_characteristic[13];
            Move = individual_characteristic[14];
        }
    }

    public class Errors
    {

    }

}
