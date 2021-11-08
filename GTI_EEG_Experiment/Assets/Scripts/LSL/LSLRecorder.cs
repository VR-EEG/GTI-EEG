using System;
using System.Collections;
using System.Collections.Generic;
using Leap.Unity.Interaction;
using UnityEngine;
using UnityEngine.Serialization;
using Valve.VR.InteractionSystem;
using ViveSR.anipal.Eye;

public class LSLRecorder : MonoBehaviour
{
    public static LSLRecorder Instance { get; private set; }

    [SerializeField] private ConfigManager configManager;
    [SerializeField] private GameObject leapMainCamera;
    [SerializeField] private InteractionHand leapLeftInteractionHand;
    [SerializeField] private InteractionHand leapRightInteractionHand;
    
    
    private LSLDataFrame _lslDataFrame;

    private double _timestampBegin;
    private double _timestampEnd;

    private bool _recordLsl;
    
    // toolCueOrientationInt Stream variables
    private int _trialId;
    private int _blockId;
    private int _utcon;
    private int _toolId;
    private int _cueOrientationId;
    private int _toolIsCurrentlyAttachedToHand;
    private int _toolIsCurrentlyDisplayedOnTable;

    // toolCueOrientationString Stream values
    private string _toolName;
    private string _cueOrientationName; 
    private string _cueName;
    private string _toolHandleOrientation;
    private string _closestAttachmentPointOnToolToHand;
    
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
            _toolId,
            _cueOrientationId,
            _toolIsCurrentlyAttachedToHand,
            _toolIsCurrentlyDisplayedOnTable
        };

        string[] toolCueOrientationString =
        {
            _toolName,
            _cueOrientationName,
            _cueName,
            _toolHandleOrientation,
            _closestAttachmentPointOnToolToHand
        };

        // Eye openness
        SRanipal_Eye_v2.GetEyeOpenness(EyeIndex.LEFT, out _eyeOpennessLeft);
        SRanipal_Eye_v2.GetEyeOpenness(EyeIndex.RIGHT, out _eyeOpennessRight);

        // Pupil Diameter
        SRanipal_Eye_v2.GetVerboseData(out var verboseData); 
        _pupilDiameterMillimetersLeft = verboseData.left.pupil_diameter_mm;
        _pupilDiameterMillimetersRight = verboseData.right.pupil_diameter_mm;
        
        Transform hmdTransform;
                
        // Using Leap Motion    
        if (configManager.isUsingLeap)
        {
            // Hands, dependent on handedness 
            Transform handTransform;
            Leap.Hand leapHand;
            InteractionHand leapUsedInteractionHand;
            
            // VR Glasses Transform
            hmdTransform = leapMainCamera.transform;
            
            // Hand transform 
            /*if (handednessOfPlayerSteamVrFormat == SteamVR_Input_Sources.LeftHand)
            {
                leapUsedInteractionHand = leapLeftInteractionHand; // left hand
            }
            else
            {
                leapUsedInteractionHand = leapRightInteractionHand; // right hand (right or ambidextrous) 
            }
            leapHand = leapUsedInteractionHand.leapHand;
            handTransform = leapUsedInteractionHand.transform;

            // In case of leap, set SteamVR values to default 
            dataPoint.controllerTriggerPressed = false;
            dataPoint.controllerTransform = null;
            dataPoint.controllerPosition = Vector3.zero;
            dataPoint.controllerRotation = Vector3.zero;
            dataPoint.controllerScale = Vector3.zero;
            
            // Set leap specific values 
            dataPoint.leapIsGrasping = leapUsedInteractionHand.isGraspingObject;
            dataPoint.leapGrabStrength = leapHand.GrabStrength;
            dataPoint.leapGrabAngle = leapHand.GrabAngle;
            dataPoint.leapPinchStrength = leapHand.PinchStrength;
            dataPoint.leapHandPosition = handTransform.position;
            dataPoint.leapHandPalmPosition = leapHand.PalmPosition.ToVector3();
            dataPoint.leapHandRotation = leapHand.Rotation.ToQuaternion().eulerAngles;*/
        }
        
        // Using SteamVR
        else
        {
            // Hand Transform depending on handedness
            /*Transform handTransform;
            if (handednessOfPlayerSteamVrFormat == SteamVR_Input_Sources.LeftHand)
            {
               handTransform = steamVrLeftHand.transform; // left hand
            }
            else
            {
                handTransform = steamVrRightHand.transform; // right hand
            }*/

            // VR Glasses Transform 
            hmdTransform = Player.instance.hmdTransform;
            
            /*// Set SteamVR specific values 
            dataPoint.controllerTriggerPressed = steamVrAction.state;
            dataPoint.controllerTransform = handTransform; 
            dataPoint.controllerPosition = handTransform.position; 
            dataPoint.controllerRotation = handTransform.rotation.eulerAngles; 
            dataPoint.controllerScale = handTransform.lossyScale;
            
            // In case of SteamVR set Leap values to default
            dataPoint.leapIsGrasping = false;
            dataPoint.leapGrabStrength = 0;
            dataPoint.leapGrabAngle = 0;
            dataPoint.leapPinchStrength = 0 ;
            dataPoint.leapHandPosition = Vector3.zero;
            dataPoint.leapHandPalmPosition = Vector3.zero;
            dataPoint.leapHandRotation = Vector3.zero;*/
        }
        

        // origin has same value (in m) as verboseData.combined.eye_data.gaze_origin_mm (in mm)
        SRanipal_Eye_v2.GetGazeRay(GazeIndex.COMBINE, out var rayCombineEye); 
        _eyePositionCombinedWorld = hmdTransform.position + rayCombineEye.origin; // ray origin is at transform of hmd + offset 
        _eyeDirectionCombinedWorld = hmdTransform.rotation * rayCombineEye.direction; // ray direction is local, so multiply with hmd transform to get world direction 

        RaycastHit hitPointOnObjectCombinedEyes;
        Physics.Raycast(_eyePositionCombinedWorld, _eyeDirectionCombinedWorld, out hitPointOnObjectCombinedEyes, 10f);
        var boundsCombinedEyes = hitPointOnObjectCombinedEyes.collider.bounds;

        // Get Left Eye Position and Gaze Direction 
        SRanipal_Eye_v2.GetGazeRay(GazeIndex.LEFT, out var rayLeftEye);
        _eyePositionLeftWorld = hmdTransform.position + rayLeftEye.origin; // ray origin is at transform of hmd + offset 
        _eyeDirectionLeftWorld = hmdTransform.rotation * rayLeftEye.direction; // ray direction is local, so multiply with hmd transform to get world direction 

        RaycastHit hitPointOnObjectLeftEye;
        Physics.Raycast(_eyePositionLeftWorld, _eyeDirectionLeftWorld, out hitPointOnObjectLeftEye, 10f);
        var boundsLeftEye = hitPointOnObjectLeftEye.collider.bounds;

        
        // Get Right Eye Position and Gaze Direction
        SRanipal_Eye_v2.GetGazeRay(GazeIndex.RIGHT, out var rayRightEye);
        _eyePositionRightWorld = hmdTransform.position + rayRightEye.origin; // ray origin is at transform of hmd + offset 
        _eyeDirectionRightWorld = hmdTransform.rotation * rayRightEye.direction; // ray direction is local, so multiply with hmd transform to get world direction 

        RaycastHit hitPointOnObjectRightEye;
        Physics.Raycast(_eyePositionRightWorld, _eyeDirectionRightWorld, out hitPointOnObjectRightEye, 10f);
        var boundsRightEye = hitPointOnObjectRightEye.collider.bounds;


        var hmdPos = hmdTransform.position;
        var hmdForward = hmdTransform.forward;
        var hmdRight = hmdTransform.right;
        var hmdRot = hmdTransform.rotation.eulerAngles;
        var hmdUp = hmdTransform.up;

        float[] eyeTrackingGazeHMDFloat =
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
            hitPointOnObjectCombinedEyes.point.x,
            hitPointOnObjectCombinedEyes.point.y,
            hitPointOnObjectCombinedEyes.point.z,
            boundsCombinedEyes.center.x,
            boundsCombinedEyes.center.y,
            boundsCombinedEyes.center.z,
            hitPointOnObjectLeftEye.point.x,
            hitPointOnObjectLeftEye.point.y,
            hitPointOnObjectLeftEye.point.z,
            boundsLeftEye.center.x,
            boundsLeftEye.center.y,
            boundsLeftEye.center.z,
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

        string[] eyeTrackingGazeHMDString =
        {
            hitPointOnObjectCombinedEyes.collider.name,
            hitPointOnObjectLeftEye.collider.name,
            hitPointOnObjectRightEye.collider.name
        };

        // todo gather lslIInput 26 float


        SaveTimeStamps(timestamps);
        SaveToolCueOrientation(toolCueOrientationInt, toolCueOrientationString);
        SaveEyeTrackingData(eyeTrackingGazeHMDFloat, eyeTrackingGazeHMDString);
        
        // todo push lslIEyeTrackingGazeHMDFloat 55 float
        // todo push lslIEyeTrackingGazeHMDString 3 string

        // todo push lslIInput 26 float
    }

    
    private void SaveTimeStamps(double[] timestamps)
    {
        LSLStreams.Instance.lslOTimeStamps.push_sample(timestamps);
    }

    private void SaveToolCueOrientation(int[] toolCueOrientationInt, string[] toolCueOrientationString)
    {
        LSLStreams.Instance.lslOToolCueOrientationInt.push_sample(toolCueOrientationInt);
        LSLStreams.Instance.lslOToolCueOrientationString.push_sample(toolCueOrientationString);
    }
    
    private void SaveEyeTrackingData(float[] floatValues, string[] stringValues)
    {
        LSLStreams.Instance.lslOEyeTrackingGazeHMDFloat.push_sample(floatValues);
        LSLStreams.Instance.lslOEyeTrackingGazeHMDString.push_sample(stringValues);
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
    
    public void SetUtcon(int utcon, int cueOrientationId,
        string cueOrientationName, string cueName)
    {
        _utcon = utcon;
        _cueOrientationId = cueOrientationId;
        _cueOrientationName = cueOrientationName;
        _cueName = cueName;
    }

    public void SetToolInfo(int attachedToHand, int onTable, int toolId,
        string toolName, string toolHandleOrientation,
        string closestAttachmentPointOnToolToHand)
    {
        _toolIsCurrentlyAttachedToHand = attachedToHand;
        _toolIsCurrentlyDisplayedOnTable = onTable;
        _toolId = toolId;
        _toolName = toolName;
        _toolHandleOrientation = toolHandleOrientation;
        _closestAttachmentPointOnToolToHand = closestAttachmentPointOnToolToHand;
    }
    
    
    #endregion
}