/*
 * Author: Stefan Balle
 * E-mail: sballe@uni-osnabrueck.de
 * Year: 2020
 */

using TMPro;
using UnityEngine;
using Valve.VR;

public class TextController : MonoBehaviour
{

    public string configManagerTag;
    private ObjectTransformHelper objectTransformHelper;

    [SerializeField] private int fontSizeSmall=15;
    [SerializeField] private int fontSizeLarge=60;
    
    void Start()
    {
//        configManager = GameObject.FindGameObjectWithTag(configManagerTag).GetComponent<ConfigManager>();
        objectTransformHelper = GetComponent<ObjectTransformHelper>();
    }


    public void ShowText(string text, bool large=false)
    {
        if (large)
        {
            ChangeCueFontSize(fontSizeLarge);
        }
        else
        {
            ChangeCueFontSize(fontSizeSmall);
        }
        
        ChangeCueText(text);
    }
    
    
    public void ChangeCueFontSize(int size)
    {
        transform.GetChild(0).GetComponent<TextMeshPro>().fontSize = size;
    }
    
    public void ChangeCueText(string text)
    {
       transform.GetChild(0).GetComponent<TextMeshPro>().text = text;
    }
    
    
}
