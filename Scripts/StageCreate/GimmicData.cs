using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyScriptableObject/GimmicData")]
public class GimmicData : ScriptableObject{
    
    //ギミックの種類
    public enum GimmicType
    {
        Object,
        Enemy,
        Item,
        Required,
        None
    }

    /// <summary>
    /// ギミック用のリスト
    /// </summary>
    [System.Serializable]
    public class GimmicList
    {
        [SerializeField]
        private string gimmicName;          //ギミックの名前
        public string GetGimmicName {
            get
            {
                return gimmicName;
            }
        }
        [SerializeField]
        private GimmicType gimmicType;      //ギミックの種類
        public GimmicType GetGimmicType
        {
            get
            {
                return gimmicType;
            }
        }
        [SerializeField]
        private int cost;                   //必要コスト
        public int GetGimmicCost
        {
            get
            {
                return cost;
            }
        }
        [SerializeField]
        private string gimmicDetails;       //ギミックの説明
        public string GetGimmicDetails
        {
            get
            {
                return gimmicDetails;
            }
        }
        [SerializeField]
        private Sprite gimmicImage;         //プレビュー上の画像
        public Sprite GetGimmicImage
        {
            get
            {
                return gimmicImage;
            }
        }
        [SerializeField]
        private GameObject gimmicPrefub;    //ギミックのプレファブ
        public GameObject GetGimmicPrefub
        {
            get
            {
                return gimmicPrefub;
            }
        }
    }
    public GimmicList[] gimmicList;         //ギミックのリスト
}
