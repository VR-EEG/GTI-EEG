/*
 * Author: Stefan Balle
 * E-mail: sballe@uni-osnabrueck.de
 * Year: 2020
 */

using System;
using System.Collections;
using UnityEngine;
using ViveSR.anipal.Eye;


public class EyeTrackingController : MonoBehaviour
{

    // Tag of the ConfigManager 
    public string configManagerTag;
    
    // Config manager
    private ConfigManager configManager;
   
    // Eye Tracking Validation 
    public EyeTrackingValidation eyeTrackingValidation;

    // Keep track of number of calibrations and validations per scene call
    public int numberOfValidationAttempts;
    
    // Keep track of whether calibration/ validation is running 

    // Validation max error for validation to be accepted
    public float validationErrorMarginDegrees;
    

    private void Start()
    {
        // Find ConfigManager
        configManager = GameObject.FindGameObjectWithTag(configManagerTag).GetComponent<ConfigManager>();
        
        // Reset number of calibration/ validation attempts, count anew for each scene load  

        // Reset is calibrated/ validated
        configManager.eyeTrackingIsCalibrated = configManager.eyeTrackingIsValidated = false;
        
        // Reset latest validation result
        configManager.latestEyeTrackingValidationResults = new Vector3(float.NaN, float.NaN, float.NaN);
    }
    
    

    // Coroutine of Launching Eye Calibration to prevent busy waiting


    // Launch eye tracker validation 
    public void LaunchEyeValidation()
    {
        Debug.Log("[EyeTrackingManager] Starting Eye Tracker Validation.");
        
        // Increment number of validation attempts 
        numberOfValidationAttempts += 1;
        
        // Keep track of running state

        // Reset latest validation results
        configManager.latestEyeTrackingValidationResults = new Vector3(float.NaN, float.NaN, float.NaN);
        
        // Start validation
        StartCoroutine("LaunchValidation");
    }

    
    // Coroutine to start the eye tracking validation 
    IEnumerator LaunchValidation()
    {
        // Start validation 
        eyeTrackingValidation.StartValidation();

        // Check whether validation finished 
        yield return new WaitForSeconds(1f);
        while (!eyeTrackingValidation.validationFinished)
        {
            yield return new WaitForSeconds(0.5f);
        }
        
        // Validation finished 
        configManager.eyeTrackingIsValidated = true;
        EyetrackingManagerNew.Instance.ValidationCompleted();
        Debug.Log("[EyeTrackingManager] Finished Eye Tracker Validation.");

        yield break;
    }
}
