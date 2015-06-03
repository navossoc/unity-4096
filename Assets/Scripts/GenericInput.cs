﻿using UnityEngine;

public class GenericInput : MonoBehaviour
{
    public KeyDown OnKeyDown;

    private Vector2 lastAxesValues = Vector2.zero;

    public delegate void KeyDown(int x, int y);

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

            OnKeyDown((int)tempX, (int)tempY);
        }
    }
}