using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyScriptableObject/CreateStageData")]
public class CreateStageData : ScriptableObject{

    [SerializeField]
    private int[] gimmicID = new int[400];      //配置されているギミックのID
    [SerializeField]
    private int[] gimmicRotate = new int[400];  //配置されているギミックの角度
    [SerializeField]
    private int totalCost = 0;                      //ステージの総コスト
    [SerializeField]
    private string stageName;                       //ステージ名
    [SerializeField]
    private string stageDetails;                    //ステージ詳細
    [SerializeField]
    private bool isEdit=true;                       //編集許可されているか
    [SerializeField]
    private int offlineStageID = -1;            //オフラインでのステージID(自作の場合-1)

    public int GetGimmicID(int x,int y)
    {
        return gimmicID[x * 20 + y];
    }
    public void SetGimmicID(int x, int y , int ID)
    {
        gimmicID[x * 20 + y] = ID;
    }

    public int GetOfflineStageID()
    {
        return offlineStageID;
    }

    public int GetgimmicRotate(int x, int y)
    {
        return gimmicRotate[x * 20 + y];
    }
    public void SetgimmicRotate(int x, int y, int rotate)
    {
        gimmicRotate[x * 20 + y] = rotate;
    }
    public void UpdateScriptableObject(SaveData.FromJsonStageData loadSaveData)
    {
        gimmicID = loadSaveData.gimmicID;
        gimmicRotate = loadSaveData.gimmicRotate;
        totalCost = loadSaveData.totalCost;
        stageName = loadSaveData.stageName;
        stageDetails = loadSaveData.stageDetails;
        isEdit = loadSaveData.isEdit;
    }
    public void CloneScriptableObject(CreateStageData cloneStageData)
    {
        gimmicID = cloneStageData.gimmicID;
        gimmicRotate = cloneStageData.gimmicRotate;
        totalCost = cloneStageData.totalCost;
        stageName = cloneStageData.stageName;
        stageDetails = cloneStageData.stageDetails;
        isEdit = cloneStageData.isEdit;
        offlineStageID = cloneStageData.GetOfflineStageID();
    }

    public void RequiredClear(int id)
    {
        for (int i=0;i<400;i++) {
            if (gimmicID[i] == id)
            {
                gimmicID[i] = 0;
                break;
            }
        }
    }
}
