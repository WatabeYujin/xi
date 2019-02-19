using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Goal : MonoBehaviour {
    [SerializeField]
    private string moveScene;
    [SerializeField]
    private StageCreateController stageCreateController;

    void OnTriggerEnter(Collider col) {
        if (col.tag != "Player") return;
        if (stageCreateController==null)
            col.GetComponent<ResultEvent>().Goal(transform.position);
        else
            stageCreateController.StageMenuOpenCloase(false);
    }
}
