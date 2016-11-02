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
            Checkers.SetUpBoard();
           // Checkers.EmptyBoard();
           //// Checkers.Gameboard[9] = -1;
           // //Checkers.Gameboard[11] = -1;
           // Checkers.Gameboard[27] = -1;

           // Checkers.Gameboard[29] = -1;
           // Checkers.Gameboard[25] = -1;
           // Checkers.Gameboard[45] = -1;
           //// Checkers.Gameboard[43] = -1;
           // Checkers.Gameboard[52] = 1;
            Checkers.PrintGameBoard2D(Checkers.Gameboard);
            Checkers.GetPossibleMoves();
        }
    }
}
