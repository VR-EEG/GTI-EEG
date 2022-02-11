using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Valve.VR.InteractionSystem;
using ViveSR.anipal.Eye;

public class EyetrackingManagerNew : MonoBehaviour
{
    public static EyetrackingManagerNew Instance { get; private set; }

    [SerializeField] private float ValidationThreshold;
    private EyetrackingUINew EyetrackingUI;

    [SerializeField] private bool StartValidationAutomaticallyAfterCalibration;

    public GameObject GrayRoomSphere;
   // public EyeTrackingValidation _eyetrackingValidation;
    public int SamplingRate = 90;
    private Transform _hmdTransform;
    private bool _eyeValidationSucessful;
    private bool _calibrationSuccess;
    private bool _calibrationInProgress;
    private bool _calibrationInitialized;
    private bool _setupColosed;

    private bool _validationInProgress;
    private bool _validationSucessful;
    private bool _validationInitialized;



    private float eyeValidationDelay;

    private Vector3 _eyeValidationErrorAngles;
    private Vector3 _leftEyeValidationErrorAngles;
    private Vector3 _rightEyeValidationErrorAngles;

   // private EyeTrackingValidation.EyeTrackingValidationData _validationData;
    
    
    private Vector3 CombinedEyeAngleOffset;
    private Vector3 LeftEyeAngleOffset;
    private Vector3 RightEyeAngleOffset;


    private EyetrackingValidation _eyetrackingValidation;
    private EyeValidationData _currentValidationData;


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
        EyetrackingUI = GetComponent<EyetrackingUINew>();
        
        _eyetrackingValidation = GetComponent<EyetrackingValidation>();
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
        NewExperimentManager.Instance.EndEyeCalibration();
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

        if (StartValidationAutomaticallyAfterCalibration)
        {
            StartValidation();
        }

    }
    
    public bool IsValidationIntialized()
    {
        return _validationInitialized;
    }
    
    public bool IsValidationInProgress()
    {
        return _validationInProgress;
    }
    
   
    public void ValidationCompleted(bool result)
    {
        _currentValidationData = _eyetrackingValidation.GetValidationData();
        _validationInProgress = false;
        _validationSucessful = result;
    }


    public Vector3 GetValidationResults()
    {
        if (_validationSucessful)
        {
            return _currentValidationData.EyeValidationError;
        }
        else
        {
            return Vector3.zero;
        }
    }
    
    public bool IsValidationSucessful()
    {
        return _validationSucessful;
    }

    public void StartValidation()
    {
        _validationInitialized = true;
        _validationInProgress=true;
        _eyetrackingValidation.ValidateEyeTracking();
    }

    public void StartEEGBaselineCheck()
    {
        _eyetrackingValidation.StartEEGBaselineCheck();
    }

    public void StartRecording()
    {
        Debug.Log("<color=green>Recording eye-tracking Data!</color>");
        // usally done here , however we use LSL so bricked
    }

    public Transform GetHmdTransform()
    {
        return _hmdTransform = Player.instance.hmdTransform;
    }


    
    
}
