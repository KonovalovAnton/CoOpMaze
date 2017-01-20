using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapasitorScript : MonoBehaviour {

    [SerializeField] float charge = 0;
    PhotonView pv;

    public void Charge(float deltaCharge)
    {
        pv.RPC("ChargeCapasitor", PhotonTargets.AllViaServer, deltaCharge);
    }

    [PunRPC]
    public void ChargeCapasitor(float deltaCharge)
    {
        charge += deltaCharge;
    }

	// Use this for initialization
	void Start () {
        pv = GetComponent<PhotonView>();
	}
}
