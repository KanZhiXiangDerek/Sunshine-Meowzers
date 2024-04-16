using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
public class LightBehaviour : MonoBehaviour
{
    [SerializeField] Light2D light2d;

    [SerializeField] private float timeDelay;
    [SerializeField] private float[] intensity;
    [SerializeField] private float timeBtwIntensity = 3.0f;
    [SerializeField] private bool adjustRadius = true;
    [SerializeField] private float[] radius;
    [SerializeField] private float timeBtwRadiusLerp = 3.0f;
    [SerializeField] private Color32[] colors;
    [SerializeField] private float timeBtwColor = 2.0f;
    private int colorIndex;
    private int radiusIndex;
    private int intensityIndex;
    void Start()
    {
        light2d.color = colors[0];
        colorIndex = 0;
        Invoke("ChangeLightProp", timeDelay);
    }

    private void ChangeLightProp()
    {
        StartCoroutine(ChangeColor());
        if (adjustRadius)
        {
            StartCoroutine(ChangeRadius());
        }
        StartCoroutine(ChangeIntensity());
    }

    IEnumerator ChangeColor()
    {
        while (true)
        {
            colorIndex = (colorIndex + 1) % colors.Length;
            yield return LerpFunction(colors[colorIndex], timeBtwColor);
        }
    }

    IEnumerator ChangeRadius()
    {
        while (true)
        {
            radiusIndex = (radiusIndex + 1) % radius.Length;
            yield return LerpRadius(radius[radiusIndex], timeBtwRadiusLerp);
        }
    }

    IEnumerator ChangeIntensity()
    {
        while (true)
        {
            intensityIndex = (intensityIndex + 1) % intensity.Length;
            yield return LerpIntensity(intensity[intensityIndex], timeBtwIntensity);
        }
    }

    IEnumerator LerpFunction(Color endValue, float duration)
    {
        float time = 0;
        Color startValue = light2d.color;

        while (time < duration)
        {
            light2d.color = Color.Lerp(startValue, endValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        light2d.color = endValue;
    }

    IEnumerator LerpRadius(float endRadius, float duration)
    {
        float time = 0;
        float startRadius = light2d.pointLightOuterRadius;

        while (time < duration)
        {
            light2d.pointLightOuterRadius = Mathf.Lerp(startRadius, endRadius, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        light2d.pointLightOuterRadius = endRadius;
    }

    IEnumerator LerpIntensity(float endIntensity, float duration)
    {
        float time = 0;
        float startIntensity = light2d.intensity;

        while (time < duration)
        {
            light2d.intensity = Mathf.Lerp(startIntensity, endIntensity, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        light2d.intensity = endIntensity;
    }

}
