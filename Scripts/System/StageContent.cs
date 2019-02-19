using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageContent : MonoBehaviour {
    [SerializeField]
    private Text stageNameText;
    [SerializeField]
    private Text stageDetailsText;

    public void StageTextSet(string stageName,string stageDetails)
    {
        stageNameText.text = stageName;
        stageDetailsText.text = stageDetails;
    }

}
