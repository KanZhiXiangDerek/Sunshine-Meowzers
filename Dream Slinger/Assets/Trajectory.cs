using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trajectory : MonoBehaviour
{
    [SerializeField] int dotNumbers;
    [SerializeField] GameObject dotPrefab;
    [SerializeField] GameObject dotParent;
    [SerializeField] float dotSpacing;
    //[SerializeField] Color32[] dotColors;
    //Color32 currentDotColor;

    Vector2 dotPos;
    Vector2 pos;
    float timeStamp;
    Transform[] dotsList;

    void Start()
    {
        PrepareDot();
        HideDot();
    }

    void PrepareDot()
    {
        dotsList = new Transform[dotNumbers];

        for (int i = 0; i < dotNumbers; i++)
        {
            dotsList[i] = Instantiate(dotPrefab, null).transform;
            dotsList[i].parent = dotParent.transform;
        }
    }

    public void UpdateDots(Vector3 playerPos, Vector2 force, float gravityMutipler)
    {
        timeStamp = dotSpacing;

        for (int i = 0; i < dotNumbers; i++)
        {
            pos.x = (playerPos.x + force.x * timeStamp);
            pos.y = (playerPos.y + force.y * timeStamp) - (Physics2D.gravity.magnitude * gravityMutipler * timeStamp * timeStamp) /2f;
            Debug.Log("Gravity Magnitude" + Physics2D.gravity.magnitude);

            dotsList[i].position = pos;
            timeStamp += dotSpacing;
        }
    }
    public void ShowDot()
    {
        dotParent.gameObject.SetActive(true);
    }

    public void HideDot()
    {
        dotParent.gameObject.SetActive(false);
    }

    //public void UpdateDotColor(bool boolean)
    //{
    //    if (boolean)
    //    {
    //        currentDotColor = dotColors[0];
    //    }
    //    else
    //    {
    //        currentDotColor = dotColors[1];
    //    }

    //    for (int i = 0; i < dotNumbers; i++)
    //    {
    //        dotsList[i].gameObject.GetComponent<SpriteRenderer>().color = currentDotColor;
    //    }
    //}
}
