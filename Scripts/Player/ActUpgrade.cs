using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ActUpgrade : MonoBehaviour
{
    [SerializeField]
    PlayerController playerController;
    [SerializeField]
    private Life life;
    [SerializeField]
    private RimitTime rimitTime;

    void HealAndTimeCountUpEvent() {
        const int m_healPoint = 50;
        const int m_timeCountUP = 50;
        life.Heal(m_healPoint);
        rimitTime.TimeCountUp(m_timeCountUP);
    }

    IEnumerator WindowOpenEvent()
    {
        for(float i = 1; i < 6; i++)
        {
            transform.localScale = Vector3.one * i / 5;
            yield return null;
        }
    }
    IEnumerator WindowCloseEvent()
    {
        for (float i = 5; i > 0; i--)
        {
            transform.localScale = Vector3.one * i / 5;
            yield return null;
        }
        gameObject.SetActive(false);
    }

    public void WindowOpen()
    {
        StartCoroutine(WindowOpenEvent());
    }

    public void WindowClose()
    {
        StartCoroutine(WindowCloseEvent());
    }
}
