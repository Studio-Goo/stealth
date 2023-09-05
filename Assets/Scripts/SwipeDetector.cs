using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeDetector : MonoBehaviour
{
    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;
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
            if (direction >= -0.3f && direction <= 0.3f)
            {

                if(endTouchPosition.x > startTouchPosition.x)
                {
                    Debug.Log(">>>");
                    slime.lastGesture = Shape.Horizontal;
                } else
                {
                    Debug.Log("<<<");
                    slime.lastGesture = Shape.Horizontal;
                }
            }

            // Diagonal Positive (bottom left to top right)
            if(direction > 0.3f && direction <= 5)
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
            if (direction < -0.3f && direction >= -5)
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
            if (direction > 5 && direction < 10000 || direction < -5 ||  double.IsInfinity(direction))
            {

                if (startTouchPosition.y > endTouchPosition.y)
                {
                    Debug.Log("Down");
                    slime.lastGesture = Shape.Vertical;

                }
                else
                {
                    Debug.Log("Up");
                    slime.lastGesture = Shape.Vertical;
                }
            }

            //if (endtouchposition.x == starttouchposition.x)
            //{
            //    debug.log("jump");
            //}

            //if (endtouchposition.x > starttouchposition.x && ydiff > -50 && ydiff < 50)
            //{
            //    debug.log(">>>");
            //}

            //if (endtouchposition.x < starttouchposition.x && ydiff > -50 && ydiff < 50)
            //{
            //    debug.log("<<<");
            //}

            //if (starttouchposition.y < endtouchposition.y && xdiff > -50 && xdiff < 50)
            //{
            //    debug.log("up");
            //}

            //if (starttouchposition.y > endtouchposition.y && xdiff > -50 && xdiff < 50)
            //{
            //    debug.log("down");
            //}
        }
    }
}
