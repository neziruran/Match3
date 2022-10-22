namespace ToonBlast.Model {

	public class PieceSpawner : IPieceSpawner
	{
		private const int MinPieceType = (int) PieceTypes.Blue;
		private const int MaxPieceType = (int) PieceTypes.Yellow + 1;
		private const int MinSpecialPieceType = (int) SpecialPieceTypes.HorizontalPower;
		private const int MaxSpecialPieceType = (int) SpecialPieceTypes.BombPower + 1;

		public IGridPiece CreateBasicPiece() {
			int pieceType = UnityEngine.Random.Range(MinPieceType,MaxPieceType);
			return CreateBasicPiece(pieceType);
		}

		public IGridPiece CreateBasicPiece(int pieceType)
		{
			IGridPiece gridPiece = new NormalGridPiece();
			gridPiece = new NormalGridPiece(pieceType);
	      
			return gridPiece;
		}

		private int RandomSpecialPiece() {
			return UnityEngine.Random.Range(MinSpecialPieceType, MaxSpecialPieceType);
		}

		public IGridPiece CteatSpecialPiece( )
		{
			int pieceType = RandomSpecialPiece();
			IGridPiece gridPiece = new NormalGridPiece();
	        
			switch (pieceType)
			{
				case 0:
					gridPiece = new HorizontalPowerGridPiece(pieceType);
					break;
				case 1:
					gridPiece = new VerticalPowerGridPiece(pieceType);
					break;
				case 2:
					gridPiece = new BombPowerGridPiece(pieceType);
					break;
		        
			}
			
			return gridPiece;
		}
	}

}