using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private CinemachineVirtualCamera cam;

    private GameObject currentPlayer;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        currentPlayer = Instantiate(playerPrefab, spawnPoint);
        cam.Follow = currentPlayer.transform;
    }



    public void PlayerDeath()
    {
        Destroy(currentPlayer);

        currentPlayer = Instantiate(playerPrefab, spawnPoint);
        cam.Follow = currentPlayer.transform;
    }
}
