using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class GripPointSelection : MonoBehaviour
{
    private Interactable _interactable;

    private SteamVR_Skeleton_Poser _skeletonPoser;


    [SerializeField] private float toolLength;

    [SerializeField] private AttachmentTrigger handleAttachmentTrigger;
    [SerializeField] private AttachmentTrigger effectorAttachmentTrigger;
    private GameObject dominantAttachmentTrigger;

    private bool _handleIsCloser;

    private SteamVR_Skeleton_Poser.PoseBlendingBehaviour _congruentHandlePoseBlendingBehaviour;
    private SteamVR_Skeleton_Poser.PoseBlendingBehaviour _congruentEffectorPoseBlendingBehaviour;
    
    private SteamVR_Skeleton_Poser.PoseBlendingBehaviour _incongruentMainPoseBlendingBehavior;
    private SteamVR_Skeleton_Poser.PoseBlendingBehaviour _incongruentHandlePoseBlendingBehaviour;
    private SteamVR_Skeleton_Poser.PoseBlendingBehaviour _incongruentEffectorPoseBlendingBehaviour;
    private Hand _hand;

    private bool _toolOrientationCongruent;
    
    
     
    // Start is called before the first frame update
    void Start()
    {
        _interactable = GetComponent<Interactable>();
        _skeletonPoser = GetComponent<SteamVR_Skeleton_Poser>();

        _congruentHandlePoseBlendingBehaviour = _skeletonPoser.GetBlendingBehaviour("BlendToHandle");
        _congruentEffectorPoseBlendingBehaviour = _skeletonPoser.GetBlendingBehaviour("BlendToEffector");
        
        _incongruentMainPoseBlendingBehavior = _skeletonPoser.GetBlendingBehaviour("BlendToMainIncongruent");
        _incongruentHandlePoseBlendingBehaviour = _skeletonPoser.GetBlendingBehaviour("BlendToHandleIncongruent");
        _incongruentEffectorPoseBlendingBehaviour = _skeletonPoser.GetBlendingBehaviour("BlendToEffectorIncongruent");
        

        NewExperimentManager.Instance.OnToolShown += SetToolOrientation; 
        

        _interactable.onAttachedToHand += ObjectAttached;


        handleAttachmentTrigger.InsideAttachmentTrigger += HandCloseToHandle;
        //handleAttachmentTrigger.OutsideAttachmentTrigger += ;
        
       effectorAttachmentTrigger.InsideAttachmentTrigger += HandCloseToEffector;

       

       _hand = Player.instance.rightHand;
    }

    // Update is called once per frame
    void Update()
    {

        
        if (_interactable.isHovering)
        {


            if (_toolOrientationCongruent)
            {
                _incongruentMainPoseBlendingBehavior.influence = 0;
                _incongruentHandlePoseBlendingBehaviour.influence = 0;
                _incongruentEffectorPoseBlendingBehaviour.influence = 0;
                _congruentEffectorPoseBlendingBehaviour.influence = 1;
                _congruentHandlePoseBlendingBehaviour.influence = 1;
            }
            else
            {
                _incongruentMainPoseBlendingBehavior.influence = 1;
                _incongruentHandlePoseBlendingBehaviour.influence = 1;
                _incongruentEffectorPoseBlendingBehaviour.influence = 1;
                _congruentEffectorPoseBlendingBehaviour.influence = 0;
                _congruentHandlePoseBlendingBehaviour.influence = 0;
            }
             
            
            
            
            if (_handleIsCloser)
            {
                var closestGraspPosition = CalculateClosestPointOnTool(this.transform.position,
                    handleAttachmentTrigger.transform.position, _hand.hoverSphereTransform.position, true);
            
                var totalDistance = Vector3.Distance(this.transform.position, handleAttachmentTrigger.transform.position);
                var distanceToGraspPoint = Vector3.Distance(this.transform.position, closestGraspPosition);
                var placementRatio = Mathf.Clamp01(distanceToGraspPoint / totalDistance);
                if (_toolOrientationCongruent)
                {
                    _congruentEffectorPoseBlendingBehaviour.value = 0f;
                    _congruentHandlePoseBlendingBehaviour.value = placementRatio;
                }
                else
                {
                    _incongruentEffectorPoseBlendingBehaviour.value = 0f;
                    _incongruentHandlePoseBlendingBehaviour.value = placementRatio;
                }
                
            }
            else
            {
                var closestGraspPosition = CalculateClosestPointOnTool(this.transform.position,
                    effectorAttachmentTrigger.transform.position, _hand.hoverSphereTransform.position, false);
                var totalDistance = Vector3.Distance(this.transform.position, effectorAttachmentTrigger.transform.position);
                var distanceToGraspPoint = Vector3.Distance(this.transform.position, closestGraspPosition);
                var placementRatio = Mathf.Clamp01(distanceToGraspPoint / totalDistance);
               
                if (_toolOrientationCongruent)
                {
                    _congruentHandlePoseBlendingBehaviour.value = 0f;
                    _congruentEffectorPoseBlendingBehaviour.value = placementRatio;
                }
                else
                {
                    _incongruentHandlePoseBlendingBehaviour.value = 0f;
                    _incongruentEffectorPoseBlendingBehaviour.value = placementRatio;
                }
            }
        }
    }


    private void ObjectAttached(Hand hand)
    {
        
        

       
        
        
    }


    private void SetToolOrientation(object sender, ToolShownEventArgs toolShownEventArgs)
    {
        _toolOrientationCongruent = Convert.ToBoolean(toolShownEventArgs.orientation);
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

    
}
