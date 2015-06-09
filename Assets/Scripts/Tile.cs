using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
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

    private Image tileImage;
    private Text tileText;

    /*
     * Properties
     */

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
            for (int i = 0; i < Exponent; i++)
            {
                result *= 2;
            }

            return result;
        }
    }

    public int Value
    {
        get
        {
            return Exponent;
        }

        set
        {
            if (value < 0 || value > 12)
            {
                Debug.LogErrorFormat("[{0}] Number {1} out of range", name, value);
            }

            value = Mathf.Clamp(value, 0, 12);

            if (value == 0)
            {
                TileText.text = string.Empty;
            }
            else
            {
                TileText.text = (1 << value).ToString();
            }

            Exponent = value;

            UpdateColor();
        }
    }

    private int Exponent { get; set; }

    /*
     * Operators
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
     * Override
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
        return string.Format("{0} = 2^{1}", name, Exponent);
    }

    // Use this for initialization
    private void Start()
    {
        Value = 0;
    }

    /// <summary>
    /// Change Tile color based on his value
    /// </summary>
    private void UpdateColor()
    {
        // Background
        TileImage.color = ImageColors[Exponent];

        // Text color
        if (Exponent <= 2)
        {
            TileText.color = TextColors[Exponent];
        }
        else
        {
            TileText.color = TextColors[3];
        }
    }
}
