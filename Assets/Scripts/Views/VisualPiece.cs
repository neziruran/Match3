using UnityEngine;

namespace ToonBlast.ViewComponents {
    
    [RequireComponent(typeof(SpriteRenderer))]
    public class VisualPiece : MonoBehaviour {

        public void SetSprite(Sprite sprite) {
            var spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = sprite;
        }
        
    }
}