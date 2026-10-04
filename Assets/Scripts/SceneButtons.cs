using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneButtons : MonoBehaviour
{
    public void GoToPreferences()
    {
        SceneManager.LoadScene("preferences");
    }

    public void GoToGame()
    {
        SceneManager.LoadScene("chapter2");
    }

    public void GoToExit()
    {
        SceneManager.LoadScene("exit");
    }

    public void GoToIntro()
    {
        SceneManager.LoadScene("intro");
    }
}