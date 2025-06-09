using Mirror;
using UnityEngine;

using UnityEngine.SceneManagement;

public class ResetAll : MonoBehaviour
{
    // void Awake()
    // {
    //     DontDestroyOnLoad(gameObject);
    // }

    public void GameReset()
    {
        // if (NetworkClient.isConnected)
        // {
        Time.timeScale = 1;
        SceneManager.LoadScene("Running");
        // }
        // else
        // {
        //     SceneManager.LoadScene("Running");
        // }
    }

    public void GameExit()
    {
        Time.timeScale = 1;
        DataStorager.SaveStatus();
        NetworkClient.Disconnect();
        // Destroy(GameObject.Find("MultyScript"));
        GameObject manager = GameObject.Find("MultyScript");
        if (manager)
        {
            Destroy(manager);
        }
        SceneManager.LoadScene("Initalize");
    }

    public void GameStart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Running");
    }

    public void GoToShop(){
        SceneManager.LoadScene("Shop");
    }

    public void GoToSettings(){
        SceneManager.LoadScene("Settings");
    }

    public void GoToMusic(){
        GameObject manager = GameObject.Find("MultyScript");
        if (manager)
        {
            Destroy(manager);
        }
        SceneManager.LoadScene("MusicLobby");
    }
}
