using UnityEngine;

public class GenericInput : MonoBehaviour
{
    /*
     * Fields
     */

    private Vector2 lastAxesValues = Vector2.zero;

    /*
     * Delegates
     */

    public delegate void KeyDown(int x, int y);

    /*
     * Events
     */

    public event KeyDown OnKeyDown;

    /*
     * Methods
     */

    // Update is called once per frame
    private void Update()
    {
        float tempX = Input.GetAxisRaw("Horizontal");
        float tempY = Input.GetAxisRaw("Vertical");

        // Compare last value
        if (lastAxesValues.x != tempX || lastAxesValues.y != tempY)
        {
            // Update last value
            lastAxesValues.x = tempX;
            lastAxesValues.y = tempY;

            if (OnKeyDown != null)
            {
                OnKeyDown((int)tempX, (int)tempY);
            }
        }
    }
}
