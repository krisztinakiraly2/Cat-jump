using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlGame : MonoBehaviour
{
    public GameObject GameParent;
    public Animator GameAnimator;
    
    // Start is called before the first frame update
    void Start()
    {
        GameAnimator.SetBool("Start", true);
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneChanger.menu)
            GameAnimator.SetBool("Start", false);
    }

    public void MoveParentDown()
    {
        float X = GameParent.transform.position.x;
        float Y = GameParent.transform.position.y;
        GameParent.transform.position = new Vector3(GameParent.transform.position.x, GameParent.transform.position.y - SceneChanger.sink_height, 0);
        //Debug.LogWarning($"Before: X: {X}, Y: {Y}\n" + $"After: X: {GameParent.transform.position.x},y: {GameParent.transform.position.y}, Z: {GameParent.transform.position.z}");
    }

}
