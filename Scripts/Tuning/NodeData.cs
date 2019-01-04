using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyScriptableObject/NodeData")]
public class NodeData : ScriptableObject {

    [System.Serializable]
    public class NodeList
    {
        public string nodeName;
        public string nodeDetails;
        public string GetNodeName
        {
            get
            {
                return nodeName;
            }
        }
        public string GetNodeDetails
        {
            get
            {
                return nodeDetails;
            }
        }
    }
    public enum Mode
    {
        Status,
        Straight,
        Flick,
        Snipe
    }
    public Mode mode;
    public NodeList[] nodelist;
}
