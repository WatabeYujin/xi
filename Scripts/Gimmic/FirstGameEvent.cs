using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FantomLib;

public class FirstGameEvent : MonoBehaviour {
    
    [SerializeField]
    private TalkControl talkControl;
    [SerializeField]
    private GameObject FirstWindow;
    [SerializeField]
    private GameObject YesNo;
    [SerializeField]
    private GameObject NameInputWindow;
    [SerializeField]
    private FadeInOut fade;

    private Image image;
    [SerializeField]
    private SaveData saveData;

    private SavaScriptableObject2 savaScriptableObject;

    [RuntimeInitializeOnLoadMethod]
    void Start () {
        saveData.Load();
        image = GetComponent<Image>();
        savaScriptableObject = Resources.Load("SaveData") as SavaScriptableObject2;
        if (saveData.Load() && savaScriptableObject.playerName != "")
        {
            Debug.Log(savaScriptableObject.name);
            StartTalk();
            Destroy(gameObject);
            return;
        }
        saveData.DataInitialization();
        NameInputWindow.SetActive(true);
    }
    public void NameCheck(string name)
    {
        if (name == "")
        {
            OutName("何も入力されていませんが・・？");
            return;
        }
        savaScriptableObject.playerName= name;
        saveData.Save();
        StartCoroutine(NameInput());
    }

    void OutName(string printText)
    {
        talkControl.TalkSet(printText, 1, 1, 1, true);
    }

    IEnumerator NameInput()
    {
        NameInputWindow.SetActive(false);
        fade.FadeInEvent(0);
        talkControl.TalkSet("[%p]ですね。了解しました。", 1, 1, 1, true);
        
        for(float i = 1; i > 0; i -= 0.1f)
        {
            image.color = new Color(0, 0, 0, i);
            yield return null;
        }
        fade.FadeOutEvent(4.5f);
        yield return new WaitForSeconds(4.5f);
        talkControl.TalkSet("これからよろしくお願いします。[%p]。", 1, 1, 1, true);
        Destroy(gameObject);
    }

    void StartTalk() {
        string[] m_talkstring =
        {
            "お帰りなさい[%p]。またよろしくお願いします。",
            "お帰りなさい[%p]。またよろしくお願いします。",
            "お帰りなさい[%p]。またよろしくお願いします。",
            "お疲れ様です[%p]。",
            "お疲れ様です[%p]。",
            "お疲れ様です[%p]。",
            "お待ちしておりました。こちらの準備はできていますよ。",
            "お待ちしておりました。こちらの準備はできていますよ。",
            "お待ちしておりました。こちらの準備はできていますよ。",
            "うぇるかむほーむ！[%p]！",
            "てってれー"
        };
        int m_rand = Random.Range(0, m_talkstring.Length + 1);
        talkControl.TalkSet(m_talkstring[m_rand], 1, 1, 1, false);
    }
}
