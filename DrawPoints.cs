using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DrawPoints : MonoBehaviour
{
    public TextMeshPro Score;
    public TextMeshPro BestScore;
    static int points = 0;
    static int bestScore = 0;

    public void Start()
    {
        points = 0;
    }

    public static void increasePoints()
    {
        ++points;
        if(points >= bestScore)
            bestScore = points;
    }

    public static void decreasePoints()
    {
        if (points == bestScore)
            --bestScore;

        --points;
    }

    void Update()
    {
        if(Score != null) Score.text = points.ToString();
        if(BestScore != null) BestScore.text = bestScore.ToString();
    }
}
