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

        string readData;

        public string id_client ;
        public string id_name ;

        public Form3()
        {
            InitializeComponent();
        }

        public void menu_auth(string login, string password,bool flag)
        {
            login_text.Text = login;
            Password_text.Text = password;
            if (flag == true) {
                try
                {

                    readData = "Conected to Chat Server ...";
                    msg();
                    clientSocket.Connect(IPAddress.Parse("188.233.49.10"), Int32.Parse("368"));

                    serverStream = clientSocket.GetStream();

                    // LOGIN__CLIENT->LOGIN = PASSWORD =

                    string Message = "LOGIN__CLIENT" + " LOGIN=" + login + " PASSWORD=" + password;


                    int Messagesize = Encoding.UTF8.GetByteCount(Message);

                  /*  byte[] outStreamsize = new byte[Messagesize];
                    outStreamsize = Encoding.UTF8.GetBytes(Messagesize.ToString());
                    serverStream.Write(outStreamsize, 0, outStreamsize.Length);*/

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

        private void connect_room_Click(object sender, EventArgs e)
        {
            string status = "connect_room_id";
            string permission = "player";

            if (serverStream != null && clientSocket.Connected == true)
            {
                int Messagesize = Encoding.UTF8.GetByteCount("LOGIN_DISCONN вышел из меню");

                /*byte[] outStreamsize = new byte[Messagesize];
                outStreamsize = Encoding.UTF8.GetBytes(Messagesize.ToString());
                serverStream.Write(outStreamsize, 0, outStreamsize.Length);*/

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
   
                /*byte[] outStreamsize = new byte[Messagesize];
                outStreamsize = Encoding.UTF8.GetBytes(Messagesize.ToString());
                serverStream.Write(outStreamsize, 0, outStreamsize.Length);*/

                byte[] outStream = new byte[Messagesize];
                outStream = Encoding.UTF8.GetBytes("LOGIN_DISCONN вышел из меню");
                serverStream.Write(outStream, 0, outStream.Length);

            }

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

                /*byte[] outStreamsize = new byte[Messagesize];
                outStreamsize = Encoding.UTF8.GetBytes(Messagesize.ToString());
                serverStream.Write(outStreamsize, 0, outStreamsize.Length);
                */
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
          //  textBox1.Visible = false;
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
                        int size = readData.Length;
                        int id_client_pos = readData.IndexOf("ID_CLIENT", 13);
                        int id_name_pos = readData.IndexOf("ID_NAME", 13);
                        id_client = readData.Substring(id_client_pos + 10, id_name_pos - id_client_pos - 10);
                        id_name = readData.Substring(id_name_pos + 8, size - id_name_pos - 8);

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
                }
            }
        }
        private void getMessage()
       
        { int  ReceiveBufferSize = 1024;

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
        private void button3_Click(object sender, EventArgs e)
        {
           
            try
            {
                readData = "Conected to Chat Server ...";
                msg();
                clientSocket.Connect(IPAddress.Parse("188.233.49.10"), Int32.Parse("368"));

                serverStream = clientSocket.GetStream();

                // LOGIN__CLIENT->LOGIN = PASSWORD =

                string Message = "LOGIN__CLIENT" + " LOGIN=" + login_text.Text + " PASSWORD=" + Password_text.Text;


                int Messagesize = Encoding.UTF8.GetByteCount(Message);

                /*byte[] outStreamsize = new byte[Messagesize];
                outStreamsize = Encoding.UTF8.GetBytes(Messagesize.ToString());
                serverStream.Write(outStreamsize, 0, outStreamsize.Length);*/

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
                //if (serverStream == null) serverStream.Close();
            }
        }

        private void Registration_button_Click(object sender, EventArgs e)
        {
            if (back.Visible == true)
            {
                try
                {
                    readData = "Conected to Chat Server ...";
                    msg();
                    clientSocket.Connect(IPAddress.Parse("188.233.49.10"), Int32.Parse("368"));

                    serverStream = clientSocket.GetStream();

                    // LOGIN__CLIENT->LOGIN = PASSWORD =

                    string Message = "REGIST_CLIENT" + " LOGIN=" + login_text.Text + " PASSWORD=" + Password_text.Text + " NAME=" +name_text.Text;


                    int Messagesize = Encoding.UTF8.GetByteCount(Message);

                   /* byte[] outStreamsize = new byte[Messagesize];
                    outStreamsize = Encoding.UTF8.GetBytes(Messagesize.ToString());
                    serverStream.Write(outStreamsize, 0, outStreamsize.Length);*/

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
                    //if (serverStream == null) serverStream.Close();
                }
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
