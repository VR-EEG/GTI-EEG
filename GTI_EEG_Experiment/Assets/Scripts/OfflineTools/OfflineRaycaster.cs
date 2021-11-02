using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Profiling.Memory.Experimental;

public class OfflineRaycaster : MonoBehaviour
{

    private bool isRunning;
    private CsvIO csvIo;

    public TableManager tableManager;
    public ToolManager toolManager;
    public TriggerManager triggerManager;
    public GameObject room;

    public string dataPointsCsvPath;
    public string metaDataFolderPath;
    public string outputCsvPath;
    public string hitColliderMustNotIncludeString;

    public bool indicateEyes;
    public int yieldAfterHowManyDataPoints = 100; // prevent freezing of Editor with low number, but computation takes longer

    private Dictionary<int,SubjectMetaData> metaDataDictionary;
    private GameObject eyePositionSphere;
    private GameObject eyeDirectionCube;
    
    // Start is called before the first frame update
    void Start()
    {
        csvIo = GetComponent<CsvIO>();
        metaDataDictionary = new Dictionary<int, SubjectMetaData>();
        isRunning = false;
        
        // Create Sphere and cube to indicate eye position and direction
        eyePositionSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        eyePositionSphere.transform.localScale = new Vector3(0.03f,0.03f,0.03f);
        eyePositionSphere.name = "OfflineRaycasterEyepositionSphere";
        eyePositionSphere.GetComponent<Renderer>().material.color = new Color(1,0,0,1);
        Destroy(eyePositionSphere.GetComponent<Collider>());
        eyeDirectionCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        eyeDirectionCube.transform.localScale = new Vector3(0.01f,0.01f,0.2f);
        eyeDirectionCube.name = "OfflineRaycasterEyedirectionCylinder";
        eyeDirectionCube.GetComponent<Renderer>().material.color = new Color(1,0,0,1);
        Destroy(eyeDirectionCube.GetComponent<Collider>());
        if (!indicateEyes)
        {
            eyePositionSphere.SetActive(false);
            eyeDirectionCube.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && !isRunning)
        {
            Debug.Log("[OfflineRaycaster] Starting.");
            isRunning = true;
            StartCoroutine("Run");
        }
    }

    
    // Raycaster 
    IEnumerator Run()
    {
        
        // Process the meta data to later reset the table positions  
        Debug.Log("[OfflineRaycaster] Processing subject meta data.");
        foreach (string metaPath in Directory.GetFiles(metaDataFolderPath))
        {
            // Read meta file
            string metaContent = csvIo.ReadFileFromPath(metaPath);
            
            // Parse 
            SubjectMetaData subjectMeta =
                JsonUtility.FromJson<SubjectMetaData>(metaContent);
            
            // Add to dictionary 
            metaDataDictionary.Add(subjectMeta.subjectId,subjectMeta);
        }
        Debug.Log("[OfflineRaycaster] Finished processing subject meta data.");
        
        
        
        // Process trial data 
        Debug.Log("[OfflineRaycaster] Starting processing of data points.");
        using (StreamReader sr = new StreamReader(dataPointsCsvPath))
        {
            
            // Keep track of subject id and utcon to be able to update only if necessary
            int currUtcon = -2;
            int currSubjectId = -2; 
            
            
            // Process line 
            int i = 0; // line counter 
            while (sr.Peek() >= 0 && i < 10000)
            {
                // Get line
                string line = sr.ReadLine();
                string[] elems = line.Split(',');
                
                //Debug.Log(line);
                //Debug.Log(elems[78] + " " + elems[17] + " " + elems[18] + " " + elems[19] + " " + elems[20] + " " + elems[21] + " " + elems[22]);

                // Skip csv header line 
                if (i == 0)
                {
                    // Write header to output file
                    using (StreamWriter outFile = File.AppendText(outputCsvPath))
                    {
                        outFile.WriteLine(line);
                    }
                    
                    i += 1;
                    continue;
                }
                
                
                // Extract utcon, eye information, subject id for all other data points 
                int subjectId = Convert.ToInt32(elems[86]);
                int utcon = Convert.ToInt32(elems[78]);
                Vector3 eyePosCombined = new Vector3(float.Parse(elems[17]),float.Parse(elems[18]),float.Parse(elems[19]));
                Vector3 eyeDirectionCombined = new Vector3(float.Parse(elems[20]),float.Parse(elems[21]),float.Parse(elems[22]));

                // Show sphere where eyes are and indicate direction with warped cube
                if (indicateEyes)
                {
                    eyePositionSphere.transform.position = eyePosCombined;
                    eyeDirectionCube.transform.position = eyePosCombined;
                    eyeDirectionCube.transform.rotation = Quaternion.LookRotation(eyeDirectionCombined);
                }
                
                //Debug.Log(subjectId.ToString() + " " + utcon.ToString() + " " + eyePosCombined.ToString() + " " + eyeDirectionCombined.ToString());

                // Check if update from subject id is necessary
                // Before change of utcon to change table first 
                if (subjectId != currSubjectId)
                {
                    UpdateFromSubjectId(subjectId);
                    currSubjectId = subjectId;
                }
                
                
                // Check if update from utcon is necessary
                if (utcon != currUtcon)
                {
                    UpdateFromUtcon(utcon);
                    currUtcon = utcon;
                }
                
                
                // Do the Raycast 
                string raycastResult = PerformRaycast(eyePosCombined, eyeDirectionCombined);
                
                
                // Write result to disk 
                using (StreamWriter outFile = File.AppendText(outputCsvPath))
                {
                    outFile.WriteLine(line + raycastResult);
                }
                
                
                // Next line
                i += 1;
                
                
                // Status update
                if (i % 500 == 0)
                {
                    Debug.Log("[OfflineRaycaster] Processed " + i.ToString() + " datapoints.");
                }

                // Prevent freeze in Unity Editor
                if (i % yieldAfterHowManyDataPoints == 0)
                {
                    yield return null; // Wait for next update 
                }

            }
            
            Debug.Log("[OfflineRaycaster] Processed all data points.");  
        }
        
        Debug.Log("[OfflineRaycaster] Finished.");
    }


