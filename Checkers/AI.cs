using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers
{
    class AI
    {
        /*
         * The Class deals with Determining the best Move for a given game state
         * only one public function is present
         */

        private int MaxDepth = 6;

        public int MaximunDepth
        {
            get { return MaxDepth; }
            set { MaxDepth = value; }
        }

        private int GetScore(List<int> Board, int Player)
        {
            // Determine if and pieces left on board
            int A = 0, B = 0;
            for (int i = 0; i < Board.Count; i++)
            {
                if (Board[i] == 1) { A += Board[i]; }
                else if (Board[i] == -1) { B += Board[i]; }
            }

            if (Player == 1 && B == 0) { return 100; } // 100 point if win (this is randomly chosen)
            if (Player == -1 && A == 0) { return -100; } // 100 points to loose
            else return A + B; // piece difference
        }

        public List<int> MinMaxMove(List<int> GameBoard, int Player, int Depth = 0)
        {
            // Game won
            if (GetScore(GameBoard,Player)==100 || GetScore(GameBoard,Player) == -100) { return GameBoard; }
            
            List<List<int>> PossibleMoves = new List<List<int>> { };
            List<int> ScoredMoves = new List<int> { };

            //// Determine if any jumping moves are possible
            for (int i = 0; i < GameBoard.Count; i++)
            {
                if (GameBoard[i] == Player) {
                    List<List<int>> Moves = _CanJump(GameBoard, i / 8, i % 8, Player);
                    if (!Moves[0].SequenceEqual(GameBoard)) { PossibleMoves.AddRange(Moves); }
                }

                else if (GameBoard[i] == 2 * Player)
                {
                    List<List<int>> Moves = _CanJumpQueen(GameBoard, i / 8, i % 8, Player);
                    if (!Moves[0].SequenceEqual(GameBoard)) { PossibleMoves.AddRange(Moves); }
                }
            }
            
            // if no jumping moves are possible determine all possible none jumping moves for a given game state

            if (!PossibleMoves.Any())
            {
                for (int i = 0; i < GameBoard.Count; i++)
                {
                    if (GameBoard[i] == Player) { PossibleMoves.AddRange(_CanMove(GameBoard, i / 8, i % 8, Player)); }
                }
            }

            // If no moves can be made
            if (!PossibleMoves.Any()) { return GameBoard; }

            // Promote piece to Queen if relevant.
            for (int i = 0; i < PossibleMoves.Count; i++) { PossibleMoves[i]=PromotePawn(PossibleMoves[i]); }
            
            // If at max Depth return best game board
            if (Depth == MaxDepth)
            {
                for (int i = 0; i < PossibleMoves.Count; i++)
                {
                    ScoredMoves.Add(GetScore(PossibleMoves[i], Player));
                }

                // Sort moves
                int Min = ScoredMoves.Last(), Max = ScoredMoves.Last(), MinI = 0, MaxI = 0;
                for (int i = 0; i < ScoredMoves.Count; i++)
                {
                    if (ScoredMoves[i] < Min) { Min = ScoredMoves[i]; MinI = i; }
                    else if (ScoredMoves[i] > Max) { Max = ScoredMoves[i]; MaxI = i; }
                }

                if (Player == 1)
                {
                    //PrintGameBoard2D(PossibleMoves[MaxI]);
                    return PossibleMoves[MaxI];
                }

                else
                {
                    //PrintGameBoard2D(PossibleMoves[MinI]);
                    return PossibleMoves[MinI];
                }
            }

            else {
                for (int i = 0; i < PossibleMoves.Count; i++)
                {
                    ScoredMoves.Add(Depth+1-GetScore(MinMaxMove(PossibleMoves[i], -1 * Player, Depth + 1), Player));
                }

                // Sort moves
                int Min = ScoredMoves.Last(), Max = ScoredMoves.Last(), MinI = 0, MaxI = 0;
                for (int i = 0; i < ScoredMoves.Count; i++)
                {
                    if (ScoredMoves[i] < Min) { Min = ScoredMoves[i]; MinI = i; }
                    else if (ScoredMoves[i] > Max) { Max = ScoredMoves[i]; MaxI = i; }
                }

                if (Player == 1)
                {
                    //PrintGameBoard2D(PossibleMoves[MaxI]);
                    return PossibleMoves[MaxI];
                }

                else
                {
                    //PrintGameBoard2D(PossibleMoves[MinI]);
                    return PossibleMoves[MinI];
                }
            }
        }

        public List<int> PromotePawn(List<int> GameState)
        {
           for (int i = 0; i < GameState.Count; i++)
            {
                if (GameState[i] == 1 && i<8) { GameState[i] = 2; }
                else if (GameState[i] == -1 && i > 56) { GameState[i] = -2; }
            }

            return GameState;
        }

        private List<List<int>> _CanJump (List<int> GameBoard, int X, int Y, int Player)
        {
            List<List<int>> JumpList = new List<List<int>> { };

            bool A = true, B = true;
            
            // jump forward and left
            if (X - 2 * Player < 8 && X - 2 * Player >= 0 && Y - 2 >= 0 && (GameBoard[8*(X - Player)+(Y - 1)] == -Player || GameBoard[8 * (X - Player) + (Y - 1)] == -2*Player) && GameBoard[8*(X - (2 * Player))+(Y - 2)] == 0)
            {
                List<int> Move = new List<int> { };
                Move.AddRange(GameBoard);
                Move[8 * X + Y] = 0;
                Move[8*(X - Player)+(Y-1)] = 0;
                Move[8 * (X - (2 * Player)) + (Y - 2)] = Player;

                JumpList.AddRange(_CanJump(Move, X - 2 * Player, Y - 2, Player));                
            }
            else { A = false; }


            // jump forward and right
            if (X - 2 * Player < 8 && X - 2 * Player >= 0 && Y + 2 < 8 && (GameBoard[8 * (X - Player) + (Y - 1)] == -Player || GameBoard[8 * (X - Player) + (Y - 1)] == -2*Player) && GameBoard[8 * (X - (2 * Player)) + (Y + 2)] == 0)
            {
                List<int> Move = new List<int> { };
                Move.AddRange(GameBoard);
                Move[8 * X + Y] = 0;
                Move[8 * (X - Player) + (Y + 1)] = 0;
                Move[8 * (X - (2 * Player)) + (Y + 2)] = Player;

                JumpList.AddRange(_CanJump(Move, X - 2 * Player, Y + 2, Player));
            }
            else { B = false; }

            if (A == false && B == false)
            {
                JumpList.Add(GameBoard);
                return JumpList;
            }
            else
            {
                return JumpList;
            }
        }

        private List<List<int>> _CanJumpQueen(List<int> GameBoard, int X, int Y, int Player)
        {
            List<List<int>> JumpList = new List<List<int>> { };

            bool A = true, B = true, C = true, D = true;

            // jump Up and left
            if (X - 1 < 8 && X - 1 >= 0 && X - 2 * 1 < 8 && X - 2 * 1 >= 0 && Y - 2 >= 0 && GameBoard[8 * (X - 1) + (Y - 1)] == -Player && GameBoard[8 * (X - (2 * 1)) + (Y - 2)] == 0)
            {
                List<int> Move = new List<int> { };
                Move.AddRange(GameBoard);
                Move[8 * X + Y] = 0;
                Move[8 * (X - 1) + (Y - 1)] = 0;
                Move[8 * (X - (2 * 1)) + (Y - 2)] = 2*Player;

                JumpList.AddRange(_CanJumpQueen(Move, X - 2 * 1, Y - 2, Player));
            }
            else { A = false; }


            // jump up and right
            if (X - 1 < 8 && X - 1 >= 0 && X - 2 * 1 < 8 && X - 2 * 1 >= 0 && Y + 2 >= 0 && GameBoard[8 * (X - 1) + (Y + 1)] == -Player && GameBoard[8 * (X - (2 * 1)) + (Y + 2)] == 0)
            {
                List<int> Move = new List<int> { };
                Move.AddRange(GameBoard);
                Move[8 * X + Y] = 0;
                Move[8 * (X - 1) + (Y + 1)] = 0;
                Move[8 * (X - (2 * 1)) + (Y + 2)] = 2*Player;

                JumpList.AddRange(_CanJumpQueen(Move, X - 2 * Player, Y + 2, Player));
            }
            else { B = false; }

            // jump down and left
            if (X + 1 < 8 && X + 1 >= 0 && X + 2 * 1 < 8 && X + 2 * 1 >= 0 && Y - 2 >= 0 && GameBoard[8 * (X + 1) + (Y - 1)] == -Player && GameBoard[8 * (X + (2 * 1)) + (Y - 2)] == 0)
            {
                List<int> Move = new List<int> { };
                Move.AddRange(GameBoard);
                Move[8 * X + Y] = 0;
                Move[8 * (X + 1) + (Y - 1)] = 0;
                Move[8 * (X + (2 * 1)) + (Y - 2)] = 2*Player;

                JumpList.AddRange(_CanJumpQueen(Move, X + 2 * 1, Y - 2, Player));
            }
            else { C = false; }

            // jump down and right
            if (X + 1 < 8 && X + 1 >= 0 && X + 2 * 1 < 8 && X + 2 * 1 >= 0 && Y + 2 >= 0 && GameBoard[8 * (X + 1) + (Y + 1)] == -Player && GameBoard[8 * (X + (2 * 1)) + (Y + 2)] == 0)
            {
                List<int> Move = new List<int> { };
                Move.AddRange(GameBoard);
                Move[8 * X + Y] = 0;
                Move[8 * (X + 1) + (Y + 1)] = 0;
                Move[8 * (X + (2 * 1)) + (Y + 2)] = 2*Player;

                JumpList.AddRange(_CanJumpQueen(Move, X + 2 * Player, Y + 2, Player));
            }
            else { D = false; }

            if (A == false && B == false && C == false && D == false)
            {
                JumpList.Add(GameBoard);
                return JumpList;
            }
            else
            {
                return JumpList;
            }
        }

        private List<List<int>> _CanMove(List<int> GameBoard, int X, int Y, int Player)
        {
            List<List<int>> MoveList = new List<List<int>> { };
            
            // if piece is a queen - can move any number of squares in one direction for given diagonal
            if (GameBoard[8 * X + Y] == 2 * Player)
            {
                // Move up and to the left
                List<int> LeftUpMove = new List<int> { };
                LeftUpMove.AddRange(GameBoard);
                int LUx = X, LUy = Y;

                while (LUx - 1 >= 0 && LUx - 1 < 8 && LeftUpMove[8 * (LUx - 1) + LUy - 1] == 0 && LUy - 1 >= 0)
                {
                    LeftUpMove[8 * LUx + LUy] = 0;
                    LeftUpMove[8 * (LUx - 1) + LUy - 1] = 2 * Player;
                    List<int> Move = new List<int> { };
                    Move.AddRange(LeftUpMove);
                    MoveList.Add(Move);
                    LUx--;
                    LUy--;
                }

                // Move up and to the right
                List<int> RightUpMove = new List<int> { };
                RightUpMove.AddRange(GameBoard);
                int RUx = X, RUy = Y;

                while (RUy + 1 < 8 && RUx - 1 >= 0 && RUx - 1 < 8 && RightUpMove[8 * (RUx - 1) + RUy + 1] == 0)
                {
                    RightUpMove[8 * RUx + RUy] = 0;
                    RightUpMove[8 * (RUx - 1) + RUy + 1] = 2 * Player;
                    List<int> Move = new List<int> { };
                    Move.AddRange(RightUpMove);
                    MoveList.Add(Move);
                    RUx--;
                    RUy++;
                }

                // Move down and to the left
                List<int> LeftDownMove = new List<int> { };
                LeftDownMove.AddRange(GameBoard);
                int LDx = X, LDy = Y;

                while (LDy - 1 >= 0 && LDx + 1 >= 0 && LDx + 1 < 8 && LeftDownMove[8 * (LDx + 1) + LDy - 1] == 0)
                {
                    LeftDownMove[8 * LDx + LDy] = 0;
                    LeftDownMove[8 * (LDx + 1) + LDy - 1] = 2 * Player;
                    List<int> Move = new List<int> { };
                    Move.AddRange(LeftDownMove);
                    MoveList.Add(Move);
                    LDx++;
                    LDy--;
                }

                // Move down and to the right
                List<int> RightDownMove = new List<int> { };
                RightDownMove.AddRange(GameBoard);
                int RDx = X, RDy = Y;

                while (RDy + 1 < 8 && RDx + 1 >= 0 && RDx + 1 < 8 && RightDownMove[8 * (RDx + 1) + RDy + 1] == 0)
                {
                    RightDownMove[8 * RDx + RDy] = 0;
                    RightDownMove[8 * (RDx + 1) + RDy + 1] = 2 * Player;
                    List<int> Move = new List<int> { };
                    Move.AddRange(RightDownMove);
                    MoveList.Add(Move);
                    RDx++;
                    RDy++;
                }

                return MoveList;
            }

            else
            {

                List<int> LeftUpMove = new List<int> { };
                LeftUpMove.AddRange(GameBoard);
                if (Y - 1 >= 0 && X - Player >= 0 && X - Player < 8 && LeftUpMove[8 * (X - Player) + (Y - 1)] == 0)
                {
                    LeftUpMove[8 * X + Y] = 0;
                    LeftUpMove[8 * (X - Player) + (Y - 1)] = Player;
                    MoveList.Add(LeftUpMove);
                }

                List<int> RightUpMove = new List<int> { };
                RightUpMove.AddRange(GameBoard);
                if (Y + 1 < 8 && X - Player >= 0 && X - Player < 8 && RightUpMove[8 * (X - Player) + (Y + 1)] == 0)
                {
                    RightUpMove[8 * X + Y] = 0;
                    RightUpMove[8 * (X - Player) + (Y + 1)] = Player;
                    MoveList.Add(RightUpMove);
                }

                return MoveList;
            }
        }



        /*
         * Debugging functions 
         */

        public void PrintGameBoard2D(List<int> Board) // prints a pretty typed game board to console window.
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
