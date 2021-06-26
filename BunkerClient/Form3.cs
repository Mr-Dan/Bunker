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
    public partial class Main_menu : Form
    {
        Game_menu F1 = new Game_menu();
        public string codeRoomRand = null;

        TcpClient clientSocket = new TcpClient();
        NetworkStream serverStream = null;

        string readData= "start the client";

        public string id_client;
        public string id_name;
        public int rules_page = 0;
        public bool connect = true;
        public static object locker = new object();

        public static string save_login;
        public static string save_passport;
        public bool save_flag = false;
        public Thread conn_Thread;
        public Thread Registration_Thread;
        public Thread conn_after_the_game_Thread;
        public Main_menu()
        {
            InitializeComponent();
            this.DoubleBuffered = true;

        }
        private void conn()
        {

            try
            {
                clientSocket = new TcpClient();
                connect = true;
                msg();
                clientSocket.Connect(IPAddress.Parse(ip_text.Text.Trim()), Int32.Parse(port_text.Text.Trim()));
                serverStream = clientSocket.GetStream();            
                string Message = "LOGIN__CLIENT" + " {" + login_text.Text.Trim() + "}" + "{" + Password_text.Text.Trim() + "}";             
                send_Messege(Message, true);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Сервер не работает " + ex, "Ошибка", MessageBoxButtons.OK);
            }
        }
        private void conn_after_the_game()
        {
                     
            if (save_flag == true)
            {
                try
                {
                    clientSocket = new TcpClient();
                    connect = true;
                    msg();
                    clientSocket.Connect(IPAddress.Parse(ip_text.Text), Int32.Parse(port_text.Text));
                    serverStream = clientSocket.GetStream();

                    string Message = "LOGIN__CLIENT" + " {" + save_login + "}" + "{" + save_passport + "}";
                   
                    send_Messege(Message, true);
                }
                catch (Exception ex)
                {                                
                  MessageBox.Show("Сервер не работает "+ ex, "Ошибка", MessageBoxButtons.OK);               
                }
            }
        }
        private void Registration()
        {
                          
                try
                {

                    clientSocket = new TcpClient();
                    connect = true;
                    readData = "Conected to Chat Server ...";
                    msg();
                    clientSocket.Connect(IPAddress.Parse(ip_text.Text.Trim()), Int32.Parse(port_text.Text.Trim()));
                    serverStream = clientSocket.GetStream();

                    string Message = "REGIST_CLIENT" + " {" + login_text.Text.Trim() + "}" + "{" + Password_text.Text.Trim() + "}" + "{" + name_text.Text.Trim() + "}";
                
                    send_Messege(Message, true);
                }
                catch (Exception ex)
                {
                MessageBox.Show("Сервер не работает " + ex, "Ошибка", MessageBoxButtons.OK);
                }
           
        }
        public void menu_auth(string login, string password,bool flag)
        {
            save_login = login;
            save_passport = password;
            save_flag = flag;

            login_text.Text = save_login;
            Password_text.Text = save_passport;

            conn_after_the_game_Thread = new Thread(new ThreadStart(conn_after_the_game));
            conn_after_the_game_Thread.Start();

            
        }

        private void connect_room_Click(object sender, EventArgs e)
        {
            if (code_text.Text.Length>0 && code_text.Text.IndexOf(" ")== -1   )
            {
                string status = "connect_room_id";
                string permission = "player";

                if (serverStream != null && clientSocket.Connected == true)
                {
                    
                    string message = "LOGIN_DISCONN вышел из меню";
                    send_Messege(message, false);
                }

                if (clientSocket.Connected != false) clientSocket.Close();
                if (serverStream != null) serverStream.Close();
                codeRoomRand = code_text.Text;

                F1.Show();
                F1.PassValue(codeRoomRand, id_client, id_name, login_text.Text.Trim(), Password_text.Text.Trim(), ip_text.Text.Trim(), port_text.Text.Trim(), status, permission);
                this.Hide();
             }

        }

        private void Form3_FormClosing(object sender, FormClosingEventArgs e)
        {
           
            if (serverStream != null && clientSocket.Connected == true)
            {
               
                string message = "LOGIN_DISCONN вышел из меню";
                send_Messege(message, false);
            }

            if (clientSocket.Connected != false)clientSocket.Close();                   
            if (serverStream != null) serverStream.Close();
            Application.Exit();
        }

        public string IDCodeRoom()
        {
            string Code = null;
           

            Random rnd = new Random();
            for (int i = 0; i < 6; i++)
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
                

                string message = "LOGIN_DISCONN вышел из меню";
                send_Messege(message, false);
            }

            if (clientSocket.Connected != false) clientSocket.Close();
            if (serverStream != null) serverStream.Close();
            code_text.Text = code_text.Text.Trim();
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

            string get_line = rules_text(0);
            string[,] rules_array = new string[1, 2];
            int i = get_line.IndexOf("|");
            rules_array[0, 0] = get_line.Substring(0, i);
            rules_array[0, 1] = get_line.Substring(i + 1, get_line.Length - i-1);

            heading_label.Text = rules_array[0, 0];
            rules_label.Text = rules_array[0, 1];
        }
        private void msg()
        {


            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(msg));
            }
            else
            {
                if (readData.Length > 0)
                {
                    if (readData.IndexOf("LOGIN_CONNECT", 0, 13) > -1)
                    {
                        Thread.Sleep(300);
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
                        code_text.Visible = true;

                        login_text.Visible = false;
                        Authorization_label.Visible = false;
                        Password_label.Visible = false;
                        Password_text.Visible = false;
                        Authorization_button.Visible = false;
                        Registration_button.Visible = false;
                        back.Visible = false;
                        name_text.Visible = false;
                        name_label.Visible = false;

                        main_panel.Visible = false;
                    }
                    if (readData.IndexOf("LOGIN_DISCONN", 0, 13) > -1)
                    {
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
                        disconn_panel.Visible = true;
                        Error_label.Text = Get_Info[1];
                        connect = false;
                        serverStream.Close();
                        clientSocket.Close();
                        readData = "";
                    }
                }
            }
        }
        private void getMessage() 
        {
            try
            {
                while (true)
                {
                    byte[] inStream = new byte[4];
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
                        readData = readData.Trim('\0');
                        msg();
                    }
                    if (connect == false) break;
                }
            }
            catch(Exception ex)
            {

               
                clientSocket.Close();
                serverStream.Close();

            }
        }

        private void send_Messege(string message,bool flag)
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

        private void exit_button_Click(object sender, EventArgs e)
        {
            if (serverStream != null && clientSocket.Connected == true)
            {
                
                string message = "LOGIN_DISCONN вышел из меню";
                send_Messege(message, false);

            }

            if (clientSocket.Connected != false) clientSocket.Close();
            if (serverStream != null) serverStream.Close();
            Application.Exit();
        }

        private void rules_button_Click(object sender, EventArgs e)
        {
            if (rules_panel.Visible == false) rules_panel.Visible = true;
            else rules_panel.Visible = false;

        }

        private string rules_text(int page)
        {
            string[,] rules_array = new string[9,2];
            rules_array[0, 0] = "История";
            rules_array[0, 1] = "На земле вот-вот произойдет катастрофа, а может уже и началась!\n" +
            "Я, как и большинство людей в панике пытаюсь выжить, и найти укрытие, чтобы спасти свою жизнь.В поисках убежища я увидел группу людей, но не знал что делать... Стоит подойти к ним или нет, ведь опасность сейчас на каждом шагу!\n" +
            "Но так как терять было нечего я решил пойти к ним...\n" +
            "Как только я начал подходить к людям, я понял, что мне невероятно повезло — все эти люди находились возле бункера на который была последняя надежда!\n" +
            "Как оказалось они ждут людей, которым все еще удалось выжить, и разбили временный лагерь...\n" +
            "Приняли меня хорошо, но что меня ждет дальше мне было не известно.Все что я понимал, так это то, что предстоит решить, кто действительно заслуживает попасть в бункер, чтобы остаться в живых. Тех, кто не попадет - ждет верная смерть.\n" +
            "И тут началась моя история выживания...\n";

            rules_array[1, 0] = "Обзор";
            rules_array[1, 1] = "Катаклизм\n" +
           "Описание текущего для игры катаклизма. Как это произошло, что случилось и четкое понимание того, с чем связаны проблемы, что даст вам понять в процессе игры кто среди людей вам подходит, а кого нужно выгнать.\n" +
           "Бункер\n" +
           "Описание найденного бункера. Единственный шанс, чтобы выжить в случае катаклизма - это попасть в бункер. У вас есть информация о времени его постройки, местонахождении и данные о спальных комнатах.\n" +
           "Так же вам доступна следующая информация:\n" +
           "- Размер бункера — общая площадь убежища.\n" +
           "- Время нахождения — сколько вам потребуется времени, чтобы пережить катастрофу.\n" +
           "- В бункере имеется — те вещи, которые находятся внутри бункера и которые могут быть полезны при выживании.\n" +
           "В зависимости от того, что находится в бункере, вам так же предстоит определить кто из выживших будет более полезен, учитывая данные обстоятельства.\n" +
           "Описание персонажа\n" +
           "Описание вашего персонажа. Ваш герой состоит из следующих характеристик:\n" + 
           "-Пол\n" +
           "-Возраст\n" +
           "-Професия\n" +
           "-Хобби\n" +
           "-Здоровье\n" +
           "-Багаж\n" +
           "-Фобия\n" +
           "-Характер\n";

            rules_array[2, 0] = "Процесс игры";
            rules_array[2, 1] = "В первом игровом круге все начинается с представления друг друга\n" +
            "В процессе знакомства игрок раскрывает свой основной параметр — это специальность и характеристики в зависимости от количества игроков\n" +
            "На это дается каждому игроку по n минуте \n" +
            "После того как каждый игрок рассказал о своем параметре, игроки получают n минуту общего времени на коллективное обсуждение\n" +
            "После этого приходит время голосования. Игроки должны выбрать того, кто покинет временный лагерь и не попадет в бункер. Разглашать еще не открытые параметры в целях переубеждения игроков - запрещается\n" +
            "После голосования начинается следующий игровой круг\n" +
            "Во всех последующих раундах раскрывается только один параметр, вне зависимости от количества игроков.\n" +
            "В конце игры игроки, которые попали в бункер, раскрывают свои характеристики. Ведущий подводит итог\n";

            rules_array[3, 0] = "Количество игроков";
            rules_array[3, 1] = "Количество игроков:6-7. Могут попасть в бункер:3. Характеристик в первый раунд (исключая специальность):3\n" +
            "Количество игроков:8-9. Могут попасть в бункер:4. Характеристик в первый раунд (исключая специальность):3\n" +
            "Количество игроков:10-11. Могут попасть в бункер:5. Характеристик в первый раунд (исключая специальность):2\n" +
            "Количество игроков:12. Могут попасть в бункер:6. Характеристик в первый раунд (исключая специальность):1\n";

            rules_array[4, 0] = "Ваш ход";
            rules_array[4, 1] = "Ваш ход — самое время блеснуть! Вам дали время представить своего персонажа.\n" +
            "Делайте игру интересной, включайте фантазию на максимум и расскажите свою историю!\n" +
            "Не нужно просто зачитывать свои характеристики без эмоций, так игра будет не интересной.\n" +
            "Помните — вы любым образом должны попасть в бункер!\n" +
            "В зависимости от количества игроков, в первый раунд игры вы открываете нужное количество характеристик.\n" +
            "Во все последующие ходы вы открываете по одной характеристике за ход (раунд).\n";

            rules_array[5, 0] = "Голосование";
            rules_array[5, 1] = "В игре есть голосование за исключение человека из временного лагеря.\n" +
            "Голосование - обязательный процесс игры. Оно проводится в конце игрового раунда и оглашает голосование ведущий игры, он же лидер в лагере.\n" +
            "Если большинство игроков решили не голосовать (например недостаточно информации об игроках) в текущем раунде — голосование автоматически\n" +
            "объявляется закрытым, и игроки переходят в следующий раунд. В таком случае, в следующем раунде после окончания коллективного обсуждения,\n" +
            "игроки обязательно должны провести голосование. Процесс голосования будет проходить 2 раза, чтобы выгнать сразу 2‑х игроков. Переход в новый раунд без исключения игроков — невозможен.\n" +
            "Если игроки решили провести голосование, тогда ведущий игры дает каждому игроку по 30 сек. на высказывание перед тем как запустить голосование\n" +
            "Время на высказывание вы можете потратить на объяснения за кого нужно проголосовать или же дополнительно защитить себя от голосов против вас, так же вы можете просто дать общий комментарий.\n" +
            "Игрок может отказаться от высказывания, в таком случае слово переходит к следующему игроку без дополнительного времени.\n" +
            "На голосование у игроков есть 15 сек.\n" +
            "Когда все голоса собраны есть несколько вариантов развития событий:\n" +
            "1) Один игрок набрал больше чем всех голосов — исключение из лагеря без оправдания.\n" +
            "2) Несколько игроков получили наибольшее одинаковое количество голосов — каждый из этих игроков получает 30 сек. на оправдание.\n" +
            "После завершения оправданий каждого из игроков ведущий игры начинает новое голосование с такими же правилами.\n" +
            "После финального завершения голосования (когда кандидаты на исключение определены) ведущий игры объявляет прощальную речь для каждого из игроков, который покидает временный лагерь.\n" +
            "На прощальную речь у каждого игрока есть 15 сек. За это время игрок или другие игроки могут использовать спец. возможности и возможности от специальности.\n" +
            "После завершения 15 сек. игрок покидает временный лагерь и больше не может принимать участие в игре (раскрывать характеристики или использовать любые игровые возможности). Не стоит забывать о том, что в игре есть возможность вернуть игрока в лагерь.\n";


            rules_array[6, 0] = "Коллективное обсуждение";
            rules_array[6, 1] = "Во время общего обсуждения каждый игрок имеет возможность что-то сказать. Здесь нет ограничений на каждого человека. Общее обсуждение длится 1 минуту.\n";

            rules_array[7, 0] = "Победа в игре";
            rules_array[7, 1] = "По завершению последнего голосования, когда определилось нужное количество людей для прохождения в бункер, игра завершается. Игроки, которые попали в бункер и переживут катаклизм - считаются победителями!\n" +
            "Остальные игроки так и остаются возле лагеря поскольку искать другой бункер - нет смысла, ведь ситуация с катаклизмом совсем критическая.\n" +
            "Некоторые потеряли надежду и решили погибнуть на месте, часть все-таки решила испытать удачу и попытаются выжить без бункера...\n";

            rules_array[8, 0] = "Характеристики персонажа";
            rules_array[8, 1] = "У каждого персонажа есть несколько характеристик, которые он получает в начале игры.\n" +
            "Любой из параметров можно изменить используя специальную возможность, которая это позволяет сделать.\n" +
            "Специальность — есть специальности, которые дают персонажу дополнительные бонусы. Их можно использовать в любой момент игры или в зависимости от описания действия\n" +
            "У каждой специальности есть свой стаж работы:\n" +
            " Дилетант — до 1 месяца\n" +
            " Стажер — от 1 до 3 месяцев\n" +
            " Новичок — от 4 месяцев до 1 года\n" +
            " Любитель — от 1 до 2 лет\n" +
            " Опытный — от 2 до 5 лет\n" +
            " Эксперт — от 5 до 10 лет\n" +
            " Профессионал — от 10 лет\n" +
            "Чайлдфри — человек, который сознательно не желает заводить детей.\n" +
            "Если женщина в возрасте 49 лет или больше - она не может иметь детей\n" +
            "Если мужчина в возрасте 75 лет и больше - он не может иметь детей.\n" +
            "Также, если возраст мужчины больше 35 лет шанс того, что ребенок будет здоровый уменьшается.\n" +
            "Присутствуют следующие этапы жизни: молодой, взрослый и пожилой.\n" +
            "У характеристики 'Здоровье' есть разные степени проблем: легкая, средняя, тяжелая и критическая.\n" +
            "Уровень хобби оценивается следующим образом:\n" +
            " Дилетант — до 1 месяца\n" +
            " Начинающий — от 1 до 4 месяцев\n" +
            " Новичок — от 5 месяцев до 1 года\n" +
            " Любитель — от 1 до 2 лет\n" +
            " Продвинутый — от 2 до 5 лет\n" +
            " Мастер (гуру) — больше 5 лет\n";


            return rules_array[page, 0] + "|" +rules_array[page, 1];
        }

        private void button_next_Click(object sender, EventArgs e)
        {
            
            rules_page++;
            string get_line = rules_text(rules_page);
            string[,] rules_array = new string[1, 2];
            int i = get_line.IndexOf("|");
            rules_array[0, 0] = get_line.Substring(0, i);
            rules_array[0, 1] = get_line.Substring(i+1, get_line.Length -i-1);

            heading_label.Text = rules_array[0, 0];
            rules_label.Text = rules_array[0, 1];

            if (rules_page > 0) button_prev.Visible = true;
            if (rules_page + 1 > 8) button_next.Visible = false;
        }

        private void button_prev_Click(object sender, EventArgs e)
        {
            rules_page--;
            string get_line = rules_text(rules_page);
            string[,] rules_array = new string[1, 2];
            int i = get_line.IndexOf("|");
            rules_array[0, 0] = get_line.Substring(0, i);
            rules_array[0, 1] = get_line.Substring(i + 1, get_line.Length - i - 1);

            heading_label.Text = rules_array[0, 0];
            rules_label.Text = rules_array[0, 1];
            if (rules_page == 0) button_prev.Visible = false;
            if (rules_page + 1 <= 8) button_next.Visible = true;
        }
    

        private void about_the_game_button_Click(object sender, EventArgs e)
        {
            
            if (about_the_game_panel.Visible == false) about_the_game_panel.Visible = true;
            else about_the_game_panel.Visible = false;
        }

        private void Error_button_Click(object sender, EventArgs e)
        {
            disconn_panel.Visible = false;
        }
    }
}
