using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LavelUpButton : MonoBehaviour {
    
    private int nodeID;

    private string nodePointName;

    [SerializeField]
    private NodeData nodeData;

    [SerializeField]
    private Text nodeText;
    SaveScriptableObject2 saveData;

    public void LevelUp() {
        if (!NodePointCheck(true)) return;
        
        LevelChange(true);
    }

    public void LevelDown() {
        if (!NodePointCheck(false)) return;
        LevelChange(false);
    }

    /// <summary>
    /// レベル変更処理を行う
    /// </summary>
    /// <param name="isLevelUP">上昇させる場合true</param>
    void LevelChange(bool isLevelUP)
    {
        switch (nodeData.mode)
        {
            case NodeData.Mode.Status:
                if (isLevelUP)
                {
                    saveData.statusNode[nodeID].LevelUp();
                    saveData.statusNodePoint--;
                }
                else
                {
                    saveData.statusNode[nodeID].LevelDown();
                    saveData.statusNodePoint++;
                }
                break;
            case NodeData.Mode.Straight:
                if (isLevelUP)
                {
                    saveData.straightNode[nodeID].LevelUp();
                    saveData.straightNodePoint--;
                }
                else
                {
                    saveData.straightNode[nodeID].LevelDown();
                    saveData.straightNodePoint++;
                }
                break;
            case NodeData.Mode.Flick:
                if (isLevelUP)
                {
                    saveData.flickDodgeNode[nodeID].LevelUp();
                    saveData.flickDodgeNodePoint--;
                }
                else
                {
                    saveData.flickDodgeNode[nodeID].LevelDown();
                    saveData.flickDodgeNodePoint++;
                }
                break;
            case NodeData.Mode.Snipe:
                if (isLevelUP)
                {
                    saveData.snipeCannonNode[nodeID].LevelUp();
                    saveData.snipeCannonNodePoint--;
                }
                else
                {
                    saveData.snipeCannonNode[nodeID].LevelDown();
                    saveData.snipeCannonNodePoint++;
                }
                break;
        }
        TextSet();
        saveData.isChanged=true;
    }

    public void NodeDataSet(NodeData m_nodeData,int m_nodeID, SaveScriptableObject2 m_savedata) {

        saveData = m_savedata;
        nodeData = m_nodeData;
        nodeID = m_nodeID;
        nodePointName = "NodeLevel" + nodeData.mode.ToString();
        TextSet();

    }

    void TextSet() {
        string m_level="";
        switch (nodeData.mode)
        {
            case NodeData.Mode.Status:
                m_level = saveData.statusNode[nodeID].GetLevel.ToString();
                break;
            case NodeData.Mode.Straight:
                m_level = saveData.straightNode[nodeID].GetLevel.ToString();
                break;
            case NodeData.Mode.Flick:
                m_level = saveData.flickDodgeNode[nodeID].GetLevel.ToString();
                break;
            case NodeData.Mode.Snipe:
                m_level = saveData.snipeCannonNode[nodeID].GetLevel.ToString();
                break;
        }

        nodeText.text = nodeData.nodelist[nodeID].nodeName.ToString() + "　Lv：" + m_level;
    }
    

    /// <summary>
    /// ノードポイントが割り振り可能か調べる
    /// </summary>
    /// <param name="isUpper">レベルを上昇させる場合trueに</param>
    /// <returns>割り振り可能であればtrueを返す</returns>
    bool NodePointCheck(bool isUpper)
    {
        switch (nodeData.mode)
        {
            case NodeData.Mode.Status:
                if (isUpper) {
                    if (saveData.statusNodePoint <= 0) return false;
                    return true;
                }
                else
                {
                    if (saveData.statusNode[nodeID].GetLevel <= 0) return false;
                    return true;
                }
            case NodeData.Mode.Straight:
                if (isUpper)
                {
                    if (saveData.straightNodePoint < 0) return false;
                    return true;
                }else
                {
                    if (saveData.straightNode[nodeID].GetLevel <= 0) return false;
                    return true;
                }
            case NodeData.Mode.Flick:
                if (isUpper)
                {
                    if (saveData.flickDodgeNodePoint < 0) return false;
                    return true;
                }
                else
                {
                    if (saveData.flickDodgeNode[nodeID].GetLevel <= 0) return false;
                    return true;
                }
            case NodeData.Mode.Snipe:
                if (isUpper)
                {
                    if (saveData.snipeCannonNodePoint < 0) return false;
                    return true;
                }
                else
                {
                    if (saveData.snipeCannonNode[nodeID].GetLevel <= 0) return false;
                    return true;
                }
            default:
                return false;
        }
    }
}
