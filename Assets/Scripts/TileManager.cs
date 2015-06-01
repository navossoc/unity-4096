using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class TileManager : MonoBehaviour
{
    // Tile Prefab
    public GameObject tilePrefab;

    // Configuration
    private RectOffset tilesParentPadding;
    private const int tilesPerRow = 4;
    private const int tileWidth = 96, tileHeight = 96;
    private RectOffset tileMargin;

    private const int startTiles = 2;

    // Tile array reference
    private Tile[,] tileObjects;

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
                GameObject tile = Instantiate<GameObject>(tilePrefab);
                tile.name = string.Format("Tile ({0}, {1})", i, j);

                RectTransform tileRect = tile.GetComponent<RectTransform>();

                // Resize tile
                tileRect.sizeDelta = new Vector2(tileWidth, tileHeight);

                // Set parent
                tileRect.SetParent(gameObject.transform, false);

                x += tileMargin.left;

                // Move tile
                tileRect.Translate(x, -y, 0);

                x += tileWidth + tileMargin.right;

                // Assign tile
                tileObjects[i, j] = tile.GetComponent<Tile>();
                tileObjects[i, j].Value = 0;
            }

            y += tileHeight + tileMargin.bottom;
        }

        // Panel
        x += tilesParentPadding.right;
        y += tilesParentPadding.bottom;

        // Resize
        RectTransform panelRect = GetComponent<RectTransform>();
        panelRect.sizeDelta = new Vector2(x, y);

        // Add random tiles
        for (int i = 0; i < startTiles; i++)
        {
            AddRandomTile();
        }
    }

    private void MoveUp()
    {
        Debug.Log("[Move] Up");
        // Column
        for (int j = 0; j < tilesPerRow; j++)
        {

            // Row (reverse)
            for (int i = 1; i < tilesPerRow; i++)
            {
                Tile tileFrom = tileObjects[i, j];

                // If the tile is empty (skip)
                if (tileFrom == 0)
                {
                    continue;
                }

                Tile tileTo = null;

                // Search for empty cells in the same column
                for (int ii = i - 1; ii >= 0; ii--)
                {
                    Tile tileTemp = tileObjects[ii, j];

                    // If the near tile is empty or is equal
                    if (tileTemp == 0 ||
                        (tileTemp == tileFrom && !tileTemp.Merged))
                    {
                        tileTo = tileTemp;
                        continue;
                    }

                    // No more cells to look
                    break;
                }

                // Move tile
                if (tileTo == 0)
                {
                    tileTo.Value = tileFrom.Value;
                    tileFrom.Value = 0;
                }
                // Merge tile
                else if (tileFrom == tileTo)
                {
                    tileTo.Value = tileFrom.Value + 1;
                    tileTo.Merged = true;
                    tileFrom.Value = 0;
                }
            }

            // Reset all merges in this column
            for (int i = 0; i < tilesPerRow; i++)
            {
                tileObjects[i, j].Merged = false;
            }

        }
    }

    private void MoveLeft()
    {
        Debug.Log("[Move] Left");
        // Row
        for (int i = 0; i < tilesPerRow; i++)
        {

            // Column (reverse)
            for (int j = 1; j < tilesPerRow; j++)
            {
                Tile tileFrom = tileObjects[i, j];

                // If the tile is empty (skip)
                if (tileFrom == 0)
                {
                    continue;
                }

                Tile tileTo = null;

                // Search for empty cells in the same row
                for (int jj = j - 1; jj >= 0; jj--)
                {
                    Tile tileTemp = tileObjects[i, jj];

                    // If the near tile is empty or is equal
                    if (tileTemp == 0 ||
                        (tileTemp == tileFrom && !tileTemp.Merged))
                    {
                        tileTo = tileTemp;
                        continue;
                    }

                    // No more cells to look
                    break;
                }

                // Move tile
                if (tileTo == 0)
                {
                    tileTo.Value = tileFrom.Value;
                    tileFrom.Value = 0;
                }
                // Merge tile
                else if (tileFrom == tileTo)
                {
                    tileTo.Value = tileFrom.Value + 1;
                    tileTo.Merged = true;
                    tileFrom.Value = 0;
                }
            }

            // Reset all merges in this row
            for (int j = 0; j < tilesPerRow; j++)
            {
                tileObjects[i, j].Merged = false;
            }

        }
    }

    private void MoveDown()
    {
        Debug.Log("[Move] Down");
        // Column
        for (int j = 0; j < tilesPerRow; j++)
        {

            // Row (reverse)
            for (int i = tilesPerRow - 2; i >= 0; i--)
            {
                Tile tileFrom = tileObjects[i, j];

                // If the tile is empty (skip)
                if (tileFrom == 0)
                {
                    continue;
                }

                Tile tileTo = null;

                // Search for empty cells in the same column
                for (int ii = i + 1; ii < tilesPerRow; ii++)
                {
                    Tile tileTemp = tileObjects[ii, j];

                    // If the near tile is empty or is equal
                    if (tileTemp == 0 ||
                        (tileTemp == tileFrom && !tileTemp.Merged))
                    {
                        tileTo = tileTemp;
                        continue;
                    }

                    // No more cells to look
                    break;
                }

                // Move tile
                if (tileTo == 0)
                {
                    tileTo.Value = tileFrom.Value;
                    tileFrom.Value = 0;
                }
                // Merge tile
                else if (tileFrom == tileTo)
                {
                    tileTo.Value = tileFrom.Value + 1;
                    tileTo.Merged = true;
                    tileFrom.Value = 0;
                }
            }

            // Reset all merges in this column
            for (int i = 0; i < tilesPerRow; i++)
            {
                tileObjects[i, j].Merged = false;
            }

        }
    }

    private void MoveRight()
    {
        Debug.Log("[Move] Right");
        // Row
        for (int i = 0; i < tilesPerRow; i++)
        {

            // Column (reverse)
            for (int j = tilesPerRow - 2; j >= 0; j--)
            {
                Tile tileFrom = tileObjects[i, j];

                // If the tile is empty (skip)
                if (tileFrom == 0)
                {
                    continue;
                }

                Tile tileTo = null;

                // Search for empty cells in the same row
                for (int jj = j + 1; jj < tilesPerRow; jj++)
                {
                    Tile tileTemp = tileObjects[i, jj];

                    // If the near tile is empty or is equal
                    if (tileTemp == 0 ||
                        (tileTemp == tileFrom && !tileTemp.Merged))
                    {
                        tileTo = tileTemp;
                        continue;
                    }

                    // No more cells to look
                    break;
                }

                // Move tile
                if (tileTo == 0)
                {
                    tileTo.Value = tileFrom.Value;
                    tileFrom.Value = 0;
                }
                // Merge tile
                else if (tileFrom == tileTo)
                {
                    tileTo.Value = tileFrom.Value + 1;
                    tileTo.Merged = true;
                    tileFrom.Value = 0;
                }
            }

            // Reset all merges in this row
            for (int j = 0; j < tilesPerRow; j++)
            {
                tileObjects[i, j].Merged = false;
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        int horizontal = 0, vertical = 0;

        // Mobile
#if UNITY_ANDROID
        horizontal = (int)Input.GetAxisRaw("Horizontal");
        vertical = (int)Input.GetAxisRaw("Vertical");
#else
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            horizontal = -1;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            horizontal = 1;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            vertical = -1;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            vertical = 1;
        }
#endif

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

        #region DEBUG
        // DEBUG: spawn blocks
        if (Input.GetKeyDown(KeyCode.Mouse2))
        {
            Debug.Log("Add random block");
            AddRandomTile();
        }

        // HACK: tests
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            tileObjects[0, 0].Value = 1;
            tileObjects[0, 1].Value = 1;
            tileObjects[0, 2].Value = 1;
            tileObjects[0, 3].Value = 1;
            //
            tileObjects[1, 0].Value = 1;
            tileObjects[1, 1].Value = 1;
            tileObjects[1, 2].Value = 2;
            tileObjects[1, 3].Value = 3;
            //
            tileObjects[2, 0].Value = 3;
            tileObjects[2, 1].Value = 2;
            tileObjects[2, 2].Value = 1;
            tileObjects[2, 3].Value = 1;
            //
            tileObjects[3, 0].Value = 1;
            tileObjects[3, 1].Value = 0;
            tileObjects[3, 2].Value = 1;
            tileObjects[3, 3].Value = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            tileObjects[3, 0].Value = 0;
            tileObjects[3, 1].Value = 0;
            tileObjects[3, 2].Value = 0;
            tileObjects[3, 3].Value = 1;
            //
            tileObjects[2, 0].Value = 0;
            tileObjects[2, 1].Value = 0;
            tileObjects[2, 2].Value = 1;
            tileObjects[2, 3].Value = 0;
            //
            tileObjects[1, 0].Value = 0;
            tileObjects[1, 1].Value = 1;
            tileObjects[1, 2].Value = 0;
            tileObjects[1, 3].Value = 0;
            //
            tileObjects[0, 0].Value = 1;
            tileObjects[0, 1].Value = 0;
            tileObjects[0, 2].Value = 0;
            tileObjects[0, 3].Value = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            tileObjects[0, 0].Value = 1;
            tileObjects[0, 1].Value = 1;
            tileObjects[0, 2].Value = 0;
            tileObjects[0, 3].Value = 0;
            //
            tileObjects[1, 0].Value = 0;
            tileObjects[1, 1].Value = 0;
            tileObjects[1, 2].Value = 1;
            tileObjects[1, 3].Value = 1;
            //
            tileObjects[2, 0].Value = 0;
            tileObjects[2, 1].Value = 1;
            tileObjects[2, 2].Value = 0;
            tileObjects[2, 3].Value = 1;
            //
            tileObjects[3, 0].Value = 1;
            tileObjects[3, 1].Value = 0;
            tileObjects[3, 2].Value = 0;
            tileObjects[3, 3].Value = 1;
        }
        #endregion
    }

    public void Move(int x, int y, bool relativeToSelf = true)
    {
        //Debug.LogFormat("[Move] x: {0}, y: {1}", x, y);
    }

    // Tiles methods
    private void AddRandomTile()
    {
        Vector2 cell;
        if (RandomAvailableCell(out cell))
        {
            int value = Random.value < 0.9f ? 1 : 2;
            int x = (int)cell.x;
            int y = (int)cell.y;
            tileObjects[x, y].Value = value;
        }
        else
        {
            Debug.LogWarning("[Tiles] No space available!");
        }
    }

    private bool RandomAvailableCell(out Vector2 cell)
    {
        List<Vector2> cells = AvailableCells();

        if (cells.Count > 0)
        {
            cell = cells[Random.Range(0, cells.Count - 1)];
            return true;
        }

        cell = Vector2.zero;
        return false;
    }

    private List<Vector2> AvailableCells()
    {
        List<Vector2> cells = new List<Vector2>();

        for (int i = 0; i < tilesPerRow; i++)
        {
            for (int j = 0; j < tilesPerRow; j++)
            {
                if (tileObjects[i, j].Value == 0)
                {
                    cells.Add(new Vector2(i, j));
                }
            }
        }

        return cells;
    }
}
