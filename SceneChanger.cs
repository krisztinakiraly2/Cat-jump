using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public delegate void PauseAnimation();
public delegate void StopAnimation();
public delegate void Restart();

public class SceneChanger : MonoBehaviour
{
    public static bool menu = true;
    public static float sink_height;
    public float Sink_Height;

    public static event PauseAnimation pa;
    public static event StopAnimation sa;
    public static event Restart restart;

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
        pa?.Invoke();
    }

    public static void Stop()
    {
        sa?.Invoke();
    }

    public void NewGame()
    {
        ChangeScene("GameScene");
        restart?.Invoke();
    }
}
