namespace Match3.Model {

	public interface IPieceSpawner {
		
		IGridPiece CreateBasicPiece();
		
		IGridPiece CreateBasicPiece(int pieceType);

		IGridPiece CteatSpecialPiece();
	}

}