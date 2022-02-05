/*
 * Author: Stefan Balle
 * E-mail: sballe@uni-osnabrueck.de
 * Year: 2020
 */

using System.Collections;
using System.Collections.Generic;
using Leap.Unity;
using TMPro;
using UnityEngine;
using Valve.VR;

public class TextController : MonoBehaviour
{
    public TableManager tableManager;
    public ToolManager toolManager;
    private ConfigManager configManager;
    public string configManagerTag;
    private ObjectTransformHelper objectTransformHelper;

    [SerializeField] private int fontSizeSmall=15;
    [SerializeField] private int fontSizeLarge=60;
    
    void Start()
    {
        configManager = GameObject.FindGameObjectWithTag(configManagerTag).GetComponent<ConfigManager>();
        objectTransformHelper = GetComponent<ObjectTransformHelper>();
    }


    public void UpdateCueText(string text, bool large)
    {
        if (large)
        {
            ChangeCueFontSize(fontSizeLarge);
        }
        else
        {
            ChangeCueFontSize(fontSizeSmall);
        }
        
        ChangeCueText("text");
    }
    
    public void UpdateCueText(ExperimentManager.CueStates cueState)
    {
        switch (cueState)
        {
            case ExperimentManager.CueStates.Start:
                ChangeCueFontSize(15);
                ChangeCueText("Interact with\ntrigger to begin.");
                break;
            case ExperimentManager.CueStates.StartMoveHead:
                ChangeCueFontSize(15);
                ChangeCueText("Move head\nto start position.");
                break;
            case ExperimentManager.CueStates.Lift:
                ChangeCueFontSize(60);
                ChangeCueText("Lift");
                break;
            case ExperimentManager.CueStates.Use:
                ChangeCueFontSize(60);
                ChangeCueText("Use");
                break;
            case ExperimentManager.CueStates.Pause:
                ChangeCueFontSize(15);
                ChangeCueText("Block BetweenBlocks.\nInteract with trigger to\nstart eye tracker calibration.");
                break;
            case ExperimentManager.CueStates.End:
                ChangeCueFontSize(15);
                ChangeCueText("Experiment is over.");
                break;
            case ExperimentManager.CueStates.PracticeStart:
                ChangeCueFontSize(15);
                ChangeCueText("This is the\npractice section.");
                break;
            case ExperimentManager.CueStates.PracticeEnd:
                ChangeCueFontSize(15);
                ChangeCueText("This is the end\nof the practice section.\nThe measured experiment\nstarts after eye calibration.");
                break;
            case ExperimentManager.CueStates.Empty:
                ChangeCueFontSize(60);
                ChangeCueText("");
                break;
            default:
                Debug.Log("Got invalid cue state, not updating cue text.");
                break;
        }
    }
    
    public void ChangeCueFontSize(int size)
    {
        transform.GetChild(0).GetComponent<TextMeshPro>().fontSize = size;
    }
    
    public void ChangeCueText(string text)
    {
       transform.GetChild(0).GetComponent<TextMeshPro>().text = text;
    }
    
    public void UpdateCueTextFromUtcon(int utcon)
    {
        string cueOrientationName = toolManager.GetCueOrientationNameFromUtcon(utcon);
        
        if (cueOrientationName.ToLower().Contains("lift"))
        {
            UpdateCueText(ExperimentManager.CueStates.Lift); // change cue to lift 
        }
        else if (cueOrientationName.ToLower().Contains("use"))
        {
            UpdateCueText(ExperimentManager.CueStates.Use); // change cue to use 
        }
        else // Should not happen, config files need to be created properly 
        {
            Debug.Log("[TextController] Error in the cue orientation name, it does not hold use or lift information!");
        }
    }
    
    // Set the position and rotation of the cue 
    public void UpdateCueTransform()
    {
        Debug.Log("[TextController] Updating Cue Transform.");
        
        UpdateCueText(ExperimentManager.CueStates.Empty);
        transform.ResetLocalPose();
        Vector3 tableRotationEuler = tableManager.GetTableRotation();
        Vector3 tableSurfaceCenterPosition = tableManager.GetTableSurfaceCenterPosition();
        Vector3 offsetTableCenterToCueInAir = new Vector3(configManager.cuePositionOffsetTowardsSubject, configManager.cuePositionOffsetUpwards, 0);
        Vector3 rotatedOffset = Quaternion.Euler(tableRotationEuler) * offsetTableCenterToCueInAir;
        Vector3 cuePosition = tableSurfaceCenterPosition + rotatedOffset;
        transform.position = cuePosition;
        transform.rotation = Quaternion.Euler(tableRotationEuler);
        
    }
    
}
