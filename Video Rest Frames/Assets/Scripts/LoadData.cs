using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using System;
using LogSystem;
using GoogleSheetsToUnity;

public class LoadData : MonoBehaviour
{
    Dictionary<string, Dictionary<(int, int), List<float>>> data = new Dictionary<string, Dictionary<(int, int), List<float>>>();
    public VideoPlayer video;
    [SerializeField] private Camera camera;
    public Renderer material;
    private float Opacity = 0.0f, strideX = 15.0f, strideY = 15.0f;
    Color color;
    private LogHeadRotation log;
    float min, max, range;
    private VideoController videoController;
    [SerializeField]
    private string folder;
    private List<Vector2> rotation = new List<Vector2>();
    private int preframe = -1;
    private List<List<string>> rotData = new List<List<string>>();
    private bool token;
    private int bufferLimit = 60;
    private int counterFinal = 0;
    [SerializeField]
    private ApplicationController appController;
    public bool test;

    void Start()
    {
        videoController = video.transform.GetComponent<VideoController>();
        folder = video.clip.name;
        test = false;
        token = false;
        //data_matrix = new float[video.frameCount, 67, 61];
        //local_path = AssetDatabase.GetAssetPath(dataFile);
        //min_max = DataHandler.loadFile(local_path, ref data_matrix);
        color = material.GetComponent<MeshRenderer>().sharedMaterial.color;
        color.a = Opacity;
        material.sharedMaterial.color = color;
        min = DataHandler.loadFile(folder + "/percentileData")[0];
        max = DataHandler.loadFile(folder + "/percentileData")[1];
        Debug.Log("min: " + min + "; max: " + max);
        range = max - min;
        log = (LogHeadRotation)FindObjectOfType(typeof(LogHeadRotation));
        foreach(VideoClip clip in videoController.videos)
        {
            data[clip.name] = (DataHandler.getDataMatrix(clip.name));
        }
    }

    Vector2 cameraRotation()
    {
        return new Vector2(GetHeadRotation.getYaw(), GetHeadRotation.getPitch());
    }


    private Vector4 indexOfNeighbour()
    {
        Vector2 cam_rotation = cameraRotation();
        int x_index1, y_index1; //Centers for most top left overlapped sliding window
        int x_index2, y_index2; //Centers for most bottom right overlapped sliding window
        x_index1 = Mathf.CeilToInt(cam_rotation.x  / strideX) * (int)strideX;
        x_index2 = Mathf.FloorToInt(cam_rotation.x / strideX) * (int)strideX;
        x_index1 = x_index1 > 165 ? -360 + x_index1 : x_index1 < -165 ? 360 + x_index1 : x_index1;
        x_index2 = x_index2 > 165 ? -360 + x_index2 : x_index2 < -165 ? 360 + x_index2 : x_index2;

        y_index1 = Mathf.CeilToInt(cam_rotation.y / strideY) * (int)strideY;
        y_index2 = Mathf.FloorToInt(cam_rotation.y  / strideY) * (int)strideY;
        y_index1 = y_index1 > 90 ? -y_index1 + 180 : y_index1 < -90 ? -y_index2 - 180 : y_index1;
        y_index2 = y_index2 > 90 ? -y_index2 + 180 : y_index2 < -90 ? -y_index2 - 180 : y_index2;

        return new Vector4(x_index1, x_index2, y_index1, y_index2);
    }


    private void Update()
    {
        folder = video.clip.name;
        if (counterFinal == bufferLimit && !test) // append to spreasheet if the buffer list reaches the buffer limit
        {
            List<List<string>> finalListToSave = new List<List<string>>();
            foreach (var insideList in rotData)
                finalListToSave.Add(insideList);
            log.LogPerformance(finalListToSave);
            rotData = new List<List<string>>();
            counterFinal = 0;
        }
        if ((int)video.frame != preframe || test)
        {
            renderGRF();
            preframe = (int)video.frame;
            List<string> list = log.getDummyData(folder);
            list.Add((video.frame+1).ToString());
            list.Add(GetHeadRotation.getYaw().ToString());
            list.Add(GetHeadRotation.getPitch().ToString());
            list.Add(color.a.ToString());
            rotData.Add(list);
            counterFinal++;
        }
        if ((int)video.frame == (int)video.frameCount - 1 && !video.isPlaying && !test) // append to spreasheet if the buffer list reaches the buffer limit
        {
            List<List<string>> finalListToSave = new List<List<string>>();
            foreach (var insideList in rotData)
                finalListToSave.Add(insideList);
            log.LogPerformance(finalListToSave);
        }

        if((int)video.frame == (int)video.frameCount - 1 && !video.isPlaying)
        {
            if(folder != videoController.videos[videoController.videos.Length-1].name)
            {
                videoController.setNextClip();
            }
            else
            {
                appController.exit = true;
            }
            
        }
    }

