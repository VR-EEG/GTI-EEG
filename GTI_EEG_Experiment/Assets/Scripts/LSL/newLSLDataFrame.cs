using NewExperiment;

namespace LSL
{
    public struct LSLLiveDataFrame
    {
        //LIFE DATA  recorded every frame - all floats 
        //HMD 
        public float HMDPositionX;
        public float HMDPositionY;
        public float HMDPositionZ;

        public float HMDRotationX;
        public float HMDRotationY;
        public float HMDRotationZ;

        //controller
        public float ControllerPositionX;
        public float ControllerPositionY;
        public float ControllerPositionZ;

        public float ControllerRotationX;
        public float ControllerRotationY;
        public float ControllerRotationZ;
        //Gaze Data
        public float CombinedGazePositionX;
        public float CombinedGazePositionY;
        public float CombinedGazePositionZ;

        public float CombinedGazeDirectionX;
        public float CombinedGazeDirectionY;
        public float CombinedGazeDirectionZ;
        
        //HitObjectInfo
        public float IsObjectAttachedToHand; //1 is attached , 0 is not Attached
        public float IsObjectHit; // 1 is hit , 0 is not hit 
        public float HitPositionOnObjectX;
        public float HitPositionOnObjectY;
        public float HitPositionOnObjectZ;
        
    }
    
    public class LSLTimeStampData
    {
        //timestamps for float data 
        public double TimeStamp;
    }
    
    
    
    public class LSLStaticDataFrame
    {
        //STATIC TRIAL DATA -for all trial information- all double
        public double TrialBeginStamp; // 0.5 after ???
        public double ShowCueTimeStamp;
        public double DisappearCueTimeStamp;
        public double ShowToolTimeStamp;
        public double BeepTimeStamp;
        public double TrialEndTimeStamp;

        //TrialInformation
        public double ToolID;  //0-> hammer, 1 -> fork ...
        public double ToolOrientaiton; //0 incongruent , congruent
        public double Task; // 0> use, lift
    }

    public class ConfigurationData
    {
        //SAVED TO JSON
        
        
    }
   
}