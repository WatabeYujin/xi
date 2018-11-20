using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideWeponBullet : MonoBehaviour {
    [SerializeField]
    private Rigidbody thisRigidbody;

    private Transform targetEnemy;
    [SerializeField]
    private TrailRenderer trailRenderer;            //弾のTrailRenderer
    [SerializeField]
    private int damage = 5;                         //弾の攻撃力
    [SerializeField]
    private float bulletSpeed = 100;                //弾速
    [SerializeField]
    private GameObject hitEffect;                   //命中時のエフェクト

    private bool isDestroy = false;
    private Collider hitCollider;
    private const string enemyTagName = "Enemy";    //敵のtag名
    private const string playerTagName = "Player";  //プレイヤーのtag名  

    private float startTime;

    void Start() {
        const float m_startBulletHeight = 100;
        thisRigidbody.AddForce(transform.up * m_startBulletHeight, ForceMode.Impulse);
        startTime = Time.time;
    }

    void Update () {
        EnemyTrack();
    }

    void EnemyTrack() {
        const float m_startTime = 0.6f;
        if (Time.time - startTime < m_startTime) return;
        transform.LookAt(targetEnemy);
        thisRigidbody.velocity=transform.forward * bulletSpeed;
    }

    public void SetTargetTransform(Transform target) {
        targetEnemy = target;
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
        if (hitCollider.tag == playerTagName) return;
        if (hitCollider.tag == this.tag) return;
        if (hitCollider.tag == enemyTagName) HitEvent();
        EffectSpawn();
        BulletDelete();
    }

    /// <summary>
    /// 命中時の処理
    /// </summary>
    void HitEvent() {
        hitCollider.GetComponent<Life>().Damage(damage);
    }
}
