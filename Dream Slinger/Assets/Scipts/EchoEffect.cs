using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoEffect : MonoBehaviour
{
    private float timeBtwSpawns;
  
    [SerializeField] private float startTimeBtwSpawns;
    [SerializeField] private float spawnLifetime;
    [SerializeField] private Color32 effectColor;
    [SerializeField] private Color32 transparant;
    private bool canSpawn;

    public GameObject echo;
    private void Start()
    {
        timeBtwSpawns = startTimeBtwSpawns;

    }
    void Update()
    {
        if (timeBtwSpawns <= 0 && canSpawn)
        {
            GameObject Instance = GameObject.Instantiate(echo, transform.position, Quaternion.identity);
            SpriteRenderer goSR = Instance.GetComponent<SpriteRenderer>();
            StartCoroutine(LerpFunction(transparant, goSR, spawnLifetime, Instance));
            timeBtwSpawns = startTimeBtwSpawns;
        }
        else if(canSpawn)
        {
            timeBtwSpawns -= Time.deltaTime;
        }
    }

    public void SetCanSpawnEffect(bool boolean)
    {
        canSpawn = boolean;
    }

    IEnumerator LerpFunction(Color endValue, SpriteRenderer sr,float duration, GameObject go)
    {
        float time = 0;
        Color startValue = sr.color;

        while (time < duration)
        {
            sr.color = Color.Lerp(startValue, endValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        sr.color = endValue;
        Destroy(go);
    }
}
