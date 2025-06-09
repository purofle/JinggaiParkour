using Mirror;
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
        NetworkClient.Disconnect();
        GameObject manager = GameObject.Find("NetworkManager");
        if (manager)
        {
            Destroy(manager);
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
