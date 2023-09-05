using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventScript : MonoBehaviour
{
    //insert list for obstacle prefabs
    UnityEvent m_MyEvent;

    // Start is called before the first frame update
    void Start()
    {
        m_MyEvent.AddListener(testCase);
    }

    public void testCase()
    {
        print("Test Case");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
