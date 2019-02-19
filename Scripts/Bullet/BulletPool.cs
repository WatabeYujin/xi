using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour {
    private GameObject bulletPrefub;
    Vector3 spawnpos;
    Quaternion spawnQua;

    private List<GameObject> bullets = new List<GameObject>();

	public GameObject Objectspawn(GameObject spawnprefub,Vector3 pos,Quaternion qua)
    {
        spawnpos = pos;
        spawnQua = qua;
        bulletPrefub = spawnprefub;
        if (bullets.Count == 0)
        {
            NewIns();
            return bullets[0];
        }
        for (int i = 0; i < bullets.Count; i++)
        {
            if (bullets[i].activeSelf == false)
            {
                ResetSpawnBullet(bullets[i]);
                return bullets[i];
            }
        }
        NewIns();
        return bullets[bullets.Count-1];
    }

    //リストに不足している為新たに弾丸を生成
    private void NewIns()
    {
        GameObject m_obj = Instantiate(bulletPrefub);
        m_obj.transform.position = spawnpos;
        m_obj.transform.rotation = spawnQua;
        bullets.Add(m_obj);
        
    }

    //リスト内の弾丸を再利用
    private void ResetSpawnBullet(GameObject bullet)
    {
        bullet.transform.position = spawnpos;
        bullet.transform.rotation = spawnQua;
        bullet.SetActive(true);
    }
}
