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

    [SerializeField] private float Speed;
    
    private TableConfigurationController _tableConfigurationController;
    private TableCalibrationUI _tableCalibrationUI;
    

    private float depth;
    private float length;
    private float height;

    private Vector2 horizontalMovement;

    private bool _isActive;

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
        if (Input.GetKeyDown(KeyCode.N))
        {
            Debug.Log("test");
           SetActive(!_isActive);
           _isActive = !_isActive;
        }
    }


    public void MoveLeft(SteamVR_Action_Boolean leftInput, SteamVR_Input_Sources fromSource)
    {
        Vector3 inputDirection = Vector3.left;
        MoveTable(inputDirection);
    }
    
    public void MoveRight(SteamVR_Action_Boolean rightInput, SteamVR_Input_Sources fromSource)
    {
        Vector3 inputDirection = Vector3.right;
        MoveTable(inputDirection);
    }
    
    public void MoveForward(SteamVR_Action_Boolean forwardInput, SteamVR_Input_Sources fromSource)
    {
        Vector3 inputDirection = Vector3.forward;
        MoveTable(inputDirection);
    }
    
    public void MoveBackward(SteamVR_Action_Boolean backwardInput, SteamVR_Input_Sources fromSource)
    {
        Vector3 inputDirection = Vector3.back;
        MoveTable(inputDirection);
    }
    
    public void MoveUp(SteamVR_Action_Boolean upInput, SteamVR_Input_Sources fromSource)
    {
        Vector3 inputDirection = Vector3.up;
        MoveTable(inputDirection);
    }
    
    public void MoveDown(SteamVR_Action_Boolean downInput, SteamVR_Input_Sources fromSource)
    {
        Vector3 inputDirection = Vector3.down;
        MoveTable(inputDirection);
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
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
        _tableCalibrationUI = GetComponent<TableCalibrationUI>();
        _tableConfigurationController = GetComponent<TableConfigurationController>();
        
        _tableCalibrationUI.SetActive(true);
        
        Debug.Assert(Table!=null, "table was not assigned");
        Debug.Assert(Room!=null, "room is not assigned");
        _tableConfigurationController.Init(Table, Room);
        
        MoveLeftInput.AddOnStateDownListener(MoveLeft,SteamVR_Input_Sources.Any);
        MoveRightInput.AddOnStateDownListener(MoveRight,SteamVR_Input_Sources.Any);
        MoveFowardInput.AddOnStateDownListener(MoveForward,SteamVR_Input_Sources.Any);
        MoveBackwardInput.AddOnStateDownListener(MoveBackward,SteamVR_Input_Sources.Any);
        MoveUpwardInput.AddOnStateDownListener(MoveUp,SteamVR_Input_Sources.Any);
        MoveDownWardInput.AddOnStateDownListener(MoveDown,SteamVR_Input_Sources.Any);
    }
    
    
    

    public void AutoCalibrateTablePosition()
    {
        
        Vector3 positonGuess = Player.instance.feetPositionGuess;

        Quaternion playerRotation = Quaternion.Euler(Player.instance.transform.forward);
        
        var tablePosition = new Vector3(positonGuess.x, positonGuess.y, positonGuess.z + 1);
        
        _tableConfigurationController.SetRotation(playerRotation);

        _tableConfigurationController.SetPosition(tablePosition);
        
    }


    public void SetTableScale(float length, float height, float depth )
    {
        
        _tableConfigurationController.SetScale(length,height,depth);
    }
    
}
