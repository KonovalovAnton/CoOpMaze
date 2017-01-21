using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapasitorScript : MonoBehaviour, IActivate {

    [SerializeField] float activateAmount;
    [SerializeField] float maxAmount;
    [SerializeField] float charge;
    [SerializeField] float dischargeSpeed;

    double currentTime;
    PhotonView pv;

    public void Charge(float deltaCharge)
    {
        pv.RPC("ChargeCapasitor", PhotonTargets.AllViaServer, deltaCharge);
    }

    [PunRPC]
    public void ChargeCapasitor(float deltaCharge)
    {
        charge += deltaCharge;
        if(charge < 0)
        {
            charge = 0;
        }

        if(charge > maxAmount)
        {
            charge = maxAmount;
        }
    }

    public bool IsActivated()
    {
        return charge - activateAmount > 0;
    }

    public void Update()
    {
        if(pv.isMine)
        {
            pv.RPC("ChargeCapasitor", PhotonTargets.AllViaServer, -dischargeSpeed * Time.deltaTime);
            currentTime = PhotonNetwork.time;
        }
    }

    void Start ()
    {
        pv = GetComponent<PhotonView>();
        currentTime = PhotonNetwork.time;
    }
}
