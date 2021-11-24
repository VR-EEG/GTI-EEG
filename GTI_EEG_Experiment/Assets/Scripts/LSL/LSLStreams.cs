using System;
using UnityEngine;
using LSL;

public class LSLStreams : MonoBehaviour
{
    #region Fields

    public static LSLStreams Instance { get; private set; } // used to allow easy access of this script in other scripts

    private ConfigManager _configManager;
    
    private string subjectID;

    private const double NominalRate = liblsl.IRREGULAR_RATE; // irregular sampling rate

    // variables to save date to LSL
    private liblsl.StreamInfo lslIFrameTracking;
    public liblsl.StreamOutlet lslOFrameTracking; // saved in LSLRecorder.cs

    private liblsl.StreamInfo lslIFrameTimeStamp;
    public liblsl.StreamOutlet lslOFrameTimeStamp; // saved in LSLRecorder.cs
    
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
    public liblsl.StreamOutlet lslOTrialStopMeasurementTimeStamp; // saved in MeasurementManager.cs
    
    private liblsl.StreamInfo lslIToolCueOrientationInt;
    public liblsl.StreamOutlet lslOToolCueOrientationInt; // saved in LSLRecorder.cs

    private liblsl.StreamInfo lslIToolCueOrientationString;
    public liblsl.StreamOutlet lslOToolCueOrientationString; // saved in LSLRecorder.cs
    
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
        _configManager = GameObject.FindWithTag("ConfigManager").GetComponent<ConfigManager>();
        subjectID = _configManager.subjectId;
        
        lslIFrameTracking = new liblsl.StreamInfo(
            "FrameTracking",
            "Markers",
            1,
            NominalRate,
            liblsl.channel_format_t.cf_int32,
            subjectID);
        lslIFrameTracking.desc().append_child("CurrentFrame");
        lslOFrameTracking = new liblsl.StreamOutlet(lslIFrameTracking);

        
        lslIFrameTimeStamp = new liblsl.StreamInfo(
            "TimeStamps",
            "Markers",
            1,
            NominalRate,
            liblsl.channel_format_t.cf_double64,
            subjectID
        );
        lslIFrameTimeStamp.desc().append_child("CurrentFrameTimeStamp");
        lslOFrameTimeStamp = new liblsl.StreamOutlet(lslIFrameTimeStamp);
        
        lslITrialStartMeasurementTimeStamp = new liblsl.StreamInfo(
            "TrialStartMeasurementTimeStamp",
            "Markers",
            1,
            NominalRate,
            liblsl.channel_format_t.cf_double64,
            subjectID
        );
        lslITrialStartMeasurementTimeStamp.desc().append_child("TrialStartMeasurementTimeStamp");
        lslOTrialStartMeasurementTimeStamp = new liblsl.StreamOutlet(lslITrialStartMeasurementTimeStamp);
        
        lslICueTimeStamp = new liblsl.StreamInfo(
            "CueTimeStamp",
            "Markers",
            1,
            NominalRate,
            liblsl.channel_format_t.cf_double64,
            subjectID
        );
        lslICueTimeStamp.desc().append_child("CueTimeStamp");
        lslOCueTimeStamp = new liblsl.StreamOutlet(lslICueTimeStamp);

        lslICueDisappearedTimeStamp = new liblsl.StreamInfo(
            "CueDisappearedTimeStamp",
            "Markers",
            1,
            NominalRate,
            liblsl.channel_format_t.cf_double64,
            subjectID
        );
        lslICueDisappearedTimeStamp.desc().append_child("CueDisappearedTimeStamp");
        lslOCueDisappearedTimeStamp = new liblsl.StreamOutlet(lslICueDisappearedTimeStamp);
        
        lslIObjectShownTimeStamp = new liblsl.StreamInfo(
            "ObjectShownTimeStamp",
            "Marker",
            1,
            NominalRate,
            liblsl.channel_format_t.cf_double64,
            subjectID
            );
        lslIObjectShownTimeStamp.desc().append_child("ObjectShownTimeStamp");
        lslOObjectShownTimeStamp = new liblsl.StreamOutlet(lslIObjectShownTimeStamp);
        
        lslIBeepPlayedTimeStamp = new liblsl.StreamInfo(
            "BeepPlayedTimeStamp",
            "Marker",
            1,
            NominalRate,
            liblsl.channel_format_t.cf_double64,
            subjectID
        );
        lslIBeepPlayedTimeStamp.desc().append_child("BeepPlayedTimeStamp");
        lslOBeepPlayedTimeStamp = new liblsl.StreamOutlet(lslIBeepPlayedTimeStamp);

