using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using NewExperiment;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = System.Random;

public class NewExperimentManager : MonoBehaviour
{
    private ExperimentState _experimentState = ExperimentState.MainMenu;
    private TrailState _trailState = TrailState.Pause;
    

   
    [SerializeField] private int amountOfBlocks;
    [SerializeField] private List<GameObject> tools;
    [SerializeField] private List<string> ques;
    
    private string _participantID;
    private GameObject _currentTool;
    private List<BlockItem> _experimentBlocks;
    private BlockItem _currentBlock;
    private Tuple<int, int, int> _currentTrail;
    void Start()
    {
        _participantID = Randomization.GenerateID();
        _experimentBlocks = GenerateExperimentBlocks(amountOfBlocks);
        _currentBlock = _experimentBlocks[0];
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
        if (Input.GetKeyDown(KeyCode.A))
        {
            NextTrail();
            
        }
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

    public void AcceptButtonPress()
    {
        if (_experimentState == ExperimentState.Experiment)
        {
            if (_trailState == TrailState.EndOfTrail)
            {
                
            }
        }
        //Collect Time Stamp
    }

    private void NextTrail()
    {
        if (_experimentState == ExperimentState.Pause)
            return;
        
        if (_currentBlock.TrailItems.Count >1)
        {
            _currentTrail = null;
            _currentTrail = _currentBlock.TrailItems[1];
            _currentBlock.TrailItems.RemoveAt(0);
            
            
            Debug.Log(_currentTrail.Item1);
            
        }
        else
        {
            _experimentState = ExperimentState.Pause;
            Debug.Log("pause");
        }
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

enum TrailState
{
    Trail,
    EndOfTrail,
    Pause
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