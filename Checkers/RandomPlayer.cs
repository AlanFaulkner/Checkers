using System;
using System.Collections.Generic;

namespace Checkers
{
    internal class RandomPlayer : IPlayer
    {
        private static Random RN = new Random();
        private PieceMovment Piece = new PieceMovment { };

        public List<int> MakeMove(List<int> Gameboard, int Player)
        {
            List<List<int>> PossibleMoves = Piece.DetermineValidMoves(Gameboard, Player);
            return PossibleMoves[RN.Next(0, PossibleMoves.Count)];
        }

        // random shuffel - not implimented
        private static List<List<int>> Shuffel(List<List<int>> Data)
        {
            for (int i = 0; i < Data.Count; i++)
            {
                Swap(Data, i, RN.Next(0, Data.Count));
            }

            return Data;
        }

        private static void Swap(List<List<int>> Data, int A, int B)
        {
            List<int> Temp = Data[B];
            Data[B].Clear();
            Data[B].AddRange(Data[A]);
            Data[A].Clear();
            Data[A].AddRange(Temp);
        }
    }
}