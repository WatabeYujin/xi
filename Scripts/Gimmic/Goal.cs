using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Goal : MonoBehaviour {
    [SerializeField]
    private string moveScene;
    [SerializeField]
    private bool lastAct = false;
    [SerializeField]
    private int upgradePoint=3;

    void OnTriggerEnter(Collider col) {
        if (col.tag != "Player") return;
        col.GetComponent<ResultEvent>().Goal(moveScene,lastAct,transform.position, upgradePoint);
    }
}
