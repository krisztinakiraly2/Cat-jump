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

    private void Start()
    {
        Jump_Height = jump_height;
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
        float X = catParent.transform.position.x;
        float Y = catParent.transform.position.y;
        catParent.transform.position = new Vector3(catParent.transform.position.x - jump_lenght, catParent.transform.position.y + jump_height- jump_height_shift, 0);
        //Debug.LogWarning($"Before: X: {X}, Y: {Y}\n" + $"After: X: {catParent.transform.position.x},y: {catParent.transform.position.y}, Z: {catParent.transform.position.z}");
    }

    public void MoveParentRight()
    {
        float X = catParent.transform.position.x;
        float Y = catParent.transform.position.y;
        catParent.transform.position = new Vector3(catParent.transform.position.x + jump_lenght, catParent.transform.position.y + jump_height- jump_height_shift, 0);
        //Debug.LogWarning($"Before: X: {X}, Y: {Y}\n" + $"After: X: {catParent.transform.position.x},y: {catParent.transform.position.y}, Z: {catParent.transform.position.z}");
    }

    public void MoveParentDown()
    {
        catParent.transform.position = new Vector3(catParent.transform.position.x, catParent.transform.position.y - SceneChanger.sink_height, 0);
        Game.Height -= SceneChanger.sink_height;
        //Debug.LogWarning($"X: {catParent.transform.position.x},y: {catParent.transform.position.y}, Z: {catParent.transform.position.z}\n");
    }
}
