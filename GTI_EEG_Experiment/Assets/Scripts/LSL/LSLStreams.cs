using UnityEngine;
using LSL;

public class LSLStreams : MonoBehaviour
{
    public static LSLStreams Instance { get; private set; } // used to allow easy access of this script in other scripts
    private string participantUID;

    private const double NominalRate = liblsl.IRREGULAR_RATE; // irregular sampling rate

    // variables to save date to LSL
    public liblsl.StreamInfo lslICondition;
    public liblsl.StreamOutlet lslOCondition; // empty Stream, saved here
    public liblsl.StreamInfo lslIFrameTracking;
    public liblsl.StreamOutlet lslOFrameTracking; // saved in ETRecorder.cs
    public liblsl.StreamInfo lslIEyeTrackingWorld;
    public liblsl.StreamOutlet lslOEyeTrackingWorld; // saved in ETRecorder.cs
    public liblsl.StreamInfo lslIEyeTrackingLocal;
    public liblsl.StreamOutlet lslOEyeTrackingLocal; // saved in ETRecorder.cs
    public liblsl.StreamInfo lslIHeadTracking;
    public liblsl.StreamOutlet lslOHeadTracking; // saved in ETRecorder.cs
    public liblsl.StreamInfo lslIHeadTrackingObjectNames;
    public liblsl.StreamOutlet lslOHeadTrackingObjectNames; // saved in ETRecorder.cs
    public liblsl.StreamInfo lslIHeadTrackingObjectGroups;
    public liblsl.StreamOutlet lslOHeadTrackingObjectGroups; // saved in ETRecorder.cs
    public liblsl.StreamInfo lslIHeadTrackingObjectPositions;
    public liblsl.StreamOutlet lslOHeadTrackingObjectPositions; // saved in ETRecorder.cs
    public liblsl.StreamInfo lslIHeadTrackingPositionOnObjects;
    public liblsl.StreamOutlet lslOHeadTrackingPositionOnObjects; // saved in ETRecorder.cs
    public liblsl.StreamInfo lslIHitObjectNames;
    public liblsl.StreamOutlet lslOHitObjectNames; // saved in ETRecorder.cs
    public liblsl.StreamInfo lslIHitObjectGroups;
    public liblsl.StreamOutlet lslOHitObjectGroups; // saved in ETRecorder.cs
    public liblsl.StreamInfo lslIHitObjectPositions;
    public liblsl.StreamOutlet lslOHitObjectPositions; // saved in ETRecorder.cs
    public liblsl.StreamInfo lslIHitPositionOnObjects;
    public liblsl.StreamOutlet lslOHitPositionOnObjects; // saved in ETRecorder.cs
    
    // GTI-EEG
    
    public liblsl.StreamInfo lslITimeStamps;
    public liblsl.StreamOutlet lslOTimeStamps; // saved in MeasurementManager.cs
    
    public liblsl.StreamInfo lslIEyeTrackingHMDGaze;
    public liblsl.StreamOutlet lslOEyeTrackingHMDGaze; // saved in MeasurementManager.cs
    
    public liblsl.StreamInfo lslIHandTool;
    public liblsl.StreamOutlet lslOHandTool; // saved in MeasurementManager.cs
    
    public liblsl.StreamInfo lslISubjectData;
    public liblsl.StreamOutlet lslOSubjectData; // saved in MeasurementManager.cs

