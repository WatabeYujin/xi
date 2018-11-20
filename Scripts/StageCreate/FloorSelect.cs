using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorSelect : MonoBehaviour {
    [SerializeField]
    private GameObject test;
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0)) {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit)){
                Vector3 m_pos = hit.point;
                FloorSet(new Vector2(m_pos.x,m_pos.z));
            }
        }
    }
    
    void FloorSet(Vector2 pos)
    {
        pos = FloorPosCheck(pos);
        if (pos == new Vector2(-1, -1))
            return;
        GameObject obj = Instantiate(test);
        obj.transform.position = new Vector3(pos.x,0,pos.y);
    }

    Vector2 FloorPosCheck(Vector2 hitPos)
    {
        Vector2 m_minLimitPos = new Vector2(0, 0);
        Vector2 m_maxLimitPos = new Vector2(200, 200);
        if ((int)hitPos.x <= m_minLimitPos.x) 
            return new Vector2(-1, -1);
        if ((int)hitPos.y <= m_minLimitPos.y)
            return new Vector2(-1, -1);
        if ((int)hitPos.x >= m_maxLimitPos.x)
            return new Vector2(-1, -1);
        if ((int)hitPos.y >= m_maxLimitPos.y)
            return new Vector2(-1, -1);
        //0～5だったら2.5
        //それ以外だったら7.5
        float m_firstDigitX = (int)hitPos.x % 10;
        float m_firstDigitY = (int)hitPos.y % 10;
        hitPos = new Vector2((int)hitPos.x - m_firstDigitX, (int)hitPos.y - m_firstDigitY);
        if (m_firstDigitX <= 5)
            m_firstDigitX = 2.5f;
        else
            m_firstDigitX = 7.5f;
        if (m_firstDigitY <= 5)
            m_firstDigitY = 2.5f;
        else
            m_firstDigitY = 7.5f;
        hitPos = new Vector2(hitPos.x + m_firstDigitX, hitPos.y + m_firstDigitY);
        return hitPos;
    }
}
