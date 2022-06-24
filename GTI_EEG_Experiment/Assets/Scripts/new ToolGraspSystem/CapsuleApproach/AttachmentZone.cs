using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class AttachmentZone : MonoBehaviour
{

    private Hand _hand;

    private Transform _armExtension;

    private Collider _collider;

    [SerializeField] private GameObject Ball;

    [SerializeField]private GameObject Handle;
    [SerializeField]private GameObject Effector;
    [SerializeField] private Transform ClosestPoint;

    private float _distanceToHand;
    
    private float _distanceToHandle;
    private float _distanceToEffector;
    private float _orthonormalDistance;
    
    private static float _handleEffectorDistance;


    private bool _isCloserToHandle;
    // Start is called before the first frame update
    void Start()
    {
        _hand = Player.instance.rightHand;
        _armExtension = _hand.GetComponentInChildren<ArmExtension>().transform;
        _collider = this.transform.GetComponent<Collider>();
        _handleEffectorDistance = Vector3.Distance(Effector.transform.position, Handle.transform.position);
    }

    public float GetDistanceToHand()
    {
        return _distanceToHand;
    }

    public void SetDebugstate(bool state)
    {
        Handle.GetComponent<MeshRenderer>().enabled = state;
        Effector.GetComponent<MeshRenderer>().enabled = state;
        Ball.GetComponent<MeshRenderer>().enabled = state;
    }

    public float HandleEffectorDistance
    {
        get
        {
            if (_handleEffectorDistance == 0f)
            {
                _handleEffectorDistance=Vector3.Distance(Effector.transform.position, Handle.transform.position);
                
            }
            return _handleEffectorDistance;
        }
    }

    public float GetDistanceToEffector()
    {
        return _distanceToEffector;
    }
    public float GetDistanceToHandle()
    {
        return _distanceToEffector;
    }

    public float OrthonormalDistance()
    {
        return _orthonormalDistance;
    }
    
    public bool IsHandCloserToHandle()
    {
        return _isCloserToHandle;
    }



    public void SetColor(Color color)
    {
        Ball.GetComponent<Renderer>().material.color = color;
    }

    // Update is called once per frame
    void Update()
    {
        //var handPosition = _hand.transform.position;
        var armPosition = _armExtension.transform.position;
        ClosestPoint.transform.position = _collider.ClosestPoint(armPosition);
        Ball.transform.localPosition = new Vector3(0f,
            0f, ClosestPoint.localPosition.z);
        _orthonormalDistance = (Ball.transform.localPosition.z);
        //Debug.Log(_orthonormalDistance);
        _distanceToHand = Vector3.Distance(armPosition, Ball.transform.position);
        _distanceToHandle = Vector3.Distance(Handle.transform.position, Ball.transform.position);
        _distanceToEffector = Vector3.Distance(Effector.transform.position, Ball.transform.position);
        
        


        _isCloserToHandle = _distanceToHandle < _distanceToEffector;

    }

   
    
    
}
