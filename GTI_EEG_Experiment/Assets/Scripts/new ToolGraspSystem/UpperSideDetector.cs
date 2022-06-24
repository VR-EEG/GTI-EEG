using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class UpperSideDetector : MonoBehaviour
{
    [SerializeField] private Transform LeftSide;
    [SerializeField] private Transform RightSide;

    [SerializeField]
    private Hand _hand;

    private Transform _handBack;

    private bool _rightSideIsUp;
    // Start is called before the first frame update

    private void Start()
    {
        _hand = Player.instance.rightHand;
        _handBack = _hand.GetComponentInChildren<HandBack>().transform;
    }

    // Update is called once per frame
    void Update()
    {

        if (Vector3.Distance(RightSide.transform.position, _handBack.transform.position) <
            Vector3.Distance(LeftSide.transform.position, _handBack.transform.position))
        {
            _rightSideIsUp=true;
        }
        else
        {
            _rightSideIsUp = false;
        }
           
    }
    
    
    public bool GetRightSideIsUp()
    {
        return _rightSideIsUp;
    }
}
