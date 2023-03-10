using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class TouchInput : MonoBehaviour
{
    /*
     * Fields
     */

    [Range(5, 100)]
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Can be tweaked on inspector")]
    public float MinSwipeDist = 10f;

    [Range(0f, 1f)]
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Can be tweaked on inspector")]
    public float MinSwipeTime = 0.05f;

    [Range(1f, 5f)]
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Can be tweaked on inspector")]
    public float MaxSwipeTime = 1f;

    private const int Deadzone = 5;

    private float startTime;
    private Vector2 startPos;

    /*
     * Delegates
     */

    public delegate void SwipeAction(int x, int y);

    /*
     * Events
     */

    public event SwipeAction OnSwipe;

    /*
     * Methods
     */

    // Update is called once per frame
    private void Update()
    {
        // Must have at least one touch on the screen
        if (Input.touchCount < 1)
        {
            return;
        }

        // First touch
        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began)
        {
            startPos = touch.position;
            startTime = Time.time;
        }
        else if (touch.phase == TouchPhase.Ended)
        {
            // Delta position
            Vector2 swipePosition = touch.position - startPos;

            // Vector magnitude (^2)
            float swipeDistance = swipePosition.sqrMagnitude;

            // Cancel short distances
            if (swipeDistance < (MinSwipeDist * MinSwipeDist))
            {
                Debug.LogWarningFormat("[Swipe] Too short distance {0}", swipeDistance);
                return;
            }

            float swipeTime = Time.time - startTime;

            // Cancel short times
            if (swipeTime < MinSwipeTime)
            {
                Debug.LogWarningFormat("[Swipe] Too short time {0}", swipeTime);
                return;
            }

            // Cancel long times
            if (swipeTime > MaxSwipeTime)
            {
                Debug.LogWarningFormat("[Swipe] Too long time {0}", swipeTime);
                return;
            }

            Vector2 swipeDirection = swipePosition.normalized;

            int swipeDirectionX = Mathf.RoundToInt(swipeDirection.x * Deadzone);
            int swipeDirectionY = Mathf.RoundToInt(swipeDirection.y * Deadzone);

            if (OnSwipe != null)
            {
                OnSwipe(swipeDirectionX, swipeDirectionY);
            }

#if UNITY_EDITOR
            // Draw the swipe gesture
            Debug.DrawLine(startPos, touch.position, Color.magenta, 3f, false);
#endif
        }
    }
}
