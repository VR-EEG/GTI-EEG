using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableCalibrationUI : MonoBehaviour
{
    private bool _isActive;

    public void SetActive(bool state)
    {
        _isActive = state;
    }
    
    private void OnGUI()
    {
        if (_isActive)
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
            
            
            if (GUI.Button(new Rect(valX, Screen.height/2, w*1.5f, 80), "Start Calibration", buttonStyle))
            {
                //StartEyeTrackingCalibration();
                TableConfigurationManager.Instance.AutoCalibrateTablePosition();
            }
            
            
            
            valX += Mathf.RoundToInt(w)+2;
            
            
            if (GUI.Button(new Rect(valX, Screen.height/2, w*1.5f, 80), "Set Table Scale", buttonStyle))
            {

                var vec = new Vector3(1, 1, 6);

                var length = 6f; 
                var depth = 1f; 
                var height = 2f; 
                //StartEyeTrackingCalibration(); / h / l / d
                TableConfigurationManager.Instance.SetTableScale(length,height,depth);
            }

            









        }
        
    }
    
    public enum UIState
    {
        MainMenu,
        EnterTableSize,
        
    }
    
}
