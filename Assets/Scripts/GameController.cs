using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class GameController : MonoBehaviour
{
    /*
     * Fields
     */

    // Screens
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Need to be linked in inspector")]
    public GameObject WinScreen;
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Need to be linked in inspector")]
    public GameObject LoseScreen;

    // Input
    private GenericInput genericInput;
    private TouchInput touchInput;

    private ScoreManager scoreManager;
    private TileManager tileManager;

    /*
     * Enums
     */

    public enum GameState
    {
        Playing,
        Winner,
        Loser
    }

    /*
     * Properties
     */

    public GenericInput GenericInput
    {
        get { return genericInput ?? (genericInput = GetComponent<GenericInput>()); }
    }

    public TouchInput TouchInput
    {
        get { return touchInput ?? (touchInput = GetComponent<TouchInput>()); }
    }

    public ScoreManager ScoreManager
    {
        get { return scoreManager ?? (scoreManager = GetComponent<ScoreManager>()); }
    }

    public TileManager TileManager
    {
        get { return tileManager ?? (tileManager = FindObjectOfType<TileManager>()); }
    }

    /*
     * Methods
     */

    // Use this for initialization
    private void Start()
    {
        // Called for every tile merged
        TileManager.OnScore += TileManager_OnScore;
        TileManager.OnStateChange += TileManager_OnStateChange;

        // Subscribe to input events
        GenericInput.OnKeyDown += TileManager.Move;
        TouchInput.OnSwipe += TileManager.Move;
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
        ScoreManager.AddPoints(tile.Score);
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
            GenericInput.OnKeyDown -= TileManager.Move;
            TouchInput.OnSwipe -= TileManager.Move;
        }
    }
}
