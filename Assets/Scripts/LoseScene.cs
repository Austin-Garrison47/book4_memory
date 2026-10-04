using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LoseScene : MonoBehaviour
{
    public TMP_Text scoreText;

    void Start()
    {
        int score = PlayerPrefs.GetInt("score", 0);
        scoreText.text = "Score: " + score;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("chapter2");
    }
}
