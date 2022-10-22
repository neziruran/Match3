using System.Collections.Generic;
using ToonBlast.Model;
using ToonBlast.ViewComponents;
using UnityEngine;

namespace ToonBlast {
	
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
