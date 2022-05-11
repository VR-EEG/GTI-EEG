using System;
using UnityEngine;


    public class LslEventManager:MonoBehaviour
    {
        private NewExperimentManager _experimentManager;
        
        private EyetrackingValidation _eyetrackingValidation;
        
        private int currentTrialId;
        private int currentTrialTool;
        private int currentTrialOrientation;
        private int currentInstruction;

        private void Start()
        {
            _experimentManager = NewExperimentManager.Instance;

            _eyetrackingValidation = EyetrackingManagerNew.Instance.EyetrackingValidation;
            
            
            _experimentManager.OnTrialBegin += CollectTrialBeginInformation;
            _experimentManager.OnTrialEnd += CollectTrialEndTimeStamp;

            _experimentManager.OnBeepSound += CollectBeepSoundTimeStamp;
            _experimentManager.OnCueShowBegin += CollectCueShowBeginTimeStamp;
            _experimentManager.OnCueShowEnd += CollectCueShowEndTimeStamp;
            _experimentManager.OnToolIsShown += CollectToolShownTimeStamp;

            _eyetrackingValidation.BaseLineCheckStarted += CollectBaseLineStartTimeStamp;
            
            _eyetrackingValidation.BaseLineCheckEnded += CollectBaseLineEndTimeStamp;

        }

        private void CollectBaseLineStartTimeStamp()
        {
            double[] currentTimestamp =
            {
                TimeManager.Instance.GetCurrentUnixTimeStamp(),
            };
            LSLStreams.Instance.lslOBaselineBeginTimeStamp.push_sample(currentTimestamp);
        }
        
        private void CollectBaseLineEndTimeStamp()
        {
            double[] currentTimestamp =
            {
                TimeManager.Instance.GetCurrentUnixTimeStamp(),
            };
            LSLStreams.Instance.lslOBaselineEndTimeStamp.push_sample(currentTimestamp);
        }



        private void CollectTrialBeginInformation(int trialNumber, int toolID, int task, int orientation)
        {
            double[] currentTimestamp =
            {
                TimeManager.Instance.GetCurrentUnixTimeStamp(),
            };
            LSLStreams.Instance.lslOTrialStartMeasurementTimeStamp.push_sample(currentTimestamp);

            int[] trialData =
            {
                trialNumber,
                toolID,
                task,
                orientation
            };
            LSLStreams.Instance.lslOTrialInformationInt.push_sample(trialData);
        }

        private void CollectTrialEndTimeStamp()
        {
            double[] currentTimestamp = { TimeManager.Instance.GetCurrentUnixTimeStamp()};
            LSLStreams.Instance.lslOButtonPressedTimeStamp.push_sample(currentTimestamp);
        }

        
        private void CollectCueShowBeginTimeStamp()
        {
            double[] currentTimestamp = { TimeManager.Instance.GetCurrentUnixTimeStamp()};
            LSLStreams.Instance.lslOCueTimeStamp.push_sample(currentTimestamp);
        }
        
        private void CollectCueShowEndTimeStamp()
        {
            double[] currentTimestamp = { TimeManager.Instance.GetCurrentUnixTimeStamp()};
            LSLStreams.Instance.lslOCueDisappearedTimeStamp.push_sample(currentTimestamp);
        }

        private void CollectBeepSoundTimeStamp()
        {
            double[] currentTimestamp = { TimeManager.Instance.GetCurrentUnixTimeStamp()};
            LSLStreams.Instance.lslOBeepPlayedTimeStamp.push_sample(currentTimestamp);
        }
        
        private void CollectToolShownTimeStamp()
        {
            double[] currentTimestamp = { TimeManager.Instance.GetCurrentUnixTimeStamp()};
            LSLStreams.Instance.lslOObjectShownTimeStamp.push_sample(currentTimestamp);
        }

        private void GatherTrialInformation()
        {
            
        }


    }