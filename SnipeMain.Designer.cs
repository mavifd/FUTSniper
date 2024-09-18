namespace SnipeBot
{
    partial class SnipeMain
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
            this.StartBtn = new System.Windows.Forms.Button();
            this.StopBtn = new System.Windows.Forms.Button();
            this.LoggerTXT = new System.Windows.Forms.RichTextBox();
            this.priceSetupBox = new System.Windows.Forms.GroupBox();
            this.changevalueTxt = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.maxchangeTxt = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.changeTxt = new System.Windows.Forms.TextBox();
            this.maxpriceTxt = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.topmostcb = new System.Windows.Forms.CheckBox();
            this.resetstatsBtn = new System.Windows.Forms.Button();
            this.errorcountTxt = new System.Windows.Forms.Label();
            this.loopcountTxt = new System.Windows.Forms.Label();
            this.buycountTxt = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.StatUpdater = new System.Windows.Forms.Timer(this.components);
            this.priceSetupBox.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // StartBtn
            // 
            this.StartBtn.ForeColor = System.Drawing.Color.DarkBlue;
            this.StartBtn.Location = new System.Drawing.Point(12, 12);
            this.StartBtn.Name = "StartBtn";
            this.StartBtn.Size = new System.Drawing.Size(118, 41);
            this.StartBtn.TabIndex = 0;
            this.StartBtn.Text = "Başlat";
            this.StartBtn.UseVisualStyleBackColor = true;
            this.StartBtn.Click += new System.EventHandler(this.StartBtn_Click);
            // 
            // StopBtn
            // 
            this.StopBtn.Enabled = false;
            this.StopBtn.Location = new System.Drawing.Point(12, 59);
            this.StopBtn.Name = "StopBtn";
            this.StopBtn.Size = new System.Drawing.Size(118, 41);
            this.StopBtn.TabIndex = 1;
            this.StopBtn.Text = "Durdur";
            this.StopBtn.UseVisualStyleBackColor = true;
            this.StopBtn.Click += new System.EventHandler(this.StopBtn_Click);
            // 
            // LoggerTXT
            // 
            this.LoggerTXT.BackColor = System.Drawing.Color.CornflowerBlue;
            this.LoggerTXT.ForeColor = System.Drawing.Color.White;
            this.LoggerTXT.Location = new System.Drawing.Point(136, 12);
            this.LoggerTXT.Name = "LoggerTXT";
            this.LoggerTXT.ReadOnly = true;
            this.LoggerTXT.Size = new System.Drawing.Size(739, 367);
            this.LoggerTXT.TabIndex = 2;
            this.LoggerTXT.Text = "\"C:\\Program Files (x86)\\Microsoft\\Edge\\Application\\msedge.exe\" --remote-debugging" +
    "-port=9111\n\nEdge tamamen kapalı olduğundan emin ol.\n";
            // 
            // priceSetupBox
            // 
            this.priceSetupBox.Controls.Add(this.changevalueTxt);
            this.priceSetupBox.Controls.Add(this.label4);
            this.priceSetupBox.Controls.Add(this.maxchangeTxt);
            this.priceSetupBox.Controls.Add(this.label13);
            this.priceSetupBox.Controls.Add(this.label12);
            this.priceSetupBox.Controls.Add(this.changeTxt);
            this.priceSetupBox.Controls.Add(this.maxpriceTxt);
            this.priceSetupBox.Controls.Add(this.label11);
            this.priceSetupBox.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.priceSetupBox.Location = new System.Drawing.Point(12, 106);
            this.priceSetupBox.Name = "priceSetupBox";
            this.priceSetupBox.Size = new System.Drawing.Size(118, 273);
            this.priceSetupBox.TabIndex = 34;
            this.priceSetupBox.TabStop = false;
            this.priceSetupBox.Text = "Fiyat Ayarları";
            // 
            // changevalueTxt
            // 
            this.changevalueTxt.Font = new System.Drawing.Font("Nirmala UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.changevalueTxt.Location = new System.Drawing.Point(6, 233);
            this.changevalueTxt.Name = "changevalueTxt";
            this.changevalueTxt.Size = new System.Drawing.Size(99, 27);
            this.changevalueTxt.TabIndex = 60;
            this.changevalueTxt.Text = "50";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Nirmala UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.DarkGreen;
            this.label4.Location = new System.Drawing.Point(6, 206);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 20);
            this.label4.TabIndex = 59;
            this.label4.Text = "Değişim ";
            // 
            // maxchangeTxt
            // 
            this.maxchangeTxt.Font = new System.Drawing.Font("Nirmala UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.maxchangeTxt.Location = new System.Drawing.Point(6, 172);
            this.maxchangeTxt.Name = "maxchangeTxt";
            this.maxchangeTxt.Size = new System.Drawing.Size(99, 27);
            this.maxchangeTxt.TabIndex = 58;
            this.maxchangeTxt.Text = "350";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Nirmala UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.ForeColor = System.Drawing.Color.DarkGreen;
            this.label13.Location = new System.Drawing.Point(6, 145);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(108, 20);
            this.label13.TabIndex = 57;
            this.label13.Text = "En Yüksek Artış";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Nirmala UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.DarkGreen;
            this.label12.Location = new System.Drawing.Point(6, 84);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(81, 20);
            this.label12.TabIndex = 56;
            this.label12.Text = "Artış Oranı";
            this.label12.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // changeTxt
            // 
            this.changeTxt.Font = new System.Drawing.Font("Nirmala UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.changeTxt.Location = new System.Drawing.Point(6, 111);
            this.changeTxt.Name = "changeTxt";
            this.changeTxt.Size = new System.Drawing.Size(99, 27);
            this.changeTxt.TabIndex = 55;
            this.changeTxt.Text = "50";
            // 
            // maxpriceTxt
            // 
            this.maxpriceTxt.Font = new System.Drawing.Font("Nirmala UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.maxpriceTxt.Location = new System.Drawing.Point(6, 50);
            this.maxpriceTxt.Name = "maxpriceTxt";
            this.maxpriceTxt.Size = new System.Drawing.Size(99, 27);
            this.maxpriceTxt.TabIndex = 51;
            this.maxpriceTxt.Text = "200";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Nirmala UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.DarkGreen;
            this.label11.Location = new System.Drawing.Point(6, 23);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(81, 20);
            this.label11.TabIndex = 50;
            this.label11.Text = "Tavan Fiyat";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.topmostcb);
            this.groupBox1.Controls.Add(this.resetstatsBtn);
            this.groupBox1.Controls.Add(this.errorcountTxt);
            this.groupBox1.Controls.Add(this.loopcountTxt);
            this.groupBox1.Controls.Add(this.buycountTxt);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(881, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(116, 367);
            this.groupBox1.TabIndex = 35;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Durum";
            // 
            // topmostcb
            // 
            this.topmostcb.AutoSize = true;
            this.topmostcb.Checked = true;
            this.topmostcb.CheckState = System.Windows.Forms.CheckState.Checked;
            this.topmostcb.Location = new System.Drawing.Point(6, 269);
            this.topmostcb.Name = "topmostcb";
            this.topmostcb.Size = new System.Drawing.Size(105, 24);
            this.topmostcb.TabIndex = 65;
            this.topmostcb.Text = "Üstte Kalsın";
            this.topmostcb.UseVisualStyleBackColor = true;
            this.topmostcb.CheckedChanged += new System.EventHandler(this.topmostcb_CheckedChanged);
            // 
            // resetstatsBtn
            // 
            this.resetstatsBtn.Location = new System.Drawing.Point(6, 299);
            this.resetstatsBtn.Name = "resetstatsBtn";
            this.resetstatsBtn.Size = new System.Drawing.Size(98, 62);
            this.resetstatsBtn.TabIndex = 36;
            this.resetstatsBtn.Text = "İstatistikleri Sıfırla";
            this.resetstatsBtn.UseVisualStyleBackColor = true;
            this.resetstatsBtn.Click += new System.EventHandler(this.resetstatsBtn_Click);
            // 
            // errorcountTxt
            // 
            this.errorcountTxt.AutoSize = true;
            this.errorcountTxt.Font = new System.Drawing.Font("Nirmala UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.errorcountTxt.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.errorcountTxt.Location = new System.Drawing.Point(6, 133);
            this.errorcountTxt.Name = "errorcountTxt";
            this.errorcountTxt.Size = new System.Drawing.Size(17, 20);
            this.errorcountTxt.TabIndex = 64;
            this.errorcountTxt.Text = "0";
            // 
            // loopcountTxt
            // 
            this.loopcountTxt.AutoSize = true;
            this.loopcountTxt.Font = new System.Drawing.Font("Nirmala UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loopcountTxt.Location = new System.Drawing.Point(6, 89);
            this.loopcountTxt.Name = "loopcountTxt";
            this.loopcountTxt.Size = new System.Drawing.Size(17, 20);
            this.loopcountTxt.TabIndex = 63;
            this.loopcountTxt.Text = "0";
            // 
            // buycountTxt
            // 
            this.buycountTxt.AutoSize = true;
            this.buycountTxt.Font = new System.Drawing.Font("Nirmala UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buycountTxt.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.buycountTxt.Location = new System.Drawing.Point(6, 45);
            this.buycountTxt.Name = "buycountTxt";
            this.buycountTxt.Size = new System.Drawing.Size(17, 20);
            this.buycountTxt.TabIndex = 62;
            this.buycountTxt.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Nirmala UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(6, 111);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 20);
            this.label3.TabIndex = 61;
            this.label3.Text = "Hata:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Nirmala UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(6, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 20);
            this.label2.TabIndex = 60;
            this.label2.Text = "Döngü:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Nirmala UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(6, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 20);
            this.label1.TabIndex = 59;
            this.label1.Text = "Alınan:";
            // 
            // StatUpdater
            // 
            this.StatUpdater.Enabled = true;
            this.StatUpdater.Interval = 1000;
            this.StatUpdater.Tick += new System.EventHandler(this.StatUpdater_Tick);
            // 
            // SnipeMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(1005, 385);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.priceSetupBox);
            this.Controls.Add(this.LoggerTXT);
            this.Controls.Add(this.StopBtn);
            this.Controls.Add(this.StartBtn);
            this.Font = new System.Drawing.Font("Ebrima", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "SnipeMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SnipeMain";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.SnipeMain_Load);
            this.priceSetupBox.ResumeLayout(false);
            this.priceSetupBox.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button StartBtn;
        private System.Windows.Forms.Button StopBtn;
        private System.Windows.Forms.RichTextBox LoggerTXT;
        private System.Windows.Forms.GroupBox priceSetupBox;
        private System.Windows.Forms.TextBox maxchangeTxt;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox changeTxt;
        private System.Windows.Forms.TextBox maxpriceTxt;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label errorcountTxt;
        private System.Windows.Forms.Label loopcountTxt;
        private System.Windows.Forms.Label buycountTxt;
        private System.Windows.Forms.Timer StatUpdater;
        private System.Windows.Forms.Button resetstatsBtn;
        private System.Windows.Forms.TextBox changevalueTxt;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox topmostcb;
    }
}