using System.Collections.Generic;
using UnityEngine;

namespace NewExperiment
{
    public class ConfigDataManager: MonoBehaviour
    {
        public static ConfigDataManager Instance { get ; private set; }
        
        
        
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            
        }





        public void SaveBlockInformation(List<BlockItem> blocks)
        {
            for (int i = 0; i < blocks.Count; i++)
            {
                
            }
        }
        
    }


    public struct BlockInformation
    {
        public int blockNumber;
        public int randomisationSeed;
        public int numberOfTrials;
        public List<TrialInformation> Trials;
    }
    
    public class TrialData
    {
        public int blockNumber;
        public int randomisationSeed;
        public int numberOfTrials;
    }

    public class ConfigurationData
    {
        public List<string> MappedTool;
        public List<string> Cues;
        public List<string> Orientation;
        public List<Vector3> ToolColliderExtends;
        public List<Vector3> ToolColliderCenter;
        public Vector3 PlayerPositionOffset;
        public Vector3 TableScale;
        public Vector3 Spawnpoint;
        public Vector3 ButtonPosition;
    }
}