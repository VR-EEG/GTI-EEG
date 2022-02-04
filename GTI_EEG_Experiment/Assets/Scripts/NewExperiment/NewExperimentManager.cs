using System;
using System.Collections;
using System.Collections.Generic;
using NewExperiment;
using UnityEngine;
using Random = System.Random;

public class NewExperimentManager : MonoBehaviour
{
    private ExperimentState _experimentState = ExperimentState.MainMenu;
    

   
    [SerializeField] private int amountOfBlocks;
    [SerializeField] private List<GameObject> tools;
    [SerializeField] private List<string> ques;
    
    private Tuple<int, int, int > _currentTrailItem;
    private string _participantID;
    private GameObject _currentTool;
    private List<BlockItem> _experimentBlocks;
    void Start()
    {
        _participantID = Randomization.GenerateID();
        _experimentBlocks = GenerateExperimentBlocks(amountOfBlocks);
    }
    
    private List<BlockItem> GenerateExperimentBlocks(int amount)
    {
       var  blocks = new List<BlockItem>(); 
        
        for (int i = 0; i < amount; i++)
        {
            var blockItem = new BlockItem
            {
                ParticipantID = _participantID,
                BlockNumber = i,
                RandomisationSeed = Randomization.GenerateID().GetHashCode(),
                TrailItems=new List<Tuple<int, int, int>>()
            };
            var trailItems = new List<Tuple<int, int, int>>();
            for (var j = 0; j < tools.Count; j++)
            {
                for (var k = 0; k < ques.Count; k++)
                {
                    var congruentOrientation = new Tuple<int, int, int>(j, k, 0);
                    trailItems.Add(congruentOrientation);
                    var incongruentOrientation= new Tuple<int, int, int>(j, k, 1);
                    trailItems.Add(incongruentOrientation);
                }
            }

            blockItem.TrailItems = Randomization.Shuffle(trailItems, new Random(blockItem.RandomisationSeed));
            blocks.Add(blockItem);
        }

        return blocks;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RandomizeForParticipant()
    {
        
    }


    public void StartExperiment()
    {
        
    }


    public void StartTableCalibration()
    {
        
    }

    public void StartEyeCalibration()
    {
        
    }


    private IEnumerator ExperimentStart()
    {
        //Get Trail data 
        while (_experimentState== ExperimentState.Experiment)
        {
            //if trail data is empty 
            yield return new WaitForEndOfFrame();
        }

    }
    
}

[Serializable] public class BlockItem
{
    public int RandomisationSeed;
    public string ParticipantID;
    public int BlockNumber;
    public List<Tuple<int, int, int>> TrailItems;
}



enum ExperimentState
{
    ParticipantID,
    MainMenu,
    Training,
    Pause,
    Experiment,
    TableCalibration,
    EyetrackingCalibration
}