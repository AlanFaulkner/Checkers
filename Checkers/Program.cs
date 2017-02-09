using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Checkers
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new User_Interface());

            //List<int> Description = new List<int> { 64, 33, 1 };

            //Evolve_NN_Evaluator evolve = new Evolve_NN_Evaluator(40, Description, 500, 0.05);
            //var time = System.Diagnostics.Stopwatch.StartNew();
            //evolve.EvolvePlayer();
            //time.Stop();
            //TimeSpan t = TimeSpan.FromMilliseconds(time.ElapsedMilliseconds);
            //string answer = string.Format("{0:D2}h:{1:D2}m:{2:D2}s:{3:D3}ms",
            //                        t.Hours,
            //                        t.Minutes,
            //                        t.Seconds,
            //                        t.Milliseconds);
            //Console.Write(answer);

            // testing
            NN_Evaluator NN = new NN_Evaluator();
            int degreeOfParallelism = Environment.ProcessorCount;

            Parallel.For(0, 10000, i =>
            {
                Thread.Sleep(1);
                lock (NN)
                {
                    NN.Score++;
                }
            });
            Console.Write(NN.Score);
            // validation
            List<int> score = new List<int> { 0, 0, 0 };
            //NN_Evaluator NN = new NN_Evaluator();
            NN.Load_Network("Gene0.txt");
            for (int i = 0; i < 5; i++)
            {
                CheckersGamePlay Game = new CheckersGamePlay();
                if (i % 2 == 0)
                {
                    Game.SetPlayer1 = 1;
                    Game.SetPlayer2 = 2;

                    var NN2 = Game.Player2Type[2] as NN_MinMax;
                    NN2.SetNN = NN;
                }
                else
                {
                    Game.SetPlayer1 = 2;
                    Game.SetPlayer2 = 1;

                    var NN2 = Game.Player1Type[2] as NN_MinMax;
                    NN2.SetNN = NN;
                }

                Game.PlayGame();

                if (i % 2 == 0)
                {
                    if (Game.GetTotalNumberOfMoves >= 100) { score[1]++; }
                    else if (Game.GetCurrentPlayer == 1) { score[0]++; }
                    else { score[2]++; }
                }
                else
                {
                    if (Game.GetTotalNumberOfMoves >= 100) { score[1]++; }
                    else if (Game.GetCurrentPlayer == 1) { score[2]++; }
                    else { score[0]++; }
                }
            }

            Console.Write("\n\nValidation Results\n------------------\n\n" + score[0] + "  " + score[1] + "  " + score[2] + "\n");
        }
    }
}