using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolData : MonoBehaviour
{
    public BoxCollider ToolOversizedCollider;


    private void Awake()
    {
        ToolOversizedCollider = GetComponentInChildren<BoxCollider>();
        
        this.gameObject.SetActive(false);
    }
}
