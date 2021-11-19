using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using Valve.VR.InteractionSystem;
using Valve.VR;
using Sigtrap.VrTunnellingPro;

public class ApplicationDebug : MonoBehaviour
{
    [SerializeField]
    VideoPlayer video;


    [SerializeField]
    LoadData GRFRender;

    [SerializeField]
    Tunnelling GRF;


    [SerializeField]
    SteamVR_Action_Boolean Pause;

    [SerializeField]
    SteamVR_Action_Boolean GRFActive;

    bool videoStarted = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (video.isPlaying)
        {
            videoStarted = true;
        }
        if (videoStarted)
        {
            GRFRender.test = true;
            if (Pause.GetStateDown(SteamVR_Input_Sources.RightHand))
            {
                if (video.isPlaying)
                    video.Pause();
                else
                    video.Play();
            }
            if (GRFActive.GetStateDown(SteamVR_Input_Sources.RightHand))
            {
                GRF.enabled = !GRF.isActiveAndEnabled;
            }
        }
        
    }
}
