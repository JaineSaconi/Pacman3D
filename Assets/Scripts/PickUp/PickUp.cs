using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public float destroyTime = 10;
    public int scoreAmount;

    void Start()
    {
        Destroy(gameObject, destroyTime);
    }

    void OnCollisionEnter(Collision col)
    {

        if (col.gameObject.name == "Chomp")
        {
            Destroy(gameObject);
            GameManager.intance.AddScore(scoreAmount);
        }
    }
}
