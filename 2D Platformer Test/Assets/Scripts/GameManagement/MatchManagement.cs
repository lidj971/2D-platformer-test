using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MatchManagement : MonoBehaviour
{
    List<Player> Players;
    public float maxRoundTime;
    public float currentRoundTime;

    public float maxCountDownTime;
    public float currentCountDownTime;

    public int rounds;
    public int currentRound = 0;

    public GameObject Hud;
    public Text timeText;
    public Text roundCounter;
    public Text player1score;
    public Text player2score;
    public Text hunterName;

    public GameObject countDownPanel;
    public Text countDownText;

    public Transform[] hunterSpawns;
    public Transform[] praySpawns;

    private Player hunter;
    private Player pray;

    public bool isPlaying {get; private set;}

    public void GetPlayers(List<Player> playerList)
    {
        Players = playerList;
    }
    
    public void StartGame()
    {
        SetPlayersCanMove(false);
        SetHunter(Players[0]);        
        SetPlayersPositions();

        currentRound++;
        currentRoundTime = maxRoundTime;
        currentCountDownTime = maxCountDownTime;

        Hud.SetActive(false);
        UpdateCountDownPanel();
        countDownPanel.SetActive(true);
    }

    void Update()
    {
        if (isPlaying)
        {
            if (currentRoundTime > 0)
            {
                currentRoundTime -= Time.deltaTime;
            }
            else if(currentRound < rounds && currentRoundTime <= 0)
            {
                EndRound(pray);
            }
            DisplayTime(currentRoundTime,timeText);
        }
        else
        {
            if (currentRound > rounds)
            {
                EndGame();
            }
            else
            {
                currentCountDownTime -= Time.deltaTime;
                countDownText.text = currentCountDownTime.ToString();
                if (currentCountDownTime <= 0)
                {
                    StartRound();
                }
            }
        }
    }

    public void EndRound(Player winner)
    {
        isPlaying = false;
        SetPlayersCanMove(false);
        SetHunter(pray);
        SetPlayersPositions();

        currentRoundTime = maxRoundTime;
        currentCountDownTime = maxCountDownTime;
        currentRound++;
        winner.score++;

        Hud.SetActive(false);
        UpdateCountDownPanel();
        countDownPanel.SetActive(true);
    }

    private void StartRound()
    {
        SetPlayersCanMove(true);
        hunter.HunterIcon.SetActive(true);
        pray.HunterIcon.SetActive(false);
        countDownPanel.SetActive(false);
        UpdateHud();
        Hud.gameObject.SetActive(true);
        isPlaying = true;
    }

    void EndGame()
    {
        if (Players[0].score > Players[1].score)
        {
            int ajustedWinnerIndex = Players[0].playerConfig.PlayerIndex + 1;
            Debug.Log("Player " + ajustedWinnerIndex.ToString() + " Has Won");
        }
        else if (Players[0].score < Players[1].score)
        {
            int ajustedWinnerIndex = Players[1].playerConfig.PlayerIndex + 1;
            Debug.Log("Player " + ajustedWinnerIndex.ToString() + " Has Won");
        }
        else
        {
            Debug.Log("Its A Draw");
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void SetHunter(Player hunter)
    {
        hunter.isHunter = true;
        this.hunter = hunter;
        foreach(Player player in Players)
        {
            if(player != hunter)
            {
                player.isHunter = false;
                pray = player;
            }
        }
        hunter.HunterIcon.SetActive(true);
        pray.HunterIcon.SetActive(false);
    }

    private void SetPlayersPositions()
    {
        hunter.transform.position = hunterSpawns[Random.Range(0, hunterSpawns.Length)].position;
        pray.transform.position = praySpawns[Random.Range(0, praySpawns.Length)].position;
    }

    private void SetPlayersCanMove(bool canMove)
    {
        foreach (Player player in Players)
        {
            player.SetCanMove(canMove);
        }
    }

    private void UpdateHud()
    {
        roundCounter.text = currentRound.ToString();
        player1score.text = Players[0].score.ToString();
        player2score.text = Players[1].score.ToString();
    }

    private void UpdateCountDownPanel()
    {
        int ajustedHunterIndex = hunter.playerConfig.PlayerIndex + 1;
        hunterName.text = "Player " + ajustedHunterIndex.ToString();
    }

    void DisplayTime(float timeToDisplay,Text textObject)
    {
        if (timeToDisplay < 0)
        {
            timeToDisplay = 0;
        }
        else if (timeToDisplay > 0)
        {
            timeToDisplay += 1;
        }
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        textObject.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
