using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundManager : MonoBehaviour
{
    [SerializeField] private SoundClip[] playerJump;
    [SerializeField] private SoundClip[] playerLand;
    public void PlayerJumpSFX(Vector3 playerPos)
    {
        int rngNo = Random.Range(0, playerJump.Length);
        PlayAudioAtPos(playerJump[rngNo].audioClip, playerPos, playerJump[rngNo].volume);
    }

    public void PlayerLandSFX(Vector3 playerPos)
    {
        int rngNo = Random.Range(0, playerLand.Length);
        PlayAudioAtPos(playerLand[rngNo].audioClip, playerPos, playerLand[rngNo].volume);
    }
    private void PlayAudioAtPos(AudioClip audioClip, Vector3 pos, float volume)
    {
        AudioSource.PlayClipAtPoint(audioClip, pos, volume);
    }
}
