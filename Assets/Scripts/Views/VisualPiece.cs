using UnityEngine;

namespace Match3.ViewComponents {
    
    [RequireComponent(typeof(SpriteRenderer))]
    public class VisualPiece : MonoBehaviour {

        public void SetSprite(Sprite sprite) {
            var spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = sprite;
        }
        
    }
}