using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class SaveData : MonoBehaviour {
    private string filePath ;
    private SavaScriptableObject2 savaScriptableObject;
    private string savedataPath = "SaveData";

    void Start()
    {
        savaScriptableObject = Resources.Load(savedataPath) as SavaScriptableObject2;
        filePath = UnityEngine.Application.persistentDataPath + "Assets\\Resources\\SaveDataJson.json";
    }

    public bool Save()
    {
        string m_json = JsonUtility.ToJson(savaScriptableObject);
        File.WriteAllText(filePath, m_json);
        if (!File.Exists(filePath)) return false;
        else return true;
    }

    
    public bool Load()
    {
        if (!File.Exists(filePath)) return false;
        string m_json = File.ReadAllText(filePath);
        JsonUtility.FromJsonOverwrite(m_json, savaScriptableObject);
        return true;
    }


    public void DataInitialization()
    {
        SavaScriptableObject2 m_initialization = Resources.Load(savedataPath) as SavaScriptableObject2;
        string m_json = JsonUtility.ToJson(savaScriptableObject);
        File.WriteAllText(filePath, m_json);
    }
}
