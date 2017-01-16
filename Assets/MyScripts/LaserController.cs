using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour {

    [SerializeField] GameObject laserR;
    [SerializeField] GameObject laserL;

    PhotonView pv;

    LineRenderer lineR;
    LineRenderer lineL;

    RobotController robot;

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

	void Update () {
        if(pv.isMine)
        {
            RaycastHit info;
            if(Input.GetMouseButton(0))
            {
                if(!shooting)
                {
                    laserR.SetActive(true);
                    laserL.SetActive(true);

                    robot.SwitchToShoot(true);
                    shooting = true;
                }

                if(Physics.Raycast(robot.ActiveCamera.transform.position, robot.ActiveCamera.transform.forward, out info))
                {
                    positionsR[0] = laserR.transform.position;
                    positionsR[1] = info.point;
                    lineR.SetPositions(positionsR);

                    positionsL[0] = laserL.transform.position;
                    positionsL[1] = info.point;
                    lineL.SetPositions(positionsL);
                }
            }
            else if(shooting)
            {
                laserR.SetActive(false);
                laserL.SetActive(false);
                robot.SwitchToShoot(false);
                shooting = false;
            }
        }
	}
}