        lslIButtonPressedTimeStamp = new liblsl.StreamInfo(
            "ButtonPressedTimeStamp",
            "Marker",
            1,
            NominalRate,
            liblsl.channel_format_t.cf_double64,
            subjectID
            );
        lslIButtonPressedTimeStamp.desc().append_child("ButtonPressedTimeStamp");
        lslOButtonPressedTimeStamp = new liblsl.StreamOutlet(lslIButtonPressedTimeStamp);
        
        
        lslITrialStopMeasurementTimeStamp = new liblsl.StreamInfo(
            "TrialStopMeasurementTimeStamp",
            "Marker",
            1,
            NominalRate,
            liblsl.channel_format_t.cf_double64,
            subjectID
        );
        lslITrialStopMeasurementTimeStamp.desc().append_child("TrialStopMeasurementTimeStamp");
        lslOTrialStopMeasurementTimeStamp = new liblsl.StreamOutlet(lslITrialStopMeasurementTimeStamp);
        
        
        lslIToolCueOrientationInt = new liblsl.StreamInfo(
            "ToolCueOrientationInt",
            "Markers",
            7,
            NominalRate,
            liblsl.channel_format_t.cf_int32,
            subjectID
        );
        lslIToolCueOrientationInt.desc().append_child("trialID");
        lslIToolCueOrientationInt.desc().append_child("blockNumber");
        lslIToolCueOrientationInt.desc().append_child("utcon");
        lslIToolCueOrientationInt.desc().append_child("toolId");
        lslIToolCueOrientationInt.desc().append_child("cueOrientationId");
        lslIToolCueOrientationInt.desc().append_child("toolIsCurrentlyAttachedToHand");
        lslIToolCueOrientationInt.desc().append_child("toolIsCurrentlyDisplayedOnTable");
        lslOToolCueOrientationInt = new liblsl.StreamOutlet(lslIToolCueOrientationInt);
        
        
        lslIToolCueOrientationString = new liblsl.StreamInfo(
            "ToolCueOrientationString",
            "Markers",
            5,
            NominalRate,
            liblsl.channel_format_t.cf_string,
            subjectID
        );
        lslIToolCueOrientationString.desc().append_child("toolName");
        lslIToolCueOrientationString.desc().append_child("cueOrientationName");
        lslIToolCueOrientationString.desc().append_child("cueName");
        lslIToolCueOrientationString.desc().append_child("toolHandleOrientation");
        lslIToolCueOrientationString.desc().append_child("closestAttachmentPointOnToolToHand");
        lslOToolCueOrientationString = new liblsl.StreamOutlet(lslIToolCueOrientationString);
        

        lslIEyeTrackingGazeHMDFloat = new liblsl.StreamInfo(
            "EyeTrackingGazeHMDFloat",
            "Markers",
            58,
            NominalRate,
            liblsl.channel_format_t.cf_float32,
            subjectID
        );
        lslIEyeTrackingGazeHMDFloat.desc().append_child("eyeOpennessLeft");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("eyeOpennessRight");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("pupilDiameterMillimetersLeft");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("pupilDiameterMillimetersRight");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("eyePositionCombinedWorld.x");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("eyePositionCombinedWorld.y");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("eyePositionCombinedWorld.z");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("eyeDirectionCombinedWorld.x");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("eyeDirectionCombinedWorld.y");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("eyeDirectionCombinedWorld.z");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("eyePositionLeftWorld.x");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("eyePositionLeftWorld.y");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("eyePositionLeftWorld.z");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("eyeDirectionLeftWorld.x");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("eyeDirectionLeftWorld.y");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("eyeDirectionLeftWorld.z");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("eyePositionRightWorld.x");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("eyePositionRightWorld.y");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("eyePositionRightWorld.z");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("eyeDirectionRightWorld.x");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("eyeDirectionRightWorld.y");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("eyeDirectionRightWorld.z");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("hitPointOnObjectCombinedEyesValidity");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("hitPointOnObjectCombinedEyes.x");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("hitPointOnObjectCombinedEyes.y");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("hitPointOnObjectCombinedEyes.z");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("hitObjectCenterInWorldCombinedEyes.x");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("hitObjectCenterInWorldCombinedEyes.y");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("hitObjectCenterInWorldCombinedEyes.z");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("hitPointOnObjectLeftEyeValidity");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("hitPointOnObjectLeftEye.x");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("hitPointOnObjectLeftEye.y");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("hitPointOnObjectLeftEye.z");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("hitObjectCenterInWorldLeftEye.x");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("hitObjectCenterInWorldLeftEye.y");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("hitObjectCenterInWorldLeftEye.z");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("hitPointOnObjectRightEyeValidity");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("hitPointOnObjectRightEye.x");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("hitPointOnObjectRightEye.y");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("hitPointOnObjectRightEye.z");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("hitObjectCenterInWorldRightEye.x");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("hitObjectCenterInWorldRightEye.y");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("hitObjectCenterInWorldRightEye.z");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("hmdPos.x");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("hmdPos.y");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("hmdPos.z");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("hmdDirectionForward.x");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("hmdDirectionForward.y");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("hmdDirectionForward.z");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("hmdDirectionRight.x");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("hmdDirectionRight.y");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("hmdDirectionRight.z");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("hmdRotation.x");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("hmdRotation.y");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("hmdRotation.z");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("hmdDirectionUp.x");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("hmdDirectionUp.y");
        lslIEyeTrackingGazeHMDFloat.desc().append_child("hmdDirectionUp.z");
        lslOEyeTrackingGazeHMDFloat = new liblsl.StreamOutlet(lslIEyeTrackingGazeHMDFloat);
        
