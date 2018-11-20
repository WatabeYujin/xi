using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class ResultEvent : MonoBehaviour {
    [SerializeField]
    private int actCount;
    [SerializeField]
    private GameObject warpEffect;
    [SerializeField]
    private GameObject warpParticle;
    [SerializeField]
    private FadeInOut fadeInOut;
    [SerializeField]
    private GameObject ActWindow;
    [SerializeField]
    private GameObject playerSet;
    [SerializeField]
    private ActUpgrade actUpgrade;
    [SerializeField]
    private GameObject actIcon;
    [SerializeField]
    private GameObject upgradeWindow;

    private string moveScene;
    private bool lastAct = false;
    private AsyncOperation loadScene;
    private Vector3 teleporterPosition;
    private int upGradePoint;
    private int nowAct = 0;
    private List<GameObject> actIconList = new List<GameObject>();

    void Start()
    {
        nowAct = 0;
    }

    public void Goal(string moveSceneName,bool isLastAct,Vector3 m_teleporterPosition,int m_upgradePoint)    
    {
        teleporterPosition = m_teleporterPosition;
        moveScene = moveSceneName;
        lastAct = isLastAct;
        upGradePoint = m_upgradePoint;
        StartCoroutine(GoalEvent());
    }

    IEnumerator GoalEvent()
    {
        const float m_fadeTime = 1;
        
        SceneMove(false);
        EffectSpawn(warpEffect);
        GetComponent<PlayerController>().ControlStop();
        StartCoroutine(PlayerMove());

        for (int i = 30; i > 0; i--)
        {
            Camera.main.fieldOfView -= 1.5f;
            yield return null;
        }
        EffectSpawn(warpParticle);
        fadeInOut.FadeInEvent(m_fadeTime);
        for (float i = 10; i > 0; i--)
        {
            //gameObject.SetActive(false);
            Camera.main.fieldOfView += 4.5f;
            yield return null;
        }
        yield return new WaitForSeconds(m_fadeTime);
        ActWindow.SetActive(true);
        yield return StartCoroutine(ActIconPrint(nowAct));
        //gameObject.SetActive(true);
        transform.position = Vector3.zero;
        if (lastAct)
            StartCoroutine(LastAct());
        else
            StartCoroutine(NextAct(m_fadeTime, transform));
    }

    IEnumerator ActIconPrint(int nowActCount)
    {
        const float m_iconMovetime = 0.2f;
        const int m_iconInterval = 75;
        int m_iconFirstPositionX = Screen.width / 2 -m_iconInterval / 2 * (actCount-1);
        Color m_activActColor = Color.red;
        Color m_notActivActColor = Color.gray;
        for (int i = 0; i < actCount; i++)
        {
            GameObject m_icon = Instantiate(actIcon, ActWindow.transform);
            actIconList.Add(m_icon);
            int m_iconPos =
                m_iconInterval * i + m_iconFirstPositionX;
            actIconList[i].transform.DOMoveX(m_iconPos, m_iconMovetime);
            Image image = actIconList[i].GetComponent<Image>();
            if (i <= nowAct) image.color= m_activActColor;
            else image.color = m_notActivActColor;
        }
        yield return new WaitForSeconds(m_iconMovetime);
    }

    IEnumerator ActIconDelete()
    {
        const float m_iconMovetime = 0.2f;
        
        for (int i = 0; i < actCount; i++)
        {
            actIconList[i].transform.DOMoveX(Screen.width+100, 0.2f);
            Destroy(actIconList[i], m_iconMovetime);
        }
        yield return new WaitForSeconds(m_iconMovetime);
        actIconList.Clear();
    }
    
    IEnumerator NextAct(float time, Transform colTransform)
    {
        nowAct++;
        if (upGradePoint != 0)
            yield return StartCoroutine(UpgradeWindowOpen());
        else
        {
            upgradeWindow.SetActive(false);
            yield return new WaitForSeconds(2);
        }
        SceneMove(true);
        yield return StartCoroutine(ActIconDelete());
        fadeInOut.FadeOutEvent(time);
        yield return new WaitForSeconds(time);
        Debug.Log("windowClose");
        colTransform.GetComponent<PlayerController>().ControlStart();
        
    }

    IEnumerator UpgradeWindowOpen()
    {
        upgradeWindow.SetActive(true);
        upgradeWindow.GetComponent<ActUpgrade>().WindowOpen();
        float waitTime = 10;
        yield return new WaitForSeconds(waitTime);
        upgradeWindow.GetComponent<ActUpgrade>().WindowClose();
    }

    IEnumerator LastAct() {
        const string m_string = "Title";
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(m_string);
    }

    void SceneMove(bool isLoad)
    {
        switch (isLoad)
        {
            case false:
                DontDestroyOnLoad(playerSet);
                loadScene = SceneManager.LoadSceneAsync(moveScene);
                loadScene.allowSceneActivation = false;
                break;
            case true:
                SceneManager.UnloadSceneAsync(moveScene);
                loadScene.allowSceneActivation = true;
                break;
        }
    }

    IEnumerator PlayerMove()
    {
        const float m_moveTime = 1;
        Vector3 m_movePosition = new Vector3(teleporterPosition.x,
                                             transform.position.y,
                                             teleporterPosition.z);
        transform.DOMove(
            m_movePosition,
            m_moveTime);
        yield return new WaitForSeconds(m_moveTime);
    }

    void EffectSpawn(GameObject effect)
    {
        Transform m_effect = Instantiate(effect).transform;
        m_effect.position = teleporterPosition;
    }
    
}
