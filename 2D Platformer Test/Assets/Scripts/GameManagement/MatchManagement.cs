using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
        foreach (Player player in Players)
        {
            player.KillVelocity();
            player.DeactivatePlayer();
            player.CanMove(false);
        }

        Players[0].isHunter = true;
        Players[1].isHunter = false;
        Players[0].transform.position = hunterSpawns[Random.Range(0, hunterSpawns.Length)].position;
        Players[1].transform.position = praySpawns[Random.Range(0, praySpawns.Length)].position;
        pray = Players[1];
        hunter = Players[0];
        currentRound++;
        currentRoundTime = maxRoundTime;
        currentCountDownTime = maxCountDownTime;
        roundCounter.text = currentRound.ToString();
        player1score.text = Players[0].score.ToString();
        player2score.text = Players[1].score.ToString();
        Hud.SetActive(false);
        countDownPanel.SetActive(true);
        int ajustedHunterIndex = hunter.playerConfig.PlayerIndex + 1;
        hunterName.text = "Player " + ajustedHunterIndex.ToString();
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
            else
            {
                EndGame();
            }
            DisplayTime(currentRoundTime,timeText);
        }
        else
        {
            currentCountDownTime -= Time.deltaTime;
            countDownText.text = currentCountDownTime.ToString();
            if (currentCountDownTime <= 0)
            {
                Players[0].ActivatePlayer();
                Players[1].ActivatePlayer();
                Players[0].CanMove(true);
                Players[1].CanMove(true);

                countDownPanel.SetActive(false);
                Hud.gameObject.SetActive(true);
                isPlaying = true;
            }
        }
    }

    public void EndRound(Player winner)
    {
        isPlaying = false;
        foreach (Player player in Players)
        {
            player.KillVelocity();
            player.DeactivatePlayer();
            player.CanMove(false);            
        }
        winner.score++;
        player1score.text = Players[0].score.ToString();
        player2score.text = Players[1].score.ToString();
        Hud.SetActive(false);
        StartRound();
    }

    void StartRound()
    {
        pray.isHunter = true;
        hunter.isHunter = false;
        var temp = hunter;
        hunter = pray;
        pray = temp;
        pray.transform.position = praySpawns[Random.Range(0, praySpawns.Length)].position;
        hunter.transform.position = hunterSpawns[Random.Range(0, hunterSpawns.Length)].position;

        currentRoundTime = maxRoundTime;
        currentCountDownTime = maxCountDownTime;

        currentRound++;
        roundCounter.text = currentRound.ToString();
        countDownPanel.SetActive(true);
        int ajustedHunterIndex = hunter.playerConfig.PlayerIndex + 1;
        hunterName.text = "Player " + ajustedHunterIndex.ToString();
    }

    void EndGame()
    {

        Player winner = new Player();
        if(Players[0].score > Players[1].score)
        {
            winner = Players[0];
        }else if (Players[0].score < Players[1].score)
        {
            winner = Players[1];
        }
        else
        {
            winner.name = "noOne";
        }
        
        Debug.Log(winner.name + "hasWon");
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
