using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lavel : MonoBehaviour {
    
    [SerializeField]
    private GameObject node;
    [SerializeField]
    private Transform nodeViewContent;
    [SerializeField]
    NodeData[] nodeData = new NodeData[5];
    [SerializeField]
    private GameObject buttonList;
    [SerializeField]
    private GameObject nodeList;

    private List<GameObject> spawnNode = new List<GameObject>();
      
    public void NodeSpawn(int data)
    {
        Vector2 m_pos = new Vector2(0, 125f);
        const float m_nodeSpace = 45f;
        foreach(GameObject deleteNode in spawnNode)
        {
            Destroy(deleteNode);
        }
        spawnNode.Clear();
        for(int i = 0;i< nodeData[data].nodelist.Length; i++) {
            //if()
            GameObject m_nodeObj = Instantiate(node, nodeViewContent);
            m_nodeObj.GetComponent<RectTransform>().anchoredPosition =
                new Vector2(m_pos.x,m_pos.y - (i * m_nodeSpace));
            //m_nodeObj.transform.GetComponent<LavelUpButton>().
            //    NodeDataSet(nodeData[data],i);
            
            spawnNode.Add(m_nodeObj);
        }
    }

    public void ListOpen(bool isButtonListOpen)
    {
        buttonList.SetActive(isButtonListOpen);
        nodeList.SetActive(!isButtonListOpen);
    }
}
