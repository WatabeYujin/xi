using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSpawn : MonoBehaviour {
    [SerializeField]
    private ParticleSystem ps;
    

    void OnEnable(){
        StartCoroutine(isActivechange());
    }

    IEnumerator isActivechange()
    {
        yield return new WaitForSeconds(ps.main.duration);
        gameObject.SetActive(false);
    }
}
