﻿using System.Collections.Generic;

namespace ToonBlast.Model
{
    public interface IGridPiece
    {
        int pieceTypeNumber { get; set; }
        string ToString();
        bool powerPiece { get; set; }
        List<IGridPiece> GetNeighbors(int x, int y, IGridPiece[,] boardState);
    }
    
    
}