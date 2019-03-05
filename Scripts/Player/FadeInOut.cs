using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FadeInOut : MonoBehaviour {
    [SerializeField]
    private bool startFadeIn = true;
    [SerializeField]
    private Image fadeImage;
    const string m_AlphaPropertieName = "_Alpha";
    private void Start()
    {
        if (!startFadeIn) return;
        FadeOutEvent(0.3f);
    }

    public void FadeInEvent(float time) {
        StartCoroutine(FadeIn(time));
    }

    public void FadeOutEvent(float time) {
        StartCoroutine(FadeOut(time));
    }

    IEnumerator FadeIn(float time) {
        fadeImage.rectTransform.localScale = new Vector3(1,1,1);
        fadeImage.enabled = true;
        Material m_material = fadeImage.material;
        float current = 0;
        while (current < time)
        {
            m_material.SetFloat(m_AlphaPropertieName, current / time);
            yield return new WaitForEndOfFrame();
            current += Time.deltaTime;
        }
        m_material.SetFloat(m_AlphaPropertieName, 1);
    }

    IEnumerator FadeOut(float time) {
        fadeImage.rectTransform.localScale = new Vector3(-1, 1, 1);
        fadeImage.enabled = true;
        Material m_material = fadeImage.material;
        float current = 0;
        while (current < time)
        {
            m_material.SetFloat(m_AlphaPropertieName, 1 - current / time);
            yield return new WaitForEndOfFrame();
            current += Time.deltaTime;
        }
        m_material.SetFloat(m_AlphaPropertieName, 0);
        fadeImage.enabled = false;
    }


}
