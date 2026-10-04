using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class MemoryDatabase : MonoBehaviour
{
    public IEnumerator SaveMemory(
        string playerName,
        int colors,
        float seconds)
    {
        string url =
            "http://localhost/saveMemory.php" +
            "?name=" + UnityWebRequest.EscapeURL(playerName) +
            "&colors=" + colors +
            "&seconds=" + seconds.ToString("F1");

        UnityWebRequest request =
            UnityWebRequest.Get(url);

        yield return request.SendWebRequest();

        if (request.result ==
            UnityWebRequest.Result.Success)
        {
            Debug.Log(request.downloadHandler.text);
        }
        else
        {
            Debug.Log(request.error);
        }
    }
}