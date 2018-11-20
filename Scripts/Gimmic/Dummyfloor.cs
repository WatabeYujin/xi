using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Dummyfloor : MonoBehaviour {
    [SerializeField]
    private float stelthRange = 40;

    private Transform playerTransform;
    private bool isStelth = false;
    private Vector3 firstScale;

	void Start () {
        playerTransform = GameObject.Find("Player").transform;
        firstScale=transform.localScale;
	}
	
	void Update () {
        float m_range = (transform.position - playerTransform.position).magnitude;
        if (stelthRange >= m_range) StelthOn();
        else StelthOff();
	}

    void StelthOn()
    {
        if (isStelth) return;
        isStelth = true;
        transform.DORotate(new Vector3(0, 360), 1, RotateMode.FastBeyond360);
        transform.DOScale(Vector3.zero, 1);
    }

    void StelthOff()
    {
        if (!isStelth) return;
        isStelth = false;
        transform.DORotate(new Vector3(0, 0), 1, RotateMode.FastBeyond360);
        transform.DOScale(firstScale, 1);
    }
}