    private void Awake()
    {
        // TODO -------------
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // todo is participant id necessary here for us?
        // todo do we need such a stream?
        lslIFrameTracking = new liblsl.StreamInfo(
            "FrameTracking",
            "Markers",
            1,
            NominalRate,
            liblsl.channel_format_t.cf_int32,
            participantUID);
        lslIFrameTracking.desc().append_child("CurrentFrame");
        lslOFrameTracking = new liblsl.StreamOutlet(lslIFrameTracking);
        // World Coordinates
        // saved: Tobii timestamps (1); origin coordinates (3); direction coordinates (3), Left & right eye blinks (2), Check if ray is valid (1)
        lslIEyeTrackingWorld = new liblsl.StreamInfo(
            "EyeTrackingWorld",
            "Markers",
            10,
            NominalRate,
            liblsl.channel_format_t.cf_float32,
            participantUID);
        lslIEyeTrackingWorld.desc().append_child("ETWTime");
        lslIEyeTrackingWorld.desc().append_child("ETWoriginX");
        lslIEyeTrackingWorld.desc().append_child("ETWoriginY");
        lslIEyeTrackingWorld.desc().append_child("ETWoriginZ");
        lslIEyeTrackingWorld.desc().append_child("ETWdirectionX");
        lslIEyeTrackingWorld.desc().append_child("ETWdirectionY");
        lslIEyeTrackingWorld.desc().append_child("ETWdirectionZ");
        lslIEyeTrackingWorld.desc().append_child("leftBlink");
        lslIEyeTrackingWorld.desc().append_child("rightBlink");
        lslIEyeTrackingWorld.desc().append_child("valid");
        lslOEyeTrackingWorld = new liblsl.StreamOutlet(lslIEyeTrackingWorld);
        // Hit Object Names
        // saved: max 10 objects that the participant could potentially have looked up 
        lslIHitObjectNames = new liblsl.StreamInfo(
            "HitObjectNames",
            "Markers",
            30,
            NominalRate,
            liblsl.channel_format_t.cf_string,
            participantUID);
        lslIHitObjectNames.desc().append_child("HON");
        lslOHitObjectNames = new liblsl.StreamOutlet(lslIHitObjectNames);
        // Hit Object Groups 
        // saved: max 10 object groups that the participant could potentially have looked up 
        lslIHitObjectGroups = new liblsl.StreamInfo(
            "HitObjectGroups",
            "Markers",
            30,
            NominalRate,
            liblsl.channel_format_t.cf_string,
            participantUID);
        lslIHitObjectGroups.desc().append_child("HOG");
        lslOHitObjectGroups = new liblsl.StreamOutlet(lslIHitObjectGroups);
        // Hit Object Coordinates (in World Coordinates)
        // saved: 3 coordinates for each object that was potentially looked up (obj1_x, obj1_y, obj1_z, obj2_x, ...)
        lslIHitObjectPositions = new liblsl.StreamInfo(
            "HitObjectPositions",
            "Markers",
            90,
            NominalRate,
            liblsl.channel_format_t.cf_float32,
            participantUID);
        lslIHitObjectPositions.desc().append_child("HOPX");
        lslIHitObjectPositions.desc().append_child("HOPY");
        lslIHitObjectPositions.desc().append_child("HOPZ");
        lslOHitObjectPositions = new liblsl.StreamOutlet(lslIHitObjectPositions);
        // Hit Positions on Objects (in World Coordinates)
        // saved: 3 coordinates on each object that was potentially looked up (obj1_x, obj1_y, obj1_z, obj2_x, ...)
        lslIHitPositionOnObjects = new liblsl.StreamInfo(
            "HitPositionOnObjects",
            "Markers",
            90,
            NominalRate,
            liblsl.channel_format_t.cf_float32,
            participantUID);
        lslIHitPositionOnObjects.desc().append_child("HPOOX");
        lslIHitPositionOnObjects.desc().append_child("HPOOY");
        lslIHitPositionOnObjects.desc().append_child("HPOOZ");
        lslOHitPositionOnObjects = new liblsl.StreamOutlet(lslIHitPositionOnObjects);
        // Local Coordinates
        // saved: origin coordinates (3); direction coordinates (3)
        lslIEyeTrackingLocal = new liblsl.StreamInfo(
            "EyeTrackingLocal",
            "Markers",
            6,
            NominalRate,
            liblsl.channel_format_t.cf_float32,
            participantUID);
        lslIEyeTrackingLocal.desc().append_child("ETLoriginX");
        lslIEyeTrackingLocal.desc().append_child("ETLoriginY");
        lslIEyeTrackingLocal.desc().append_child("ETLoriginZ");
        lslIEyeTrackingLocal.desc().append_child("ETLdirectionX");
        lslIEyeTrackingLocal.desc().append_child("ETLdirectionY");
        lslIEyeTrackingLocal.desc().append_child("ETLdirectionZ");
        lslOEyeTrackingLocal = new liblsl.StreamOutlet(lslIEyeTrackingLocal);

        // Head Tracking
        // saved: Head (camera) position (3); nose vector (3)
        lslIHeadTracking = new liblsl.StreamInfo(
            "HeadTracking",
            "Markers",
            6,
            NominalRate,
            liblsl.channel_format_t.cf_float32,
            participantUID);
        lslIHeadTracking.desc().append_child("HToriginX");
        lslIHeadTracking.desc().append_child("HToriginY");
        lslIHeadTracking.desc().append_child("HToriginZ");
        lslIHeadTracking.desc().append_child("HTdirectionX");
        lslIHeadTracking.desc().append_child("HTdirectionY");
        lslIHeadTracking.desc().append_child("HTdirectionZ");
        lslOHeadTracking = new liblsl.StreamOutlet(lslIHeadTracking);
        // Hit Object Names (Head Tracking, nose vector)
        // saved: max 10 objects that the participant could potentially have looked up 
        lslIHeadTrackingObjectNames = new liblsl.StreamInfo(
            "HeadTrackingObjectNames",
            "Markers",
            30,
            NominalRate,
            liblsl.channel_format_t.cf_string,
            participantUID);
        lslIHeadTrackingObjectNames.desc().append_child("HTON");
        lslOHeadTrackingObjectNames = new liblsl.StreamOutlet(lslIHeadTrackingObjectNames);
        // Hit Object Groups (Head Tracking, nose vector)
        // saved: max 10 object groups that the participant could potentially have looked up 
        lslIHeadTrackingObjectGroups = new liblsl.StreamInfo(
            "HeadTrackingObjectGroups",
            "Markers",
            30,
            NominalRate,
            liblsl.channel_format_t.cf_string,
            participantUID);
        lslIHeadTrackingObjectGroups.desc().append_child("HTOG");
        lslOHeadTrackingObjectGroups = new liblsl.StreamOutlet(lslIHeadTrackingObjectGroups);
        // Hit Object Coordinates (in World Coordinates) (Head Tracking, nose vector)
        // saved: 3 coordinates for each object that was potentially looked up (obj1_x, obj1_y, obj1_z, obj2_x, ...)
        lslIHeadTrackingObjectPositions = new liblsl.StreamInfo(
            "HeadTrackingObjectPositions",
            "Markers",
            90,
            NominalRate,
            liblsl.channel_format_t.cf_float32,
            participantUID);
        lslIHeadTrackingObjectPositions.desc().append_child("HTOPX");
        lslIHeadTrackingObjectPositions.desc().append_child("HTOPY");
        lslIHeadTrackingObjectPositions.desc().append_child("HTOPZ");
        lslOHeadTrackingObjectPositions = new liblsl.StreamOutlet(lslIHeadTrackingObjectPositions);
        // Hit Positions on Objects (in World Coordinates) (Head Tracking, nose vector)
        // saved: 3 coordinates on each object that was potentially looked up (obj1_x, obj1_y, obj1_z, obj2_x, ...)
        lslIHeadTrackingPositionOnObjects = new liblsl.StreamInfo(
            "HeadTrackingPositionOnObjects",
            "Markers",
            90,
            NominalRate,
            liblsl.channel_format_t.cf_float32,
            participantUID);
        lslIHeadTrackingPositionOnObjects.desc().append_child("HTPOOX");
        lslIHeadTrackingPositionOnObjects.desc().append_child("HTPOOY");
        lslIHeadTrackingPositionOnObjects.desc().append_child("HTPOOZ");
        lslOHeadTrackingPositionOnObjects = new liblsl.StreamOutlet(lslIHeadTrackingPositionOnObjects);
    }
}