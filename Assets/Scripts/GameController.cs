using UnityEngine;

public class GameController : MonoBehaviour
{
    // Input
    private GenericInput genericInput;

    private TileManager tileManager;

    // Use this for initialization
    private void Start()
    {
        // Inputs
        genericInput = GetComponent<GenericInput>();

        // Tile Manager
        tileManager = GameObject.FindObjectOfType<TileManager>();

        // Subscribe to events
        genericInput.OnKeyDown += tileManager.Move;
    }
}
