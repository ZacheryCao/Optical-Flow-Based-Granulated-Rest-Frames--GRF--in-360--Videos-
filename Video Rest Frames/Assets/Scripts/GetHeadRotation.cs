using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GetHeadRotation
{
    
    public static float getPitch()
    {
        return -WrapAngle(Camera.main.transform.eulerAngles.x);
        //return -UnityEditor.TransformUtils.GetInspectorRotation(Camera.main.transform).x;
    }

    public static float getYaw()
    {
        return WrapAngle(Camera.main.transform.eulerAngles.y);
        //return UnityEditor.TransformUtils.GetInspectorRotation(Camera.main.transform).y;
    }

    private static float WrapAngle(float angle)
    {
        angle %= 360;
        if (angle > 180)
            return angle - 360;

        return angle;
    }

    private static float UnwrapAngle(float angle)
    {
        if (angle >= 0)
            return angle;

        angle = -angle % 360;

        return 360 - angle;
    }
}
