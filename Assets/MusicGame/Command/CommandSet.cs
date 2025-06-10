using Mirror;
using Mirror.Discovery;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CommandSet : MonoBehaviour
{
    public void Retry()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MusicGame");
    }

    public void Exit()
    {
        Time.timeScale = 1;
        NetworkClient.Shutdown();
        NetworkServer.Shutdown();
        GameObject manager = GameObject.Find("NetworkManager");
        if (manager)
        {
            manager.GetComponent<NetworkDiscovery>().StopDiscovery();
            DestroyImmediate(manager);
        }
        SceneManager.LoadScene("MusicLobby");
    }

    public void GoToStart()
    {
        GameObject manager = GameObject.Find("NetworkManager");
        if (manager)
        {
            Destroy(manager);
        }
        SceneManager.LoadScene("Initalize");
    }

    public void GoToInstallPacks()
    {
        SceneManager.LoadScene("DownloadLobby");
    }
}
