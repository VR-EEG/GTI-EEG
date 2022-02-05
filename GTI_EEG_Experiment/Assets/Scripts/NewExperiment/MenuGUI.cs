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
        private void Start()
        {
            
            _y = (int)(Screen.height * yRatio);
            _x = (int)(Screen.width * xRatio);

            Debug.Log(_x+ " - " +_y);
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

            var state = NewExperimentManager.Instance.GetExperimentState();


            switch (state)
            {
                case ExperimentState.MainMenu:
                {
                    if (GUI.Button(new Rect(x, y, buttonSizeW, buttonSizeH), "Tutorial", buttonStyle))
                    {
                        NewExperimentManager.Instance.StartTutorial();
                    }

                    x += buttonSizeW + space;


                    if (GUI.Button(new Rect(x, y, buttonSizeW, buttonSizeH), "Experiment", buttonStyle))
                    {
                        NewExperimentManager.Instance.StartExperiment();
                    }
            
            
                    x += buttonSizeW + space;
            
                    if (GUI.Button(new Rect(x, y, buttonSizeW, buttonSizeH), "Configuration", buttonStyle))
                    {
                        NewExperimentManager.Instance.StartTutorial();
                    }

                    break;
                }
                case ExperimentState.Training:
                {
                    if (GUI.Button(new Rect(x, y, buttonSizeW, buttonSizeH), "Finish", buttonStyle))
                    {
                        NewExperimentManager.Instance.StartExperiment();
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


                    if (GUI.Button(new Rect(x, y, buttonSizeW, buttonSizeH), "Calibrate Eyetracking", buttonStyle))
                    {
                        
                    }
            
            
                    x += buttonSizeW + space;
            
                    if (GUI.Button(new Rect(x, y, buttonSizeW, buttonSizeH), "Configuration", buttonStyle))
                    {
                        NewExperimentManager.Instance.StartTutorial();
                    }

                    break;
                }
                case ExperimentState.Finished:
                {
                    if (GUI.Button(new Rect(x, y, buttonSizeW, buttonSizeH), "Finish", buttonStyle))
                    {
                        
                    }

                    break;
                };
            }
        }
    }
}