using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGeneration : MonoBehaviour
{
    public GameObject group;
    public GameObject prevGroup;

    private Vector3 startPositionGroup;
    private Vector3 startPositionPrevGroup;
    private Vector3 startPositionCameraBounds;

    public GameObject cameraBounds;

    public GameObject player;

    private void Awake()
    {
        startPositionGroup = group.transform.position;
        startPositionPrevGroup = prevGroup.transform.position;
        startPositionCameraBounds = cameraBounds.transform.position;

        GameManager.OnGameStateChanged += GameManagerOnOnGameStateChanged;
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= GameManagerOnOnGameStateChanged;
    }

    public void GameManagerOnOnGameStateChanged(GameState state)
    {
        if (state == GameState.MainMenu)
        {
            // Resetting map position
            group.transform.position = startPositionGroup;
            prevGroup.transform.position = startPositionPrevGroup;
            cameraBounds.transform.position = startPositionCameraBounds;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(player.transform.position.x > group.transform.GetChild(2).transform.position.x)
        {
            var tempGroup = prevGroup;

            prevGroup = group;

            tempGroup.transform.position += new Vector3(160, 0, 0);
            cameraBounds.transform.position += new Vector3(80, 0, 0);

            group = tempGroup;
        }
    }
}
