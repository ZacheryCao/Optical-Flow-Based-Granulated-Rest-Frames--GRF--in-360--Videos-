using Sigtrap.VrTunnellingPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aligncages : MonoBehaviour
{
    [SerializeField]
    private GameObject cage;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        cage.transform.position = Camera.main.transform.position;        
    }
}
