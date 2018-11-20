using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySwitch : MonoBehaviour {
    
    private Life playerLife;

    [SerializeField]
    private Transform[] spawnPosition;
    [SerializeField]
    private GameObject[] enemy;

    private List<Transform> spawnEnemy = new List<Transform>();

    private bool isTrigger = false;

    void Start() {
        playerLife = GameObject.Find("Player").GetComponent<Life>();
    }

    void Update()
    {
        if (isTrigger&&playerLife.GetLife() == 0)
        {
            isTrigger = false;
            EnemyDead();
        }
    }

    void OnTriggerEnter(Collider col) {
        if (isTrigger) return;
        if (col.tag != "Player") return;
        Spawn();
    }

    void Spawn() {
        isTrigger = true;
        for (int i = 0;i<enemy.Length;i++) {
            GameObject m_obj = Instantiate(enemy[i]);
            m_obj.transform.position = spawnPosition[i].position;
            spawnEnemy.Add(m_obj.transform);
        }
    }

    void EnemyDead() {
        foreach (Transform m_spawnEnemy in spawnEnemy)
            Destroy(m_spawnEnemy.gameObject);
        spawnEnemy.Clear();
        isTrigger = false;
    }
}
