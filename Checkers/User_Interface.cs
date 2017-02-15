using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Checkers
{
    public partial class User_Interface : Form
    {
        private Button[] buttonarray = new Button[64];
        private UIGamePlay Game = new UIGamePlay();

        // Scoring
        private int P1_Score = 0;

        private int P2_Score = 0;

        // Gameplay
        private int CurrentGame = 0;
        private int MaxNumberOfGames = 1;
        private int Player1 = 0;
        private int Player2 = 0;
        private int SelectedPiece = -1;
        private List<int> PossibleMoveLocations = new List<int>();

        public User_Interface()
        {
            InitializeComponent();
            BuildGameBoard();
            InitaliseScore();
            InitaliseCurrentPlayer();
            Player1Select.SelectedIndex = 0;
            Player1Select.SelectedIndexChanged += new System.EventHandler(EnableNNSettings);
            Player2Select.SelectedIndex = 0;
            Player2Select.SelectedIndexChanged += new System.EventHandler(EnableNNSettings);
            NumberOfGames.SelectedIndex = 0;
            Player1Filename.Text = "NeuralNetPlayer.net";
        }

        // Design Elements

        private void BuildGameBoard()
        {
            int Horizantal = 30;
            int Vertical = 100;

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    buttonarray[8 * i + j] = new Button();
                    buttonarray[8 * i + j].Size = new Size(40, 40);
                    buttonarray[8 * i + j].Location = new Point(Horizantal, Vertical);
                    buttonarray[8 * i + j].FlatStyle = FlatStyle.Flat;
                    buttonarray[8 * i + j].FlatAppearance.BorderColor = Color.Black;
                    buttonarray[8 * i + j].FlatAppearance.BorderSize = 1;
                    buttonarray[8 * i + j].Tag = 8 * i + j;
                    buttonarray[8 * i + j].Click += new System.EventHandler(this.PieceSelection);
                    buttonarray[i].Enabled = false;

                    if ((j % 2 != 0 && i % 2 == 0) || (j % 2 == 0 && i % 2 != 0))
                    {
                        buttonarray[8 * i + j].BackColor = Color.White;
                    }
                    else
                    {
                        buttonarray[8 * i + j].BackColor = Color.Black;
                    }
                    this.Controls.Add(buttonarray[8 * i + j]);
                    Horizantal += 41;
                }
                Horizantal = 30;
                Vertical += 41;
            }
        }

        private void PieceSelection(object sender, EventArgs e)
        {
            var Square = (Button)sender;

            // Select Piece
            if (SelectedPiece == -1)
            {
                SelectedPiece = Convert.ToInt32(Square.Tag);
                HighlightMoves();
            }

            // deselect
            else if (SelectedPiece == Convert.ToInt32(Square.Tag))
            {
                RemoveAllHighlightedSquares();
                DisableGameboardSquares();
                HighlightAllPossibleMoves();
                SelectedPiece = -1;
            }

            // Make Move
            else
            {
                MakeMove(Convert.ToInt32(Square.Tag));
                if (Game.GameOver()) { EndGame(); }
                else { ChangePlayers(); }
            }
        }

        private void PlayButton_Click(object sender, EventArgs e)
        {
            Game.InitaliseGame();
            GetSettings();
            UpdateBoard();
            UpdateScore();
            UpdateCurrentPlayer();

            // start game
            if (Player1 > 0) { AI_Move(); }
            else { HighlightAllPossibleMoves(); }
        }

        private void InitaliseScore()
        {
            int X = 130;
            int Y = 30;

            Label ScoreTitle = new Label();
            ScoreTitle.Location = new Point(X + 30, Y);
            ScoreTitle.Text = "Score";
            ScoreTitle.Font = new Font("Arial", 14, FontStyle.Bold);

            Label Score = new Label();
            Score.Name = "Score";
            Score.Location = new Point(X + 40, Y + 30);
            Score.Text = "0 : 0";
            Score.Font = new Font("Arial", 14, FontStyle.Bold);
            Score.Size = new Size(50, 25);

            PictureBox Rebel = new PictureBox();
            Rebel.Location = new Point(X, Y + 30);
            Rebel.Image = global::Checkers.Properties.Resources.rebbellogo;
            Rebel.Size = new Size(25, 25);
            Rebel.SizeMode = PictureBoxSizeMode.StretchImage;

            PictureBox Empire = new PictureBox();
            Empire.Location = new Point(X + 100, Y + 30);
            Empire.Image = global::Checkers.Properties.Resources.Empirelogo;
            Empire.Size = new Size(25, 25);
            Empire.SizeMode = PictureBoxSizeMode.StretchImage;

            this.Controls.Add(ScoreTitle);
            this.Controls.Add(Score);
            this.Controls.Add(Rebel);
            this.Controls.Add(Empire);
        }

        private void InitaliseCurrentPlayer()
        {
            int X = 470;
            int Y = 50;

            PictureBox CurrentPlayer = new PictureBox();
            CurrentPlayer.Name = "CurrentPlayerImage";
            CurrentPlayer.Size = new Size(40, 40);
            CurrentPlayer.Location = new Point(X, Y);
            CurrentPlayer.SizeMode = PictureBoxSizeMode.StretchImage;
            CurrentPlayer.Image = global::Checkers.Properties.Resources.rebbellogo; // change when needed

            Label CurrentPlayerTitle = new Label();
            CurrentPlayerTitle.Location = new Point(X - 20, Y + 42);
            CurrentPlayerTitle.Text = "Current Player";
            CurrentPlayerTitle.Font = new Font("Arial", 8, FontStyle.Bold);

            this.Controls.Add(CurrentPlayer);
            this.Controls.Add(CurrentPlayerTitle);
        }
        
        private void EnableNNSettings(object sender, EventArgs e)
        {
            var box = (ComboBox)sender;
            if (box.SelectedIndex == 4) { NNSettings.Enabled = true; }
            else { NNSettings.Enabled = false; }
        }

        private void GetSettings()
        {
            P1_Score = 0;
            P2_Score = 0;

            if (NNSettings.Enabled == true)
            {
                NN_Evaluator Net = new NN_Evaluator();
                Net.Load_Network(Player1Filename.Text);
                var NN = Game.AI_Players[3] as NN_ReinforcmentLearning;
                NN.SetNeuralNetwork = Net;
                if (Player1Training.Checked == true) { NN.EnableTraining = true; }
                else { NN.EnableTraining = false; }
            }
            Settings.Enabled = false;
            Player1 = Player1Select.SelectedIndex;
            Player2 = Player2Select.SelectedIndex;
            MaxNumberOfGames = Convert.ToInt32(NumberOfGames.Text);
        }

        // Highlighting Squares and other UI related functions

        private void DisableGameboardSquares()
        {
            for (int i = 0; i < buttonarray.Count(); i++) { buttonarray[i].Enabled = false; }
        }

        private void HighlightGameboardSpace(int Location, Color HighlightColor)
        {
            if (buttonarray[Location].FlatAppearance.BorderColor == Color.Black)
            {
                buttonarray[Location].FlatAppearance.BorderSize = 3;
                buttonarray[Location].FlatAppearance.BorderColor = HighlightColor;
            }
        }

        private void RemoveAllHighlightedSquares()
        {
            for (int i = 0; i < buttonarray.Count(); i++)
            {
                buttonarray[i].FlatAppearance.BorderSize = 1;
                buttonarray[i].FlatAppearance.BorderColor = Color.Black;
            }
        }

        private void HighlightAllPossibleMoves()
        {
            List<List<int>> PiecesWithValidMoves = Game.DetermineMoves();
            for (int i = 0; i < PiecesWithValidMoves.Count; i++)
            {
                for (int j = 0; j < Game.GameBoard.Count; j++)
                {
                    if ((Game.GameBoard[j] == Game.CurrentPlayer || Game.GameBoard[j] == 2 * Game.CurrentPlayer) && PiecesWithValidMoves[i][j] == 0)
                    {
                        HighlightGameboardSpace(j, Color.MediumPurple);
                        buttonarray[j].Enabled = true;
                    }
                }
            }
        }

        private void HighlightMoves()
        {
            buttonarray[SelectedPiece].Enabled = true;
            List<List<int>> PiecesWithValidMoves = Game.DetermineMoves();
            for (int i = 0; i < PiecesWithValidMoves.Count; i++)
            {
                for (int j = 0; j < Game.GameBoard.Count; j++)
                {
                    if (Game.GameBoard[j] == 0 && PiecesWithValidMoves[i][SelectedPiece] == 0 && (PiecesWithValidMoves[i][j] == Game.CurrentPlayer || PiecesWithValidMoves[i][j] == 2 * Game.CurrentPlayer))
                    {
                        buttonarray[j].Enabled = true;
                        PossibleMoveLocations.Add(j);
                        HighlightGameboardSpace(j, Color.LawnGreen);
                    }
                }
            }
        }

        private void UpdateScore()
        {
            Label score = Controls.OfType<Label>().FirstOrDefault(b => b.Name.Equals("Score"));
            score.Text = P1_Score + " : " + P2_Score;
        }

        private void UpdateBoard()
        {
            RemoveAllHighlightedSquares();
            for (int i = 0; i < Game.GameBoard.Count; i++)
            {
                if (Game.GameBoard[i] == 1)
                {
                    buttonarray[i].BackgroundImage = global::Checkers.Properties.Resources.rebbellogo;
                    buttonarray[i].BackgroundImageLayout = ImageLayout.Stretch;
                }
                else if (Game.GameBoard[i] == 2)
                {
                    buttonarray[i].BackgroundImage = global::Checkers.Properties.Resources.Jedi;
                    buttonarray[i].BackgroundImageLayout = ImageLayout.Stretch;
                }
                else if (Game.GameBoard[i] == -1)
                {
                    buttonarray[i].BackgroundImage = global::Checkers.Properties.Resources.Empirelogo;
                    buttonarray[i].BackgroundImageLayout = ImageLayout.Stretch;
                }
                else if (Game.GameBoard[i] == -2)
                {
                    buttonarray[i].BackgroundImage = global::Checkers.Properties.Resources.SithLogo;
                    buttonarray[i].BackgroundImageLayout = ImageLayout.Stretch;
                }
                else if (((i / 8) % 2 == 0 && (i % 8) % 2 == 0) || ((i / 8) % 2 != 0 && (i % 8) % 2 != 0))
                {
                    buttonarray[i].BackgroundImage = null;
                }
            }
        }

        private void UpdateCurrentPlayer()
        {
            PictureBox pic = Controls.OfType<PictureBox>().FirstOrDefault(b => b.Name.Equals("CurrentPlayerImage"));
            if (Game.CurrentPlayer == -1)
            {
                pic.Image = global::Checkers.Properties.Resources.Empirelogo;
            }
            else pic.Image = global::Checkers.Properties.Resources.rebbellogo;
        }


        // General game play functions 

        private void MakeMove(int SelectedMove)
        {
            List<List<int>> PossibleMoves = Game.DetermineMoves();
            for (int i = 0; i < PossibleMoves.Count; i++)
            {
                if (PossibleMoves[i][SelectedPiece] == 0 && (PossibleMoves[i][SelectedMove] == Game.CurrentPlayer || PossibleMoves[i][SelectedMove] == 2 * Game.CurrentPlayer))
                {
                    Game.GameBoard = PossibleMoves[i];
                }
            }

            SelectedPiece = -1;
            RemoveAllHighlightedSquares();
            UpdateBoard();
        }

        private void ChangePlayers()
        {
            Game.SwitchPlayers();
            UpdateCurrentPlayer();
            if (Game.CurrentPlayer == 1 && Player1 > 0) { AI_Move(); }
            else if (Game.CurrentPlayer == -1 && Player2 > 0) { AI_Move(); }
            else
            {
                RemoveAllHighlightedSquares();
                DisableGameboardSquares();
                HighlightAllPossibleMoves();
            }
        }

        private void EndGame()
        {
            CurrentGame++;

            if (((Game.CurrentPlayer == 1 && Player1==4)||(Game.CurrentPlayer == -1 && Player2 == 4)) && Player1Training.Checked == true)
            {
                //User_Interface.ActiveForm.Text = "Learning.... (Updating Neural Net)";
                Train_Neural_Network.RunWorkerAsync("Win");

            }
            else if (((Game.CurrentPlayer == 1 && Player1 != 4) || (Game.CurrentPlayer == -1 && Player2 != 4)) && Player1Training.Checked == true)
            {
               // User_Interface.ActiveForm.Text = "Learning.... (Updating Neural Net)";
                Train_Neural_Network.RunWorkerAsync("Loose");
            }

            else
            {
                EndGameTidyUp();
            }
        }

        private void EndGameTidyUp()
        {
                if (Game.CurrentPlayer == 1) { P1_Score++; }
                else { P2_Score++; }

                UpdateScore();

                if (CurrentGame < MaxNumberOfGames)
                {
                    Game.InitaliseGame();
                    UpdateBoard();
                    HighlightAllPossibleMoves();
                }
                else
                {
                    DisableGameboardSquares();
                    int Player = 0;
                    if (Game.CurrentPlayer == 1) { Player = 1; }
                    else { Player = 2; }
                    MessageBox.Show("Congratulations Player " + Player + " you have won!");
                    Settings.Enabled = true;
                }
        }

        // AI Functions

        private void AI_Move()
        {
            //User_Interface.ActiveForm.Text = "Thinking...";
            MakeAI_Move.RunWorkerAsync();  
        }

        private void MakeAI_Move_DoWork(object sender, DoWorkEventArgs e)
        {
            if (Game.CurrentPlayer == 1) { Game.AIMove(Player1-1); }
            else { Game.AIMove(Player2-1); }
        }

        private void MakeAI_Move_Complete(object sender, RunWorkerCompletedEventArgs e)
        {
            UpdateBoard();
            //User_Interface.ActiveForm.Text = "Checkers";
            if (Game.GameOver()) { EndGame(); }
            else { ChangePlayers(); }
        }

        private void Train_Neural_Net_Training(object sender, DoWorkEventArgs e)
        {
            string Result = e.Argument.ToString();
            var NN = Game.AI_Players[3] as NN_ReinforcmentLearning;
            NN.Train_Neural_Network(Result, Player1Filename.Text);
        }



        private void Train_Neural_Net_Complete(object sender, RunWorkerCompletedEventArgs e)
        {
            UpdateScore();
           // User_Interface.ActiveForm.Text = "Checkers";
            EndGameTidyUp();
        }
    }
}
