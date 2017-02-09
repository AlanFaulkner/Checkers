using System;
using System.Collections.Generic;

// current not used anywhere!!!

namespace Checkers
{
    internal class ReinforcementAlgorithum
    {
        private List<NN_Evaluator> AIs = new List<NN_Evaluator> { new NN_Evaluator(), new NN_Evaluator() };
        private CheckersGamePlay Game = new CheckersGamePlay();
        private double DiscountRate = 0.9;
        private double DecayRate = 0.99;    // Simulates the network forgetting over time.
        private int NumberOfEpochs = 100;

        public void TrainPlayer()
        {
            AIs[0].Load_Network("Gene0.txt");
            //AIs[1].Load_Network("Gene1.txt");

            for (int i = 0; i < NumberOfEpochs; i++)
            {
                Game.SetPlayer1 = 3;
                var Player1 = Game.Player1Type[3] as NN_ReinforcmentLearning;
                Player1.SetNeuralNetwork = AIs[0];
                Player1.EnableTraining = true;

                Game.SetPlayer2 = 1; // Train against minmax player
                //var Player2 = Game.Player1Type[3] as NN_ReinforcmentLearning;
                //Player2.SetNeuralNetwork = AIs[1];
                //Player2.EnableTraining = true;

                Game.PlayGame();

                // Get feedback on Neural Net's success
                List<double> Player1TrainingOutputs = Player1.GetTrainingOutputs;
                //List<double> Player2TrainingOutputs = Player2.GetTrainingOutputs;

                if (Game.GetCurrentPlayer == 1)
                {
                    Player1TrainingOutputs = ScoreTrainingOutputs(Player1TrainingOutputs, 1);
                    //    Player2TrainingOutputs = ScoreTrainingOutputs(Player2TrainingOutputs, -2);
                }
                else if (Game.GetCurrentPlayer == 0)
                {
                    Player1TrainingOutputs = ScoreTrainingOutputs(Player1TrainingOutputs, -2);
                    //    Player2TrainingOutputs = ScoreTrainingOutputs(Player2TrainingOutputs, 1);
                }
                else if (Game.GetTotalNumberOfMoves >= 100)
                {
                    Player1TrainingOutputs = ScoreTrainingOutputs(Player1TrainingOutputs, 0);
                    //   Player2TrainingOutputs = ScoreTrainingOutputs(Player2TrainingOutputs, 0);
                }

                // Train Networks
                AIs[0].Back_prop_Stochastic(Player1.GetTrainingInputs, Player1TrainingOutputs, 0.7, 1e-10, 1);
                // AIs[1].Back_prop_Stochastic(Player2.GetTrainingInputs, Player2TrainingOutputs, 0.7, 1e-10, 1);
            }
        }

        private List<double> ScoreTrainingOutputs(List<double> TrainingOutputs, int Result)
        {
            for (int i = 0; i < TrainingOutputs.Count; i++)
            {
                TrainingOutputs[i] = Activation_Function(TrainingOutputs[i], true);
            }

            TrainingOutputs[TrainingOutputs.Count - 1] = TrainingOutputs[TrainingOutputs.Count - 1] + Result;

            for (int i = TrainingOutputs.Count - 2; i >= 0; i--)
            {
                TrainingOutputs[i] = TrainingOutputs[i] + (DiscountRate * TrainingOutputs[i + 1]);
            }

            for (int i = 0; i < TrainingOutputs.Count; i++)
            {
                TrainingOutputs[i] = DecayRate * Activation_Function(TrainingOutputs[i], false);
            }

            return TrainingOutputs;
        }

        private double Activation_Function(double X, bool DyDx)
        {
            if (DyDx == true) { return 1 - (Math.Tan(X) * Math.Tan(X)); }
            else { return Math.Tanh(X); }
        }

    }
}