using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wisp : MonoBehaviour {

    [SerializeField]
    private int damage;
    [SerializeField]
    private float findRange = 50;
    [SerializeField]
    private float speed = 5;
    [SerializeField]
    private Life life;
    [SerializeField]
    private ParticleSystem particle;
    [SerializeField]
    private Rigidbody thisRigidbody;
    [SerializeField]
    private SphereCollider sphereCollider;

    private float lastAttackTime;
    private Transform playerTransform;
    private Vector3 firstPos;
    private float firstRadius;
    private ParticleSystem.MainModule particleMain;

    void Start()
    {
        firstPos = transform.position;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        firstRadius = sphereCollider.radius;
        particleMain = particle.main;
        particleMain.maxParticles = life.GetLife()*2;
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        float m_speed = speed / (float)life.GetLife();
        transform.LookAt(Look());
        thisRigidbody.velocity= transform.forward * m_speed;
    }

    Vector3 Look()
    {
        if ((transform.position - playerTransform.position).magnitude < findRange)
            return playerTransform.position;
        else
            return firstPos;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Bullet") Damage();
    }

    void OnTriggerStay(Collider col)
    {
        if (col.tag == "Player") Attack(col);
    }

    void Damage()
    {
        particleMain.maxParticles-=2;
        SizeChange();
    }

    void SizeChange()
    {
        const float minRadiusSize = 0.4f;
        float m_size = (float)life.GetLife() / (float)life.GetMaxLife();
        if (m_size < minRadiusSize) m_size = minRadiusSize;
        
        sphereCollider.radius = firstRadius * m_size;
    }
    
    void Attack(Collider col) {
        float m_attackInterval = 0.1f;

        if (Time.time - lastAttackTime < m_attackInterval) return;
        lastAttackTime = Time.time;
        col.GetComponent<Life>().Damage(damage);
    }
}