using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSpawn : MonoBehaviour {
    [SerializeField]
    private GameObject[] a = new GameObject[400];

    private GameObject[,] floor = new GameObject[20,20];
    [SerializeField]
    private List<GameObject> unit;

    private Vector2 stageSpace = new Vector2(10,10);
    private Vector3 stageBase = new Vector3(-95, 0.5f, -95);
    const int floorScale = 20;
    void Awake()
    {
        StageSet();
    }

    void StageSet()
    {
        return;
        for(int i = 0; i < floorScale; i++)
        {
            for (int j = 0; j < floorScale; j++){
                //Transform m_obj = Instantiate(floor[i, j]).transform;
                Transform m_obj = Instantiate(floor[i, j]).transform;
                m_obj.parent = transform;
                m_obj.position = 
                    new Vector3(stageBase.x + stageSpace.x * i, stageBase.y, stageBase.z + stageSpace.y * j);
            }
        }
    }
}
