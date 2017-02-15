using System;
using System.Collections.Generic;
using System.Linq;

namespace Checkers
{
    internal class NN_ReinforcmentLearning : IPlayer
    {
        private List<List<double>> InputMove = new List<List<double>> { };
        private List<double> OutputMove = new List<double> { };

        public List<List<double>> GetTrainingInputs
        {
            get { return InputMove; }
        }

        public List<double> GetTrainingOutputs
        {
            get { return OutputMove; }
        }

        private PieceMovment Piece = new PieceMovment();
        private Random RN = new Random();
        private NN_Evaluator NN;

        public NN_Evaluator SetNeuralNetwork
        {
            set { NN = value; }
        }

        private bool Training = false;

        public bool EnableTraining
        {
            set { Training = value; }
        }

        private double DiscountRate = 0.9;
        private double DecayRate = 0.99;    // Simulates the network forgetting over time.

        public List<int> MakeMove(List<int> Gameboard, int Player)
        {
            if (Training == true) { SaveInitalGamestate(Gameboard); }
            List<List<double>> PossibleMoves = GetPossibleMoves(Gameboard, Player);

            List<double> MoveScore = new List<double> { };
            for (int i = 0; i < PossibleMoves.Count; i++)
            {
                List<double> temp = NN.Get_Network_Output(Remove_InaccessableBoardLocations(PossibleMoves[i]));
                MoveScore.Add(temp[0]);
            }

            if (Training == false)
            {
                double Max = MoveScore[0];
                int LocationOfMax = 0;
                for (int i = 0; i < MoveScore.Count; i++) { if (MoveScore[i] > Max) { Max = MoveScore[i]; LocationOfMax = i; } }

                List<int> SelectedMove = PossibleMoves[LocationOfMax].ConvertAll(Convert.ToInt32);
                return SelectedMove;
            }
            else
            {
                int MoveID = SelectMove_ByDistrubution(MoveScore);
                OutputMove.Add(MoveScore[MoveID]);
                List<int> SelectedMove = PossibleMoves[MoveID].ConvertAll(Convert.ToInt32);
                return SelectedMove;
            }
        }

        public void Train_Neural_Network(string Result,string Filename)
        {            
            if (Result == "Win")
            {
                OutputMove = ScoreTrainingOutputs(OutputMove, 1);
            }

            else 
            {
                OutputMove = ScoreTrainingOutputs(OutputMove, -2);
            }            

            // Train Networks
            NN.Back_prop_Stochastic(InputMove, OutputMove, 0.1, 0.7, 1e-10, 10);
            NN.Save_Network(Filename);
            InputMove.Clear();
            OutputMove.Clear();
        }

        public List<double> Remove_InaccessableBoardLocations(List<double> Data)
        {
            // hacky to convert game board description from 64 total squares to 32 playable squares - may need to completely convert gameboard description to 32 squars throughout.
            List<double> Result = new List<double> { };
            for (int i = 0; i < Data.Count; i++)
            {
                if(((i / 8)%2 ==0 && i%2==0)||((i/8)%2!=0 && i % 2 != 0))
                {
                    Result.Add(Data[i]);
                }
            }
            return Result;
        }

        private List<double> RescaleList(List<double> Data)
        {
            double Max = Data.Max();
            double Min = Data.Min();
            for (int i = 0; i < Data.Count; i++)
            {
                Data[i] = (Data[i] - Min) / (Max - Min);
            }

            return Data;
        }

        private List<double> NormaliseData(List<double> Data)
        {
            double Sum = Data.Sum();
            for (int i = 0; i < Data.Count; i++) { Data[i] /= Sum; }
            return Data;
        }

        private int SelectMove_ByDistrubution(List<double> MoveScore)
        {
            List<double> Score = new List<double> { };
            Score.AddRange(MoveScore);
            Score = NormaliseData(Score);

            double cumulatedProbability = RN.NextDouble();

            for (int i = 0; i < MoveScore.Count; i++)
            {
                if ((cumulatedProbability -=Score[i]) <= 0)
                    return i;
            }

            throw new Exception("Out of range. Is Move Data normalised?");
        }

        private int SelectMove_BestPossible(List<double> Data)
        {
            return Data.IndexOf(Data.Max());
        }

        private void SaveInitalGamestate(List<int> Gameboard)
        {
            List<double> Input = Gameboard.Select<int, double>(i => i).ToList();
            Input = Remove_InaccessableBoardLocations(Input);
            InputMove.Add(Input);
        }

        private List<List<double>> GetPossibleMoves(List<int> Gameboard, int Player)
        {
            List<List<int>> PossibleMoves = Piece.DetermineValidMoves(Gameboard, Player);
            List<List<double>> MoveList_as_double = new List<List<double>> { };

            for (int i = 0; i < PossibleMoves.Count; i++)
            {
                List<double> Move_as_double = PossibleMoves[i].Select<int, double>(a => a).ToList();
                MoveList_as_double.Add(Move_as_double);
            }

            return MoveList_as_double;
        }

        private List<double> ScoreTrainingOutputs(List<double> TrainingOutputs, int Result)
        {
            for (int i = 0; i < TrainingOutputs.Count; i++)
            {
                TrainingOutputs[i] = Activation_Function(TrainingOutputs[i], true);
            }

            TrainingOutputs[TrainingOutputs.Count - 1] += Result;

            for (int i = TrainingOutputs.Count - 2; i >= 0; i--)
            {
                TrainingOutputs[i] = (TrainingOutputs[i] + (DiscountRate * TrainingOutputs[i + 1]));
            }

            for (int i = 0; i < TrainingOutputs.Count; i++)
            {
                TrainingOutputs[i] = DecayRate * Activation_Function(TrainingOutputs[i], false);
            }

            return TrainingOutputs;
        }

        private double Activation_Function(double X, bool DyDx)
        {
            // Only seems to work is a sigmoid function is used tanH and rectulinear functions all seems to result in infite weight values for some reason.
            //if (DyDx == true) { return 1 - (Math.Tan(X) * Math.Tan(X)); }
            //else { return Math.Tanh(X); }

            //if (DyDx==true && X>0) { return 1; }
            //else if (DyDx==true && X<0) { return 0.01; }
            //else if (DyDx == false && X > 0) { return X; }
            //else { return 0.01*X; }

            if (DyDx == true) { return X * (1 - X); }
            else return 1 / (1 + Math.Exp(-X));
        }
    }
}