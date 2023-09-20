using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Animator>().SetBool("isPlayingEnd", false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HandleStartAnimation()
    {
        // Set the "isPlaying" parameter to true in the Animator.
        GetComponent<Animator>().SetBool("isPlayingEnd", true);

        // Now you can also switch the game state to Playing if needed.
        //UpdateGameState(GameState.Playing);
    }
}
