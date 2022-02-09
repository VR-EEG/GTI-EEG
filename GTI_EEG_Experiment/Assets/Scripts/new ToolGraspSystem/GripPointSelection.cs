using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class GripPointSelection : MonoBehaviour
{
    private int _currentToolOrientation;

    private Interactable _interactable;

    private SteamVR_Skeleton_Poser _skeletonPoser;


    [SerializeField] private float toolLength;

    [SerializeField] private AttachmentTrigger handleAttachmentTrigger;
    [SerializeField] private AttachmentTrigger effectorAttachmentTrigger;
    private GameObject dominantAttachmentTrigger;

    private bool _handleIsCloser;

    private SteamVR_Skeleton_Poser.PoseBlendingBehaviour _handlePoseBlendingBehaviour;
    private SteamVR_Skeleton_Poser.PoseBlendingBehaviour _effectorPoseBlendingBehaviour;

    private Hand _hand;
     
    // Start is called before the first frame update
    void Start()
    {
        _interactable = GetComponent<Interactable>();
        _skeletonPoser = GetComponent<SteamVR_Skeleton_Poser>();

        _handlePoseBlendingBehaviour = _skeletonPoser.GetBlendingBehaviour("BlendToHandle");
        _effectorPoseBlendingBehaviour = _skeletonPoser.GetBlendingBehaviour("BlendToEffector");

        _interactable.onAttachedToHand += ObjectAttached; 
        _interactable.onDetachedFromHand += ObjectDetached;

        handleAttachmentTrigger.InsideAttachmentTrigger += HandCloseToHandle;
        //handleAttachmentTrigger.OutsideAttachmentTrigger += ;
        
       effectorAttachmentTrigger.InsideAttachmentTrigger += HandCloseToEffector;

       _hand = Player.instance.rightHand;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void ObjectAttached(Hand hand)
    {
        
        

        if (_handleIsCloser)
        {
            var closestGraspPosition = CalculateClosestPointOnTool(this.transform.position,
                handleAttachmentTrigger.transform.position, _hand.transform.position, true);
            
            var totalDistance = Vector3.Distance(this.transform.position, handleAttachmentTrigger.transform.position);
            var distanceToGraspPoint = Vector3.Distance(this.transform.position, closestGraspPosition);
            var placementRatio = Mathf.Clamp01(distanceToGraspPoint / totalDistance);
            _effectorPoseBlendingBehaviour.value = 0f;
            _handlePoseBlendingBehaviour.value = placementRatio;
        }
        else
        {
            var closestGraspPosition = CalculateClosestPointOnTool(this.transform.position,
                handleAttachmentTrigger.transform.position, _hand.transform.position, false);
            var totalDistance = Vector3.Distance(this.transform.position, effectorAttachmentTrigger.transform.position);
            var distanceToGraspPoint = Vector3.Distance(this.transform.position, closestGraspPosition);
            var placementRatio = Mathf.Clamp01(distanceToGraspPoint / totalDistance);
            _handlePoseBlendingBehaviour.value = 0f;
            _effectorPoseBlendingBehaviour.value = placementRatio;
        }
        
        
    }


    private Vector3 CalculateClosestPointOnTool(Vector3 centerPosition, Vector3 edgePosition, Vector3 handPosition, bool centerIsLeft)
    {
        var cE = Vector3.Distance(centerPosition, edgePosition);
        var cH = Vector3.Distance(centerPosition, handPosition);
        //  ch^2 = q*ce 
        //q = ch^2/ce
        var q = (cH* cH) / cE;
        if (centerIsLeft)
        {
            return centerPosition - this.transform.forward * q;
        }
        else
        {
            return centerPosition + this.transform.forward  * q;
        }
        
        
    }


    private void HandCloseToHandle()
    {
        Debug.Log("inside");
        _handleIsCloser = true;
    }
    

    private void HandCloseToEffector()
    {
        _handleIsCloser = false;
    }

    private void ObjectDetached(Hand hand)
    {
        _effectorPoseBlendingBehaviour.value = 0f;
        _handlePoseBlendingBehaviour.value = 0f;
    }
}
