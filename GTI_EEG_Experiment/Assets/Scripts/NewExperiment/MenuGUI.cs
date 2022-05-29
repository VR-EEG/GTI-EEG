using System;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace NewExperiment
{
    public class MenuGUI: MonoBehaviour
    {
        [SerializeField] private float xRatio=0.5f;
        [SerializeField] private float yRatio=0.5f;
        [SerializeField] private int buttonSizeW = 200;
        [SerializeField] private int buttonSizeH = 50;
        [SerializeField] private int fontSize = 30;
        private int space =20;
        private int _y;
        private int _x;


        private NewExperimentManager _manager;
        private void Start()
        {
            
            _y = (int)(Screen.height * yRatio);
            _x = (int)(Screen.width * xRatio);

            Debug.Log(_x+ " - " +_y);
            
            
            _manager= NewExperimentManager.Instance;
        }

        private void OnGUI()
        {
            GUI.backgroundColor = Color.cyan;
            GUI.skin.textField.fontSize = fontSize;
            var x = _y/2;
            var y = _x/2;
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

            var state = _manager.GetExperimentState();


            switch (state)
            {
                case ExperimentState.MainMenu:
                {
                    if (GUI.Button(new Rect(x, y, (int) (buttonSizeW*1.5), buttonSizeH), "Begin Experiment", buttonStyle))
                    {
                        NewExperimentManager.Instance.StartTutorial();
                    }

                    x += buttonSizeW + space;
                    
                    x += buttonSizeW + space;
            
                    if (GUI.Button(new Rect(x, y, buttonSizeW, buttonSizeH), "Configuration", buttonStyle))
                    {
                        NewExperimentManager.Instance.StartTableCalibration();
                    }

                    break;
                }
                case ExperimentState.Training:
                {
                    x += buttonSizeW + space;
                    
                    x += buttonSizeW + space;
                    
                    x += buttonSizeW + space;
                    
                    
                    x += buttonSizeW + space;
                    
                    
                    if (GUI.Button(new Rect(x, y, buttonSizeW, buttonSizeH), "Finish", buttonStyle))
                    {
                        NewExperimentManager.Instance.SetBetweenBlocks();
                    }

                    break;
                }
                case ExperimentState.BetweenBlocks:
                {
                    if (GUI.Button(new Rect(x, y, buttonSizeW, buttonSizeH), "Continue", buttonStyle))
                    {
                        NewExperimentManager.Instance.ContinueExperiment();
                    }

                    x += buttonSizeW + space;


                    if (GUI.Button(new Rect(x, y, buttonSizeW, buttonSizeH), "Calibration", buttonStyle))
                    {
                        NewExperimentManager.Instance.StartEyeCalibration();
                    }
                    x += buttonSizeW + space;
            
                    if (GUI.Button(new Rect(x, y, buttonSizeW, buttonSizeH), "Table", buttonStyle))
                    {
                        NewExperimentManager.Instance.StartTableCalibration();
                    }

                    break;
                }
                case ExperimentState.EyetrackingCalibration:
                    break;
                case ExperimentState.Finished:
                {
                    break;
                };
            }
            
            
            //display

            var xDisplay = _x / 4;
            var yDisplay = _y / 8;
            
            GUI.Box(new Rect(xDisplay, yDisplay, buttonSizeW*2, buttonSizeH),state.ToString(), boxStyle);
            if (state == ExperimentState.Experiment||state== ExperimentState.BetweenBlocks||state == ExperimentState.Finished)
            {
                var xD = xDisplay + buttonSizeW * 2 + 1;
                GUI.Box(new Rect(xD, yDisplay, buttonSizeW, buttonSizeH),"Block "+_manager.GetCurrentBlock(), boxStyle);
                
                GUI.Box(new Rect(xD+buttonSizeW+1, yDisplay, buttonSizeW, buttonSizeH),"Trial "+_manager.GetCurrentTrial(), boxStyle);
            }
            
            
            

        }
    }
}