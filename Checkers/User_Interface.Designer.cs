namespace Checkers
{
    partial class User_Interface
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
            this.Settings = new System.Windows.Forms.Panel();
            this.PlayGameButton = new System.Windows.Forms.Button();
            this.NumberOfGames = new System.Windows.Forms.ComboBox();
            this.NumberOfGamesLable = new System.Windows.Forms.Label();
            this.Player2Select = new System.Windows.Forms.ComboBox();
            this.Player2Title = new System.Windows.Forms.Label();
            this.NNSettings = new System.Windows.Forms.GroupBox();
            this.Player1Training = new System.Windows.Forms.CheckBox();
            this.Player1Filename = new System.Windows.Forms.TextBox();
            this.Player1FilenameTitle = new System.Windows.Forms.Label();
            this.Player1Select = new System.Windows.Forms.ComboBox();
            this.Player1Title = new System.Windows.Forms.Label();
            this.MakeAI_Move = new System.ComponentModel.BackgroundWorker();
            this.Train_Neural_Network = new System.ComponentModel.BackgroundWorker();
            this.Settings.SuspendLayout();
            this.NNSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // Settings
            // 
            this.Settings.Controls.Add(this.PlayGameButton);
            this.Settings.Controls.Add(this.NumberOfGames);
            this.Settings.Controls.Add(this.NumberOfGamesLable);
            this.Settings.Controls.Add(this.Player2Select);
            this.Settings.Controls.Add(this.Player2Title);
            this.Settings.Controls.Add(this.NNSettings);
            this.Settings.Controls.Add(this.Player1Select);
            this.Settings.Controls.Add(this.Player1Title);
            this.Settings.Location = new System.Drawing.Point(375, 140);
            this.Settings.Name = "Settings";
            this.Settings.Size = new System.Drawing.Size(247, 309);
            this.Settings.TabIndex = 13;
            // 
            // PlayGameButton
            // 
            this.PlayGameButton.Location = new System.Drawing.Point(3, 250);
            this.PlayGameButton.Name = "PlayGameButton";
            this.PlayGameButton.Size = new System.Drawing.Size(234, 38);
            this.PlayGameButton.TabIndex = 11;
            this.PlayGameButton.Text = "Play Game";
            this.PlayGameButton.UseVisualStyleBackColor = true;
            this.PlayGameButton.Click += new System.EventHandler(this.PlayButton_Click);
            // 
            // NumberOfGames
            // 
            this.NumberOfGames.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.NumberOfGames.FormattingEnabled = true;
            this.NumberOfGames.Items.AddRange(new object[] {
            "1",
            "3",
            "5",
            "7"});
            this.NumberOfGames.Location = new System.Drawing.Point(116, 196);
            this.NumberOfGames.Name = "NumberOfGames";
            this.NumberOfGames.Size = new System.Drawing.Size(57, 21);
            this.NumberOfGames.TabIndex = 10;
            // 
            // NumberOfGamesLable
            // 
            this.NumberOfGamesLable.AutoSize = true;
            this.NumberOfGamesLable.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
            this.NumberOfGamesLable.Location = new System.Drawing.Point(3, 199);
            this.NumberOfGamesLable.Name = "NumberOfGamesLable";
            this.NumberOfGamesLable.Size = new System.Drawing.Size(106, 14);
            this.NumberOfGamesLable.TabIndex = 9;
            this.NumberOfGamesLable.Text = "Number of games";
            // 
            // Player2Select
            // 
            this.Player2Select.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Player2Select.FormattingEnabled = true;
            this.Player2Select.Items.AddRange(new object[] {
            "Human",
            "MinMax - Easy",
            "MinMax - Medium",
            "MinMax - Hard",
            "Neural Network"});
            this.Player2Select.Location = new System.Drawing.Point(3, 78);
            this.Player2Select.Name = "Player2Select";
            this.Player2Select.Size = new System.Drawing.Size(120, 21);
            this.Player2Select.TabIndex = 8;
            // 
            // Player2Title
            // 
            this.Player2Title.AutoSize = true;
            this.Player2Title.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
            this.Player2Title.Location = new System.Drawing.Point(3, 63);
            this.Player2Title.Name = "Player2Title";
            this.Player2Title.Size = new System.Drawing.Size(50, 14);
            this.Player2Title.TabIndex = 7;
            this.Player2Title.Text = "Player 2";
            // 
            // NNSettings
            // 
            this.NNSettings.Controls.Add(this.Player1Training);
            this.NNSettings.Controls.Add(this.Player1Filename);
            this.NNSettings.Controls.Add(this.Player1FilenameTitle);
            this.NNSettings.Enabled = false;
            this.NNSettings.Location = new System.Drawing.Point(3, 115);
            this.NNSettings.Name = "NNSettings";
            this.NNSettings.Size = new System.Drawing.Size(235, 60);
            this.NNSettings.TabIndex = 6;
            this.NNSettings.TabStop = false;
            this.NNSettings.Text = "Neural Network Settings";
            // 
            // Player1Training
            // 
            this.Player1Training.AutoSize = true;
            this.Player1Training.Location = new System.Drawing.Point(127, 36);
            this.Player1Training.Name = "Player1Training";
            this.Player1Training.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Player1Training.Size = new System.Drawing.Size(100, 17);
            this.Player1Training.TabIndex = 5;
            this.Player1Training.Text = "Enable Training";
            this.Player1Training.UseVisualStyleBackColor = true;
            // 
            // Player1Filename
            // 
            this.Player1Filename.Location = new System.Drawing.Point(7, 33);
            this.Player1Filename.Name = "Player1Filename";
            this.Player1Filename.Size = new System.Drawing.Size(100, 20);
            this.Player1Filename.TabIndex = 3;
            // 
            // Player1FilenameTitle
            // 
            this.Player1FilenameTitle.AutoSize = true;
            this.Player1FilenameTitle.Location = new System.Drawing.Point(7, 18);
            this.Player1FilenameTitle.Name = "Player1FilenameTitle";
            this.Player1FilenameTitle.Size = new System.Drawing.Size(49, 13);
            this.Player1FilenameTitle.TabIndex = 2;
            this.Player1FilenameTitle.Text = "Filename";
            // 
            // Player1Select
            // 
            this.Player1Select.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Player1Select.FormattingEnabled = true;
            this.Player1Select.Items.AddRange(new object[] {
            "Human",
            "MinMax - Easy",
            "MinMax - Medium",
            "MinMax - Hard",
            "Neural Network"});
            this.Player1Select.Location = new System.Drawing.Point(3, 31);
            this.Player1Select.Name = "Player1Select";
            this.Player1Select.Size = new System.Drawing.Size(120, 21);
            this.Player1Select.TabIndex = 1;
            // 
            // Player1Title
            // 
            this.Player1Title.AutoSize = true;
            this.Player1Title.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
            this.Player1Title.Location = new System.Drawing.Point(3, 16);
            this.Player1Title.Name = "Player1Title";
            this.Player1Title.Size = new System.Drawing.Size(50, 14);
            this.Player1Title.TabIndex = 0;
            this.Player1Title.Text = "Player 1";
            // 
            // MakeAI_Move
            // 
            this.MakeAI_Move.WorkerReportsProgress = true;
            this.MakeAI_Move.DoWork += new System.ComponentModel.DoWorkEventHandler(this.MakeAI_Move_DoWork);
            this.MakeAI_Move.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.MakeAI_Move_Complete);
            // 
            // Train_Neural_Network
            // 
            this.Train_Neural_Network.WorkerReportsProgress = true;
            this.Train_Neural_Network.DoWork += new System.ComponentModel.DoWorkEventHandler(this.Train_Neural_Net_Training);
            this.Train_Neural_Network.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.Train_Neural_Net_Complete);
            // 
            // User_Interface
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(634, 461);
            this.Controls.Add(this.Settings);
            this.Name = "User_Interface";
            this.Text = "Checkers";
            this.Settings.ResumeLayout(false);
            this.Settings.PerformLayout();
            this.NNSettings.ResumeLayout(false);
            this.NNSettings.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel Settings;
        private System.Windows.Forms.Button PlayGameButton;
        private System.Windows.Forms.ComboBox NumberOfGames;
        private System.Windows.Forms.Label NumberOfGamesLable;
        private System.Windows.Forms.ComboBox Player2Select;
        private System.Windows.Forms.Label Player2Title;
        private System.Windows.Forms.GroupBox NNSettings;
        private System.Windows.Forms.CheckBox Player1Training;
        private System.Windows.Forms.TextBox Player1Filename;
        private System.Windows.Forms.Label Player1FilenameTitle;
        private System.Windows.Forms.ComboBox Player1Select;
        private System.Windows.Forms.Label Player1Title;
        private System.ComponentModel.BackgroundWorker MakeAI_Move;
        private System.ComponentModel.BackgroundWorker Train_Neural_Network;
    }
}