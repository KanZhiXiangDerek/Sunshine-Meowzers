using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMan : MonoBehaviour
{
    public static GameMan instance = null;
    [SerializeField] private GameObject player;
    [SerializeField] private LevelContainer[] levels;
    GameObject currentLevelPrefab;
    private int levelIndex;
    [SerializeField] private TimeMan timeManager;
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

    public void TimeSlow()
    {
        timeManager.TimeSlowDown();
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
        if(currentLevelPrefab != null)
        {
            Destroy(currentLevelPrefab);
        }
       
        currentLevelPrefab = Instantiate(levels[levelIndex].levelPrefab, transform.position, Quaternion.identity);
        ResetPlayerPos(levels[levelIndex].levelPrefab.GetComponent<AreaManager>().GetCheckPointPos());
    }

    public void NextLevel()
    {
        if (levelIndex < levels.Length)
            levelIndex += 1;
        else
            levelIndex = 0;
        SetLevel();
    }

    public void ResetPlayerPos(Vector2 spawnPos)
    {
        player.transform.position = spawnPos;
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }
}

