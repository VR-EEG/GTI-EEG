using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class AttachmentTrigger : MonoBehaviour
{
    public event Action InsideAttachmentTrigger;
    public event Action OutsideAttachmentTrigger;

    private Hand _hand;
    private void Start()
    {
        _hand = Player.instance.rightHand;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.name=="HoverPoint")
        {
            InsideAttachmentTrigger.Invoke();
        }
           
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.name=="HoverPoint")
        {
            OutsideAttachmentTrigger?.Invoke();
        }
    }
}
