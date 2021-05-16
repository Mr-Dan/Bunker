using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BunkerClient
{
    public partial class Form3 : Form
    {
        Form1 F1 = new Form1();
        public string codeRoomRand = null;

        TcpClient clientSocket = new TcpClient();
        NetworkStream serverStream = null;

        string readData= "start the client";

        public string id_client ;
        public string id_name ;

        public string save_login;
        public string save_passport;
        public bool save_flag = false;
        public Thread conn_Thread;
        public Thread Registration_Thread;
        public Thread conn_after_the_game_Thread;
        public Form3()
        {
            InitializeComponent();
        }
        private void conn()
        {

            try
            {
                msg();
                clientSocket.Connect(IPAddress.Parse(ip_text.Text), Int32.Parse(port_text.Text));
                serverStream = clientSocket.GetStream();            
                string Message = "LOGIN__CLIENT" + " {" + login_text.Text + "}" + "{" + Password_text.Text + "}";
                int Messagesize = Encoding.UTF8.GetByteCount(Message);

                byte[] outStream = new byte[Messagesize];
                outStream = Encoding.UTF8.GetBytes(Message);
                serverStream.Write(outStream, 0, outStream.Length);
                Thread ctThread = new Thread(getMessage);
                ctThread.Start();


            }
            catch (Exception ex)
            {
                if (clientSocket == null) clientSocket.Close();

                if (clientSocket.Connected == false)
                    MessageBox.Show("Сервер не работает ", "Ошибка", MessageBoxButtons.OK);
                connect_room.Enabled = true;

            }
        }
        private void conn_after_the_game()
        {
            login_text.Text = save_login;
            Password_text.Text = save_passport;
            if (save_flag == true)
            {
                try
                {

                    msg();
                    clientSocket.Connect(IPAddress.Parse(ip_text.Text), Int32.Parse(port_text.Text));
                    serverStream = clientSocket.GetStream();
                    // LOGIN__CLIENT->LOGIN = PASSWORD =

                    string Message = "LOGIN__CLIENT" + " {" + save_login + "}" + "{" + save_passport + "}";
                    int Messagesize = Encoding.UTF8.GetByteCount(Message);

                    byte[] outStream = new byte[Messagesize];
                    outStream = Encoding.UTF8.GetBytes(Message);
                    serverStream.Write(outStream, 0, outStream.Length);

                    Thread ctThread = new Thread(getMessage);
                    ctThread.Start();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    if (clientSocket == null)
                        clientSocket.Close();

                    if (clientSocket.Connected == false)  // Port is in use and connection is successful
                        MessageBox.Show("Сервер не работает ", "Ошибка", MessageBoxButtons.OK);
                    connect_room.Enabled = true;
                }
            }
        }
        private void Registration()
        {
            
                
                try
                {
                    readData = "Conected to Chat Server ...";
                    msg();
                    clientSocket.Connect(IPAddress.Parse(ip_text.Text), Int32.Parse(port_text.Text));
                    serverStream = clientSocket.GetStream();
                    // LOGIN__CLIENT->LOGIN = PASSWORD =
                    string Message = "REGIST_CLIENT" + " {" + login_text.Text + "}" + "{" + Password_text.Text + "}" + "{" + name_text.Text + "}";

                    int Messagesize = Encoding.UTF8.GetByteCount(Message);
                    byte[] outStream = new byte[Messagesize];
                    outStream = Encoding.UTF8.GetBytes(Message);
                    serverStream.Write(outStream, 0, outStream.Length);

                    Thread ctThread = new Thread(getMessage);
                    ctThread.Start();

                }
                catch (Exception ex)
                {
                    if (clientSocket == null) clientSocket.Close();

                    if (clientSocket.Connected == false)
                        MessageBox.Show("Сервер не работает ", "Ошибка", MessageBoxButtons.OK);
                    connect_room.Enabled = true;

                }
           
        }
        public void menu_auth(string login, string password,bool flag)
        {
            save_login = login;
            save_passport = password;
            save_flag = flag;


             conn_after_the_game_Thread = new Thread(new ThreadStart(conn_after_the_game));
            conn_after_the_game_Thread.Start();

            
        }

        private void connect_room_Click(object sender, EventArgs e)
        {
            string status = "connect_room_id";
            string permission = "player";

            if (serverStream != null && clientSocket.Connected == true)
            {
                int Messagesize = Encoding.UTF8.GetByteCount("LOGIN_DISCONN вышел из меню");
              
                byte[] outStream = new byte[Messagesize];
                outStream = Encoding.UTF8.GetBytes("LOGIN_DISCONN вышел из меню");
                serverStream.Write(outStream, 0, outStream.Length);

            }

            if (clientSocket.Connected != false) clientSocket.Close();
            if (serverStream != null) serverStream.Close();
            codeRoomRand = code_text.Text;

            F1.Show();
            F1.PassValue(codeRoomRand, id_client, id_name, login_text.Text, Password_text.Text, ip_text.Text,port_text.Text,status, permission); 
            this.Hide();

        }

        private void Form3_FormClosing(object sender, FormClosingEventArgs e)
        {
           
            if (serverStream != null && clientSocket.Connected == true)
            {
                int Messagesize = Encoding.UTF8.GetByteCount("LOGIN_DISCONN вышел из меню");                
                byte[] outStream = new byte[Messagesize];
                outStream = Encoding.UTF8.GetBytes("LOGIN_DISCONN вышел из меню");
                serverStream.Write(outStream, 0, outStream.Length);

            }

          /* Под вопросом
           * 
           * if(conn_Thread != null) 
                if (conn_Thread.IsAlive == true) 
                {
                conn_Thread.Abort();
                conn_Thread.Join();
                }

            if (Registration_Thread != null)
                if (Registration_Thread.IsAlive == true)
                {
                Registration_Thread.Abort();
                Registration_Thread.Join();
                }

            if (conn_after_the_game_Thread != null)
                if (conn_after_the_game_Thread.IsAlive == true)
                {
                conn_after_the_game_Thread.Abort();
                conn_after_the_game_Thread.Join();
                } */

            if (clientSocket.Connected != false)clientSocket.Close();                   
            if (serverStream != null) serverStream.Close();
            Application.Exit();
        }

        public string IDCodeRoom()
        {
            string Code = null;
           

            Random rnd = new Random();
            for (int i = 0; i < 3; i++)
            {
                
                Code = Code + (char)(rnd.Next(97, 123));
            }

            return Code;
        }

        private void new_game_Click(object sender, EventArgs e)
        {
            string status = "new_room_id";
            string permission = "admin";
            if (serverStream != null && clientSocket.Connected == true)
            {
                int Messagesize = Encoding.UTF8.GetByteCount("LOGIN_DISCONN вышел из меню");

                byte[] outStream = new byte[Messagesize];
                outStream = Encoding.UTF8.GetBytes("LOGIN_DISCONN вышел из меню");
                serverStream.Write(outStream, 0, outStream.Length);

            }

            if (clientSocket.Connected != false) clientSocket.Close();

            if (serverStream != null) serverStream.Close();

            codeRoomRand = IDCodeRoom();

            F1.Show();
            F1.PassValue(codeRoomRand, id_client, id_name, login_text.Text, Password_text.Text, ip_text.Text, port_text.Text, status, permission);

            this.Hide();

        }

        private void Form3_Load(object sender, EventArgs e)
        {
           

            connect_room.Visible = false;
            new_game.Visible = false;
            name_text.Visible = false;
            name_label.Visible = false;
            back.Visible = false;
            code_text.Visible = false;
            code_label.Visible = false;
          
        }
        private void msg()
        {


            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(msg));
            }
            else
            {
                if (readData.Length > 0)
                {
                    if (readData.IndexOf("LOGIN_CONNECT", 0, 13) > -1)
                    {

                        textBox1.Text = textBox1.Text + Environment.NewLine + readData;
                        //LOGIN_CONNECT  Id name;
                        int first = 0;
                        int next = 0;
                        string[] Get_Info = new string[2];

                        for (int i = 0; i < 2; i++)
                        {
                            first = readData.IndexOf("{", next);
                            next = readData.IndexOf("}", first);
                            Get_Info[i] = readData.Substring(first + 1, next - first - 1);
                            first = first + 1;
                            next = next + 1;
                        }

                        
                        id_client = Get_Info[0];
                        id_name = Get_Info[1];

                        id_auth.Text = id_client;
                        name_auth.Text = id_name;

                        connect_room.Visible = true;
                        new_game.Visible = true;
                        textBox1.Visible = true;
                        code_text.Visible = true;
                        code_label.Visible = true;

                        login_text.Visible = false;
                        Authorization_label.Visible = false;
                        Password_label.Visible = false;
                        Password_text.Visible = false;
                        Authorization_button.Visible = false;
                        Registration_button.Visible = false;
                        back.Visible = false;
                        name_text.Visible = false;
                        name_label.Visible = false;
                    }
                    else
                    {

                    }
                }
            }
        }
        private void getMessage()
       
        { int  ReceiveBufferSize = 1024;

            try
            {
                while (true)
                {
                   

                        byte[] inStream = new byte[ReceiveBufferSize];
                        serverStream = clientSocket.GetStream();

                        serverStream.Read(inStream, 0, ReceiveBufferSize);
                        string returndata = Encoding.UTF8.GetString(inStream);
                        readData = "" + returndata;
                        readData = readData.Trim('\0');

                         msg();
                  

                }
            }
            catch(Exception ex)
            {

               
                clientSocket.Close();
                serverStream.Close();

            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
           
            conn_Thread = new Thread(new ThreadStart(conn));
            conn_Thread.Start(); // запускаем поток
            
        }

        private void Registration_button_Click(object sender, EventArgs e)
        {
            if (back.Visible == true)
            {
                 Registration_Thread = new Thread(new ThreadStart(Registration));
            Registration_Thread.Start(); // запускаем поток
            }
            else
            {
                Authorization_button.Visible = false;
                name_label.Visible = true;
                name_text.Visible = true;
                back.Visible = true;
            }
        }

        private void back_Click(object sender, EventArgs e)
        {
            Authorization_button.Visible = true;
            name_label.Visible = false;
            name_text.Visible = false;
            back.Visible = false;
        }

        
    }
}
