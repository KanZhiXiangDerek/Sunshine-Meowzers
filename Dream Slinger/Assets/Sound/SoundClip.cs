using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Sound", menuName = "ScriptableObjects/Sound", order = 1)]
public class SoundClip: ScriptableObject
{
    public string audioName;
    public AudioClip audioClip;

    [Range(0,1) ]
    public float volume = 1.0f;

    [Range(0, 3)]
    public float pitch = 1.0f;

}
