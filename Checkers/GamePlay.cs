using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers
{
    class GamePlay
    {
        List<int> GameBoard = new List<int> { };
        int player = 1;
        string Player1 = "AI";
        string Player2 = "AI";

        // Public access to game board list.
        public List<int> Gameboard
        {
            get { return GameBoard; }
            set { GameBoard = value; }
        }

        public int CurrentPlayer // sets player turn to either 1 or -1
        {
            get { return player; }
            set { player = value; }
        }

        public string PlayerOne
        {
            get { return Player1; }
            set { Player1 = value; }
        }

        public string PlayerTwo
        {
            get { return Player2; }
            set { Player2 = value; }
        }


        // ##########################################
        // ## Functions relating to the game board ##
        // ##########################################

        public void SetUpBoard() // initializes game board
        {
            List<int> gameboard = new List<int> { };

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
                    //gameboard.Add(0);
                }
            }

            // testing
            //gameboard[34] = 2;
            //gameboard[32] = 2;
            //gameboard[36] = -2;

            Gameboard = gameboard;
        }

        public void EmptyBoard()
        {
            Gameboard.Clear();
            for (int i = 0; i < 64; i++) {
                Gameboard.Add(0);
            }
        }

        // ###################################
        // ## Functions related to movement ##
        // ###################################
        
        public void PlayGame()
        {
            AI GameAI = new AI();
            bool End = false;

            SetUpBoard();
            PrintGameBoard2D(Gameboard);
            
            while (!End)
            {
                
                if (CurrentPlayer == 1 && Player1 == "Human")
                {
                    Console.Write("Enter Move: ");
                    string Line = Console.ReadLine();
                    List<int> Input = Line.Split(',').Select(Int32.Parse).ToList();
                    for (int i = 0; i < Input.Count; i = i + 2)
                    {
                        if (i < Input.Count - 2) { Gameboard[8 * Input[i] + Input[i + 1]] = 0; }
                        Gameboard[8 * Input[i] + Input[i + 1]] = CurrentPlayer;
                        PrintGameBoard2D(Gameboard);
                        CurrentPlayer = -1;
                    }
                }

                else if (CurrentPlayer == 1 && Player1 == "AI")
                {
                    // Make move using MinMax
                    List<int> NewGameBoard = GameAI.MinMaxMove(Gameboard, 1);
                    if (Gameboard.SequenceEqual(NewGameBoard)) { break; }
                    else
                    {
                        Gameboard = NewGameBoard;
                        PrintGameBoard2D(Gameboard);
                        CurrentPlayer = -1;
                    }
                }


                if (CurrentPlayer == -1 && Player2 == "Human")
                {
                    Console.Write("Enter Move: ");
                    string Line = Console.ReadLine();
                    List<int> Input = Line.Split(',').Select(Int32.Parse).ToList();
                    for (int i = 0; i < Input.Count; i = i + 2)
                    {
                        if (i < Input.Count - 2) { Gameboard[8 * Input[i] + Input[i + 1]] = 0; }
                        Gameboard[8 * Input[i] + Input[i + 1]] = CurrentPlayer;
                        PrintGameBoard2D(Gameboard);
                        CurrentPlayer = 1;
                    }
                }

                else if (CurrentPlayer == -1 && Player2 == "AI")
                {
                    // Make move using MinMax
                    List<int> NewGameBoard = GameAI.MinMaxMove(Gameboard, -1);
                    if (Gameboard.SequenceEqual(NewGameBoard)) { break; }
                    else
                    {
                        Gameboard = NewGameBoard;
                        PrintGameBoard2D(Gameboard);
                        CurrentPlayer = 1;
                    }
                }
                }
        }

        // ########################
        // ## End Game Functions ##
        // ########################

        // #########################################
        // ## Useful functions used for debugging ##
        // #########################################

        
        public void PrintGameBoard2D(List<int> Board) // prints a pretty typed game board to console window.
        {
            
            Console.WindowHeight = 35; // set number of rows for console window height
           // Console.Clear();

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
