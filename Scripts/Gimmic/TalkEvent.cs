using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkEvent : MonoBehaviour {
    private TalkControl talkControl;
    private bool isRunning = false;

    [SerializeField]
    private string[] talktext;


    void OnTriggerEnter(Collider col) {
        if (isRunning) return;
        if (col.tag != "Player") return;
        talkControl = col.GetComponent<TalkControl>();
        StartCoroutine(Talk());
    }

    IEnumerator Talk() {
        const float m_waitTime=0.2f;
        foreach(string text in talktext)
        {
            talkControl.TalkSet(text, 1, 1, 1, true);
            yield return new WaitForSeconds(text.Length * m_waitTime);
            Debug.Log(text.Length * m_waitTime);
        }
        Destroy(this);
    }
}
