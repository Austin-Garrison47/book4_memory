using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class ManagePreferences : MonoBehaviour
{
    public TMP_InputField playerNameInput;
    public TMP_Dropdown colorDropdown;
    public Slider timeSlider;

    public void StartGame()
    {
        PlayerPrefs.SetString("playerName", playerNameInput.text);

        int numberOfColors = int.Parse(
            colorDropdown.options[colorDropdown.value].text.Split(' ')[0]
        );

        PlayerPrefs.SetInt("numberOfColors", numberOfColors);
        PlayerPrefs.SetFloat("timeLimit", timeSlider.value);

        PlayerPrefs.Save();

        SceneManager.LoadScene("chapter2");
    }
}