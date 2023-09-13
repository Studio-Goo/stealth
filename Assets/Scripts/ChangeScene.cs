using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void Start()
    {
        Debug.developerConsoleVisible = true;
    }

    public void MoveToScene(int sceneID)
    {
        Debug.LogError(sceneID);
        SceneManager.LoadScene(sceneID);
    }
}
