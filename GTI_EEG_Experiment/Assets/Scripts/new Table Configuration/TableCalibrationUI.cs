using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class TableCalibrationUI : MonoBehaviour
{
    private bool _isActive;

    private float _windowHeight;
    private float _windowWidth;
    
    
    private float _length;
    private float _depth;
    private float _height;
    
    private string _heightText;
    private string _depthText;
    private string _lengthText;

    private float _x;
    private float _y;
    private float _z;

    private string _xText;
    private string _yText;
    private string _zText;

    private string _xButtonText;
    private string _yButtonText;
    private string _zButtonText;

    private bool _overridePosition;

    private bool overrideScalePossible;
    

    public void SetActive(bool state)
    {
        _isActive = state;
    }
    
    private void Start()
    {
        _windowHeight = Screen.height;
        _windowWidth = Screen.width;

        overrideScalePossible = false;

    }

    private void OnGUI()
    {
        if (_isActive)
        {
            var position = TableConfigurationManager.Instance.GetTablePosition();
            var y =(int) _windowHeight / 10;
            var x = (int)_windowWidth / 10;
            var w = 200;
            var wm = 100;
            var heightBox = 40;
            var h = 50;
            var valX = x;
            var valY = y;
            var spacing = 1;
            
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
            
            
            //X
            GUI.Box(new Rect(valX, valY, wm, heightBox), position.x.ToString("0.00",CultureInfo.InvariantCulture), boxStyle);
            valY += heightBox + spacing;
            GUI.Box(new Rect(valX, valY, wm, heightBox), "X", boxStyle);
            valY += heightBox + spacing;
            
            _xText = GUI.TextField(new Rect(valX, valY, wm, 40), _xText);
            
            //Y
            valY = y;
            valX += wm+spacing;
            GUI.Box(new Rect(valX, valY, wm, heightBox), position.y.ToString("0.00",CultureInfo.InvariantCulture), boxStyle);
            valY += heightBox + spacing;
            GUI.Box(new Rect(valX, valY, wm, heightBox), "Y", boxStyle);
            valY += heightBox + spacing;
            _yText = GUI.TextField(new Rect(valX, valY, wm, 40), _yText);
            //Z
            valY = y;
            valX += wm+spacing;
            GUI.Box(new Rect(valX, valY, wm, 40), position.z.ToString("0.00",CultureInfo.InvariantCulture), boxStyle);
            valY += heightBox + spacing;
            GUI.Box(new Rect(valX, valY, wm, 40), "Z", boxStyle);
            valY += heightBox + spacing;
            _zText = GUI.TextField(new Rect(valX, valY, wm, 40), _zText);
            
            //set button
            valX += wm+spacing;
            valY = y;
            GUI.backgroundColor = Color.cyan;
            if (GUI.Button(new Rect(valX, valY, w, 80), "Set", buttonStyle))
            {
                CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
                float tmpPosX;
                float tmpPosY;
                float tmpPosZ;
                
                Vector3 TablePosition = TableConfigurationManager.Instance.GetTablePosition();
                Vector3 pos = new Vector3();

                bool overrideX=false;
                bool overrideY=false;
                bool overrideZ = false;
                
                if (float.TryParse(_xText, out tmpPosX))
                {
                    pos.x += tmpPosX;
                    overrideX = true;
                }
                
                if (float.TryParse(_yText, out tmpPosY))
                {
                    pos.y += tmpPosY;
                    overrideY = true;
                }

                if (float.TryParse(_zText, out tmpPosZ))
                {
                    pos.z += tmpPosZ; 
                    overrideZ=true;
                }

                if (overrideX)
                {
                    TablePosition.x = pos.x;
                }
                
                if (overrideY)
                {
                    TablePosition.y= pos.y;
                }
                
                if (overrideZ)
                {
                    TablePosition.z = pos.z;
                }
                
                TableConfigurationManager.Instance.SetTablePosition(TablePosition);
            }
            
            valX += w+spacing;

            if (GUI.Button(new Rect(valX, valY, w, 80), "Save", buttonStyle))
            {
                Vector3 TablePosition = TableConfigurationManager.Instance.GetTablePosition();
                
                TableConfigurationManager.Instance.SaveTablePosition(TablePosition.x,TablePosition.y,TablePosition.z);
                
            }




            valY =(int) _windowHeight*3 / 10;
            valX =(int) _windowWidth/2;
            GUI.backgroundColor = Color.black;
            
            
            GUI.Box(new Rect(valX, valY+41, w, 40), "length", boxStyle);
            _lengthText = GUI.TextField(new Rect(valX, valY, w, 40), _lengthText);
            
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

                if (float.TryParse(_lengthText, out _length))
                {
                    overrideScalePossible=true;
                    TableConfigurationManager.Instance.SetLength(_length);
                    TableConfigurationManager.Instance.SaveTableSize();
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
                    overrideScalePossible=true;
                    TableConfigurationManager.Instance.SetDepth(_depth);
                    TableConfigurationManager.Instance.SaveTableSize();
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
                    overrideScalePossible=true;
                    TableConfigurationManager.Instance.SetLength(_height);
                    TableConfigurationManager.Instance.SaveTableSize();
                }
            }

            valY = (int)_windowHeight * 3 / 10 + heightBox*2+ spacing;
            valX =(int) _windowWidth/2;
            
            if (overrideScalePossible)
            {
                Debug.Log("hello");
            }
            
            if (overrideScalePossible)
            {
                GUI.backgroundColor = Color.cyan;
            
                if (GUI.Button(new Rect(valX, valY, w*2, heightBox*1.5f), "Set Table Scale", buttonStyle))
                {
                    TableConfigurationManager.Instance.SetTableScale(_length,_height,_depth);
                    overrideScalePossible = false;
                }
            }
            
            
            
            
            valX = x;
            GUI.backgroundColor = Color.red;
            if (GUI.Button(new Rect(valX, Screen.height/2, w*1.5f, 80), "AutoCalibrate Position", buttonStyle))
            {
                TableConfigurationManager.Instance.AutoCalibrateTablePosition();
                TableConfigurationManager.Instance.AutoCalibrateButtonPosition();
            }
            
            
            valX = (int) (x+w*1.5f+1);
            if (GUI.Button(new Rect(valX, Screen.height/2, w*1.5f, 80), "AutoCalibrate Button", buttonStyle))
            {
                TableConfigurationManager.Instance.AutoCalibrateButtonPosition();
            }

            
            
            

            
            
           
            
           
        }
    }
}
