using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageSpawn : MonoBehaviour {

    [SerializeField]
    GimmicData gimmicData;
    [SerializeField]
    CreateStageData data;
    [SerializeField]
    CreateStageData defaultdata;
    [SerializeField]
    private Transform[,] spawnObj = new Transform[20, 20];
    [SerializeField]
    private bool isEdit;
    [SerializeField]
    private Transform player;
    [SerializeField]
    private Transform goal;
    [SerializeField]
    private Text costText;

    private int totalcost = 0;

    private Vector3 stageBase = new Vector3(2.5f, 0f, 2.5f);
    private const int floorSize = 20;
    private const int costLimit = 500;
    private const int startGimmicID = 1;
    private const int goalGimmicID = 2;
    private const float floorScale = 5;
    private PlayerController playerController;

    private void Start()
    {
        if (!isEdit)
        {
            StartCoroutine(LoadStage());
            return;
        }
    }
    void OnEnable()
    {
        playerController = PlayerController.playerController;
    }
    private IEnumerator LoadStage()
    {
        yield return StageLoad();
        playerController.GetSetisActive=true;
    }

    public void StageSet()
    {
        StartCoroutine(StageLoad());
    }

    IEnumerator StageLoad()
    {
        for (int list_x = 0; list_x < spawnObj.GetLength(0); list_x++)
        {
            for (int list_y = 0; list_y < spawnObj.GetLength(1); list_y++)
            {
                if (data.GetGimmicID(list_x, list_y) == 0)
                    continue;
                StageObjUpdate(list_x, list_y, data.GetGimmicID(list_x, list_y), data.GetgimmicRotate(list_x, list_y));
                yield return new WaitForEndOfFrame();
            }
        }
    }

    public void StageObjUpdate(int x ,int y,int id,int rotate)
    {
        if (id == startGimmicID || id == goalGimmicID)
        {
            RequiredUpdate(x, y, id);
            return;
        }
        if (spawnObj[x, y] != null)
        {
            int m_id = data.GetGimmicID(x,y);
            totalcost -= gimmicData.gimmicList[m_id].GetGimmicCost;
            Destroy(spawnObj[x, y].gameObject);
            data.SetGimmicID(x, y , 0);
        }
        if (id != 0)
        {
            Transform m_obj = Instantiate(gimmicData.gimmicList[id].GetGimmicPrefub,this.transform).transform;
            m_obj.position = new Vector3(x * floorScale, 0, y * floorScale) + stageBase;
            m_obj.eulerAngles = Vector3.up * rotate;
            spawnObj[x, y] = m_obj;
            data.SetGimmicID(x,y,id);
            data.SetgimmicRotate(x, y,rotate);
            totalcost += gimmicData.gimmicList[id].GetGimmicCost;
            EnemyAction m_enemyaction = m_obj.GetComponent<EnemyAction>();
            if (m_enemyaction != null)
                m_enemyaction.SetSpaenPos(m_obj.position);
        }
        ViewCost();
    }



    void RequiredUpdate(int x, int y, int id)
    {
        StageObjUpdate(x, y, 0, 0);
        switch (id)
        {
            case startGimmicID:
                player.position= new Vector3(x * floorScale, 0, y * floorScale) + stageBase;
                break;
            case goalGimmicID:
                goal.position = new Vector3(x * floorScale, 0, y * floorScale) + stageBase;
                break;
            default:
                break;
        }
        data.RequiredClear(id);
        data.SetGimmicID(x, y, id);
        data.SetgimmicRotate(x, y, 0);
    }

    public void SetGimmicReset()
    {
        for(int x = 0; x < 20; x++)
        {
            for (int y = 0; y < 20; y++)
            {
                
                if (spawnObj[x, y] != null)
                    Destroy(spawnObj[x, y].gameObject);
                data.SetGimmicID(x, y, 0);
                data.SetgimmicRotate(x, y, 0);
            }
        }
        StageObjUpdate(0, 0, 1, 0);
        StageObjUpdate(19, 19, 2, 0);
        totalcost = 0;
    }

    /// <summary>
    /// 指定地点の必須オブジェクトの有無を調べる
    /// </summary>
    /// <param name="x">xの座標</param>
    /// <param name="y">yの座標</param>
    /// <returns>存在する場合trueを返す</returns>
    public bool RequiredCheck(int x, int y)
    {
        int m_getID = data.GetGimmicID(x, y);
        if (gimmicData.gimmicList[m_getID].GetGimmicType == GimmicData.GimmicType.Required)
            return true;
        else
            return false;
    }
    public void StageReset()
    {
        data = defaultdata;
        StartCoroutine(StageLoad());
    }
    
    void ViewCost()
    {
        if (!isEdit) return;
            costText.text = "TOTALCOST：" + totalcost + "／" + costLimit;
    }
}
