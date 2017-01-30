using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid_Space_Manager : MonoBehaviour {

    //The controller for the game (AAJ)
    private GameObject gameController;

    //The space is either occupied or not (AAJ)
    public bool is_ocuppied = false;

    //The space can be moved to (AAJ)
    public bool can_move_to = false;

    //The space is being used as part of the path  (AAJ)
    public bool part_of_path = false;
    
    //Grid coordinates of this space (AAJ)
    public int x_coordinate = 0;
    public int z_coordinate = 0;

    //Saves the cubes original color (AAJ)
    private Color original_color;

    //Only starts the move once per turn (AAJ)
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
        //Marks the space as occupied if a token in it and labels adjacent squares as movable (AAJ)
        if ((transform.childCount >= 1) && (gameController.GetComponent<GameController>().moveState) && (gameController.GetComponent<GameController>().pHero != null) &&
            (transform.GetChild(0).name == gameController.GetComponent<GameController>().pHero.name))
        {
            //Stats the move at the beginning of the turn (AAJ)
            if(start_of_turn)
            {
                is_ocuppied = true;

                //Test Print
                /*
                Debug.Log(x_coordinate);
                Debug.Log(z_coordinate);
                Debug.Log(transform.GetChild(0).name);
                Debug.Log("Hero is");
                Debug.Log(gameController.GetComponent<GameController>().pHero.name);
                */

                transform.parent.GetComponent<Grid_Manager>().markAdjacent(x_coordinate, z_coordinate);

                //Saves the start coordinates on the path (AAJ)
                transform.parent.GetComponent<Grid_Manager>().start_x_coordinate = x_coordinate;
                transform.parent.GetComponent<Grid_Manager>().start_z_coordinate = z_coordinate;
                
                start_of_turn = false;
            }//if
        }//if
        else if(transform.childCount >= 1)
        {
            is_ocuppied = true;
        }//else if
        else
        {
            is_ocuppied = false;
        }

        //Marks the space with the correct path color during the move state (AAJ)
        if (gameController.GetComponent<GameController>().moveState)
        {
            if (part_of_path)
            {
                GetComponent<Renderer>().material.color = Color.green;
            }
            else if (can_move_to)
            {
                GetComponent<Renderer>().material.color = Color.blue;
            }
            else
            {
                GetComponent<Renderer>().material.color = original_color;
            }//else
        }//if
	}

    /// <summary>
    /// Detects a click on the square when moving a token (AAJ)
    /// </summary>
    void OnMouseDown()
    {
        //Updates the start x and z coordinates (AAJ)
        int start_x_coordinate = transform.parent.GetComponent<Grid_Manager>().start_x_coordinate;
        int start_z_coordinate = transform.parent.GetComponent<Grid_Manager>().start_z_coordinate;

        //Mark unoccuppied neighboring spaces as movable (AAJ)
        if (!is_ocuppied && can_move_to && !part_of_path)
        {
            transform.parent.GetComponent<Grid_Manager>().markAdjacent(x_coordinate,z_coordinate);

            //The clicked square becomes part of the path (AAJ)
            part_of_path = true;
            
        }//if
        else if(part_of_path)
        {
            //Updates the token position and clears the path indicators (AAJ)
            Vector3 tempVector = Vector3.zero;
            tempVector.y = transform.parent.GetComponent<Grid_Manager>().grid_list[start_x_coordinate][start_z_coordinate].transform.GetChild(0).transform.localPosition.y;
            transform.parent.GetComponent<Grid_Manager>().grid_list[start_x_coordinate][start_z_coordinate].transform.GetChild(0).transform.parent = transform;
            transform.GetChild(0).transform.localPosition = tempVector;

            transform.parent.GetComponent<Grid_Manager>().resetPartOfPath();
            transform.parent.GetComponent<Grid_Manager>().resetCanMoveTo();

            //Resets start of turn to true (AAJ)
            start_of_turn = true;

            //Resets the start coordinates (AAJ)
            transform.parent.GetComponent<Grid_Manager>().start_x_coordinate = 0;
            transform.parent.GetComponent<Grid_Manager>().start_z_coordinate = 0;

            //Changes the game state from the move state to the attack state (AAJ)
            gameController.GetComponent<GameController>().moveState = false;
            gameController.GetComponent<GameController>().attackState = true;

        }//else
        else if(x_coordinate == start_x_coordinate && z_coordinate == start_z_coordinate)
        {
            //Undoes the path (AAJ)
            transform.parent.GetComponent<Grid_Manager>().resetPartOfPath();
            transform.parent.GetComponent<Grid_Manager>().markAdjacent(start_x_coordinate, start_z_coordinate);
        }//else if()
    }
}
