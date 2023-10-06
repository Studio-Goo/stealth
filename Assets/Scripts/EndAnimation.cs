using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndAnimation : MonoBehaviour
{
    public GameObject endAnimation;
    public AudioSource endAudio;

    private GameObject slime;

    public void HandleEndAnimation()
    {
        slime = Slime.Instance.GetGameObject();
        Vector3 slimePosition = slime.transform.position;
        
        //set end animation position to player position
        Vector3 newPosition = endAnimation.transform.position;
        newPosition.x = slimePosition.x + 5.6f;
        endAnimation.transform.position = newPosition;


        //start the animation
        slime.SetActive(false);

        // Set the "isPlaying" parameter to true in the Animator.
        endAnimation.SetActive(true);
        endAudio.Play();

        StartCoroutine(WaitForAnimationComplete());

        //change the state to playing after the animation
        //GameManager.Instance.UpdateGameState(GameState.Playing);
    }

    IEnumerator WaitForAnimationComplete()
    {
        // Wait for the animation to finish.
        yield return new WaitForSeconds(3.0f);

        endAnimation.SetActive(false);
        endAudio.Stop();
    

        GameManager.Instance.UpdateGameState(GameState.GameOver);
    }
}
