using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerPellets : MonoBehaviour
{
    int score = 6;

    void Start()
    {
        GameManager.intance.AddPellet();
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "Chomp")
        {
            GameManager.intance.PowerPelletColide(score);
            Destroy(gameObject);
        }
    }
}
