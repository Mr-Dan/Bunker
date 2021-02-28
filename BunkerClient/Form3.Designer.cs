namespace BunkerClient
{
    partial class Form3
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
            this.components = new System.ComponentModel.Container();
            this.connect_room = new System.Windows.Forms.Button();
            this.new_game = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.textBox1 = new System.Windows.Forms.TextBox();
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
            this.code_label = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // connect_room
            // 
            this.connect_room.Location = new System.Drawing.Point(36, 551);
            this.connect_room.Name = "connect_room";
            this.connect_room.Size = new System.Drawing.Size(168, 23);
            this.connect_room.TabIndex = 0;
            this.connect_room.Text = "Подключится к комнате";
            this.connect_room.UseVisualStyleBackColor = true;
            this.connect_room.Click += new System.EventHandler(this.connect_room_Click);
            // 
            // new_game
            // 
            this.new_game.Location = new System.Drawing.Point(36, 603);
            this.new_game.Name = "new_game";
            this.new_game.Size = new System.Drawing.Size(168, 23);
            this.new_game.TabIndex = 1;
            this.new_game.Text = "Создать Новую Комнату";
            this.new_game.UseVisualStyleBackColor = true;
            this.new_game.Click += new System.EventHandler(this.new_game_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(844, 551);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(170, 130);
            this.textBox1.TabIndex = 2;
            // 
            // Authorization_button
            // 
            this.Authorization_button.Location = new System.Drawing.Point(507, 351);
            this.Authorization_button.Name = "Authorization_button";
            this.Authorization_button.Size = new System.Drawing.Size(100, 23);
            this.Authorization_button.TabIndex = 3;
            this.Authorization_button.Text = "Авторизоваться";
            this.Authorization_button.UseVisualStyleBackColor = true;
            this.Authorization_button.Click += new System.EventHandler(this.button3_Click);
            // 
            // login_text
            // 
            this.login_text.Location = new System.Drawing.Point(507, 222);
            this.login_text.Name = "login_text";
            this.login_text.Size = new System.Drawing.Size(100, 20);
            this.login_text.TabIndex = 4;
            // 
            // Password_text
            // 
            this.Password_text.Location = new System.Drawing.Point(507, 261);
            this.Password_text.Name = "Password_text";
            this.Password_text.Size = new System.Drawing.Size(100, 20);
            this.Password_text.TabIndex = 5;
            // 
            // Authorization_label
            // 
            this.Authorization_label.AutoSize = true;
            this.Authorization_label.Location = new System.Drawing.Point(624, 225);
            this.Authorization_label.Name = "Authorization_label";
            this.Authorization_label.Size = new System.Drawing.Size(38, 13);
            this.Authorization_label.TabIndex = 6;
            this.Authorization_label.Text = "Логин";
            // 
            // Password_label
            // 
            this.Password_label.AutoSize = true;
            this.Password_label.Location = new System.Drawing.Point(624, 264);
            this.Password_label.Name = "Password_label";
            this.Password_label.Size = new System.Drawing.Size(45, 13);
            this.Password_label.TabIndex = 7;
            this.Password_label.Text = "Пороль";
            // 
            // Registration_button
            // 
            this.Registration_button.Location = new System.Drawing.Point(507, 380);
            this.Registration_button.Name = "Registration_button";
            this.Registration_button.Size = new System.Drawing.Size(100, 23);
            this.Registration_button.TabIndex = 8;
            this.Registration_button.Text = "Регистрация";
            this.Registration_button.UseVisualStyleBackColor = true;
            this.Registration_button.Click += new System.EventHandler(this.Registration_button_Click);
            // 
            // name_text
            // 
            this.name_text.Location = new System.Drawing.Point(507, 307);
            this.name_text.Name = "name_text";
            this.name_text.Size = new System.Drawing.Size(100, 20);
            this.name_text.TabIndex = 9;
            // 
            // name_label
            // 
            this.name_label.AutoSize = true;
            this.name_label.Location = new System.Drawing.Point(624, 310);
            this.name_label.Name = "name_label";
            this.name_label.Size = new System.Drawing.Size(27, 13);
            this.name_label.TabIndex = 10;
            this.name_label.Text = "имя";
            // 
            // back
            // 
            this.back.Location = new System.Drawing.Point(507, 409);
            this.back.Name = "back";
            this.back.Size = new System.Drawing.Size(100, 23);
            this.back.TabIndex = 11;
            this.back.Text = "Назад";
            this.back.UseVisualStyleBackColor = true;
            this.back.Click += new System.EventHandler(this.back_Click);
            // 
            // name_auth
            // 
            this.name_auth.AutoSize = true;
            this.name_auth.Location = new System.Drawing.Point(99, 399);
            this.name_auth.Name = "name_auth";
            this.name_auth.Size = new System.Drawing.Size(0, 13);
            this.name_auth.TabIndex = 12;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(140, 399);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(27, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "имя";
            // 
            // id_auth
            // 
            this.id_auth.AutoSize = true;
            this.id_auth.Location = new System.Drawing.Point(99, 423);
            this.id_auth.Name = "id_auth";
            this.id_auth.Size = new System.Drawing.Size(0, 13);
            this.id_auth.TabIndex = 14;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(140, 423);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(15, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "id";
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(897, 80);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 23);
            this.label1.TabIndex = 19;
            this.label1.Text = "Port";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(897, 45);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 23);
            this.label3.TabIndex = 18;
            this.label3.Text = "IP";
            // 
            // port_text
            // 
            this.port_text.Location = new System.Drawing.Point(951, 86);
            this.port_text.Name = "port_text";
            this.port_text.Size = new System.Drawing.Size(30, 20);
            this.port_text.TabIndex = 17;
            this.port_text.Text = "368";
            // 
            // ip_text
            // 
            this.ip_text.Location = new System.Drawing.Point(935, 48);
            this.ip_text.Name = "ip_text";
            this.ip_text.Size = new System.Drawing.Size(79, 20);
            this.ip_text.TabIndex = 16;
            this.ip_text.Text = "188.233.49.10";
            // 
            // code_text
            // 
            this.code_text.Location = new System.Drawing.Point(36, 512);
            this.code_text.Multiline = true;
            this.code_text.Name = "code_text";
            this.code_text.Size = new System.Drawing.Size(168, 21);
            this.code_text.TabIndex = 20;
            // 
            // code_label
            // 
            this.code_label.AutoSize = true;
            this.code_label.Location = new System.Drawing.Point(210, 515);
            this.code_label.Name = "code_label";
            this.code_label.Size = new System.Drawing.Size(75, 13);
            this.code_label.TabIndex = 21;
            this.code_label.Text = "Код Комнаты";
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1064, 762);
            this.Controls.Add(this.code_label);
            this.Controls.Add(this.code_text);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.port_text);
            this.Controls.Add(this.ip_text);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.id_auth);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.name_auth);
            this.Controls.Add(this.back);
            this.Controls.Add(this.name_label);
            this.Controls.Add(this.name_text);
            this.Controls.Add(this.Registration_button);
            this.Controls.Add(this.Password_label);
            this.Controls.Add(this.Authorization_label);
            this.Controls.Add(this.Password_text);
            this.Controls.Add(this.login_text);
            this.Controls.Add(this.Authorization_button);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.new_game);
            this.Controls.Add(this.connect_room);
            this.Name = "Form3";
            this.Text = "Form3";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form3_FormClosing);
            this.Load += new System.EventHandler(this.Form3_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button connect_room;
        private System.Windows.Forms.Button new_game;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TextBox textBox1;
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
        private System.Windows.Forms.Label code_label;
    }
}