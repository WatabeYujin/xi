using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField]
    private Transform cameraTransform;

    private Touch[] oldTouches = new Touch[2];

    private void Update()
    {
        ModeChange();
        TouchGet();
    }

    public Touch TapData(int fingerID)
    {
        Touch[] myTouches = Input.touches;

        return myTouches[fingerID];
    }

    void ModeChange()
    {
        if (Input.touchCount == 2)
        {
            //2本指スワイプ

            //ピンチ操作
        }
    }

    void Move()
    {

    }

    void Zoom()
    {

    }
    private Touch[] TouchGet()
    {
        Touch[] touches = Input.touches;
        return touches;
    }
}
