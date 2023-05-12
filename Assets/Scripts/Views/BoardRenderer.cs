using Match3.Model;
using UnityEngine;

namespace Match3.ViewComponents {

	public class BoardRenderer : MonoBehaviour {
		
		[SerializeField] private PieceTypeDatabase pieceTypeDatabase;
		[SerializeField] private VisualPiece visualPiecePrefab;
		[SerializeField] private GameManager gameManager;
		private BoardGrid boardGrid;
		private float lastClick;
		private const float PieceFallSpeed = 0.6f;
		public void Initialize(BoardGrid boardGrid) {
			this.boardGrid = boardGrid;

			CenterCamera();
			CreateVisualPiecesFromBoardState();
		}

		private void CenterCamera() {
			Camera.main.transform.position = new Vector3((boardGrid.Width-1)*0.5f,-(boardGrid.Height-1)*0.5f);
		}

		private void CreateVisualPiecesFromBoardState() {
			DestroyVisualPieces();

			foreach (var pieceInfo in boardGrid.IteratePieces()) {
				
				var visualPiece = CreateVisualPiece(pieceInfo.gridPiece);
				visualPiece.transform.localPosition = LogicPosToVisualPos(pieceInfo.pos.x, pieceInfo.pos.y);

			}
		}
		
		public Vector3 LogicPosToVisualPos(float x,float y) { 
			return new Vector3(x, -y, -y);
		}

		private BoardPos ScreenPosToLogicPos(float x, float y) { 
			
			var worldPos = Camera.main.ScreenToWorldPoint(new Vector3(x,y,-Camera.main.transform.position.z));
			var boardSpace = transform.InverseTransformPoint(worldPos);

			return new BoardPos() {
				x = Mathf.RoundToInt(boardSpace.x),
				y = -Mathf.RoundToInt(boardSpace.y)
			};

		}

		private VisualPiece CreateVisualPiece(IGridPiece gridPiece) {
			
			var pieceObject = Instantiate(visualPiecePrefab, transform, true);
			var sprite = pieceTypeDatabase.GetSpecialSpriteForPieceType(0);
			if (gridPiece.powerPiece) 
			{
				sprite = pieceTypeDatabase.GetSpecialSpriteForPieceType(gridPiece.pieceTypeNumber);
			}
			else
			{
				sprite = pieceTypeDatabase.GetSpriteForPieceType(gridPiece.pieceTypeNumber);
			}

			pieceObject.SetSprite(sprite);
			return pieceObject;
			
		}

		private void DestroyVisualPieces() {
			foreach (var visualPiece in GetComponentsInChildren<VisualPiece>()) {
				Object.Destroy(visualPiece.gameObject);
			}
		}

		private void Update() {
			if (GameManager.gameState != GameState.Started || boardGrid == null) {
				return;
			}

			if (lastClick + PieceFallSpeed > Time.time)
			{
				return;
			}

			if (Input.GetMouseButtonDown(0))
			{
				lastClick = Time.time;
				var pos = ScreenPosToLogicPos(Input.mousePosition.x, Input.mousePosition.y);

				if (!boardGrid.IsWithinBounds(pos.x, pos.y)) return;
				//Adding the connected piece to GameManager to find all the pieces count
				var connections = new ResolveResult();
				boardGrid.FindAndRemoveConnectedAt(pos.x, pos.y, connections);
				gameManager.AddToCollectedPiece(connections, ref pieceTypeDatabase);
				var result = boardGrid.Resolve();
					
				DestroyVisualPieces();
				UpdateBoardVisuals(result);

			}
		}
		/// <summary>
		/// taking list of pieces 
		/// And Applying Tween animation
		/// </summary>
		/// <param name="result"></param>
		private void UpdateBoardVisuals(ResolveResult result)
		{
			foreach (var pieceInfo in boardGrid.IteratePieces()) {
				var visualPiece = CreateVisualPiece(pieceInfo.gridPiece).transform;
				if (result.changes.ContainsKey(pieceInfo.gridPiece)) {
					var selectedPiece = result.changes[pieceInfo.gridPiece];
					var fromVal = LogicPosToVisualPos(selectedPiece.FromPos.x,selectedPiece.FromPos.y);
					fromVal.y += selectedPiece.WasCreated ?  selectedPiece.CreationTime : 0;
					visualPiece.localPosition = fromVal;
					var toVal = LogicPosToVisualPos(selectedPiece.ToPos.x,selectedPiece.ToPos.y);
					Tween.Move(visualPiece, toVal, PieceFallSpeed, 0,  this);
				}else {
					visualPiece.localPosition = LogicPosToVisualPos(pieceInfo.pos.x, pieceInfo.pos.y);
				}
			}
		}
	}

}
