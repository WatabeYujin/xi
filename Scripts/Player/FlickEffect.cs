using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickEffect : ObjectPool
{
    [SerializeField]
    GameObject trailprefub;
	
    public void EffectSpawn()
    {
        GameObject m_obj = Objectspawn(trailprefub, transform.position,new Quaternion(0, 0, 0, 0));
        m_obj.GetComponent<TrailRenderer>().Clear();
        m_obj.transform.parent = transform.transform;
        StartCoroutine(ChangeActive(m_obj));
    }

    IEnumerator ChangeActive(GameObject obj)
    {
        const float m_waitTime=1;
        yield return new WaitForSeconds(0.1f);
        obj.transform.parent = null;
        yield return new WaitForSeconds(m_waitTime);
        obj.SetActive(false);
    }
}
