using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwipeMenu : MonoBehaviour
{
    public GameObject swiper;
    private float swipePos = 0;
    private float[] position;

    void Update()
    {
        position = new float[transform.childCount];
        float distance = 1f / (position.Length - 1f);
        for (int i = 0; i < position.Length; i++)
        {
            position[i] = distance * i;
        }
        if (Input.GetMouseButton(0))
        {
            swipePos = swiper.GetComponent<Scrollbar>().value;
        }
        else
        {
            for (int i = 0; i < position.Length; i++)
            {
                if (swipePos < position[i] + (distance / 2) && swipePos > position[i] - (distance / 2))
                {
                    swiper.GetComponent<Scrollbar>().value = Mathf.Lerp(swiper.GetComponent<Scrollbar>().value, position[i], 0.1f);
                }
            }
        }
        for (int i = 0; i < position.Length; i++)
        {
            if (swipePos < position[i] + (distance / 2) && swipePos > position[i] - (distance / 2))
            {
                transform.GetChild(i).localScale = Vector2.Lerp(transform.GetChild(i).localScale, new Vector2(2f, 2f), 0.1f);
            }
            for (int a = 0; a < position.Length; a++)
            {
                if (a != i)
                {
                    transform.GetChild(a).localScale = Vector2.Lerp(transform.GetChild(a).localScale, new Vector2(1f, 1f), 0.1f);
                }
            }
        }
    }
}
