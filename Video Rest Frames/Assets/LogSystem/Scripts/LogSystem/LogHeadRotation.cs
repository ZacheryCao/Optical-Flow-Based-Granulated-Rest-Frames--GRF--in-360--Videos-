using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Valve.VR.InteractionSystem;
using GoogleSheetsToUnity;
using Valve.VR;
using UnityEngine.Video;

namespace LogSystem
{
    [RequireComponent(typeof(ReportHMDModel))]
    [RequireComponent(typeof(IPAddressReport))]
    public class LogHeadRotation : Log
    {
        public enum Condition
        {
            GRF,
            NoGRF,
            none
        }
        [SerializeField]
        private string videoName;
        public Condition condition;
        private string userID = "-1";
        private string controllerModel = "";
        private string hmdModel = "";
        private bool init = false;
        private string associatedSheet = "";
        private string associatedWorksheet = "";

        private string externalIP;
        private string localIP;

        private string headRotationYaw = "";
        private string headRotationPitch = "";
        private string exposedOpticalFlow = "";
        private string frameNumber = "";


        private void Awake()
        {
            ButtonsControllerMainScreen.OnSetUserID += BufferUserID;
            ReportHMDModel.OnReportHMDModel += BufferHMDModel;
            IPAddressReport.OnReportIpAddress += BufferIpAddresses;
            ReportHMDModel.OnReportControllerModel += BufferControllerModel;
        }
        private void Start()
        {
            associatedWorksheet = condition.ToString();
            BufferConditionInfoExperiment(condition);
        }

        public void setVideoName(string name)
        {
            videoName = name;
        }

        private void Update()
        {
            if (!init && externalIP!=null)
            {
                SpreadsheetManager.Append(new GSTU_Search(associatedSheet, associatedWorksheet), new ValueRange(getDummyData(videoName)), null);
                //StartCoroutine(BufferInitInfo());
                init = true;
            }
            videoName = GameObject.FindObjectOfType<VideoPlayer>().clip.name;
        }

        private void OnDestroy()
        {
            Close();
            ButtonsControllerMainScreen.OnSetUserID -= BufferUserID;
            ReportHMDModel.OnReportHMDModel -= BufferHMDModel;
            IPAddressReport.OnReportIpAddress -= BufferIpAddresses;
            ReportHMDModel.OnReportControllerModel -= BufferControllerModel;
        }

        IEnumerator BufferInitInfo()
        {
            yield return new WaitForSeconds(1f);
            SpreadsheetManager.Append(new GSTU_Search(associatedSheet, associatedWorksheet), new ValueRange(getDummyData(videoName)), null);
        }

        private void BufferIpAddresses(string _externalIP, string _localIP)
        {
            externalIP = _externalIP;
            localIP = _localIP;
        }

        private void BufferHMDModel(string _hmdModel)
        {
            hmdModel = _hmdModel;
        }

        private void BufferControllerModel(string _controllerModel)
        {
            controllerModel = _controllerModel;
        }

        protected void BufferConditionInfoExperiment(Condition _condition)
        {
            Create(userID, _condition.ToString(), associatedWorksheet);
            condition = _condition;
            string header = "Date;VideoName;Condition;ExternalIP;InternalIP;UserID;HMDModel;ControllerModel;Frame No.;HeadRotation_Yaw;HeadRotation_Pitch;ExposedOpticalFlow";
            WriteLine(header);
        }

        public void BufferUserID(string _id)
        {
            userID = _id;
        }


        public void LogPerformance(List<List<string>> dataList)
        {
            Save(dataList);
        }

        public void LogEarlyQuit(List<List<string>> data, string videoName)
        {
            List<string> list = new List<string>();
            list.Add(System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
            list.Add(videoName);
            list.Add(externalIP);
            list.Add(localIP);
            list.Add("Earlyquit");
            SpreadsheetManager.Append(new GSTU_Search(associatedSheet, associatedWorksheet), new ValueRange(list), null);
            SaveLocal(data, true);
        }

        protected override void Save(List<List<string>> data)
        {
            SaveLocal(data, false);
            SaveRemote(data);
        }

        public List<string> getDummyData(string videoName)
        {
            List<string> list = new List<string>();
            list.Add(System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
            list.Add(videoName);
            list.Add(externalIP);
            list.Add(localIP);
            list.Add(userID);
            list.Add(hmdModel);
            list.Add(controllerModel);
            return list;
        }

        private void SaveRemote(List<List<string>> data)
        {
            SpreadsheetManager.Append(new GSTU_Search(associatedSheet, associatedWorksheet), new ValueRange(data), null);
        }

        private void SaveLocal(List<List<string>> data, bool earlyQuit)
        {
            foreach(List<string> i in data)
            {
                string line = "";
                line += i[0] + ";";
                line += i[1] + ";";
                line += i[2] + ";" + i[3] + ";";
                line += i[4] + ";" + i[5] + ";" + i[6] + ";";
                if (earlyQuit)
                {
                    line += "Early Quit;";
                }
                else
                {
                    line += i[7] + ";" + i[8] + ";";
                    line += i[9] + ";" + i[10];
                }
                WriteLine(line);
                Flush();
            }

        }
    }
}