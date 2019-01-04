using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "MyScriptableObject/Savedate")]
public class SaveScriptableObject2 : ScriptableObject
{
    public List<NodeDataClass> statusNode;          //ステータスノードのデータ
    public List<NodeDataClass> straightNode;        //ストレートノードのデータ
    public List<NodeDataClass> flickDodgeNode;      //ステータスノードのデータ
    public List<NodeDataClass> snipeCannonNode;     //ステータスノードのデータ

    public int statusNodePoint;                     //ステータスノードのポイント
    public int statusNodeAmountPoint;               //ステータスノードの総ポイント
    private const int statusNodeMaxPoitnt = 30;     //ステータスノードポイントの最大値（定数）
    public int straightNodePoint;                   //ストレートノードのポイント
    public int straightNodeAmountPoint;             //ストレートノードの総ポイント
    private const int straightNodeMaxPoitnt = 30;   //ステータスノードポイントの最大値（定数）
    public int flickDodgeNodePoint;                 //フリックドッジノードのポイント
    public int flickDodgeNodeAmountPoint;           //フリックドッジノードの総ポイント
    private const int flickDodgeNodeMaxPoitnt = 30; //ステータスノードポイントの最大値（定数）
    public int snipeCannonNodePoint;                //スナイプカノンノードのポイント
    public int snipeCannonNodeAmountPoint;          //スナイプカノンノードの総ポイント
    private const int snipeCannonNodeMaxPoitnt = 30;//ステータスノードポイントの最大値（定数）

    public List<bool> gimmicPossession;             //ギミックの所持状態

    public int missionProgress;                     //オフラインミッションの進捗
    public string playerName;                       //プレイヤーの名前

    public bool isChanged = false;                  //変更を加えたか否か

    private int flagCredit = 0;                     //通貨

    public void UpdateScriptableObject(SaveData.FromJsonSaveData loadSaveData)
    {
        statusNode = new List<NodeDataClass>(loadSaveData.statusNode);
        statusNodePoint = loadSaveData.statusNodePoint;
        statusNodeAmountPoint = loadSaveData.statusNodeAmountPoint;
        straightNode = new List<NodeDataClass>(loadSaveData.straightNode);
        straightNodePoint = loadSaveData.straightNodePoint;
        straightNodeAmountPoint = loadSaveData.straightNodeAmountPoint;
        flickDodgeNode = new List<NodeDataClass>(loadSaveData.flickDodgeNode);
        flickDodgeNodePoint = loadSaveData.flickDodgeNodePoint;
        flickDodgeNodeAmountPoint = loadSaveData.flickDodgeNodeAmountPoint;
        snipeCannonNode = new List<NodeDataClass>(loadSaveData.snipeCannonNode);
        snipeCannonNodePoint = loadSaveData.snipeCannonNodePoint;
        snipeCannonNodeAmountPoint = loadSaveData.snipeCannonNodeAmountPoint;
        gimmicPossession = new List<bool>(loadSaveData.gimmicPossession);
        missionProgress = loadSaveData.missionProgress;
        playerName = loadSaveData.playerName;
        flagCredit = loadSaveData.flagCredit;
        Debug.Log(playerName);
    }

    public void DefaultLoadData(DefaultScriptableObject loadSaveData)
    {
        statusNode.Clear();
        
        statusNode.AddRange(loadSaveData.statusNode);
        statusNodePoint = loadSaveData.statusNodePoint;
        statusNodeAmountPoint = loadSaveData.statusNodeAmountPoint;
        straightNode.Clear();
        straightNode.AddRange(loadSaveData.straightNode);
        straightNodePoint = loadSaveData.straightNodePoint;
        straightNodeAmountPoint = loadSaveData.straightNodeAmountPoint;
        flickDodgeNode.Clear();
        flickDodgeNode.AddRange(loadSaveData.flickDodgeNode);
        flickDodgeNodePoint = loadSaveData.flickDodgeNodePoint;
        flickDodgeNodeAmountPoint = loadSaveData.flickDodgeNodeAmountPoint;
        snipeCannonNode.Clear();
        snipeCannonNode.AddRange(loadSaveData.snipeCannonNode);
        snipeCannonNodePoint = loadSaveData.snipeCannonNodePoint;
        snipeCannonNodeAmountPoint = loadSaveData.snipeCannonNodeAmountPoint;
        gimmicPossession = new List<bool>(loadSaveData.gimmicPossession);
        missionProgress = 0;
        playerName = "";
        flagCredit = 0;
        Debug.Log(playerName);
    }

    //public List<T> CopyList<T>(List<T> target) {
    //    var res = new List<T>();
    //    foreach (T item in target) {
    //        res.Add((item as System.ICloneable));
    //    }
    //    return res;
    //}
}

/// <summary>
/// オフラインミッションデータ用クラス
/// </summary>
[System.Serializable]
public class MissionDataClass
{
    [SerializeField]
    private string missionName;         //ミッション名
    [SerializeField]
    private string missionDetails;      //ミッション詳細
    [SerializeField]
    private bool isAbleTakeOrder;       //ミッションが受注可能か
    [SerializeField]
    private bool isClear;               //ミッションのクリア状況

    /// <summary>
    /// ミッション名の取得
    /// </summary>
    public string GetMissionName
    {
        get
        {
            return missionName;
        }
    }

    /// <summary>
    /// ミッション詳細の取得
    /// </summary>
    public string GetMissionDetails
    {
        get
        {
            return missionDetails;
        }
    }

    /// <summary>
    /// ミッション受注可能か取得
    /// </summary>
    public bool GetIsAbleTakeOrder
    {
        get
        {
            return isAbleTakeOrder;
        }
    }

    /// <summary>
    /// ミッションの受注可能に更新する
    /// </summary>
    public void IsAbleTakeOrderUpdate()
    {
        isAbleTakeOrder = true;
    }

    /// <summary>
    /// ミッションのクリア状況の取得
    /// </summary>
    public bool GetClearStatus
    {
        get
        {
            return isClear;
        }
    }

    /// <summary>
    /// ミッションクリアの際の更新
    /// </summary>
    /// <returns>クリア済みだった場合falseを返す</returns>
    public bool ClearMission()
    {
        if (isClear) return false;
        isClear = true;
        return true;
    }
}

/// <summary>
/// ノードデータ用クラス
/// </summary>
[System.Serializable]
public class NodeDataClass : System.ICloneable
{

    [SerializeField]
    private int level;      //ノードのレベル
    [SerializeField]
    private bool possession;//ノードの所持状態

    public object Clone()
    {
        NodeDataClass dataClass = new NodeDataClass();
        dataClass.level = level;
        dataClass.possession = possession;
        return dataClass;
    }

    //public NodeDataClass(NodeDataClass value)
    //{
    //    int m_level = value.level;
    //    bool m_possession = value.possession;
    //    level = m_level;
    //    possession = m_possession;

    //}
    //public NodeDataClass()
    //{
    //}



    /// <summary>
    /// レベルの取得
    /// </summary>
    public int GetLevel
    {
        get
        {
            if (!possession) return 0;
            return level;
        }
    }

    /// <summary>
    /// レベルを一つ上げる
    /// </summary>
    public void LevelUp()
    {
        level++;
        
        Debug.Log(level);
    }

    /// <summary>
    /// レベルを一つ下げる
    /// </summary>
    public void LevelDown()
    {
        level--;
    }

    /// <summary>
    /// 所持状態の取得・更新
    /// </summary>
    public bool GetSetPossesion
    {
        get
        {
            return possession;
        }
        set
        {
            possession = value;
        }
    }
}
