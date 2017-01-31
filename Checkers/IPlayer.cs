using System.Collections.Generic;

namespace Checkers
{
    internal interface IPlayer
    {
        List<int> MakeMove(List<int> Gameboard, int Player);
    }
}