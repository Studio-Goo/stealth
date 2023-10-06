using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartAnimation : MonoBehaviour
{
    public GameObject startAnimation;
    public GameObject player;
    public GameObject slimePosition;
    public AudioSource startAudio;

    public void Start()
    {
        PlayerPrefs.SetInt("obstaclePassed", 0);
    }

    public void HandleStartAnimation()
    {
        // Set the "isPlaying" parameter to true in the Animator.
        startAnimation.SetActive(false);
        startAnimation.SetActive(true);
        startAudio.Play();
        slimePosition.SetActive(true);
        slimePosition.GetComponent<SlimePosition>().isAnimating = true;

        StartCoroutine(WaitForAnimationComplete());
    }

    IEnumerator WaitForAnimationComplete()
    {
        // Wait for the animation to finish.
        yield return new WaitForSeconds(4.4f);

        startAudio.Stop();
        slimePosition.SetActive(false);
        slimePosition.GetComponent<SlimePosition>().isAnimating = false;
        player.SetActive(true);

        GameManager.Instance.UpdateGameState(GameState.Playing);
    }
}
