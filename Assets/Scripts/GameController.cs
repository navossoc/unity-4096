using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    /*
     * Fields
     */

    // Screens
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Need to be linked in inspector")]
    public GameObject WinScreen, LoseScreen, QuitScreen;

    private const float OverlayTime = 0.5f;

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
        Undefined,
        Playing,
        Winner,
        Loser
    }

    /*
     * Properties
     */

    public GameState CurrentState { get; private set; }

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

    public void RestartGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void ReturnMainMenu()
    {
        SceneManager.LoadScene("Main");
    }

    public void CancelQuitScreen()
    {
        QuitScreen.SetActive(false);
    }

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
            QuitAnimation();
        }
    }

    private void TileManager_OnScore(Tile tile)
    {
        // Update score and high score
        ScoreManager.AddPoints(tile.Score);
    }

    private void TileManager_OnStateChange(GameState state)
    {
        // Prevent multiple calls for the same state
        if (CurrentState == state)
        {
            return;
        }

        // Update game state
        CurrentState = state;

        if (state != GameState.Playing)
        {
            if (state == GameState.Winner)
            {
                Invoke("WinAnimation", OverlayTime);
            }
            else if (state == GameState.Loser)
            {
                Invoke("LoseAnimation", OverlayTime);
            }

            // Unsubscribe to input events
            GenericInput.OnKeyDown -= TileManager.Move;
            TouchInput.OnSwipe -= TileManager.Move;
        }
    }

    private void WinAnimation()
    {
        WinScreen.SetActive(true);
    }

    private void LoseAnimation()
    {
        LoseScreen.SetActive(true);
    }

    private void QuitAnimation()
    {
        // Checks if there is no other screen active
        if (!WinScreen.activeInHierarchy && !LoseScreen.activeInHierarchy)
        {
            QuitScreen.SetActive(true);
        }
        else
        {
            ReturnMainMenu();
        }
    }
}
