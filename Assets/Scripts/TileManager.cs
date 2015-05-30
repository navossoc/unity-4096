using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TileManager : MonoBehaviour
{
    // Tiles panel (parent)
    public Transform TilesParent;
    public GameObject TilePrefab;

    // Configuration
    private RectOffset tilesParentPadding;
    private const int tilesPerRow = 4;
    private const int tileWidth = 96, tileHeight = 96;
    private RectOffset tileMargin;

    // Tile array reference
    private Tile[,] tileObjects;
    // Tile array values
    private int[,] tileValues;

    // Use this for initialization
    void Start()
    {
        // left, right, top, bottom
        tilesParentPadding = new RectOffset(8, 8, 8, 8);
        tileMargin = new RectOffset(4, 4, 4, 4);

        // HACK: Clear all
        foreach (Transform child in transform)
        {
            DestroyObject(child.gameObject);
        }

        // Allocate memory
        tileObjects = new Tile[tilesPerRow, tilesPerRow];
        tileValues = new int[tilesPerRow, tilesPerRow];

        int x = tilesParentPadding.left;
        int y = tilesParentPadding.top;

        // For each row
        for (int i = 0; i < tilesPerRow; i++)
        {
            x = tilesParentPadding.left;
            y += tileMargin.top;

            // For each column
            for (int j = 0; j < tilesPerRow; j++)
            {
                // Create a new tile
                GameObject tile = Instantiate<GameObject>(TilePrefab);
                tile.name = string.Format("Tile ({0}, {1})", i, j);

                RectTransform tileRect = tile.GetComponent<RectTransform>();

                // Resize tile
                tileRect.sizeDelta = new Vector2(tileWidth, tileHeight);

                // Set parent
                tileRect.SetParent(TilesParent, false);

                x += tileMargin.left;

                // Move tile
                tileRect.Translate(x, -y, 0);

                x += tileWidth + tileMargin.right;

                // Assign tile
                tileObjects[i, j] = tile.GetComponent<Tile>();
                tileObjects[i, j].SetValue(1);
                tileValues[i, j] = 0;
            }

            y += tileHeight + tileMargin.bottom;
        }

        // Panel
        x += tilesParentPadding.right;
        y += tilesParentPadding.bottom;

        // Resize
        RectTransform panelRect = GetComponent<RectTransform>();
        panelRect.sizeDelta = new Vector2(x, y);
    }

    private void MoveUp()
    {
        Debug.Log("[Move] Up");
    }

    private void MoveLeft()
    {
        Debug.Log("[Move] Left");
    }

    private void MoveDown()
    {
        Debug.Log("[Move] Down");
    }

    private void MoveRight()
    {
        Debug.Log("[Move] Right");
    }

    // Update is called once per frame
    void Update()
    {
        int horizontal = Mathf.RoundToInt(Input.GetAxis("Horizontal"));
        int vertical = Mathf.RoundToInt(Input.GetAxis("Vertical"));

        if (horizontal != 0 && vertical != 0)
        {
            Debug.LogError("[Input] Can't handle both axis at the same time");
            return;
        }

        if (horizontal < 0)
        {
            MoveLeft();
        }
        else if (horizontal > 0)
        {
            MoveRight();
        }

        if (vertical < 0)
        {
            MoveDown();
        }
        else if (vertical > 0)
        {
            MoveUp();
        }

        if (horizontal != 0 || vertical != 0)
        {
            Move(horizontal, vertical);
        }
    }

    public void Move(int x, int y)
    {
        Debug.LogFormat("[Move] x: {0}, y: {1}", x, y);
    }
}
