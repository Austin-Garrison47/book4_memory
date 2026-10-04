using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ExitScene : MonoBehaviour
{
    public TMP_Text playerText;
    public TMP_Text colorsText;
    public TMP_Text timeText;

    void Start()
    {
        string playerName =
            PlayerPrefs.GetString("finalPlayerName", "Player");

        int colors =
            PlayerPrefs.GetInt("finalColors", 2);

        float time =
            PlayerPrefs.GetFloat("finalTime", 0f);

        playerText.text =
            "Player: " + playerName;

        colorsText.text =
            "Colors: " + colors;

        timeText.text =
            "Time: " + time.ToString("F1") + " seconds";
    }

    public void PlayAgain()
    {
        Time.timeScale = 1;

        SceneManager.LoadScene("intro");
    }
}