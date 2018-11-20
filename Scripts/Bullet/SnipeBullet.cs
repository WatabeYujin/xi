using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnipeBullet : Status
{
    [SerializeField]
    private Rigidbody thisRigidbody;                //弾のRigidbody
    [SerializeField]
    private TrailRenderer trailRenderer;            //弾のTrailRenderer
    [SerializeField]
    private string targetTagName = "Enemy";         //敵のtag名
    [SerializeField]
    private string shooterTagName = "Player";       //プレイヤーのtag名 
    [SerializeField]
    private GameObject hitEffect;                   //命中時のエフェクト
    
    private Vector3 bulletSpawnPosition;            //弾の発射地点
    private bool isDestroy = false;                 //弾が消失するモードに入っているか
    private Collider hitCollider;                   //命中した対象のCollider

    void Start()
    {

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
        const float m_baseBulletSpeed = 1000;
        thisRigidbody.AddForce(
            transform.forward * snipeCanonStatus.bulletSpeed * m_baseBulletSpeed
        );
    }

    void Update()
    {
        if (BulletRangeCheck())
            BulletDelete();
    }

    /// <summary>
    /// 射程チェック
    /// </summary>
    /// <returns>射程内ならtrue 射程外ならfalseを返す</returns>
    bool BulletRangeCheck()
    {
        if (snipeCanonStatus == null)
            return false;
        if ((transform.position - bulletSpawnPosition).magnitude >= snipeCanonStatus.bulletRange)
            return true;
        else return false;
    }

    /// <summary>
    /// 弾の消去処理
    /// </summary>
    void BulletDelete()
    {
        isDestroy = true;
        thisRigidbody.velocity = Vector3.zero;
        float m_deleteTime = trailRenderer.time;
        Destroy(gameObject, m_deleteTime);
    }


    void EffectSpawn()
    {
        const float m_deleteTime = 2;
        Transform m_hitEffect = Instantiate(hitEffect).transform;
        m_hitEffect.position = transform.position;
        m_hitEffect.rotation = transform.rotation;
        Destroy(m_hitEffect.gameObject, m_deleteTime);
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
        if (hitCollider.tag == shooterTagName) return;
        if (hitCollider.tag == this.tag) Destroy(hitCollider.gameObject);
        if (hitCollider.tag == targetTagName) {
            HitEvent();
            return;
        }
        EffectSpawn();
        BulletDelete();
    }

    /// <summary>
    /// 命中時の処理
    /// </summary>
    void HitEvent() {
        hitCollider.GetComponent<Life>().Damage(snipeCanonStatus.damage);
    }

    public void SetStatus(SnipeCanonStatus value)
    {
        snipeCanonStatus = value;
        if (value == null)
            Destroy(gameObject);
        SpawnPositionSet();
        StartBulletMove();
    }
}