    private void renderGRF()
    {
        float alpha = (opticFlow(folder) - min) / range;
        color.a = alpha < 0 ? 0 : alpha > 1 ? 1 : alpha;
        material.sharedMaterial.color = color;
    }

    private float opticFlow(string dataFolder)
    {
        Vector4 neighBor = indexOfNeighbour();
        float flo = 0.000f;
        if ((int)video.frame < 1 || (int)video.frame > data[folder][(0, 0)].Count - 1) return 0;
        Vector2 coord = DataHandler.nearestWindow(neighBor, cameraRotation());
        return data[folder][((int)coord.x, (int)coord.y)][(int)video.frame - 1];
    }

    //private float opticFlow(string dataFolder)
    //{
    //    float flow_left_bottom, flow_right_bottom, flow_left_top, flow_right_top;
    //    Vector4 neighBor = indexOfNeighbour();
    //    if ((int)video.frame < 1 || (int)video.frame > data[(0, 0)].Count - 1) return 0;
    //    flow_left_bottom = data[((int)neighBor.x, (int)neighBor.z)][(int)video.frame-1];
    //    flow_left_top = data[((int)neighBor.x, (int)neighBor.w)][(int)video.frame - 1];
    //    flow_right_bottom = data[((int)neighBor.y, (int)neighBor.z)][(int)video.frame - 1];
    //    flow_right_top = data[((int)neighBor.y, (int)neighBor.w)][(int)video.frame - 1];
    //    return DataHandler.OpFlo(neighBor,cameraRotation(),flow_left_top, flow_right_top, flow_left_bottom, flow_right_bottom);
    //}

    //private float opticFlow(string dataFolder)
    //{
    //    Vector4 neighBor = indexOfNeighbour();
    //    float flo = 0.000f;
    //    float windowsCount = 0.000f;
    //    Debug.Log("Frame: " + video.frame);
    //    Debug.Log("last: " + (data[(0, 0)].Count - 1));
    //    if ((int)video.frame < 1 || (int)video.frame > data[(0, 0)].Count - 1) return 0;
    //    for (float i = neighBor.x; i <= neighBor.z; i += 15)
    //    {
    //        for (float j = neighBor.y; j <= neighBor.w; j += 15)
    //        {
    //            flo += data[((int)i, (int)j)][(int)video.frame - 1]* weight((int)i, (int)j);
    //            windowsCount += weight((int)i, (int)j);
    //            Debug.Log("Flo: " + data[((int)i, (int)j)][(int)video.frame - 1]);
    //            Debug.Log("Weight: " + weight((int)i, (int)j));
    //        }
    //    }
    //    return flo / windowsCount;
    //}

    private float weight(int x, int y)
    {
        Vector2 camR = cameraRotation();
        float h = 0.000f;
        float w = 0.000f;
        if(x <= camR.x)
        {
            w = 88.000f + (float)x - (float)camR.x;
        }
        else
        {
            w = 88.000f + (float)camR.x - (float)x;
        }
        if(y >= camR.y)
        {
            h = 88.000f * 160.000f / 144.000f + (float)y - (float)camR.y;
        }
        else
        {
            h = 88.000f * 160.000f / 144.000f - (float)y + (float)camR.y;
        }
        return h * w / (88.000f * 88.000f * 160.000f / 144.000f);
    }

    //private float opticFlow(string dataFolder)
    //{
    //    Vector2 camR = cameraRotation();
    //    Vector4 neighBor = indexOfNeighbour();
    //    List<Vector2> n = new List<Vector2>();
    //    n.Add(new Vector2(neighBor.x, neighBor.w));
    //    n.Add(new Vector2(neighBor.x, neighBor.z));
    //    n.Add(new Vector2(neighBor.y, neighBor.z));
    //    n.Add(new Vector2(neighBor.y, neighBor.w));
    //    float o = data[((int)neighBor.x, (int)neighBor.w)][(int)video.frame - 1];
    //    float d = Vector2.Distance(new Vector2(neighBor.x, neighBor.w), camR);
    //    foreach (Vector2 item in n)
    //    {
    //        float dd = Vector2.Distance(item, camR);
    //        if (dd < d)
    //        {
    //            d = dd;
    //            o = data[((int)item.x, (int)item.y)][(int)video.frame - 1];
    //        }
    //    }
    //    return o;
    //}

}
