using UnityEngine;

public class GameController : MonoBehaviour
{
    // Screens
    public GameObject WinScreen;
    public GameObject LoseScreen;

    // Input
    private GenericInput genericInput;
    private TouchInput touchInput;

    private ScoreManager scoreManager;
    private TileManager tileManager;

    public enum GameState
    {
        Playing,
        Winner,
        Loser
    }

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
        tileManager.OnScore += TileManager_OnScore;
        tileManager.OnStateChange += TileManager_OnStateChange;

        // Subscribe to input events
        genericInput.OnKeyDown += tileManager.Move;
        touchInput.OnSwipe += tileManager.Move;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            ReturnMainMenu();
        }
    }

    private void RestartGame()
    {
        // TODO: confirm this action
        Application.LoadLevel("Game");
    }

    private void ReturnMainMenu()
    {
        // TODO: confirm this action
        Application.LoadLevel("Main");
    }

    private void TileManager_OnScore(Tile tile)
    {
        // Update score and high score
        scoreManager.AddPoints(tile.Score);
    }

    private void TileManager_OnStateChange(GameState state)
    {
        if (state != GameState.Playing)
        {
            if (state == GameState.Winner)
            {
                WinScreen.SetActive(true);
            }
            else if (state == GameState.Loser)
            {
                LoseScreen.SetActive(true);
            }

            // Unsubscribe to input events
            genericInput.OnKeyDown -= tileManager.Move;
            touchInput.OnSwipe -= tileManager.Move;
        }
    }
}
