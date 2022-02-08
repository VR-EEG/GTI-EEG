using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonConfiguration : MonoBehaviour
{
    [SerializeField] private Transform buttonPositionOnTable;
    // Start is called before the first frame update
    
    public void AdjustPosition()
    {
        this.transform.position = buttonPositionOnTable.transform.position;
    }
    
}
