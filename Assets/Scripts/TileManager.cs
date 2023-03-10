using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    /*
     * Fields
     */

    // Tile sorting order
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Variable used by Tile Class")]
    public static int SortOrder = 1;

    // Configuration
    private const int TilesPerRow = 4;
    private const int StartTiles = 2;

    private GameController controller;
    private Preferences pref;

    // Tile array reference
    private Tile[,] tileObjects;

    // Action sound
    private AudioSource audioSource;

    /*
     * Delegates
     */

    public delegate void Score(Tile tile);

    public delegate void State(GameController.GameState state);

    /*
     * Events
     */

    public event Score OnScore;

    public event State OnStateChange;

    /*
     * Properties
     */

    public bool ValidAction { get; set; }

    /*
     * Methods
     */

    public void Move(int x, int y)
    {
        ////Debug.LogFormat("[Move] x: {0}, y: {1}", x, y);

        // Ignore no movement
        if (x == 0 && y == 0)
        {
            return;
        }

        int uX = Mathf.Abs(x);
        int uY = Mathf.Abs(y);

        // Same "weight" on both axis
        if (uX == uY)
        {
            return;
        }

        // Stop all animations in progress
        StopAllCoroutines();

        // Update all tiles
        UpdateTiles();

        // Reset action state
        ValidAction = false;

        // Handle axes behavior
        if (uX > uY)
        {
            // Axis X
            if (x < 0)
            {
                MoveLeft();
            }
            else if (x > 0)
            {
                MoveRight();
            }
        }
        else if (uX < uY)
        {
            // Axis Y
            if (y < 0)
            {
                MoveDown();
            }
            else if (y > 0)
            {
                MoveUp();
            }
        }

        // If an action was made (merge/move)
        if (ValidAction)
        {
            // If the game is not won
            if (controller.CurrentState != GameController.GameState.Winner)
            {
                AddRandomTile();
            }

            if (pref.Sound == Preferences.SoundEffect.On)
            {
                // Play sound
                audioSource.Play();
            }
        }

        // Check if there is no more actions
        if (!MovesAvailable())
        {
            // Notify about game state change
            OnStateChange(GameController.GameState.Loser);
        }
    }

    private void UpdateTiles()
    {
        // For each row
        for (int i = 0; i < TilesPerRow; i++)
        {
            // Reset all tiles in this row
            for (int j = 0; j < TilesPerRow; j++)
            {
                Tile tile = tileObjects[i, j];
                tile.Merged = false;
                tile.Refresh();
                tile.transform.localPosition = Vector3.zero;
                tile.transform.localScale = Vector3.one;
                SortOrder = 1;
            }
        }
    }

    // Use this for initialization
    private void Start()
    {
        // Game Controller
        controller = GameObject.FindObjectOfType<GameController>();

        // Preferences
        pref = GetComponent<Preferences>();

        // Action sound
        audioSource = GetComponent<AudioSource>();

        // Allocate memory
        tileObjects = new Tile[TilesPerRow, TilesPerRow];

        // For each row
        for (int i = 0; i < TilesPerRow; i++)
        {
            // For each column
            for (int j = 0; j < TilesPerRow; j++)
            {
                // Find tile
                string name = string.Format("Tiles/Tile ({0}, {1})/Tile", i, j);
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

        // Notify about game state change
        OnStateChange(GameController.GameState.Playing);
    }

    private void MoveUp()
    {
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

                if (tileTo == 0)
                {
                    MoveTile(tileFrom, tileTo);
                }
                else if (tileFrom == tileTo)
                {
                    MergeTile(tileFrom, tileTo);
                }
            }
        }
    }

    private void MoveLeft()
    {
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

                if (tileTo == 0)
                {
                    MoveTile(tileFrom, tileTo);
                }
                else if (tileFrom == tileTo)
                {
                    MergeTile(tileFrom, tileTo);
                }
            }
        }
    }

    private void MoveDown()
    {
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

                if (tileTo == 0)
                {
                    MoveTile(tileFrom, tileTo);
                }
                else if (tileFrom == tileTo)
                {
                    MergeTile(tileFrom, tileTo);
                }
            }
        }
    }

    private void MoveRight()
    {
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

                if (tileTo == 0)
                {
                    MoveTile(tileFrom, tileTo);
                }
                else if (tileFrom == tileTo)
                {
                    MergeTile(tileFrom, tileTo);
                }
            }
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

            Tile tile = tileObjects[x, y];
            tile.Value = value;

            // Disabled animations
            if (pref.Animation == Preferences.AnimationEffect.None)
            {
                tile.Refresh();
            }
            else
            {
                // Play animation
                StartCoroutine(tile.AppearAnimation());
            }
        }
    }

    private void MergeTile(Tile tileFrom, Tile tileTo)
    {
        tileTo.Value = tileFrom.Value + 1;
        tileTo.Merged = true;
        tileFrom.Value = 0;

        ValidAction = true;

        // Notify event
        OnScore(tileTo);

        // Simple animations
        if (pref.Animation < Preferences.AnimationEffect.Complete)
        {
            tileFrom.Refresh();
            tileTo.Refresh();
        }
        else
        {
            // Play animation
            StartCoroutine(tileFrom.MergeAnimation(tileTo));
        }

        // Tile 4096
        if (tileTo == 12)
        {
            // Notify about game state change
            OnStateChange(GameController.GameState.Winner);
        }
    }

    private void MoveTile(Tile tileFrom, Tile tileTo)
    {
        tileTo.Value = tileFrom.Value;
        tileFrom.Value = 0;

        ValidAction = true;

        // Simple animations
        if (pref.Animation < Preferences.AnimationEffect.Complete)
        {
            tileTo.Refresh();
            tileFrom.Refresh();
        }
        else
        {
            // Play animation
            StartCoroutine(tileFrom.MoveAnimation(tileTo));
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
                if (tileObjects[i, j] == 0)
                {
                    cells.Add(new Vector2(i, j));
                }
            }
        }

        return cells;
    }

    private Tile GetTile(int i, int j)
    {
        if (i < 0 || i >= TilesPerRow)
        {
            return null;
        }

        if (j < 0 || j >= TilesPerRow)
        {
            return null;
        }

        return tileObjects[i, j];
    }

    private bool TileMatchesAvailable()
    {
        // Row
        for (int i = 0; i < TilesPerRow; i++)
        {
            // Column
            for (int j = 0; j < TilesPerRow; j++)
            {
                Tile tile = tileObjects[i, j];

                // If the tile is empty (skip)
                if (tile == 0)
                {
                    continue;
                }

                // Check all directions
                if (tile == GetTile(i, j + 1))
                {
                    // Up
                    return true;
                }
                else if (tile == GetTile(i - 1, j))
                {
                    // Left
                    return true;
                }
                else if (tile == GetTile(i, j - 1))
                {
                    // Down
                    return true;
                }
                else if (tile == GetTile(i + 1, j))
                {
                    // Right
                    return true;
                }
            }
        }

        return false;
    }

    private bool MovesAvailable()
    {
        if (AvailableCells().Count != 0)
        {
            return true;
        }

        if (TileMatchesAvailable())
        {
            return true;
        }

        return false;
    }
}
