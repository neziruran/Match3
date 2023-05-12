using System.Collections.Generic;
using Match3.Model;

namespace Match3
{
    public class HorizontalPowerGridPiece : IGridPiece
    {
        public HorizontalPowerGridPiece (int pieceNumber)
        {
            pieceTypeNumber = pieceNumber;
            this.powerPiece = true;
        }

        public int pieceTypeNumber { get; set; }

        public bool powerPiece { get; set; }

        public List<IGridPiece> GetNeighbors(int x, int y, IGridPiece[,] boardState) {
            List<IGridPiece> neighbors = new List<IGridPiece>();
            for (int i = 0; i < boardState.GetLength(0); i++) {
                var piece = boardState[i, y];
                if (!neighbors.Contains(piece)) {
                    neighbors.Add(piece);
                }
            }
            return neighbors;
           
        }
    }
    
    
}