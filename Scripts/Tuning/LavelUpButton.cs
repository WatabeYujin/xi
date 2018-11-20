using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LavelUpButton : MonoBehaviour {
    enum Mode
    {
        Status,
        Stick,
        Dash,
        Snipe
    }
    [SerializeField]
    private Mode mode;

    [SerializeField]
    private int nodeID;

    private string nodePointName;
    [SerializeField]
    private int nodePoint;
    
    private int nodeLevel;

    private NodeData nodeData;

    [SerializeField]
    private Text nodeText;

    private SavaScriptableObject2 saveData;
    private string savedataPath = "SaveData";

    void Start() {
        saveData = Resources.Load(savedataPath) as SavaScriptableObject2;
    }

    void NodeStatusGet() {
        GetSetData(true);
    }

    public void LevelUp() {
        NodeStatusGet();
        if (nodePoint++ > nodeData.nodelist[nodeID].maxLevel) return;
        if (nodeLevel < 0) return;
    }

    public void LevelDown() {
        NodeStatusGet();
        if (nodePoint-- < nodeData.nodelist[nodeID].minLevel) return;
        LevelChange(false);
    }

    void LevelChange(bool isLevelUP)
    {
        if (isLevelUP) {
            nodePoint--;
            nodeLevel++;
            GetSetData(false);
        }
        else {
            nodePoint++;
            nodeLevel--;
            GetSetData(false);
        }
        Text nodePointText = GameObject.Find("NodePointText").GetComponent<Text>();
        TextSet();
    }

    public void NodeDataSet(NodeData m_nodeData,int m_nodeID) {
        GetSetData(false);

        nodeData = m_nodeData;
        nodeID = m_nodeID;
        nodePointName = "NodeLevel" + mode.ToString();
        NodeStatusGet();
        TextSet();

    }

    void TextSet() {

        nodeText.text = nodeData.nodelist[nodeID].nodeName.ToString() + "　Lv：" + nodePoint.ToString();
    }

    void GetSetData(bool isGet)
    {
        switch (mode)
        {
            case Mode.Status:
                if (isGet)
                {
                    nodeLevel = saveData.statusNode[nodeID].GetLevel;
                    nodePoint = saveData.statusNodePoint;
                }
                else
                {
                    saveData.straightNode[nodeID].SetLevel=nodeLevel;
                    saveData.statusNodePoint = nodePoint;
                }
                break;
            case Mode.Stick:
                if (isGet)
                {
                    nodeLevel = saveData.straightNode[nodeID].GetLevel;
                    nodePoint = saveData.straightNodePoint;
                }
                else
                {
                    saveData.straightNode[nodeID].SetLevel = nodeLevel;
                    saveData.straightNodePoint = nodePoint;
                }
                break;
            case Mode.Dash:
                if (isGet)
                {
                    nodeLevel = saveData.flickDodgeNode[nodeID].GetLevel;
                    nodePoint = saveData.flickDodgeNodePoint;
                }
                else
                {
                    saveData.flickDodgeNode[nodeID].SetLevel = nodeLevel;
                    saveData.flickDodgeNodePoint = nodePoint;
                }
                break;
            case Mode.Snipe:
                if (isGet)
                {
                    nodeLevel = saveData.straightNode[nodeID].GetLevel;
                    nodePoint = saveData.snipeCannonNodePoint;
                }
                else
                {
                    saveData.snipeCannonNode[nodeID].SetLevel = nodeLevel;
                    saveData.snipeCannonNodePoint = nodePoint;
                }
                break;
        }
    }
}
