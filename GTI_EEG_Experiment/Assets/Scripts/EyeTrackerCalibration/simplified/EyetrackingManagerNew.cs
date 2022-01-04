﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ViveSR.anipal.Eye;

public class EyetrackingManagerNew : MonoBehaviour
{
    public static EyetrackingManagerNew Instance { get; private set; }


    private EyetrackingUINew EyetrackingUI;
    public ConfigManager configManager;

    public GameObject GrayRoomSphere;
    public EyeTrackingValidation _eyetrackingValidation;
    public int SamplingRate = 90;
    private Transform _hmdTransform;
    private bool _eyeValidationSucessful;
    private bool _calibrationSuccess;
    private bool _calibrationInProgress;
    private bool _isCalibrated;
    private bool _calibrationInitialized;
    private bool _setupColosed;
    
    
    private bool _validationCompleted;
    


    private float eyeValidationDelay;

    private Vector3 _eyeValidationErrorAngles;
    private Vector3 _leftEyeValidationErrorAngles;
    private Vector3 _rightEyeValidationErrorAngles;

    private EyeTrackingValidation.EyeTrackingValidationData _validationData;
    
    public delegate void OnCompletedEyeValidation(bool wasSuccessful);
    public event OnCompletedEyeValidation NotifyEyeValidationCompletnessObservers;
    
    
    private Vector3 CombinedEyeAngleOffset;
    private Vector3 LeftEyeAngleOffset;
    private Vector3 RightEyeAngleOffset;


    private void Awake()
    {
           
        //singleton pattern a la Unity
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);         //the Eyetracking Manager should be persistent by changing the scenes maybe change it on the the fly
        }
        else
        {
            Destroy(gameObject);
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        
        configManager = GameObject.FindGameObjectWithTag("ConfigManager").GetComponent<ConfigManager>();
        EyetrackingUI = GetComponent<EyetrackingUINew>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
  


    public void StartSetup()
    {
        _setupColosed = false;
        EyetrackingUI.Activate(true);
    }
    
    public void CloseSetup()
    {
        _calibrationInitialized = false;
        EyetrackingUI.Activate(false);
        _setupColosed = true;
    }
    

    public bool IsSetupClosed()
    {
        return _setupColosed;
    }
    
    
    public bool IsCalibrationInitialized()
    {
        return _calibrationInitialized;
    }

    public bool CalibrationSucessful()
    {
        return _calibrationSuccess;
    }

    public bool IsCalibrationInProgress()
    {
        return _calibrationInProgress;
    }
    
    public void StartCalibration()
    {
        _calibrationInitialized=true;
        configManager.resumeExperimentAfterEyeTrackerCalibrationValidation = true;
        StartCoroutine(CalibrateDevice());
    }

    private IEnumerator CalibrateDevice()
    {
        bool success;
        _calibrationInProgress = true;
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        success= SRanipal_Eye_v2.LaunchEyeCalibration();
        
        yield return new WaitForEndOfFrame();
        _calibrationSuccess = success;
        _calibrationInProgress = false;

    }

    /*public void StartValidation()
    {
        _eyetrackingValidation.SetHMDTransform(ExperimentManager.Instance.GetActiveCamera());
        _eyetrackingValidation.StartValidateEyetracker();
    }

    public void StartRecording()
    {
        Debug.Log("<color=green>Recording eye-tracking Data!</color>");
        device.StartRecording();
    }

    public void SaveEyetrackingData(string fileName="Test")
    {
        device.SaveRecordedData(Path.Combine(DataSavingManager.Instance.GetSavePath(), fileName + ".json"));
    }*/

    
    
}
