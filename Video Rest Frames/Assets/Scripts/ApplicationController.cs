using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationController : MonoBehaviour
{
    private IEnumerator coroutine;
    public bool exit;
    // Start is called before the first frame update
    void Start()
    {
        exit = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (exit)
        {
            coroutine = AppQuit(2f);
            StartCoroutine(coroutine);
        }
    }


    private IEnumerator AppQuit(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Application.OpenURL("https://uncg.qualtrics.com/jfe/form/SV_0rG3j4DsfUeT24u");
        Application.Quit();
    }
}
