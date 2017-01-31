using System;
using System.Collections.Generic;
using System.Linq;

namespace Checkers
{
    internal class MinMax
    {
        private int MaxDepth = 8;       // Max number of levels for game tree

        public int MaximunDepth
        {
            get { return MaxDepth; }
            set { MaxDepth = value; }
        }

        private bool EndGame(List<int> GameState)
        {
            int P1 = 0;
            int P2 = 0;

            for (int i = 0; i < GameState.Count; i++)
            {
                if (GameState[i] > 0) { P1++; }
                else if (GameState[i] < 0) { P2++; }
            }

            if (P1 == 0 || P2 == 0) { return true; }
            else { return false; }
        }

        private List<List<int>> DetermineMoves(List<int> GameBoard, int Player)
        {
            List<List<int>> PossibleMoves = new List<List<int>> { };

            // Proritise Jumping moves to enforce taking of peices
            for (int i = 0; i < GameBoard.Count; i++)
            {
                if (GameBoard[i] == Player || GameBoard[i] == 2 * Player)
                {
                    PossibleMoves.AddRange(Jumps(GameBoard, i / 8, i % 8, Player));
                }
            }

            if (!PossibleMoves.Any())
            {
                for (int i = 0; i < GameBoard.Count; i++)
                {
                    if (GameBoard[i] == Player || GameBoard[i] == 2 * Player)
                    {
                        PossibleMoves.AddRange(_CanMove(GameBoard, i / 8, i % 8));
                    }
                }
            }

            return PossibleMoves;
        }

        private List<List<int>> Jumps(List<int> GameBoard, int X, int Y, int Player)
        {
            List<List<int>> Moves = new List<List<int>> { };

            // Determine all aviable Jumping moves by recussion.
            List<List<int>> Jump = _CanJump(GameBoard, X, Y, Player);
            if (Jump.Any())
            {
                for (int i = 0; i < Jump.Count; i++)
                {
                    // convert jump cordinates into new game board
                    List<int> NewGameState = new List<int> { };
                    NewGameState.AddRange(GameBoard);
                    NewGameState[Jump[i][2]] = NewGameState[Jump[i][0]];
                    NewGameState[Jump[i][0]] = 0;
                    NewGameState[Jump[i][1]] = 0;

                    // promote peice if needed
                    if (Jump[i][2] / 8 == 0 && Player == 1) { NewGameState[Jump[i][2]] = 2; }
                    else if (Jump[i][2] / 8 == 7 && Player == -1) { NewGameState[Jump[i][2]] = -2; }

                    List<List<int>> FutureJumps = Jumps(NewGameState, Jump[i][2] / 8, Jump[i][2] % 8, Player);
                    if (FutureJumps.Any()) { Moves.AddRange(FutureJumps); }
                    else { Moves.Add(NewGameState); }
                }
            }

            return Moves;
        }

        public List<int> MinMaxMove(List<int> GameBoard, int Player, int Depth = 1)
        {
            List<List<int>> MoveList = DetermineMoves(GameBoard, Player);

            if (Depth >= MaxDepth || EndGame(GameBoard) || !MoveList.Any())
            {
                GameBoard.Add(ScoreGameState(GameBoard, Player));
                return GameBoard;
            }

            for (int i = 0; i < MoveList.Count; i++)
            {
                List<int> NextMove = MinMaxMove(MoveList[i], -Player, Depth + 1);
                MoveList[i].Add(NextMove[NextMove.Count - 1]);
            }

            if (Depth % 2 != 0) { MoveList = MoveList.OrderByDescending(a => a[64]).ToList(); }
            else { MoveList = MoveList.OrderBy(a => a[64]).ToList(); }

            List<int> SelectedMove = new List<int> { };
            for (int i = 0; i < 64; i++) { SelectedMove.Add(MoveList[0][i]); }

            return SelectedMove;
        }

        private void PrintResult(List<List<int>> Data)
        {
            for (int i = 0; i < Data.Count; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    for (int k = 0; k < 8; k++)
                    {
                        Console.Write($"{ Data[i][8 * j + k],3}");
                    }
                    Console.WriteLine();
                }
                //  Console.Write("Score = " + Data[i][64] + "\n");
                Console.WriteLine();
            }

            Console.WriteLine();
        }

        private int ScoreGameState(List<int> State, int Player)
        {
            int P1_PeiceCount = 0;
            int P2_PeiceCount = 0;

            // calculate number of peices each person has
            for (int i = 0; i < State.Count; i++)
            {
                if (State[i] > 0) { P1_PeiceCount += State[i]; }
                else { P2_PeiceCount += Math.Abs(State[i]); }
            }

            if (Player == 1)
            {
                return P1_PeiceCount - P2_PeiceCount;
            }
            else
            {
                return P2_PeiceCount - P1_PeiceCount;
            }
        }

