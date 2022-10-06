using Core.Scripts.Managers;

namespace Core.Scripts.Levels
{
    public class LevelMoves : LevelManager
    {
        // Variables
        public int numMoves;
        public int targetScore;
        private int _movesUsed = 0;

        // Start is called before the first frame update
        void Start()
        {
            // Set the HUD 
            HUD.SetText();
            HUD.SetScore(CurrentScore);
            HUD.SetTarget(targetScore);
            HUD.SetRemaining(numMoves);

            //Debug.Log("Number of moves: " + numMoves + " TargetScore: " + targetScore); 
        }

        /// <summary>
        /// Called when the player makes a move. In our case, when a tile gets popped.
        /// </summary>
        public override void OnPlayerMove()
        {
            // We increment the number of moves.
            _movesUsed++;

            //Debug.Log("Moves remaining: " + (numMoves - movesUsed));

            HUD.SetRemaining(numMoves - _movesUsed);

            // If the number of moves available is 0
            if (numMoves - _movesUsed == 0 || CurrentScore >= targetScore)
            {
                GameOver();
            }
        }
    }
}