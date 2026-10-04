using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class ManageAudioGame : MonoBehaviour
{
    private const int STATE_PLAY_SEQUENCE = 1;
    private const int STATE_WAIT_FOR_USER_INPUT = 2;
    private const int STATE_PROCESS_USER_INPUT = 3;

    private int currentState;

    int[] sequenceOfColor = new int[100];
    int[] userSequence = new int[100];

    int indexOfColor = 0;

    bool newColorHasBeenGenerated;
    bool allBoxesDisplayed;
    bool startTimer;
    bool waitingTimerActivated;
    bool waitingTimeElapsed;

    float timer;
    float waitingTime;
    float timeDelayBetweenDisplays;

    int animationIndex;
    int nbColorsSubmitted;

    public TMP_Text nbMemorized;
    public TMP_Text playerNameText;
    public TMP_Text timerText;

    int numberOfColors;

    float elapsedTime;
    float timeLimit;

    bool gameOver = false;
    bool isPaused = false;

    AudioSource audioSource;

    void Start()
    {
        Time.timeScale = 1;

        currentState = STATE_PLAY_SEQUENCE;

        sequenceOfColor = new int[100];
        userSequence = new int[100];

        newColorHasBeenGenerated = false;
        allBoxesDisplayed = false;
        startTimer = false;
        waitingTimerActivated = false;
        waitingTimeElapsed = false;

        timer = 0;
        waitingTime = 0;

        animationIndex = 0;
        nbColorsSubmitted = 0;

        timeDelayBetweenDisplays = 1.0f;

        audioSource = GetComponent<AudioSource>();

        numberOfColors = PlayerPrefs.GetInt("numberOfColors", 4);

        configureColors();

        string playerName =
            PlayerPrefs.GetString("playerName", "Player");

        playerNameText.text = "Player: " + playerName;

        timeLimit =
            PlayerPrefs.GetFloat("timeLimit", 60f);

        elapsedTime = 0f;

        updateUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;

            if (isPaused)
                Time.timeScale = 0;
            else
                Time.timeScale = 1;
        }

        if (isPaused)
            return;

        if (!gameOver)
        {
            elapsedTime += Time.deltaTime;

            timerText.text =
                "Time: " + elapsedTime.ToString("F1");

            if (elapsedTime >= timeLimit)
            {
                gameOver = true;

                SaveGameInfo();

                Time.timeScale = 1;

                SceneManager.LoadScene("exit");

                return;
            }
        }

        switch (currentState)
        {
            case STATE_PLAY_SEQUENCE:

                if (!newColorHasBeenGenerated)
                {
                    initTimer();
                    generateNewColor();

                    newColorHasBeenGenerated = true;
                    allBoxesDisplayed = false;

                    hideBoxes();
                }

                timer += Time.deltaTime;

                if (startTimer &&
                    timer >= timeDelayBetweenDisplays &&
                    !waitingTimerActivated)
                {
                    timer = 0;

                    hideBoxes();

                    displayBox(
                        sequenceOfColor[animationIndex]
                    );

                    animationIndex++;

                    if (animationIndex >= indexOfColor)
                    {
                        startTimer = false;
                        animationIndex = 0;
                        waitingTimerActivated = true;
                    }
                }

                if (waitingTimerActivated)
                {
                    waitingTime += Time.deltaTime;

                    if (waitingTime >= 0.5f)
                    {
                        hideBoxes();

                        waitingTime = 0;
                        waitingTimerActivated = false;
                        waitingTimeElapsed = true;

                        showAllActiveColors();

                        currentState =
                            STATE_WAIT_FOR_USER_INPUT;
                    }
                }

                break;

            case STATE_WAIT_FOR_USER_INPUT:
                break;

            case STATE_PROCESS_USER_INPUT:
                break;
        }
    }

    void configureColors()
    {
        GameObject.Find("red").SetActive(true);

        GameObject.Find("green")
            .SetActive(numberOfColors >= 2);

        GameObject.Find("blue")
            .SetActive(numberOfColors >= 3);

        GameObject.Find("yellow")
            .SetActive(numberOfColors >= 4);
    }

    void initTimer()
    {
        startTimer = true;
        timer = 0;
        animationIndex = 0;
    }

    public void generateNewColor()
    {
        int r = Random.Range(1, numberOfColors + 1);

        sequenceOfColor[indexOfColor] = r;

        indexOfColor++;

        updateUI();

        print("New color: " + r);
    }

    void hideBoxes()
    {
        for (int i = 1; i <= numberOfColors; i++)
        {
            GameObject box =
                GameObject.FindWithTag("" + i);

            if (box != null)
            {
                Image img =
                    box.GetComponent<Image>();

                Color c = img.color;
                c.a = 0;

                img.color = c;
            }
        }
    }

    void showAllActiveColors()
    {
        for (int i = 1; i <= numberOfColors; i++)
        {
            GameObject box =
                GameObject.FindWithTag("" + i);

            if (box != null)
            {
                Image img =
                    box.GetComponent<Image>();

                Color c = img.color;
                c.a = 1;

                img.color = c;
            }
        }
    }

    public void displayBox(int index)
    {
        GameObject box =
            GameObject.FindWithTag("" + index);

        if (box != null)
        {
            Image img =
                box.GetComponent<Image>();

            Color c = img.color;
            c.a = 1;

            img.color = c;

            playNote(index);
        }
    }

    public void submitColor(int color)
    {
        if (currentState !=
            STATE_WAIT_FOR_USER_INPUT)
        {
            return;
        }

        playNote(color);

        userSequence[nbColorsSubmitted] = color;

        if (userSequence[nbColorsSubmitted] !=
            sequenceOfColor[nbColorsSubmitted])
        {
            loadLoseLevel();
            return;
        }

        nbColorsSubmitted++;

        if (nbColorsSubmitted >= indexOfColor)
        {
            print("Correct sequence");

            updateUI();

            nbColorsSubmitted = 0;
            newColorHasBeenGenerated = false;
            waitingTimeElapsed = false;

            currentState = STATE_PLAY_SEQUENCE;
        }
    }

    void updateUI()
    {
        if (nbMemorized != null)
        {
            nbMemorized.text = "" + Mathf.Max(1, indexOfColor);
        }
    }

    void playNote(int index)
    {
        if (audioSource == null)
            return;

        switch (index)
        {
            case 1:
                audioSource.pitch = 1.0f;
                break;

            case 2:
                audioSource.pitch = 1.2f;
                break;

            case 3:
                audioSource.pitch = 1.4f;
                break;

            case 4:
                audioSource.pitch = 1.6f;
                break;
        }

        audioSource.Play();
    }

    void SaveGameInfo()
    {
        PlayerPrefs.SetString(
            "finalPlayerName",
            PlayerPrefs.GetString(
                "playerName",
                "Player"
            )
        );

        PlayerPrefs.SetInt(
            "finalColors",
            numberOfColors
        );

        PlayerPrefs.SetFloat(
            "finalTime",
            elapsedTime
        );

        PlayerPrefs.SetInt(
            "score",
            Mathf.Max(0, indexOfColor - 1)
        );

        PlayerPrefs.Save();
    }

    void loadLoseLevel()
    {
        SaveGameInfo();

        Time.timeScale = 1;

        SceneManager.LoadScene("exit");
    }

    public void ExitGame()
    {
        SaveGameInfo();

        string playerName =
            PlayerPrefs.GetString("playerName", "Player");

        MemoryDatabase database =
            GameObject.Find("databaseManager")
            .GetComponent<MemoryDatabase>();

        StartCoroutine(
            database.SaveMemory(
                playerName,
                numberOfColors,
                elapsedTime
            )
        );

        StartCoroutine(GoToExitAfterSave());
    }

    IEnumerator GoToExitAfterSave()
    {
        yield return new WaitForSeconds(0.5f);

        Time.timeScale = 1;

        SceneManager.LoadScene("exit");
    }


}