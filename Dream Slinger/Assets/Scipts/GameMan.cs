using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using UnityEngine.UI;
public class GameMan : MonoBehaviour
{
    public static GameMan instance = null;
    public GameObject player;
    private GameObject currentPlayer;

    [SerializeField] private LevelContainer[] levels;

    [SerializeField] private GameObject missionSelectionPage;
    [SerializeField] private MMF_Player screenFade;
    GameObject currentLevelPrefab;

    private int levelIndex;
    bool isSettingLevel;

    float timer;
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
       
    }

    void Update()
    {
        timer += Time.deltaTime;
    }

    public void SetLevel()
    {
        if (currentLevelPrefab != null)
        {
            Destroy(currentLevelPrefab);
        }

        currentPlayer.transform.position = levels[levelIndex].levelPrefab.GetComponent<AreaManager>().GetCheckPointPos();
        currentLevelPrefab = Instantiate(levels[levelIndex].levelPrefab, transform.position, Quaternion.identity);
        isSettingLevel = false;
    }

    public void SetLevelIndex(int index)
    {
        timer = 0;
        if (index >= levels.Length)
        {
            Debug.Log("No Level with this index " + index);
            index = 0;     
        }
        levelIndex = index;
        if (currentLevelPrefab != null)
        {
            Destroy(currentLevelPrefab);
        }

        if (currentPlayer == null)
        {
            Vector2 spawnPos = levels[index].levelPrefab.GetComponent<AreaManager>().GetCheckPointPos();
            currentPlayer = Instantiate(player, spawnPos, Quaternion.identity);
        }
      
        currentLevelPrefab = Instantiate(levels[index].levelPrefab, transform.position, Quaternion.identity);
        currentPlayer.transform.position = levels[index].levelPrefab.GetComponent<AreaManager>().GetCheckPointPos();

        isSettingLevel = false;
    }

    public void NextLevel()
    {
        if (!isSettingLevel)
        {
            isSettingLevel = true;
            if(levels[levelIndex].levelIsCompleted == false)
            {
                levels[levelIndex].levelIsCompleted = true;
                levels[levelIndex].bestCompletionTime = timer;
                Debug.Log("Level " + (levelIndex + 1) + " is completed for the first time at " + timer);
                timer = 0;
            }
            else
            {
                if (timer <= levels[levelIndex].bestCompletionTime)
                {
                    levels[levelIndex].bestCompletionTime = timer;
                    Debug.Log("Level " + (levelIndex + 1) + " is completed faster at " + timer);
                }
                else
                {
                    Debug.Log("Level " + (levelIndex + 1) + " is completed slower at " + timer);
                }
                timer = 0;
            }
          
            levelIndex = (levelIndex + 1) % levels.Length;
            ScreenFade();
            Invoke("SetLevel", 0.8f);
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

    public void ScreenFade()
    {
        screenFade.PlayFeedbacks();
    }

    public void SetMissionSelectionScreen(bool boolean)
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        ScreenFade();
        Destroy(currentLevelPrefab);
        Destroy(currentPlayer);
        missionSelectionPage.SetActive(boolean);
    }
}