        lslIGazeValidity = new liblsl.StreamInfo(
            "GazeValidity",
            "Markers",
            14,
            NominalRate,
            liblsl.channel_format_t.cf_int8,
            subjectID
        );
        lslIGazeValidity.desc().append_child("LeftDataGazeOriginValidity");
        lslIGazeValidity.desc().append_child("LeftDataGazeDirectionValidity");
        lslIGazeValidity.desc().append_child("LeftDataPupilDiameterValidity");
        lslIGazeValidity.desc().append_child("LeftDataEyeOpennessValidity");
        lslIGazeValidity.desc().append_child("LeftDataPupilPositionInSensorAreaValidity");
        lslIGazeValidity.desc().append_child("LeftAllValidity");
        lslIGazeValidity.desc().append_child("LeftOriginAndDirectionValidity");
        lslIGazeValidity.desc().append_child("RightDataGazeOriginValidity");
        lslIGazeValidity.desc().append_child("RightDataGazeDirectionValidity");
        lslIGazeValidity.desc().append_child("RightDataPupilDiameterValidity");
        lslIGazeValidity.desc().append_child("RightDataEyeOpennessValidity");
        lslIGazeValidity.desc().append_child("RightDataPupilPositionInSensorAreaValidity");
        lslIGazeValidity.desc().append_child("RightAllValidity");
        lslIGazeValidity.desc().append_child("RightOriginAndDirectionValidity");
        lslOGazeValidity = new liblsl.StreamOutlet(lslIGazeValidity);

        lslIEyeTrackingGazeHMDString = new liblsl.StreamInfo(
            "EyeTrackingGazeHMDString",
            "Markers",
            3,
            NominalRate,
            liblsl.channel_format_t.cf_string,
            subjectID
        );
        lslIEyeTrackingGazeHMDString.desc().append_child("hitObjectNameCombinedEyes");
        lslIEyeTrackingGazeHMDString.desc().append_child("hitObjectNameLeftEye");
        lslIEyeTrackingGazeHMDString.desc().append_child("hitObjectNameRightEye");
        lslOEyeTrackingGazeHMDString = new liblsl.StreamOutlet(lslIEyeTrackingGazeHMDString);

        
        lslIInput = new liblsl.StreamInfo(
            "Input",
            "Markers",
            23,
            NominalRate,
            liblsl.channel_format_t.cf_float32,
            subjectID
        );
        lslIInput.desc().append_child("controllerTriggerPressed");
        lslIInput.desc().append_child("controllerPosition.x");
        lslIInput.desc().append_child("controllerPosition.y");
        lslIInput.desc().append_child("controllerPosition.z");
        lslIInput.desc().append_child("controllerRotation.x");
        lslIInput.desc().append_child("controllerRotation.y");
        lslIInput.desc().append_child("controllerRotation.z");
        lslIInput.desc().append_child("controllerScale.x");
        lslIInput.desc().append_child("controllerScale.y");
        lslIInput.desc().append_child("controllerScale.z");
        lslIInput.desc().append_child("leapIsGrasping");
        lslIInput.desc().append_child("leapGrabStrength");
        lslIInput.desc().append_child("leapGrabAngle");
        lslIInput.desc().append_child("leapPinchStrength");
        lslIInput.desc().append_child("leapHandPosition.x");
        lslIInput.desc().append_child("leapHandPosition.y");
        lslIInput.desc().append_child("leapHandPosition.z");
        lslIInput.desc().append_child("leapHandPalmPosition.x");
        lslIInput.desc().append_child("leapHandPalmPosition.y");
        lslIInput.desc().append_child("leapHandPalmPosition.z");
        lslIInput.desc().append_child("leapHandRotation.x");
        lslIInput.desc().append_child("leapHandRotation.y");
        lslIInput.desc().append_child("leapHandRotation.z");
        lslOInput = new liblsl.StreamOutlet(lslIInput);
    }
    
    #endregion
}