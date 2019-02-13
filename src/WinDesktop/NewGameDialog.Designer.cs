namespace WinDesktop
{
    partial class NewGameDialog
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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.PlayerNumSelect_Label = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cbxNumPlayers = new System.Windows.Forms.ComboBox();
            this.cbxGameWidth = new System.Windows.Forms.ComboBox();
            this.cbxGameHeight = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Font = new System.Drawing.Font("Open Sans", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(149, 430);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(60, 36);
            this.button1.TabIndex = 0;
            this.button1.Text = "START";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Font = new System.Drawing.Font("Open Sans", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(314, 430);
            this.button2.Margin = new System.Windows.Forms.Padding(2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(67, 36);
            this.button2.TabIndex = 1;
            this.button2.Text = "CANCEL";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // PlayerNumSelect_Label
            // 
            this.PlayerNumSelect_Label.AutoSize = true;
            this.PlayerNumSelect_Label.Location = new System.Drawing.Point(76, 98);
            this.PlayerNumSelect_Label.Name = "PlayerNumSelect_Label";
            this.PlayerNumSelect_Label.Size = new System.Drawing.Size(123, 13);
            this.PlayerNumSelect_Label.TabIndex = 4;
            this.PlayerNumSelect_Label.Text = "Select number of players";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Open Sans", 20F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(117, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(294, 37);
            this.label1.TabIndex = 6;
            this.label1.Text = "NEW GAME OPTIONS";
            // 
            // cbxNumPlayers
            // 
            this.cbxNumPlayers.FormattingEnabled = true;
            this.cbxNumPlayers.Items.AddRange(new object[] {
            "3",
            "4",
            "5",
            "6"});
            this.cbxNumPlayers.Location = new System.Drawing.Point(215, 94);
            this.cbxNumPlayers.Name = "cbxNumPlayers";
            this.cbxNumPlayers.Size = new System.Drawing.Size(48, 21);
            this.cbxNumPlayers.TabIndex = 7;
            this.cbxNumPlayers.Text = "6";
            // 
            // cbxGameWidth
            // 
            this.cbxGameWidth.FormattingEnabled = true;
            this.cbxGameWidth.Items.AddRange(new object[] {
            "10",
            "11",
            "12",
            "13",
            "14",
            "15"});
            this.cbxGameWidth.Location = new System.Drawing.Point(215, 144);
            this.cbxGameWidth.Name = "cbxGameWidth";
            this.cbxGameWidth.Size = new System.Drawing.Size(48, 21);
            this.cbxGameWidth.TabIndex = 8;
            this.cbxGameWidth.Text = "10";
            // 
            // cbxGameHeight
            // 
            this.cbxGameHeight.FormattingEnabled = true;
            this.cbxGameHeight.Items.AddRange(new object[] {
            "10",
            "11",
            "12",
            "13",
            "14",
            "15"});
            this.cbxGameHeight.Location = new System.Drawing.Point(215, 193);
            this.cbxGameHeight.Name = "cbxGameHeight";
            this.cbxGameHeight.Size = new System.Drawing.Size(48, 21);
            this.cbxGameHeight.TabIndex = 9;
            this.cbxGameHeight.Text = "10";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(109, 147);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(97, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Game Board Width";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(104, 196);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Game Board Height";
            // 
            // NewGameDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(535, 522);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbxGameHeight);
            this.Controls.Add(this.cbxGameWidth);
            this.Controls.Add(this.cbxNumPlayers);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.PlayerNumSelect_Label);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "NewGameDialog";
            this.Text = "NewGameDialog";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label PlayerNumSelect_Label;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbxNumPlayers;
        private System.Windows.Forms.ComboBox cbxGameWidth;
        private System.Windows.Forms.ComboBox cbxGameHeight;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}