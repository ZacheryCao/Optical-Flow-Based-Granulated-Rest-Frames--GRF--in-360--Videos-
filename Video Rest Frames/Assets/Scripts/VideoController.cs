using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    public VideoClip[] videos;
    private VideoPlayer videoPlayer;
    private int index = 0;
    // Start is called before the first frame update
    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
    }

    // Update is called once per frame

    public void setNextClip()
    {
        index++;
        if (index >= videos.Length || videoPlayer.clip == videos[index]) return;
        videoPlayer.clip = videos[index];
        videoPlayer.Play();
    }

}
