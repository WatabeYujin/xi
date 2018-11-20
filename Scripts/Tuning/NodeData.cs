using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class NodeData : ScriptableObject {

    [System.Serializable]
    public class NodeList
    {
        public string nodeName;
        public int lavel;
        public int minLevel = -100;
        public int maxLevel = 100;  
    }
    public enum Mode
    {
        Status,
        Stick,
        Rockon,
        Dash,
        Snipe
    }
    public Mode mode;
    public NodeList[] nodelist;
}
