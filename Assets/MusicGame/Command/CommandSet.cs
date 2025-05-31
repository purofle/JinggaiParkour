using UnityEngine;
using UnityEngine.SceneManagement;

public class CommandSet : MonoBehaviour
{
    public void Retry(){
        SceneManager.LoadScene("MusicGame");
    }

    public void Exit(){
        Time.timeScale = 1;
        SceneManager.LoadScene("MusicLobby");
    }

    public void GoToStart(){
        SceneManager.LoadScene("Start");
    }
}
