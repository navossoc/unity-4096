using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    private int exponent = 0;

    private Image tileImage;
    private Text tileText;

    private static Color32[] colorTable =
    {
        new Color32(204, 192, 179, 255),    // 1 (none)
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

    // Use this for initialization
    void Awake()
    {
        tileImage = GetComponent<Image>();
        tileText = GetComponentInChildren<Text>();
    }

    /*
     * Properties
     */

    public bool Merged { get; set; }

    public int Value
    {
        get
        {
            return exponent;
        }

        set
        {
            if (value < 0 || value > 12)
            {
                Debug.LogErrorFormat("[{0}] Number {1} out of range", name, value);
            }
            value = Mathf.Clamp(value, 0, 12);

            if (value > 0)
            {
                tileText.text = (1 << value).ToString();
            }
            else
            {
                tileText.text = "";
            }
            exponent = value;

            UpdateColor();
        }
    }

    /// <summary>
    /// Change Tile color based on his value
    /// </summary>
    private void UpdateColor()
    {
        // Background
        tileImage.color = colorTable[exponent];
        // Text color
        if (exponent <= 2)
        {
            tileText.color = new Color32(119, 110, 101, 255);
        }
        else
        {
            tileText.color = new Color32(249, 246, 242, 255);
        }
    }

    public override string ToString()
    {
        return string.Format("{0} = 2^{1}", name, exponent);
    }

    /*
     * Operators
     */

    public static bool operator ==(Tile lhs, Tile rhs)
    {
        // If both are null, or both are same instance, return true.
        if (System.Object.ReferenceEquals(lhs, rhs))
        {
            return true;
        }

        // If one is null, but not both, return false.
        if (((object)lhs == null) || ((object)rhs == null))
        {
            return false;
        }

        // Return true if the fields match:
        return lhs.Value == rhs.Value;
    }

    public static bool operator !=(Tile lhs, Tile rhs)
    {
        return !(lhs == rhs);
    }

    public static bool operator ==(Tile tile, int number)
    {
        // If is null, return false.
        if (((object)tile == null))
        {
            return false;
        }

        // Return true if the fields match:
        return tile.Value == number;
    }

    public static bool operator !=(Tile tile, int number)
    {
        return !(tile == number);
    }
}
