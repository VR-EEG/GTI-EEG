using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolData : MonoBehaviour
{
    private BoxCollider _toolOversizedCollider;


    private void Awake()
    {
        _toolOversizedCollider = GetComponentInChildren<BoxCollider>();
        
        this.gameObject.SetActive(false);
    }

    public BoxCollider GetToolOversizedCollider()
    {
        if (_toolOversizedCollider != null)
        {
            return _toolOversizedCollider;
        }
        else
        {
            _toolOversizedCollider = GetComponentInChildren<BoxCollider>();
            return _toolOversizedCollider;
        }
       
    }
}
