using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GimmicListView : MonoBehaviour {
    [SerializeField]
    private SaveScriptableObject2 saveData;
    [SerializeField]
    private GimmicData gimmicData;
    [SerializeField]
    private GameObject gimmicViewContent;
    [SerializeField]
    private StageCreateController stageCreateController;
    [SerializeField]
    private Transform gimmicViewList;
    private List<GameObject> spawnGimmicContents = new List<GameObject>();

    void Start () {
        ListOpen();

    }
	
    public void DropDown(int value)
    {
        GimmicData.GimmicType m_gimmicType;
        switch (value)
        {
            case 0:
                m_gimmicType = GimmicData.GimmicType.None;
                break;
            case 1:
                m_gimmicType = GimmicData.GimmicType.Object;
                break;
            case 2:
                m_gimmicType = GimmicData.GimmicType.Enemy;
                break;
            case 3:
                m_gimmicType = GimmicData.GimmicType.Item;
                break;
            case 4:
                m_gimmicType = GimmicData.GimmicType.Required;
                break;
            default:
                m_gimmicType = GimmicData.GimmicType.None;
                break;
        }
        ListOpen(m_gimmicType);
    }

    public void ListOpen(GimmicData.GimmicType gimmicType = GimmicData.GimmicType.None)
    {
        foreach(GameObject spawn in spawnGimmicContents)
        {
            Destroy(spawn);
        }
        for(int i=1;i<gimmicData.gimmicList.Length-1;i++)
        {
            //関数で表示するか非表示か確かめる
            if(!isView(gimmicType, i))
                continue;   //非表示の場合次のリストを調べる

            GameObject m_nodeObj = Instantiate(gimmicViewContent, gimmicViewList);
            spawnGimmicContents.Add(m_nodeObj);
            m_nodeObj.GetComponent<GimmicContent>().DataSet(gimmicData.gimmicList[i].GetGimmicName, gimmicData.gimmicList[i].GetGimmicImage);
            //ラムダ式だとすべての値が変更されてしまう為
            int m_id = i;
            //ラムダ式でないと引数付きイベントが設定できないので
            //m_nodeObj.GetComponent<Button>().onClick.AddListener(() => { stageCreateController.GimmicButton(m_id); });
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerDown; //PointerClickの部分は追加したいEventによって変更してね
            entry.callback.AddListener((x) => { stageCreateController.GimmicButton(m_id); });  //ラムダ式の右側は追加するメソッドです。
            m_nodeObj.GetComponent<EventTrigger>().triggers.Add(entry);
        }
    }

    /// <summary>
    /// ギミックの表示非表示を調べる
    /// </summary>
    /// <param name="gimmicType">表示するギミックの種類</param>
    /// <param name="gimmicID">ギミックのID</param>
    /// <returns>表示する場合trueを返す</returns>
    bool isView(GimmicData.GimmicType gimmicType, int gimmicID)
    {
        //大前提としてギミックを所持しているか
        if (saveData.gimmicPossession[gimmicID] == false)
            return false;
        //表示するギミックが一致する・あるいは全表示か
        if (gimmicType != GimmicData.GimmicType.None &&
            gimmicType != gimmicData.gimmicList[gimmicID].GetGimmicType)
            return false;

        //全条件をクリアしている為ギミックを表示
        return true;
    }
}
