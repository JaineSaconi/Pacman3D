using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReadScore : MonoBehaviour
{

    public Text higScore;
    public Text level;
    // Start is called before the first frame update
    void Start()
    {
        higScore.text = "Highscore: " + ScoreHolder.score;
        level.text = "Level: " + ScoreHolder.level;
    }
}
