using UnityEngine;

public class GameController : MonoBehaviour
{
    // Input
    private GenericInput genericInput;
    private TouchInput touchInput;

    private TileManager tileManager;

    // Use this for initialization
    private void Start()
    {
        // Inputs
        genericInput = GetComponent<GenericInput>();
        touchInput = GetComponent<TouchInput>();

        // Tile Manager
        tileManager = GameObject.FindObjectOfType<TileManager>();

        // Subscribe to events
        genericInput.OnKeyDown += tileManager.Move;
        touchInput.OnSwipe += tileManager.Move;
    }
}
