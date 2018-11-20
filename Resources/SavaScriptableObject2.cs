using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Savedate")]
public class SavaScriptableObject2 : ScriptableObject
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

    public int missionProgress;                     //オフラインミッションの進捗
    public string playerName;                       //プレイヤーの名前
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
public class NodeDataClass
{
    [SerializeField]
    private string name;    //ノードの名前（読み取り専用）
    [SerializeField]
    private string details; //ノードの名前（読み取り専用）
    [SerializeField]
    private int level;      //ノードのレベル
    [SerializeField]
    private bool possession;//ノードの所持状態

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
    /// レベルの更新
    /// </summary>
    public int SetLevel
    {
        set{
            level = value;
        }
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

    /// <summary>
    /// ノード名の取得
    /// </summary>
    public string GetNodeName
    {
        get
        {
            return name;
        }
    }

    /// <summary>
    /// ノード詳細の取得
    /// </summary>
    public string GetNodeDetails
    {
        get
        {
            return details;
        }
    }
}