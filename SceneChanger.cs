using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public delegate void PauseAnimation();
public delegate void StopAnimation();
public delegate void Restart();
public delegate void RefreshOptions();

public class SceneChanger : MonoBehaviour
{
    public static bool menu = true;
    public static float sink_height;
    public float Sink_Height;

    public static bool doneCat = false;
    public static bool doneBg = false;

    public static event PauseAnimation pa;
    public static event StopAnimation sa;
    public static event Restart restart;
    public static event RefreshOptions refresh;

    public void Start()
    {
        sink_height = Sink_Height;
        Menu.pause += Pause;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            OpenMenu();
        }
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        if (sceneName == "GameScene")
        {
            menu = false;
            Menu.first = true;
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

    public static void Pause()
    {
        pa?.Invoke();
    }

    public static void Resume()
    {
        pa?.Invoke();
        refresh?.Invoke();
        doneCat = false;
        doneBg = false;
    }

    public static void Stop()
    {
        sa?.Invoke();
    }

    public void NewGame()
    {
        ChangeScene("GameScene");
        restart?.Invoke();
        doneCat = false;
        doneBg = false;
    }

    public static void OpenMenu()
    {
        refresh?.Invoke();
        doneCat = false;
        doneBg = false;
    }
}
