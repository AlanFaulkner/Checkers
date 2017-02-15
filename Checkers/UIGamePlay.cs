using System.Collections.Generic;
using System.Linq;

namespace Checkers
{
    internal class UIGamePlay
    {
        private List<int> Gameboard = new List<int> { };
        public List<IPlayer> AI_Players = new List<IPlayer> { };
        private int Currentplayer = 1;

        public List<int> GameBoard
        {
            get { return Gameboard; }
            set { Gameboard = value; }
        }

        public int CurrentPlayer
        {
            get { return Currentplayer; }
        }

        public UIGamePlay()
        {
            Currentplayer = 1;

            MinMaxPlayer AI_MinMax_Easy = new MinMaxPlayer();
            AI_MinMax_Easy.MaxmiumDepth = 4;
            AI_Players.Add(AI_MinMax_Easy);

            MinMaxPlayer AI_MinMax_Medium = new MinMaxPlayer();
            AI_MinMax_Medium.MaxmiumDepth = 6;
            AI_Players.Add(AI_MinMax_Medium);

            MinMaxPlayer AI_MinMax_Hard = new MinMaxPlayer();
            AI_MinMax_Hard.MaxmiumDepth = 8;
            AI_Players.Add(AI_MinMax_Hard);

            NN_ReinforcmentLearning AI_NN = new NN_ReinforcmentLearning();
            AI_Players.Add(AI_NN);
        }

        public void InitaliseGame()
        {
            List<int> gameboard = new List<int> {
                -1,0,-1,0,-1,0,-1,0,
                0,-1,0,-1,0,-1,0,-1,
                -1,0,-1,0,-1,0,-1,0,
                0,0,0,0,0,0,0,0,
                0,0,0,0,0,0,0,0,
                0,1,0,1,0,1,0,1,
                1,0,1,0,1,0,1,0,
                0,1,0,1,0,1,0,1

                //0,0,0,0,0,0,0,0,
                //0,0,0,-1,0,0,0,0,
                //0,0,0,0,2,0,0,0,
                //0,0,0,0,0,0,0,0,
                //0,0,0,0,0,0,0,0,
                //0,0,0,0,0,0,0,0,
                //0,0,0,0,0,0,0,0,
                //0,0,0,0,0,0,0,0
            };

            Gameboard = gameboard;
            Currentplayer = 1;
        }

        public List<List<int>> DetermineMoves()
        {
            PieceMovment Piece = new PieceMovment();
            return Piece.DetermineValidMoves(Gameboard, Currentplayer);
        }

        public void AIMove(int AI_Type)
        {
            Gameboard = AI_Players[AI_Type].MakeMove(Gameboard, Currentplayer);
        }

        public void SwitchPlayers()
        {
            if (Currentplayer == 1) { Currentplayer = -1; }
            else { Currentplayer = 1; }
        }

        public bool GameOver()
        {
            int Player1 = 0, Player2 = 0;
            for (int i = 0; i < Gameboard.Count; i++)
            {
                if (Gameboard[i] > 0) { Player1++; }
                else if (Gameboard[i] < 0) { Player2++; }
            }

            if (Player1 == 0 || Player2 == 0) { return true; }
            else if (NoVaildMoves()) { SwitchPlayers(); return true; }
            else { return false; }
        }

        private bool NoVaildMoves()
        {
            PieceMovment Peices = new PieceMovment();
            int LocalPlayer;
            if (CurrentPlayer == 0) { LocalPlayer = 1; }
            else { LocalPlayer = -1; }
            List<List<int>> VaildMoves = Peices.DetermineValidMoves(Gameboard, LocalPlayer);

            if (VaildMoves.Any()) { return false; }
            else { return true; }
        }
    }
}