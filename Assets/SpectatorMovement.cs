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
        if(pv.isMine)
        {
            GetComponentInChildren<Camera>().enabled = true;
        }
    }
    void Update()
    {
        if(pv.isMine)
        {
            rotationX += Input.GetAxis("Mouse X") * cameraSensitivity * Time.deltaTime;
            rotationY += Input.GetAxis("Mouse Y") * cameraSensitivity * Time.deltaTime;
            rotationY = Mathf.Clamp(rotationY, -90, 90);

            transform.localRotation = Quaternion.AngleAxis(rotationX, Vector3.up);
            transform.localRotation *= Quaternion.AngleAxis(rotationY, Vector3.left);

            transform.localPosition += Input.GetAxis("Vertical") * transform.forward * Time.deltaTime * normalMoveSpeed;
            transform.localPosition += Input.GetAxis("Horizontal") * transform.right * Time.deltaTime * normalMoveSpeed;
        }
    }
}
