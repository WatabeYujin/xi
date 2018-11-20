using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RimitTime : MonoBehaviour {
    [SerializeField]
    private Text text;
    [SerializeField]
    private int timeRimit = 100;
    
    private float time = 0;
    
    private bool isTimeRunning = true;
    private bool isGameOver = false;
    private bool half=false;
    
    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        if (isGameOver) return;
        if (!isTimeRunning) return;
        time += Time.deltaTime;
        PrintTime(timeRimit - time);
        if (time >= timeRimit)
        {
            isGameOver = true;
        }
        if (!half && timeRimit / 2 <= time)
        {
            text.color = Color.yellow;
            half = true;
        }
        else if (timeRimit - timeRimit / 4 <= time) text.color = Color.red;
    }

    public float timecheck
    {
        get
        {
            return time;
        }
    }
    private void PrintTime(float printtime)
    {
        int[] time_ms = new int[3];
        string[] time_string = new string[3];
        time_ms[0] = (int)printtime / 60;
        time_ms[1] = (int)printtime % 60;
        time_ms[2] = (int)(printtime*100 % 100);
        for (int i = 0; i < 3; i++)
        {
            if (time_ms[i] < 10) time_string[i] = "0" + time_ms[i].ToString();
            else time_string[i] = time_ms[i].ToString();
        }
        text.text = time_string[0] + "：" + time_string[1] + "：" + time_string[2];
    }

    public void TimeSecrease() {
        time += 10;
    }

    public void IsTime(bool isTime) {   
        isTimeRunning = isTime;
    }

    public void TimeCountUp(int count)
    {
        int m_countUpTime = 0;
        m_countUpTime += count;
        StartCoroutine(TimeCountUpEvent(m_countUpTime));
    }

    IEnumerator TimeCountUpEvent(int upCount) {
        const int m_time = 20;
        for(int i = 0;i< m_time; i++) {
            time -= upCount / m_time;
            PrintTime(timeRimit - time);
            yield return null;
        }
    }
}
