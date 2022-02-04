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
    private BlockData _currentBlockData;
    void Start()
    {
        _trialCount = 0;
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
        else
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
        if (_trialState != TrialState.EndOfTrail)
            return;
        if (_experimentState == ExperimentState.Pause)
            return;
        
        if (_currentBlock.TrailItems.Count >1)
        {
            _currentTrial = null;
            _currentTrial = _currentBlock.TrailItems[1];
            _currentBlock.TrailItems.RemoveAt(0);
            _trialCount++;
            StartTrial();
        }
        else
        {
            _experimentState = ExperimentState.Pause;
            _trialState = TrialState.StandBy;
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

 public class BlockItem
{
    public int RandomisationSeed;
    public string ParticipantID;
    public int BlockNumber;
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
    Pause,
    Experiment,
    TableCalibration,
    EyetrackingCalibration
}