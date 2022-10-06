using System.Collections.Generic;
using Core.Scripts.Managers;
using Core.Scripts.UI;
using UnityEngine;

namespace Core.Scripts.Tiles
{
    public class Tile : MonoBehaviour
    {
        #region Variables
    
        public int score = 0;
        private int x;
        private int y;
        private BoardManager.TileType tileType;

        private MovableTile movableTileComponent;
        private TileColor tileColorComponent;
        private ClearableTile clearableTileComponent;
        #endregion
    
        #region Getters & Setters
        public int X { get => x;
            set { if (IsMovable()) x = value; } }
        public int Y { get => y;
            set { if (IsMovable()) y = value; } }

        public BoardManager.TileType GetTileType => tileType;

        public MovableTile GetMovableTileComponent => movableTileComponent;

        public TileColor GetTileColorComponent => tileColorComponent;

        public ClearableTile GetClearableTileComponent => clearableTileComponent;

        #endregion
        
        private void Awake()
        {
            movableTileComponent = GetComponent<MovableTile>();
            tileColorComponent = GetComponent<TileColor>();
            clearableTileComponent = GetComponent<ClearableTile>();
        }
        
        public void LoadBoosterTileRule()
        {
            var connectedTiles = GetConnectedTiles().Count;
        
            switch (connectedTiles)
            {
                
                case < 3: // default sprite
                    tileColorComponent.SetBooster(tileColorComponent.defaultDictionary);
                    break;
                // RULE A
                case > 3 and < 5:
                    tileColorComponent.SetBooster(tileColorComponent.aRule);
                    break;
                // RULE B
                case > 6 and < 8:
                    tileColorComponent.SetBooster(tileColorComponent.bRule);
                    break;
                // RULE C
                case > 8:
                    tileColorComponent.SetBooster(tileColorComponent.cRule);
                    break;
            }
        }
    
        public void Init(int _x, int _y, BoardManager _boardManager, BoardManager.TileType _tileType)
        {
            x = _x;
            y = _y;
            BoardManager.Instance = _boardManager;
            tileType = _tileType;
        }

        #region Adjacent Tiles
        // Expression body definition to implement read-only properties
        // They are similar to lambdas but fundamentally different.
        // Also, unlike lambdas, they are accessible via their name.

        // Return the adjacent tiles to this tile
        private Tile Left => x > 0 ? BoardManager.Instance.GetTiles[x - 1, y] : null;  // If X = 0, we can not return anything to our left because there are no tiles to our left. Therefore we return null.
        private Tile Top => y > 0 ? BoardManager.Instance.GetTiles[x, y - 1] : null;
        private Tile Right => x < BoardManager.Instance.GetRows - 1 ? BoardManager.Instance.GetTiles[x + 1, y] : null;
        private Tile Bottom => y < BoardManager.Instance.GetColumns - 1 ? BoardManager.Instance.GetTiles[x, y + 1] : null;

        // We store the adjcent tiles in an array so that we can return them all at once.
        private Tile[] Neighbours => new[]
        {
            Left,
            Top,
            Right,
            Bottom,
        };

        /// <summary>
        /// A recursive method to get the all the adjacent tiles.
        /// </summary>
        /// <param name="exclude">A list to make sure that we never double check a tile that has already been checked before.</param>
        /// <returns>A list of all the connected tiles to this tile</returns>
        public List<Tile> GetConnectedTiles(List<Tile> exclude = null)
        {
            // Initially return itself
            List<Tile> result = new List<Tile>() { this };

            if (exclude == null)
            {
                // exclude = a new list that contains this tile.
                exclude = new List<Tile> { this };
            }
            else
            {
                exclude.Add(this);
            }

            foreach (Tile neighbour in Neighbours)
            {
                // Skip the neighbour
                // We use the keyword continue to break one iteration, if one of the below conditions occurs, and we continue with the next iteration in the loop.
                if (neighbour == null || exclude.Contains(neighbour) || neighbour.tileColorComponent.TileColors != tileColorComponent.TileColors) continue;

                // Recursive 
                result.AddRange(neighbour.GetConnectedTiles(exclude));
            }

            return result;
        }
    
    
        #endregion

        #region Mouse Event
        private void OnMouseDown()
        {
            // Do nothing if the game is over.
            if (LevelManager.Instance != null && LevelManager.Instance.GetGameOver || PauseMenu.pauseIsClicked)
                return;
            BoardManager.Instance.DeleteConnectedTiles(x, y);
        }
        #endregion

        // Returns true if it exists
        public bool IsMovable()
        {
            return movableTileComponent != null;
        }

        // Returns true if it exists
        public bool IsClearable()
        {
            return clearableTileComponent != null;
        }
    }
}

