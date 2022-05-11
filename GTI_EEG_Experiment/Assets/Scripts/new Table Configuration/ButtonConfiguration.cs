using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonConfiguration : MonoBehaviour
{
    [SerializeField] private Transform buttonPositionOnTable;
    // Start is called before the first frame update

    [SerializeField] private GameObject button;
    [SerializeField] private GameObject SpawnZone;

    [SerializeField]private Vector2 offsetOfButton= new Vector2(0.4f, 0f);

    private void Start()
    {
        this.transform.position = buttonPositionOnTable.transform.position;
        var pos = button.transform.localPosition;
        pos = new Vector3(pos.x + offsetOfButton.x, pos.y, pos.z + offsetOfButton.y);
        button.transform.localPosition = pos;
        SpawnZone.SetActive(false);
    }

    public void AdjustPosition()
    {
        this.transform.position = buttonPositionOnTable.transform.position;
    }

    public void DisplaySpawnZone(bool state)
    {
        SpawnZone.SetActive(state);
    }
    
}
