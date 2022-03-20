using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEditor;

public class PlayerConfigurationManager : MonoBehaviour
{
    private List<PlayerConfiguration> playerConfigs;

    [SerializeField]
    private int MaxPlayers = 2;

    //public PlayerData basePlayerData;

    public static PlayerConfigurationManager Instance { get; private set; }

    private void Awake()
    {
        if(Instance != null)
        {
            Debug.Log("SINGLETON - Trying to create another instance of singleton");
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
            playerConfigs = new List<PlayerConfiguration>();
        }
    }

    public List<PlayerConfiguration> GetPlayerConfigurations()
    {
        return playerConfigs;
    }

    public void SetPlayerSkills(int index,List<PlayerState> skills)
    {
        foreach(PlayerState skill in skills)
        {
            playerConfigs[index].Input.gameObject.AddComponent(skill.GetType());
        }
    }

    /*public void SetPlayerData(int index,PlayerData playerData)
    {
        playerConfigs[index].playerData = playerData;
    }*/

    public void ReadyPlayer(int index)
    {
        playerConfigs[index].IsReady = true;
        if(playerConfigs.Count == MaxPlayers && playerConfigs.All(p => p.IsReady == true))
        {
            SceneManager.LoadScene("SampleScene");
        }
    }

    public void HandlePlayerJoin(PlayerInput pi)
    {
        Debug.Log("Player Joined" + pi.playerIndex);

        if(!playerConfigs.Any(p => p.PlayerIndex == pi.playerIndex))
        {
            pi.transform.SetParent(transform);
            playerConfigs.Add(new PlayerConfiguration(pi/*,basePlayerData*/));
        }
    }

}

public class PlayerConfiguration
{
    public PlayerConfiguration(PlayerInput pi/*,PlayerData basePlayerData*/)
    {
        PlayerIndex = pi.playerIndex;
        Input = pi;
        //var dataName = AssetDatabase.GenerateUniqueAssetPath("Assets/Scripts/Player/Data/Player" + PlayerIndex.ToString() + ".asset");
        //AssetDatabase.CopyAsset(AssetDatabase.GetAssetPath(basePlayerData), dataName);
    }
    public PlayerInput Input { get; set; }
    public int PlayerIndex { get; set; }
    public bool IsReady { get; set; }
    public List<PlayerState> Skills;
    //public PlayerData playerData;
}