using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class TableConfigurationController : MonoBehaviour
{
    private ObjectTransformHelper transformHelper;
    
    private GameObject _table;
    private GameObject _room;
    private Bounds _bounds;
    
    // Start is called before the first frame update


    private void Awake()
    {
        transformHelper = GetComponent<ObjectTransformHelper>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Quaternion GetTableRotation()
    {
        return _table.transform.rotation;
    }
    
    public void SetPosition(Vector3 position)
    {
        _table.transform.position = position;
        _room.transform.position = position;
    }


    public void SetRotation(Quaternion rotation)
    {
        _table.transform.rotation = rotation;
        _room.transform.rotation = rotation;
    }

    public void Init(GameObject table, GameObject room)
    {
        _table = table;
        _room = room;
    }



    public void SetScale(float length, float height, float depth )
    {
        transform.localScale = Vector3.one;
        
        _bounds= transformHelper.GetBoundingBox(_table);


        float scaleY = (height / _bounds.size.y)/100;
        float scaleX = (length / _bounds.size.x)/100;
        float scaleZ = (depth / _bounds.size.z)/100;
        _table.transform.localScale = new Vector3(scaleX, scaleY, scaleZ);

        //_button.transform.position = _buttonPos.position;







    }
}
