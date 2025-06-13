using System.Collections;
using System.Collections.Generic;
using Mirror.Discovery;
using TMPro;
using UnityEngine;

public class NetworkDis : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    private HashSet<string> urlSet = new();

    public void ClearUrls()
    {
        urlSet.Clear();
    }

    public void OnDiscoveredServer(ServerResponse response)
    {
        string storage_url = response.uri.Host + ":" + response.uri.Port;
        if (urlSet.Contains(storage_url))
        {
            return;
        }
        urlSet.Add(storage_url);
        dropdown.AddOptions(new List<string>() { storage_url });
    }
}
