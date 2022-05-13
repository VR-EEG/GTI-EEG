using System;
using UnityEngine;
using LSL;
using UnityEditor;

public class LSLStreams : MonoBehaviour
{
    #region Fields

    public static LSLStreams Instance { get; private set; } // used to allow easy access of this script in other scripts
    
    
    private string uniqueIdentifier;

    private const double NominalRate = liblsl.IRREGULAR_RATE;
    
    
    private liblsl.StreamInfo lslIValidationResultFloat;
    public liblsl.StreamOutlet lslOValidationResultFloat;
    
    private liblsl.StreamInfo lslIValidationResultTimeStamp;
    public liblsl.StreamOutlet lslOValidationResultTimeStamp;
    
    // irregular sampling rate
    private liblsl.StreamInfo lslIBaselineBeginTimeStamp;
    public liblsl.StreamOutlet lslOBaselineBeginTimeStamp;
    
    private liblsl.StreamInfo lslIBaselineEndTimeStamp;
    public liblsl.StreamOutlet lslOBaselineEndTimeStamp;
    
    // variables to save date to LSL
    private liblsl.StreamInfo lslIFrameTracking;
    public liblsl.StreamOutlet lslOFrameTracking; // saved in LSLRecorder.cs


    
    private liblsl.StreamInfo lslIEyetrackingFrameTimeStamp;
    public liblsl.StreamOutlet LslOEyetrackingFrameTimeStamp; // saved in LSLRecorder.cs
    
    private liblsl.StreamInfo lslITrialStartMeasurementTimeStamp;
    public liblsl.StreamOutlet lslOTrialStartMeasurementTimeStamp; // saved in MeasurementManager.cs
    
    private liblsl.StreamInfo lslICueTimeStamp;
    public liblsl.StreamOutlet lslOCueTimeStamp; // saved in ExperimentManager.cs
    
    private liblsl.StreamInfo lslICueDisappearedTimeStamp;
    public liblsl.StreamOutlet lslOCueDisappearedTimeStamp; // saved in ExperimentManager.cs
    
    private liblsl.StreamInfo lslIObjectShownTimeStamp;
    public liblsl.StreamOutlet lslOObjectShownTimeStamp; // saved in ExperimentManager.cs
    
    private liblsl.StreamInfo lslIBeepPlayedTimeStamp;
    public liblsl.StreamOutlet lslOBeepPlayedTimeStamp; // saved in ExperimentManager.cs
    
    private liblsl.StreamInfo lslIButtonPressedTimeStamp;
    public liblsl.StreamOutlet lslOButtonPressedTimeStamp; // saved in ExperimentManager.cs
    
    private liblsl.StreamInfo lslITrialStopMeasurementTimeStamp;
    public liblsl.StreamOutlet lslOTrialStopMeasurementTimeStamp; 
    
    private liblsl.StreamInfo lslITrialInformationInt;
    public liblsl.StreamOutlet lslOTrialInformationInt; // saved in LSLRecorder.cs
    
    private liblsl.StreamInfo lslIEyeTrackingGazeHMDFloat;
    public liblsl.StreamOutlet lslOEyeTrackingGazeHMDFloat; // saved in LSLRecorder.cs
    
    private liblsl.StreamInfo lslIGazeValidity;
    public liblsl.StreamOutlet lslOGazeValidity; // saved in LSLRecorder.cs
    
    private liblsl.StreamInfo lslIEyeTrackingGazeHMDString;
    public liblsl.StreamOutlet lslOEyeTrackingGazeHMDString; // saved in LSLRecorder.cs

    private liblsl.StreamInfo lslIInput;
    public liblsl.StreamOutlet lslOInput; // saved in LSLRecorder.cs

    #endregion


    #region Private methods

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    #endregion

    #region Streams initialization

