using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGeneration : MonoBehaviour
{

    public GameObject ceiling;
    public GameObject background;
    public GameObject floor;
    public GameObject prevCeiling;
    public GameObject prevBackground;
    public GameObject prevFloor;

    public GameObject cameraBounds;

    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(player.transform.position.x > floor.transform.position.x)
        {
            var tempCeiling = prevCeiling;
            var tempBackground = prevBackground;
            var tempFloor = prevFloor;

            prevCeiling = ceiling;
            prevBackground = background;
            prevFloor = floor;

            tempCeiling.transform.position += new Vector3(160, 0, 0);
            tempBackground.transform.position += new Vector3(160, 0, 0);
            tempFloor.transform.position += new Vector3(160, 0, 0);
            cameraBounds.transform.position += new Vector3(80,0,0);

            ceiling = tempCeiling;
            background = tempBackground;
            floor = tempFloor;
        }
    }
}
