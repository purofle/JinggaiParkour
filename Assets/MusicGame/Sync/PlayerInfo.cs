using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PlayerInfo : NetworkBehaviour
{
    [SyncVar]
    public float achievement;

    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        CmdUpdateInfo(BeatmapManager.Instance.GetNegaProgress());
    }

    [Command]
    void CmdUpdateInfo(float achi)
    {
        achievement = achi;
    }
}
