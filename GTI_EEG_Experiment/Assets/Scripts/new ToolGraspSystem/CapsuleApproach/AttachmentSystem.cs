using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class AttachmentSystem : MonoBehaviour
{
    [SerializeField] private AttachmentZone _congruent;
    [SerializeField] private AttachmentZone _incongruent;
    [SerializeField]
    private UpperSideDetector _upperSideDetector;

    private float _congruentDistance;
    private float _incongruentDistance;

    private bool _attached;

    private bool _isInteractable;
    
    private Interactable _interactable;

    private SteamVR_Skeleton_Poser _skeletonPoser;
    
    //up
    private SteamVR_Skeleton_Poser.PoseBlendingBehaviour _congruentMainPoseBlendingBehavior;
    private SteamVR_Skeleton_Poser.PoseBlendingBehaviour _congruentHandlePoseBlendingBehaviour;
    private SteamVR_Skeleton_Poser.PoseBlendingBehaviour _congruentEffectorPoseBlendingBehaviour;
    
    private SteamVR_Skeleton_Poser.PoseBlendingBehaviour _incongruentMainPoseBlendingBehavior;
    private SteamVR_Skeleton_Poser.PoseBlendingBehaviour _incongruentHandlePoseBlendingBehaviour;
    private SteamVR_Skeleton_Poser.PoseBlendingBehaviour _incongruentEffectorPoseBlendingBehaviour;
    
    //down
    
    private SteamVR_Skeleton_Poser.PoseBlendingBehaviour _congruentMainPoseBlendingBehaviorDown;
    private SteamVR_Skeleton_Poser.PoseBlendingBehaviour _congruentHandlePoseBlendingBehaviourDown;
    private SteamVR_Skeleton_Poser.PoseBlendingBehaviour _congruentEffectorPoseBlendingBehaviourDown;
    
    private SteamVR_Skeleton_Poser.PoseBlendingBehaviour _incongruentMainPoseBlendingBehaviorDown;
    private SteamVR_Skeleton_Poser.PoseBlendingBehaviour _incongruentHandlePoseBlendingBehaviourDown;
    private SteamVR_Skeleton_Poser.PoseBlendingBehaviour _incongruentEffectorPoseBlendingBehaviourDown;

    private float _distanceBetweenHandleAndEffector;
    // Start is called before the first frame update
    void Start()
    {
        _interactable = GetComponent<Interactable>();
        _skeletonPoser = GetComponent<SteamVR_Skeleton_Poser>();
        _congruentMainPoseBlendingBehavior = _skeletonPoser.GetBlendingBehaviour("BlendToMain");
        _congruentHandlePoseBlendingBehaviour = _skeletonPoser.GetBlendingBehaviour("BlendToHandle");
        _congruentEffectorPoseBlendingBehaviour = _skeletonPoser.GetBlendingBehaviour("BlendToEffector");
        
        _incongruentMainPoseBlendingBehavior = _skeletonPoser.GetBlendingBehaviour("BlendToMainIncongruent");
        _incongruentHandlePoseBlendingBehaviour = _skeletonPoser.GetBlendingBehaviour("BlendToHandleIncongruent");
        _incongruentEffectorPoseBlendingBehaviour = _skeletonPoser.GetBlendingBehaviour("BlendToEffectorIncongruent");
        
        _congruentMainPoseBlendingBehaviorDown = _skeletonPoser.GetBlendingBehaviour("BlendToMainDown"); 
        _congruentHandlePoseBlendingBehaviourDown = _skeletonPoser.GetBlendingBehaviour("BlendToHandleDown"); 
        _congruentEffectorPoseBlendingBehaviourDown= _skeletonPoser.GetBlendingBehaviour("BlendToEffectorDown");
        
        _incongruentMainPoseBlendingBehaviorDown = _skeletonPoser.GetBlendingBehaviour("BlendToMainDownIncongruent"); 
        _incongruentHandlePoseBlendingBehaviourDown = _skeletonPoser.GetBlendingBehaviour("BlendToHandleDownIncongruent"); 
        _incongruentEffectorPoseBlendingBehaviourDown= _skeletonPoser.GetBlendingBehaviour("BlendToEffectorDownIncongruent");
        
        _interactable.onAttachedToHand += OnAttachHand;
        _interactable.onDetachedFromHand += OnDetachFromHand;
        _isInteractable = true;

        _distanceBetweenHandleAndEffector = _congruent.HandleEffectorDistance;
    }

    private void OnAttachHand(Hand hand)
    {
        _isInteractable = false;
    }

    private void OnDetachFromHand(Hand hand)
    {
        _attached = false;
    }

    // Update is called once per frame
    
    private void ResetBlendingPoses()
    {
        _incongruentMainPoseBlendingBehavior.value = 0;
        _incongruentHandlePoseBlendingBehaviour.value = 0;
        _incongruentEffectorPoseBlendingBehaviour.value = 0;
        
        _congruentMainPoseBlendingBehavior.value = 0;
        _congruentEffectorPoseBlendingBehaviour.value = 0;
        _congruentHandlePoseBlendingBehaviour.value= 0;

        _congruentHandlePoseBlendingBehaviourDown.value = 0f;
        _congruentMainPoseBlendingBehaviorDown.value = 0f;
        _congruentEffectorPoseBlendingBehaviourDown.value = 0f;
        
        _incongruentMainPoseBlendingBehavior.influence = 0;
        _incongruentHandlePoseBlendingBehaviour.influence = 0;
        _incongruentEffectorPoseBlendingBehaviour.influence = 0;
        
        _congruentMainPoseBlendingBehavior.influence = 0;
        _congruentEffectorPoseBlendingBehaviour.influence = 0;
        _congruentHandlePoseBlendingBehaviour.influence= 0;
        
        _congruentMainPoseBlendingBehaviorDown.influence = 0;
        _congruentHandlePoseBlendingBehaviourDown.influence = 0;
        _congruentEffectorPoseBlendingBehaviourDown.influence = 0;
        
        _incongruentMainPoseBlendingBehaviorDown.influence = 0;
        _incongruentHandlePoseBlendingBehaviourDown.influence = 0;
        _incongruentEffectorPoseBlendingBehaviourDown.influence = 0;
    }
    
    void Update()
    {
        if (!_isInteractable) return;

        var rightIsUpperside = _upperSideDetector.GetRightSideIsUp();
        ResetBlendingPoses();
        _congruentDistance = _congruent.GetDistanceToHand();
        _incongruentDistance = _incongruent.GetDistanceToHand();

        if (rightIsUpperside)
        {
            if (_congruentDistance < _incongruentDistance)
            {
                var ratio = (_congruent.OrthonormalDistance()+_distanceBetweenHandleAndEffector/2)/(_distanceBetweenHandleAndEffector);

                if (_congruent.IsHandCloserToHandle())
                {
                    _congruentHandlePoseBlendingBehaviour.value = 1f-ratio;
                    _congruentMainPoseBlendingBehavior.value = ratio;
                    _congruentHandlePoseBlendingBehaviour.influence = 1f-ratio;
                    _congruentMainPoseBlendingBehavior.influence = ratio;
                }
                else
                { 
                    _congruentEffectorPoseBlendingBehaviour.value = ratio; 
                    _congruentMainPoseBlendingBehavior.value = (1f-  ratio); 
                    _congruentEffectorPoseBlendingBehaviour.influence = ratio; 
                    _congruentMainPoseBlendingBehavior.influence= (1f-  ratio);
                } 
            
                _congruent.SetColor(_congruent.IsHandCloserToHandle() ? Color.red : Color.blue); 
                _incongruent.SetColor(Color.white);
            }
            else
            {
                var ratio = (_incongruent.OrthonormalDistance()+_distanceBetweenHandleAndEffector/2)/(_distanceBetweenHandleAndEffector);
                _incongruentMainPoseBlendingBehavior.influence = 1;
                _incongruentHandlePoseBlendingBehaviour.influence = 1;
                _incongruentEffectorPoseBlendingBehaviour.influence = 1;
            
            
                if (_incongruent.IsHandCloserToHandle())
                {
                    _incongruentHandlePoseBlendingBehaviour.value = 1f-ratio;
                    _incongruentMainPoseBlendingBehavior.value = 1;
                }
                else
                {
                    _incongruentEffectorPoseBlendingBehaviour.value = ratio;
                    _incongruentMainPoseBlendingBehavior.value = 1;
                }
            
                _incongruent.SetColor(_congruent.IsHandCloserToHandle() ? Color.red : Color.blue);
                _congruent.SetColor(Color.white);
            }
        }
        else
        {
            if (_congruentDistance < _incongruentDistance)
            {
                var ratio = (_congruent.OrthonormalDistance()+_distanceBetweenHandleAndEffector/2)/(_distanceBetweenHandleAndEffector);

                _congruentMainPoseBlendingBehavior.influence = 0f;
                _incongruentMainPoseBlendingBehavior.influence = 0f;
                _congruentMainPoseBlendingBehaviorDown.influence = 0f;
                
                _congruentMainPoseBlendingBehaviorDown.influence = 2f; // this for some reasons worked to get the overweight influence
                if (_congruent.IsHandCloserToHandle())
                {
                    _congruentHandlePoseBlendingBehaviourDown.influence = 1f;
                    _congruentEffectorPoseBlendingBehaviourDown.value = 0f;
                    _congruentHandlePoseBlendingBehaviourDown.value = Mathf.Clamp01(1f-ratio);
                    _congruentMainPoseBlendingBehaviorDown.value = Mathf.Clamp01(ratio);
                }
                else
                {
                    _congruentEffectorPoseBlendingBehaviourDown.influence = 1f;
                    _congruentHandlePoseBlendingBehaviourDown.value = 0f;
                    _congruentEffectorPoseBlendingBehaviourDown.value = Mathf.Clamp01(ratio); 
                    _congruentMainPoseBlendingBehaviorDown.value = Mathf.Clamp01(1f-ratio);
                } 
            
                _congruent.SetColor(_congruent.IsHandCloserToHandle() ? Color.red : Color.blue); 
                _incongruent.SetColor(Color.white);
            }
            else
            {
                var ratio = (_incongruent.OrthonormalDistance()+_distanceBetweenHandleAndEffector/2)/(_distanceBetweenHandleAndEffector);
                
                _congruentMainPoseBlendingBehavior.influence = 0f;
                _incongruentMainPoseBlendingBehavior.influence = 0f;
                _congruentMainPoseBlendingBehaviorDown.influence = 0;
                
                _incongruentMainPoseBlendingBehaviorDown.influence = 2f; // this for some reasons worked to get the overweight influence
                if (_congruent.IsHandCloserToHandle())
                {
                    _incongruentHandlePoseBlendingBehaviourDown.influence = 1f;
                    _incongruentEffectorPoseBlendingBehaviourDown.value = 0f;
                    _incongruentHandlePoseBlendingBehaviourDown.value = Mathf.Clamp01(1f-ratio);
                    _incongruentMainPoseBlendingBehaviorDown.value = Mathf.Clamp01(ratio);
                }
                else
                {
                    _incongruentEffectorPoseBlendingBehaviourDown.influence = 1f;
                    _incongruentHandlePoseBlendingBehaviourDown.value = 0f;
                    _incongruentEffectorPoseBlendingBehaviourDown.value = Mathf.Clamp01(ratio); 
                    _incongruentMainPoseBlendingBehaviorDown.value = Mathf.Clamp01(1f-ratio);
                } 
            
                _incongruent.SetColor(_congruent.IsHandCloserToHandle() ? Color.red : Color.blue);
                _congruent.SetColor(Color.white);
            }
        }
    }

    private void LateUpdate()
    {
        if (!_interactable.attachedToHand)
        {
            _isInteractable = true;
        }
    }
}
