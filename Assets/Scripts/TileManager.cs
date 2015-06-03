using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    // Tile Prefab
    public GameObject TilePrefab;

    // Configuration
    private const int TilesPerRow = 4;
    private const int StartTiles = 2;

    // Tile array reference
    private Tile[,] tileObjects;

    public void Move(int x, int y)
    {
        Debug.LogFormat("[Move] x: {0}, y: {1}", x, y);

        int uX = Mathf.Abs(x);
        int uY = Mathf.Abs(y);

        bool spawn = false;

        // Handle axes behavior
        if (uX > uY)
        {
            // Axis X
            if (x < 0)
            {
                spawn = MoveLeft();
            }
            else if (x > 0)
            {
                spawn = MoveRight();
            }
        }
        else if (uX < uY)
        {
            // Axis Y
            if (y < 0)
            {
                spawn = MoveDown();
            }
            else if (y > 0)
            {
                spawn = MoveUp();
            }
        }
        else if (uX != 0 && uY != 0)
        {
            Debug.LogWarning("Can't handle both axes at the same time");
        }

        // If an action was made (merge/move)
        if (spawn)
        {
            AddRandomTile();
        }
    }

    // Use this for initialization
    private void Start()
    {
        // Allocate memory
        tileObjects = new Tile[TilesPerRow, TilesPerRow];

        // For each row        
        for (int i = 0; i < TilesPerRow; i++)
        {
            // For each column            
            for (int j = 0; j < TilesPerRow; j++)
            {
                // Find tile
                string name = string.Format("Tile ({0}, {1})", i, j);
                GameObject tile = GameObject.Find(name);

                if (tile)
                {
                    // Assign tile
                    tileObjects[i, j] = tile.GetComponent<Tile>();
                }
                else
                {
                    Debug.LogError("Failed to find " + name);
                }
            }
        }

        // Add random tiles
        for (int i = 0; i < StartTiles; i++)
        {
            AddRandomTile();
        }
    }

    private bool MoveUp()
    {
        bool spawn = false;

        // Column
        for (int j = 0; j < TilesPerRow; j++)
        {
            // Row (reverse)
            for (int i = 1; i < TilesPerRow; i++)
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
                    spawn = true;
                }
                else if (tileFrom == tileTo)
                {
                    // Merge tile
                    tileTo.Value = tileFrom.Value + 1;
                    tileTo.Merged = true;
                    tileFrom.Value = 0;
                    spawn = true;
                }
            }

            // Reset all merges in this column
            for (int i = 0; i < TilesPerRow; i++)
            {
                tileObjects[i, j].Merged = false;
            }
        }

        return spawn;
    }

    private bool MoveLeft()
    {
        bool spawn = false;

        // Row
        for (int i = 0; i < TilesPerRow; i++)
        {
            // Column (reverse)
            for (int j = 1; j < TilesPerRow; j++)
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
                    spawn = true;
                }
                else if (tileFrom == tileTo)
                {
                    // Merge tile
                    tileTo.Value = tileFrom.Value + 1;
                    tileTo.Merged = true;
                    tileFrom.Value = 0;
                    spawn = true;
                }
            }

            // Reset all merges in this row
            for (int j = 0; j < TilesPerRow; j++)
            {
                tileObjects[i, j].Merged = false;
            }
        }

        return spawn;
    }

    private bool MoveDown()
    {
        bool spawn = false;

        // Column
        for (int j = 0; j < TilesPerRow; j++)
        {
            // Row (reverse)
            for (int i = TilesPerRow - 2; i >= 0; i--)
            {
                Tile tileFrom = tileObjects[i, j];

                // If the tile is empty (skip)
                if (tileFrom == 0)
                {
                    continue;
                }

                Tile tileTo = null;

                // Search for empty cells in the same column
                for (int ii = i + 1; ii < TilesPerRow; ii++)
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
                    spawn = true;
                }
                else if (tileFrom == tileTo)
                {
                    // Merge tile
                    tileTo.Value = tileFrom.Value + 1;
                    tileTo.Merged = true;
                    tileFrom.Value = 0;
                    spawn = true;
                }
            }

            // Reset all merges in this column
            for (int i = 0; i < TilesPerRow; i++)
            {
                tileObjects[i, j].Merged = false;
            }
        }

        return spawn;
    }

    private bool MoveRight()
    {
        bool spawn = false;

        // Row
        for (int i = 0; i < TilesPerRow; i++)
        {
            // Column (reverse)
            for (int j = TilesPerRow - 2; j >= 0; j--)
            {
                Tile tileFrom = tileObjects[i, j];

                // If the tile is empty (skip)
                if (tileFrom == 0)
                {
                    continue;
                }

                Tile tileTo = null;

                // Search for empty cells in the same row
                for (int jj = j + 1; jj < TilesPerRow; jj++)
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
                    spawn = true;
                }
                else if (tileFrom == tileTo)
                {
                    // Merge tile
                    tileTo.Value = tileFrom.Value + 1;
                    tileTo.Merged = true;
                    tileFrom.Value = 0;
                    spawn = true;
                }
            }

            // Reset all merges in this row
            for (int j = 0; j < TilesPerRow; j++)
            {
                tileObjects[i, j].Merged = false;
            }
        }

        return spawn;
    }

    // Update is called once per frame
    private void Update()
    {
        // TODO: move to GameController?
        if (Input.GetButtonDown("Cancel"))
        {
            Application.Quit();
        }

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

            tileObjects[1, 0].Value = 1;
            tileObjects[1, 1].Value = 1;
            tileObjects[1, 2].Value = 2;
            tileObjects[1, 3].Value = 3;

            tileObjects[2, 0].Value = 3;
            tileObjects[2, 1].Value = 2;
            tileObjects[2, 2].Value = 1;
            tileObjects[2, 3].Value = 1;

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

            tileObjects[2, 0].Value = 0;
            tileObjects[2, 1].Value = 0;
            tileObjects[2, 2].Value = 1;
            tileObjects[2, 3].Value = 0;

            tileObjects[1, 0].Value = 0;
            tileObjects[1, 1].Value = 1;
            tileObjects[1, 2].Value = 0;
            tileObjects[1, 3].Value = 0;

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

            tileObjects[1, 0].Value = 0;
            tileObjects[1, 1].Value = 0;
            tileObjects[1, 2].Value = 1;
            tileObjects[1, 3].Value = 1;

            tileObjects[2, 0].Value = 0;
            tileObjects[2, 1].Value = 1;
            tileObjects[2, 2].Value = 0;
            tileObjects[2, 3].Value = 1;

            tileObjects[3, 0].Value = 1;
            tileObjects[3, 1].Value = 0;
            tileObjects[3, 2].Value = 0;
            tileObjects[3, 3].Value = 1;
        }
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

        for (int i = 0; i < TilesPerRow; i++)
        {
            for (int j = 0; j < TilesPerRow; j++)
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
