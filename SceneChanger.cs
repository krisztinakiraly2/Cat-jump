using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public static bool menu = true;
    public static float sink_height;
    public float Sink_Height;

    private void Start()
    {
        sink_height = Sink_Height;
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        if (sceneName == "GameScene")
        {
            menu = false;
        }
        else
        {
            menu = true;
        }

    }

    public void Exit()
    {
        Application.Quit();
    }

    public void Pause()
    {

    }

    public void NewGame()
    {
        ChangeScene("GameScene");
    }
}
