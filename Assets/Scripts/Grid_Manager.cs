using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid_Manager : MonoBehaviour {

    //The controller for the game (AAJ)
    private GameObject gameController;

    //The turn controller (AAJ)
    private GameObject turnController;

    //The height of the grid (AAJ)
    public int grid_height = 8;

    //The width of the grid (AAJ)
    public int grid_width = 8;

    //The double list of all grid spaces (AAJ)
    public GameObject[][] grid_list;

    //The basic instance of a grid space (AAJ)
    public GameObject grid_space_prefab;

    //Flips which  color the grid spaces are spawned as (AAJ)
    private bool flipColor = true;

    //Start of path coordinates (AAJ)
    public int start_x_coordinate = 0;
    public int start_z_coordinate = 0;

    //The tokens (AAJ)
    public GameObject human1;
    public GameObject human2;
    public GameObject human3;
    public GameObject wolf1;

    // Use this for initialization
    void Awake ()
    {
        //Gets the game controller (AAJ)
        gameController = GameObject.FindGameObjectWithTag("GameController");

        //Gets the turn controller (AAJ)
        turnController = GameObject.FindGameObjectWithTag("Turn");

        //Initializes the grid list (AAJ)
        grid_list = new GameObject[grid_height][];
        
        for(int i = 0; i < grid_height; i++)
        {
            grid_list[i] = new GameObject[grid_width];
        }//for

        //Spawns in the grid (AAJ)
        for (int i = 0; i < grid_height; i++)
        {
            for (int j = 0; j < grid_width; j++)
            {
                //The transform of the current grid space being instantiated (AAJ)
                Vector3 tempPositionVector = transform.localPosition;
                tempPositionVector.x += grid_space_prefab.transform.localScale.x * j;
                tempPositionVector.z += grid_space_prefab.transform.localScale.z * i;

                //Creates the new grid space (AAJ)
                grid_list[i][j] = Instantiate(grid_space_prefab, tempPositionVector, Quaternion.identity);

                //Parents the grid space to the grid (AAJ)
                grid_list[i][j].transform.parent = transform;

                //Gives the grid space its coordinates (AAJ)
                grid_list[i][j].GetComponent<Grid_Space_Manager>().x_coordinate = i;
                grid_list[i][j].GetComponent<Grid_Space_Manager>().z_coordinate = j;

                //Alternates the color of the grid spaces (AAJ)
                if (flipColor)
                {
                    grid_list[i][j].GetComponent<Renderer>().material.color = Color.gray;
                    flipColor = false;
                }//if
                else
                {
                    flipColor = true;
                }//else
            }//for

            //Alters the flip pattern for color on each row of the grid if the grid width is even (AAJ)
            if(grid_width % 2 == 0)
            {
                if(flipColor)
                {
                    flipColor = false;
                }//if
                else
                {
                    flipColor = true;
                }//else
            }//if

        }//for

        //Places the humans on the grid (AAJ)
        human1 = GameObject.Find("Human 1");
        Vector3 tempVector = Vector3.zero;
        tempVector.y = human1.transform.localPosition.y;
        human1.transform.parent = grid_list[0][4].transform;
        human1.transform.localPosition = tempVector;

        human2 = GameObject.Find("Human 2");
        tempVector = Vector3.zero;
        tempVector.y = human2.transform.localPosition.y;
        human2.transform.parent = grid_list[0][5].transform;
        human2.transform.localPosition = tempVector;

        human3 = GameObject.Find("Human 3");
        tempVector = Vector3.zero;
        tempVector.y = human3.transform.localPosition.y;
        human3.transform.parent = grid_list[0][6].transform;
        human3.transform.localPosition = tempVector;

        //Places the wolves on the grid (AAJ)
        wolf1 = GameObject.FindGameObjectWithTag("wolf");
        tempVector = Vector3.zero;
        tempVector.y = wolf1.transform.localPosition.y;
        wolf1.transform.parent = grid_list[7][4].transform;
        wolf1.transform.localPosition = tempVector;
    }
	
	// Update is called once per frame
	void Update () {

    }

    //Marks the squares adjacent to a location as movable (AAJ)
    public void markAdjacent(int z, int x)
    {
        //Resets all spaces to not movable before marking new ones (AAJ)
        resetCanMoveTo();

        //Test Print
        /*
        Debug.Log("marking");
        Debug.Log(z);
        Debug.Log(x);
        */

        if((z + 1 < grid_height) && (!grid_list[z + 1][x].GetComponent<Grid_Space_Manager>().is_ocuppied) &&
            (!grid_list[z + 1][x].GetComponent<Grid_Space_Manager>().part_of_path))
        {
            grid_list[z + 1][x].GetComponent<Grid_Space_Manager>().can_move_to = true;
        }//if

        if ((z - 1 >= 0) && (!grid_list[z - 1][x].GetComponent<Grid_Space_Manager>().is_ocuppied) &&
            (!grid_list[z - 1][x].GetComponent<Grid_Space_Manager>().part_of_path))
        {
            grid_list[z - 1][x].GetComponent<Grid_Space_Manager>().can_move_to = true;
        }//if

        if ((x + 1 < grid_width) && (!grid_list[z][x + 1].GetComponent<Grid_Space_Manager>().is_ocuppied) &&
            (!grid_list[z][x + 1].GetComponent<Grid_Space_Manager>().part_of_path))
        {
            grid_list[z][x + 1].GetComponent<Grid_Space_Manager>().can_move_to = true;
        }//if

        if ((x - 1 >= 0) && (!grid_list[z][x - 1].GetComponent<Grid_Space_Manager>().is_ocuppied) &&
            (!grid_list[z][x - 1].GetComponent<Grid_Space_Manager>().part_of_path))
        {
            grid_list[z][x - 1].GetComponent<Grid_Space_Manager>().can_move_to = true;
        }//if
    }//markAdjacent

    //Limits the squares that can be targeted (AAJ)
    public void canBeTargeted(int z, int x)
    {
        //Resets all spaces that are targetable (AAJ)
        resetTargetable();

        for (int i = 1; i <= gameController.GetComponent<GameController>().pHero.GetComponent<Character>().atkRange; i++)
        {
            if (z + i < grid_height)
            {
                grid_list[z + i][x].GetComponent<Grid_Space_Target>().targetable = true;
            }//if

            if (z - i >= 0)
            {
                grid_list[z - i][x].GetComponent<Grid_Space_Target>().targetable = true;
            }//if

            if (x + i < grid_width)
            {
                grid_list[z][x + i].GetComponent<Grid_Space_Target>().targetable = true;
            }//if

            if (x - i >= 0)
            {
                grid_list[z][x - i].GetComponent<Grid_Space_Target>().targetable = true;
            }//if
        }//for
    }//markAdjacent

    /// <summary>
    /// Resets all spaces to not movable (AAJ)
    /// </summary>
    public void resetCanMoveTo()
    {
        for (int i = 0; i < grid_height; i++)
        {
            for (int j = 0; j < grid_width; j++)
            {
                grid_list[i][j].GetComponent<Grid_Space_Manager>().can_move_to = false;
            }//for
        }//for
    }//resetCanMoveTo()

    /// <summary>
    /// Resets the grid's path (AAJ)
    /// </summary>
    public void resetPartOfPath()
    {
        for (int i = 0; i < grid_height; i++)
        {
            for (int j = 0; j < grid_width; j++)
            {
                grid_list[i][j].GetComponent<Grid_Space_Manager>().part_of_path = false;
            }//for
        }//for
    }//resetPartOfPath

    /// <summary>
    /// Resets all spaces to not targetable (AAJ)
    /// </summary>
    public void resetTargetable()
    {
        for (int i = 0; i < grid_height; i++)
        {
            for (int j = 0; j < grid_width; j++)
            {
                grid_list[i][j].GetComponent<Grid_Space_Target>().targetable = false;
            }//for
        }//for
    }//resetCanMoveTo()
}
