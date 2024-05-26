using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public delegate void EndGame();

public class ControllCat : MonoBehaviour
{
    public Animator catAnimator;
    public GameObject catParent;
    public Game game;
    public float jump_height;
    public float jump_lenght;
    public static float Jump_Height;
    public float jump_height_shift;
    bool isAnimationStopped = false;
    bool hasNotCollided = true;
    float rightBorder = 1800;
    float leftBorder = 0;
    public static bool MoveFieldDown = false;

    public static event EndGame endGame;

    private void PauseAnimation()
    {
        if (catAnimator != null && !SceneChanger.doneCat)
        {
            isAnimationStopped = !isAnimationStopped;
            catAnimator.enabled = !catAnimator.enabled;
            SceneChanger.doneCat = true;
        }
    }

    private void StopAnimation()
    {
        SceneChanger.pa -= PauseAnimation;
        if (catAnimator != null)
        {
            isAnimationStopped = true;
            catAnimator.enabled = false;
        }
    }

    private void restart()
    {
        SceneChanger.pa += PauseAnimation;
        SceneChanger.sa += StopAnimation;
    }

    private void Start()
    {
        Jump_Height = jump_height;
        jump_height += 10;
        SceneChanger.restart += restart;
        restart();
        catAnimator.SetBool("Stay", true);
    }

    // Update is called once per frame
    public void MoveDown()
    {
        catAnimator.SetBool("Left",false);
        catAnimator.SetBool("Right",false);
    }

    public void Jump(bool isLeft)
    {
        if (isLeft)
        {
            catAnimator.SetBool("Left", true);
            catAnimator.SetBool("Right", false);
        }
        else
        {
            catAnimator.SetBool("Left", false);
            catAnimator.SetBool("Right", true);
        }
    }

    public void MoveParentLeft()
    {
        if(!isAnimationStopped)
        {
            float X = catParent.transform.position.x;
            float Y = catParent.transform.position.y;

            float newX;

            if (X - jump_lenght > leftBorder)
            {
                newX = X - jump_lenght;
            }
            else
            {
                newX = game.getLeftColoumnPos(false);
            }

            catParent.transform.position = new Vector3(newX, Y, 0);
            DrawPoints.increasePoints();

            MoveFieldDown = true;
        }
    }

    public void MoveParentRight()
    {
        if(!isAnimationStopped)
        {
            float X = catParent.transform.position.x;
            float Y = catParent.transform.position.y;

            float newX;


            if (X + jump_lenght < rightBorder)
            {
                newX = X + jump_lenght;
            }
            else
            {
                newX = game.getLeftColoumnPos(true);
            }

            catParent.transform.position = new Vector3(newX, Y, 0);
            DrawPoints.increasePoints();

            MoveFieldDown = true;
        }
    }

    public void MoveParentDown()
    {
        if(!isAnimationStopped)
        {
            catParent.transform.position = new Vector3(catParent.transform.position.x, catParent.transform.position.y - SceneChanger.sink_height, 0);
            Game.Height -= SceneChanger.sink_height;
        }
    }
    public void CollisionTrigger(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bottom"))
        {
            SceneChanger.Stop();
            endGame?.Invoke();
        }

        if(collision.gameObject.CompareTag("Cloud") && hasNotCollided)
        {
            catAnimator.SetBool("Fall", true);
            DrawPoints.decreasePoints();
            hasNotCollided = false;
        }
    }

    public void Fall()
    {
        Vector3 v = catParent.transform.position;
        catParent.transform.position = new Vector3(v.x, 0, v.z);
        catAnimator.SetBool("Stay", false);
    }
}
