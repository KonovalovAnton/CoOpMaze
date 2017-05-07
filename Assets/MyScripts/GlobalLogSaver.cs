using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GlobalLogSaver : MonoBehaviour{

    static GlobalLogSaver _instance;
	public static GlobalLogSaver Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = new GlobalLogSaver();
            }
            return _instance;
        }
    }

    List<string> logs = new List<string>();
    StreamWriter sr;


    public void AddLogString(string log)
    {
        logs.Add(log);
    }

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }

        string dir = "replays/";
        Directory.CreateDirectory(dir);

        string path = System.DateTime.Today.ToShortDateString() + "_" + System.DateTime.Now.ToShortTimeString() + ".txt";
        path = path.Replace('/', '_');
        path = path.Replace(':', '_');
        path = path.Replace(' ', '_');
        
        sr = File.CreateText(dir + path);
    }

    private void Update()
    {
        if(logs.Count > 0)
        {
            foreach (var item in logs)
            {
                sr.WriteLine(item);
            }
            logs.Clear();
        }
    }

    private void OnDestroy()
    {
        sr.Close();
    }


}
