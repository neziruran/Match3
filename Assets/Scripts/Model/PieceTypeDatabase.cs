using System.Collections.Generic;
using UnityEngine;

namespace ToonBlast.Model {

	public class PieceTypeDatabase : MonoBehaviour {
		[SerializeField] private List<Sprite> spritesPerPieceTypeId;
		[SerializeField] private List<Sprite> specialSpritesPerPieceTypeId;
		public Sprite GetSpriteForPieceType(int pieceType) {
			if (pieceType >= 0 && pieceType < spritesPerPieceTypeId.Count) { 
				return spritesPerPieceTypeId[pieceType];
			}
			return null;
		}

		public Sprite GetSpecialSpriteForPieceType(int pieceType) {
			if (pieceType >= 0 && pieceType < specialSpritesPerPieceTypeId.Count) { 
				return specialSpritesPerPieceTypeId[pieceType];
			}
			return null;
		}
	}

}