using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    /*
     * Fields
     */

    // animation
    private const float AppearingTime = 0.2f;
    private const float MergingTime = 0.05f;
    private const float MovingTime = 0.1f;
    private static readonly Vector3 MergingScale = new Vector3(1.2f, 1.2f, 1.2f);

    private static readonly Color32[] ImageColors =
    {
        Color.clear,                        // 1 (none)
        new Color32(238, 228, 218, 255),    // 2
        new Color32(237, 224, 200, 255),    // 4
        new Color32(242, 177, 121, 255),    // 8
        new Color32(245, 149, 99, 255),     // 16
        new Color32(246, 124, 95, 255),     // 32
        new Color32(246, 94, 59, 255),      // 64
        new Color32(237, 207, 114, 255),    // 128
        new Color32(237, 204, 97, 255),     // 256
        new Color32(237, 200, 80, 255),     // 512
        new Color32(237, 197, 63, 255),     // 1024
        new Color32(237, 194, 46, 255),     // 2048
        new Color32(237, 194, 46, 255),     // 4096
    };

    private static readonly Color32[] TextColors =
    {
        Color.clear,                        // 1 (none)
        new Color32(119, 110, 101, 255),    // 2
        new Color32(119, 110, 101, 255),    // 4
        new Color32(249, 246, 242, 255),    // 8+
    };

    private Canvas tileCanvas;
    private Image tileImage;
    private Text tileText;

    /*
     * Properties
     */

    public Canvas TileCanvas
    {
        get { return tileCanvas ?? (tileCanvas = GetComponent<Canvas>()); }
    }

    public Image TileImage
    {
        get { return tileImage ?? (tileImage = GetComponent<Image>()); }
    }

    public Text TileText
    {
        get { return tileText ?? (tileText = GetComponentInChildren<Text>()); }
    }

    public bool Merged { get; set; }

    public int Score
    {
        get
        {
            int result = 1;
            for (int i = 0; i < Value; i++)
            {
                result *= 2;
            }

            return result;
        }
    }

    public int Value { get; set; }

    /*
     * Methods - Operators
     */

    public static bool operator ==(Tile lhs, Tile rhs)
    {
        // If both are null, or both are same instance, return true
        if (object.ReferenceEquals(lhs, rhs))
        {
            return true;
        }

        // If one is null, but not both, return false
        if (((object)lhs == null) || ((object)rhs == null))
        {
            return false;
        }

        // Return true if the value match
        return lhs.Value == rhs.Value;
    }

    public static bool operator !=(Tile lhs, Tile rhs)
    {
        return !(lhs == rhs);
    }

    public static bool operator ==(Tile tile, int number)
    {
        // If is null, return false
        if ((object)tile == null)
        {
            return false;
        }

        // Return true if the value match
        return tile.Value == number;
    }

    public static bool operator !=(Tile tile, int number)
    {
        return !(tile == number);
    }

    /*
     * Methods - Override
     */

    public override bool Equals(object obj)
    {
        // If parameter is null return false
        if (obj == null)
        {
            return false;
        }

        // If parameter cannot be cast to Tile return false
        Tile tile = obj as Tile;
        if ((object)tile == null)
        {
            return false;
        }

        // Return true if the value match
        return Value == tile.Value;
    }

    public bool Equals(Tile tile)
    {
        // If parameter is null return false
        if ((object)tile == null)
        {
            return false;
        }

        // Return true if the value match
        return Value == tile.Value;
    }

    public override int GetHashCode()
    {
        return Merged ? -Value : Value;
    }

    public override string ToString()
    {
        return string.Format("{0} = 2^{1}", name, Value);
    }

    /*
     * Animations
     */

    public IEnumerator AppearAnimation()
    {
        ////Debug.LogFormat("Appear: {0}", transform.parent.name);

        // Wait the other tiles move first
        yield return new WaitForSeconds(MovingTime);

        // Update color/text
        Refresh();

        // Bring to front
        BringToFront();

        // Scale
        for (float t = 0f; t < 1f; t += Time.deltaTime / AppearingTime)
        {
            transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, t);
            yield return null;
        }

        // Make sure the scale is reached
        transform.localScale = Vector3.one;
    }

    public IEnumerator MergeAnimation(Tile tileTo)
    {
        ////Debug.LogFormat("Merge: {0} => {1}", transform.parent.name, tileTo.transform.parent.name);

        // Wait for tile move
        IEnumerator it = MoveAnimation(tileTo);
        while (it.MoveNext())
        {
            yield return it.Current;
        }

        // Bring to front
        BringToFront();

        // Scale to 120%
        for (float t = 0f; t < 1f; t += Time.deltaTime / MergingTime)
        {
            tileTo.transform.localScale = Vector3.Lerp(Vector3.one, MergingScale, t);
            yield return null;
        }

        // Scale to 100%
        for (float t = 0f; t < 1f; t += Time.deltaTime / MergingTime)
        {
            tileTo.transform.localScale = Vector3.Lerp(MergingScale, Vector3.one, t);
            yield return null;
        }

        // Make sure the scale is reached
        tileTo.transform.localScale = Vector3.one;

        // Reset position
        transform.localPosition = Vector3.zero;
    }

    public IEnumerator MoveAnimation(Tile tileTo)
    {
        ////Debug.LogFormat("Move: {0} => {1}", transform.parent.name, tileTo.transform.parent.name);

        // Distance between tiles
        Vector3 deltaLocalTo = tileTo.transform.parent.localPosition - transform.parent.localPosition;

        // Bring to front
        BringToFront();

        // Move
        for (float t = 0f; t < 1f; t += Time.deltaTime / MovingTime)
        {
            transform.localPosition = Vector3.Lerp(Vector3.zero, deltaLocalTo, t);
            yield return null;
        }

        // Make sure the position is reached
        transform.localPosition = deltaLocalTo;

        // Update color/text
        tileTo.Refresh();

        // Update color/text
        Refresh();

        // Reset position
        transform.localPosition = Vector3.zero;
    }

    /*
     * Methods
     */

    /// <summary>
    /// Update Tile graphics
    /// </summary>
    public void Refresh()
    {
        UpdateColor();
        UpdateText();
        SendToBack();
    }

    // Use this for initialization
    private void Start()
    {
        Refresh();
    }

    /// <summary>
    /// Change Tile color based on his exponent value
    /// </summary>
    private void UpdateColor()
    {
        // Background
        TileImage.color = ImageColors[Value];

        // Text color
        if (Value <= 2)
        {
            TileText.color = TextColors[Value];
        }
        else
        {
            TileText.color = TextColors[3];
        }
    }

    /// <summary>
    /// Change Tile text based on his exponent value
    /// </summary>
    private void UpdateText()
    {
        if (Value == 0)
        {
            TileText.text = string.Empty;
        }
        else
        {
            TileText.text = (1 << Value).ToString();
        }
    }

    /// <summary>
    /// Bring Tile to front layer
    /// </summary>
    private void BringToFront()
    {
        TileCanvas.sortingOrder = ++TileManager.SortOrder;
    }

    /// <summary>
    /// Send Tile to back layer
    /// </summary>
    private void SendToBack()
    {
        TileCanvas.sortingOrder = 1;
    }
}
