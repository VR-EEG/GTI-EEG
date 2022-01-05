using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyetrackingUINew : MonoBehaviour
{
    private bool _isActivated;

    private ConfigManager _configManager;

    private void Start()
    {
        _isActivated = false;
        _configManager = GameObject.FindGameObjectWithTag("ConfigManager").GetComponent<ConfigManager>();
    }

    public void Activate(bool state)
    {
        _isActivated = state;
    }

    private void OnGUI()
    {
        if (_isActivated)
        {
            var x = 100;
            var y = 100;
            var w = 200;
            var h = 50;
            
            var valX = x;
            var valY = y;
            
            var boxStyle = new GUIStyle(GUI.skin.box)
            {
            
                fontSize = 30,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleCenter
            };

            var buttonStyle = new GUIStyle(GUI.skin.button)
            {
                fontSize = 28,
                alignment = TextAnchor.MiddleCenter
            };
            
            GUI.backgroundColor = Color.cyan;
                valX = x;
                
                if (GUI.Button(new Rect(valX, Screen.height/2, w*1.5f, 80), "Calibration", buttonStyle))
                {
                  //StartEyeTrackingCalibration();
                  EyetrackingManagerNew.Instance.StartCalibration();
                }
                
                valX +=  Mathf.RoundToInt(w*1.5f) + 2;
                
                if (GUI.Button(new Rect(valX, Screen.height/2, w*1.5f, 80), "Validation", buttonStyle))
                {
                    EyetrackingManagerNew.Instance.StartValidation();
                }
                
                valY = Screen.height / 2 + 100;
                valX += Mathf.RoundToInt(w);
                valX +=  Mathf.RoundToInt(w*1.5f) + 2;
                
                
                if (GUI.Button(new Rect(valX, Screen.height/2, w*1.5f, 80), "Continue Experiment", buttonStyle))
                {
                    EyetrackingManagerNew.Instance.CloseSetup();
                }
                
                
                valX = x;

                if (EyetrackingManagerNew.Instance.IsCalibrationInitialized())
                {
                    if (!EyetrackingManagerNew.Instance.IsCalibrationInProgress())
                    {
                        if (EyetrackingManagerNew.Instance.CalibrationSucessful())
                        {
                            GUI.color = Color.green;
                            GUI.Box(new Rect(valX, valY, w*1.5f, 80), new GUIContent("SUCCESS"), boxStyle);
                        }
                        else
                        {
                            GUI.color = Color.red;
                            GUI.Box(new Rect(valX, valY, w*1.5f, 80), new GUIContent("FAILED"), boxStyle);
                        }
                    }
                    else
                    {
                        GUI.color = Color.grey;
                        GUI.Box(new Rect(valX, valY, w*1.5f, 80), new GUIContent("IN PROGRESS..."), boxStyle);
                    }
                }
                else
                {
                    GUI.color = Color.grey;
                    GUI.Box(new Rect(valX, valY, w*1.5f, 80), new GUIContent("-"), boxStyle);
                }
                
                valX +=  Mathf.RoundToInt(w*1.5f) + 2;
                
                if (EyetrackingManagerNew.Instance.IsValidationIntialized())
                {
                    if (!EyetrackingManagerNew.Instance.IsValidationInProgress())
                    {
                        if (EyetrackingManagerNew.Instance.IsValidationSucessful())
                        {
                            GUI.color = Color.green;
                            GUI.Box(new Rect(valX, valY, w*1.5f, 80), new GUIContent(_configManager.latestEyeTrackingValidationResults.ToString()), boxStyle);
                        }
                        else
                        {
                            GUI.color = Color.red;
                            GUI.Box(new Rect(valX, valY, w*1.5f, 80), new GUIContent(_configManager.latestEyeTrackingValidationResults.ToString()), boxStyle);
                        }
                    }
                    else
                    {
                        GUI.color = Color.grey;
                        GUI.Box(new Rect(valX, valY, w*1.5f, 80), new GUIContent("IN PROGRESS..."), boxStyle);
                    }
                    
                }
                else
                {
                    GUI.color = Color.grey;
                    GUI.Box(new Rect(valX, valY, w*1.5f, 80), new GUIContent("-"), boxStyle);
                }
        }
         
    }
}
