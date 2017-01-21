using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollider : MonoBehaviour
{

    public Transform pivot;
    public float L;
    public float Speed;

    private bool isColliding = false;
    private float delta = 0.05f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!isColliding && Vector3.Distance(transform.position, pivot.position) < L)
        {
            transform.Translate(-(pivot.position - transform.position).normalized * Time.deltaTime * Speed);
        }
    }

    void OnCollisionEnter(Collision col)
    {
        isColliding = true;
    }

    void OnCollisionStay(Collision col)
    {
        if (Vector3.Distance(transform.position, pivot.position) > delta)
        {
            transform.Translate((pivot.position - transform.position).normalized * Time.deltaTime * Speed);
        }
    }

    void OnCollisionExit(Collision col)
    {
        isColliding = false;
    }
}