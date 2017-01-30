using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid_Space_Target : MonoBehaviour {

    //The controller for the game (AAJ)
    private GameObject gameController;

    //The space can be targeted (AAJ)
    public bool targetable = false;
    
    //The space is being targeted (AAJ)
    public bool targeted = true;

    //Saves the cubes original color (AAJ)
    private Color original_color;

    //Only starts the targeting once per turn (AAJ)
    private bool start_of_turn = true;

    // Use this for initialization
    void Start ()
    {
        //Gets the game controller (AAJ)
        gameController = GameObject.FindGameObjectWithTag("GameController");

        //Saves the original color (AAJ)
        original_color = GetComponent<Renderer>().material.color;
    }
	
	// Update is called once per frame
	void Update ()
    {
        //Can only be targeted during the attack state if their is a token on this space (AAJ)
        if ((transform.childCount >= 1) && (gameController.GetComponent<GameController>().attackState) && (gameController.GetComponent<GameController>().pHero != null) &&
            (transform.GetChild(0).name == gameController.GetComponent<GameController>().pHero.name))
        {
            //Only marks things as targetable at the start of the attack phase (AAJ)
            if (start_of_turn)
            {
                transform.parent.GetComponent<Grid_Manager>().canBeTargeted(GetComponent<Grid_Space_Manager>().x_coordinate, GetComponent<Grid_Space_Manager>().z_coordinate);

                //Prevents the token from targeting itself (AAJ)
                targetable = false;
                targeted = false;
            }//if
        }//if

        //Only changes the targeting and colors during the attack state (AAJ)
        if (gameController.GetComponent<GameController>().attackState)
        {
            if(targetable && transform.childCount >= 1)
            {
                targeted = true;
            }//if
            else
            {
                targeted = false;
            }//else

            if (targeted)
            {
                GetComponent<Renderer>().material.color = Color.red;
            }//if
            else
            {
                GetComponent<Renderer>().material.color = original_color;
            }//else
        }//if
    }

    /// <summary>
    /// Detects a click on the square when attacking a token (AAJ)
    /// </summary>
    void OnMouseDown()
    {
        //Updates the start x and z coordinates (AAJ)
        int start_x_coordinate = transform.parent.GetComponent<Grid_Manager>().start_x_coordinate;
        int start_z_coordinate = transform.parent.GetComponent<Grid_Manager>().start_z_coordinate;

        if ((transform.childCount >= 1) && (gameController.GetComponent<GameController>().pHero != null) &&
            (transform.GetChild(0).name == gameController.GetComponent<GameController>().pHero.name))
        {
            //Does nothing (AAJ)
        }
        else if (targeted)
        {
            //Attacks the target (AAJ)
            transform.GetChild(0).GetComponent<Character>().attacked(gameController.GetComponent<GameController>().pHero.GetComponent<Character>().atk);

            //Ends the attack state after attacking (AAJ)
            transform.parent.GetComponent<Grid_Manager>().resetTargetable();
            gameController.GetComponent<GameController>().attackState = false;
        }//else if()
        else
        {
            //Ends the attack state without attacking (AAJ)
            transform.parent.GetComponent<Grid_Manager>().resetTargetable();
            gameController.GetComponent<GameController>().attackState = false;
        }//
    }
}
