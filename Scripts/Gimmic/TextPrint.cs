using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextPrint : MonoBehaviour {
    [SerializeField]
    private Text text;
    [SerializeField]
    private string enterText;
    private TalkControl talkControl;

    void Start()
    {
        talkControl = GameObject.Find("Player").GetComponent<TalkControl>();
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag  != "Player") return;
        TextStringSet(enterText);
    }

    void TextStringSet(string m_string)
    {
        text.text = m_string;
        talkControl.TalkSet(m_string, 1, 1, 1, false);
        Destroy(this);
    }
}
