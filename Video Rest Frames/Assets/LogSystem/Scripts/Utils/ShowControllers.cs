using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class ShowControllers : MonoBehaviour
{
    public bool showController = true;

    private void FixedUpdate()

    //protected void OnHandInitialized(int deviceIndex) 
    {
        var hand = GetComponent<Hand>();
        if (showController)
        {
            hand.ShowController();
            hand.SetSkeletonRangeOfMotion(Valve.VR.EVRSkeletalMotionRange.WithController);
        }
        else
        {
            hand.HideController();
            hand.SetSkeletonRangeOfMotion(Valve.VR.EVRSkeletalMotionRange.WithController);
        }
    }

}
