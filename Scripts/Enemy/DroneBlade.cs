using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneBlade : MonoBehaviour {
    [SerializeField]
    private float speed = 1000;

	void Update () {
        transform.localEulerAngles = new Vector3(0, Time.time * speed, 0);
	}
}