        private List<List<int>> _CanMove(List<int> GameBoard, int X, int Y)
        {
            // return list of valid positions first value is the original locacation of peice second number is possible end location after move has been made.

            List<List<int>> ValidPositions = new List<List<int>> { }; // store valid board locations at end of the move

            // Move direction is relative to white
            // If piece is a white pawn or a queen of any colour
            // White pawn can move forward only one square up left and up rigth.
            // Queen can move any number of squares in one direction for given diagonal

            if (GameBoard[8 * X + Y] != -1)
            {
                // forward and left
                int LUx = X, LUy = Y;

                while (LUy - 1 >= 0 && LUx - 1 >= 0 && LUx - 1 < 8 && GameBoard[8 * (LUx - 1) + (LUy - 1)] == 0)
                {
                    List<int> Left = new List<int> { };
                    Left.AddRange(GameBoard);
                    if (LUx == 0) { Left[8 * (LUx - 1) + (LUy - 1)] = 2; }
                    else { Left[8 * (LUx - 1) + (LUy - 1)] = Left[8 * X + Y]; }
                    Left[8 * X + Y] = 0;
                    ValidPositions.Add(Left);

                    if (GameBoard[8 * X + Y] == 1) { break; } // break out of loop is pawn as can only move one square
                    LUx--;
                    LUy--;
                }

                // forward and right
                int RUx = X, RUy = Y;

                while (RUy + 1 < 8 && RUx - 1 >= 0 && RUx - 1 < 8 && GameBoard[8 * (RUx - 1) + (RUy + 1)] == 0)
                {
                    List<int> Right = new List<int> { };
                    Right.AddRange(GameBoard);
                    if (RUx == 0) { Right[8 * (RUx - 1) + (RUy + 1)] = 2; }
                    else { Right[8 * (RUx - 1) + (RUy + 1)] = Right[8 * X + Y]; }
                    Right[8 * X + Y] = 0;
                    ValidPositions.Add(Right);

                    if (GameBoard[8 * X + Y] == 1) { break; } // break out of loop is pawn as can only move one square
                    RUx--;
                    RUy++;
                }
            }

            // Red Pawn
            // Queen can move backwards
            if (GameBoard[8 * X + Y] != 1)
            {
                // Back and left
                int LDx = X, LDy = Y;

                while (LDy - 1 >= 0 && LDx + 1 >= 0 && LDx + 1 < 8 && GameBoard[8 * (LDx + 1) + (LDy - 1)] == 0)
                {
                    List<int> Left = new List<int> { };
                    Left.AddRange(GameBoard);
                    if (LDx == 8) { Left[8 * (LDx + 1) + (LDy - 1)] = -2; }
                    else { Left[8 * (LDx + 1) + (LDy - 1)] = Left[8 * X + Y]; }
                    Left[8 * X + Y] = 0;
                    ValidPositions.Add(Left);

                    if (GameBoard[8 * X + Y] == -1) { break; } // break out of loop is pawn as can only move one square
                    LDx++;
                    LDy--;
                }

                // forward and right
                int RDx = X, RDy = Y;

                while (RDy + 1 < 8 && RDx + 1 >= 0 && RDx + 1 < 8 && GameBoard[8 * (RDx + 1) + (RDy + 1)] == 0)
                {
                    List<int> Right = new List<int> { };
                    Right.AddRange(GameBoard);
                    if (RDx == 8) { Right[8 * (RDx + 1) + (RDy + 1)] = -2; }
                    else { Right[8 * (RDx + 1) + (RDy + 1)] = Right[8 * X + Y]; }
                    Right[8 * X + Y] = 0;
                    ValidPositions.Add(Right);

                    if (GameBoard[8 * X + Y] == -1) { break; } // break out of loop is pawn as can only move one square
                    RDx++;
                    RDy++;
                }
            }

            return ValidPositions;
        }

        private List<List<int>> _CanJump(List<int> GameBoard, int X, int Y, int Player)
        {
            List<List<int>> ValidPositions = new List<List<int>> { }; // store valid board locations at end of the move

            if (GameBoard[8 * X + Y] != -1)
            {
                // Jump forward and left

                // X = row on board Y = column. both have limits 0<X<8 & 0<Y<8
                // valid if opponent piece is placed diagonally next to player piece, and square behind is free.

                if ((X - 2 >= 0 && X - 2 < 8) && (Y - 2 >= 0) && (GameBoard[8 * (X - 1) + (Y - 1)] == -Player || GameBoard[8 * (X - 1) + (Y - 1)] == -2 * Player) && GameBoard[8 * (X - 2) + (Y - 2)] == 0)
                {
                    List<int> JumpFL = new List<int> { 8 * X + Y, 8 * (X - 1) + (Y - 1), 8 * (X - (2)) + (Y - 2) };
                    ValidPositions.Add(JumpFL);
                }

                // Jump forward and right
                if ((X - 2 >= 0 && X - 2 < 8) && (Y + 2 < 8) && (GameBoard[8 * (X - 1) + (Y + 1)] == -Player || GameBoard[8 * (X - 1) + (Y + 1)] == -2 * Player) && GameBoard[8 * (X - 2) + (Y + 2)] == 0)
                {
                    List<int> JumpFR = new List<int> { 8 * X + Y, 8 * (X - 1) + (Y + 1), 8 * (X - (2)) + (Y + 2) };
                    ValidPositions.Add(JumpFR);
                }
            }

            if (GameBoard[8 * X + Y] != 1)
            {
                // Jump back and left
                if ((X + 2 >= 0 && X + 2 < 8) && (Y - 2 >= 0) && (GameBoard[8 * (X + 1) + (Y - 1)] == -Player || GameBoard[8 * (X + 1) + (Y - 1)] == -2 * Player) && GameBoard[8 * (X + 2) + (Y - 2)] == 0)
                {
                    List<int> JumpBL = new List<int> { 8 * X + Y, 8 * (X + 1) + (Y - 1), 8 * (X + 2) + (Y - 2) };
                    ValidPositions.Add(JumpBL);
                }

                // Jump back and right
                if ((X + 2 >= 0 && X + 2 < 8) && (Y + 2 < 8) && (GameBoard[8 * (X + 1) + (Y + 1)] == -Player || GameBoard[8 * (X + 1) + (Y + 1)] == -2 * Player) && GameBoard[8 * (X + 2) + (Y + 2)] == 0)
                {
                    List<int> JumpBR = new List<int> { 8 * X + Y, 8 * (X + 1) + (Y + 1), 8 * (X + 2) + (Y + 2) };
                    ValidPositions.Add(JumpBR);
                }
            }

            return ValidPositions;
        }
    }
}