using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class TableCalibrationUI : MonoBehaviour
{
    private bool _isActive;
    private float _length;
    private float _depth;
    private float _height;
    private string _heightText;
    private string _depthText;
    private string _lengthText;
    private bool _lengthGotReadIn;

    public void SetActive(bool state)
    {
        _isActive = state;
    }
    
    private void Start()
    {
        if (PlayerPrefs.HasKey("TableLength"))
        {
            _length = PlayerPrefs.GetFloat("TableLength");
            _lengthText = _length.ToString(CultureInfo.InvariantCulture);
        }
        
        if (PlayerPrefs.HasKey("TableHeight"))
        {
            _height = PlayerPrefs.GetFloat("TableHeight");
            _heightText = _height.ToString(CultureInfo.InvariantCulture);
        }
        
        if(PlayerPrefs.HasKey("TableDepth"))
        {
            _depth = PlayerPrefs.GetFloat("TableDepth");
            _depthText = _depth.ToString(CultureInfo.InvariantCulture);
        }
        
        if (_length != 0 && _height != 0 && _depth != 0)
        {
            TableConfigurationManager.Instance.SetTableScale(_length,_height,_depth);
        }
        
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

            valY =+ 200;
            valX += 400;

            GUI.Box(new Rect(valX, valY+41, w, 40), "length", boxStyle);
            _lengthText = GUI.TextField(new Rect(valX, valY, w, 40), _lengthText);

            
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

                if (float.TryParse(_lengthText, out _length))
                {
                    Debug.Log("got it");
                    PlayerPrefs.SetFloat("TableLength",_length);
                }
            }
            
            valX += w +2 ;
            
            GUI.Box(new Rect(valX, valY+41, w, 40), "depth", boxStyle);
            _depthText = GUI.TextField(new Rect(valX, valY, w, 40), _depthText);

            
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
                if (float.TryParse(_depthText, out _depth))
                {
                    PlayerPrefs.SetFloat("TableDepth",_depth);
                }
            }
            valX += w +2 ;

            GUI.Box(new Rect(valX, valY+41, w, 40), "height", boxStyle);
            _heightText = GUI.TextField(new Rect(valX, valY, w, 40), _heightText);
            
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

                if (float.TryParse(_heightText, out _height))
                {
                    PlayerPrefs.SetFloat("TableHeight",_height);
                }
            }
            valX = x;
            
            GUI.backgroundColor = Color.cyan;
            
            if (GUI.Button(new Rect(valX, Screen.height/2, w*1.5f, 80), "AutoCalibrate Position", buttonStyle))
            {
                TableConfigurationManager.Instance.AutoCalibrateTablePosition();
            }
            
            valX += Mathf.RoundToInt(2*w)+2;
            
            if (GUI.Button(new Rect(valX, Screen.height/2, w*1.5f, 80), "Set Table Scale", buttonStyle))
            {
                TableConfigurationManager.Instance.SetTableScale(_length,_height,_depth);
            }
        }
    }
}
