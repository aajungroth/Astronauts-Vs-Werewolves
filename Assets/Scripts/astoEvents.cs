using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class astoEvents : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GameController.Instance.pHero = gameObject;
       // GameController.Instance.subtoEventsList(this);
	}

    void OnDestroy()
    {
       // GameController.Instance.unsubFromEventsList(this);
    }
    // Update is called once per frame
    void astroEventUpdate(){
        Debug.Log("called astorEventUpdate");
	}
}
