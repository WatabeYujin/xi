using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booby : MonoBehaviour {
    [SerializeField]
    private LineRenderer lineRenderer;
    [SerializeField]
    private int damage = 3;
    [SerializeField]
    private float interval = 0.2f;
    private float lastHitTime=0;
    [SerializeField]
    private string playerTagName = "Player";
    private PlayerController playerController;

    private Vector3 leserPosition = new Vector3(0, 1f, 0);

    private void Start()
    {
        playerController = PlayerController.playerController;
    }

    void Update () {
        Laser();
	}

    void Laser()
    {
        RaycastHit hit;
        // 正規化して方向ベクトルを求める
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        if (Physics.Raycast(transform.position, fwd, out hit))
        {
            if (isHit(hit.transform))
            {
                hit.transform.GetComponent<Life>().Damage(damage);
            }
            if (null != lineRenderer)
            {
                lineRenderer.SetPosition(0, transform.position+ leserPosition);
                lineRenderer.SetPosition(1, hit.point);
            }
        }
        else
        {
            if (null != lineRenderer)
            {
                lineRenderer.SetPosition(0, transform.position+ leserPosition);
                lineRenderer.SetPosition(1, transform.position + (transform.forward * 400));
            }
        }
    }

    bool isHit(Transform hitObj)
    {
        if (hitObj.tag != playerTagName)
            return false;
        if (Time.time - lastHitTime < interval)
            return false;
        if (!playerController.GetSetisActive)
            return false;
        lastHitTime = Time.time;
        return true;
    }
}
