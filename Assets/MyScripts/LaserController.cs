using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour, IPunObservable
{

    [SerializeField] GameObject laserR;
    [SerializeField] GameObject laserL;
    [SerializeField] float chargeSpeed = 1;

    PhotonView pv;

    LineRenderer lineR;
    LineRenderer lineL;

    RobotController robot;
    RaycastHit info;

    Vector3[] positionsR;
    Vector3[] positionsL;

    void Start () {
        lineR = laserR.GetComponent<LineRenderer>();
        lineL = laserL.GetComponent<LineRenderer>();

        positionsR = new Vector3[2];
        positionsL = new Vector3[2];

        pv = GetComponent<PhotonView>();
        robot = GetComponent<RobotController>();
    }

    bool shooting = false;
    bool pressed = false;

    private void EnableLaser(bool enable)
    {
        laserR.SetActive(enable);
        laserL.SetActive(enable);
        shooting = enable;
        if(pv.isMine)
        {
            robot.SwitchToShoot(enable);
        }
    }

	void Update () {
        if(pv.isMine)
        {
            pressed = Input.GetMouseButton(0);
            if (pressed)
            {
                RaycastCheck();
            }
        }
        HandleLasers();
           
	}

    void RaycastCheck()
    {
        if (Physics.Raycast(robot.ActiveCamera.transform.position, robot.ActiveCamera.transform.forward, out info))
        {
            positionsR[0] = laserR.transform.position;
            positionsR[1] = info.point;

            positionsL[0] = laserL.transform.position;
            positionsL[1] = info.point;
            CapasitorScript cap = info.transform.gameObject.GetComponent<CapasitorScript>();
            if(cap != null)
            {
                cap.Charge(chargeSpeed * Time.deltaTime);
            }
        }
    }

    Vector3 cacheTarget = Vector3.zero;
    void HandleLasers()
    {
        if (!shooting && pressed)
        {
            EnableLaser(true);
        }
        else if (shooting && !pressed)
        {
            EnableLaser(false);
            LaserVisuals lv = new LaserVisuals()
            {
                id = gameObject.GetInstanceID(),
                enable = false
            };
            string log = JsonUtility.ToJson(lv);
            GlobalLogSaver.Instance.AddLogString(log);
        }

        if (pressed)
        {
            lineR.SetPositions(positionsR);
            lineL.SetPositions(positionsL);
            if((positionsR[1] - cacheTarget).magnitude > 0.1f)
            {
                LaserVisuals lv = new LaserVisuals()
                {
                    id = gameObject.GetInstanceID(),
                    target = positionsR[1],
                    enable = true
                };
                string log = JsonUtility.ToJson(lv);
                GlobalLogSaver.Instance.AddLogString(log);
                cacheTarget = positionsR[1];
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(pressed);
            stream.SendNext(positionsR);
            stream.SendNext(positionsL);
        }
        else
        {
            pressed = (bool)stream.ReceiveNext();
            positionsR = (Vector3[])stream.ReceiveNext();
            positionsL = (Vector3[])stream.ReceiveNext();
        }
    }
}

class LaserVisuals : LogObject
{
    public int id;
    public Vector3 target;
    public bool enable;
    public LaserVisuals() : base("LaserVisuals") { }
}
