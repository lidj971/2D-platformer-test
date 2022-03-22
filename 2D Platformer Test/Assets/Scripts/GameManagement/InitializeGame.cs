using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeGame : MonoBehaviour
{
    [SerializeField]
    private GameObject playerPrefab;
    [SerializeField]
    private MultipleTargetCamera targetCamera;

    private MatchManagement matchManager;
    // Start is called before the first frame update
    void Start()
    {
        List<Player> players = new List<Player>();
        List<Transform> playerTransforms = new List<Transform>();
        matchManager = GetComponentInParent<MatchManagement>();
        var playerConfigs = PlayerConfigurationManager.Instance.GetPlayerConfigurations().ToArray();
        for(int i = 0;i < playerConfigs.Length; i++)
        {
            var player = Instantiate(playerPrefab, transform.position, transform.rotation, gameObject.transform);
            player.GetComponent<Player>().InitializePlayer(playerConfigs[i]);
            player.GetComponent<Player>().matchManager = matchManager;
            players.Add(player.GetComponent<Player>());
            playerTransforms.Add(player.transform);
        }

        matchManager = GetComponentInParent<MatchManagement>();
        matchManager.GetPlayers(players);

        targetCamera.GetPlayerTransforms(playerTransforms);

        matchManager.StartGame();
    }
}
