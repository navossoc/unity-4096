using System.Diagnostics.CodeAnalysis;
using UnityEngine;

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

    public void RestartGame()
    {
        // TODO: confirm this action
        Application.LoadLevel("Game");
    }

    public void ReturnMainMenu()
    {
        Application.LoadLevel("Main");
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
        if (WinScreen.activeInHierarchy)
        {
            return;
        }

        WinScreen.SetActive(true);
    }

    private void LoseAnimation()
    {
        if (LoseScreen.activeInHierarchy)
        {
            return;
        }

        LoseScreen.SetActive(true);
    }

    private void QuitAnimation()
    {
        if (QuitScreen.activeInHierarchy)
        {
            return;
        }

        QuitScreen.SetActive(true);
    }
}
