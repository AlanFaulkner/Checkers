using System.Collections.Generic;
using System.Linq;

namespace Checkers
{
    internal class NN_MinMax : IPlayer
    {
        private int MaxDepth = 1;

        private PieceMovment Piece = new PieceMovment();
        private NN_Evaluator NN = new NN_Evaluator();

        public NN_Evaluator SetNN
        {
            set { NN = value; }
        }

        public int MaxmiumDepth
        {
            get { return MaxDepth; }
            set { MaxDepth = value; }
        }

        public List<int> MakeMove(List<int> Gameboard, int Player)
        {
            List<List<int>> MoveList = Piece.DetermineValidMoves(Gameboard, Player);
            List<List<double>> Scores = new List<List<double>> { };
            for (int i = 0; i < MoveList.Count; i++)
            {
                List<double> StateScore = new List<double> { };
                StateScore.Add(i);
                StateScore.Add(MinMaxScore(MoveList[i], Player));
                Scores.Add(StateScore);
            }
            Scores = Scores.OrderByDescending(a => a[1]).ToList();
            return MoveList[(int)Scores[0][0]];
        }

        private double MinMaxScore(List<int> GameBoard, int Player, int Depth = 1)
        {
            List<List<int>> MoveList = Piece.DetermineValidMoves(GameBoard, -Player);
            if (Depth >= MaxDepth || EndGame(GameBoard) || !MoveList.Any()) { return NN_ScoreState(GameBoard); }
            else
            {
                List<double> LevelScore = new List<double> { };
                for (int i = 0; i < MoveList.Count; i++)
                {
                    LevelScore.Add(MinMaxScore(MoveList[i], -Player, Depth + 1));
                }

                if (Depth % 2 != 0) { return LevelScore.Max(); }
                else { return LevelScore.Min(); }
            }
        }

        private bool EndGame(List<int> GameState)
        {
            int P1 = 0;
            int P2 = 0;

            for (int i = 0; i < GameState.Count; i++)
            {
                if (GameState[i] > 0) { P1++; }
                else if (GameState[i] < 0) { P2++; }
            }

            if (P1 == 0 || P2 == 0) { return true; }
            else { return false; }
        }

        private double NN_ScoreState(List<int> Gameboard)
        {
            List<double> Input = Gameboard.Select<int, double>(i => i).ToList();
            List<double> Output = NN.Get_Network_Output(Input);
            return Output[0];
        }
    }
}