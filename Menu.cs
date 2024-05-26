using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public delegate void Pause();

public class Menu : MonoBehaviour
{
    public GameObject menuParent;
    public GameObject gameOverParent;
    public static event Pause pause;

    private bool isMenuShown = false;
    public static bool first = true;
    
    void Start()
    {
        SceneChanger.refresh += ControllMenu;
        SceneChanger.restart += Restart;
        ControllCat.endGame += GameOver;
    }

    void GameOver()
    {
        if(gameOverParent != null)
            gameOverParent.transform.position = new Vector3(730,700-1560,0);
    }

    public void Restart()
    {
        if(gameOverParent != null)
            gameOverParent.transform.position = new Vector3(730,1630,0);
    }

    public void ControllMenu()
    {
        if(isMenuShown == true)
        {
            isMenuShown = false;
            if(menuParent != null)
                menuParent.transform.position = new Vector3(730,1630,0);
            if(first)
                first = false;
            else
                {
                    pause?.Invoke();
                }
        }
        else
        {
            isMenuShown = true;
            if (menuParent != null)
                menuParent.transform.position = new Vector3(730,700,0);
            if(first)
                first = false; 
            else
                {
                    pause?.Invoke();
                }
        }
    }
}
