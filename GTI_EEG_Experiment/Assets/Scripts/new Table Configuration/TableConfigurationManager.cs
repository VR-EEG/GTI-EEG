using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class TableConfigurationManager : MonoBehaviour
{
    
    public static TableConfigurationManager Instance { get; private set; }


    public GameObject Table;

    public GameObject Room;
    
    private TableConfigurationController _tableConfigurationController;
    private TableCalibrationUI _tableCalibrationUI;

    private float depth;
    private float length;
    private float height;

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
    
    // Start is called before the first frame update
    void Start()
    {
        _tableCalibrationUI = GetComponent<TableCalibrationUI>();
        _tableConfigurationController = GetComponent<TableConfigurationController>();
        
        _tableCalibrationUI.SetActive(true);
        
        Debug.Assert(Table!=null, "table was not assigned");
        Debug.Assert(Room!=null, "room is not assigned");
        _tableConfigurationController.Init(Table, Room);
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
