using System;
using System.Collections;
using System.Collections.Generic;
using Leap.Unity;
using Leap.Unity.Interaction;
using UnityEngine;
using UnityEngine.Serialization;
using Valve.VR;
using Valve.VR.InteractionSystem;
using ViveSR.anipal.Eye;

public class LSLRecorder : MonoBehaviour
{
    public static LSLRecorder Instance { get; private set; }

    [SerializeField] private InteractionHand leapLeftInteractionHand;
    [SerializeField] private InteractionHand leapRightInteractionHand;
    [SerializeField] private Hand steamVrLeftHand;
    [SerializeField] private Hand steamVrRightHand;
    [SerializeField] private GameObject leapMainCamera;


    // SteamVR 
    public SteamVR_Action_Boolean steamVrAction;
    
    // Handedness in SteamVR format 
    private SteamVR_Input_Sources _handednessOfPlayerSteamVrFormat;

    private LSLDataFrame _lslDataFrame;
    private ConfigManager _configManager;
    
    // VR Glasses Transform 
    Transform _hmdTransform;

    private double _timestampBegin;
    private double _timestampEnd;

    private bool _recordLsl;

    // eyeTrackingGazeHMDFloat Stream values
    private float _eyeOpennessLeft;
    private float _eyeOpennessRight;
    private float _pupilDiameterMillimetersLeft;
    private float _pupilDiameterMillimetersRight;
    private Vector3 _eyePositionCombinedWorld;
    private Vector3 _eyeDirectionCombinedWorld;
    private Vector3 _eyePositionLeftWorld;
    private Vector3 _eyeDirectionLeftWorld;
    private Vector3 _eyePositionRightWorld;
    private Vector3 _eyeDirectionRightWorld;
  
    
    // input Stream values
    private float _controllerTriggerPressed;
    // private Transform _controllerTransform;
    private Vector3 _controllerPosition;
    private Vector3 _controllerRotation;
    private Vector3 _controllerScale;
    
    // LeapMotion Input 
    private float _leapIsGrasping;
    private float _leapGrabStrength;
    private float _leapGrabAngle; 
    private float _leapPinchStrength;
    private Vector3 _leapHandPosition;
    private Vector3 _leapHandPalmPosition;
    private Vector3 _leapHandRotation;
    
    private float _hitPointOnObjectCombinedEyesValidity;
    private float _hitPointOnObjectLeftEyeValidity;
    private float _hitPointOnObjectRightEyeValidity;
    private string _hitObjectNameCombinedEyes;
    private string _hitObjectNameLeftEye;
    private string _hitObjectNameRightEye;


    #region Private Methods

    private void Awake()
    {
        if (Instance == null) Instance = this;

        _hmdTransform = Player.instance.hmdTransform;
    }
    
    private void Start()
    {
        _configManager = GameObject.FindWithTag("ConfigManager").GetComponent<ConfigManager>();
    }

