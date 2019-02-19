using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class ResultEvent : MonoBehaviour {
    [SerializeField]
    private GameObject warpEffect;
    [SerializeField]
    private GameObject warpParticle;
    [SerializeField]
    private FadeInOut fadeInOut;
    [SerializeField]
    private GameObject playerSet;
    [SerializeField]
    SaveScriptableObject2 savedata;
    [SerializeField]
    CreateStageData loadstage;
    [SerializeField]
    StageData offlinestageList;
    [SerializeField]
    private StageCreateController stageCreateController;
    
    private AsyncOperation loadScene;
    private Vector3 teleporterPosition;
    private int upGradePoint;


    void OnTriggerEnter(Collider col)
    {
        if (col.tag != "Player") return;
        if (stageCreateController == null)
            Goal(transform.position);
        else
            stageCreateController.StageMenuOpenCloase(false);
    }

    public void Goal(Vector3 m_teleporterPosition)    
    {
        teleporterPosition = m_teleporterPosition;
        StartCoroutine(GoalEvent());
    }

    IEnumerator GoalEvent()
    {
        const float m_fadeTime = 1;
        
        EffectSpawn(warpEffect);
        RewardGetEvent();
        PlayerController.playerController.ControlStop();
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
        //gameObject.SetActive(true);
        transform.position = Vector3.zero;
        StartCoroutine(LastAct());
    }
    IEnumerator LastAct() {
        const string m_string = "Title";
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(m_string);
    }
    

    void RewardGetEvent()
    {
        savedata.flagCredit += RewardFlagCreditCheck();
        int m_stageID = loadstage.GetOfflineStageID();
        if (m_stageID == savedata.missionProgress)
            savedata.missionProgress++;
        GetComponent<SaveData>().Save();
    }

    int RewardFlagCreditCheck()
    {
        return 100;         //仮で固定値
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
