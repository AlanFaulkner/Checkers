using System;
using System.Collections.Generic;
using System.Linq;

namespace Checkers
{
    internal class CheckersGamePlay
    {
        private List<int> Gameboard = new List<int> { };

        public List<IPlayer> Player1Type = new List<IPlayer> { new RandomPlayer(), new MinMaxPlayer(), new NN_MinMax(), new NN_ReinforcmentLearning() };
        public List<IPlayer> Player2Type = new List<IPlayer> { new RandomPlayer(), new MinMaxPlayer(), new NN_MinMax(), new NN_ReinforcmentLearning() };
        private int MoveTotal = 0;

        public int GetTotalNumberOfMoves
        {
            get { return MoveTotal; }
        }

        private int Player1;

        public int SetPlayer1
        {
            set { Player1 = value; }
        }

        private int Player2;

        public int SetPlayer2
        {
            set { Player2 = value; }
        }

        private int CurrentPlayer = 0;

        public int GetCurrentPlayer
        {
            get { return CurrentPlayer; }
        }

        private void SetupBoard()
        {
            List<int> gameboard = new List<int> {
                -1,0,-1,0,-1,0,-1,0,
                0,-1,0,-1,0,-1,0,-1,
                -1,0,-1,0,-1,0,-1,0,
                0,0,0,0,0,0,0,0,
                0,0,0,0,0,0,0,0,
                0,1,0,1,0,1,0,1,
                1,0,1,0,1,0,1,0,
                0,1,0,1,0,1,0,1
            };

            Gameboard = gameboard;
        }

        private void SwitchPlayer()
        {
            if (CurrentPlayer == 0) { CurrentPlayer = 1; }
            else { CurrentPlayer = 0; }
        }

        private bool GameOver()
        {
            int Player1 = 0, Player2 = 0;
            for (int i = 0; i < Gameboard.Count; i++)
            {
                if (Gameboard[i] > 0) { Player1++; }
                else if (Gameboard[i] < 0) { Player2++; }
            }

            if (Player1 == 0 || Player2 == 0) { return true; }
            else if (NoVaildMoves()) { return true; }
            else { return false; }
        }

        private bool NoVaildMoves()
        {
            PieceMovment Peices = new PieceMovment();
            int LocalPlayer;
            if (CurrentPlayer == 0) { LocalPlayer = 1; }
            else { LocalPlayer = -1; }
            List<List<int>> VaildMoves = Peices.DetermineValidMoves(Gameboard, LocalPlayer);

            if (VaildMoves.Any()) { return false; }
            else { return true; }
        }

        private void MakeMove()
        {
            if (CurrentPlayer == 0) { Gameboard = Player1Type[Player1].MakeMove(Gameboard, 1); }
            else { Gameboard = Player2Type[Player2].MakeMove(Gameboard, -1); }
        }

        private void PrintGameboard()
        {
            Console.WindowHeight = 35;
            //Console.Clear();
            for (int i = 0; i < 8; i++)
            {
                if (i == 0) { Console.Write("---------------------------------\n"); }
                Console.Write("|   |   |   |   |   |   |   |   |\n");
                for (int j = 0; j < 8; j++)
                {
                    if (j == 0) { Console.Write("|"); }
                    Console.Write(" ");
                    if (Gameboard[8 * i + j] == 1) { Console.Write("o"); }
                    else if (Gameboard[8 * i + j] == -1) Console.Write("x");
                    else if (Gameboard[8 * i + j] == 2) { Console.Write("O"); }
                    else if (Gameboard[8 * i + j] == -2) { Console.Write("X"); }
                    else Console.Write(" ");
                    Console.Write(" |");
                }
                Console.Write("\n|   |   |   |   |   |   |   |   |\n");
                Console.Write("---------------------------------\n");
            }

            Console.WriteLine();
        }

        public void PlayGame()
        {
            SetupBoard();
            // PrintGameboard();

            while (!GameOver() && MoveTotal <= 100)
            {
                MakeMove();
                SwitchPlayer();
                //        PrintGameboard();
                MoveTotal++;
            }
        }
    }
}