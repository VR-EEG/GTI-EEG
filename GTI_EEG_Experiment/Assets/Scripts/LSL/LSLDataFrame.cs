using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Contains all LSL recorded data per subject (concatenation of ExperimentBlockData,
ExperimentTrialData, ExperimentDataPoint)*/

[Serializable]
public class LSLDataFrame
{
    // todo refine and assign the variables in fixed update 
    
    // Subject ID to make sure no blocks get lost if filename is changed 
    public int subjectId;

    // Date Time Subject Meta Data was created
    public string dateTimeSubjectMetaDataCreated;

    // Eye tracking validation result immediately before current block 
    public Vector3 eyeTrackingOverallCombinedValidationResults;
    
    /// <summary>
    /// TimeStamps stream
    /// channel_count: 3
    /// </summary>
    // TimeStamps 
    public double CurrentTimeStamp;
    public double timeStampDataPointStart;
    public double timeStampDataPointEnd;

    
    /// <summary>
    /// ToolCueOrientation ints stream
    /// channel_count: 7
    /// </summary>
    // Tool, cue, orientation info 
    public int trialID; 
    public int blockNumber; // Current block
    public int utcon; 
    public int toolId; 
    public int cueOrientationId; 
    public int toolIsCurrentlyAttachedToHand; // boolean; 1 for true, 0 for false
    public int toolIsCurrentlyDisplayedOnTable; // boolean; 1 for true, 0 for false
    
    /// <summary>
    /// ToolCueOrientation strings stream
    /// channel_count: 5
    /// </summary>
    public string toolName;
    public string cueOrientationName; 
    public string cueName;
    public string toolHandleOrientation;
    public string closestAttachmentPointOnToolToHand;

    
    /// <summary>
    /// EyeTrackingGazeHMDFloat stream
    /// channel_count: 55
    /// </summary>
    // EyeTracking 
    public float eyeOpennessLeft;
    public float eyeOpennessRight;
    public float pupilDiameterMillimetersLeft;
    public float pupilDiameterMillimetersRight;
    public Vector3 eyePositionCombinedWorld;
    public Vector3 eyeDirectionCombinedWorld;
    public Vector3 eyePositionLeftWorld;
    public Vector3 eyeDirectionLeftWorld;
    public Vector3 eyePositionRightWorld;
    public Vector3 eyeDirectionRightWorld;
    
    // GazeRay hit object 
    public Vector3 hitPointOnObjectCombinedEyes;
    public Vector3 hitObjectCenterInWorldCombinedEyes;
    public Vector3 hitPointOnObjectLeftEye;
    public Vector3 hitObjectCenterInWorldLeftEye;
    public Vector3 hitPointOnObjectRightEye;
    public Vector3 hitObjectCenterInWorldRightEye;
    
    /// <summary>
    /// GazeValidity stream
    /// channel_count: 55
    /// </summary>
    //validity left
    
    public bool LeftDataGazeOriginValidity; /** The validity of the origin of gaze of the eye data */
    public bool LeftDataGazeDirectionValidity; /** The validity of the direction of gaze of the eye data */
    public bool LeftDataPupilDiameterValidity; /** The validity of the diameter of gaze of the eye data */
    public bool LeftDataEyeOpennessValidity; /** The validity of the openness of the eye data */
    public bool LeftDataPupilPositionInSensorAreaValidity;  /** The validity of normalized position of pupil */
    public bool LeftAllValid; 
    public bool LeftOriginAndDirectionValid; // combined gaze seems to only have origin and direction set to valid 
    
    //validity right
    
    public bool RightDataGazeOriginValidity; /** The validity of the origin of gaze of the eye data */
    public bool RightDataGazeDirectionValidity; /** The validity of the direction of gaze of the eye data */
    public bool RightDataPupilDiameterValidity; /** The validity of the diameter of gaze of the eye data */
    public bool RightDataEyeOpennessValidity; /** The validity of the openness of the eye data */
    public bool RightDataPupilPositionInSensorAreaValidity;  /** The validity of normalized position of pupil */
    public bool RightAllValid; 
    public bool RightOriginAndDirectionValid; 
    
    // HMD 
    public Vector3 hmdPos;
    public Vector3 hmdDirectionForward;
    public Vector3 hmdDirectionRight;
    public Vector3 hmdRotation;
    public Vector3 hmdDirectionUp;
    
    /// <summary>
    /// GazeRayHitObjectString stream
    /// channel_count: 3
    /// </summary>
    public string hitObjectNameCombinedEyes;
    public string hitObjectNameLeftEye;
    public string hitObjectNameRightEye;

    
    /// <summary>
    /// Input stream
    /// channel_count: 23
    /// </summary>
    // SteamVR input 
    public float controllerTriggerPressed; // boolean; 1 for true, 0 for false
    public Vector3 controllerPosition;
    public Vector3 controllerRotation;
    public Vector3 controllerScale;

    // LeapMotion Input 
    public float leapIsGrasping; // boolean; 1 for true, 0 for false
    public float leapGrabStrength;
    public float leapGrabAngle; 
    public float leapPinchStrength;
    public Vector3 leapHandPosition;
    public Vector3 leapHandPalmPosition;
    public Vector3 leapHandRotation;
}