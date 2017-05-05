using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplayLoader : MonoBehaviour {

    [SerializeField]
    GameObject player;
    [SerializeField]
    GameObject camera;

    Dictionary<int, GameObject> playersDict = new Dictionary<int, GameObject>();
    List<LogObject> logs = new List<LogObject>();
    int pointer = 0;

	void Start () {
        GameObject.Instantiate(camera, Vector3.zero, Quaternion.identity);
        string[] lines = System.IO.File.ReadAllLines("save.txt");
        foreach (var item in lines)
        {
            LogObject l = JsonUtility.FromJson<LogObject>(item);
            switch(l.LogType)
            {
                case "PlayerBirth":
                    logs.Add(JsonUtility.FromJson<PlayerBirth>(item));
                    break;
                case "PlayerTransform":
                    logs.Add(JsonUtility.FromJson<PlayerTransform>(item));
                    break;
                case "ButtonActivation":
                    logs.Add(JsonUtility.FromJson<ButtonActivation>(item));
                    break;
                case "CapacitorCharge":
                    logs.Add(JsonUtility.FromJson<CapacitorCharge>(item));
                    break;
                case "LaserVisuals":
                    logs.Add(JsonUtility.FromJson<LaserVisuals>(item));
                    break;
            }
        }
    }

    bool pause = false;
    public void Pause()
    {
        if(!pause)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = GetComponentInChildren<UnityEngine.UI.Slider>().value * 2f;
        }

        pause = !pause;
    }

    public void ChangeScale()
    {
        if(!pause)
        {
            Time.timeScale = GetComponentInChildren<UnityEngine.UI.Slider>().value * 2f;
        }
    }

	void Update () {
		while(pointer < logs.Count && logs[pointer].timeStamp < Time.time)
        {
            switch (logs[pointer].LogType)
            {
                case "PlayerBirth":
                    Instantiate(logs[pointer] as PlayerBirth);
                    break;
                case "PlayerTransform":
                    Move(logs[pointer] as PlayerTransform); ;
                    break;
                case "ButtonActivation":
                    Button(logs[pointer] as ButtonActivation);
                    break;
                case "CapacitorCharge":
                    Capacitor(logs[pointer] as CapacitorCharge);
                    break;
                case "LaserVisuals":
                    Laser(logs[pointer] as LaserVisuals);
                    break;
            }
            pointer++;
        }
	}

    void Instantiate(PlayerBirth p)
    {
        playersDict.Add(p.id, GameObject.Instantiate(player,Vector3.zero, Quaternion.identity));
    }

    private void Button(ButtonActivation p)
    {
        ButtonScript.buttons[p.id].SetActive(p.active);
    }

    void Move(PlayerTransform p)
    {
        GameObject o = playersDict[p.id];
        Debug.Log("Pointer: " + pointer +"; Move: " + o.name + "  to " + p.pos);
        o.GetComponent<PlayerReplayComponent>().TargetPos = p.pos;
        o.transform.eulerAngles = new Vector3(o.transform.eulerAngles.x, p.rot, o.transform.eulerAngles.z);
    }

    void Laser(LaserVisuals lv)
    {
        GameObject p = playersDict[lv.id];
        if (!lv.enable)
        {
            LineRenderer[] arr = p.GetComponentsInChildren<LineRenderer>();
            foreach (var item in arr)
            {
                item.enabled = false;
                //item.SetPositions(null);
            }
        }
        else
        {
            LineRenderer[] arr = p.GetComponentsInChildren<LineRenderer>();
            foreach (var item in arr)
            {
                item.enabled = true;
                item.SetPositions(new Vector3[]{item.transform.position, lv.target});
            }
        }
    }

    void Capacitor(CapacitorCharge p)
    {
        CapasitorScript.capacitors[p.id].charge = p.charge;
    }
}
