using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSpawn : MonoBehaviour {

    private GameObject[,] floor = new GameObject[20,20];
    [SerializeField]
    private Transform[,] spawnObj;

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
                spawnObj[i,j] = Instantiate(floor[i, j]).transform;
                spawnObj[i, j].parent = transform;
                spawnObj[i, j].position = 
                    new Vector3(stageBase.x + stageSpace.x * i, stageBase.y, stageBase.z + stageSpace.y * j);
            }
        }
    }
    public void StageObjUpdate(int x ,int y,int id)
    {
        Destroy(spawnObj[x, y].gameObject);

    }
}
