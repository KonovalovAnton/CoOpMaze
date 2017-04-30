using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReplayComponent : MonoBehaviour {

    float t = 10;
    Vector3 prevPos;
    Vector3 targetPos;
    public Vector3 TargetPos
    {
        get
        {
            return targetPos;
        }
        set
        {
            prevPos = new Vector3( transform.position.x, transform.position.y, transform.position.z);
            targetPos = value;
            t = 0;
        }
    }
	
	void Update () {
        if (t < 0.1f)
        {
            transform.position = Vector3.Lerp(prevPos, targetPos, t / 0.1f);
            t += Time.deltaTime;
        }
	}
}
