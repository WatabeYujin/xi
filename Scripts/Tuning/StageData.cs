using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyScriptableObject/StageData")]
public class StageData : ScriptableObject
{
    [System.Serializable]
    public class StageList
    {
        [SerializeField]
        private string stageName = "";          //ステージ名
        [SerializeField,TextArea(1,3)]
        private string stageDetails ="";        //ステージの詳細
        [SerializeField]
        private CreateStageData createstagedata;//読み込むステージのデータ
        [SerializeField]
        private int stageRunk = 0;              //受注可能なステージのランク
        [SerializeField]
        private bool isRunkUpMission = false;

        public string GetNodeName
        {
            get
            {
                return stageName;
            }
        }
        public string GetNodeDetails
        {
            get
            {
                return stageDetails;
            }
        }
        public CreateStageData GetStageData
        {
            get
            {
                return createstagedata;
            }
        }
        public int GetStageRunk
        {
            get
            {
                return stageRunk;
            }
        }
    }
    public StageList[] stageList;
}
