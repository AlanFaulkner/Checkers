using System.Collections.Generic;
using System.Linq;

namespace Checkers
{
    internal class NN_MinMax : IPlayer
    {
        private int MaxDepth = 4;

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
            return MinMax(Gameboard, Player);
        }

        private List<int> MinMax(List<int> GameBoard, int Player, int Depth = 1)
        {
            List<List<int>> MoveList = Piece.DetermineValidMoves(GameBoard, Player);
            List<double> LevelScore = new List<double> { };

            if (Depth >= MaxDepth || EndGame(GameBoard) || !MoveList.Any()) { return GameBoard; }
            else
            {
                for (int i = 0; i < MoveList.Count; i++)
                {
                    List<int> State = MinMax(MoveList[i], -Player, Depth + 1);
                    LevelScore.Add(NN_ScoreState(State));
                }
            }

            if (Depth % 2 != 0) { return MoveList[LevelScore.IndexOf(LevelScore.Max())]; }
            else { return MoveList[LevelScore.IndexOf(LevelScore.Min())]; }
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
            List<double> Output = NN.Get_Network_Output(Remove_InaccessableBoardLocations(Input));
            return Output[0];
        }

        public List<double> Remove_InaccessableBoardLocations(List<double> Data)
        {
            // hacky to convert game board description from 64 total squares to 32 playable squares - may need to completely convert gameboard description to 32 squars throughout.
            List<double> Result = new List<double> { };
            for (int i = 0; i < Data.Count; i++)
            {
                if (((i / 8) % 2 == 0 && i % 2 == 0) || ((i / 8) % 2 != 0 && i % 2 != 0))
                {
                    Result.Add(Data[i]);
                }
            }
            return Result;
        }
    }
}