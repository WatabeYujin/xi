using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickWeponBullet : ObjectPool
{
    [SerializeField]
    private Rigidbody thisRigidbody;                //弾のRigidbody
    [SerializeField]
    private TrailRenderer trailRenderer;            //弾のTrailRenderer
    [SerializeField]
    private Collider thisCollider;
    [SerializeField]
    private string targetTagName = "Enemy";         //敵のtag名
    [SerializeField]
    private string shooterTagName = "Player";       //プレイヤーのtag名  
    [SerializeField]
    private GameObject hitEffect;                   //命中時のエフェクト
    [SerializeField]
    private PhysicMaterial bounceMat;
    

    private Status.StraightWeponStatus straightWeponStatus;
    private Vector3 bulletSpawnPosition;            //弾の発射地点
    private bool isDestroy = false;                 //弾が消失するモードに入っているか
    private Transform hitTransform;                  //命中した対象のTransform
    private const float baseBulletSpeed = 0.1f;
    private SaveScriptableObject2 save;
    private int bounceCount = 0;
    private int throughCount = 0;
    private float damageBoost = 1f;

    private void OnEnable()
    {
        isDestroy = false;
        trailRenderer.Clear();
    }

    /// <summary>
    /// 弾の発射地点を取得
    /// </summary>
    void SpawnPositionSet() {
        bulletSpawnPosition = transform.position;
    }

    /// <summary>
    /// 弾の移動を開始させる処理
    /// </summary>
    void StartBulletMove() {
        bounceCount = save.straightNode[0].GetLevel;
        throughCount = save.straightNode[1].GetLevel;
        BulletSize();
        if (throughCount >= 1)
            thisCollider.isTrigger = true;
        else if (bounceCount >= 1)
            thisCollider.material = bounceMat;
        if (thisRigidbody == null) return;
        thisRigidbody.isKinematic = false;
        StraightAddForce();
        thisRigidbody.AddForce(transform.right * Recoil() * baseBulletSpeed / 2);
    }

    /// <summary>
    /// 弾の反動計算
    /// </summary>
    /// <returns>ブレの大きさ（float）を返す</returns>
    float Recoil() {
        
        const float m_baseDiminution = 2f;
        if (straightWeponStatus.recoil == 0) return 0;
        float m_recoil = Random.Range(-straightWeponStatus.recoil, straightWeponStatus.recoil);
        m_recoil /= (m_baseDiminution + save.straightNode[10].GetLevel);
        return m_recoil;
    }
    
    void Update() {
        if (straightWeponStatus == null)
            return;
        if (BulletRangeCheck())
            StartCoroutine(BulletDelete());
    }

    /// <summary>
    /// 射程チェック
    /// </summary>
    /// <returns>射程内ならfalse 射程外ならtrueを返す</returns>
    bool BulletRangeCheck()
    {
        const float m_levelMagnification = 5;
        float m_range = 0;
        m_range = straightWeponStatus.bulletRange + (save.straightNode[2].GetLevel* m_levelMagnification);
        if ((transform.position - bulletSpawnPosition).magnitude >= m_range)
            return true;
        else return false;
    }

    /// <summary>
    /// 弾の消去処理
    /// </summary>
    IEnumerator BulletDelete()
    {
        thisRigidbody.isKinematic = true;
        isDestroy = true;
        thisRigidbody.velocity = Vector3.zero;
        float m_deleteTime = 0;
        if (trailRenderer != null)
            m_deleteTime = trailRenderer.time;
        yield return new WaitForSeconds(m_deleteTime);
        gameObject.SetActive(false);
    }

    void BulletSize()
    {
        const float m_baseScale = 0.2f;
        const float m_baseTrailScale = 0.3f;
        float m_scale = m_baseScale * save.straightNode[4].GetLevel;
        transform.localScale = Vector3.one + Vector3.one  * m_scale;
        trailRenderer.startWidth = m_scale / 3 + m_baseTrailScale;
    }

    void EffectSpawn()
    {
        Transform m_hitEffect = Objectspawn(hitEffect, transform.position, transform.rotation).transform;
    }
    
    void OnCollisionEnter(Collision col) {
        hitTransform = col.transform;
        HitEvent();
    }

    void OnTriggerEnter(Collider col)
    {
        hitTransform = col.transform;
        HitEvent();
    }


    void OnTriggerExit(Collider col)
    {
        ThroughEffectRescission();
    }



    void HitEvent()
    {
        if (!isDestroy)
            TargetTagCheck();
        switch (HitActionCheck())
        {
            case 2:
                //貫通のパターン
                ThroughEffect();
                break;
            case 1:
                //跳弾のパターン
                BounceEffect();
                break;
            default:
                //何もないパターン
                StartCoroutine(BulletDelete());
                break;
        }
    }
    
    //直進の加速の際に呼び出す
    void StraightAddForce()
    {
        float m_addBulletSpeed=2f;

        m_addBulletSpeed *= save.straightNode[3].GetLevel;
        thisRigidbody.AddForce(
            transform.forward * (straightWeponStatus.bulletSpeed * baseBulletSpeed + m_addBulletSpeed)
        );
    }

    /// <summary>
    /// 命中時の弾丸の効果を調べる
    /// </summary>
    /// <returns></returns>
    int HitActionCheck()
    {
        int m_value=0;

        //貫通回数の確認
        if (throughCount > 0)
            m_value = 2;
        //跳弾回数の確認
        else if (bounceCount > 0)
            m_value = 1;
        return m_value;
    }
    
    //貫通効果付加時の処理
    void ThroughEffect() {
        damageBoost += 0.2f;
        throughCount--;
    }

    //貫通効果解除の際の処理
    void ThroughEffectRescission()
    {
        if (!thisCollider.isTrigger)
            return;
        if (throughCount > 0)
            return;

        thisCollider.isTrigger = false;
        //もし跳弾が可能だった場合、マテリアルを適用する。
        if (bounceCount > 0)
            thisCollider.material = bounceMat;
    }

    //跳弾効果付加時の処理
    void BounceEffect() {
        bounceCount--;
        //跳弾回数がなくなった場合、マテリアルを外す
        if (bounceCount <= 0)
            thisCollider.material = null;
    }

    /// <summary>
    /// 命中相手のtagが条件に当てはまるか確かめる
    /// </summary>
    void TargetTagCheck() {
        if (hitTransform.tag == "Event") return;
        if (hitTransform.tag == shooterTagName) return;
        if (hitTransform.tag == this.tag) return;
        if (hitTransform.tag == targetTagName) DamgeEvent();
        EffectSpawn();
    }

    /// <summary>
    /// 命中時の処理
    /// </summary>
    void DamgeEvent() {
        float m_lastDamage = straightWeponStatus.GetDamage() * damageBoost;
        hitTransform.GetComponent<Life>().Damage((int)m_lastDamage);
    }

    public void SetStatus(Status.StraightWeponStatus value)
    {
        if (save == null)
            save = (SaveScriptableObject2)Resources.Load("SaveData");
        straightWeponStatus = value;
        SpawnPositionSet();
        StartBulletMove();
    }
}
