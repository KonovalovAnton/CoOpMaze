using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectatorMovement : MonoBehaviour {

    public float cameraSensitivity = 90;
    public float normalMoveSpeed = 10;

    private float rotationX = 0.0f;
    private float rotationY = 0.0f;

    PhotonView pv;

    void Start()
    {
        pv = GetComponent<PhotonView>();
        if(pv.isMine || !PhotonNetwork.connected)
        {
            GetComponentInChildren<Camera>().enabled = true;
        }
    }
    void Update()
    {
        if(pv.isMine || !PhotonNetwork.connected)
        {
            float horizontal = Input.GetKey(KeyCode.A) ? -1 : Input.GetKey(KeyCode.D) ? 1 : 0;
            float vertical = Input.GetKey(KeyCode.S) ? -1 : Input.GetKey(KeyCode.W) ? 1 : 0;
            rotationX += Input.GetAxis("Mouse X") * cameraSensitivity;
            rotationY += Input.GetAxis("Mouse Y") * cameraSensitivity;
            rotationY = Mathf.Clamp(rotationY, -90, 90);

            transform.localRotation = Quaternion.AngleAxis(rotationX, Vector3.up);
            transform.localRotation *= Quaternion.AngleAxis(rotationY, Vector3.left);

            transform.localPosition += vertical * transform.forward * normalMoveSpeed;
            transform.localPosition += horizontal * transform.right * normalMoveSpeed;
        }
    }
}
