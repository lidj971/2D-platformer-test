using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class PlayerSetupMenuController : MonoBehaviour
{
    private int PlayerIndex;

    [SerializeField]
    private TextMeshProUGUI titleText;
    [SerializeField]
    private GameObject readyPanel;
    [SerializeField]
    private GameObject menuPanel;
    [SerializeField]
    private Button readyButton;

    public List<PlayerState> currentPlayerSkills;

    //public PlayerData currentPlayerData;

    private float ignoreInputTime = 1.5f;
    private bool inputEnabled;

    public void SetPlayerIndex(int pi)
    {
        PlayerIndex = pi;
        titleText.SetText("Player " + (pi + 1).ToString());
        ignoreInputTime = Time.time + ignoreInputTime;
    }

    public void Awake()
    {
        //currentPlayerData = PlayerConfigurationManager.Instance.basePlayerData;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time > ignoreInputTime)
        {
            inputEnabled = true;
        }
    }

    public void AddSkill(PlayerState skill)
    {
        if (currentPlayerSkills.Contains(skill))
        {
            currentPlayerSkills.Remove(skill);
        }
        else
        {
            currentPlayerSkills.Add(skill);
        }
    }

    /*public void SetNbJump(int nbJump)
    {
        if(currentPlayerData.amountOfJumps == nbJump)
        {
            currentPlayerData.amountOfJumps = PlayerConfigurationManager.Instance.basePlayerData.amountOfJumps;
        }
        else
        {
            currentPlayerData.amountOfJumps = nbJump;
        }
    }*/

    public void SetPlayer()
    {
        if (!inputEnabled) return;

        PlayerConfigurationManager.Instance.SetPlayerSkills(PlayerIndex, currentPlayerSkills);
        //PlayerConfigurationManager.Instance.SetPlayerData(PlayerIndex, currentPlayerData);
        readyPanel.SetActive(true);
        readyButton.Select();
        menuPanel.SetActive(false);
    }

    public void ReadyPlayer()
    {
        if (!inputEnabled) return;
        PlayerConfigurationManager.Instance.ReadyPlayer(PlayerIndex);
        readyButton.gameObject.SetActive(false);
    }
}
