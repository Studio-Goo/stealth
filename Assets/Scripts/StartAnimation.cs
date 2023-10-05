using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartAnimation : MonoBehaviour
{
    public GameObject startAnimation;
    public GameObject player;
    public Slime slimeScript;
    public GameObject slimePosition;
    public AudioSource startAudio;
    // Start is called before the first frame update
    public void Start()
    {
        //startAnimation.SetActive(false);
        //startAudio.SetActive(true);

        PlayerPrefs.SetInt("obstaclePassed", 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HandleStartAnimation()
    {
        // Set the "isPlaying" parameter to true in the Animator.
        startAnimation.SetActive(true);
        startAudio.Play();
        slimePosition.SetActive(true);

        StartCoroutine(WaitForAnimationComplete());

        //change the state to playing after the animation
        //GameManager.Instance.UpdateGameState(GameState.Playing);
    }

    IEnumerator WaitForAnimationComplete()
    {
        // Wait for the animation to finish.
        yield return new WaitForSeconds(4.4f);

        //startAnimation.SetActive(false);
        startAudio.Stop();
        player.SetActive(true);
        slimePosition.SetActive(false);

    

        GameManager.Instance.UpdateGameState(GameState.Playing);
    }
}
