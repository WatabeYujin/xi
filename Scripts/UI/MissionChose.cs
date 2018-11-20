using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class MissionChose : MonoBehaviour {
    private string sceneName;
    [SerializeField]
    private GameObject yesButton;
    [SerializeField]
    private GameObject noButton;
    [SerializeField]
    private Image Window;
    [SerializeField]
    private FadeInOut fadeInOut;

    private AsyncOperation loadScene;

    public void MissionButton(string m_sceneName)
    {
        sceneName = m_sceneName;
        StartCoroutine(UIMove(false));
    }

    IEnumerator UIMove(bool isOpen) {
        const float m_buttonMoveTime = 0.1f;
        const float m_buttonSpace = 50;
        Window.enabled = !isOpen;
        switch (isOpen) {
            case false:
                yesButton.transform.position = new Vector2(yesButton.transform.position.x, Screen.height);
                noButton.transform.position = new Vector2(noButton.transform.position.x, -Screen.height);
                yesButton.SetActive(true);
                noButton.SetActive(true);
                yesButton.transform.DOLocalMoveY(m_buttonSpace, m_buttonMoveTime);
                noButton.transform.DOLocalMoveY(-m_buttonSpace, m_buttonMoveTime);
                yield return new WaitForSeconds(m_buttonMoveTime);
                break;
            case true:
                yesButton.transform.DOLocalMoveY(-Screen.height, m_buttonMoveTime);
                noButton.transform.DOLocalMoveY(Screen.height, m_buttonMoveTime);
                yield return new WaitForSeconds(m_buttonMoveTime);
                yesButton.SetActive(false);
                noButton.SetActive(false);
                break;
        }
    }


    public void yes()
    {
        StartCoroutine(SceneMove());
    }

    public void no()
    {
        StartCoroutine(UIMove(true));
    }

    IEnumerator SceneMove()
    {
        const float m_time = 0.5f;

        fadeInOut.FadeInEvent(m_time);
        yield return new WaitForSeconds(m_time);
        SceneManager.LoadScene(sceneName);
    }
}
