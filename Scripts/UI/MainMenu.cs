﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {
    
    [SerializeField]
    private GameObject nowOpenMenu;
    [SerializeField]
    private FadeInOut fadeInOut;

    

    public void OpenClose(GameObject openObj) {
        StartCoroutine(MenuOpen(openObj));
    }

    IEnumerator MenuOpen(GameObject openObject) {
        const float m_fadeTime = 0.2f;
        fadeInOut.FadeInEvent(m_fadeTime);
        yield return new WaitForSeconds(m_fadeTime);
        openObject.SetActive(true);
        if(nowOpenMenu)nowOpenMenu.SetActive(false);
        fadeInOut.FadeOutEvent(m_fadeTime);
        yield return new WaitForSeconds(m_fadeTime);
        nowOpenMenu = openObject;
    }
}
