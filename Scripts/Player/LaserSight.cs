using UnityEngine;
using System.Collections;

public class LaserSight : MonoBehaviour
{
    [SerializeField]
    LineRenderer lineRenderer;
    const float distance = 50;
    void Update()
    {
        RaycastHit hit;
        // 正規化して方向ベクトルを求める
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        int m_layermask = 1 << 11;
        if (Physics.Raycast(transform.position, fwd, out hit, distance, ~m_layermask))
        {

            if (null != lineRenderer)
            {
                //Instantiate(lineRenderer, hit.point, Quaternion.identity);
                lineRenderer.SetPosition(0, transform.position);
                lineRenderer.SetPosition(1, hit.point);
            }
        }else
        {
            if (null != lineRenderer)
            {
                //Instantiate(lineRenderer, hit.point, Quaternion.identity);
                lineRenderer.SetPosition(0, transform.position);
                lineRenderer.SetPosition(1, transform.position +(transform.forward* distance));
            }
        }
    }
}