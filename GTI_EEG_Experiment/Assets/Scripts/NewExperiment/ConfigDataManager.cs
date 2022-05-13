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
                var block = blocks[i];
                BlockInformation blockInformation;

                blockInformation.blockNumber = i;
                blockInformation.randomisationSeed = block.RandomisationSeed;
                blockInformation.numberOfTrials = block.TrailItems.Count;

                blockInformation.Trials = new List<TrialData>();
                foreach (var tuple in block.TrailItems)
                {
                    var trialData = new TrialData()
                    {
                        toolID = tuple.Item1,
                        taskID = tuple.Item2,
                        orientationID = tuple.Item3
                    };
                    blockInformation.Trials.Add(trialData);
                }
            }
        }


        public void SaveConfigurationFile(List<GameObject> tools, List<string> tasks, GameObject table, GameObject player)
        {
            ConfigurationData configurationData = new ConfigurationData();
            
            configurationData.Orientation = new List<string>() { "Congruent", "InCongruent" };

            configurationData.Cues = tasks;

            for (int i = 0; i < tools.Count; i++)
            {
                configurationData.MappedTool.Add(tools[i].gameObject.name);
                
                configurationData.ToolColliderCenter.Add(tools[i].GetComponent<ToolData>().ToolOversizedCollider.center);
                configurationData.ToolColliderExtends.Add(tools[i].GetComponent<ToolData>().ToolOversizedCollider.size);
            }


            var calibratedTablePositions = TableConfigurationManager.Instance.spawnAndButtonPosition;

            configurationData.ButtonPosition = calibratedTablePositions.GetChild(0).position;
            configurationData.Spawnpoint = calibratedTablePositions.GetChild(1).position;


            configurationData.roomPosition = TableConfigurationManager.Instance.Room.transform.position;
        }
        
    }


    public struct BlockInformation
    {
        public int blockNumber;
        public int randomisationSeed;
        public int numberOfTrials;
        public List<TrialData> Trials;
    }
    
    public class TrialData
    {
        public int toolID;
        public int taskID;
        public int orientationID;
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
        public Vector3 roomPosition;
    }
}