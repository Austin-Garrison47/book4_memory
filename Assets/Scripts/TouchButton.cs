using UnityEngine;
using UnityEngine.EventSystems;

public class TouchButton : MonoBehaviour
{
    public void touchButton()
    {
        GameObject selectedButton =
            EventSystem.current.currentSelectedGameObject;

        int colorNumber = int.Parse(selectedButton.tag);

        GameObject.Find("gameManager")
            .GetComponent<ManageAudioGame>()
            .submitColor(colorNumber);
    }
}