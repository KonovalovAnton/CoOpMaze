using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformSaver : MonoBehaviour {

    float interval = 0.1f;
    float t = 0;

	// Use this for initialization
	void Start () {
        PlayerBirth b = new PlayerBirth()
        {
            id = gameObject.GetInstanceID()
        };
        PlayerTransform p = new PlayerTransform()
        {
            id = gameObject.GetInstanceID(),
            pos = transform.position,
            rot = transform.rotation.eulerAngles,
            scale = transform.localScale
        };
        string log = JsonUtility.ToJson(b);
        GlobalLogSaver.Instance.AddLogString(log);
        log = JsonUtility.ToJson(p);
        GlobalLogSaver.Instance.AddLogString(log);
    }
	
	// Update is called once per frame
	void Update () {
        t += Time.deltaTime;
        if(t > interval)
        {
            PlayerTransform p = new PlayerTransform()
            {
                id = gameObject.GetInstanceID(),
                pos = transform.position,
                rot = transform.rotation.eulerAngles,
                scale = transform.localScale
            };
            string log = JsonUtility.ToJson(p);
            GlobalLogSaver.Instance.AddLogString(log);
            t = 0;
        }
	}
}

class LogObject
{
    public float timeStamp;
    public string LogType;
    public LogObject(string type)
    {
        timeStamp = Time.time;
        LogType = type;
    }
}

class PlayerTransform : LogObject
{
    public Vector3 pos;
    public Vector3 rot;
    public Vector3 scale;
    public int id;
    public PlayerTransform() : base("PlayerTransform") {}
}

class PlayerBirth : LogObject
{
    public int id;
    public PlayerBirth() : base("PlayerBirth") { }
}
