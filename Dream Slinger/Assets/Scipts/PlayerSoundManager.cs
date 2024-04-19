using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundManager : MonoBehaviour
{
    [SerializeField] private SoundClip[] playerJump;
    [SerializeField] private SoundClip[] playerLand;
    [SerializeField] private SoundClip[] playerKill;
    [SerializeField] private SoundClip[] playerPortal;
    [SerializeField] private SoundClip[] playerDie;
    [SerializeField] private SoundClip[] playerPlatform;
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

    public void PlayerKillSFX(Vector3 playerPos)
    {
        int rngNo = Random.Range(0, playerKill.Length);
        PlayAudioAtPos(playerKill[rngNo].audioClip, playerPos, playerKill[rngNo].volume);
    }

    public void PlayerDieSFX(Vector3 playerPos)
    {
        int rngNo = Random.Range(0, playerDie.Length);
        PlayAudioAtPos(playerDie[rngNo].audioClip, playerPos, playerDie[rngNo].volume);
    }

    public void PlayerPortalSFX(Vector3 playerPos)
    {
        int rngNo = Random.Range(0, playerPortal.Length);
        PlayAudioAtPos(playerPortal[rngNo].audioClip, playerPos, playerPortal[rngNo].volume);
    }

    public void PlayerPlatformSFX(Vector3 playerPos)
    {
        int rngNo = Random.Range(0, playerPlatform.Length);
        PlayAudioAtPos(playerPlatform[rngNo].audioClip, playerPos, playerPlatform[rngNo].volume);
    }
    private void PlayAudioAtPos(AudioClip audioClip, Vector3 pos, float volume)
    {
        AudioSource.PlayClipAtPoint(audioClip, pos, volume);
    }
}
