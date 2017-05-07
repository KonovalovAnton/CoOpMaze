using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {

    public GameObject game;
    public GameObject replay;

	// Use this for initialization
	void Start () {
        if (StartOptions.replay)
        {
            replay.SetActive(true);
        }
        else
        {
            game.SetActive(true); 
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
