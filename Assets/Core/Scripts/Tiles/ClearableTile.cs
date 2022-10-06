using System.Collections;
using Core.Scripts.Managers;
using UnityEngine;

namespace Core.Scripts.Tiles
{
    public class ClearableTile : MonoBehaviour
    {
        // Variables
        public AnimationClip clearAnimation;

        private bool isBeingCleared = false;

        protected Tile tile;

        // Getters & Setters
        public bool IsBeingCleared => isBeingCleared;

        private void Awake()
        {
            tile = GetComponent<Tile>();
        }

        public void Clear()
        {
            if (LevelManager.Instance != null)
                LevelManager.Instance.OnPieceCleared(tile);
        
            isBeingCleared = true;
            StartCoroutine(ClearAnimationCoroutine());
        }

        private IEnumerator ClearAnimationCoroutine()
        {
            Animator animator = GetComponent<Animator>();

            if (animator)
                animator.Play(clearAnimation.name);

            yield return new WaitForSeconds(clearAnimation.length);

            Destroy(gameObject);
        }
    }
}

