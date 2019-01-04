using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GimmicContent : MonoBehaviour {

    [SerializeField]
    private Text gimmicNameText;
    [SerializeField]
    private Image gimmicImage;
    [SerializeField]
    private StageCreateController stageCreateController;
    private int gimmicID = 0;

    public void DataSet(string gimmicName, Sprite gimmicSprite) {
        gimmicNameText.text = gimmicName;
        gimmicImage.sprite = gimmicSprite;
    }
}
