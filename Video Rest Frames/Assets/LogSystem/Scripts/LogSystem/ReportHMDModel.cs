using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleSheetsToUnity;
using System;
using Valve.VR;
using UnityEngine.XR;

namespace LogSystem
{

    public class ReportHMDModel : MonoBehaviour
    {
        public static Action<string> OnReportHMDModel;
        public static Action<string> OnReportControllerModel;

        void Start()
        {
            OnReportHMDModel(SteamVR.instance.hmd_ModelNumber);
            OnReportControllerModel(GetControllerTypeSteamVRToString());
        }

        public static string GetControllerTypeSteamVRToString()
        {
            if (SteamVR.initializedState == SteamVR.InitializedStates.InitializeFailure)
                return null;

            var rightHand = OpenVR.System.GetTrackedDeviceIndexForControllerRole(ETrackedControllerRole.RightHand);
            var sb = new System.Text.StringBuilder((int)64);
            ETrackedPropertyError propError = ETrackedPropertyError.TrackedProp_UnknownProperty;
            OpenVR.System.GetStringTrackedDeviceProperty(rightHand, ETrackedDeviceProperty.Prop_ModelNumber_String, sb, 2000, ref propError);
            return sb.ToString();
        }
    }
}