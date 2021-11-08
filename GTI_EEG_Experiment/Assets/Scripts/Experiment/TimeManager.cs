using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    
    public static TimeManager Instance { get; private set; }
    
    private bool _experimentStarted;
    private double _timeSinceStart;

    private double _applicationStartTime;
    private double _experimentStartTime;
    private double _trainingStartTime;
    private double _experimentEndTime;
    
    
    float _deltaTime = 0.0f;
    

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);       
        }
        else
        {
            Destroy(gameObject);
        }
        
        _applicationStartTime = GetCurrentUnixTimeStamp();
    }
    void Start()
    {
        _timeSinceStart = 0f;
    }
    

    // Update is called once per frame
    void Update()
    {
        _deltaTime += (Time.unscaledDeltaTime - _deltaTime) * 0.1f;
        
        if (_experimentStarted)
        {
            _timeSinceStart += Time.deltaTime;
        }
    }
    
    public double GetCurrentUnixTimeStamp()
    {
        System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
        return (System.DateTime.UtcNow - epochStart).TotalSeconds;
    }
    
    public float GetCurrentFPS()
    {
        return 1.0f / _deltaTime;
    }
    

  
    
    public void SetExperimentStartTime()
    {
        var time = GetCurrentUnixTimeStamp();
        _experimentStartTime = time;
    }
    
    public void SetExperimentEndTime()
    {
        var time = GetCurrentUnixTimeStamp();
        _experimentEndTime = time;
    }

    public double GetApplicationDuration()
    {
        return (_experimentEndTime - _applicationStartTime);
    }
    

    public double GetExperimentDuration()
    {
        return (_experimentEndTime - _experimentStartTime);
    }
    
    private double GetTimeSinceStartUp()
    {
        return _timeSinceStart;
    }
}