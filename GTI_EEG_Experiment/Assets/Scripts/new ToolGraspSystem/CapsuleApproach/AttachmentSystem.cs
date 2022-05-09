﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class AttachmentSystem : MonoBehaviour
{
    [SerializeField] private TriggerTest _congruent;
    [SerializeField] private TriggerTest _incongruent;

    private float _concruentDistance;
    private float _incongruentDistance;

    private bool _attached;

    private bool _isInteractable;
    
    private Interactable _interactable;

    private SteamVR_Skeleton_Poser _skeletonPoser;
    
    
    SteamVR_Skeleton_Poser.PoseBlendingBehaviour _congruentMainPoseBlendingBehavior;
    
    private SteamVR_Skeleton_Poser.PoseBlendingBehaviour _congruentHandlePoseBlendingBehaviour;
    private SteamVR_Skeleton_Poser.PoseBlendingBehaviour _congruentEffectorPoseBlendingBehaviour;
    
    private SteamVR_Skeleton_Poser.PoseBlendingBehaviour _incongruentMainPoseBlendingBehavior;
    private SteamVR_Skeleton_Poser.PoseBlendingBehaviour _incongruentHandlePoseBlendingBehaviour;
    private SteamVR_Skeleton_Poser.PoseBlendingBehaviour _incongruentEffectorPoseBlendingBehaviour;
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

        _interactable.onAttachedToHand += OnAttachHand;
        _interactable.onDetachedFromHand += OnDetachFromHand;
        _isInteractable = true;
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
    void Update()
    {
        if (!_isInteractable) return;
        
        _incongruentMainPoseBlendingBehavior.value = 0;
        _incongruentHandlePoseBlendingBehaviour.value = 0;
        _incongruentEffectorPoseBlendingBehaviour.value = 0;
        _congruentMainPoseBlendingBehavior.value = 0;
        _congruentEffectorPoseBlendingBehaviour.value = 0;
        _congruentHandlePoseBlendingBehaviour.value= 0;
        
        _incongruentMainPoseBlendingBehavior.influence = 0;
        _incongruentHandlePoseBlendingBehaviour.influence = 0;
        _incongruentEffectorPoseBlendingBehaviour.influence = 0;
        _congruentMainPoseBlendingBehavior.influence = 0;
        _congruentEffectorPoseBlendingBehaviour.influence = 0;
        _congruentHandlePoseBlendingBehaviour.influence= 0;
        
       
        
        _concruentDistance = _congruent.GetDistanceToHand();
        _incongruentDistance = _incongruent.GetDistanceToHand();
        
        if (_concruentDistance < _incongruentDistance)
        {
            _congruentMainPoseBlendingBehavior.value = 0;
            _incongruentMainPoseBlendingBehavior.value = 0;
            _incongruentHandlePoseBlendingBehaviour.value = 0;
            _incongruentEffectorPoseBlendingBehaviour.value = 0;
            
            _congruentMainPoseBlendingBehavior.influence = 0;
            _incongruentMainPoseBlendingBehavior.influence = 0;
            _incongruentHandlePoseBlendingBehaviour.influence = 0;
            _incongruentEffectorPoseBlendingBehaviour.influence = 0;
           
            if (_congruent.IsHandCloserToHandle())
            {
                _congruentHandlePoseBlendingBehaviour.value = 1;
                _congruentEffectorPoseBlendingBehaviour.value = 0;
                
                _congruentHandlePoseBlendingBehaviour.influence = 1;
                _congruentEffectorPoseBlendingBehaviour.influence = 0;
                
            }
            else
            {
                _congruentHandlePoseBlendingBehaviour.value = 0;
                _congruentEffectorPoseBlendingBehaviour.value = 1;
                _congruentHandlePoseBlendingBehaviour.influence = 0;
                _congruentEffectorPoseBlendingBehaviour.influence= 1;
            }
            _congruent.SetColor(_congruent.IsHandCloserToHandle() ? Color.red : Color.blue);
            _incongruent.SetColor(Color.white);
        }
        else
        {
            _congruentMainPoseBlendingBehavior.value = 0;
            _incongruentMainPoseBlendingBehavior.value = 0;
            _congruentEffectorPoseBlendingBehaviour.value = 0;
            _congruentHandlePoseBlendingBehaviour.value = 0;
            _congruentMainPoseBlendingBehavior.influence = 0;
            _incongruentMainPoseBlendingBehavior.influence = 0;
            _congruentEffectorPoseBlendingBehaviour.influence = 0;
            _congruentHandlePoseBlendingBehaviour.influence = 0;
            
            
            if (_incongruent.IsHandCloserToHandle())
            {
               
                _incongruentHandlePoseBlendingBehaviour.value = 1;
                _incongruentEffectorPoseBlendingBehaviour.value = 0;
                _incongruentHandlePoseBlendingBehaviour.influence = 1;
                _incongruentEffectorPoseBlendingBehaviour.influence = 0;
            }
            else
            {
                _incongruentHandlePoseBlendingBehaviour.value = 0;
                _incongruentEffectorPoseBlendingBehaviour.value = 1;
                _incongruentHandlePoseBlendingBehaviour.influence = 0;
                _incongruentEffectorPoseBlendingBehaviour.influence = 1;
            }
            
            _incongruent.SetColor(_congruent.IsHandCloserToHandle() ? Color.red : Color.blue);
            _congruent.SetColor(Color.white);
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