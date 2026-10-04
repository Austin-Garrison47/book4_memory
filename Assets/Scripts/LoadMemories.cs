using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.Collections;

public class LoadMemories : MonoBehaviour
{
    public TMP_Text highScoresText;

    void Start()
    {
        StartCoroutine(GetMemories());
    }

    IEnumerator GetMemories()
    {
        UnityWebRequest request =
            UnityWebRequest.Get(
                "http://localhost/getMemories.php"
            );

        yield return request.SendWebRequest();

        if (request.result ==
            UnityWebRequest.Result.Success)
        {
            string data =
                request.downloadHandler.text;

            string[] rows =
                data.Split('\n');

            highScoresText.text =
                "Top 5 Scores\n";

            int rank = 1;

            foreach (string row in rows)
            {
                if (string.IsNullOrWhiteSpace(row))
                    continue;

                string[] parts =
                    row.Split('|');

                if (parts.Length >= 3)
                {
                    highScoresText.text +=
                        rank + ". " +
                        parts[0] +
                        " - " +
                        parts[1] +
                        " colors - " +
                        parts[2] +
                        " sec\n";

                    rank++;
                }
            }
        }
        else
        {
            highScoresText.text =
                "Could not load scores.";
        }
    }
}