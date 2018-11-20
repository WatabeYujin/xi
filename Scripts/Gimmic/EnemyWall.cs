using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWall : MonoBehaviour {
    [SerializeField]
    private Transform[] targetEnemy;
	
	void Update () {
        if (!EnemyCheck()) return;
        Destroy(gameObject);
	}

    bool EnemyCheck() {
        foreach(Transform enemy in targetEnemy) {
            if (enemy != null) return false;
        }
        return true;
    }
}
