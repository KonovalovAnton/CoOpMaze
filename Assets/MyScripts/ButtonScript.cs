using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour, IPunObservable, IActivate {

    public static Dictionary<int, ButtonScript> buttons = new Dictionary<int, ButtonScript>();

    [SerializeField] bool active = false;
    [SerializeField] bool stay = false;

    PhotonView pv;

    [SerializeField] Transform button_model;

    void Start()
    {
        pv = GetComponent<PhotonView>();
        buttons.Add(pv.viewID, this);
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

    bool cache = false;
    private void Update()
    {
        if(PhotonNetwork.connected && active != cache)
        {
            Log();
            cache = active;
        }
    }

    public void SetActive(bool act)
    {
        active = act;
    }

    public bool IsActivated()
    {
        return active;
    }

    public void Log()
    {
        ButtonActivation p = new ButtonActivation()
        {
            id = pv.viewID,
            active = IsActivated(),
        };
        string log = JsonUtility.ToJson(p);
        GlobalLogSaver.Instance.AddLogString(log);
    }
}

public interface IActivate
{
    bool IsActivated();
}

class ButtonActivation : LogObject
{
    public int id;
    public bool active;
    public ButtonActivation() : base("ButtonActivation") { }
}