using System;
using System.Collections.Generic;

namespace Checkers
{
    internal class Program
    {
        
        private static void Main(string[] args)
        {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new UI());
            
            //CheckersGamePlay game = new CheckersGamePlay();
            //game.SetPlayer1 = 1;
            //var depth = game.Player1Type[1] as MinMaxPlayer;
            //depth.MaxmiumDepth = 4;
            //Console.Write(depth.MaxmiumDepth);
            //game.PlayGame();
            //Console.Write(game.GetCurrentPlayer);
            List<int> Description = new List<int> { 64, 33, 1 };
            Evolve_NN_Evaluator evolve = new Evolve_NN_Evaluator(2, Description, 0.01);
            //evolve.PlayGame(evolve.GenePool[0], evolve.GenePool[1]);
            evolve.EvaluateGenePool();
            Console.Write("\nGene 1 score = " + evolve.GenePool[0].Score);
            Console.Write("\nGene 2 score = " + evolve.GenePool[1].Score);
            evolve.SaveGenePool();
        }
    }
}