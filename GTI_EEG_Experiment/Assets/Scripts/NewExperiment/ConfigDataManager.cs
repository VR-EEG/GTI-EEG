using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

namespace NewExperiment
{
    public class ConfigDataManager: MonoBehaviour
    {
        public static ConfigDataManager Instance { get ; private set; }


        private Vector3 playerOffset;

        private List<GameObject> tools;
        private List<string> tasks;
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }


        public void SetPlayerOffset(Vector3 offset)
        {
            playerOffset = offset;
            SaveConfigurationFile();
        }


        public void SaveBlockInformation(List<BlockItem> blocks)
        {
            List<BlockInformation> blockInfoList= new List<BlockInformation>();
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
                
                blockInfoList.Add(blockInformation);
            }
            
            DataSavingManager.Instance.SaveList(blockInfoList,"gti_blockInfo_"+TimeManager.Instance.GetCurrentUnixTimeStamp(),true);
            
        }

        private void SaveConfigurationFile()
        {
            SaveConfigurationFile(tools,tasks);
        }
        
        public void SaveConfigurationFile(List<GameObject> tools, List<string> tasks)
        {
            this.tools = tools;
            this.tasks = tasks;
            ConfigurationData configurationData = new ConfigurationData();
            
            configurationData.Orientation = new List<string>() { "Congruent", "InCongruent" };

            configurationData.Cues = tasks;

            configurationData.MappedTool = new List<string>();
            configurationData.ToolColliderCenter = new List<Vector3>();
            configurationData.ToolColliderExtends = new List<Vector3>();

            for (int i = 0; i < tools.Count; i++)
            {
                configurationData.MappedTool.Add(i+ ":" +"'"+tools[i].gameObject.name+"'");
                
                BoxCollider collider = tools[i].GetComponent<ToolData>().GetToolOversizedCollider();
                if (collider != null)
                {
                    configurationData.ToolColliderCenter.Add(collider.center);
                    configurationData.ToolColliderExtends.Add(collider.size);
                }
                /*configurationData.ToolColliderCenter.Add(tools[i].GetComponent<ToolData>().GetToolOversizedCollider().center);
                configurationData.ToolColliderExtends.Add(tools[i].GetComponent<ToolData>().GetToolOversizedCollider().size);*/
            }


            var calibratedTablePositions = TableConfigurationManager.Instance.spawnAndButtonPosition;

            configurationData.GlobalSpawnAndButtonPosition = calibratedTablePositions.position;


            var localOffset = TableConfigurationManager.Instance.ButtonAndSpawnPointRelation();
            configurationData.ButtonPositionLocalOffset = localOffset.GetChild(0).localPosition;
            configurationData.SpawnpointLocalOffset = localOffset.GetChild(1).localPosition;


            configurationData.RoomPosition = TableConfigurationManager.Instance.Room.transform.position;
            
            configurationData.TableSizes = TableConfigurationManager.Instance.GetTableSizes();
            configurationData.TablePosition = TableConfigurationManager.Instance.GetTableSizes();


            configurationData.PlayerPositionOffset = playerOffset;
            
            DataSavingManager.Instance.Save(configurationData, "gti_config_"+TimeManager.Instance.GetCurrentUnixTimeStamp());
        }
        
    }


    public struct BlockInformation
    {
        public int blockNumber;
        public int randomisationSeed;
        public int numberOfTrials;
        public List<TrialData> Trials;
    }
    
    [Serializable] public class TrialData
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
        public Vector3 TableSizes;
        public Vector3 TablePosition;
        public Vector3 GlobalSpawnAndButtonPosition;
        public Vector3 SpawnpointLocalOffset;
        public Vector3 ButtonPositionLocalOffset;
        public Vector3 RoomPosition;
    }
}