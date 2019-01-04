using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "MyScriptableObject/Defaultdate")]
public class DefaultScriptableObject : ScriptableObject
{
    [SerializeField]
    public NodeDataClass[] statusNode =
        new NodeDataClass[] { };                    //ステータスノードのデータ
    public NodeDataClass[] straightNode =
        new NodeDataClass[] { };                    //ストレートノードのデータ
    public NodeDataClass[] flickDodgeNode =
        new NodeDataClass[] { };                    //ステータスノードのデータ
    public NodeDataClass[] snipeCannonNode =
        new NodeDataClass[] { };                    //ステータスノードのデータ

    public int statusNodePoint;                     //ステータスノードのポイント
    public int statusNodeAmountPoint;               //ステータスノードの総ポイント
    public int straightNodePoint;                   //ストレートノードのポイント
    public int straightNodeAmountPoint;             //ストレートノードの総ポイント）
    public int flickDodgeNodePoint;                 //フリックドッジノードのポイント
    public int flickDodgeNodeAmountPoint;           //フリックドッジノードの総ポイント
    public int snipeCannonNodePoint;                //スナイプカノンノードのポイント
    public int snipeCannonNodeAmountPoint;          //スナイプカノンノードの総ポイント

    public List<bool> gimmicPossession;             //ギミックの所持状態

}