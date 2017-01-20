using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour, IPunObservable {

    public bool Activated { get { return active; } }

    [SerializeField] bool active = false;
    [SerializeField] bool stay = false;

    PhotonView pv;

    void Start()
    {
        pv = GetComponent<PhotonView>();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(active);
        }
        else
        {
            active = (bool)stream.ReceiveNext();
        }
    }

    void OnTriggerEnter(Collider col)
    {
        RobotController r = col.GetComponent<RobotController>();
        if(r!= null && pv.isMine)
        {
            active = true;   
        }
    }

    void OnTriggerExit(Collider col)
    {
        RobotController r = col.GetComponent<RobotController>();
        if (r != null && stay && pv.isMine)
        {
            active = false;
        }
    }
}
