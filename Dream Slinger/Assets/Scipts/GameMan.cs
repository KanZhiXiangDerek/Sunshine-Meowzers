using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMan : MonoBehaviour
{
    public static GameMan instance = null;
    public GameObject player;
    private GameObject currentPlayer;

    [SerializeField] private LevelContainer[] levels;
    GameObject currentLevelPrefab;

    private int levelIndex;
    bool isSettingLevel;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(transform.gameObject);
    }

    private void Start()
    {
        ResetPlayerPos(levels[levelIndex].levelPrefab.GetComponent<AreaManager>().GetCheckPointPos());
    }

    void Update()
    {

    }

    public void SetLevel()
    {
        if (currentLevelPrefab != null)
        {
            Destroy(currentLevelPrefab);
        }

        currentLevelPrefab = Instantiate(levels[levelIndex].levelPrefab, transform.position, Quaternion.identity);
        currentPlayer.transform.position = levels[levelIndex].levelPrefab.GetComponent<AreaManager>().GetCheckPointPos();
        //ResetPlayerPos(levels[levelIndex].levelPrefab.GetComponent<AreaManager>().GetCheckPointPos());
        isSettingLevel = false;
    }

    public void NextLevel()
    {
        if (!isSettingLevel)
        {
            isSettingLevel = true;
            levelIndex = (levelIndex + 1) % levels.Length;
            Invoke("SetLevel", 1.05f);
        }
    }

    public GameObject GetPlayerObj()
    {
        return currentPlayer;
    }

    public void ResetPlayerPos(Vector2 spawnPos)
    {
        if (currentPlayer != null)
        {
            Destroy(currentPlayer);
        }
        currentPlayer = Instantiate(player.gameObject, spawnPos, Quaternion.identity);
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }
}
