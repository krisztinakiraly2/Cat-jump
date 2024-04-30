using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : MonoBehaviour
{
    private Rigidbody2D rb;
    public int jump = 130;
    public ControllCat controllCat;
    private SpriteRenderer SpriteRenderer;
    private BoxCollider2D boxCollider;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        SpriteRenderer = rb.GetComponent<SpriteRenderer>();
        boxCollider = rb.GetComponent<BoxCollider2D>();
        controllCat.enabled = true;
        controllCat.catAnimator.SetBool("Start", true);
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
                    MoveDown();

        if(SceneChanger.menu)
            controllCat.catAnimator.SetBool("Start", false);

    }

    void MoveDown()
    {
        float y = Input.GetAxis("Vertical");
        rb.velocity = new Vector2(rb.velocityX, y);
        controllCat.MoveDown();
    }
}
