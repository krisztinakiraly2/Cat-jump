using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlGame : MonoBehaviour
{
    public GameObject GameParent;
    public Animator GameAnimator;
    bool isAnimationStopped = false;

    private void PauseAnimation()
    {
        if (GameAnimator != null && !SceneChanger.doneBg)
        {
            isAnimationStopped = !isAnimationStopped;
            GameAnimator.enabled = !GameAnimator.enabled;
            SceneChanger.doneBg = true;
        }
    }

    private void StopAnimation()
    {
        SceneChanger.pa -= PauseAnimation;
        if (GameAnimator != null)
        {
            isAnimationStopped = true;
            GameAnimator.enabled = false;
        }
    }

    private void restart()
    {
        SceneChanger.pa += PauseAnimation;
        SceneChanger.sa += StopAnimation;
    }

    // Start is called before the first frame update
    void Start()
    {
        GameAnimator.SetBool("Start", true);
        SceneChanger.restart += restart;
        restart();
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneChanger.menu)
            GameAnimator.SetBool("Start", false);
    }

    public void MoveParentDown()
    {
        if(!isAnimationStopped)
        {
            float X = GameParent.transform.position.x;
            float Y = GameParent.transform.position.y;
            GameParent.transform.position = new Vector3(GameParent.transform.position.x, GameParent.transform.position.y - SceneChanger.sink_height, 0);
        }
    }

}
