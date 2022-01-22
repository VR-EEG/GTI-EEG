using System;
using System.Collections;
using System.Collections.Generic;
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
    public GameObject Table;
    public GameObject Room;
    public GameObject Button;
    public GameObject CueText;
    public Transform ButtonPosition;

    [SerializeField] private float Speed;
    [SerializeField] private Vector2 ButtonOffsetToPlayer;
    
    
    
    private TableConfigurationController _tableConfigurationController;
    private TableCalibrationUI _tableCalibrationUI;


    private float _depth;
    private float _length;
    private float _height;

    private Vector3 _tablePosition;

    private bool _isActive;

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

    }
    private void Update()
    {
        _tablePosition = Table.transform.position;
        if (Input.GetKeyDown(KeyCode.N))
        {
            SetActive(!_isActive);
           _isActive = !_isActive;
        }
    }


    public void MoveLeft(SteamVR_Action_Boolean leftInput, SteamVR_Input_Sources fromSource)
    {
        _controllerIsOverriding = true;
        Vector3 inputDirection = Vector3.left;
        MoveTable(inputDirection);
    }
    
    public void MoveRight(SteamVR_Action_Boolean rightInput, SteamVR_Input_Sources fromSource)
    {
        _controllerIsOverriding = true;
        Vector3 inputDirection = Vector3.right;
        MoveTable(inputDirection);
    }
    
    public void MoveForward(SteamVR_Action_Boolean forwardInput, SteamVR_Input_Sources fromSource)
    {
        _controllerIsOverriding = true;
        Vector3 inputDirection = Vector3.forward;
        MoveTable(inputDirection);
    }
    
    public void MoveBackward(SteamVR_Action_Boolean backwardInput, SteamVR_Input_Sources fromSource)
    {
        _controllerIsOverriding = true;
        Vector3 inputDirection = Vector3.back;
        MoveTable(inputDirection);
    }
    
    public void MoveUp(SteamVR_Action_Boolean upInput, SteamVR_Input_Sources fromSource)
    {
        _controllerIsOverriding = true;
        Vector3 inputDirection = Vector3.up;
        MoveTable(inputDirection);
    }
    
    public void MoveDown(SteamVR_Action_Boolean downInput, SteamVR_Input_Sources fromSource)
    {
        _controllerIsOverriding = true;
        Vector3 inputDirection = Vector3.down;
        MoveTable(inputDirection);
    }

    public bool ControllerIsOverriding()
    {
        return _controllerIsOverriding;
    }

    public void SetControllerIsOverriding(bool state)
    {
        _controllerIsOverriding = state;
    }

    public void SetActive(bool state)
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
    }
    
    public void MoveTable(Vector3 direction)
    {
        Debug.Log("move " + direction);
        Table.transform.position += direction*Speed*Time.deltaTime;
        Room.transform.position = Table.transform.position;

    }

    public void MoveButton(Vector3 direction)
    {
        Button.transform.position += direction;
    }

    public void SetTablePosition(Vector3 position)
    {
        Table.transform.position = position;
        
        Room.transform.position = Table.transform.position;
        
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
        _tableCalibrationUI = GetComponent<TableCalibrationUI>();
        _tableConfigurationController = GetComponent<TableConfigurationController>();
        
        _tableCalibrationUI.SetActive(true);
        
        Debug.Assert(Table!=null, "table was not assigned");
        Debug.Assert(Room!=null, "room is not assigned");
        _tableConfigurationController.Init(Table, Room,Button, ButtonPosition);
        
        MoveLeftInput.AddOnStateDownListener(MoveLeft,SteamVR_Input_Sources.Any);
        MoveRightInput.AddOnStateDownListener(MoveRight,SteamVR_Input_Sources.Any);
        MoveFowardInput.AddOnStateDownListener(MoveForward,SteamVR_Input_Sources.Any);
        MoveBackwardInput.AddOnStateDownListener(MoveBackward,SteamVR_Input_Sources.Any);
        MoveUpwardInput.AddOnStateDownListener(MoveUp,SteamVR_Input_Sources.Any);
        MoveDownWardInput.AddOnStateDownListener(MoveDown,SteamVR_Input_Sources.Any);
        var x =  PlayerPrefs.GetFloat("TableX");
        var y = PlayerPrefs.GetFloat("TableY");
        var z = PlayerPrefs.GetFloat("TableZ");
        _tablePosition = new Vector3(x, y, z);
        SetTablePosition(_tablePosition);

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
        
    }

    public void AutoCalibrateButtonPosition()
    {
        Vector3 positonGuess = Player.instance.feetPositionGuess;


        Quaternion rot = _tableConfigurationController.GetTableRotation();

        var offset = ButtonOffsetToPlayer;



        Vector3 pos = new Vector3(positonGuess.x+offset.x, _height, positonGuess.z+offset.y);

        pos.x += ButtonOffsetToPlayer.x;
        pos.z += ButtonOffsetToPlayer.y;

        Button.transform.position = pos;
        Button.transform.rotation = rot;


    }
    
    public void SetTableScale(float length, float height, float depth )
    {
        _length = length/100;
        _height = height/100;
        _depth = depth/100;
        _tableConfigurationController.SetScale(length,height,depth);
        
    }
    
    
    
    
    
}
