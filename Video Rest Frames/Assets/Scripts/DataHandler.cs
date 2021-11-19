using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

public static class DataHandler
{

    public static Dictionary<(int, int), List<float>> getDataMatrix(string dataFolder)
    {
        Dictionary<(int, int), List<float>> dataMatrix = new Dictionary<(int, int), List<float>>();

        for (int i = -180; i<180; i+=15)
        {
            for(int j = -90; j <=90; j+=15)
            {
                string path = dataFolder + "/horizontal_" + i.ToString() + "_vertical_" + j.ToString();
                TextAsset strReader = (TextAsset)Resources.Load(path);
                string[] records = strReader.text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
                List<float> OpticFlow = new List<float>();
                foreach (string s in records)
                {
                    if (s == "") continue;
                    var contentSplited = s.Split(',');
                    if (contentSplited[1] == "") continue;
                    if (float.TryParse(contentSplited[1], out float z))
                    {
                        OpticFlow.Add(z);
                    }
                    else
                    {
                        OpticFlow.Add(0);
                    }
                }
                dataMatrix[(i, j)] = OpticFlow;
            }
        }
        return dataMatrix;
    }

    public static Vector2 nearestWindow(Vector4 vec, Vector2 headRot)
    {
        float a, b;
        if(Mathf.Abs(vec.x-headRot.x)> Mathf.Abs(vec.y - headRot.x))
        {
            a = vec.y;
        }
        else
        {
            a = vec.x;
        }
        if (Mathf.Abs(vec.z - headRot.y) > Mathf.Abs(vec.w - headRot.y))
        {
            b = vec.w;
        }
        else
        {
            b = vec.z;
        }
        return new Vector2(a, b);
    }

    public static float OpFlo(Vector4 vec, Vector2 headRot, float tl, float tr, float bl, float br)
    {
        float ratio1 = (15 - (headRot.x - vec.x)) / 15.00f;
        float ratio2 = (15 - (headRot.y - vec.w)) / 15.00f;
        return (1 - ratio1) * (ratio2 * bl + (1 - ratio2) * tl) + ratio1 * (ratio2 * br + (1 - ratio2) * tr);
    }

    public static float calFlow(float a, float b, float c, float d)
    {
        float average;
        average = 0.018f * (a + b + c + d);
        average += 0.761f * (a + b + c + d) / 4;
        average += 0.108f * (a + b + c + d) / 2;
        average += 0.122f * (a + b + c + d) / 2;
        return average * 0.798f;
    }

    public static Vector2 loadFile(string filePath)
    {
        Vector2 tmp = new Vector2(0,0);
        TextAsset strReader = (TextAsset)Resources.Load(filePath);
        Debug.Log(strReader.text);
        string[] records = strReader.text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
        if (float.TryParse(records[0], out float z))
        {
            tmp.x = z;
        }
        if (float.TryParse(records[1], out float t))
        {
            tmp.y = t;
        }
        return tmp;
    }


}

