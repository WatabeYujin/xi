using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAction : MonoBehaviour {
    [SerializeField]
    private NavMeshAgent navMeshAgent;
    [SerializeField]
    private EnemyActionPattern enemyActionPattern;
    [SerializeField]
    private Transform[] patrolPoint;
    [SerializeField]
    private GameObject shotBullet;
    [SerializeField]
    private float shotRate;
    [SerializeField]
    private Transform shotTransform;
    [SerializeField]
    private float findRange = 50;

    [SerializeField]
    private Rigidbody thisRigidbody;

    private bool isFind = false;
    private float lastShotTime;
    private int nowPatrolPoint = 0;
    private Transform playerTransform;

    enum EnemyActionPattern
    {
        Stop,
        Rotate,
        Chase,
        Satellite
    }

    void Start() {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        if (patrolPoint.Length == 0) return;
        navMeshAgent.SetDestination(patrolPoint[0].position);
    }

    void Update () {
        ShotCheck();
        AIPattern();
        Find(FindCheck());
	}

    void ShotCheck() {
        if (!AttackRateCheck()) return;
        if (!shotBullet) return;
        BulletShot(shotBullet, shotTransform);
    }

    Transform BulletShot(GameObject shotBullet, Transform shotTransform)
    {
        Transform m_bullet = Instantiate(shotBullet).transform;
        m_bullet.position = shotTransform.position;
        m_bullet.forward = shotTransform.forward;
        return m_bullet;
    }

    bool AttackRateCheck() {
        if (Time.time - lastShotTime < shotRate) return false;
        lastShotTime = Time.time;
        return true;
    }

    void AIPattern() {
        if (!isFind) {
            Patrol();
            return;
        }
        switch (enemyActionPattern)
        {
            case EnemyActionPattern.Stop:
                Stop();
                break;
            case EnemyActionPattern.Rotate:
                Rotate();
                break;
            case EnemyActionPattern.Chase:
                Chase();
                break;
            case EnemyActionPattern.Satellite:
                Satellite();
                break;
            default:
                break;
        }
    }

    void Patrol() {
        if (patrolPoint.Length == 0) return;
        if (navMeshAgent.isStopped)
            navMeshAgent.isStopped = false;
        Vector3 m_pos = patrolPoint[nowPatrolPoint].position;
        if (Vector3.Distance(transform.position, m_pos) > navMeshAgent.stoppingDistance) return;
        nowPatrolPoint++;
        if (nowPatrolPoint >= patrolPoint.Length)
            nowPatrolPoint = 0;
        navMeshAgent.SetDestination(patrolPoint[nowPatrolPoint].position);
    }

    void Stop() {
        if (!navMeshAgent.isStopped)
            navMeshAgent.isStopped = true;
    }

    void Rotate() {
        if (!navMeshAgent.isStopped)
            navMeshAgent.isStopped = true;
        const float m_rotateAdjustment = 3600;
        Vector3 newRotation = playerTransform.position - transform.position;
        
        transform.rotation = Quaternion.Slerp(transform.rotation, 
            Quaternion.LookRotation(new Vector3(newRotation.x, 0, newRotation.z)), navMeshAgent.angularSpeed / m_rotateAdjustment);
    }

    void Chase() {
        if(navMeshAgent.isStopped)
            navMeshAgent.isStopped = false;
        navMeshAgent.SetDestination(playerTransform.position);
    }

    /// <summary>
    /// プレイヤーを中心に軸回転するAI
    /// 一定距離より近ければ離れる
    /// </summary>
    void Satellite() {
        Rotate();
        Vector3 m_moveVelocity = new Vector3(0,0,0);
        if((transform.position - playerTransform.position).magnitude > findRange )
            m_moveVelocity += transform.forward * -1;
        m_moveVelocity += transform.right * 5;
        thisRigidbody.velocity = m_moveVelocity * navMeshAgent.speed;
    }

    public void Find(bool isfind) {
        isFind = isfind;
    }

    bool FindCheck() {
        if ((transform.position - playerTransform.position).magnitude > findRange) return false;
        return true;
    }
}
