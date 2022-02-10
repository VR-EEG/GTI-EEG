using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class AttachmentTrigger : MonoBehaviour
{
    public event EventHandler<InsideTriggerEventArgs> OnInsideTriggerEvent;
    public event Action OutsideAttachmentTrigger;

    private Hand _hand;


    private bool _handIsCongruent;
    private bool _activated;
    private bool _isInsideTrigger;
    private float _currentAngle;

    private void Update()
    {

        if (_isInsideTrigger)
        {
            var triggerEventArgs = new InsideTriggerEventArgs(_currentAngle);
            OnInsideTriggerEvent?.Invoke(this, triggerEventArgs);
            _isInsideTrigger = false;
        }
    }

    private void Start()
    {
        _hand = Player.instance.rightHand;
        
      
    }

    public void Activate(bool state)
    {
        _activated = state;
    }
    

    private void OnTriggerStay(Collider other)
    {
      
        
        if (other.name=="HoverPoint")
        {
            _currentAngle = Quaternion.Dot(_hand.transform.rotation, this.transform.rotation);
            _isInsideTrigger=true;
        }
        
    }
    

    private void OnTriggerEnter(Collider other)
    {
        /*if (other.name == "HoverPoint")
        {
            var localPosition = other.transform.InverseTransformPoint(this._hand.transform.position);
            if (localPosition.x < 0)
            {
                Debug.Log(localPosition + " concruent");
            }
            else
            {
                Debug.Log(localPosition + "incongruent");
            }
        }*/
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name=="HoverPoint")
        {
            OutsideAttachmentTrigger?.Invoke();
        }
    }
}

public class InsideTriggerEventArgs : EventArgs
{
    public InsideTriggerEventArgs(float angle)
    {
        HandOrientation = angle;
    }
    public float HandOrientation;
}
