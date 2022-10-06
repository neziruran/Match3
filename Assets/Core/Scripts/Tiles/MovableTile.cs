using System.Collections;
using Core.Scripts.Managers;
using UnityEngine;

namespace Core.Scripts.Tiles
{
    public class MovableTile : MonoBehaviour
    {
        IEnumerator moveCoroutine;

        private Tile tile;

        private void Awake()
        {
            tile = GetComponent<Tile>();
        }

        public void Move(int newX, int newY, float time)
        {
            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
            }

            moveCoroutine = MoveCoroutine(newX, newY, time);
            StartCoroutine(moveCoroutine);
        }

        /// <summary>
        /// Interpolate between the tile's start and end positions,
        /// moving it a tiny bit each frame.
        /// </summary>
        private IEnumerator MoveCoroutine(int newX, int newY, float time)
        {
            tile.X = newX;
            tile.Y = newY;

            Vector2 startPos = transform.position;
            Vector2 endPos = BoardManager.Instance.GetWorldPosition(newX, newY) * BoardManager.Instance.GetSpacing;

            // Interpolate between startPos and endPos
            // When t = 0; we return startPos | t = 1; we return endPos | t = 0.5; we return a value in between startPos and endPos
            // We are looping t from 0 to time so that our animation will take n time seconds to execute.
            for (float t = 0; t <= 1 * time ; t+= Time.deltaTime)
            {
                transform.position = Vector3.Lerp(startPos, endPos, t / time);
                yield return 0;
            }

            // Set the tile to endPos in case our loop does not get all the way to endPos
            transform.position = endPos;
        }
    }
}
