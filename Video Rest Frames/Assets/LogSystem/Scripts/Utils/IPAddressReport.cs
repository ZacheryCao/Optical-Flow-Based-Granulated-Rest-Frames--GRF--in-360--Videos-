using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;

public class IPAddressReport : MonoBehaviour {
    public static Action<string, string> OnReportIpAddress;
    public string ExternalIP { get; set; }
    public string LocalIP { get; set; }

    void Start()
    {
        GetLocalIPv4();
        StartCoroutine(GetIPAddress());
    }

    public void GetLocalIPv4()
    {
        LocalIP = Dns.GetHostEntry(Dns.GetHostName())
            .AddressList.First(
                f => f.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            .ToString();

    }

    //Taken from Goodgulf
    IEnumerator GetIPAddress()
    {
        UnityWebRequest www = UnityWebRequest.Get("http://checkip.dyndns.org");
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            string result = www.downloadHandler.text;

            // This results in a string similar to this: <html><head><title>Current IP Check</title></head><body>Current IP Address: 123.123.123.123</body></html>
            // where 123.123.123.123 is your external IP Address.

            string[] a = result.Split(':'); // Split into two substrings -> one before : and one after. 
            string a2 = a[1].Substring(1);  // Get the substring after the :
            string[] a3 = a2.Split('<');    // Now split to the first HTML tag after the IP address.
            string a4 = a3[0];              // Get the substring before the tag.

            ExternalIP = a4;
            OnReportIpAddress(ExternalIP, LocalIP);
        }
    }
}