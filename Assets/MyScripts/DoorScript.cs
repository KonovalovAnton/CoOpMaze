using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class DoorScript : MonoBehaviour {

    static float delta = 0.1f;

    public GameObject[] activators;
    [SerializeField] Transform open;
    [SerializeField] Transform closed;
    [SerializeField] float speed;
	
    bool ToOpen()
    {
        if(activators == null || activators.Length == 0)
        {
            return true;
        }

        foreach(GameObject go in activators)
        {
            IActivate a = go.GetComponent<IActivate>();
            if(!a.IsActivated())
            {
                return false;
            }
        }

        return true;
    }

	// Update is called once per frame
	void Update () {
		if(ToOpen())
        {
            if(Vector3.Distance(transform.position, open.position) > delta)
            {
                transform.Translate((open.position - transform.position).normalized * speed * Time.deltaTime);
            }
        }
        else
        {
            if (Vector3.Distance(transform.position, closed.position) > delta)
            {
                transform.Translate((closed.position - transform.position).normalized * speed * Time.deltaTime);
            }
        }
	}
}
