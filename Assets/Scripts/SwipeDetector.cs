using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeDetector : MonoBehaviour
{
    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;
    public Slime slime;

    // If distance between start & end is higher, count as a swipe.
    private const float SWIPE_TRESHOLD = 100.0f;

    private void Update()
    {
        if (Input.touchCount > 0 && GameManager.Instance.gameState == GameState.Playing)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:

                    // Saving touch start position
                    startTouchPosition = Input.GetTouch(0).position;


                    break;

                case TouchPhase.Ended:


                    endTouchPosition = Input.GetTouch(0).position;

                    // Calculating difference between touch start and end
                    Vector2 difference = endTouchPosition - startTouchPosition;


                    if(difference.magnitude > SWIPE_TRESHOLD)
                    {
                        if (Mathf.Abs(difference.x) > Mathf.Abs(difference.y))
                        {

                            //horizontal swipe (left & right)
                            //slime.UpdateGesture(difference.x > 0 ? Shape.Horizontal /* right */ : Shape.Horizontal /* left */);


                        }
                        else
                        {

                            // Vertical swipe ( Up & Down )
                            slime.UpdateGesture(difference.y > 0 ? Shape.Up /* up */ : Shape.Down /* down */);


                        }

                        // TODO swipe success indication (shader)
                        // ...

                    }
                    else
                    {

                        // Swipe treshold not exceeded, so detected as TAP
                        slime.UpdateGesture(Shape.Suspended);


                    }

                    break;
            }
        }
    }
}
