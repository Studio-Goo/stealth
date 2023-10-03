using UnityEngine;

public class SlimePosition : MonoBehaviour
{
    private float startX = -62.18f; // Initial X position
    private float endX = -55.14f;   // Desired spawn X position
    private float duration = 4.4f;  // Duration of the animation in seconds

    public float journeyLength;
    public float distanceCovered;
    public float fractionOfJourney;
    public float newX;
    private float startTime;

    public GameObject slime;

    void Start()
    {
        startTime = Time.time;
    }

    void Update()
    {
        // Calculate how far along the animation we are as a value between 0 and 1.
        journeyLength = Mathf.Abs(endX - startX);
        distanceCovered = (Time.time - startTime) * (journeyLength / duration);
        fractionOfJourney = distanceCovered / journeyLength;

        // Calculate the new X position for the slime using Mathf.Lerp.
        newX = Mathf.Lerp(startX, endX, fractionOfJourney);
        
        // Update the slime's position, keeping its Y and Z coordinates unchanged.
        Vector3 newPosition = new Vector3(newX, slime.transform.position.y, slime.transform.position.z);
        slime.transform.position = newPosition;

    }

    public void ResetPosition()
    {
        journeyLength = 0;
        distanceCovered = 0;
        fractionOfJourney = 0;
        newX = 0;
        startTime = Time.time;
    }
}
