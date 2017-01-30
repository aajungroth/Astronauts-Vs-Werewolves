using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TurnController : MonoBehaviour {

    public Queue<GameObject> turn = new Queue<GameObject>();
    public Grid_Manager grid_Manager;
    public List<GameObject> humanTeam = new List<GameObject>();
    public List<GameObject> wolfTeam = new List<GameObject>();
    public GameObject[] checker;
    public int hSpot; // keeping track of spot in human team list for the turn searches
    public int wSpot; // same for wolf
    public bool hTurn = true;

    private static TurnController tcinstance;

    public static TurnController TCInstance
    {
        get
        {
            if (tcinstance == null)
            {
                tcinstance = FindObjectOfType<TurnController>();
            }
            if (FindObjectsOfType<TurnController>().Length > 1) //Making sure its a singleton
            {
                Debug.LogError("More than one Turn Controller ERROR");
            }
            return tcinstance;
        }
    }

    // Use this for initialization
    void Start () {
        GameController.Instance.subtoEventsList(this);
        Debug.Log("TC Started");
        hSpot = 0;
        wSpot = 0;

        /*while ((hSpot < humanTeam.Count) || (wSpot < wolfTeam.Count)) //initial set up of turn que, just flips back and forth
        {
            if (hTurn == true && (hSpot < humanTeam.Count))
            {
                turn.Enqueue(humanTeam[hSpot]);
                hSpot++;
            }
            else if (hTurn == false && (wSpot < wolfTeam.Count))
            {
                turn.Enqueue(wolfTeam[wSpot]);
                wSpot++;
            }
            hTurn = !hTurn;

        }*/

        turn.Enqueue(grid_Manager.human3);
        turn.Enqueue(grid_Manager.wolf1);
        turn.Enqueue(grid_Manager.human2);
        turn.Enqueue(grid_Manager.human1);

        hTurn = true;
        Debug.Log("Que size");
        Debug.Log(turn.Count);
        GameController.Instance.pHero = turnPicker();
	}


    public GameObject turnPicker() // is supposed to return the object whose turn it is
    {
        Debug.Log("IN TURNPICKER");
        while(turn.Peek().GetComponent<Character>().alive == false)
        {
            turn.Dequeue();
        }
        GameObject temp = turn.Dequeue();
        turn.Enqueue(temp);
        checker = turn.ToArray();
        Debug.Log("Switched team");
        
        return temp;
    }


 
 
    
    /*public void unsubFromQ(GameObject charac ) // allows characters to get out of q 
    {
        while (turn.Contains(charac))
        {
            List<GameObject> temp = turn.ToList();
            temp.Remove(charac);
            turn.Clear();
            turn = new Queue<GameObject>(temp);
        }
    }*/

    // Update is called once per frame
}
