using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using FantomLib;

public class TalkControl : MonoBehaviour {
    [SerializeField]
    private AudioMixer mixer;
    [SerializeField]
    private AudioSource callSE;

    private IEnumerator talkIEnumerator;
    private SavaScriptableObject2 savaScriptableObject;

    void Awale() {
        savaScriptableObject = Resources.Load("SaveData")as SavaScriptableObject2;
    }

    public void TalkSet(string talkText,float talkSpeed,float talkPich,float talkvalue,bool isShowToast)
    {
#if UNITY_ANDROID
        talkText=talkStringCheck(talkText);
        callSE.Stop();
        if(talkIEnumerator!= null) StopCoroutine(talkIEnumerator);
        talkIEnumerator = null;
        talkIEnumerator = TalkEvent(talkText,talkSpeed,talkPich,talkvalue,isShowToast);
        StartCoroutine(talkIEnumerator);
#endif
    }

    string talkStringCheck(string checkText)
    {
        const string m_playerCode = "[%p]";

        string m_playerName = savaScriptableObject.playerName;
        if (!checkText.Contains(m_playerCode)) return checkText;
        return checkText.Replace(m_playerCode, m_playerName);
    }

    IEnumerator TalkEvent(string talkText, float talkSpeed, float talkPich, float talkvalue, bool isShowToast)
    {
        callSE.Play();
        yield return new WaitForSeconds(0.1f);
#if UNITY_ANDROID
        AndroidPlugin.AddTextToSpeechSpeed(talkSpeed);
        AndroidPlugin.AddTextToSpeechPitch(talkPich-1);
        float m_voiceVolue;
        mixer.GetFloat("Voice", out m_voiceVolue);
        AndroidPlugin.AddTextToSpeechPitch(m_voiceVolue * talkvalue);
        AndroidPlugin.StartTextToSpeech(talkText, gameObject.name, "OnStatus", "OnStart", "OnDone", "OnStop");
        if (isShowToast)
            AndroidPlugin.ShowToast(talkText);
#endif
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            //AndroidPlugin.StopTextToSpeech();
        }
    }

    private void OnApplicationQuit()
    {
        //AndroidPlugin.StopTextToSpeech();
    }
}
