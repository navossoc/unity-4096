using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TileManager : MonoBehaviour
{
    // Tiles panel (parent)
    public Transform TilesParent;
    public GameObject TilePrefab;

    // Configuration
    private const int tilesParentPadding = 16;
    private const int tilesPerRow = 4;
    private const int tileWidth = 96, tileHeight = 96;
    private const int tileMargin = 8;

    // Number of tiles
    private const int totalTiles = tilesPerRow * tilesPerRow;

    // Tile array reference
    private Tile[] tileObjects;
    // Tile array values
    private int[] tileValues;

    // Use this for initialization
    void Start()
    {
        // HACK: Clear all
        foreach (Transform child in transform)
        {
            DestroyObject(child.gameObject);
        }

        // Allocate memory
        tileObjects = new Tile[totalTiles];
        tileValues = new int[totalTiles];

        int x = tilesParentPadding, y = -tilesParentPadding;

        // For each cell
        for (int i = 0; i < totalTiles; i++)
        {
            // Create a new tile
            GameObject tile = Instantiate<GameObject>(TilePrefab);
            tile.name = "Tile " + i;

            RectTransform tileRect = tile.GetComponent<RectTransform>();

            // Tile in a row
            if (i % tilesPerRow == 0)
            {
                x = tilesParentPadding;
            }
            else
            {
                x += tileWidth + tileMargin;
            }

            // Tile in a column
            if (i % tilesPerRow == 0 && i > 0)
            {
                y -= tileHeight + tileMargin;
            }

            // Set parent
            tileRect.SetParent(TilesParent, false);

            // Move tile
            tileRect.Translate(x, y, 0);

            // Assign tile
            tileObjects[i] = tile.GetComponent<Tile>();
            tileObjects[i].SetValue(0);
            tileValues[i] = 0;
        }

        // Panel
        x += tileWidth + tilesParentPadding;
        y -= tileHeight + tilesParentPadding;
        y = Mathf.Abs(y);

        // Resize
        RectTransform panelRect = GetComponent<RectTransform>();
        panelRect.sizeDelta = new Vector2(x, y);
    }

    // Update is called once per frame
    void Update()
    {
        int horizontal = Mathf.RoundToInt(Input.GetAxis("Horizontal"));
        int vertical = Mathf.RoundToInt(Input.GetAxis("Vertical"));
    }
}
