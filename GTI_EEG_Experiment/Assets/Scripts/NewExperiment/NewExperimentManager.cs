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
    private TrialState _trialState = TrialState.StandBy;
    
    [SerializeField] private int amountOfBlocks;
    [SerializeField] private float trialDuration=3;
    [SerializeField] private List<GameObject> tools;
    [SerializeField] private List<string> ques;
    
    private string _participantID;
    private GameObject _currentTool;
    private List<BlockItem> _experimentBlocks;
    private BlockItem _currentBlock;
    private Tuple<int, int, int> _currentTrial;
    private int _trialCount;
    private int _blockCount;
    private BlockData _currentBlockData;

    private bool test;
    void Start()
    {
        _trialCount = 0;
        _blockCount = 0;
        _participantID = Randomization.GenerateID();
        _experimentBlocks = GenerateExperimentBlocks(amountOfBlocks);
        _currentBlock = _experimentBlocks[0];
        _currentTrial = _currentBlock.TrailItems[0];
        _currentBlockData = new BlockData();
    }
    
    private List<BlockItem> GenerateExperimentBlocks(int amount)
    {
       var  blocks = new List<BlockItem>(); 
        
        for (int i = 0; i < amount; i++)
        {
            var blockItem = new BlockItem
            {
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
            _experimentState = ExperimentState.BetweenTrials;
            if (!test)
            {
                test = true;
                StartNewBlock();
            }

            _experimentState = ExperimentState.Experiment;
            
            ButtonPress();
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

    public void ButtonPress()
    {
        if (_trialState == TrialState.StandBy&& _experimentState == ExperimentState.Experiment) //the very first trial
        {
            StartTrial();
        }
        else if(_trialState == TrialState.EndOfTrail)
        {
            NextTrial();
        }
    }

   

    private void StartTrial()
    {
        StartCoroutine(RunTrial(trialDuration));
    }

    private IEnumerator RunTrial(float trialDuration)
    {
        _trialState = TrialState.Trail;
        var beginTimeStamp = TimeManager.Instance.GetCurrentUnixTimeStamp();
        //Show Cue
        
        yield return new WaitForSeconds(trialDuration);
        
        var endTimeStamp = TimeManager.Instance.GetCurrentUnixTimeStamp();
        var trialInformation = new TrialInformation()
        {
            ToolId = _currentTrial.Item1,
            Que = _currentTrial.Item2,
            Orientation = _currentTrial.Item3,
            TrialNumber = _trialCount,
            TimeStampBegin = beginTimeStamp,
            TimeStampEnd = endTimeStamp
        };
        
        _currentBlockData.trialList.Add(trialInformation);
        _trialState = TrialState.EndOfTrail;
    }

    private void NextTrial()
    {
        if (_experimentState == ExperimentState.BetweenTrials)
            return;
        _trialState = TrialState.Trail;
        
        if (_currentBlock.TrailItems.Count -1  >0)
        {
            _currentTrial = null;
            _currentBlock.TrailItems.RemoveAt(0);
            _currentTrial = _currentBlock.TrailItems[0];
            _trialCount++;
            StartTrial();
        }
        else
        {
            FinalizeBlock();   
        }
    }

    private void StartNewBlock()
    {
        if (_experimentState == ExperimentState.BetweenTrials)
        {
            if (_experimentBlocks.Count - 1 > 0)
            {
                _currentBlock = null;
                _experimentBlocks.RemoveAt(0);
                _currentBlock = _experimentBlocks[0];
                _blockCount++;
                _currentBlockData = new BlockData();
                _currentBlockData.timeStampBegin = TimeManager.Instance.GetCurrentUnixTimeStamp();
                _currentBlockData.participantID = _participantID;
                _currentBlockData.index = _blockCount;
                _experimentState = ExperimentState.Experiment;
            }
        }
    }

    private void SaveBlock()
    {
        _currentBlockData.timeStampEnd = TimeManager.Instance.GetCurrentUnixTimeStamp();
        DataSavingManager.Instance.Save(_currentBlockData,_participantID +" - " +_blockCount );
    }

    private void FinalizeBlock()
    {
        _trialState = TrialState.StandBy;
        if (_experimentBlocks.Count - 1 > 0)
        {
            _experimentState = ExperimentState.BetweenTrials;
            SaveBlock();
            Debug.Log("pause");
        }
        else
        {
            SaveBlock();
            _experimentState = ExperimentState.Finished;
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

 public class BlockItem
{
    public int RandomisationSeed;
    public List<Tuple<int, int, int>> TrailItems;
}




enum TrialState
{
    Trail,
    EndOfTrail,
    StandBy
}





enum ExperimentState
{
    ParticipantID,
    MainMenu,
    Training,
    BetweenTrials,
    Experiment,
    Finished,
    TableCalibration,
    EyetrackingCalibration
}