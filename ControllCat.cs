using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class ControllCat : MonoBehaviour
{
    public Animator catAnimator;
    
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
}
