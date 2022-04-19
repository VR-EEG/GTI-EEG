using System.Collections;
using Leap.Unity.Infix;
using UnityEngine;
using Valve.VR.InteractionSystem;
using ViveSR.anipal.Eye;

namespace LSL
{
    public class newLSLRecorder: MonoBehaviour
    {
        
        //TODO assign these via Init method
        private Transform HMD;
        private Hand Hand; // there is only a right hand in this experiment
        private Transform CurrentTaskObject;


        public void Init()
        {
            //Get References in scene
        }
        
        public void AssignNewTaskObject(GameObject TaskObject)
        {
            CurrentTaskObject = TaskObject.transform;
        }
        
        private IEnumerator StartRecording()
        {
            var hmdPos = HMD.position;
            var hmdRot = HMD.rotation.eulerAngles;
            var hmdForward = HMD.forward;
            
            var handTransform = Hand.transform;
            var handPos = handTransform.position;
            var handRot = handTransform.rotation.eulerAngles;
            
            
            SRanipal_Eye_v2.GetVerboseData(out var verboseData);
            
            SRanipal_Eye_v2.GetGazeRay(GazeIndex.COMBINE, out var rayCombineEye);
            
            var combinedEyeData =  verboseData.combined.eye_data;
            float combinedValidityBitMask = combinedEyeData.eye_data_validata_bit_mask;
            
            var eyePositionCombinedWorld = hmdPos + rayCombineEye.origin;
            var eyeDirectionCombinedWorld = hmdPos + rayCombineEye.direction;
            var eyePositionCombinedLocal = rayCombineEye.origin;
            var eyeDirectionCombinedLocal = rayCombineEye.direction;

            SRanipal_Eye_v2.GetGazeRay(GazeIndex.LEFT, out var rayLeftEye);
            
            var leftEyeData =  verboseData.left;
            float leftEyeDataValidityBitMask = leftEyeData.eye_data_validata_bit_mask;
            var leftOpenness = leftEyeData.eye_openness;

            var eyePositionLeftWorld = hmdPos + rayLeftEye.origin;
            var eyeDirectionLeftWorld = hmdPos + rayLeftEye.direction;
            var eyePositionLeftLocal = rayLeftEye.origin;
            var eyeDirectionLeftLocal = rayLeftEye.direction;

            
            SRanipal_Eye_v2.GetGazeRay(GazeIndex.RIGHT, out var rayRightEye);
            
            var rightEyeData =  verboseData.right;
            float rightEyeDataValidityBitMask = rightEyeData.eye_data_validata_bit_mask;
            var rightOpenness = rightEyeData.eye_openness;
            
            var eyePositionRightWorld = hmdPos + rayRightEye.origin;
            var eyeDirectionRightWorld = hmdPos + rayRightEye.direction;
            var eyePositionRightLocal = rayRightEye.origin;
            var eyeDirectionRightLocal = rayRightEye.direction;
            
            //tool object

            var taskObjectPosition = CurrentTaskObject.position;
            var taskObjectRotation = CurrentTaskObject.rotation.eulerAngles;
            var taskObjectIsInHand = Hand.ObjectIsAttached(CurrentTaskObject.gameObject) ? 1f : 0f;
            
            
            
            // Raycast

            var taskObjectWasHit=0f;
            var targetHitPosition= new Vector3();
            var targetPosition= new Vector3();
            if (Physics.Raycast(eyePositionCombinedWorld, eyeDirectionCombinedWorld,
                    out var hitInfo, 10f))
            {
                if (hitInfo.collider.gameObject == CurrentTaskObject.gameObject)
                {
                    taskObjectWasHit=1f;
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
                
                hmdRot.x,
                hmdRot.y,
                hmdRot.z,
                
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
            
            LSLStreams.Instance.lslOFrameTracking.push_sample(liveDataFrame);
      
           


            yield return new WaitForEndOfFrame();
        }
    }
}