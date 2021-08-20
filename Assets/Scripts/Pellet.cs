using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pellet : MonoBehaviour
{
    // Start is called before the first frame update
    int score = 3;

    void Start()
    {
        GameManager.intance.AddPellet();
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "Chomp")
        {
            GameManager.intance.ReducePellet(score);
            Destroy(gameObject);
        }
    }

}
