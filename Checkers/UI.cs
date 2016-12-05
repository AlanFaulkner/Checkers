using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Checkers
{
    public partial class UI : Form
    {
        GamePlay Game = new GamePlay();
        AI GameAI = new AI();
        int SelectedPiece = -1;
        bool JumpingMove = false;
        List<List<int>> Possiblemoves = new List<List<int>> { };

        public UI()
        {
            InitializeComponent();
            Player1.SelectedItem = "Human";
            Player2.SelectedItem = "Human";
            List<Button> GameBoard = Gameboard.Controls.OfType<Button>().ToList();
            for (int i = 0; i < GameBoard.Count; i++)
            {
                if (((i / 8) % 2 == 0 && (i % 8) % 2 == 0) || ((i / 8) % 2 != 0 && (i % 8) % 2 != 0))
                {
                    GameBoard[i].BackgroundImage = BackgroundImage = null;
                    GameBoard[i].BackColor = Color.Black;
                    GameBoard[i].FlatStyle = FlatStyle.Flat;
                    GameBoard[i].Enabled = false;
                }

                else
                {
                    GameBoard[i].BackgroundImage = BackgroundImage = null;
                    GameBoard[i].BackColor = Color.Gainsboro;
                    GameBoard[i].ForeColor = Color.Gainsboro;
                    GameBoard[i].FlatStyle = FlatStyle.Flat;
                    GameBoard[i].Enabled = false;
                }
            }
        }

        private void UpdateBoard()
        {
            List<Button> GameBoard = Gameboard.Controls.OfType<Button>().ToList();
            GameBoard.Reverse();
            for (int i = 0; i < Game.Gameboard.Count; i++)
            {
                if (Game.Gameboard[i] == 1)
                {
                    GameBoard[i].BackgroundImage = Checkers.Properties.Resources.rebbellogo;
                    GameBoard[i].BackgroundImageLayout = ImageLayout.Stretch;
                    GameBoard[i].FlatStyle = FlatStyle.Flat;
                    GameBoard[i].FlatAppearance.BorderColor = Color.Black;
                    GameBoard[i].Enabled = true;
                }
                else if (Game.Gameboard[i] == 2)
                {
                    GameBoard[i].BackgroundImage = Checkers.Properties.Resources.Jedi;
                    GameBoard[i].BackgroundImageLayout = ImageLayout.Stretch;
                    GameBoard[i].FlatStyle = FlatStyle.Flat;
                    GameBoard[i].FlatAppearance.BorderColor = Color.Black;
                    GameBoard[i].Enabled = true;
                }
                else if (Game.Gameboard[i] == -1)
                {
                    GameBoard[i].BackgroundImage = Checkers.Properties.Resources.Empirelogo;
                    GameBoard[i].BackgroundImageLayout = ImageLayout.Stretch;
                    GameBoard[i].FlatStyle = FlatStyle.Flat;
                    GameBoard[i].FlatAppearance.BorderColor = Color.Black;
                    GameBoard[i].Enabled = true;
                }
                else if (Game.Gameboard[i] == -2)
                {
                    GameBoard[i].BackgroundImage = Checkers.Properties.Resources.SithLogo;
                    GameBoard[i].BackgroundImageLayout = ImageLayout.Stretch;
                    GameBoard[i].FlatStyle = FlatStyle.Flat;
                    GameBoard[i].FlatAppearance.BorderColor = Color.Black;
                    GameBoard[i].Enabled = true;
                }
                else if (((i / 8) % 2 == 0 && (i % 8) % 2 == 0) || ((i / 8) % 2 != 0 && (i % 8) % 2 != 0))
                {
                    GameBoard[i].BackgroundImage = BackgroundImage = null;
                    GameBoard[i].BackColor = Color.Black;
                    GameBoard[i].FlatStyle = FlatStyle.Flat;
                    GameBoard[i].FlatAppearance.BorderColor = Color.Black;
                    GameBoard[i].Enabled = true;
                }

                else
                {
                    GameBoard[i].BackgroundImage = BackgroundImage = null;
                    GameBoard[i].BackColor = Color.Gainsboro;
                    GameBoard[i].ForeColor = Color.Gainsboro;
                    GameBoard[i].FlatStyle = FlatStyle.Flat;
                    GameBoard[i].Enabled = false;
                }
            }
        }

        private void PlayButton_Click(object sender, EventArgs e)
        {
            PlayButton.Enabled = false;
            Player1.Enabled = false;
            Player2.Enabled = false;

            // set AI difficulty if relevant
            if (Player1.SelectedItem.ToString() == "AI Easy" || Player2.SelectedItem.ToString() == "AI Easy") { GameAI.MaximunDepth = 4; }
            else if (Player1.SelectedItem.ToString() == "AI Medium" || Player2.SelectedItem.ToString() == "AI Medium") { GameAI.MaximunDepth = 8; }
            else if (Player1.SelectedItem.ToString() == "AI Hard" || Player2.SelectedItem.ToString() == "AI Hard") { GameAI.MaximunDepth = 12; }

            // Set up Board
            Gameboard.Enabled = true;
            Game.SetUpBoard();
            UpdateBoard();

            // initalise score and player
            pictureBox1.Image = Properties.Resources.rebbellogo;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            Score.Enabled = true;
            Score.Text = "0 : 0";

            // Play game
            if (Player1.SelectedItem.ToString() != "Human") { AiMove(); }
        }

        private void AiMove()
        {
            UI.ActiveForm.Text = "Thinking...";
            Gameboard.Enabled = false;
            backgroundWorker1.RunWorkerAsync();
            
        }


        private void Button_Click(object sender, EventArgs e)
        {
            var Button = (Button)sender;
            List<Button> GameBoardDisplay = Gameboard.Controls.OfType<Button>().ToList();
            GameBoardDisplay.Reverse(); // apparently the buttons are added backwards.

            // If no piece has been selected SelectedPeice = -1 else it is equal to the location of the chosen piece.
            if (SelectedPiece == -1)
            {

                // Select a piece belonging to the current player
                if (Game.Gameboard[Convert.ToInt32(Button.Tag)] == Game.CurrentPlayer || Game.Gameboard[Convert.ToInt32(Button.Tag)] == 2 * Game.CurrentPlayer)
                {
                    SelectedPiece = Convert.ToInt32(Button.Tag); // store original location

                    GameBoardDisplay[SelectedPiece].FlatAppearance.BorderSize = 3;
                    GameBoardDisplay[SelectedPiece].FlatAppearance.BorderColor = Color.Goldenrod;

                    // Looks for possible jumping moves that a given piece can make else shows no jumping moves.
                    JumpingMove = true;
                    Possiblemoves = _CanJump(Game.Gameboard, SelectedPiece / 8, SelectedPiece % 8, Game.CurrentPlayer);
                    if (!Possiblemoves.Any())
                    {
                        JumpingMove = false;
                        Possiblemoves = _CanMove(Game.Gameboard, SelectedPiece / 8, SelectedPiece % 8, Game.CurrentPlayer);
                    }

                    // Determine is any moves for that piece are possible throw error if there are none.
                    if (!Possiblemoves.Any())
                    {
                        MessageBox.Show("There are no valid moves for that piece!");
                        GameBoardDisplay[SelectedPiece].FlatAppearance.BorderSize = 3;
                        GameBoardDisplay[SelectedPiece].FlatAppearance.BorderColor = Color.Black;
                        SelectedPiece = -1;
                    }

                }

                else
                {
                    // Throw error message if you do not select your own piece.
                    MessageBox.Show("Select your own piece!");
                }
            }
            else
            {
                // De-select piece.
                if (Convert.ToInt32(Button.Tag) == SelectedPiece)
                {
                    if (JumpingMove == true) { _CanJump(Game.Gameboard, SelectedPiece / 8, SelectedPiece % 8, Game.CurrentPlayer); }
                    else { _CanMove(Game.Gameboard, SelectedPiece / 8, SelectedPiece % 8, Game.CurrentPlayer); }
                    
                    GameBoardDisplay[SelectedPiece].FlatAppearance.BorderSize = 3;
                    GameBoardDisplay[SelectedPiece].FlatAppearance.BorderColor = Color.Black;
                    SelectedPiece = -1;
                }

                else
                {
                    // make valid move

                    // determine if move is valid and store its location in list.
                    bool IsPresent = false;
                    int index = 0;
                    for (int i = 0; i < Possiblemoves.Count; i++)
                    {
                        if (Possiblemoves[i].Contains(Convert.ToInt32(Button.Tag))) { IsPresent = true; index = i; }
                    }

                    // if valid
                    if (IsPresent == true)
                    {
                        // Take opponents piece
                        if (JumpingMove == true) {Game.Gameboard[Possiblemoves[index][0]] = 0; }
                        
                        // update new position of player piece
                        if (Game.Gameboard[SelectedPiece] == Game.CurrentPlayer) { Game.Gameboard[Convert.ToInt32(Button.Tag)] = Game.CurrentPlayer; }
                        else Game.Gameboard[Convert.ToInt32(Button.Tag)] = 2 * Game.CurrentPlayer;

                        // promote piece is relevant
                        for (int i = 0; i < Game.Gameboard.Count; i++)
                        {
                            if (Game.Gameboard[i] == 1 && i < 8) { Game.Gameboard[i] = 2; }
                            else if (Game.Gameboard[i] == -1 && i > 56) { Game.Gameboard[i] = -2; }
                        }

                        Game.Gameboard[SelectedPiece] = 0;
                        Possiblemoves.Clear();
                        UpdateBoard();
                        
                        // if player just completed a jumping move check to see if more are now possible.
                        if (JumpingMove == true)
                        {
                            SelectedPiece = Convert.ToInt32(Button.Tag);

                            // check to see if any more jumps are now possible and if not end players turn.

                            Possiblemoves = _CanJump(Game.Gameboard, SelectedPiece / 8, SelectedPiece % 8, Game.CurrentPlayer);
                            if (!Possiblemoves.Any())
                            {
                                JumpingMove = false;

                                SelectedPiece = -1;
                                UpdateScore();
                                if (Game.CurrentPlayer == 1)
                                {
                                    Game.CurrentPlayer = -1;
                                    pictureBox1.Image = Properties.Resources.Empirelogo;
                                    pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                                }
                                else { Game.CurrentPlayer = 1; pictureBox1.Image = Properties.Resources.rebbellogo; pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage; }
                            }

                        }

                        else
                        {
                            SelectedPiece = -1;
                            UpdateScore();
                            if (Game.CurrentPlayer == 1) {
                                Game.CurrentPlayer = -1;
                                pictureBox1.Image = Properties.Resources.Empirelogo;
                                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                            }
                            else { Game.CurrentPlayer = 1; pictureBox1.Image = Properties.Resources.rebbellogo; pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage; }
                        }

                    }

                    
                    else
                    {
                        // throw error if selected move is not valid.
                        MessageBox.Show("Please select a valid move for that piece!");
                    }
                }

            }

            EndGame();

            if (Player1.SelectedItem.ToString() != "Human" && Game.CurrentPlayer == 1) { AiMove(); }
            else if (Player2.SelectedItem.ToString() != "Human" && Game.CurrentPlayer == -1) { AiMove(); }

        }

        private void UpdateScore()
        {
            int P1 = 0, P2 = 0;
            for (int i = 0; i < Game.Gameboard.Count; i++)
            {
                if (Game.Gameboard[i] > 0)
                {
                    P1 += Game.Gameboard[i];
                }
                else if (Game.Gameboard[i] < 0)
                {
                    P2 += Math.Abs(Game.Gameboard[i]);
                }
            }

            Score.Text = ((P1 - P2).ToString() + " : " + (P2 - P1).ToString());
        }

        private void EndGame()
        {
            int A = 0, B = 0;
            for (int i = 0; i < Game.Gameboard.Count; i++) { if (Game.Gameboard[i] > 0) { A++; } else if (Game.Gameboard[i] < 0) { B++; } }
            if (A > 0 && B > 0) { return; } // still pieces on board
            else { MessageBox.Show("Game Over!"); Gameboard.Enabled = false; PlayButton.Enabled = true; Player1.Enabled = true; Player2.Enabled = true; }
        }

        private List<List<int>> _CanJump(List<int> GameBoard, int X, int Y, int Player)
        {
            List<Button> GameBoardDisplay = Gameboard.Controls.OfType<Button>().ToList();
            GameBoardDisplay.Reverse(); // apparently the buttons are added backwards.
            List<List<int>> ValidPositions = new List<List<int>> { }; // store valid board locations at end of the move

            if (Game.Gameboard[8 * X + Y] == Player || Game.Gameboard[8 * X + Y] == 2 * Player)
            {

                // Jump forward and left

                // X = row on board Y = column. both have limits 0<X<8 & 0<Y<8
                // valid if opponent piece is placed diagonally next to player piece, and square behind is free.

                if ((X - 2 * Player >= 0 && X - 2 * Player < 8) && (Y - 2 >= 0) && (GameBoard[8 * (X - Player) + (Y - 1)] == -Player || GameBoard[8 * (X - Player) + (Y - 1)] == -2 * Player) && GameBoard[8 * (X - (2 * Player)) + (Y - 2)] == 0)
                {
                    List<int> JumpFL = new List<int> { 8 * (X - Player) + (Y - 1), 8 * (X - (2 * Player)) + (Y - 2) };
                    ValidPositions.Add(JumpFL);

                    // Highlight or unhighlight possible move locations.
                    if (GameBoardDisplay[8 * (X - (2 * Player)) + (Y - 2)].FlatAppearance.BorderColor == Color.LawnGreen)
                    {
                        GameBoardDisplay[8 * (X - (2 * Player)) + (Y - 2)].FlatAppearance.BorderSize = 3;
                        GameBoardDisplay[8 * (X - (2 * Player)) + (Y - 2)].FlatAppearance.BorderColor = Color.Black;
                    }
                    else
                    {
                        GameBoardDisplay[8 * (X - (2 * Player)) + (Y - 2)].FlatAppearance.BorderSize = 3;
                        GameBoardDisplay[8 * (X - (2 * Player)) + (Y - 2)].FlatAppearance.BorderColor = Color.LawnGreen;
                    }
                }


                // Jump forward and right
                if ((X - 2 * Player >= 0 && X - 2 * Player < 8) && (Y + 2 < 8) && (GameBoard[8 * (X - Player) + (Y + 1)] == -Player || GameBoard[8 * (X - Player) + (Y + 1)] == -2 * Player) && GameBoard[8 * (X - (2 * Player)) + (Y + 2)] == 0)
                {
                    List<int> JumpFR = new List<int> { 8 * (X - Player) + (Y + 1), 8 * (X - (2 * Player)) + (Y + 2) };
                    ValidPositions.Add(JumpFR);

                    // Highlight or unhighlight possible move locations.
                    if (GameBoardDisplay[8 * (X - (2 * Player)) + (Y + 2)].FlatAppearance.BorderColor == Color.LawnGreen)
                    {
                        GameBoardDisplay[8 * (X - (2 * Player)) + (Y + 2)].FlatAppearance.BorderSize = 3;
                        GameBoardDisplay[8 * (X - (2 * Player)) + (Y + 2)].FlatAppearance.BorderColor = Color.Black;
                    }
                    else
                    {
                        GameBoardDisplay[8 * (X - (2 * Player)) + (Y + 2)].FlatAppearance.BorderSize = 3;
                        GameBoardDisplay[8 * (X - (2 * Player)) + (Y + 2)].FlatAppearance.BorderColor = Color.LawnGreen;
                    }
                }
            }

            if (Game.Gameboard[8 * X + Y] == 2 * Player)
            {
                // Jump back and left
                if ((X + 2 * Player >= 0 && X + 2 * Player < 8) && (Y - 2 >= 0) && (GameBoard[8 * (X + Player) + (Y - 1)] == -Player || GameBoard[8 * (X + Player) + (Y - 1)] == -2 * Player) && GameBoard[8 * (X + (2 * Player)) + (Y - 2)] == 0)
                {
                    List<int> JumpFL = new List<int> { 8 * (X + Player) + (Y - 1), 8 * (X + (2 * Player)) + (Y - 2) };
                    ValidPositions.Add(JumpFL);

                    // Highlight or unhighlight possible move locations.
                    if (GameBoardDisplay[8 * (X + (2 * Player)) + (Y - 2)].FlatAppearance.BorderColor == Color.LawnGreen)
                    {
                        GameBoardDisplay[8 * (X + (2 * Player)) + (Y - 2)].FlatAppearance.BorderSize = 3;
                        GameBoardDisplay[8 * (X + (2 * Player)) + (Y - 2)].FlatAppearance.BorderColor = Color.Black;
                    }
                    else
                    {
                        GameBoardDisplay[8 * (X + (2 * Player)) + (Y - 2)].FlatAppearance.BorderSize = 3;
                        GameBoardDisplay[8 * (X + (2 * Player)) + (Y - 2)].FlatAppearance.BorderColor = Color.LawnGreen;
                    }
                }


                // Jump back and right
                if ((X + 2 * Player >= 0 && X + 2 * Player < 8) && (Y + 2 < 8) && (GameBoard[8 * (X + Player) + (Y + 1)] == -Player || GameBoard[8 * (X + Player) + (Y + 1)] == -2 * Player) && GameBoard[8 * (X + (2 * Player)) + (Y + 2)] == 0)
                {
                    List<int> JumpFR = new List<int> { 8 * (X + Player) + (Y + 1), 8 * (X + (2 * Player)) + (Y + 2) };
                    ValidPositions.Add(JumpFR);

                    // Highlight or unhighlight possible move locations.
                    if (GameBoardDisplay[8 * (X + (2 * Player)) + (Y + 2)].FlatAppearance.BorderColor == Color.LawnGreen)
                    {
                        GameBoardDisplay[8 * (X + (2 * Player)) + (Y + 2)].FlatAppearance.BorderSize = 3;
                        GameBoardDisplay[8 * (X + (2 * Player)) + (Y + 2)].FlatAppearance.BorderColor = Color.Black;
                    }
                    else
                    {
                        GameBoardDisplay[8 * (X + (2 * Player)) + (Y + 2)].FlatAppearance.BorderSize = 3;
                        GameBoardDisplay[8 * (X + (2 * Player)) + (Y + 2)].FlatAppearance.BorderColor = Color.LawnGreen;
                    }
                }
            }

            return ValidPositions;
        }

            
        private List<List<int>> _CanMove(List<int> GameBoard, int X, int Y, int Player)
        {
            List<Button> GameBoardDisplay = Gameboard.Controls.OfType<Button>().ToList();
            GameBoardDisplay.Reverse(); // apparently the buttons are added backwards.
            List<List<int>> ValidPositions = new List<List<int>> { }; // store valid board locations at end of the move

            // if piece is a pawn or a queen - can move forward only queen can move any number of squares in one direction for given diagonal

            if (Game.Gameboard[8 * X + Y] == Player || Game.Gameboard[8 * X + Y] == 2 * Player)
            {
                // forward and left
                int LUx = X, LUy = Y;
                
                while (LUy - 1 >= 0 && LUx - Player >= 0 && LUx - Player < 8 && Game.Gameboard[8 * (LUx - Player) + (LUy - 1)] == 0)
                {

                    List<int> Left = new List<int> { 8 * (LUx - Player) + (LUy - 1) };
                    ValidPositions.Add(Left);


                    // Highlight or unhighlight possible move locations.
                    if (GameBoardDisplay[8 * (LUx - Player) + (LUy - 1)].FlatAppearance.BorderColor == Color.Red)
                    {
                        GameBoardDisplay[8 * (LUx - Player) + (LUy - 1)].FlatAppearance.BorderSize = 3;
                        GameBoardDisplay[8 * (LUx - Player) + (LUy - 1)].FlatAppearance.BorderColor = Color.Black;
                    }
                    else
                    {
                        GameBoardDisplay[8 * (LUx - Player) + (LUy - 1)].FlatAppearance.BorderSize = 3;
                        GameBoardDisplay[8 * (LUx - Player) + (LUy - 1)].FlatAppearance.BorderColor = Color.Red;
                    }

                    if (Game.Gameboard[8 * X + Y] == Player) { break; } // break out of loop is pawn as can only move one square
                    LUx--;
                    LUy--;
                }

                // forward and right
                int RUx = X, RUy = Y;

                while (RUy + 1 < 8 && RUx - Player >= 0 && RUx - Player < 8 && GameBoard[8 * (RUx - Player) + (RUy + 1)] == 0)
                {
                    List<int> Right = new List<int> { 8 * (RUx - Player) + (RUy + 1) };
                    ValidPositions.Add(Right);
                    if (GameBoardDisplay[8 * (RUx - Player) + (RUy + 1)].FlatAppearance.BorderColor == Color.Red)
                    {
                        GameBoardDisplay[8 * (RUx - Player) + (RUy + 1)].FlatAppearance.BorderSize = 3;
                        GameBoardDisplay[8 * (RUx - Player) + (RUy + 1)].FlatAppearance.BorderColor = Color.Black;
                    }
                    else
                    {
                        GameBoardDisplay[8 * (RUx - Player) + (RUy + 1)].FlatAppearance.BorderSize = 3;
                        GameBoardDisplay[8 * (RUx - Player) + (RUy + 1)].FlatAppearance.BorderColor = Color.Red;
                    }

                    if (Game.Gameboard[8 * X + Y] == Player) { break; } // break out of loop is pawn as can only move one square
                    RUx--;
                    RUy++;
                }
            }

            // Queen can move backwards
            if (Game.Gameboard[8 * X + Y] == 2 * Player)
            {
                // Back and left
                int LDx = X, LDy = Y;

                while (LDy - 1 >= 0 && LDx + Player >= 0 && LDx + Player < 8 && Game.Gameboard[8 * (LDx + Player) + (LDy - 1)] == 0)
                {

                    List<int> Left = new List<int> { 8 * (LDx + Player) + (LDy - 1) };
                    ValidPositions.Add(Left);


                    // Highlight or unhighlight possible move locations.
                    if (GameBoardDisplay[8 * (LDx + Player) + (LDy - 1)].FlatAppearance.BorderColor == Color.Red)
                    {
                        GameBoardDisplay[8 * (LDx + Player) + (LDy - 1)].FlatAppearance.BorderSize = 3;
                        GameBoardDisplay[8 * (LDx + Player) + (LDy - 1)].FlatAppearance.BorderColor = Color.Black;
                    }
                    else
                    {
                        GameBoardDisplay[8 * (LDx + Player) + (LDy - 1)].FlatAppearance.BorderSize = 3;
                        GameBoardDisplay[8 * (LDx + Player) + (LDy - 1)].FlatAppearance.BorderColor = Color.Red;
                    }

                    LDx++;
                    LDy--;
                }

                // forward and right
                int RDx = X, RDy = Y;

                while (RDy + 1 < 8 && RDx + Player >= 0 && RDx + Player < 8 && GameBoard[8 * (RDx + Player) + (RDy + 1)] == 0)
                {
                    List<int> Right = new List<int> { 8 * (RDx + Player) + (RDy + 1) };
                    ValidPositions.Add(Right);
                    if (GameBoardDisplay[8 * (RDx + Player) + (RDy + 1)].FlatAppearance.BorderColor == Color.Red)
                    {
                        GameBoardDisplay[8 * (RDx + Player) + (RDy + 1)].FlatAppearance.BorderSize = 3;
                        GameBoardDisplay[8 * (RDx + Player) + (RDy + 1)].FlatAppearance.BorderColor = Color.Black;
                    }
                    else
                    {
                        GameBoardDisplay[8 * (RDx + Player) + (RDy + 1)].FlatAppearance.BorderSize = 3;
                        GameBoardDisplay[8 * (RDx + Player) + (RDy + 1)].FlatAppearance.BorderColor = Color.Red;
                    }

                    RDx++;
                    RDy++;
                }
            }
            
            return ValidPositions;
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Game.Gameboard = GameAI.MinMaxMove(Game.Gameboard, Game.CurrentPlayer);
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ActiveForm.Text = "Checkers";
            if (Game.CurrentPlayer == 1) { Game.CurrentPlayer = -1; }
            else { Game.CurrentPlayer = 1; }
            Gameboard.Enabled = true;
            UpdateBoard();

            if (Player1.SelectedItem.ToString() != "Human" && Game.CurrentPlayer == 1) { AiMove(); }
            else if (Player2.SelectedItem.ToString() != "Human" && Game.CurrentPlayer == -1) { AiMove(); }
        }
    }
}
