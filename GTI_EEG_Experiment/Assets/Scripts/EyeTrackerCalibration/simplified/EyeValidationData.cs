using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EyeValidationData
{
    public double UnixTimestamp;
    public bool IsErrorCheck;

    public string ParticipantID;
    public int ValidationID;
    public int CalibrationFreq;
    
    public Vector3 EyeValidationError;
    public Vector3 PointToFocus;
    public Vector3 CombinedEyeAngleOffset;
    public Vector3 LeftEyeAngleOffset;
    public Vector3 RightEyeAngleOffset;
}