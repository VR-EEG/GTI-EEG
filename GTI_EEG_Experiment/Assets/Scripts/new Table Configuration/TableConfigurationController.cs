using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class TableConfigurationController : MonoBehaviour
{
    private ObjectTransformHelper transformHelper;
    
    private GameObject _table;

    private Bounds _bounds;
    
    // Start is called before the first frame update
    void Start()
    {
        transformHelper = GetComponent<ObjectTransformHelper>();    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetBounds(GameObject table)
    {
        _bounds = transformHelper.GetBoundingBox(table);
    }
    
    public void SetPosition(Vector3 position)
    {
        _table.transform.position = position;
    }


    public void SetRotation(Quaternion rotation)
    {
        _table.transform.rotation = rotation;
    }

    public void SetTable(GameObject table)
    {
        _table = table;
    }



    public void SetScale(Vector3 scale)
    {
        transform.localScale = Vector3.one;


        var size = new Vector3(scale.x, scale.y, scale.z);

      
        
        //center position


        //_table.transform.position = new Vector3(playerPositionGuess.x, playerPositionGuess.y, playerPositionGuess.z+5);

        //table height Position calculation
        
        /*
        var tableGroundNiveau = Player.instance.feetPositionGuess.y;
        
        var tableSizeHeight = _bounds.extents.y * 2;

        var tablePlateheightPosition = tableGroundNiveau + tableSizeHeight;
        */
        
        
        










    }
}
