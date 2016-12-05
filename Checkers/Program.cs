using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Checkers
{
    class Program
    {
        static public List<List<int>> Scores = new List<List<int>> { };
        static public int MaxDepth = 2;
        static void Main(string[] args)
        {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new UI());

            //GamePlay Checkers = new GamePlay();

            //Checkers.PlayGame();

           
            Random random = new Random();

            for (int i = 1; i <= MaxDepth; i++)
            {
                
                List<int> row = new List<int> { };
                for (int j = 0; j< Math.Pow(2,i); j++)
                {
                    row.Add(random.Next(0, 9));
                }
                Scores.Add(row);
            }

            for (int i = 0; i < Scores.Count; i++)
            {
                for (int j = 0; j < Scores[i].Count; j++)
                {
                    Console.Write(Scores[i][j] + " ");
                }
                Console.WriteLine();
            }

           
            List<int> state = new List<int> { 0, 0 };
            for (int i = 1; i <= MaxDepth; i++)
            {
                Console.Write("\ni = " + i + " ");
                List<int> cat = MiniMaxAB(state, i, -1000, 1000, true);
                Console.WriteLine();
                Console.Write(cat[0] + " " + cat[1] + " " + cat[2] + "\n");
            }
        }

        private static int fact(int z)
        {
            int x = 1;
            for (int i = 1; i <= z; i++) { x *= i; }
            return x;
        }

        private static int Score(List<int> State)
        {
            int x = Scores[State[0]][State[1]];
            return x;
        }

       private static List<int> MiniMaxAB(List<int> Gamestate, int Depth, int Alpha, int Beta, bool MaxPlayer, int q=0)
        {
            if (Depth == 0) {
                // if terminal state return score
                List<int> LocalState = new List<int> { };
                LocalState.AddRange(Gamestate);
                int CurrentScore = Score(LocalState);
                LocalState.Add(CurrentScore);
                Console.Write(CurrentScore);
                return LocalState;
            }

            if (MaxPlayer)
            {
                int v = -1000;
                List<int> BestResult = new List<int> { };
                List<int> result = new List<int> { };
                for (int i = 0; i < Gamestate.Count; i++)
                {
                    
                        List<int> local = new List<int> { MaxDepth-Depth, i+q}; // temp game state
                        local.AddRange(Gamestate);
                        
                        result = MiniMaxAB(local, Depth - 1, Alpha, Beta, false,i+q);
                        v = result.Last();
                    if (Alpha < v)
                    {
                        Alpha = v;

                        BestResult.Clear();
                        BestResult.AddRange(local);
                        BestResult.Add(v);
                    }

                        if (Beta <= Alpha) { break; }
                    

                }
                return BestResult;
            }

            else
            {
                int v = 1000;
                List<int> bestresult = new List<int> { };
                List<int> result = new List<int> { };
                for (int i = 0; i < Gamestate.Count; i++)
                {
                    
                        List<int> local = new List<int> {MaxDepth-Depth,i+q }; // temp game state
                        
                        result = MiniMaxAB(local, Depth - 1, Alpha, Beta, true,i+q);
                        v = result.Last();
                    if (Beta > v)
                    {
                        Beta = v;

                        bestresult.Clear();
                        bestresult.AddRange(local);
                        bestresult.Add(v);
                    }

                        if (Beta <= Alpha) { break; }
                    
                }
                return bestresult;
            }
            
            throw new Exception("else not implemented");

        }
    }
}
