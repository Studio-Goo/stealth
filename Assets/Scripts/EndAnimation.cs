using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndAnimation : MonoBehaviour
{
    public GameObject endAnimation;
    public GameObject player;
    public AudioSource endAudio;
    // Start is called before the first frame update
    public void Start()
    {
        //endAnimation.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HandleEndAnimation()
    {
        //get player position
        Vector3 playerPosition = player.transform.position;
        
        //set end animation position to player position
        Vector3 newPosition = endAnimation.transform.position;
        newPosition.x = playerPosition.x + 1.5f;
        endAnimation.transform.position = newPosition;


        //start the animation
        player.SetActive(false);

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
