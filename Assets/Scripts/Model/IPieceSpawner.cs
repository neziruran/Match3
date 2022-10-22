namespace ToonBlast.Model {

	public interface IPieceSpawner {
		
		IGridPiece CreateBasicPiece();
		
		IGridPiece CreateBasicPiece(int pieceType);

		IGridPiece CteatSpecialPiece();
	}

}