    private void FixedUpdate()
    {
        if (!_recordLsl) return;

        double[] currentTimestamp = { TimeManager.Instance.GetCurrentUnixTimeStamp()};
        
        LSLStreams.Instance.LslOEyetrackingFrameTimeStamp.push_sample(currentTimestamp);

        SRanipal_Eye_v2.GetVerboseData(out var verboseData); 

        var leftEyeData= verboseData.left;
        var rightEyeData = verboseData.right;
        
        var leftDataGazeOriginValidity = 
            leftEyeData.GetValidity(SingleEyeDataValidity.SINGLE_EYE_DATA_GAZE_ORIGIN_VALIDITY) ? 1 : 0;
        
        var leftDataGazeDirectionValidity = 
            leftEyeData.GetValidity(SingleEyeDataValidity.SINGLE_EYE_DATA_GAZE_DIRECTION_VALIDITY) ? 1 : 0;
        
        var leftDataPupilDiameterValidity = 
            leftEyeData.GetValidity(SingleEyeDataValidity.SINGLE_EYE_DATA_PUPIL_DIAMETER_VALIDITY) ? 1 : 0;
        
        var leftDataEyeOpennessValidity = 
            leftEyeData.GetValidity(SingleEyeDataValidity.SINGLE_EYE_DATA_EYE_OPENNESS_VALIDITY) ? 1 : 0;
        
        var leftDataPupilPositionInSensorAreaValidity = 
            leftEyeData.GetValidity(SingleEyeDataValidity.SINGLE_EYE_DATA_PUPIL_POSITION_IN_SENSOR_AREA_VALIDITY) ? 1 : 0;

        var leftAllValid = 
            leftDataGazeOriginValidity + 
            leftDataGazeDirectionValidity +
            leftDataPupilDiameterValidity +
            leftDataEyeOpennessValidity +
            leftDataPupilPositionInSensorAreaValidity == 5 ? 1 : 0;
        
        var leftOriginAndDirectionValid =
            leftDataGazeOriginValidity + 
            leftDataGazeDirectionValidity == 2 ? 1 : 0;
        
        var rightDataGazeOriginValidity = 
            rightEyeData.GetValidity(SingleEyeDataValidity.SINGLE_EYE_DATA_GAZE_ORIGIN_VALIDITY) ? 1 : 0;
        
        var rightDataGazeDirectionValidity = 
            rightEyeData.GetValidity(SingleEyeDataValidity.SINGLE_EYE_DATA_GAZE_DIRECTION_VALIDITY) ? 1 : 0;
        
        var rightDataPupilDiameterValidity = 
            rightEyeData.GetValidity(SingleEyeDataValidity.SINGLE_EYE_DATA_PUPIL_DIAMETER_VALIDITY) ? 1 : 0;
        
        var rightDataEyeOpennessValidity = 
            rightEyeData.GetValidity(SingleEyeDataValidity.SINGLE_EYE_DATA_EYE_OPENNESS_VALIDITY) ? 1 : 0;
        
        var rightDataPupilPositionInSensorAreaValidity = 
            rightEyeData.GetValidity(SingleEyeDataValidity.SINGLE_EYE_DATA_PUPIL_POSITION_IN_SENSOR_AREA_VALIDITY) ? 1 : 0;

        var rightAllValid = 
            rightDataGazeOriginValidity + 
            rightDataGazeDirectionValidity +
            rightDataPupilDiameterValidity +
            rightDataEyeOpennessValidity +
            rightDataPupilPositionInSensorAreaValidity == 5 ? 1 : 0;
        
        var rightOriginAndDirectionValid =
            rightDataGazeOriginValidity + 
            rightDataGazeDirectionValidity == 2 ? 1 : 0;
        
        int[] gazeDataValidity =
        {
            leftDataGazeOriginValidity,
            leftDataGazeDirectionValidity,
            leftDataPupilDiameterValidity,
            leftDataEyeOpennessValidity,
            leftDataPupilPositionInSensorAreaValidity,
            leftAllValid,
            leftOriginAndDirectionValid,
            rightDataGazeOriginValidity,
            rightDataGazeDirectionValidity,
            rightDataPupilDiameterValidity,
            rightDataEyeOpennessValidity,
            rightDataPupilPositionInSensorAreaValidity,
            rightAllValid,
            rightOriginAndDirectionValid
        };
        
        // Eye openness
        SRanipal_Eye_v2.GetEyeOpenness(EyeIndex.LEFT, out _eyeOpennessLeft);
        SRanipal_Eye_v2.GetEyeOpenness(EyeIndex.RIGHT, out _eyeOpennessRight);

        // Pupil Diameter
        _pupilDiameterMillimetersLeft = verboseData.left.pupil_diameter_mm;
        _pupilDiameterMillimetersRight = verboseData.right.pupil_diameter_mm;
        
        
        // Using Leap Motion    
        if (_configManager.isUsingLeap)
        {
            // Hands, dependent on handedness 
            Transform handTransform;
            Leap.Hand leapHand;
            InteractionHand leapUsedInteractionHand;
            
            // VR Glasses Transform
            _hmdTransform = leapMainCamera.transform;
            
            // Hand transform 
            leapUsedInteractionHand = 
                _handednessOfPlayerSteamVrFormat == SteamVR_Input_Sources.LeftHand 
                ? leapLeftInteractionHand 
                : leapRightInteractionHand;
            
            leapHand = leapUsedInteractionHand.leapHand;
            handTransform = leapUsedInteractionHand.transform;

            // In case of leap, set SteamVR values to default 
            _controllerTriggerPressed = 0;
            // _controllerTransform = null;
            _controllerPosition = Vector3.zero;
            _controllerRotation = Vector3.zero;
            _controllerScale = Vector3.zero;
            
            // Set leap specific values 
            _leapIsGrasping = leapUsedInteractionHand.isGraspingObject ? 1 : 0;
            _leapGrabStrength = leapHand.GrabStrength;
            _leapGrabAngle = leapHand.GrabAngle;
            _leapPinchStrength = leapHand.PinchStrength;
            _leapHandPosition = handTransform.position;
            _leapHandPalmPosition = leapHand.PalmPosition.ToVector3();
            _leapHandRotation = leapHand.Rotation.ToQuaternion().eulerAngles;
        }
        
        // Using SteamVR
        else
        {
            // Hand Transform depending on handedness
            Transform handTransform;
            handTransform = 
                _handednessOfPlayerSteamVrFormat == SteamVR_Input_Sources.LeftHand 
                    ? steamVrLeftHand.transform 
                    : steamVrRightHand.transform;

            // Set SteamVR specific values 
            _controllerTriggerPressed = 
                steamVrAction.state
                ? 1
                : 0;
            _controllerPosition = handTransform.position; 
            _controllerRotation = handTransform.rotation.eulerAngles; 
            _controllerScale = handTransform.lossyScale;
            
            // In case of SteamVR set Leap values to default
            _leapIsGrasping = 0;
            _leapGrabStrength = 0;
            _leapGrabAngle = 0;
            _leapPinchStrength = 0 ;
            _leapHandPosition = Vector3.zero;
            _leapHandPalmPosition = Vector3.zero;
            _leapHandRotation = Vector3.zero;
        }
        

        // origin has same value (in m) as verboseData.combined.eye_data.gaze_origin_mm (in mm)
        SRanipal_Eye_v2.GetGazeRay(GazeIndex.COMBINE, out var rayCombineEye); 
        
        _eyePositionCombinedWorld = _hmdTransform.position + rayCombineEye.origin; // ray origin is at transform of hmd + offset 
        _eyeDirectionCombinedWorld = _hmdTransform.rotation * rayCombineEye.direction; // ray direction is local, so multiply with hmd transform to get world direction 

        Bounds boundsCombinedEyes;
        if (Physics.Raycast(_eyePositionCombinedWorld, _eyeDirectionCombinedWorld, 
            out var hitPointOnObjectCombinedEyes, 10f))
        {
            _hitPointOnObjectCombinedEyesValidity = 1;
            boundsCombinedEyes = hitPointOnObjectCombinedEyes.collider.bounds;
            _hitObjectNameCombinedEyes = hitPointOnObjectCombinedEyes.collider.name;
        }
        else
        {
            _hitPointOnObjectCombinedEyesValidity = 0;
            boundsCombinedEyes = new Bounds(Vector3.zero, Vector3.zero);
            _hitObjectNameCombinedEyes = "";
        }


        // Get Left Eye Position and Gaze Direction 
        SRanipal_Eye_v2.GetGazeRay(GazeIndex.LEFT, out var rayLeftEye);
        _eyePositionLeftWorld = _hmdTransform.position + rayLeftEye.origin; // ray origin is at transform of hmd + offset 
        _eyeDirectionLeftWorld = _hmdTransform.rotation * rayLeftEye.direction; // ray direction is local, so multiply with hmd transform to get world direction 

        Bounds boundsLeftEye;
        if (Physics.Raycast(_eyePositionLeftWorld, _eyeDirectionLeftWorld, 
            out var hitPointOnObjectLeftEye, 10f))
        {
            _hitPointOnObjectLeftEyeValidity = 1;
            boundsLeftEye = hitPointOnObjectLeftEye.collider.bounds;
            _hitObjectNameLeftEye = hitPointOnObjectLeftEye.collider.name;
        }
        else
        {
            _hitPointOnObjectLeftEyeValidity = 0;
            boundsLeftEye = new Bounds(Vector3.zero, Vector3.zero);
            _hitObjectNameLeftEye = "";
        }


        // Get Right Eye Position and Gaze Direction
        SRanipal_Eye_v2.GetGazeRay(GazeIndex.RIGHT, out var rayRightEye);
        _eyePositionRightWorld = _hmdTransform.position + rayRightEye.origin; // ray origin is at transform of hmd + offset 
        _eyeDirectionRightWorld = _hmdTransform.rotation * rayRightEye.direction; // ray direction is local, so multiply with hmd transform to get world direction 

        Bounds boundsRightEye;
        if (Physics.Raycast(_eyePositionRightWorld, _eyeDirectionRightWorld, 
            out var hitPointOnObjectRightEye, 10f))
        {
            _hitPointOnObjectRightEyeValidity = 1;
            boundsRightEye = hitPointOnObjectRightEye.collider.bounds;
            _hitObjectNameRightEye = hitPointOnObjectRightEye.collider.name;
        }
        else
        {
            _hitPointOnObjectRightEyeValidity = 0;
            boundsRightEye = new Bounds(Vector3.zero, Vector3.zero);
            _hitObjectNameRightEye = "";
        }
        

        var hmdPos = _hmdTransform.position;
        var hmdForward = _hmdTransform.forward;
        var hmdRight = _hmdTransform.right;
        var hmdRot = _hmdTransform.rotation.eulerAngles;
        var hmdUp = _hmdTransform.up;

        float[] eyeTrackingGazeHmdFloat =
        {
            _eyeOpennessLeft,
            _eyeOpennessRight,
            _pupilDiameterMillimetersLeft,
            _pupilDiameterMillimetersRight,
            _eyePositionCombinedWorld.x,
            _eyePositionCombinedWorld.y,
            _eyePositionCombinedWorld.z,
            _eyeDirectionCombinedWorld.x,
            _eyeDirectionCombinedWorld.y,
            _eyeDirectionCombinedWorld.z,
            _eyePositionLeftWorld.x,
            _eyePositionLeftWorld.y,
            _eyePositionLeftWorld.z,
            _eyeDirectionLeftWorld.x,
            _eyeDirectionLeftWorld.y,
            _eyeDirectionLeftWorld.z,
            _eyePositionRightWorld.x,
            _eyePositionRightWorld.y,
            _eyePositionRightWorld.z,
            _eyeDirectionRightWorld.x,
            _eyeDirectionRightWorld.y,
            _eyeDirectionRightWorld.z,
            _hitPointOnObjectCombinedEyesValidity,
            hitPointOnObjectCombinedEyes.point.x,
            hitPointOnObjectCombinedEyes.point.y,
            hitPointOnObjectCombinedEyes.point.z,
            boundsCombinedEyes.center.x,
            boundsCombinedEyes.center.y,
            boundsCombinedEyes.center.z,
            _hitPointOnObjectLeftEyeValidity,
            hitPointOnObjectLeftEye.point.x,
            hitPointOnObjectLeftEye.point.y,
            hitPointOnObjectLeftEye.point.z,
            boundsLeftEye.center.x,
            boundsLeftEye.center.y,
            boundsLeftEye.center.z,
            _hitPointOnObjectRightEyeValidity,
            hitPointOnObjectRightEye.point.x,
            hitPointOnObjectRightEye.point.y,
            hitPointOnObjectRightEye.point.z,
            boundsRightEye.center.x,
            boundsRightEye.center.y,
            boundsRightEye.center.z,
            hmdPos.x,
            hmdPos.y,
            hmdPos.z,
            hmdForward.x,
            hmdForward.y,
            hmdForward.z,
            hmdRight.x,
            hmdRight.y,
            hmdRight.z,
            hmdRot.x,
            hmdRot.y,
            hmdRot.z,
            hmdUp.x,
            hmdUp.y,
            hmdUp.z
        };

        string[] eyeTrackingGazeHmdString =
        {
            _hitObjectNameCombinedEyes,
            _hitObjectNameLeftEye,
            _hitObjectNameRightEye
        };

        float[] input =
        {
            _controllerTriggerPressed,
            _controllerPosition.x,
            _controllerPosition.y,
            _controllerPosition.z,
            _controllerRotation.x,
            _controllerRotation.y,
            _controllerRotation.z,
            _controllerScale.x,
            _controllerScale.y,
            _controllerScale.z,
            _leapIsGrasping,
            _leapGrabStrength,
            _leapGrabAngle,
            _leapPinchStrength,
            _leapHandPosition.x,
            _leapHandPosition.y,
            _leapHandPosition.z,
            _leapHandPalmPosition.x,
            _leapHandPalmPosition.y,
            _leapHandPalmPosition.z,
            _leapHandRotation.x,
            _leapHandRotation.y,
            _leapHandRotation.z
        };

        SaveEyeTrackingData(eyeTrackingGazeHmdFloat, eyeTrackingGazeHmdString, gazeDataValidity);
        SaveInputs(input);
        
        // save current frame via LSL
        int[] currentFrame = {Time.frameCount};
        LSLStreams.Instance.lslOFrameTracking.push_sample(currentFrame);
    }
    
    private void SaveEyeTrackingData(float[] floatValues, string[] stringValues, int[] gazeValidity)
    {
        LSLStreams.Instance.lslOEyeTrackingGazeHMDFloat.push_sample(floatValues);
        LSLStreams.Instance.lslOEyeTrackingGazeHMDString.push_sample(stringValues);
        LSLStreams.Instance.lslOGazeValidity.push_sample(gazeValidity);
    }

    private void SaveInputs(float[] inputData)
    {
        LSLStreams.Instance.lslOInput.push_sample(inputData);
    }
    
    #endregion

    #region Setter Methods

    public void SetLSLRecordingStatus(bool state)
    {
        _recordLsl = state;
        leapMainCamera = MeasurementManager.Instance.leapMainCamera;
    }

    #endregion
}