using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class TriggerTest : MonoBehaviour
{

    private Hand _hand;

    private Collider _collider;

    [SerializeField] private GameObject Ball;

    [SerializeField]private GameObject Handle;
    [SerializeField]private GameObject Effector;

    private float _distanceToHand;

    private bool _isCloserToHandle;
    // Start is called before the first frame update
    void Start()
    {
        _hand = Player.instance.rightHand;
        _collider = this.transform.GetComponent<Collider>();
    }

    public float GetDistanceToHand()
    {
        return _distanceToHand;
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
        var handPosition = _hand.transform.position;
        var closesPointPosition = Ball.transform.position;
        closesPointPosition= _collider.ClosestPoint(handPosition);
        Ball.transform.position = closesPointPosition;
        _distanceToHand = Vector3.Distance(handPosition, closesPointPosition);

        _isCloserToHandle = Vector3.Distance(Handle.transform.position, closesPointPosition) <
                            Vector3.Distance(Effector.transform.position, closesPointPosition);

    }

   
    
    
}
