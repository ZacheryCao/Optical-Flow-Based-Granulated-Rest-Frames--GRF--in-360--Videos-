#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDifferentFunctions : MonoBehaviour
{
    public float camX;
    public float inspecX;
    public float camY;
    public float inspecY;
    public Vector4 v;
    private float strideX = 15.00f, strideY=15.00f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        inspecX = -UnityEditor.TransformUtils.GetInspectorRotation(Camera.main.transform).x;
        camX = GetHeadRotation.getPitch();
        inspecY = UnityEditor.TransformUtils.GetInspectorRotation(Camera.main.transform).y;
        camY = GetHeadRotation.getYaw();
        v = indexOfNeighbour();
    }

    private Vector4 indexOfNeighbour()
    {
        Vector2 cam_rotation = new Vector2(camY, camX);
        int x_index1, y_index1; //Centers for most top left overlapped sliding window
        int x_index2, y_index2; //Centers for most bottom right overlapped sliding window
        x_index1 = Mathf.CeilToInt((cam_rotation.x - 44) / strideX) * (int)strideX;
        x_index2 = Mathf.FloorToInt((cam_rotation.x + 44) / strideX) * (int)strideX;
        x_index1 = x_index1 > 165 ? -360 + x_index1 : x_index1 < -165 ? 360 + x_index1: x_index1;
        x_index2 = x_index2 > 165 ? -360 + x_index2 : x_index2 < -165 ? 360 + x_index2: x_index2;

        y_index1 = Mathf.CeilToInt((cam_rotation.y - 97.8f/2) / strideY) * (int)strideY;
        y_index2 = Mathf.FloorToInt((cam_rotation.y + 97.8f/2) / strideY) * (int)strideY;
        y_index1 = y_index1 > 90 ? -y_index1 + 180 : y_index1 < -90 ? -y_index2 - 180 : y_index1;
        y_index2 = y_index2 > 90 ? -y_index2 + 180 : y_index2 < -90 ? -y_index2 - 180 : y_index2;

        return new Vector4(x_index1, x_index2, y_index1, y_index2);
    }
}
#endif