    // Update the displayed tool 
    void UpdateFromUtcon(int utcon)
    {
        toolManager.DisplayNoTool();
        toolManager.DisplayToolOnTable(utcon);
    }

    // Update the scene setup including floor, table, trigger
    void UpdateFromSubjectId(int subjectId)
    {
        Debug.Log("[OfflineRaycaster] Updating scene setup from SubjectID " + subjectId.ToString() + ".");
        
        // Extract scene config
        SubjectMetaData metaData;
        metaDataDictionary.TryGetValue(subjectId,out metaData);
        Vector3 tablePosition = metaData.configManagerSettings.tablePosition;
        Vector3 tableRotation = metaData.configManagerSettings.tableRotation;
        Vector3 tableScale = metaData.configManagerSettings.tableScale;
        float floorHeight = metaData.configManagerSettings.floorHeight;
        
        // Update table
        tableManager.SetTableTransform(tablePosition, tableRotation, tableScale);
        
        // Update floor
        room.transform.position = new Vector3(0, floorHeight, 0);
        
        // Update trigger position
        // Only right handed participants, so keep default
        triggerManager.UpdateTriggerTransform();
    }

    
    // Do a raycast from the supplied eyes position towards the supplied eyes gaze direction
    // Raycast for all hits, but return the closest hit, that does not have specified string in its name     
    string PerformRaycast(Vector3 eyeCombinedPos, Vector3 eyeCombinedDirection)
    {
        // Raycast
        RaycastHit[] raycastHitsCombined;
        raycastHitsCombined = Physics.RaycastAll(eyeCombinedPos, eyeCombinedDirection,Mathf.Infinity);
        
        // Init hit values
        string hitObjectNameCombinedEyes = "";
        Vector3 hitPointOnObjectCombinedEyes = new Vector3();
        Vector3 hitObjectCenterInWorldCombinedEyes = new Vector3();
        
        
        // Make sure something was hit 
        if (raycastHitsCombined.Length > 0)
        {
            // Sort by distance
            raycastHitsCombined = raycastHitsCombined.OrderBy(x=>x.distance).ToArray();

            // Make sure hit was not on manual collider 
            int combinedHitsIndex = 0; 
            while (raycastHitsCombined[combinedHitsIndex].collider.name.ToLower().Contains(hitColliderMustNotIncludeString))
            {
                combinedHitsIndex += 1;
            }
            
            // Make sure array length is not exceeded
            if (combinedHitsIndex >= raycastHitsCombined.Length)
            {
                combinedHitsIndex -= 1;
            }

            // Hit on collider 
            hitObjectNameCombinedEyes = raycastHitsCombined[combinedHitsIndex].collider.name;
            hitPointOnObjectCombinedEyes = raycastHitsCombined[combinedHitsIndex].point;
            hitObjectCenterInWorldCombinedEyes = raycastHitsCombined[combinedHitsIndex].collider.bounds.center;
            
            //Debug.Log(combinedHitsIndex);
            //Debug.Log(hitObjectNameCombinedEyes);
        }
        
        return hitObjectNameCombinedEyes;
    }
    
}
