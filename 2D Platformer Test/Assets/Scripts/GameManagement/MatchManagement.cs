using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MatchManagement : MonoBehaviour
{
    List<Player> Players;
    public float maxRoundTime;
    public int rounds;
    public int currentRound = 0;
    public float currentRoundTime;
    public Text timeText;
    public Text CountDown;
    public bool isPlaying;
    public float countDownTime;
    public Transform[] hunterSpawns;
    public Transform[] praySpawns;
    private Player hunter;
    private Player pray;
    public bool hunterHasWon;

    public void GetPlayers(List<Player> playerList)
    {
        Players = playerList;
    }
    
    public void StartGame()
    {
        Players[0].isHunter = true;
        Players[1].isHunter = false;
        Players[1].transform.position = praySpawns[Random.Range(0, praySpawns.Length)].position;
        Players[0].transform.position = hunterSpawns[Random.Range(0, hunterSpawns.Length)].position;
        currentRound++;
        currentRoundTime = maxRoundTime;

        isPlaying = true;
    }

    void Update()
    {
        if (isPlaying)
        {
            if (currentRoundTime > 0)
            {
                currentRoundTime -= Time.deltaTime;
            }
            else if(currentRound < rounds)
            {
                EndRound();
            }
            else
            {
                EndGame();
            }
            DisplayTime(currentRoundTime,timeText);
        }
    }

    public void EndRound()
    {
        isPlaying = false;
        foreach (Player player in Players)
        {
            player.KillVelocity();
            player.DeactivatePlayer();
            player.CanMove(false);
            if (!player.isHunter && !hunterHasWon)
            {
                player.score++;
            }
        }
        timeText.gameObject.SetActive(false);
        StartCoroutine(StartRound());
    }

    IEnumerator StartRound()
    {
        foreach(Player player in Players)
        {     
            if (player.isHunter)
            {
                player.isHunter = false;
                pray = player;
                player.transform.position = praySpawns[Random.Range(0, praySpawns.Length)].position;
            }
            else
            {
                player.isHunter = true;
                hunter = player;
                player.transform.position = hunterSpawns[Random.Range(0, hunterSpawns.Length)].position;
            }
            player.ActivatePlayer();
        }


        currentRound++;
        currentRoundTime = maxRoundTime;

        yield return new WaitForSeconds(countDownTime);

        Players[0].CanMove(true);
        Players[1].CanMove(true);

        hunterHasWon = false;

        isPlaying = true;
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
