using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers
{
    class Program
    {

        static void Main(string[] args)
        {

             GamePlay Checkers = new GamePlay();
            // Checkers.SetUpBoard();
             Checkers.EmptyBoard();
            //// Checkers.Gameboard[9] = -1;
            ////Checkers.Gameboard[11] = -1;
            ////Checkers.Gameboard[27] = -1;

            Checkers.Gameboard[9] = -1;
            Checkers.Gameboard[18] = 1;
            ////Checkers.Gameboard[45] = -1;
            //// Checkers.Gameboard[43] = -1;
            ////Checkers.Gameboard[52] = 1;
            //Checkers.Gameboard[10] = -1;
            //Checkers.Gameboard[28] = -1;
            //Checkers.Gameboard[30] = -1;
            //Checkers.Gameboard[44] = -1;
            //Checkers.Gameboard[46] = -1;
            //Checkers.Gameboard[42] = -1;
            //Checkers.Gameboard[37] = 1;
            Checkers.PrintGameBoard2D(Checkers.Gameboard);
            // Checkers.GetPossibleMoves();
            Console.Write("\nOutputs\n\n");
            AI test = new AI();
            List<int> tr = new List<int> { };
            for (int i = 0; i < Checkers.Gameboard.Count; i++) { tr.Add((int)Checkers.Gameboard[i]); }
            List<int> BestMove = test.MinMaxMove(tr, 1);
            test.PrintGameBoard2D(BestMove);
            
           // List<List<int>> Out = test._CanJump(tr, 37 / 8, 37 % 8, 1);
            //List<List<int>> Out = test._CanMove(tr, 22 / 8, 22 % 8, -1);
            ////List<List<int>> Out = test._CanJumpQueen(tr, 37 / 8, 37 % 8, 1);
            //for (int i = 0; i < Out.Count; i++)
            //{
            //    test.PrintGameBoard2D(Out[i]);
            //}
        }
    }
}
