using System.Collections;
using UnityEngine;
using Valve.VR.InteractionSystem;
using ViveSR.anipal.Eye;

namespace LSL
{
    public class newLSLRecorder: MonoBehaviour
    {
        
        public LayerMask ignoredLayers;
        
        private Transform _hmd;
        private Hand _hand; // there is only a right hand in this experiment
        private Transform _currentTaskObject;

        private bool _isRecording;
        private bool _toolIsAssigned;
        
        private Coroutine _recordingCouroutine;


        public void StartRecording()
        {
            _recordingCouroutine= StartCoroutine(Recording());
        }

        public void StopRecording()
        {
            _isRecording = false;
        }

        public void Init()
        {
            //Get References in scene, that are active
            _hmd = Player.instance.hmdTransform;
            _hand = Player.instance.rightHand;
        }
        
        public void AssignNewTaskObject(GameObject TaskObject)
        {
            _currentTaskObject = TaskObject.transform;
            _toolIsAssigned = true;
        }

        public void ExpelAssginedObject()
        {
            _toolIsAssigned = false;
            _currentTaskObject = null;
        }
        
        private IEnumerator Recording()
        {
            if (_isRecording)
                yield return null;

            _isRecording = true;
            
            while (_isRecording)
            { 
                var hmdPos = _hmd.position;
                var hmdRot = _hmd.rotation;
                var hmdRotEuler = hmdRot.eulerAngles;
                var hmdForward = _hmd.forward;
                var handTransform = _hand.transform;
                var handPos = handTransform.position;
                var handRot = handTransform.rotation.eulerAngles;
                
                
                double[] currentTimestamp = { TimeManager.Instance.GetCurrentUnixTimeStamp()};
                
                
                SRanipal_Eye_v2.GetVerboseData(out var verboseData);
                
                SRanipal_Eye_v2.GetGazeRay(GazeIndex.COMBINE, out var rayCombineEye);
                
                var combinedEyeData =  verboseData.combined.eye_data;
                float combinedValidityBitMask = combinedEyeData.eye_data_validata_bit_mask;
                
                
                Vector3 coordinateAdaptedGazeDirectionCombined = new Vector3(verboseData.combined.eye_data.gaze_direction_normalized.x * -1,  verboseData.combined.eye_data.gaze_direction_normalized.y, verboseData.combined.eye_data.gaze_direction_normalized.z);
                var eyePositionCombinedWorld = hmdPos + combinedEyeData.gaze_origin_mm/1000;
                var eyeDirectionCombinedWorld = hmdRot * coordinateAdaptedGazeDirectionCombined;
                var eyePositionCombinedLocal = combinedEyeData.gaze_origin_mm/1000;
                var eyeDirectionCombinedLocal = coordinateAdaptedGazeDirectionCombined;

                SRanipal_Eye_v2.GetGazeRay(GazeIndex.LEFT, out var rayLeftEye);
                
                var leftEyeData =  verboseData.left;
                float leftEyeDataValidityBitMask = leftEyeData.eye_data_validata_bit_mask;
                var leftOpenness = leftEyeData.eye_openness;
                
                Vector3 coordinateAdaptedGazeDirectionLeft = new Vector3(verboseData.left.gaze_direction_normalized.x * -1,  verboseData.left.gaze_direction_normalized.y, verboseData.left.gaze_direction_normalized.z);
                var eyePositionLeftWorld = hmdPos + leftEyeData.gaze_origin_mm/1000;
                var eyeDirectionLeftWorld = hmdRot * coordinateAdaptedGazeDirectionLeft;
                var eyePositionLeftLocal = leftEyeData.gaze_origin_mm/1000;
                var eyeDirectionLeftLocal = coordinateAdaptedGazeDirectionLeft;

                
                SRanipal_Eye_v2.GetGazeRay(GazeIndex.RIGHT, out var rayRightEye);
                
                var rightEyeData =  verboseData.right;
                float rightEyeDataValidityBitMask = rightEyeData.eye_data_validata_bit_mask;
                var rightOpenness = rightEyeData.eye_openness;
                
                Vector3 coordinateAdaptedGazeDirectionRight = new Vector3(verboseData.right.gaze_direction_normalized.x * -1,  verboseData.right.gaze_direction_normalized.y, verboseData.right.gaze_direction_normalized.z);
                var eyePositionRightWorld = hmdPos + rightEyeData.gaze_origin_mm/1000;
                var eyeDirectionRightWorld = hmdRot * coordinateAdaptedGazeDirectionRight;
                var eyePositionRightLocal = rightEyeData.gaze_origin_mm/1000;
                var eyeDirectionRightLocal = coordinateAdaptedGazeDirectionRight;
                
                //tool object
                var taskObjectPosition = new Vector3();
                var taskObjectRotation = new Vector3();
                var taskObjectIsInHand = 0f; 
            
                if (_toolIsAssigned)
                {
                    taskObjectPosition = _currentTaskObject.position;
                    taskObjectRotation = _currentTaskObject.rotation.eulerAngles;
                    taskObjectIsInHand = _hand.ObjectIsAttached(_currentTaskObject.gameObject)?1:0;
                }
                
                // Raycast

                var taskObjectWasHit=0f;
                var targetHitPosition= new Vector3();
                var targetPosition= new Vector3();
                if (Physics.Raycast(eyePositionCombinedWorld, eyeDirectionCombinedWorld,
                        out var hitInfo, 10f, ~ignoredLayers))
                {
                    if (_toolIsAssigned)
                    {
                        if (hitInfo.collider == _currentTaskObject.GetComponent<ToolData>().ToolOversizedCollider)
                        {
                            taskObjectWasHit=1f;
                            Debug.Log("was hit"+_currentTaskObject.gameObject);
                        }
                    }
                    targetHitPosition = hitInfo.point;
                    targetPosition = hitInfo.collider.transform.position;
                }
                
                //fill lsl data
                float[] liveDataFrame =
                {
                    //HMD releated
                    hmdPos.x,
                    hmdPos.y,
                    hmdPos.z,
                
                    hmdRotEuler.x,
                    hmdRotEuler.y,
                    hmdRotEuler.z,
                
                    hmdForward.x,
                    hmdForward.y,
                    hmdForward.z,

                    //hand related
                    handPos.x,
                    handPos.y,
                    handPos.z,
                
                    handRot.x,
                    handRot.y,
                    handRot.z,
                
                    //combined gaze information
                    combinedValidityBitMask,
                
                    eyePositionCombinedWorld.x,
                    eyePositionCombinedWorld.y,
                    eyePositionCombinedWorld.z,
                
                    eyeDirectionCombinedWorld.x,
                    eyeDirectionCombinedWorld.y,
                    eyeDirectionCombinedWorld.z,
                
                    eyePositionCombinedLocal.x,
                    eyePositionCombinedLocal.y,
                    eyePositionCombinedLocal.z,
                
                    eyeDirectionCombinedLocal.x,
                    eyeDirectionCombinedLocal.y,
                    eyeDirectionCombinedLocal.z,
                
                    //left eye information
                
                    leftEyeDataValidityBitMask,
                    leftOpenness,
                
                    eyePositionLeftWorld.x,
                    eyePositionLeftWorld.y,
                    eyePositionLeftWorld.z,
                
                    eyeDirectionLeftWorld.x,
                    eyeDirectionLeftWorld.y,
                    eyeDirectionLeftWorld.z,
                
                    eyePositionLeftLocal.x,
                    eyePositionLeftLocal.y,
                    eyePositionLeftLocal.z,
                
                    eyeDirectionLeftLocal.x,
                    eyeDirectionLeftLocal.y,
                    eyeDirectionLeftLocal.z,
                
                    //right eye information
                    rightEyeDataValidityBitMask,
                    rightOpenness,
                
                    eyePositionRightWorld.x,
                    eyePositionRightWorld.y,
                    eyePositionRightWorld.z,
                
                    eyeDirectionRightWorld.x,
                    eyeDirectionRightWorld.y,
                    eyeDirectionRightWorld.z,
                
                    eyePositionRightLocal.x,
                    eyePositionRightLocal.y,
                    eyePositionRightLocal.z,
                
                    eyeDirectionRightLocal.x,
                    eyeDirectionRightLocal.y,
                    eyeDirectionRightLocal.z,
                
                    //task object
                
                    taskObjectPosition.x,
                    taskObjectPosition.y,
                    taskObjectPosition.z,
                
                    taskObjectRotation.x,
                    taskObjectRotation.y,
                    taskObjectRotation.z,
                
                    taskObjectIsInHand,
                
                    //raycast
                
                    taskObjectWasHit,
                
                    targetHitPosition.x,
                    targetHitPosition.y,
                    targetHitPosition.z,
                
                    targetPosition.x,
                    targetPosition.y,
                    targetPosition.z,
                };
            
                LSLStreams.Instance.LslOEyetrackingFrameTimeStamp.push_sample(currentTimestamp);
                LSLStreams.Instance.lslOEyeTrackingGazeHMDFloat.push_sample(liveDataFrame);
            
                yield return new WaitForEndOfFrame();
            }
            
         
            
           
            
          
        }
    }
}