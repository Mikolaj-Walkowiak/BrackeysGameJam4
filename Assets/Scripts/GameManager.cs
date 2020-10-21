using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Yarn.Unity;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager instance;
    void Awake()
    {
        if (instance != null)
            return;
        instance = this;
    }
    #endregion

    
    public GameObject loseWinPanel;
    public Text loseWinText;
    public Text causeLoseWinText;
    public bool vhsTaken;
    public bool gameWon;
    public bool alarm;
    public InMemoryVariableStorage memory;
    public NPCManager npc;
    public PlayerController player;
    public bool isGamePaused = false;
    public bool isInExitArea;
    public AudioSource audio;
    private bool isSalsaPlaying;
    private bool gameEnd;
    public void SetLoseWinText(string text)
    {
        loseWinText.text = text;
    }

    public void SetCauseText(string text)
    {
        causeLoseWinText.text = text;
    }
    public void ResetGame()
    {
        isGamePaused = false;
        Time.timeScale = 1;
        SceneManager.LoadScene("SampleScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    void Start()
    {
        npc = NPCManager.instance;
        memory = npc.GetComponent<InMemoryVariableStorage>();
        audio = GetComponent<AudioSource>();
        audio.clip = Resources.Load<AudioClip>("Music/kozak");
        loseWinPanel.SetActive(false);
        player = PlayerController.instance;
        isInExitArea = false;
        Time.timeScale = 1;
        audio.Play();
        isSalsaPlaying = false;
        gameEnd = false;
    }

    public void OnStartGameDialogStart()
    {
        Time.timeScale = 0;
    }

    public void OnStartGameDialogEnd()
    {
        Time.timeScale = 1;
    }
    void Update()
    {
        if (gameEnd)
            return;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isGamePaused)
            {
                isGamePaused = true;
                OnStartGameDialogStart();
                SetLoseWinText("");
                SetCauseText("");
                loseWinPanel.SetActive(true);
            }
            else
            {
                isGamePaused = false;
                OnStartGameDialogEnd();
                SetLoseWinText("");
                SetCauseText("");
                loseWinPanel.SetActive(false);
            }
        }
        
        if (player.GetComponent<PlayerHealth>().CurrentHealth <= 0)
        {
            SetLoseWinText("You lost");
            SetCauseText("You died! Remember that enemies have limited ammo and you don't have to kill them :)");
            loseWinPanel.SetActive(true);
            gameEnd = true;
        }
        else if (isInExitArea)
        {
            gameEnd = true;
            if (memory.GetValue("$alarm").AsNumber == 0f &&
                memory.GetValue("$vhsTaken").AsNumber == 1f &&
                memory.GetValue("$gameWon").AsNumber == 1f)
            {
                if (!isSalsaPlaying)
                {
                    GetComponent<AudioSource>().Stop();
                    GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Music/salsa");
                    GetComponent<AudioSource>().Play();
                    isSalsaPlaying = true;
                }

                SetLoseWinText("You won");
                SetCauseText("You are the best <3");
                loseWinPanel.SetActive(true);
            }

            else if (memory.GetValue("$alarm").AsNumber == 1f &&
                     memory.GetValue("$vhsTaken").AsNumber == 1f &&
                     memory.GetValue("$gameWon").AsNumber == 1f)
            {
                SetLoseWinText("You lost");
                SetCauseText("We wanted this mission to be quiet! It was all but!");
                loseWinPanel.SetActive(true);
            }
            else if (memory.GetValue("$alarm").AsNumber == 0f &&
                     memory.GetValue("$vhsTaken").AsNumber == 0f &&
                     memory.GetValue("$gameWon").AsNumber == 1f)
            {
                SetLoseWinText("You lost");
                SetCauseText(
                    "Almost perfect. But you did forget about something important. I'm worried that someone will be in the news tommorow ;)");
                loseWinPanel.SetActive(true);
            }
            else if (memory.GetValue("$alarm").AsNumber == 1f &&
                     memory.GetValue("$vhsTaken").AsNumber == 0f &&
                     memory.GetValue("$gameWon").AsNumber == 1f)
            {
                SetLoseWinText("You lost");
                SetCauseText(
                    "It was not only loud but you will be in the news all around the world tommorow. This is really disappointing! :(");
                loseWinPanel.SetActive(true);
            }
            else if (memory.GetValue("$alarm").AsNumber == 0f &&
                     memory.GetValue("$vhsTaken").AsNumber == 0f &&
                     memory.GetValue("$gameWon").AsNumber == 0f)
            {
                SetLoseWinText("You lost");
                SetCauseText("Did you even do anything? Maybe go for a walk and try again agent!");
                loseWinPanel.SetActive(true);
            }
            else if (memory.GetValue("$alarm").AsNumber == 1f &&
                     memory.GetValue("$vhsTaken").AsNumber == 0f &&
                     memory.GetValue("$gameWon").AsNumber == 0f)
            {
                SetLoseWinText("You lost");
                SetCauseText("I get that you're scared of the alarm but that's not what we've taught you in training agent!");
                loseWinPanel.SetActive(true);
            }
            else
            {
                SetLoseWinText("You lost");
                SetCauseText("You've lost so hard we didn't even predict it's possible... Please try again");
                loseWinPanel.SetActive(true);

            }
        }

    }
    
    
}
