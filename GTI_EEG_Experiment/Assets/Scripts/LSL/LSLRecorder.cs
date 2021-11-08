using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class LSLRecorder : MonoBehaviour
{
    public static LSLRecorder Instance { get; private set; }

    [SerializeField] private ConfigManager configManager;

    private LSLDataFrame _lslDataFrame;

    private double _timestampBegin;
    private double _timestampEnd;

    private bool _recordLsl;
    
    private int _trialId;
    private int _blockId;
    private int _utcon;

    #region Private Methods

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
    }

    private void FixedUpdate()
    {
        if (!_recordLsl) return;

        double[] timestamps =
        {
            TimeManager.Instance.GetCurrentUnixTimeStamp(),
            _timestampBegin,
            _timestampEnd
        };

        int[] toolCueOrientationInt =
        {
            _trialId,
            _blockId,
            _utcon,
            // toolId; 
            // cueOrientationId; 
            // toolIsCurrentlyAttachedToHand; 
            // toolIsCurrentlyDisplayedOnTable;
        };
        // todo gather lslIToolCueOrientationInt 7 int


        // todo gather lslIToolCueOrientationString 5 string

        // todo gather lslIEyeTrackingGazeHMDFloat 55 float
        // todo gather lslIEyeTrackingGazeHMDString 3 string

        // todo gather lslIInput 26 float


        SaveTimeStamps(timestamps);

        // todo push lslIToolCueOrientationInt 7 int
        // todo push lslIToolCueOrientationString 5 string

        // todo push lslIEyeTrackingGazeHMDFloat 55 float
        // todo push lslIEyeTrackingGazeHMDString 3 string

        // todo push lslIInput 26 float
    }

    void SaveTimeStamps(double[] timestamps)
    {
        LSLStreams.Instance.lslOTimeStamps.push_sample(timestamps);
    }

    #endregion

    #region Setter Methods

    public void SetLSLRecordingStatus(bool state)
    {
        _recordLsl = state;
    }

    public void SetTimestampBegin(double timestamp)
    {
        _timestampBegin = timestamp;
    }

    public void SetTimestampEnd(double timestamp)
    {
        _timestampEnd = timestamp;
    }

    public void SetTrialID(int id)
    {
        _trialId = id;
    }

    public void SetBlockID(int id)
    {
        _blockId = id;
    }
    
    public void SetUtcon(int value)
    {
        _utcon = value;
    }
    
    #endregion
}