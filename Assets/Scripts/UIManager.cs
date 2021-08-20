using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public Text score, life, level;
    
    void Awake()
    {
        instance = this;
    }

    public void UpdateUI()
    {
      score.text = "Score: " +GameManager.intance.ReadScore();
      life.text = "Lifes: " + GameManager.intance.ReadLifes();
      level.text = "Level: " + GameManager.intance.ReadLevel();

    }

}
