using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class TableConfigurationManager : MonoBehaviour
{
    
    public static TableConfigurationManager Instance { get; private set; }


    public GameObject Table;
    
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
        _tableConfigurationController.SetTable(Table);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    
   


    public void StartCalibration()
    {
       //rotation
       
       

        Vector3 positonGuess = Player.instance.feetPositionGuess;

        Quaternion playerRotation = Quaternion.Euler(Player.instance.transform.forward);
        
        var tablePosition = new Vector3(positonGuess.x, positonGuess.y, positonGuess.z + 1);
        
        _tableConfigurationController.SetRotation(playerRotation);

        _tableConfigurationController.SetPosition(tablePosition);
        
        
        
        // set the table centered based on X and Z of the player.
        //positionGuess x and Z
        
        
        //guess the height based on position guess Y and table height. potentially reset the pivot to the floor of the table 

        Vector3 sizes= new Vector3(length,height, depth);
       // _tableConfigurationController.SetScale(sizes);
    }
    
}
