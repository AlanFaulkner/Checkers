using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers
{
    class GamePlay
    {
        List<double> GameBoard = new List<double> { };
        List<double> GameBoardTemp = new List<double> { }; // temporary copy of game board used to evaluate move

        private List<List<int>> PossibleMoves = new List<List<int>> { };
        private List<List<double>> MoveScores = new List<List<double>> { }; // each row is a resulting move made and the new score on the board. score = respective difference between pieces

        int player = 1;

        // Public access to game board list.
        public List<double> Gameboard
        {
            get { return GameBoard; }
            set { GameBoard = value; }
        }

        public int Player // sets player turn to either 1 or -1
        {
            get { return player; }
            set { player = value; }
        }


        // ##########################################
        // ## Functions relating to the game board ##
        // ##########################################

        public void SetUpBoard() // initializes game board
        {
            List<double> gameboard = new List<double> { };

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (i < 3)
                    {
                        if (i % 2 != 0 && j % 2 != 0) { gameboard.Add(-1); }
                        else if (i % 2 == 0 && j % 2 == 0) { gameboard.Add(-1); }
                        else gameboard.Add(0);
                    }
                    else if (i > 4)
                    {
                        if (i % 2 != 0 && j % 2 != 0) { gameboard.Add(1); }
                        else if (i % 2 == 0 && j % 2 == 0) { gameboard.Add(1); }
                        else gameboard.Add(0);
                    }
                    else gameboard.Add(0);
                }
            }

            Gameboard = gameboard;
        }

        public void EmptyBoard()
        {
            Gameboard.Clear();
            for (int i = 0; i < 64; i++) {
                Gameboard.Add(0);
            }
        }

        private int _Get1DLocation(int X, int Y) { return 8 * X + Y; }

        private List<int> _Get2DLocation(int X) { List<int> CartCoordinates = new List<int> { X / 8, X % 8 }; return CartCoordinates; }

        // ###################################
        // ## Functions related to movement ##
        // ###################################

        public void GetPossibleMoves()
        {


            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {

                    if (Gameboard[_Get1DLocation(i, j)] == Player) // evaluates un-crowned pieces for given player
                    {
                        // Player must preform a jumping if possible
                        List<int> Move = new List<int> { };
                        _CanJump(i, j, Move);
                        if (!PossibleMoves[PossibleMoves.Count - 1].Any()) { PossibleMoves.RemoveAt(PossibleMoves.Count - 1); } // crude fix for when jumping not possible
                        // determines if an un-crowned piece can move to an empty square that is one diagonal space away from its starting location

                        if ((Gameboard[8 * (i - Player) + j - 1] == 0 && j - 1 >= 0) && (i - Player >= 0 || i - Player < 8))
                        {
                            List<int> Row = new List<int> { i, j, i - Player, j - 1 };
                            PossibleMoves.Add(Row);
                        }

                        if ((Gameboard[8 * (i - Player) + j + 1] == 0 && j + 1 < 8) && (i - Player >= 0 || i - Player < 8))
                        {
                            List<int> Row = new List<int> { i, j, i - Player, j + 1 };
                            PossibleMoves.Add(Row);
                        }
                    }
                }
            }

            AnaysizeJumps();
            List<double> Scores = MinMax(MoveScores);

            if (Player == 1)
            {
                List<double> NewState = MoveScores[(int)Scores[2]];
                NewState.RemoveAt(NewState.Count - 1);
                Gameboard.Clear();
                GameBoard.AddRange(NewState);
                MoveScores.Clear();
                PossibleMoves.Clear();

                Player = -1;
            }

            else if (Player == -1)
            {
                List<double> NewState = MoveScores[(int)Scores[0]];
                NewState.RemoveAt(NewState.Count - 1);
                Gameboard.Clear();
                GameBoard.AddRange(NewState);
                MoveScores.Clear();
                PossibleMoves.Clear();

                Player = 1;
            }
            Console.Write("Made this Move!\n");
            PrintGameBoard2D(GameBoard);
            //Console.Write("\nScore: " + GetScore(GameBoardTemp));
        }

        public void _CanJump(int X, int Y, List<int> Play)
        {
            List<int> Move = new List<int> { };
            Move.AddRange(Play);
            bool A = true, B = true;
            // jump forward and left
            if (X - 2 * Player < 8 && X - 2 * Player >= 0 && Y - 2 >= 0 && Gameboard[_Get1DLocation(X - Player, Y - 1)] == -Player && Gameboard[_Get1DLocation(X - (2 * Player), Y - 2)] == 0)
            {

                if (!Move.Any()) // Starting location
                {
                    Move.Add(X);
                    Move.Add(Y);
                }

                Move.Add(X - Player);
                Move.Add(Y - 1);
                Move.Add(X - 2 * Player);
                Move.Add(Y - 2);

                _CanJump(X - 2 * Player, Y - 2, Move);

            }
            else { A = false; }

            Move.Clear();
            Move.AddRange(Play);

            // jump forward and right
            if (X - 2 * Player < 8 && X - 2 * Player >= 0 && Y + 2 < 8 && Gameboard[_Get1DLocation(X - Player, Y + 1)] == -Player && Gameboard[_Get1DLocation(X - (2 * Player), Y + 2)] == 0)
            {
                if (!Move.Any()) // Starting location
                {
                    Move.Add(X);
                    Move.Add(Y);
                }

                Move.Add(X - Player);
                Move.Add(Y + 1);
                Move.Add(X - 2 * Player);
                Move.Add(Y + 2);

                _CanJump(X - 2 * Player, Y + 2, Move);
            }
            else { B = false; }

            if (A == false && B == false) { PossibleMoves.Add(Move); }
        }

        private void AnaysizeJumps()
        {
            // Update game board with given move
            for (int i = 0; i < PossibleMoves.Count; i++)
            {
                GameBoardTemp.Clear();
                GameBoardTemp.AddRange(Gameboard);

                for (int j = 0; j < PossibleMoves[i].Count-2; j = j + 2)
                {
                    GameBoardTemp[_Get1DLocation(PossibleMoves[i][j], PossibleMoves[i][j + 1])] = 0;
                }
                GameBoardTemp[_Get1DLocation(PossibleMoves[i][PossibleMoves[i].Count - 2], PossibleMoves[i][PossibleMoves[i].Count - 1])] = Player;
                List<double> MoveRow = new List<double> { };
                MoveRow.AddRange(GameBoardTemp);
                MoveRow.Add(GetScore(GameBoardTemp));
                MoveScores.Add(MoveRow);
                //PrintGameBoard2D(GameBoardTemp);
                Console.Write("Score: " + GetScore(GameBoardTemp) + "\n");
            }
        }

        private List<double> MinMax(List<List<double>> MoveList)
        {
            // returns the min and max values of a list of possible moves along with the relevant move ID's denoted MinI and MaxI
            double Min = MoveList[0][64], MinI = 0, Max = MoveList[0][64], MaxI = 0;
            for (int i = 0; i < MoveList.Count; i++)
            {
                if (Min > MoveList[i][64]) { Min = MoveList[i][64]; MinI = i; }
                else if (Max < MoveList[i][64]) { Max = MoveList[i][64]; MaxI = i; }
            }

            List<double> MinMax = new List<double> { MinI,Min, MaxI,Max };

            return MinMax;
        }

        // ################################
        // ## End Move related functions ##
        // ################################

        private void PromotePeices()
        {
            for (int i=0; i < 64; i++) {
                if (Gameboard[i]==1 && i < 8) { Gameboard[i]++; }
                else if (Gameboard[i]==-1 && i > 55) { Gameboard[i]--; }
            }
        }

        // ##################################
        // ## Functions related to scoring ##
        // ##################################
        
        private int GetScore(List<double> Board)
        {
            // Determine if and pieces left on board
            int A = 0, B = 0;
            for (int i = 0; i < Board.Count; i++)
            {
                if (Board[i] == 1) { A += (int)Board[i]; }
                else if (Board[i] == -1) { B += (int)Board[i]; }
            }

            if (Player==1 && B == 0) { return 100; } // 100 point if win (this is randomly chosen)
            if (Player == -1 && A == 0) { return -100; } // 100 points to loose
            else return A + B; // piece difference
        }

        // #########################################
        // ## Useful functions used for debugging ##
        // #########################################

        
        public void PrintGameBoard2D(List<double> Board) // prints a pretty typed game board to console window.
        {
            Console.WindowHeight = 35; // set number of rows for console window height
            
            for (int i = 0; i < 8; i++)
            {
                if (i == 0) { Console.Write("---------------------------------\n"); }
                Console.Write("|   |   |   |   |   |   |   |   |\n");
                for (int j = 0; j < 8; j++)
                {
                    if (j == 0) { Console.Write("|"); }
                    Console.Write(" ");
                    if (Board[8 * i + j] == 1) { Console.Write("o"); }
                    else if (Board[8 * i + j] == -1) Console.Write("x");
                    else if (Board[8 * i + j] == 2) { Console.Write("O"); }
                    else if (Board[8 * i + j] == -2) { Console.Write("X"); }
                    else Console.Write(" ");
                    Console.Write(" |");
                }
                Console.Write("\n|   |   |   |   |   |   |   |   |\n");
                Console.Write("---------------------------------\n");
            }

            Console.WriteLine();  
        }
    }
}
