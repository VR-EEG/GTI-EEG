using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class TableConfigurationManager : MonoBehaviour
{
    
    public static TableConfigurationManager Instance { get; private set; }
    
    
    public SteamVR_ActionSet CalibrationActionSet;
    public SteamVR_Action_Boolean MoveLeftInput;
    public SteamVR_Action_Boolean MoveRightInput;
    public SteamVR_Action_Boolean MoveFowardInput;
    public SteamVR_Action_Boolean MoveBackwardInput;
    public SteamVR_Action_Boolean MoveUpwardInput;
    public SteamVR_Action_Boolean MoveDownWardInput;
    public SteamVR_Action_Boolean SelectInput;
    public GameObject Table;
    public GameObject Room;
    public GameObject Button;
    public GameObject CueText;
    public Transform spawnAndButtonPosition;

    [SerializeField] private float Speed;
    [SerializeField] private float SpawnPointOffset;
    [SerializeField] private ButtonConfiguration _buttonConfiguration;



    private TableConfigurationController _tableConfigurationController;
    private TableCalibrationUI _tableCalibrationUI;

    private int _selectIndex;


    private float _depth;
    private float _length;
    private float _height;

    private Vector3 _tablePosition;

    private bool _isActive;

    private bool _tableIsActive;
    private bool _buttonIsActive;

    private bool _controllerIsOverriding;

    private void Awake()
    {
        //singleton pattern a la Unity
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);    
        }
        else
        {
            Destroy(gameObject);
        }

        _selectIndex = 0;
    }
    
    
    

    private void MoveObject(Vector3 direction)
    {
        if (_buttonIsActive)
        {
            spawnAndButtonPosition.parent = null;
            MoveButton(direction);
        }
            
        if(_tableIsActive)
            MoveTable(direction);
        
        spawnAndButtonPosition.parent = Table.transform;

    }

    public void MoveLeft(SteamVR_Action_Boolean leftInput, SteamVR_Input_Sources fromSource)
    {
        _controllerIsOverriding = true;
        Vector3 inputDirection = Vector3.left;
        MoveObject(inputDirection);
    }
    
    public void MoveRight(SteamVR_Action_Boolean rightInput, SteamVR_Input_Sources fromSource)
    {
        _controllerIsOverriding = true;
        Vector3 inputDirection = Vector3.right;
        MoveObject(inputDirection);
    }
    
    public void MoveForward(SteamVR_Action_Boolean forwardInput, SteamVR_Input_Sources fromSource)
    {
        _controllerIsOverriding = true;
        Vector3 inputDirection = Vector3.forward;
        MoveObject(inputDirection);
    }
    
    public void MoveBackward(SteamVR_Action_Boolean backwardInput, SteamVR_Input_Sources fromSource)
    {
        _controllerIsOverriding = true;
        Vector3 inputDirection = Vector3.back;
        MoveObject(inputDirection);
    }
    
    public void MoveUp(SteamVR_Action_Boolean upInput, SteamVR_Input_Sources fromSource)
    {
        _controllerIsOverriding = true;
        Vector3 inputDirection = Vector3.up;
        MoveObject(inputDirection);
    }
    
    public void MoveDown(SteamVR_Action_Boolean downInput, SteamVR_Input_Sources fromSource)
    {
        _controllerIsOverriding = true;
        Vector3 inputDirection = Vector3.down;
        MoveObject(inputDirection);
    }
    
    public void Select(SteamVR_Action_Boolean selectInput, SteamVR_Input_Sources fromSource)
    {
        _controllerIsOverriding = true;
        _selectIndex++;

        if (_selectIndex > 2)
        {
            _selectIndex = 0;
        }
        Debug.Log(_selectIndex);
        SelectbyIndex(_selectIndex);
    }

    private void SelectbyIndex(int i)
    {
        switch (i)
        {
            case 0: 
                _tableIsActive = true;
                _buttonIsActive = false;
                NewExperimentManager.Instance.ShowText("Table");
                break;
            case 1:
                _tableIsActive = false;
                _buttonIsActive = true;
                NewExperimentManager.Instance.ShowText("Button");
                break;
            case 2:
                _tableIsActive = true;
                _buttonIsActive = true;
                NewExperimentManager.Instance.ShowText("Both");
                break;
        }
    }

    public bool ControllerIsOverriding()
    {
        return _controllerIsOverriding;
    }

    public void SetControllerIsOverriding(bool state)
    {
        _controllerIsOverriding = state;
    }


    public void SaveTablePosition()
    {
        _tablePosition = Table.transform.position;
        PlayerPrefs.SetFloat("TableX",_tablePosition.x);
        PlayerPrefs.SetFloat("TableY",_tablePosition.y);
        PlayerPrefs.SetFloat("TableZ",_tablePosition.z);
        
        SaveButtonPosition();
    }



    public void SetHeight(float height)
    {
        _height = height;
    }

    public void SetLength(float length)
    {
        _length = length;
    }

    public void SetDepth(float depth)
    {
        _depth = depth;
    }
    
    public void SaveTableSize()
    {
        PlayerPrefs.SetFloat("TableLength", _length);
        PlayerPrefs.SetFloat("TableHeight", _height);
        PlayerPrefs.SetFloat("TableDepth", _depth);
    }

    public void SaveButtonPosition()
    {
        var pos = spawnAndButtonPosition.transform.position;
        PlayerPrefs.SetFloat("ButtonX", pos.x);
        PlayerPrefs.SetFloat("ButtonY", pos.y);
        PlayerPrefs.SetFloat("ButtonZ", pos.z);
    }
    
    


    public void StartSetup()
    {
        SetActive(true);
    }

    public void CloseSetup()
    {
        SetActive(false);
        if (NewExperimentManager.Instance.GetFormerExperimentState() == ExperimentState.BetweenBlocks)
        {
            NewExperimentManager.Instance.SetBetweenBlocks();
        }
        else
        {
            NewExperimentManager.Instance.SetMainMenu();
        }

    }
    
    private void SetActive(bool state)
    {
        _tableCalibrationUI.SetActive(state);
        
        if (state)
        {
            CalibrationActionSet.Activate();
        }
        else
        {
            CalibrationActionSet.Deactivate();
        }
        
        _buttonConfiguration.DisplaySpawnZone(state);
    }
    
    public void MoveTable(Vector3 direction)
    {
        Debug.Log("move " + direction);
        Table.transform.position += direction*Speed*Time.deltaTime;
        Room.transform.position = Table.transform.position;

    }

    public void MoveButton(Vector3 direction)
    {
        spawnAndButtonPosition.transform.position += direction*Speed*Time.deltaTime;
        _buttonConfiguration.AdjustPosition();
    }

    public void SetTablePosition(Vector3 position, bool withoutButton = false)
    {
        Table.transform.position = position;
        if (!withoutButton)
        {
            _buttonConfiguration.AdjustPosition();
        }
        Room.transform.position = Table.transform.position;
        
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
        _tableCalibrationUI = GetComponent<TableCalibrationUI>();
        _tableConfigurationController = GetComponent<TableConfigurationController>();
        
        _tableCalibrationUI.SetActive(false);
        
        Debug.Assert(Table!=null, "table was not assigned");
        Debug.Assert(Room!=null, "room is not assigned");
        _tableConfigurationController.Init(Table, Room);
        
        MoveLeftInput.AddOnStateDownListener(MoveLeft,SteamVR_Input_Sources.Any);
        MoveRightInput.AddOnStateDownListener(MoveRight,SteamVR_Input_Sources.Any);
        MoveFowardInput.AddOnStateDownListener(MoveForward,SteamVR_Input_Sources.Any);
        MoveBackwardInput.AddOnStateDownListener(MoveBackward,SteamVR_Input_Sources.Any);
        MoveUpwardInput.AddOnStateDownListener(MoveUp,SteamVR_Input_Sources.Any);
        MoveDownWardInput.AddOnStateDownListener(MoveDown,SteamVR_Input_Sources.Any);
        SelectInput.AddOnStateDownListener(Select,SteamVR_Input_Sources.Any);
        
        var x =  PlayerPrefs.GetFloat("TableX");
        var y = PlayerPrefs.GetFloat("TableY");
        var z = PlayerPrefs.GetFloat("TableZ");
        _tablePosition = new Vector3(x, y, z);

        
        
        
        
        SetTablePosition(_tablePosition, false);
        
        
        
        
        if (PlayerPrefs.HasKey("TableLength"))
        {
            _length = PlayerPrefs.GetFloat("TableLength");
        }
        
        if (PlayerPrefs.HasKey("TableHeight"))
        {
            _height = PlayerPrefs.GetFloat("TableHeight");
        }
        
        if(PlayerPrefs.HasKey("TableDepth"))
        {
            _depth = PlayerPrefs.GetFloat("TableDepth");
        }
        
        if (_length != 0 && _height != 0 && _depth != 0)
        {
          //  SetTableScale(_length,_height,_depth);
        }
        
        
        var xbutton = PlayerPrefs.GetFloat("ButtonX");
        var ybutton = PlayerPrefs.GetFloat("ButtonY");
        var zbutton = PlayerPrefs.GetFloat("ButtonZ");
        //spawnAndButtonPosition.transform.position = new Vector3(xbutton, ybutton, zbutton); ;
        _buttonConfiguration.AdjustPosition();

        
        
        

    }

    public Vector3 GetTablePosition()
    {
        return _tablePosition;
    }
    

    public void AutoCalibrateTablePosition()
    {
        
        Vector3 positonGuess = Player.instance.feetPositionGuess;

        Quaternion playerRotation = Quaternion.Euler(Player.instance.transform.forward);
        
        var tablePosition = new Vector3(positonGuess.x, positonGuess.y, positonGuess.z +_depth/2);
        
        _tableConfigurationController.SetRotation(playerRotation);

        _tableConfigurationController.SetPosition(tablePosition);
        
        _buttonConfiguration.AdjustPosition();
        
    }

    public void AutoCalibrateButtonPosition()
    {
        Vector3 positonGuess = Player.instance.feetPositionGuess;


        Quaternion rot = _tableConfigurationController.GetTableRotation();
        


      
        var pos = new Vector3(positonGuess.x, positonGuess.y + _height ,positonGuess.z+SpawnPointOffset);
        spawnAndButtonPosition.position = pos;
        _buttonConfiguration.AdjustPosition();
        

    }
    
    public void SetTableScale(float length, float height, float depth )
    {
        _length = length/100;
        _height = height/100;
        _depth = depth/100;
        _tableConfigurationController.SetScale(length,height,depth);
        _buttonConfiguration.AdjustPosition();
    }
    
    
    
    
    
}
