using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour, IPunObservable, IActivate {
    
    [SerializeField] bool active = false;
    [SerializeField] bool stay = false;

    PhotonView pv;

    [SerializeField] Transform button_model;

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
        button_model.Translate(-transform.forward * .2f);
    }

    void OnTriggerExit(Collider col)
    {
        RobotController r = col.GetComponent<RobotController>();
        if (r != null && stay && pv.isMine)
        {
            active = false;
        }
        if (stay)
        {
            button_model.Translate(transform.forward * .2f);
        }
    }

    public bool IsActivated()
    {
        return active;
    }
}

public interface IActivate
{
    bool IsActivated();
}
