using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : ObjectPool
{
    [SerializeField]
    private Rigidbody thisRigidbody;                //弾のRigidbody
    [SerializeField]
    private TrailRenderer trailRenderer;            //弾のTrailRenderer
    [SerializeField]
    private string targetTagName = "Player";         //敵のtag名
    [SerializeField]
    private string[] noDeleteTags = { "Enemy" };           //判定に含まないtag  
    [SerializeField]
    private GameObject hitEffect;                   //命中時のエフェクト
    [SerializeField]
    private int damage = 5;
    [SerializeField]
    private float bulletspeed=10;
    [SerializeField]
    private float bulletdistance = -1;              //射程(-1の場合距離判定なし)
    private Vector3 bulletSpawnPosition;            //弾の発射地点

    private bool isDestroy = false;                 //弾が消失するモードに入っているか
    private Collider hitCollider;                   //命中した対象のCollider
    private void OnEnable()
    {
        isDestroy = false;
        SpawnPositionSet();
        StartBulletMove();
        if (trailRenderer!=null)
            trailRenderer.Clear();
    }

    /// <summary>
    /// 弾の発射地点を取得
    /// </summary>
    void SpawnPositionSet()
    {
        bulletSpawnPosition = transform.position;
    }

    /// <summary>
    /// 弾の移動を開始させる処理
    /// </summary>
    void StartBulletMove()
    {
        const float m_baseBulletSpeed = 100;

        if (thisRigidbody == null) return;
            thisRigidbody.AddForce(
                transform.forward * bulletspeed* m_baseBulletSpeed
            );
    }
    

    void Update()
    {
        if (BulletRangeCheck())
            StartCoroutine(BulletDelete());
    }

    /// <summary>
    /// 射程チェック
    /// </summary>
    /// <returns>射程内ならfalse 射程外ならtrueを返す</returns>
    bool BulletRangeCheck()
    {
        if (bulletdistance == -1)
            return false;
        if ((transform.position - bulletSpawnPosition).magnitude >= bulletdistance)
            return true;
        else return false;
    }

    /// <summary>
    /// 弾の消去処理
    /// </summary>
    IEnumerator BulletDelete()
    {
        isDestroy = true;
        thisRigidbody.velocity = Vector3.zero;
        float m_deleteTime = 0;
        if (trailRenderer != null)
            m_deleteTime = trailRenderer.time;
        yield return new WaitForSeconds(m_deleteTime);
        gameObject.SetActive(false);
    }

    void EffectSpawn()
    {
        Transform m_hitEffect = Objectspawn(hitEffect, transform.position, transform.rotation).transform;
    }

    void OnTriggerEnter(Collider col)
    {
        hitCollider = col;
        if (!isDestroy)
            TargetTagCheck();
    }

    /// <summary>
    /// 命中相手のtagが条件に当てはまるか確かめる
    /// </summary>
    void TargetTagCheck()
    {
        if (hitCollider.tag == "Event") return;
        foreach(string tags in noDeleteTags)
        {
            if (hitCollider.tag == tags) return;
        }
        if (hitCollider.tag == "Bullet") return;
        if (hitCollider.tag == targetTagName) HitEvent();
        EffectSpawn();
        StartCoroutine(BulletDelete());
    }

    /// <summary>
    /// 命中時の処理
    /// </summary>
    void HitEvent()
    {
        hitCollider.GetComponent<Life>().Damage(damage);
    }
}
