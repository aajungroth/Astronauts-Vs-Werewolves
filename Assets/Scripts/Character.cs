using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

    public int health;
    public bool alive = true;
    public int atk;
    public int atkRange;
    public int moveRange;

	// Use this for initialization
	void Start () {
        health = 10;
        Debug.Log("Character Spawns");
        if (gameObject.tag == "human")
        {
            health = 50;
            atk = 10;
            moveRange = 4;
            atkRange = 5;
            TurnController.TCInstance.humanTeam.Add(gameObject);
        }
        else if (gameObject.tag == "wolf")
        {
            health = 100;
            atk = 50;
            moveRange = 10;
            atkRange = 1;
            TurnController.TCInstance.wolfTeam.Add(gameObject);
        }
        else
        {
            Debug.Log("ERROR, in Character there is no tag on this one");
        }
	}

    public void attacked(int damage)
    {
            health = health - damage;
    }

    void Update()
    {
        if(health <= 0)
        {
            alive = false;

            //Marks the token as dead (AAJ)
            GetComponent<Renderer>().material.color = Color.red;
        }
    }
   /* public static explicit operator Character(GameObject v)
    {
        throw new NotImplementedException();
    }*/

    // Update is called once per frame
}
