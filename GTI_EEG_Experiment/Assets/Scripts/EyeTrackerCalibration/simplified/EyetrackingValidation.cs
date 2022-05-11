using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ViveSR.anipal.Eye;

public class EyetrackingValidation : MonoBehaviour
{

    
    #region Fields

    [Space] [Header("Eye-tracker validation field")]
    [SerializeField] private GameObject fixationPoint;
    [SerializeField] private List<Vector3> keyPositions;

    private bool _isValidationRunning;
    private bool _isErrorCheckRunning;
    private bool _isExperiment;
    
    private string _participantId;
    private string _sessionId;
    
    private Coroutine _runValidationCo;
    private Coroutine _runErrorCheckCo;
    private Transform _hmdTransform;
    private List<EyeValidationData> _eyeValidationDataFrames;
    private EyeValidationData _eyeValidationData;
    private const float ErrorThreshold = 1.0f;
    
    public event Action BaseLineCheckStarted;
    public event Action BaseLineCheckEnded;

    #endregion

    #region Private methods
    

    private void Start()
    {
        fixationPoint.SetActive(false);
        _eyeValidationDataFrames = new List<EyeValidationData>();
    }

    private void SaveValidationFile()
    {
        var fileName = _participantId + "_EyeValidation_" + TimeManager.Instance.GetCurrentUnixTimeStamp();

        if (_isExperiment)
        {
            DataSavingManager.Instance.SaveList(_eyeValidationDataFrames, fileName + "_TA");
        }
        else
        {
            DataSavingManager.Instance.SaveList(_eyeValidationDataFrames, fileName + "_Expl" + "_S_" + _sessionId);
        }
    }
    
    private Vector3 GetValidationError()
    {
        return _eyeValidationData.EyeValidationError;
    }

    public EyeValidationData GetValidationData()
    {
        return _eyeValidationData;
    }

    private IEnumerator ValidateEyeTracker(float delay=2)
    {
        if (_isValidationRunning) yield break;
        _isValidationRunning = true;
        
        _hmdTransform = EyetrackingManagerNew.Instance.GetHmdTransform();
        fixationPoint.transform.parent =  _hmdTransform.gameObject.transform;
        fixationPoint.transform.position = _hmdTransform.position + _hmdTransform.rotation * new Vector3(0,0,30);
        fixationPoint.transform.LookAt(_hmdTransform);
        
        
        yield return new WaitForSeconds(.15f);
        
        fixationPoint.SetActive(true);

        yield return new WaitForSeconds(delay);
        
        var anglesX = new List<float>();
        var anglesY = new List<float>();
        var anglesZ = new List<float>();
        
        for (var i = 1; i < keyPositions.Count; i++)
        {
            var startTime = Time.time;
            float timeDiff = 0;

            while (timeDiff < 1f)
            {
                fixationPoint.transform.position = _hmdTransform.position + _hmdTransform.rotation * Vector3.Lerp(keyPositions[i-1], keyPositions[i], timeDiff / 1f);   
                fixationPoint.transform.LookAt(_hmdTransform);
                yield return new WaitForEndOfFrame();
                timeDiff = Time.time - startTime;
            }
            
            // _validationPointIdx = i;
            startTime = Time.time;
            timeDiff = 0;
            
            while (timeDiff < 2f)
            {
                fixationPoint.transform.position = _hmdTransform.position + _hmdTransform.rotation * keyPositions[i] ;
                fixationPoint.transform.LookAt(_hmdTransform);
                EyeValidationData validationData = GetEyeValidationData();
                
                if (validationData != null)
                {
                    anglesX.Add(validationData.CombinedEyeAngleOffset.x);
                    anglesY.Add(validationData.CombinedEyeAngleOffset.y);
                    anglesZ.Add(validationData.CombinedEyeAngleOffset.z);
                    
                    validationData.EyeValidationError.x = CalculateValidationError(anglesX);
                    validationData.EyeValidationError.y = CalculateValidationError(anglesY);
                    validationData.EyeValidationError.z = CalculateValidationError(anglesZ);

                    _eyeValidationData = validationData;
                }
                
                yield return new WaitForEndOfFrame();
                timeDiff = Time.time - startTime;
            }
        }

        fixationPoint.transform.position = Vector3.zero;

        _isValidationRunning = false;
        
        fixationPoint.transform.parent = gameObject.transform;

        Debug.Log( "Get validation error" + GetValidationError() + " + " + _eyeValidationData.EyeValidationError);
        
        _eyeValidationDataFrames.Add(_eyeValidationData);
        SaveValidationFile();


        fixationPoint.SetActive(false);




        var success=false;
        
        // give feedback whether the error was too large or not
        if (CalculateValidationError(anglesX) > ErrorThreshold || 
            CalculateValidationError(anglesY) > ErrorThreshold ||
            CalculateValidationError(anglesZ) > ErrorThreshold ||
            _eyeValidationData.EyeValidationError == Vector3.zero)
        {
            success = true;
        }
        else
        {
            success = false;
        }
        
        
        EyetrackingManagerNew.Instance.ValidationCompleted(success);
    }
    
