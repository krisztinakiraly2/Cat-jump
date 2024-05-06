using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Cat : MonoBehaviour
{
    private Rigidbody2D rb;
    public ControllCat controllCat;
    private SpriteRenderer SpriteRenderer;
    private BoxCollider2D boxCollider;

    Vector3 prev;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        SpriteRenderer = rb.GetComponent<SpriteRenderer>();
        boxCollider = rb.GetComponent<BoxCollider2D>();
        controllCat.enabled = true;
        controllCat.catAnimator.SetBool("Start", true);
        prev = controllCat.catParent.transform.position; 
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            controllCat.Jump(true);
        else
            if (Input.GetKeyDown(KeyCode.RightArrow))
            controllCat.Jump(false);
            else
                if (!SceneChanger.menu)
                    controllCat.MoveDown();

        if (SceneChanger.menu)
            controllCat.catAnimator.SetBool("Start", false);

        if (prev != controllCat.catParent.transform.position)
        {
            prev = controllCat.catParent.transform.position;
            //Debug.Log(prev);
        }

    }

    void MoveDown()
    {
        if (!SceneChanger.menu)
            controllCat.MoveParentDown();
    }

    void JumpLeft()
    {
        controllCat.MoveParentLeft();
    }

    void JumpRight()
    {
        controllCat.MoveParentRight();
    }
    
    void Fall()
    {
        controllCat.Fall();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        controllCat.CollisionTrigger(collision);
    }

    void resetFall()
    {
        controllCat.catAnimator.SetBool("Fall", false);
    }
}
