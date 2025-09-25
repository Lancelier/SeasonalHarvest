using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private CinemachineVirtualCamera cam;
    [SerializeField] private Player.Season playerStartSeason;

    private GameObject currentPlayer;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        currentPlayer = Instantiate(playerPrefab, spawnPoint);
        currentPlayer.GetComponent<Player>().SetSeason(playerStartSeason);
        currentPlayer.transform.parent = null;
        cam.Follow = currentPlayer.transform;
    }

    public IEnumerator PlayerDeath(float delay = 1f)
    {
        currentPlayer.GetComponent<Player>().enabled = false;

        yield return new WaitForSeconds(delay);

        Destroy(currentPlayer);

        currentPlayer = Instantiate(playerPrefab, spawnPoint);
        currentPlayer.GetComponent<Player>().SetSeason(playerStartSeason);
        currentPlayer.transform.parent = null;
        cam.Follow = currentPlayer.transform;
    }

    public IEnumerator PlayerWin(float delay = 1f)
    {
        currentPlayer.GetComponent<Player>().enabled = false;

        yield return new WaitForSeconds(delay);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
