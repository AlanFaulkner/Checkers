using System;
using System.Collections.Generic;
using System.Linq;

namespace Checkers
{
    internal class Evolve_NN_Evaluator
    {
        private Random RN = new Random();
        private List<NN_Evaluator> Genepool = new List<NN_Evaluator> { };
        private List<List<int>> CompertionMatchUp = new List<List<int>> { };

        // Evolution variables
        private int GenepoolSize = 2;
        private int NumberOfGenerations = 10;
        private double Mutationchance = 0.001;
        private List<int> GeneDecription = new List<int> { };

        // Not really needed as set by constructor
        public int SizeOfGenepool
        {
            get { return GenepoolSize; }
            set { value = GenepoolSize; }
        }

        public List<int> DescriptionOfGene
        {
            get { return GeneDecription; }
            set { value = GeneDecription; }
        }

        public double ChanceOfMuntation
        {
            get { return Mutationchance; }
            set { value = Mutationchance; }
        }

        public List<NN_Evaluator> GenePool
        {
            get { return Genepool; }
        }

        // Constructor
        public Evolve_NN_Evaluator(int GenePoolSize, List<int> Genedescription, int NumberofGenerations, double MutationChance)
        {
            GenepoolSize = GenePoolSize;
            GeneDecription = Genedescription;
            NumberOfGenerations = NumberofGenerations;
            Mutationchance = MutationChance;
            PopulateGenepool();
            GenerateMatches(GenePoolSize);
        }

        // Initalisation Functions
        private void PopulateGenepool()
        {
            for (int i = 0; i < GenepoolSize; i++)
            {
                NN_Evaluator Evaluator = new NN_Evaluator();
                Evaluator.Create_Network(GeneDecription, RN.Next());
                //Console.Write("Parent " + i + "\n");
                //Evaluator.Print_Network_Info();
                Genepool.Add(Evaluator);
            }
        }

        private void GenerateMatches(int NumberOfPlayers)
        {
            List<int> PlayerList = new List<int> { };
            for (int i = 0; i < NumberOfPlayers; i++) { PlayerList.Add(i); }

            for (int i = 0; i < NumberOfPlayers; i++)
            {
                for (int j = i + 1; j < NumberOfPlayers; j++)
                {
                    List<int> Match = new List<int> { i, j };
                    CompertionMatchUp.Add(Match);
                }
            }
        }

        // KeyFunction
        public NN_Evaluator EvolvePlayer()
        {
            for (int i = 0; i < NumberOfGenerations; i++)
            {
                EvaluateGenePool();
                BreedNextGeneration();
                if (i % 10 == 0) { SaveGenePool(); }
            }
            EvaluateGenePool();
            SaveGenePool();

            return Genepool[0];
        }

        // Determin Genes success rate
        public void EvaluateGenePool()
        {
            for (int i = 0; i < CompertionMatchUp.Count; i++)
            {
                PlayGame(Genepool[CompertionMatchUp[i][0]], Genepool[CompertionMatchUp[i][1]]);
            }

            RankGenepool();
        }

        public void PlayGame(NN_Evaluator Player1, NN_Evaluator Player2)
        {
            CheckersGamePlay Game = new CheckersGamePlay();
            Game.SetPlayer1 = 2;
            var NN1 = Game.Player1Type[2] as NN_MinMax;
            NN1.SetNN = Player1;

            Game.SetPlayer2 = 2;
            var NN2 = Game.Player2Type[2] as NN_MinMax;
            NN2.SetNN = Player2;

            Game.PlayGame();
            if (Game.GetCurrentPlayer == 1) { Player1.Score+=1; }
            else { Player2.Score+=1; }
        }

        private void RankGenepool()
        {
            List<NN_Evaluator> Ranked = Genepool.OrderByDescending(a => a.Score).ToList();
            int Sum = 0;

            // Normalise Score
            for (int i = 0; i < GenepoolSize; i++)
            {
                Sum += Genepool[i].Score;
            }

            for (int i = 0; i < GenepoolSize; i++)
            {
                Genepool[i].Score /= Sum;
            }
        }

