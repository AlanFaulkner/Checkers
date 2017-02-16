using System;
using System.Collections.Generic;
using System.Linq;

namespace Checkers
{
    public class MinMaxPlayer : IPlayer
    {
        private int MaxDepth = 8;
        private PieceMovment Piece = new PieceMovment();

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
                    LevelScore.Add(ScoreGameState(State, Player));
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

        private int ScoreGameState(List<int> State, int Player)
        {
            int P1_PeiceCount = 0;
            int P2_PeiceCount = 0;

            // calculate number of peices each person has
            for (int i = 0; i < State.Count; i++)
            {
                if (State[i] > 0) { P1_PeiceCount += State[i]; }
                else if (State[i] < 0) { P2_PeiceCount += Math.Abs(State[i]); }
            }

            if (Player == 1)
            {
                return P1_PeiceCount - P2_PeiceCount;
            }
            else
            {
                return P2_PeiceCount - P1_PeiceCount;
            }
        }
    }
}