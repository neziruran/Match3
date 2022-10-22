using System.Collections.Generic;

namespace ToonBlast.Model {
    
    public interface IGrid {
        
        int Width { get; }
        int Height { get; }
        
        IGridPiece CreatePiece(int pieceType, int x, int y);
        
        IGridPiece GetAt(int x, int y);
        
        void MovePiece(int fromX, int fromY, int toX, int toY);
        void RemovePieceAt(int x, int y);
        
        List<IGridPiece> GetConnected(int x, int y);
        bool TryGetPiecePos(IGridPiece gridPiece, out int px, out int py);

    }
    
}