    private IEnumerator EEGBaseLineCheck(float delay=5)
    {
        if (_isErrorCheckRunning) yield break;
        _isErrorCheckRunning = true;
        Debug.Log("EEG Baselinecheck...");
        
        BaseLineCheckStarted?.Invoke();
        _hmdTransform = EyetrackingManagerNew.Instance.GetHmdTransform();
        fixationPoint.transform.parent =  _hmdTransform.gameObject.transform;
        fixationPoint.transform.position = _hmdTransform.position + _hmdTransform.rotation * new Vector3(0,0,30);
        fixationPoint.transform.LookAt(_hmdTransform);
        fixationPoint.SetActive(true);
        yield return new WaitForSeconds(5f);
       fixationPoint.SetActive(false);
       BaseLineCheckEnded?.Invoke();
       Debug.Log("...finished");
       _isErrorCheckRunning = false;

    }
    
    private EyeValidationData GetEyeValidationData()
    {
        EyeValidationData eyeValidationData = new EyeValidationData();
        
        Ray ray;
        
        eyeValidationData.UnixTimestamp = TimeManager.Instance.GetCurrentUnixTimeStamp();
        eyeValidationData.IsErrorCheck = _isErrorCheckRunning;
        
        eyeValidationData.ParticipantID = _participantId;

        eyeValidationData.PointToFocus = fixationPoint.transform.position;

        if (SRanipal_Eye_v2.GetGazeRay(GazeIndex.LEFT, out ray))
        {
            var angles = Quaternion.FromToRotation((fixationPoint.transform.position - _hmdTransform.position).normalized, _hmdTransform.rotation * ray.direction)
                .eulerAngles;
            
            eyeValidationData.LeftEyeAngleOffset = angles;
        }
        
        if (SRanipal_Eye_v2.GetGazeRay(GazeIndex.RIGHT, out ray))
        {
            var angles = Quaternion.FromToRotation((fixationPoint.transform.position - _hmdTransform.position).normalized, _hmdTransform.rotation * ray.direction)
                .eulerAngles;

            eyeValidationData.RightEyeAngleOffset = angles;
        }

        if (SRanipal_Eye_v2.GetGazeRay(GazeIndex.COMBINE, out ray))
        {
            var angles = Quaternion.FromToRotation((fixationPoint.transform.position - _hmdTransform.position).normalized, _hmdTransform.rotation * ray.direction)
                .eulerAngles;

            eyeValidationData.CombinedEyeAngleOffset = angles;
        }

        return eyeValidationData;
    }
    
    private float CalculateValidationError(List<float> angles)
    {
        return angles.Select(f => f > 180 ? Mathf.Abs(f - 360) : Mathf.Abs(f)).Sum() / angles.Count;
    }
    
    #endregion

    #region Public methods

    public void SetExperimentStatus(bool status)
    {
        _isExperiment = status;
    }

    public void ValidateEyeTracking()
    {
        if(!_isValidationRunning) _runValidationCo = StartCoroutine(ValidateEyeTracker());
    }
    
    public void StartEEGBaselineCheck()
    {
        if(!_isErrorCheckRunning) _runErrorCheckCo = StartCoroutine(EEGBaseLineCheck());
    }

    public void SetParticipantId(string id)
    {
        _participantId = id;
    }
    
    public void SetSessionId(string id)
    {
        _sessionId = id;
    }

    #endregion
}
