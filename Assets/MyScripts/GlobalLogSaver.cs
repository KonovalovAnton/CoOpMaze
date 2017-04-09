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
        sr = File.CreateText(@"C:\Users\konov_000\Desktop\git thesis\CoOpMaze\Assets\save.txt");        
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