        // Generate new generation
        private List<NN_Evaluator> BreedNextGeneration()
        {
            List<NN_Evaluator> NewGeneration = new List<NN_Evaluator> { };

            RankGenepool();
            for (int i = 0; i < GenepoolSize; i++)
            {
                NewGeneration.Add(Crossbreed_Random(SelectParent(), SelectParent()));
            }

            Genepool.Clear();
            Genepool.AddRange(NewGeneration);

            return NewGeneration;
        }

        private NN_Evaluator SelectParent()
        {
            double cumulatedProbability = RN.NextDouble();

            for (int i = 0; i < GenepoolSize; i++)
            {
                if ((cumulatedProbability -= Genepool[i].Score) <= 0)
                    return Genepool[i];
            }

            throw new InvalidOperationException();
        }

        private NN_Evaluator Crossbreed_Random(NN_Evaluator ParentA, NN_Evaluator ParentB)
        {
            NN_Evaluator Child = new NN_Evaluator();
            Child.Create_Network(GeneDecription, 1);

            for (int i = 0; i < ParentA.Network.Count; i++)
            {
                for (int j = 0; j < ParentA.Network[i].Count; j++)
                {
                    for (int k = 0; k < ParentA.Network[i][j].Weights.Count; k++)
                    {
                        // Mutate
                        if (RN.NextDouble() < Mutationchance) { Child.Network[i][j].Weights[k] = RandomWeight(0.9, -0.9); }
                        else
                        {
                            // Else randomly choose weight (gene) from either parent
                            if (RN.NextDouble() < 0.5) { Child.Network[i][j].Weights[k] = ParentA.Network[i][j].Weights[k]; }
                            else { Child.Network[i][j].Weights[k] = ParentB.Network[i][j].Weights[k]; }
                        }
                    }
                }
            }
            Child.Print_Network_Info();
            return Child;
        }

        private NN_Evaluator Crossbreed_Nonrandom(NN_Evaluator ParentA, int GenesOfA, NN_Evaluator ParentB, int GenesOfB)
        {
            int A = 0, B = 0;

            if (GenesOfA <= 0 || GenesOfB <= 0) { throw new Exception("You must selcet a number of genes to incorperate from each parent."); }

            NN_Evaluator Child = new NN_Evaluator();
            Child.Create_Network(GeneDecription, 1);

            for (int i = 0; i < ParentA.Network.Count; i++)
            {
                for (int j = 0; j < ParentA.Network[i].Count; j++)
                {
                    for (int k = 0; k < ParentA.Network[i][j].Weights.Count; k++)
                    {
                        // Mutate
                        if (RN.NextDouble() < Mutationchance) { Child.Network[i][j].Weights[k] = RandomWeight(0.9, -0.9); }
                        else
                        {
                            // Else copy selected number of weights (gene) from respective parent
                            if (A < GenesOfA)
                            {
                                Child.Network[i][j].Weights[k] = ParentA.Network[i][j].Weights[k];
                                A++;
                            }
                            else
                            {
                                Child.Network[i][j].Weights[k] = ParentB.Network[i][j].Weights[k];
                                B++;
                            }

                            if (A == GenesOfA - 1 && B == GenesOfB - 1) { A = 0; B = 0; }
                        }
                    }
                }
            }

            return Child;
        }

        private double RandomWeight(double Min, double Max)
        {
            return RN.NextDouble() * (Max - Min) + Min;
        }

        // Other useful functions
        public void SaveGenePool()
        {
            for (int i = 0; i < GenepoolSize; i++)
            {
                Genepool[i].Save_Network("Gene" + i + ".txt");
            }
            Console.Write("\nGenepool data saved.\n");
        }

        public void LoadGenePool()
        {
            throw new NotImplementedException();
        }
    }
}