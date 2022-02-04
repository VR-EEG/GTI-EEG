using System;
using System.Collections.Generic;
using UnityEngine;

namespace NewExperiment
{
    [Serializable] public class StaticExperimentData
    {
        [SerializeField] public  string participantID;
        [SerializeField] public List<TrialInformation> trailList;
        //Table Information
        [SerializeField] public  Vector3  tablePosition;
        [SerializeField] public  Vector3  tableRotation;
        [SerializeField]public float tableHeight;
        [SerializeField]public float tableLength;
        [SerializeField]public float tableDepth;
        [SerializeField] public  Vector3  buttonPosition;
        
        
    }

    [Serializable]
    public class TrialInformation
    {
        [SerializeField] public double TimeStampBegin;
        [SerializeField] public double TimeStampEnd;
        
        [SerializeField] public int TrialNumber;
        [SerializeField] public int ToolId;
        [SerializeField] public int Orientation;
        [SerializeField] public int Que;
    }
    
    [Serializable]
    public class BlockData
    {
        [SerializeField] public double  timeStampBegin;
        [SerializeField] public double  timeStampEnd;
        [SerializeField] public List<TrialInformation> trialList = new List<TrialInformation>();
    }
    
    
    
}