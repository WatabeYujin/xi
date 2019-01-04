using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyScriptableObject/StageData")]
public class StageData : ScriptableObject
{
    [System.Serializable]
    public class StageList
    {
        public string stageName;
        public string stageDetails;
        public int[][] stageGimics;
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
    }
    public StageList[] nodelist;
}
