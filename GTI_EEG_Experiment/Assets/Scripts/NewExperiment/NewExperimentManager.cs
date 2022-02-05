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
    public static NewExperimentManager Instance { get ; set; }
    
    
    private ExperimentState _experimentState = ExperimentState.MainMenu;
    private TrialState _trialState = TrialState.StandBy;
    
    [SerializeField] private int amountOfBlocks;
    [SerializeField] private float trialDuration=3;
    [SerializeField] private List<GameObject> tools;
    [SerializeField] private List<string> Tasks;
    
    private string _participantID;
    private GameObject _currentTool;
    private List<BlockItem> _experimentBlocks;
    private BlockItem _currentBlock;
    private BlockItem _tutorialBlock;
    
    private Tuple<int, int, int> _currentTrial;
    private int _trialCount;
    private int _blockCount;
    private BlockData _currentBlockData;

    private bool test;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    void Start()
    {
        _trialCount = 1;
        _blockCount = 1;
        _participantID = Randomization.GenerateID();
        _tutorialBlock = GenerateTutorialBlock();
        _experimentBlocks = GenerateExperimentBlocks(amountOfBlocks);
        _currentBlockData = new BlockData();
    }


    private BlockItem GenerateTutorialBlock()
    {
        var tutorialBlock = new BlockItem()
        {
            RandomisationSeed = 0,
            TrailItems = new List<Tuple<int, int, int>>()
        };
        for (var k = 0; k < Tasks.Count; k++)
        {
            var congruentOrientation = new Tuple<int, int, int>(0, k, 0); // only tutorial hammer is available here
            tutorialBlock.TrailItems.Add(congruentOrientation);
            var incongruentOrientation= new Tuple<int, int, int>(0, k, 1);
            tutorialBlock.TrailItems.Add(incongruentOrientation);
        }

        return tutorialBlock;
    }
    
    private List<BlockItem> GenerateExperimentBlocks(int amount)
    {
       var  blocks = new List<BlockItem>(); 
        
        for (var blockIndex = 0; blockIndex < amount; blockIndex++) // first tool which is not the tutorial hammer
        {
            var blockItem = new BlockItem
            {
                RandomisationSeed = Randomization.GenerateID().GetHashCode(),
                TrailItems=new List<Tuple<int, int, int>>()
            };
            var trailItems = new List<Tuple<int, int, int>>();
            for (var toolIndex = 1; toolIndex < tools.Count; toolIndex++)
            {
                for (var taskIndex = 0; taskIndex < Tasks.Count; taskIndex++)
                {
                    var congruentOrientation = new Tuple<int, int, int>(toolIndex, taskIndex, 0);
                    trailItems.Add(congruentOrientation);
                    var incongruentOrientation= new Tuple<int, int, int>(toolIndex, taskIndex, 1);
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
            ButtonPress();
        }
    }


    public ExperimentState GetExperimentState()
    {
        return _experimentState;
    }
    

    public void StartExperiment()
    {
        _experimentState = ExperimentState.Experiment;
        _trialState = TrialState.StandBy;
        _currentBlock = _experimentBlocks[0];
        _currentTrial = _currentBlock.TrailItems[0];
    }

    public void ContinueExperiment()
    {
        StartNewBlock();
    }
    
    public void StartTutorial()
    {
        _experimentState = ExperimentState.Training;
        _trialState = TrialState.StandBy;
        _currentTrial = _tutorialBlock.TrailItems[0];
        _currentBlock = _tutorialBlock;
    }


    public void StartTableCalibration()
    {
        
    }

    public void StartEyeCalibration()
    {
        
    }

    public void ButtonPress()
    {
        switch (_experimentState)
        {
            case ExperimentState.MainMenu:
            case ExperimentState.BetweenBlocks:
            case ExperimentState.Finished:
                return;
            case ExperimentState.Training:
                if(_trialState==TrialState.StandBy||_trialState == TrialState.EndOfTrail)
                    NextTutorialTrial();
                break;
            case ExperimentState.Experiment:
                if (_trialState == TrialState.StandBy) //the very first trial
                    StartTrial();
                else if(_trialState == TrialState.EndOfTrail)
                    NextTrial();
                break;
        }
    }

   

    private void StartTrial()
    {
        StartCoroutine(RunTrial(trialDuration));
    }

    private IEnumerator RunTrial(float trialDuration,bool isTutorial=false)
    {
        _trialState = TrialState.Trail;
        var beginTimeStamp = TimeManager.Instance.GetCurrentUnixTimeStamp();
        //Show Cue
        
        yield return new WaitForSeconds(0f);
        Debug.Log(_currentTrial.Item1 +" "+ _currentTrial.Item2 +" "+ _currentTrial.Item3);
        
        if (!isTutorial)
        {
            var endTimeStamp = TimeManager.Instance.GetCurrentUnixTimeStamp();
            var trialInformation = new TrialInformation()
            {
                ToolId = _currentTrial.Item1,
                Task = _currentTrial.Item2,
                Orientation = _currentTrial.Item3,
                TrialNumber = _trialCount,
                TimeStampBegin = beginTimeStamp,
                TimeStampEnd = endTimeStamp
            };
        
            _currentBlockData.trialList.Add(trialInformation);
        }
        
        _trialState = TrialState.EndOfTrail;
    }
    private void NextTutorialTrial()
    {
        if (_experimentState == ExperimentState.BetweenBlocks)
            return;
        _trialState = TrialState.Trail;

        var tmp = _tutorialBlock.TrailItems[0];
        _tutorialBlock.TrailItems.RemoveAt(0);
        _tutorialBlock.TrailItems.Add(tmp);
        _currentTrial = _tutorialBlock.TrailItems[0];

        StartCoroutine(RunTrial(trialDuration, true));
    }
    private void NextTrial()
    {
        if (_experimentState == ExperimentState.BetweenBlocks)
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
        if (_experimentState == ExperimentState.BetweenBlocks)
        {
            if (amountOfBlocks-_blockCount > 0)
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
        if (amountOfBlocks-_blockCount > 0)
        {
            _experimentState = ExperimentState.BetweenBlocks;
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





public enum ExperimentState
{
    ParticipantID,
    MainMenu,
    ReadyForTraining,
    Training,
    BetweenBlocks,
    Experiment,
    Finished,
    TableCalibration,
    EyetrackingCalibration
}