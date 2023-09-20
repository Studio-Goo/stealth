using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartAnimation : MonoBehaviour
{
    public GameObject startAnimation;
    public GameObject player;
    // Start is called before the first frame update
    public void Start()
    {
        //startAnimation.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HandleStartAnimation()
    {
        // Set the "isPlaying" parameter to true in the Animator.
        startAnimation.SetActive(true);

        StartCoroutine(WaitForAnimationComplete());

        //change the state to playing after the animation
        //GameManager.Instance.UpdateGameState(GameState.Playing);
    }

    IEnumerator WaitForAnimationComplete()
    {
        // Wait for the animation to finish.
        yield return new WaitForSeconds(5.0f);

        startAnimation.SetActive(false);
        player.SetActive(true);
    

        GameManager.Instance.UpdateGameState(GameState.Playing);
    }
}
