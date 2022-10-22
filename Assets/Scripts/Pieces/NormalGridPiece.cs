using System.Collections.Generic;
using ToonBlast.Model;

namespace ToonBlast
{
    public class NormalGridPiece : IGridPiece
    {
        public NormalGridPiece(int pieceNumber = 0)
        {
            pieceTypeNumber = pieceNumber ;
        }

        public int pieceTypeNumber { get; set; }

        public bool powerPiece { get; set; }

        public List<IGridPiece> GetNeighbors(int x, int y, IGridPiece[,] boardState)
        {
            List<IGridPiece> neighbors = new List<IGridPiece>();
            GetNeighbor(x - 1, y,ref neighbors, boardState); // Left
            GetNeighbor(x, y - 1,ref neighbors, boardState); // Top
            GetNeighbor(x + 1, y,ref neighbors, boardState); // Right
            GetNeighbor(x, y + 1,ref neighbors, boardState); // Bottom

            return neighbors;
        }

        void GetNeighbor(int x, int y, ref List<IGridPiece> neighbors, IGridPiece[,] boardState)
        {
            if (x < boardState.GetLength(0) && y < boardState.GetLength(1) && x >= 0 && y >= 0)
            {
                var piece = boardState[x, y];
                if (!neighbors.Contains(piece)) {
                    neighbors.Add(piece);
                }
            }
        }
    }
    
    
}