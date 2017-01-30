using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    bool done;
    public GameObject pHero;

    public List<MonoBehaviour> subdEvents = new List<MonoBehaviour>();

    private static GameController instance;

    public bool moveState;
    public bool attackState;

    //Makes the move run one at a time
    private bool runOnce = true;

    public static GameController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameController>();
            }
            if (FindObjectsOfType<GameController>().Length > 1) //Making sure its a singleton
            {
                Debug.LogError("More than one Game Controller ERROR");
            }
            return instance;
        }
    }

    // Use this for initialization
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        moveState = true;
        attackState = false;
        done = false;
     
    }

    public void subtoEventsList(MonoBehaviour pScript)
    {
        subdEvents.Add(pScript);
    }

    public void unsubFromEventsList(MonoBehaviour pScript)
    {
      
            subdEvents.Remove(pScript);
       

    }
    

    void Update()
    {   
        if (moveState == true && runOnce == true)
        {
            Debug.Log("in GC PPE and got the invoke to work");
            runOnce = false;
        }

        if (attackState == true)
        {
            runOnce = true;
        }

        if (!moveState && !attackState)
        {
            pHero = TurnController.TCInstance.turnPicker();
            moveState = true;
        }
    }
}
