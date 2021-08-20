using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager intance;
    private int pelletAmount = 0;
    public int score;
    private static int cur_level = 1;
    private static int lifes = 3;

    public List<PCTetse> ghostList = new List<PCTetse>();
    public Pacman pacman;

    static bool hasLost;
    void Awake()
    {
        intance = this;
       // Debug.Log("pacman");
    }

    void Start()
    {
        UIManager.instance.UpdateUI();

        if (hasLost)
        {
            score = 0;
            lifes = 3;
            cur_level = 1;
            hasLost = false;
        }

    }

    public void AddPellet()
    {
        pelletAmount++;
    }

    public void AddScore(int amount)
    {
        score += amount;
        UIManager.instance.UpdateUI();
    }

    public void ReducePellet(int amount)
    {
        pelletAmount--;
        score += amount;
        UIManager.instance.UpdateUI();

        if (pelletAmount <= 0)
        {
            WinCondition();
        }        
    }

    public void PowerPelletColide(int amount)
    {
        foreach (PCTetse item in ghostList)
        {
            item.state = PCTetse.GhostStates.FRIGHTEND;
        }

        ReducePellet(amount);
    }

    public void GhostCollide()
    {
        foreach (PCTetse item in ghostList)
        {
            if(item.state != PCTetse.GhostStates.GHOST_EATEN || item.state != PCTetse.GhostStates.FRIGHTEND)
            {
                LoseLife();
            } else
            {
                item.state = PCTetse.GhostStates.GHOST_EATEN;
                score += 20;                
            }
           
        }
    }

    void WinCondition()
    {
        cur_level++;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoseLife()
    {
        lifes--;
        UIManager.instance.UpdateUI();
        if ( lifes == 0)
        {
            ScoreHolder.level = cur_level;
            ScoreHolder.score = score;
            hasLost = true;
            SceneManager.LoadScene("GameOver");
            return;
        }

        foreach (PCTetse item in ghostList)
        {
            item.Reset();
        }

        pacman.Reset();
    }

    public int  ReadScore() { return score; }
    public  int ReadLevel() { return cur_level; }
    public  int ReadLifes() { return lifes; }
}
