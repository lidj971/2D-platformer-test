using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeGame : MonoBehaviour
{
    [SerializeField]
    private Transform[] playerSpawns;
    [SerializeField]
    private GameObject playerPrefab;
    [SerializeField]
    private MultipleTargetCamera targetCamera;

    // Start is called before the first frame update
    void Start()
    {
        List<Transform> playerTransforms = new List<Transform>();
        var playerConfigs = PlayerConfigurationManager.Instance.GetPlayerConfigurations().ToArray();
        for(int i = 0;i < playerConfigs.Length; i++)
        {
            var player = Instantiate(playerPrefab, playerSpawns[i].position, playerSpawns[i].rotation, gameObject.transform);
            player.GetComponent<Player>().InitializePlayer(playerConfigs[i]);
            playerTransforms.Add(player.transform);
        }
        targetCamera.GetPlayerTransforms(playerTransforms);
    }
}
