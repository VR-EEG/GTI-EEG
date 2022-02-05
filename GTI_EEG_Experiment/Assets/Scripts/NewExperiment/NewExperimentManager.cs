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

    [SerializeField] private TextController _textController;

    private ExperimentState _experimentState = ExperimentState.MainMenu;
    private TrialState _trialState = TrialState.StandBy;
    
    [SerializeField] private int amountOfBlocks;
    [SerializeField] private float trialDuration=3;
    [SerializeField] private List<GameObject> tools;
    [SerializeField] private List<string> tasks;
    
    private string _participantID;
    private GameObject _currentTool;
    private List<BlockItem> _experimentBlocks;
    private BlockItem _currentBlock;
    private BlockItem _tutorialBlock;
    
    private Tuple<int, int, int> _currentTrial;
    private int _trialCount;
    private int _blockCount;
    private BlockData _currentBlockData;
    
    //instruction texts
    private string welcomeText = "Welcome to \n the Experiment!";
    private string instructionText = "Interact with\ntrigger to begin.";
    private string tutorialText = "This is the\npractice section.";
    private string breakText = "This is the\npractice section.";
    private string endText = "Experiment complete. \n Thank you!";

    private bool _trialCompleted;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    void Start()
    {
        Debug.Assert(tasks.Count>0, "Task list is empty");
        _trialCount = 0;    //will be added at first trial
        _blockCount = 1;
        _participantID = Randomization.GenerateID();
        _tutorialBlock = GenerateTutorialBlock();
        _experimentBlocks = GenerateExperimentBlocks(amountOfBlocks);
        _currentBlockData = new BlockData();
        _textController.ShowText(welcomeText);
    }


    private BlockItem GenerateTutorialBlock()
    {
        var tutorialBlock = new BlockItem()
        {
            RandomisationSeed = 0,
            TrailItems = new List<Tuple<int, int, int>>()
        };
        for (var k = 0; k < tasks.Count; k++)
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
                for (var taskIndex = 0; taskIndex < tasks.Count; taskIndex++)
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
        StartCoroutine(ShowInstructionsSucessively());
    }

    private IEnumerator ShowInstructionsSucessively()
    {
        while (_trialState == TrialState.StandBy)
        {
            if(_trialState == TrialState.StandBy)
                _textController.ShowText(tutorialText);
            yield return new WaitForSeconds(2);
            if(_trialState == TrialState.StandBy)
                _textController.ShowText(instructionText);
            yield return new WaitForSeconds(2);
        }
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
                if(_trialState==TrialState.StandBy||_trialState == TrialState.EndOfTrial)
                    NextTutorialTrial();
                break;
            case ExperimentState.Experiment:
                if (_trialState == TrialState.StandBy)
                {
                    //the very first trial
                    _trialCount++;
                    StartTrial();
                }
                else if(_trialState == TrialState.EndOfTrial)
                    NextTrial();
                break;
        }
    }
    
    private void StartTrial()
    {
        StartCoroutine(RunTrial(trialDuration));
    }

    private void ShowTool(int toolId, int direction)
    {
        if (direction == 1)
            tools[toolId].transform.rotation *= Quaternion.Euler(0,180,0);
        tools[toolId].SetActive(true);
    }

    private void HideTool(int toolId)
    {
        tools[toolId].SetActive(false);
    }

    private IEnumerator RunTrial(float trialDuration,bool isTutorial=false)
    {
        _trialCompleted = false;
        _trialState = TrialState.Trial;
        var beginTimeStamp = TimeManager.Instance.GetCurrentUnixTimeStamp();
        yield return new WaitForSeconds(0.5f);
        _textController.ShowText(tasks[_currentTrial.Item2],true);
        yield return new WaitForSeconds(2f);
        _textController.ShowText("",true);
        yield return new WaitForSeconds(0.5f);
        ShowTool(_currentTrial.Item1,_currentTrial.Item3);
        yield return new WaitForSeconds(3f);
        //beep
        
        //TODO button click ends and time stamp is given

        
        _trialState = TrialState.EndOfTrial;

        yield return new WaitUntil(() => _trialCompleted);
        if (isTutorial) yield break;
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
   
    private void NextTrial()
    {
        if (_experimentState == ExperimentState.BetweenBlocks)
            return;
        _trialState = TrialState.Trial;
        
        if (_currentBlock.TrailItems.Count -1  >0)
        {
            _trialCompleted = true;
           HideTool(_currentTrial.Item1);
           
            StartTrial();
        }
        else
        {
            HideTool(_currentTrial.Item1);
            FinalizeBlock();   
        }
    }
    
    private void NextTutorialTrial()
    {
        if (_experimentState == ExperimentState.BetweenBlocks)
            return;
        _trialState = TrialState.Trial;

        var tmp = _tutorialBlock.TrailItems[0];
        _tutorialBlock.TrailItems.RemoveAt(0);
        _tutorialBlock.TrailItems.Add(tmp);
        _currentTrial = _tutorialBlock.TrailItems[0];

        StartCoroutine(RunTrial(trialDuration, true));
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

}

 public class BlockItem
{
    public int RandomisationSeed;
    public List<Tuple<int, int, int>> TrailItems;
}




enum TrialState
{
    Trial,
    EndOfTrial,
    StandBy
}





public enum ExperimentState
{
    MainMenu,
    Training,
    BetweenBlocks,
    Experiment,
    Finished,
    TableCalibration,
    EyetrackingCalibration
}