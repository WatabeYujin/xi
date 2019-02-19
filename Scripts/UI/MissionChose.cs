using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class MissionChose : MonoBehaviour {
    [SerializeField]
    private GameObject yesButton;
    [SerializeField]
    private GameObject noButton;
    [SerializeField]
    private Image Window;
    [SerializeField]
    private FadeInOut fadeInOut;
    [SerializeField]
    private SaveScriptableObject2 save;
    [SerializeField]
    private StageData stageData;
    [SerializeField]
    private GameObject content;
    [SerializeField]
    private Transform contentParent;
    [SerializeField]
    private CreateStageData loadstage;

    private List<GameObject> spawnStageContents = new List<GameObject>();
    
    private CreateStageData selectStageData;

    public void Awake()
    {
        MissionListOpen();
    }

    private void MissionListOpen()
    {
        foreach (GameObject spawn in spawnStageContents)
        {
            Destroy(spawn);
        }
        for (int i = stageData.stageList.Length-1; i >= 0; i--)
        {
            //ミッションが受注可能か調べる
            Debug.Log(i);
            if (stageData.stageList[i].GetStageRunk > save.missionProgress)
                continue;   //ランクが足りない場合、次のステージ

            GameObject m_contentObj = Instantiate(content, contentParent);
            spawnStageContents.Add(m_contentObj);
            m_contentObj.GetComponent<StageContent>().StageTextSet(stageData.stageList[i].GetNodeName, stageData.stageList[i].GetNodeDetails);
            //ラムダ式でないと引数付きイベントが設定できないので
            CreateStageData m_stagedata = stageData.stageList[i].GetStageData;
            m_contentObj.GetComponent<Button>().onClick.AddListener(() => { this.MissionButton(m_stagedata); });
        }
    }
    public void MissionButton(CreateStageData selectstage)
    {
        selectStageData = selectstage;
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
        const string m_loadScene ="PlayStage";

        loadstage.CloneScriptableObject(selectStageData);
        fadeInOut.FadeInEvent(m_time);
        yield return new WaitForSeconds(m_time);
        SceneManager.LoadScene(m_loadScene);
    }
}
