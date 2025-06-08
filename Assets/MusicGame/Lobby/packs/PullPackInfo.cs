using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using static OverAll;
using static PackInfo;

public class PullPackInfo : MonoBehaviour
{
    public GameObject packInfoTemplate;
    public GameObject packInfoParent;

    struct PullAPIInfo
    {
        public int code;
        public PackData[] data;
    }

    void Awake()
    {
        StartCoroutine(GetRequest("game/packs/get-packs-info"));
    }

    IEnumerator GetRequest(string url)
    {
        UnityWebRequest request = UnityWebRequest.Get($"{BACKEND_API}/{url}");
        yield return request.SendWebRequest();
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
        }
        else
        {
            PullAPIInfo data = JsonConvert.DeserializeObject<PullAPIInfo>(request.downloadHandler.text);
            if (data.code == 0)
            {
                foreach (PackData packData in data.data)
                {
                    GameObject singlePack = Instantiate(packInfoTemplate, packInfoParent.transform);
                    singlePack.GetComponent<PackInfo>().packData = packData;
                    singlePack.SetActive(true);
                }
            }
        }
    }
}
