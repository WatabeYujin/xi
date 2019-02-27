using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAction : ObjectPool
{
    [SerializeField]
    private EnemyActionPattern enemyActionPattern;
    [SerializeField]
    private GameObject shotBullet;
    [SerializeField]
    private float shotRate;
    [SerializeField]
    private Transform shotTransform;
    [SerializeField]
    private float findRange = 50;
    [SerializeField]
    private float speed = 10;
    [SerializeField]
    private float angularSpeed = 90;

    [SerializeField]
    private Rigidbody thisRigidbody;
    [SerializeField]
    private bool isFind = false;
    private float lastShotTime;
    private int nowPatrolPoint = 0;
    [SerializeField]
    private PlayerController playerController;
    private Vector3 spawnPos;

    enum EnemyActionPattern
    {
        Stop,
        Rotate,
        Chase,
        Satellite
    }

    void OnEnable() {
        if(playerController==null)
            playerController = PlayerController.playerController;
    }

    void Update () {
        if (!playerController.GetSetisActive)
            return;
        ShotCheck();
        isFind = FindCheck();
        if (isFind)
            AIPattern();
        else
            ReturnSpawnPosition();


    }

    void ReturnSpawnPosition()
    {
        Chase(spawnPos);
    }

    void ShotCheck() {
        if (!AttackRateCheck()) return;
        if (!shotBullet) return;
        BulletShot(shotBullet, shotTransform);
    }

    Transform BulletShot(GameObject shotBullet, Transform shotTransform)
    {
        Transform m_bullet = Objectspawn(shotBullet, shotTransform.position, shotTransform.rotation).transform;
        return m_bullet;
    }

    bool AttackRateCheck() {
        if (Time.time - lastShotTime < shotRate) return false;
        lastShotTime = Time.time;
        return true;
    }

    void AIPattern() {
        switch (enemyActionPattern)
        {
            case EnemyActionPattern.Stop:

                break;
            case EnemyActionPattern.Rotate:
                Rotate(playerController.transform.position);
                break;
            case EnemyActionPattern.Chase:
                Chase(playerController.transform.position);
                break;
            case EnemyActionPattern.Satellite:
                Satellite(playerController.transform.position);
                break;
            default:
                break;
        }
    }

    void Rotate(Vector3 targetPos) {
        const float m_rotateAdjustment = 360f;
        Vector3 m_lookPos = new Vector3(targetPos.x,transform.position.y, targetPos.z) - transform.position;
        Quaternion m_quaternion = Quaternion.LookRotation(m_lookPos, Vector3.up);
        m_quaternion = Quaternion.Lerp(transform.rotation, m_quaternion, angularSpeed/ m_rotateAdjustment);
        transform.rotation = m_quaternion;
    }

    void Chase(Vector3 targetPos) {
        Rotate(targetPos);
        thisRigidbody.velocity = transform.forward * speed;
    }

    /// <summary>
    /// プレイヤーを中心に軸回転するAI
    /// 一定距離より近ければ離れる
    /// </summary>
    void Satellite(Vector3 targetPos) {
        Rotate(targetPos);
        Vector3 m_moveVelocity = new Vector3(0,0,0);
        if((transform.position - playerController.transform.position).magnitude > findRange/2 )
            m_moveVelocity += transform.forward * -1;
        m_moveVelocity += transform.right * 5;
        thisRigidbody.velocity = m_moveVelocity * speed;
    }
    
    //発見判定
    bool FindCheck() {
        if ((transform.position - playerController.transform.position).magnitude > findRange)
            return false;
        Ray m_ray = new Ray(transform.position, playerController.transform.position-transform.position);
        RaycastHit hit;
        int m_layermask = 1 << gameObject.layer;
        if (!Physics.Raycast(m_ray, out hit, findRange, ~m_layermask))
            return false;
        if (hit.transform != playerController.transform)
            return false;
        return true;
    }

    public void SetSpaenPos(Vector3 pos)
    {
        spawnPos = pos;
    }
}
