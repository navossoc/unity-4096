using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    private int value = 0;

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

    /// <summary>
    /// Get tile value
    /// </summary>
    /// <returns>return an int</returns>
    public int GetValue()
    {
        return value > 0 ? (1 << value) : 0;
    }

    /// <summary>
    /// Set Tile value
    /// </summary>
    /// <param name="number">tile value (power of 2)</param>
    public void SetValue(int number)
    {
        if (number < 0 || number > 12)
        {
            Debug.LogErrorFormat("[{0}] Number {1} out of range", name, number);
        }
        number = Mathf.Clamp(number, 0, 12);

        value = number;
        if (value > 0)
        {
            tileText.text = (1 << value).ToString();
        }
        else
        {
            tileText.text = "";
        }

        UpdateColor();
    }

    /// <summary>
    /// Change Tile color based on his value
    /// </summary>
    private void UpdateColor()
    {
        // Background
        tileImage.color = colorTable[value];
        // Text color
        if (value <= 2)
        {
            tileText.color = new Color32(119, 110, 101, 255);
        }
        else
        {
            tileText.color = new Color32(249, 246, 242, 255);
        }
    }
}
