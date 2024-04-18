using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMan : MonoBehaviour
{
    public static GameMan instance = null;
    public GameObject player;
    [SerializeField] private GameObject sceneTrans;
    private GameObject currentPlayer;
    [SerializeField] private LevelContainer[] levels;
    GameObject currentLevelPrefab;
    [SerializeField] private int levelIndex;
    [SerializeField] private TimeMan timeManager;
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
        SetLevel();
    }

    void Update()
    {

    }

    public void TimeSlow(float slowDownNumber)
    {
        timeManager.TimeSlowDown(slowDownNumber);
    }

    public void ResetTimeScale()
    {
        timeManager.ResetTimeScale();
    }

    public void SlowDownLengthReduce(float reduceDivNo)
    {
        timeManager.SlowDownLengthReduce(reduceDivNo);
    }

    public void SetLevel()
    {
        if (currentLevelPrefab != null)
        {
            Destroy(currentLevelPrefab);
        }

        currentLevelPrefab = Instantiate(levels[levelIndex].levelPrefab, transform.position, Quaternion.identity);
        ResetPlayerPos(levels[levelIndex].levelPrefab.GetComponent<AreaManager>().GetCheckPointPos());
        isSettingLevel = false;
    }

    public void NextLevel()
    {
        if (!isSettingLevel)
        {
            isSettingLevel = true;
            levelIndex = (levelIndex + 1) % levels.Length;
            TriggerSceneTransition();
            Invoke("SetLevel", 1.05f);
        }
        //Invoke("ResetSceneTransPos", 2.0f);
    }

    public void ResetSceneTransPos()
    {
        //sceneTrans.GetComponent<SpriteRenderer>().enabled = false;
        //sceneTrans.transform.position = new Vector2(-250f, transform.position.y);
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

    public void TempRespawnPlayer(Vector2 spawnPos)
    {
        if (currentPlayer != null)
        {
            Destroy(currentPlayer);
        }

        currentPlayer = Instantiate(player.gameObject, spawnPos, Quaternion.identity);

        PlayerController playerController = currentPlayer.GetComponent<PlayerController>();
        playerController.ExtraJump();
        playerController.PlayerToStayInSamePos(spawnPos);
    }

    public void TriggerSceneTransition()
    {
        //sceneTrans.GetComponent<SpriteRenderer>().enabled = true;
        sceneTrans.GetComponent<Animator>().SetTrigger("IsSceneTrans");
    }
}
