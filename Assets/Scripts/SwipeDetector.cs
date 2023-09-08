using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeDetector : MonoBehaviour
{
    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;
    // If distance between start & end is higher, count as a swipe.
    private const float SWIPE_TRESHOLD = 1.0f;
    public Slime slime;

    private void Update()
    {
        if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            startTouchPosition = Input.GetTouch(0).position;
        }

        if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            endTouchPosition = Input.GetTouch(0).position;

            // Calculating the slope between the start and endpoint
            var dy = startTouchPosition.y - endTouchPosition.y;
            var dx = startTouchPosition.x - endTouchPosition.x;
            var direction = dy / dx;

            // Horizontal
            if (direction >= -SWIPE_TRESHOLD && direction <= SWIPE_TRESHOLD)
            {

                if(endTouchPosition.x > startTouchPosition.x)
                {
                    Debug.Log(">>>");
                    slime.UpdateGesture(Shape.Horizontal);
                } else
                {
                    Debug.Log("<<<");
                    slime.UpdateGesture(Shape.Horizontal);
                }
            }

            // Diagonal Positive (bottom left to top right)
            else if(direction > SWIPE_TRESHOLD && direction <= 5)
            {
                if (startTouchPosition.y > endTouchPosition.y)
                {
                    Debug.Log("<<< Down");
                }
                else
                {
                    Debug.Log(">>> Up");
                }
            }

            // Diagonal Negative (top left to bottom right)
            else if (direction < -SWIPE_TRESHOLD && direction >= -5)
            {
                if (startTouchPosition.y > endTouchPosition.y)
                {
                    Debug.Log(">>> Down");
                }
                else
                {
                    Debug.Log("<<< Up");
                }
            }

            // Vertical
            else if (direction > 5 && direction < 10000 || direction < -5 ||  double.IsInfinity(direction))
            {

                if (startTouchPosition.y > endTouchPosition.y)
                {
                    Debug.Log("Down");
                    slime.UpdateGesture(Shape.Vertical);

                }
                else
                {
                    Debug.Log("Up");
                    slime.UpdateGesture(Shape.Vertical);
                }
            }

            else {
                Debug.Log("Tap");
                slime.UpdateGesture(Shape.Suspended);
            }
        }
    }
}
