using System;
using UnityEngine;


    public class LslEventManager:MonoBehaviour
    {
        private NewExperimentManager _experimentManager;
        
        private int currentTrialId;
        private int currentTrialTool;
        private int currentTrialOrientation;
        private int currentInstruction;

        private void Start()
        {
            _experimentManager = NewExperimentManager.Instance;
            
            
            _experimentManager.OnTrialBegin += CollectTrialBeginTimeStamp;
            _experimentManager.OnTrialEnd += CollectTrialEndTimeStamp;

            _experimentManager.OnBeepSound += CollectBeepSoundTimeStamp;
            _experimentManager.OnCueShowBegin += CollectCueShowBeginTimeStamp;
            _experimentManager.OnCueShowEnd += CollectCueShowEndTimeStamp;
            _experimentManager.OnToolIsShown += CollectToolShownTimeStamp;
            
        }

        
        private void CollectTrialBeginTimeStamp(int trialNumber, int toolID, int task, int orientation)
        {
            double[] currentTimestamp =
            {
                TimeManager.Instance.GetCurrentUnixTimeStamp(),
                trialNumber,
                toolID,
                task,
                orientation
            };
            LSLStreams.Instance.lslOTrialStartMeasurementTimeStamp.push_sample(currentTimestamp);
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