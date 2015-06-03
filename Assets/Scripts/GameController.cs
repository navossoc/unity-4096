using UnityEngine;

public class GameController : MonoBehaviour
{
    // Input
    private GenericInput genericInput;
    private TouchInput touchInput;

    private ScoreManager scoreManager;
    private TileManager tileManager;

    // Use this for initialization
    private void Start()
    {
        // Inputs
        genericInput = GetComponent<GenericInput>();
        touchInput = GetComponent<TouchInput>();

        // Score Manager
        scoreManager = GameObject.FindObjectOfType<ScoreManager>();

        // Tile Manager
        tileManager = GameObject.FindObjectOfType<TileManager>();

        // Called for every tile merged
        tileManager.OnScore += tileManager_OnScore;

        // Subscribe to input events
        genericInput.OnKeyDown += tileManager.Move;
        touchInput.OnSwipe += tileManager.Move;
    }

    void tileManager_OnScore(Tile tile)
    {
        // Update score and high score
        scoreManager.AddPoints(tile.Score);
    }
}
