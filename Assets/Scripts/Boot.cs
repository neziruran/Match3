using System.Collections.Generic;
using Match3.Model;
using Match3.ViewComponents;
using UnityEngine;

namespace Match3 {
	
	public class Boot : MonoBehaviour {
		
		[SerializeField] private BoardRenderer boardRenderer;
		[SerializeField] private GameManager gameManager;
		
		public void SetUp () {
			gameManager.LoadLevelsFromFile();
			
			int[,] boardDefinition  = gameManager.levels[0];

			
			var pieceSpawner = new PieceSpawner();
			var board = BoardGrid.Create(boardDefinition, pieceSpawner);
			
			
			boardRenderer.Initialize(board);
		}
	}
	
}
