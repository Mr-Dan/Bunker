namespace BunkerClient
{
    partial class Main_menu
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main_menu));
            this.connect_room = new System.Windows.Forms.Button();
            this.new_game = new System.Windows.Forms.Button();
            this.Authorization_button = new System.Windows.Forms.Button();
            this.login_text = new System.Windows.Forms.TextBox();
            this.Password_text = new System.Windows.Forms.TextBox();
            this.Authorization_label = new System.Windows.Forms.Label();
            this.Password_label = new System.Windows.Forms.Label();
            this.Registration_button = new System.Windows.Forms.Button();
            this.name_text = new System.Windows.Forms.TextBox();
            this.name_label = new System.Windows.Forms.Label();
            this.back = new System.Windows.Forms.Button();
            this.name_auth = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.id_auth = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.port_text = new System.Windows.Forms.TextBox();
            this.ip_text = new System.Windows.Forms.TextBox();
            this.code_text = new System.Windows.Forms.TextBox();
            this.exit_button = new System.Windows.Forms.Button();
            this.main_panel = new System.Windows.Forms.Panel();
            this.rules_button = new System.Windows.Forms.Button();
            this.person_panel = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.about_the_game_button = new System.Windows.Forms.Button();
            this.rules_panel = new System.Windows.Forms.Panel();
            this.heading_label = new System.Windows.Forms.Label();
            this.button_next = new System.Windows.Forms.Button();
            this.button_prev = new System.Windows.Forms.Button();
            this.rules_label = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.about_the_game_panel = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.disconn_panel = new System.Windows.Forms.Panel();
            this.Error_button = new System.Windows.Forms.Button();
            this.Error_label = new System.Windows.Forms.Label();
            this.main_panel.SuspendLayout();
            this.person_panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.rules_panel.SuspendLayout();
            this.about_the_game_panel.SuspendLayout();
            this.disconn_panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // connect_room
            // 
            this.connect_room.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(66)))));
            this.connect_room.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.connect_room.Font = new System.Drawing.Font("Elephant", 8.999999F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.connect_room.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(142)))), ((int)(((byte)(130)))));
            this.connect_room.Location = new System.Drawing.Point(36, 610);
            this.connect_room.Name = "connect_room";
            this.connect_room.Size = new System.Drawing.Size(170, 50);
            this.connect_room.TabIndex = 0;
            this.connect_room.Text = "Подключится к комнате";
            this.connect_room.UseVisualStyleBackColor = false;
            this.connect_room.Click += new System.EventHandler(this.connect_room_Click);
            // 
            // new_game
            // 
            this.new_game.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(66)))));
            this.new_game.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.new_game.Font = new System.Drawing.Font("Elephant", 8.999999F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.new_game.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(142)))), ((int)(((byte)(130)))));
            this.new_game.Location = new System.Drawing.Point(36, 666);
            this.new_game.Name = "new_game";
            this.new_game.Size = new System.Drawing.Size(170, 50);
            this.new_game.TabIndex = 1;
            this.new_game.Text = "Создать Новую Комнату";
            this.new_game.UseVisualStyleBackColor = false;
            this.new_game.Click += new System.EventHandler(this.new_game_Click);
            // 
            // Authorization_button
            // 
            this.Authorization_button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(66)))));
            this.Authorization_button.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.Authorization_button.Font = new System.Drawing.Font("Elephant", 8.999999F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.Authorization_button.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(142)))), ((int)(((byte)(130)))));
            this.Authorization_button.Location = new System.Drawing.Point(16, 108);
            this.Authorization_button.Name = "Authorization_button";
            this.Authorization_button.Size = new System.Drawing.Size(160, 25);
            this.Authorization_button.TabIndex = 3;
            this.Authorization_button.Text = "Авторизоваться";
            this.Authorization_button.UseVisualStyleBackColor = false;
            this.Authorization_button.Click += new System.EventHandler(this.button3_Click);
            // 
            // login_text
            // 
            this.login_text.Location = new System.Drawing.Point(16, 22);
            this.login_text.Name = "login_text";
            this.login_text.Size = new System.Drawing.Size(100, 20);
            this.login_text.TabIndex = 4;
            this.login_text.WordWrap = false;
            // 
            // Password_text
            // 
            this.Password_text.Location = new System.Drawing.Point(16, 48);
            this.Password_text.Name = "Password_text";
            this.Password_text.Size = new System.Drawing.Size(100, 20);
            this.Password_text.TabIndex = 5;
            this.Password_text.WordWrap = false;
            // 
            // Authorization_label
            // 
            this.Authorization_label.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(157)))), ((int)(((byte)(157)))), ((int)(((byte)(161)))));
            this.Authorization_label.Font = new System.Drawing.Font("Elephant", 8.999999F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.Authorization_label.Location = new System.Drawing.Point(122, 29);
            this.Authorization_label.Name = "Authorization_label";
            this.Authorization_label.Size = new System.Drawing.Size(60, 15);
            this.Authorization_label.TabIndex = 6;
            this.Authorization_label.Text = "Логин";
            // 
            // Password_label
            // 
            this.Password_label.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(157)))), ((int)(((byte)(157)))), ((int)(((byte)(161)))));
            this.Password_label.Font = new System.Drawing.Font("Elephant", 8.999999F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.Password_label.Location = new System.Drawing.Point(122, 55);
            this.Password_label.Name = "Password_label";
            this.Password_label.Size = new System.Drawing.Size(60, 15);
            this.Password_label.TabIndex = 7;
            this.Password_label.Text = "Пороль";
            // 
            // Registration_button
            // 
            this.Registration_button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(66)))));
            this.Registration_button.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.Registration_button.Font = new System.Drawing.Font("Elephant", 8.999999F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.Registration_button.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(142)))), ((int)(((byte)(130)))));
            this.Registration_button.Location = new System.Drawing.Point(16, 137);
            this.Registration_button.Name = "Registration_button";
            this.Registration_button.Size = new System.Drawing.Size(160, 25);
            this.Registration_button.TabIndex = 8;
            this.Registration_button.Text = "Регистрация";
            this.Registration_button.UseVisualStyleBackColor = false;
            this.Registration_button.Click += new System.EventHandler(this.Registration_button_Click);
            // 
            // name_text
            // 
            this.name_text.Location = new System.Drawing.Point(16, 74);
            this.name_text.Name = "name_text";
            this.name_text.Size = new System.Drawing.Size(100, 20);
            this.name_text.TabIndex = 9;
            this.name_text.WordWrap = false;
            // 
            // name_label
            // 
            this.name_label.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(157)))), ((int)(((byte)(157)))), ((int)(((byte)(161)))));
            this.name_label.Font = new System.Drawing.Font("Elephant", 8.999999F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.name_label.Location = new System.Drawing.Point(122, 78);
            this.name_label.Name = "name_label";
            this.name_label.Size = new System.Drawing.Size(60, 15);
            this.name_label.TabIndex = 10;
            this.name_label.Text = "имя";
            // 
            // back
            // 
            this.back.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(66)))));
            this.back.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.back.Font = new System.Drawing.Font("Elephant", 8.999999F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.back.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(142)))), ((int)(((byte)(130)))));
            this.back.Location = new System.Drawing.Point(16, 168);
            this.back.Name = "back";
            this.back.Size = new System.Drawing.Size(160, 25);
            this.back.TabIndex = 11;
            this.back.Text = "Назад";
            this.back.UseVisualStyleBackColor = false;
            this.back.Click += new System.EventHandler(this.back_Click);
            // 
            // name_auth
            // 
            this.name_auth.AutoSize = true;
            this.name_auth.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(142)))), ((int)(((byte)(130)))));
            this.name_auth.Location = new System.Drawing.Point(47, 157);
            this.name_auth.Name = "name_auth";
            this.name_auth.Size = new System.Drawing.Size(0, 13);
            this.name_auth.TabIndex = 12;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(142)))), ((int)(((byte)(130)))));
            this.label2.Location = new System.Drawing.Point(3, 157);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(27, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "имя";
            // 
            // id_auth
            // 
            this.id_auth.AutoSize = true;
            this.id_auth.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(142)))), ((int)(((byte)(130)))));
            this.id_auth.Location = new System.Drawing.Point(47, 191);
            this.id_auth.Name = "id_auth";
            this.id_auth.Size = new System.Drawing.Size(0, 13);
            this.id_auth.TabIndex = 14;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(142)))), ((int)(((byte)(130)))));
            this.label4.Location = new System.Drawing.Point(3, 191);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(15, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "id";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(66)))));
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(142)))), ((int)(((byte)(130)))));
            this.label1.Location = new System.Drawing.Point(31, 64);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 23);
            this.label1.TabIndex = 19;
            this.label1.Text = "Port";
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(66)))));
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(142)))), ((int)(((byte)(130)))));
            this.label3.Location = new System.Drawing.Point(31, 32);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 23);
            this.label3.TabIndex = 18;
            this.label3.Text = "IP";
            // 
            // port_text
            // 
            this.port_text.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(66)))));
            this.port_text.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.port_text.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(142)))), ((int)(((byte)(130)))));
            this.port_text.Location = new System.Drawing.Point(85, 64);
            this.port_text.Name = "port_text";
            this.port_text.Size = new System.Drawing.Size(37, 20);
            this.port_text.TabIndex = 17;
            this.port_text.Text = "368";
            this.port_text.WordWrap = false;
            // 
            // ip_text
            // 
            this.ip_text.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(66)))));
            this.ip_text.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ip_text.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(142)))), ((int)(((byte)(130)))));
            this.ip_text.Location = new System.Drawing.Point(69, 35);
            this.ip_text.Name = "ip_text";
            this.ip_text.Size = new System.Drawing.Size(100, 20);
            this.ip_text.TabIndex = 16;
            this.ip_text.Text = "188.233.43.141";
            this.ip_text.WordWrap = false;
            // 
            // code_text
            // 
            this.code_text.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(66)))));
            this.code_text.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.code_text.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.code_text.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(142)))), ((int)(((byte)(130)))));
            this.code_text.Location = new System.Drawing.Point(36, 585);
            this.code_text.Multiline = true;
            this.code_text.Name = "code_text";
            this.code_text.Size = new System.Drawing.Size(170, 20);
            this.code_text.TabIndex = 20;
            this.code_text.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.code_text.WordWrap = false;
            // 
            // exit_button
            // 
            this.exit_button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(66)))));
            this.exit_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.exit_button.Font = new System.Drawing.Font("Elephant", 8.999999F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.exit_button.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(142)))), ((int)(((byte)(130)))));
            this.exit_button.Location = new System.Drawing.Point(36, 814);
            this.exit_button.Name = "exit_button";
            this.exit_button.Size = new System.Drawing.Size(170, 40);
            this.exit_button.TabIndex = 22;
            this.exit_button.Text = "Выйти";
            this.exit_button.UseVisualStyleBackColor = false;
            this.exit_button.Click += new System.EventHandler(this.exit_button_Click);
            // 
            // main_panel
            // 
            this.main_panel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(157)))), ((int)(((byte)(157)))), ((int)(((byte)(161)))));
            this.main_panel.Controls.Add(this.Authorization_button);
            this.main_panel.Controls.Add(this.login_text);
            this.main_panel.Controls.Add(this.Password_text);
            this.main_panel.Controls.Add(this.Authorization_label);
            this.main_panel.Controls.Add(this.Password_label);
            this.main_panel.Controls.Add(this.Registration_button);
            this.main_panel.Controls.Add(this.name_text);
            this.main_panel.Controls.Add(this.name_label);
            this.main_panel.Controls.Add(this.back);
            this.main_panel.Location = new System.Drawing.Point(550, 375);
            this.main_panel.Name = "main_panel";
            this.main_panel.Size = new System.Drawing.Size(200, 210);
            this.main_panel.TabIndex = 23;
            // 
            // rules_button
            // 
            this.rules_button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(66)))));
            this.rules_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rules_button.Font = new System.Drawing.Font("Elephant", 8.999999F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rules_button.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(142)))), ((int)(((byte)(130)))));
            this.rules_button.Location = new System.Drawing.Point(36, 722);
            this.rules_button.Name = "rules_button";
            this.rules_button.Size = new System.Drawing.Size(170, 40);
            this.rules_button.TabIndex = 24;
            this.rules_button.Text = "Правила";
            this.rules_button.UseVisualStyleBackColor = false;
            this.rules_button.Click += new System.EventHandler(this.rules_button_Click);
            // 
            // person_panel
            // 
            this.person_panel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(66)))));
            this.person_panel.Controls.Add(this.pictureBox1);
            this.person_panel.Controls.Add(this.id_auth);
            this.person_panel.Controls.Add(this.label4);
            this.person_panel.Controls.Add(this.label2);
            this.person_panel.Controls.Add(this.name_auth);
            this.person_panel.Location = new System.Drawing.Point(1122, 36);
            this.person_panel.Name = "person_panel";
            this.person_panel.Size = new System.Drawing.Size(141, 226);
            this.person_panel.TabIndex = 25;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.BackgroundImage")));
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(140, 140);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // about_the_game_button
            // 
            this.about_the_game_button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(66)))));
            this.about_the_game_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.about_the_game_button.Font = new System.Drawing.Font("Elephant", 8.999999F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.about_the_game_button.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(142)))), ((int)(((byte)(130)))));
            this.about_the_game_button.Location = new System.Drawing.Point(36, 768);
            this.about_the_game_button.Name = "about_the_game_button";
            this.about_the_game_button.Size = new System.Drawing.Size(170, 40);
            this.about_the_game_button.TabIndex = 25;
            this.about_the_game_button.Text = "О игре";
            this.about_the_game_button.UseVisualStyleBackColor = false;
            this.about_the_game_button.Click += new System.EventHandler(this.about_the_game_button_Click);
            // 
            // rules_panel
            // 
            this.rules_panel.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.rules_panel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("rules_panel.BackgroundImage")));
            this.rules_panel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.rules_panel.Controls.Add(this.heading_label);
            this.rules_panel.Controls.Add(this.button_next);
            this.rules_panel.Controls.Add(this.button_prev);
            this.rules_panel.Controls.Add(this.rules_label);
            this.rules_panel.Controls.Add(this.label5);
            this.rules_panel.Location = new System.Drawing.Point(300, 275);
            this.rules_panel.Name = "rules_panel";
            this.rules_panel.Size = new System.Drawing.Size(780, 580);
            this.rules_panel.TabIndex = 27;
            this.rules_panel.Visible = false;
            // 
            // heading_label
            // 
            this.heading_label.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(186)))), ((int)(((byte)(151)))));
            this.heading_label.Font = new System.Drawing.Font("Elephant", 9.75F, System.Drawing.FontStyle.Bold);
            this.heading_label.Location = new System.Drawing.Point(295, 43);
            this.heading_label.Name = "heading_label";
            this.heading_label.Size = new System.Drawing.Size(141, 30);
            this.heading_label.TabIndex = 4;
            this.heading_label.Text = "Заголовок";
            // 
            // button_next
            // 
            this.button_next.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(66)))));
            this.button_next.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_next.Font = new System.Drawing.Font("Elephant", 8.999999F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_next.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(142)))), ((int)(((byte)(130)))));
            this.button_next.Location = new System.Drawing.Point(682, 518);
            this.button_next.Name = "button_next";
            this.button_next.Size = new System.Drawing.Size(75, 47);
            this.button_next.TabIndex = 3;
            this.button_next.Text = "вперед";
            this.button_next.UseVisualStyleBackColor = false;
            this.button_next.Click += new System.EventHandler(this.button_next_Click);
            // 
            // button_prev
            // 
            this.button_prev.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(66)))));
            this.button_prev.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_prev.Font = new System.Drawing.Font("Elephant", 8.999999F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_prev.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(142)))), ((int)(((byte)(130)))));
            this.button_prev.Location = new System.Drawing.Point(21, 518);
            this.button_prev.Name = "button_prev";
            this.button_prev.Size = new System.Drawing.Size(75, 47);
            this.button_prev.TabIndex = 2;
            this.button_prev.Text = "Назад";
            this.button_prev.UseVisualStyleBackColor = false;
            this.button_prev.Visible = false;
            this.button_prev.Click += new System.EventHandler(this.button_prev_Click);
            // 
            // rules_label
            // 
            this.rules_label.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(187)))), ((int)(((byte)(151)))));
            this.rules_label.Font = new System.Drawing.Font("Elephant", 9.75F, System.Drawing.FontStyle.Bold);
            this.rules_label.Location = new System.Drawing.Point(18, 87);
            this.rules_label.MaximumSize = new System.Drawing.Size(750, 550);
            this.rules_label.Name = "rules_label";
            this.rules_label.Size = new System.Drawing.Size(739, 392);
            this.rules_label.TabIndex = 1;
            this.rules_label.Text = "Правила";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(168)))), ((int)(((byte)(128)))));
            this.label5.Font = new System.Drawing.Font("Elephant", 9.75F, System.Drawing.FontStyle.Bold);
            this.label5.Location = new System.Drawing.Point(281, 13);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(170, 17);
            this.label5.TabIndex = 0;
            this.label5.Text = "Правила игры Бункер";
            // 
            // about_the_game_panel
            // 
            this.about_the_game_panel.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.about_the_game_panel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("about_the_game_panel.BackgroundImage")));
            this.about_the_game_panel.Controls.Add(this.label6);
            this.about_the_game_panel.Location = new System.Drawing.Point(299, 36);
            this.about_the_game_panel.Name = "about_the_game_panel";
            this.about_the_game_panel.Size = new System.Drawing.Size(778, 218);
            this.about_the_game_panel.TabIndex = 28;
            this.about_the_game_panel.Visible = false;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(186)))), ((int)(((byte)(151)))));
            this.label6.Font = new System.Drawing.Font("Elephant", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(21, 12);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(736, 192);
            this.label6.TabIndex = 0;
            this.label6.Text = resources.GetString("label6.Text");
            // 
            // disconn_panel
            // 
            this.disconn_panel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("disconn_panel.BackgroundImage")));
            this.disconn_panel.Controls.Add(this.Error_button);
            this.disconn_panel.Controls.Add(this.Error_label);
            this.disconn_panel.Location = new System.Drawing.Point(500, 375);
            this.disconn_panel.Name = "disconn_panel";
            this.disconn_panel.Size = new System.Drawing.Size(300, 200);
            this.disconn_panel.TabIndex = 29;
            this.disconn_panel.Visible = false;
            // 
            // Error_button
            // 
            this.Error_button.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(82)))), ((int)(((byte)(79)))), ((int)(((byte)(83)))));
            this.Error_button.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.Error_button.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(156)))), ((int)(((byte)(142)))), ((int)(((byte)(130)))));
            this.Error_button.Location = new System.Drawing.Point(209, 163);
            this.Error_button.Name = "Error_button";
            this.Error_button.Size = new System.Drawing.Size(75, 23);
            this.Error_button.TabIndex = 1;
            this.Error_button.Text = "Закрыть";
            this.Error_button.UseVisualStyleBackColor = false;
            this.Error_button.Click += new System.EventHandler(this.Error_button_Click);
            // 
            // Error_label
            // 
            this.Error_label.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(187)))), ((int)(((byte)(151)))));
            this.Error_label.Font = new System.Drawing.Font("Elephant", 9.75F, System.Drawing.FontStyle.Bold);
            this.Error_label.Location = new System.Drawing.Point(30, 38);
            this.Error_label.Name = "Error_label";
            this.Error_label.Size = new System.Drawing.Size(236, 122);
            this.Error_label.TabIndex = 0;
            // 
            // Main_menu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(1284, 912);
            this.Controls.Add(this.about_the_game_button);
            this.Controls.Add(this.disconn_panel);
            this.Controls.Add(this.rules_button);
            this.Controls.Add(this.connect_room);
            this.Controls.Add(this.new_game);
            this.Controls.Add(this.code_text);
            this.Controls.Add(this.about_the_game_panel);
            this.Controls.Add(this.rules_panel);
            this.Controls.Add(this.person_panel);
            this.Controls.Add(this.main_panel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.exit_button);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.port_text);
            this.Controls.Add(this.ip_text);
            this.Name = "Main_menu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Меню";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form3_FormClosing);
            this.Load += new System.EventHandler(this.Form3_Load);
            this.main_panel.ResumeLayout(false);
            this.main_panel.PerformLayout();
            this.person_panel.ResumeLayout(false);
            this.person_panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.rules_panel.ResumeLayout(false);
            this.rules_panel.PerformLayout();
            this.about_the_game_panel.ResumeLayout(false);
            this.disconn_panel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button connect_room;
        private System.Windows.Forms.Button new_game;
        private System.Windows.Forms.Button Authorization_button;
        private System.Windows.Forms.TextBox login_text;
        private System.Windows.Forms.TextBox Password_text;
        private System.Windows.Forms.Label Authorization_label;
        private System.Windows.Forms.Label Password_label;
        private System.Windows.Forms.Button Registration_button;
        private System.Windows.Forms.TextBox name_text;
        private System.Windows.Forms.Label name_label;
        private System.Windows.Forms.Button back;
        private System.Windows.Forms.Label name_auth;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label id_auth;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.TextBox port_text;
        public System.Windows.Forms.TextBox ip_text;
        private System.Windows.Forms.TextBox code_text;
        private System.Windows.Forms.Button exit_button;
        private System.Windows.Forms.Panel main_panel;
        private System.Windows.Forms.Button rules_button;
        private System.Windows.Forms.Panel person_panel;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel rules_panel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label rules_label;
        private System.Windows.Forms.Button button_next;
        private System.Windows.Forms.Button button_prev;
        private System.Windows.Forms.Label heading_label;
        private System.Windows.Forms.Button about_the_game_button;
        private System.Windows.Forms.Panel about_the_game_panel;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel disconn_panel;
        private System.Windows.Forms.Label Error_label;
        private System.Windows.Forms.Button Error_button;
    }
}