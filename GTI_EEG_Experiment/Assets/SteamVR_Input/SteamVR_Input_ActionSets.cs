//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Valve.VR
{
    using System;
    using UnityEngine;
    
    
    public partial class SteamVR_Actions
    {
        
        private static SteamVR_Input_ActionSet_default p__default;
        
        private static SteamVR_Input_ActionSet_platformer p_platformer;
        
        private static SteamVR_Input_ActionSet_buggy p_buggy;
        
        private static SteamVR_Input_ActionSet_mixedreality p_mixedreality;
        
        private static SteamVR_Input_ActionSet_leaptool p_leaptool;
        
        private static SteamVR_Input_ActionSet_tableCalibration p_tableCalibration;
        
        public static SteamVR_Input_ActionSet_default _default
        {
            get
            {
                return SteamVR_Actions.p__default.GetCopy<SteamVR_Input_ActionSet_default>();
            }
        }
        
        public static SteamVR_Input_ActionSet_platformer platformer
        {
            get
            {
                return SteamVR_Actions.p_platformer.GetCopy<SteamVR_Input_ActionSet_platformer>();
            }
        }
        
        public static SteamVR_Input_ActionSet_buggy buggy
        {
            get
            {
                return SteamVR_Actions.p_buggy.GetCopy<SteamVR_Input_ActionSet_buggy>();
            }
        }
        
        public static SteamVR_Input_ActionSet_mixedreality mixedreality
        {
            get
            {
                return SteamVR_Actions.p_mixedreality.GetCopy<SteamVR_Input_ActionSet_mixedreality>();
            }
        }
        
        public static SteamVR_Input_ActionSet_leaptool leaptool
        {
            get
            {
                return SteamVR_Actions.p_leaptool.GetCopy<SteamVR_Input_ActionSet_leaptool>();
            }
        }
        
        public static SteamVR_Input_ActionSet_tableCalibration tableCalibration
        {
            get
            {
                return SteamVR_Actions.p_tableCalibration.GetCopy<SteamVR_Input_ActionSet_tableCalibration>();
            }
        }
        
        private static void StartPreInitActionSets()
        {
            SteamVR_Actions.p__default = ((SteamVR_Input_ActionSet_default)(SteamVR_ActionSet.Create<SteamVR_Input_ActionSet_default>("/actions/default")));
            SteamVR_Actions.p_platformer = ((SteamVR_Input_ActionSet_platformer)(SteamVR_ActionSet.Create<SteamVR_Input_ActionSet_platformer>("/actions/platformer")));
            SteamVR_Actions.p_buggy = ((SteamVR_Input_ActionSet_buggy)(SteamVR_ActionSet.Create<SteamVR_Input_ActionSet_buggy>("/actions/buggy")));
            SteamVR_Actions.p_mixedreality = ((SteamVR_Input_ActionSet_mixedreality)(SteamVR_ActionSet.Create<SteamVR_Input_ActionSet_mixedreality>("/actions/mixedreality")));
            SteamVR_Actions.p_leaptool = ((SteamVR_Input_ActionSet_leaptool)(SteamVR_ActionSet.Create<SteamVR_Input_ActionSet_leaptool>("/actions/leaptool")));
            SteamVR_Actions.p_tableCalibration = ((SteamVR_Input_ActionSet_tableCalibration)(SteamVR_ActionSet.Create<SteamVR_Input_ActionSet_tableCalibration>("/actions/tableCalibration")));
            Valve.VR.SteamVR_Input.actionSets = new Valve.VR.SteamVR_ActionSet[] {
                    SteamVR_Actions._default,
                    SteamVR_Actions.platformer,
                    SteamVR_Actions.buggy,
                    SteamVR_Actions.mixedreality,
                    SteamVR_Actions.leaptool,
                    SteamVR_Actions.tableCalibration};
        }
    }
}
