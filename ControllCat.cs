using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class ControllCat : MonoBehaviour
{
    public Animator catAnimator;
    public GameObject catParent;
    public float jump_height;
    public float jump_lenght;
    public static float Jump_Height;
    public float jump_height_shift;
    bool isAnimationStopped = false;
    bool hasNotCollided = true;

    private void PauseAnimation()
    {
        if (catAnimator != null)
        {
            isAnimationStopped = !isAnimationStopped;
            catAnimator.enabled = !catAnimator.enabled;
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
        SceneChanger.restart += restart;
        restart();
        catAnimator.SetBool("Stay", true);
    }

    // Update is called once per frame
    public void MoveDown()
    {
        catAnimator.SetBool("Left",false);
        catAnimator.SetBool("Right",false);
        //Debug.Log("Down");
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
            catParent.transform.position = new Vector3(catParent.transform.position.x - jump_lenght, catParent.transform.position.y + jump_height - jump_height_shift, 0);
            //Debug.LogWarning($"Before: X: {X}, Y: {Y}\n" + $"After: X: {catParent.transform.position.x},y: {catParent.transform.position.y}, Z: {catParent.transform.position.z}");
            DrawPoints.increasePoints();
        }
    }

    public void MoveParentRight()
    {
        if(!isAnimationStopped)
        {
            float X = catParent.transform.position.x;
            float Y = catParent.transform.position.y;
            catParent.transform.position = new Vector3(catParent.transform.position.x + jump_lenght, catParent.transform.position.y + jump_height - jump_height_shift, 0);
            //Debug.LogWarning($"Before: X: {X}, Y: {Y}\n" + $"After: X: {catParent.transform.position.x},y: {catParent.transform.position.y}, Z: {catParent.transform.position.z}");
            DrawPoints.increasePoints();
        }
    }

    public void MoveParentDown()
    {
        if(!isAnimationStopped)
        {
            catParent.transform.position = new Vector3(catParent.transform.position.x, catParent.transform.position.y - SceneChanger.sink_height, 0);
            Game.Height -= SceneChanger.sink_height;
            //Debug.LogWarning($"X: {catParent.transform.position.x},y: {catParent.transform.position.y}, Z: {catParent.transform.position.z}\n");
        }
    }
    public void CollisionTrigger(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bottom"))
        {
            SceneChanger.Stop();
            //Debug.Log("Has reached the bottom");
        }

        if(collision.gameObject.CompareTag("Cloud") && hasNotCollided)
        {
            catAnimator.SetBool("Fall", true);
            DrawPoints.decreasePoints();
            hasNotCollided = false;
            //Debug.Log("Falling");
        }
    }

    public void Fall()
    {
        Vector3 v = catParent.transform.position;
        catParent.transform.position = new Vector3(v.x, 0, v.z);
        catAnimator.SetBool("Stay", false);
    }
}
