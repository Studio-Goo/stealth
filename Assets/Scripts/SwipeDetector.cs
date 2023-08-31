using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeDetector : MonoBehaviour
{
    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;

    private void Update()
    {
        if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            startTouchPosition = Input.GetTouch(0).position;
        }

        if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            endTouchPosition = Input.GetTouch(0).position;

            var yDiff = startTouchPosition.y - endTouchPosition.y;
            var xDiff = startTouchPosition.x - endTouchPosition.x;

            if (endTouchPosition.x > startTouchPosition.x && yDiff > -50 && yDiff < 50)
            {
                Debug.Log(">>>");
            }

            if (endTouchPosition.x < startTouchPosition.x && yDiff > -50 && yDiff < 50)
            {
                Debug.Log("<<<");
            }

            if(startTouchPosition.y < endTouchPosition.y && xDiff > -50 && xDiff < 50)
            {
                Debug.Log("UP");
            }

            if (startTouchPosition.y > endTouchPosition.y && xDiff > -50 && xDiff < 50)
            {
                Debug.Log("DOWN");
            }
        }
    }
}
