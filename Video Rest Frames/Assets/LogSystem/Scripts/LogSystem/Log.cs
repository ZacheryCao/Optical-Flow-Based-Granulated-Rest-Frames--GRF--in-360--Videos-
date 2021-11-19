using System.IO;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace LogSystem
{
    public class Log : MonoBehaviour
    {
        protected StreamWriter log;
        protected virtual void Create(string _id, string _condition, string _type) 
        {
            log = File.CreateText(Application.persistentDataPath + "/Condition-" + _condition + "---" + System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".csv");
        }
        protected virtual void Close() 
        {
            if (log == null) return;
            log.Close();
        }
        protected virtual void Flush() 
        {
            log.Flush();
        }
        protected virtual void Save(List<List<string>> t) { }
        protected virtual void WriteLine(string _line) 
        {
            log.WriteLine(_line);
        }
    }
}