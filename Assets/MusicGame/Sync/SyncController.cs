using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class SyncController : NetworkBehaviour
{
    public GameObject triggerButton;
    public GameObject RetryButton;
    public GameObject RetryBigButton;
    public GameObject PracticeButton;
    public GameObject RankingText;
    public GameObject RankingList;

    void Start()
    {
        if (isClient)
        {
            Time.timeScale = 0;
            RetryButton.SetActive(false);
            PracticeButton.SetActive(false);
            RetryBigButton.SetActive(false);
        }
        else
        {
            RankingText.SetActive(false);
            RankingList.SetActive(false);
        }
        if (isServer)
        {
            triggerButton.SetActive(true);
        }
    }

    [Server]
    public void TriggerStart()
    {
        triggerButton.SetActive(false);
        RpcStart();
    }

    [ClientRpc]
    void RpcStart()
    {
        Time.timeScale = 1;
    }
}
