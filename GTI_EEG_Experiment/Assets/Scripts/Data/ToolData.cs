using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolData : MonoBehaviour
{
    private BoxCollider _toolOversizedCollider;

    [HideInInspector] public BoxCollider toolOverSizedCollider
    {
        get
        {
            if (_toolOversizedCollider == null)
            {
                _toolOversizedCollider = GetComponentInChildren<BoxCollider>();
            }
            else
            {
                return _toolOversizedCollider;
            }
            return null;
        }
       
    }
    private void Awake()
    {
        _toolOversizedCollider = GetComponentInChildren<BoxCollider>();
        
        this.gameObject.SetActive(false);
    }
}