    public void Start()
    {
        uniqueIdentifier = System.Guid.NewGuid().ToString(); //this variable is only used to recognize streams

        lslIValidationResultFloat = new liblsl.StreamInfo("ValidationResults",
            "Markers",
            3,
            NominalRate,
            liblsl.channel_format_t.cf_float32,
            uniqueIdentifier
            );
        lslIValidationResultFloat.desc().append_child("xError");
        lslIValidationResultFloat.desc().append_child("yError");
        lslIValidationResultFloat.desc().append_child("zError");

        lslOValidationResultFloat = new liblsl.StreamOutlet(lslIValidationResultFloat);
        
        lslIValidationResultTimeStamp = new liblsl.StreamInfo(
            "ValidationResultTimeStamp",
            "Markers",
            1,
            NominalRate,
            liblsl.channel_format_t.cf_double64,
            uniqueIdentifier
        );
        lslIValidationResultTimeStamp.desc().append_child("ValidationResultTimeStamp");
        lslOValidationResultTimeStamp = new liblsl.StreamOutlet(lslIValidationResultTimeStamp);

        lslIBaselineBeginTimeStamp = new liblsl.StreamInfo(
            "BaseLineBeginTimeStamp",
            "Markers",
            1,
            NominalRate,
            liblsl.channel_format_t.cf_double64,
            uniqueIdentifier
        );
        lslIBaselineBeginTimeStamp.desc().append_child("BaseLineBeginTimeStamp");
        lslOBaselineBeginTimeStamp = new liblsl.StreamOutlet(lslIBaselineBeginTimeStamp);
        
        
        lslIBaselineEndTimeStamp = new liblsl.StreamInfo(
            "BaseLineEndTimeStamp",
            "Markers",
            1,
            NominalRate,
            liblsl.channel_format_t.cf_double64,
            uniqueIdentifier
        );
        lslIBaselineEndTimeStamp.desc().append_child("BaseLineEndTimeStamp");
        lslOBaselineEndTimeStamp = new liblsl.StreamOutlet(lslIBaselineEndTimeStamp);

        lslITrialStartMeasurementTimeStamp = new liblsl.StreamInfo(
            "TrialStartMeasurementTimeStamp",
            "Markers",
            1,
            NominalRate,
            liblsl.channel_format_t.cf_double64,
            uniqueIdentifier
        );
        lslITrialStartMeasurementTimeStamp.desc().append_child("TrialStartMeasurementTimeStamp");
        lslOTrialStartMeasurementTimeStamp = new liblsl.StreamOutlet(lslITrialStartMeasurementTimeStamp);

        lslICueTimeStamp = new liblsl.StreamInfo(
            "CueTimeStamp",
            "Markers",
            1,
            NominalRate,
            liblsl.channel_format_t.cf_double64,
            uniqueIdentifier
        );
        lslICueTimeStamp.desc().append_child("CueTimeStamp");
        lslOCueTimeStamp = new liblsl.StreamOutlet(lslICueTimeStamp);

        lslICueDisappearedTimeStamp = new liblsl.StreamInfo(
            "CueDisappearedTimeStamp",
            "Markers",
            1,
            NominalRate,
            liblsl.channel_format_t.cf_double64,
            uniqueIdentifier
        );
        lslICueDisappearedTimeStamp.desc().append_child("CueDisappearedTimeStamp");
        lslOCueDisappearedTimeStamp = new liblsl.StreamOutlet(lslICueDisappearedTimeStamp);
    
        lslIObjectShownTimeStamp = new liblsl.StreamInfo(
            "ObjectShownTimeStamp",
            "Marker",
            1,
            NominalRate,
            liblsl.channel_format_t.cf_double64,
            uniqueIdentifier
        );
        lslIObjectShownTimeStamp.desc().append_child("ObjectShownTimeStamp");
        lslOObjectShownTimeStamp = new liblsl.StreamOutlet(lslIObjectShownTimeStamp);

        lslIBeepPlayedTimeStamp = new liblsl.StreamInfo(
            "BeepPlayedTimeStamp",
            "Marker",
            1,
            NominalRate,
            liblsl.channel_format_t.cf_double64,
            uniqueIdentifier
        );
        lslIBeepPlayedTimeStamp.desc().append_child("BeepPlayedTimeStamp");
        lslOBeepPlayedTimeStamp = new liblsl.StreamOutlet(lslIBeepPlayedTimeStamp);

        lslIButtonPressedTimeStamp = new liblsl.StreamInfo(
            "ButtonPressedTimeStamp",
            "Marker",
            1,
            NominalRate,
            liblsl.channel_format_t.cf_double64,
            uniqueIdentifier
        );
        lslIButtonPressedTimeStamp.desc().append_child("ButtonPressedTimeStamp");
        lslOButtonPressedTimeStamp = new liblsl.StreamOutlet(lslIButtonPressedTimeStamp);


        lslITrialStopMeasurementTimeStamp = new liblsl.StreamInfo(
            "TrialStopMeasurementTimeStamp",
            "Marker",
            1,
            NominalRate,
            liblsl.channel_format_t.cf_double64,
            uniqueIdentifier
        );
        lslITrialStopMeasurementTimeStamp.desc().append_child("TrialStopMeasurementTimeStamp");
        lslOTrialStopMeasurementTimeStamp = new liblsl.StreamOutlet(lslITrialStopMeasurementTimeStamp);


        lslITrialInformationInt = new liblsl.StreamInfo(
            "TrialInformationInt",
            "Markers",
            4,
            NominalRate,
            liblsl.channel_format_t.cf_int32,
            uniqueIdentifier
        );
        lslITrialInformationInt.desc().append_child("TrialNumber");
        lslITrialInformationInt.desc().append_child("ToolId");
        lslITrialInformationInt.desc().append_child("Task");
        lslITrialInformationInt.desc().append_child("Orientation");
        lslOTrialInformationInt = new liblsl.StreamOutlet(lslITrialInformationInt);
        
        
        
        
        lslIEyetrackingFrameTimeStamp = new liblsl.StreamInfo(
            "EyetrackingTimeStamp",
            "Markers",
            1,
            NominalRate,
            liblsl.channel_format_t.cf_double64,
            uniqueIdentifier
        );
        lslIEyetrackingFrameTimeStamp.desc().append_child("CurrentFrameTimeStamp");
        LslOEyetrackingFrameTimeStamp = new liblsl.StreamOutlet(lslIEyetrackingFrameTimeStamp);



        lslIEyeTrackingGazeHMDFloat = new liblsl.StreamInfo(
            "EyeTrackingGazeHMDFloat",
            "Markers",
            70,
            NominalRate,
            liblsl.channel_format_t.cf_float32,
            uniqueIdentifier
        );
        //hmd information
        lslIEyeTrackingGazeHMDFloat.desc().append_child("hmdPos.x");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("hmdPos.y");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("hmdPos.z");

        lslIEyeTrackingGazeHMDFloat.desc().append_child("hmdRotation.x");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("hmdRotation.y");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("hmdRotation.z");

        lslIEyeTrackingGazeHMDFloat.desc().append_child("hmdDirectionForward.x");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("hmdDirectionForward.y");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("hmdDirectionForward.z");

        lslIEyeTrackingGazeHMDFloat.desc().append_child("controllerPosition.x");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("controllerPosition.y");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("controllerPosition.z");

        lslIEyeTrackingGazeHMDFloat.desc().append_child("controllerRotation.x");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("controllerRotation.y");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("controllerRotation.z");

        //combined gaze information
        lslIEyeTrackingGazeHMDFloat.desc().append_child("CombinedValidityBitMask");

        lslIEyeTrackingGazeHMDFloat.desc().append_child("eyePositionCombinedWorld.x");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("eyePositionCombinedWorld.y");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("eyePositionCombinedWorld.z");

        lslIEyeTrackingGazeHMDFloat.desc().append_child("eyeDirectionCombinedWorld.x");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("eyeDirectionCombinedWorld.y");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("eyeDirectionCombinedWorld.z");

        lslIEyeTrackingGazeHMDFloat.desc().append_child("eyePositionCombinedLocal.x");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("eyePositionCombinedLocal.y");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("eyePositionCombinedLocal.z");

        lslIEyeTrackingGazeHMDFloat.desc().append_child("eyeDirectionCombinedLocal.x");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("eyeDirectionCombinedLocal.y");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("eyeDirectionCombinedLocal.z");

        // left eye information

        lslIEyeTrackingGazeHMDFloat.desc().append_child("LeftValidityBitMask");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("eyeOpennessLeft");

        lslIEyeTrackingGazeHMDFloat.desc().append_child("eyePositionLeftWorld.x");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("eyePositionLeftWorld.y");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("eyePositionLeftWorld.z");

        lslIEyeTrackingGazeHMDFloat.desc().append_child("eyeDirectionLeftWorld.x");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("eyeDirectionLeftWorld.y");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("eyeDirectionLeftWorld.z");

        lslIEyeTrackingGazeHMDFloat.desc().append_child("eyePositionLeftLocal.x");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("eyePositionLeftLocal.y");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("eyePositionLeftLocal.z");

        lslIEyeTrackingGazeHMDFloat.desc().append_child("eyeDirectionLeftLocal.x");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("eyeDirectionLeftLocal.y");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("eyeDirectionLeftLocal.z");

        //right eye 
        lslIEyeTrackingGazeHMDFloat.desc().append_child("RightValidityBitMask");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("eyeOpennessRight");

        lslIEyeTrackingGazeHMDFloat.desc().append_child("eyePositionRightWorld.x");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("eyePositionRightWorld.y");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("eyePositionRightWorld.z");

        lslIEyeTrackingGazeHMDFloat.desc().append_child("eyeDirectionRightWorld.x");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("eyeDirectionRightWorld.y");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("eyeDirectionRightWorld.z");

        lslIEyeTrackingGazeHMDFloat.desc().append_child("eyePositionRightLocal.x");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("eyePositionRightLocal.y");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("eyePositionRightLocal.z");

        lslIEyeTrackingGazeHMDFloat.desc().append_child("eyeDirectionRightLocal.x");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("eyeDirectionRightLocal.y");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("eyeDirectionRightLocal.z");

        //task object
        lslIEyeTrackingGazeHMDFloat.desc().append_child("taskObjectPosition.x");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("taskObjectPosition.y");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("taskObjectPosition.z");

        lslIEyeTrackingGazeHMDFloat.desc().append_child("taskObjectRotation.x");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("taskObjectRotation.y");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("taskObjectRotation.z");

        lslIEyeTrackingGazeHMDFloat.desc().append_child("taskObjectIsInHand");

        //task object was hit
        lslIEyeTrackingGazeHMDFloat.desc().append_child("taskObjectWasHit");

        lslIEyeTrackingGazeHMDFloat.desc().append_child("targetHitPositionX");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("targetHitPositionY");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("targetHitPositionZ");

        lslIEyeTrackingGazeHMDFloat.desc().append_child("targetPositionX");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("targetPositionY");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("targetPositionZ");
        lslOEyeTrackingGazeHMDFloat = new liblsl.StreamOutlet(lslIEyeTrackingGazeHMDFloat);

    }


    #endregion
}