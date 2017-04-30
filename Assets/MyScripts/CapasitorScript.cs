using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapasitorScript : MonoBehaviour, IActivate {

    public static Dictionary<int, CapasitorScript> capacitors = new Dictionary<int, CapasitorScript>();

    [SerializeField]
    public float activateAmount;
    [SerializeField]
    public float maxAmount;
    [SerializeField]
    public float charge;
    [SerializeField]
    float dischargeSpeed;

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
        else if(charge > maxAmount)
        {
            charge = maxAmount;
        }
    }

    public void Log()
    {
        CapacitorCharge p = new CapacitorCharge()
        {
            id = pv.viewID,
            charge = charge,
        };
        string log = JsonUtility.ToJson(p);
        GlobalLogSaver.Instance.AddLogString(log);
    }

    public bool IsActivated()
    {
        return charge - activateAmount > 0;
    }

    float cache;
    public void Update()
    {
        if(pv.isMine)
        {
            pv.RPC("ChargeCapasitor", PhotonTargets.AllViaServer, -dischargeSpeed * Time.deltaTime);
            currentTime = PhotonNetwork.time;
        }

        if(PhotonNetwork.connected && Mathf.Abs(cache - charge) > 0.1f)
        {
            cache = charge;
            Log();
        }
    }

    void Start ()
    {
        pv = GetComponent<PhotonView>();
        currentTime = PhotonNetwork.time;
        capacitors.Add(pv.viewID, this);
    }
}

class CapacitorCharge : LogObject
{
    public int id;
    public float charge;
    public CapacitorCharge() : base("CapacitorCharge") { }
}
