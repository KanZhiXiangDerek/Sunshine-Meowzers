using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[CreateAssetMenu(fileName = "New Level", menuName = "ScriptableObjects/New Level", order = 2)]
public class LevelContainer : ScriptableObject
{
    public int levelID;
    public GameObject levelPrefab;
    public float bestCompletionTime;
    public bool levelIsCompleted;
}
