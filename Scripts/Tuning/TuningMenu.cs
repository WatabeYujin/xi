using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TuningMenu : MonoBehaviour {
    
    [SerializeField]
    private GameObject node;
    [SerializeField]
    private Transform nodeViewContent;
    [SerializeField]
    private NodeData[] nodeData = new NodeData[4];
    [SerializeField]
    private SaveScriptableObject2 saveScriptableObject2;
    [SerializeField]
    private GameObject buttonList;
    [SerializeField]
    private GameObject nodeList;
    [SerializeField]
    private Text nodePointText;

    private List<GameObject> spawnNode = new List<GameObject>();

      
    public void NodeSpawn(int data)
    {
        foreach(GameObject deleteNode in spawnNode)
        {
            Destroy(deleteNode);
        }
        spawnNode.Clear();
        for(int i = 0;i< nodeData[data].nodelist.Length; i++) {
            if (!isNodePossession(data, i)) continue;

            GameObject m_nodeObj = Instantiate(node, nodeViewContent);
            m_nodeObj.transform.GetComponent<LavelUpButton>().
                NodeDataSet(nodeData[data],i,saveScriptableObject2);
            
            spawnNode.Add(m_nodeObj);
        }
    }

    public void ListOpen(bool isButtonListOpen)
    {
        buttonList.SetActive(isButtonListOpen);
        nodeList.SetActive(!isButtonListOpen);
    }

    bool isNodePossession(int nodeType,int nodeID)
    {
        switch (nodeType)
        {
            case 0:
                return saveScriptableObject2.statusNode[nodeID].GetSetPossesion;
            case 1:
                return saveScriptableObject2.straightNode[nodeID].GetSetPossesion;
            case 2:
                return saveScriptableObject2.flickDodgeNode[nodeID].GetSetPossesion;
            case 3:
                return saveScriptableObject2.snipeCannonNode[nodeID].GetSetPossesion;
            default:
                return false;
        }
    }
}
