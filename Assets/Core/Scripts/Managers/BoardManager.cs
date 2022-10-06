using System.Collections;
using System.Collections.Generic;
using Core.Scripts.Tiles;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Core.Scripts.Managers
{
    public class BoardManager : MonoBehaviour
    {
        #region Variables

        [Header("Grid Size")] [SerializeField] private int rows = 9;
        [SerializeField] private int columns = 9;

        [Tooltip("The space between each cell that make up the grid." +
                 " Default value is set to 1: This makes the grid look like a solid square with no spaces in between.")]
        [Range(1, 1.1f)]
        [SerializeField]
        private float spacing = 1f;

        [Tooltip("The higher the value, the longer it takes to fill in the grid.")] [SerializeField]
        private float fillGridTime;

        [Header("Grid Object")] [Tooltip("The object used to construct the grid.")]
        public GameObject cellPrefab;

        [Tooltip("Where the cellPrefab will be instantiated")]
        public Transform grid;


        #region Tile Object

        // This can be extended to have different types of tiles
        public enum TileType
        {
            EMPTY, // An empty tile.
            DEFAULT, // Just your basic tile type.
        };

        [System.Serializable]
        public struct TilePrefab
        {
            public TileType type;
            public GameObject prefab;
        }

        #endregion

        [Space(10)] [SerializeField] private TilePrefab[] tilePrefabs;

        private Dictionary<TileType, GameObject> tilePrefabDictionary;

        // 2D array to store the x and y coordinate of each tile.
        private Tile[,] tiles;

        private int gridSize = 0;
        private int readyTilesCount = 0;
        private bool fillingGrid = false;

        #endregion

        #region Getters & Setters

        public static BoardManager Instance { get; set; }

        public Tile[,] GetTiles => tiles;

        public int GetRows => rows;

        public int GetColumns => columns;
        
        public float GetSpacing => spacing;
        
        #endregion

        private void Awake()
        {
            // Singleton Pattern
            // If there is an instance, and it is not me, delete myself
            if (Instance != null && Instance != this)
                Destroy(this);
            else
                Instance = this;

            gridSize = rows * columns;
        }

        // Start is called before the first frame update
        void Start()
        {
            InitTileDictionary();
            CreateGrid();
            CreateEmptyTiles();
            StartCoroutine(FillGrid());
        }

        #region Board Creation

        /// <summary>
        /// Since our tilePrefabDictionary can not be displayed in the Unity inspector,
        /// we copy the values from our tilePrefab array into our dictionary object.
        /// </summary>
        private void InitTileDictionary()
        {
            tilePrefabDictionary = new Dictionary<TileType, GameObject>();

            // Loop through all prefabs in our array.
            for (int i = 0; i < tilePrefabs.Length; i++)
            {
                // Check that the dict does not already contain a key; a tile type.
                if (!tilePrefabDictionary.ContainsKey(tilePrefabs[i].type))
                    // Add new key/value pair to our dict.
                    tilePrefabDictionary.Add(tilePrefabs[i].type, tilePrefabs[i].prefab);
            }
        }

        void CreateGrid()
        {
            for (int x = 0; x < rows; x++)
            {
                for (int y = 0; y < columns; y++)
                {
                    GameObject cell = Instantiate(cellPrefab, GetWorldPosition(x, y) * spacing, Quaternion.identity);
                    cell.transform.SetParent(grid);
                }
            }
        }

        void CreateEmptyTiles()
        {
            tiles = new Tile[rows, columns];

            for (int x = 0; x < rows; x++)
            {
                for (int y = 0; y < columns; y++)
                {
                    SpawnNewTile(x, y, TileType.EMPTY);
                }
            }
        }

        /// <summary>
        /// Spawn a new tile at x and y coordinate.
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <param name="type">The type of tile</param>
        /// <returns>The tile we have just created</returns>
        private Tile SpawnNewTile(int x, int y, TileType type)
        {
            GameObject newTile = Instantiate(tilePrefabDictionary[type], GetWorldPosition(x, y) * spacing,
                Quaternion.identity);
            newTile.transform.SetParent(grid);

            // Give each tile a name so it is easier to tell which tile is which
            newTile.name = "Tile (" + x + "," + y + ")";

            tiles[x, y] = newTile.GetComponent<Tile>();
            tiles[x, y].Init(x, y, this, type);

            return tiles[x, y];
        }

        /// <summary>
        /// Convert a grid coordinate to a world position.
        /// </summary>
        public Vector2 GetWorldPosition(int x, int y)
        {
            // Get the X and Y position of the grid obj, which is the center of the grid, 
            // and subtract half of the width and height, and add our x and y coordinate.
            // Since our world units are the same spacing as our grid units this gives us the world pos for 
            // our tile pieces. The grid will start at the top left corner.
            return new Vector2(grid.position.x - rows / 2.0f + x, grid.position.y + columns / 2.0f - y);
        }

        #endregion

        #region Fill Grid

        // Call FillStep() until the board is filled
        private IEnumerator FillGrid()
        {
            fillingGrid = true;

            yield return new WaitForSeconds(fillGridTime);

            while (FillStep())
            {
                yield return new WaitForSeconds(fillGridTime);
            }

            fillingGrid = false;
            ApplyRules();
        }

        private void ApplyRules()
        {
            foreach (var tile in tiles)
            {
                tile.LoadBoosterTileRule();
            }
        }


        /// <summary>
        /// Move each tile one space
        /// </summary>
        /// <returns>movedTile</returns>
        private bool FillStep()
        {
            bool movedTile = false;

            // Loop through all columns from bottom to top
            // We do not care about the bottom row since it cannot be moved down - hence the -2
            for (int y = columns - 2; y >= 0; y--)
            {
                for (int x = 0; x < rows; x++)
                {
                    Tile tile = tiles[x, y];

                    // Check that the tile is movable
                    // If not movable we can not move it down to fill the empty space and we can ignore it.
                    if (tile.IsMovable())
                    {
                        // Get the tile below the current one - Remember 0 is at the top
                        Tile tileBelow = tiles[x, y + 1];

                        // Check that it is empty
                        if (tileBelow.GetTileType == TileType.EMPTY)
                        {
                            // Clean up the empty tiles before moving the new ones in
                            Destroy(tileBelow.gameObject);

                            // Move the current tile down into the space below it
                            // We are basically swapping a movable tile with an empty one below it
                            tile.GetMovableTileComponent.Move(x, y + 1, fillGridTime);
                            tiles[x, y + 1] = tile;
                            SpawnNewTile(x, y, TileType.EMPTY);

                            // We have moved a tile
                            movedTile = true;
                        }
                    }
                }
            }

            // After checking all the rows we reach the top to see if there is any empty spaces
            // Top Row
            for (int x = 0; x < rows; x++)
            {
                // Our tile will be at the top row
                Tile tileBelow = tiles[x, 0];

                if (tileBelow.GetTileType == TileType.EMPTY)
                {
                    Destroy(tileBelow.gameObject);

                    // Create a new tile with negative Y coordinate. This will be spawn at the top, outside the board.
                    // For this reason, we do not call SpawnNewTile() here.
                    GameObject newTile = Instantiate(tilePrefabDictionary[TileType.DEFAULT],
                        GetWorldPosition(x, -1) * spacing, Quaternion.identity);
                    newTile.transform.SetParent(grid);

                    // Assign newTile coordinate
                    tiles[x, 0] = newTile.GetComponent<Tile>();
                    // Initialize the tile with -1 on the Y coordinate so that we can move it from -1 to 0
                    tiles[x, 0].Init(x, -1, this, TileType.DEFAULT);
                    // Move the tile to the new coordinate
                    tiles[x, 0].GetMovableTileComponent.Move(x, 0, fillGridTime);
                    // Set color to a random color
                    tiles[x, 0].GetTileColorComponent
                        .SetColor((TileColor.ColorType) Random.Range(0, tiles[x, 0].GetTileColorComponent.GetNumColors));

                    readyTilesCount++;

                    movedTile = true;
                }
            }

            return movedTile;
        }

        #endregion

        #region Delete Tiles

        /// <summary>
        /// Delete a single tile.
        /// </summary>
        /// <param name="x">The tile's X coordinate</param>
        /// <param name="y">The tile's Y coordinate</param>
        /// <returns></returns>
        private bool DeleteTile(int x, int y)
        {
            if (tiles[x, y].IsClearable() && !tiles[x, y].GetClearableTileComponent.IsBeingCleared)
            {
                tiles[x, y].GetClearableTileComponent.Clear();

                // Decrease the counter of tiles that are ready
                readyTilesCount--;

                // Spawn new empty tile
                SpawnNewTile(x, y, TileType.EMPTY);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Delete all adjacent tiles
        /// </summary>
        public void DeleteConnectedTiles(int x, int y)
        {
            // Create a list of tiles to be deleted
            List<Tile> tilesToBeDeleted;

            // We check that the tiles that are ready to be popped are equal to the size of the grid given by rows * columns,
            // and that the grid is not currently being refilled with new tiles.
            // This way, popping a tile is only possible when all the tiles are ready to be interacted with.
            if (readyTilesCount == gridSize && !fillingGrid)
            {
                // We check the adjacent tiles to the one we have passed the x and y coordinates; the one we have clicked on.
                tilesToBeDeleted = tiles[x, y].GetConnectedTiles();

                // There is more than one adjacent tile
                // A match is made if there are at least two or more tiles of the same type.
                if (tilesToBeDeleted.Count > 1)
                {
                    foreach (Tile tile in tilesToBeDeleted)
                        DeleteTile(tile.X, tile.Y);

                    StartCoroutine(FillGrid());

                    // Update the moves on the UI
                    if (LevelManager.Instance != null)
                        LevelManager.Instance.OnPlayerMove();
                }
            }
        }

        #endregion
    }
}