using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class StageCreateController : MonoBehaviour
{

    [SerializeField]
    private GameObject test;
    private bool isfloorSelectActive = false;
    [SerializeField]
    private GameObject activeSelectFloorObj;
    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private Vector2 minRange;
    [SerializeField]
    private Vector2 maxRange;
    [SerializeField]
    private GameObject[] UiWindows;
    private Vector2 nowTouchPosition;
    private Vector2 oldMovePos;
    private Vector2[] firstUiPositions = new Vector2[2];
    private List<Vector2> touchPos;
    [SerializeField]
    private int mode = 0;
    [SerializeField]
    private StageSpawn stagespawn;
    private Touch[] touch;
    private Vector3 oldCameraPos;
    private int setObjID = 0;
    private float floorScale = 5;

    void Start()
    {
        firstUiPositions[0] = UiWindows[0].transform.position;
        firstUiPositions[1] = UiWindows[1].transform.position;
    }

    void Update()
    {
        if (mode == 100)
        {
            StageAllViewMode_CameraMove();
            return;
        }
        PointUp();
        if (IsExist())
        {
            mode = 10;
            return;
        }
        ModeCheck();
    }

    void PointUp()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonUp(0))
#elif UNITY_ANDROID
        if (Input.touchCount == 0)
#endif
        {
            mode = 0;
            oldMovePos = Vector2.zero;
            UIFade(true);
            if (!isfloorSelectActive) return;
            isfloorSelectActive = false;
            activeSelectFloorObj.SetActive(false);
            FloorSet(nowTouchPosition);
        }
    }

    /// <summary>
    /// �^�b�`���͂����m���A�e�폈���Ɉڂ�
    /// </summary>
    void ModeCheck()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButton(0))
        {
            FloorSelect(Input.mousePosition);
        }
        if (Input.GetMouseButton(1))
        {
            CameraMove(Input.mousePosition);
        }

#elif UNITY_ANDROID
        touch = Input.touches;
        if (Input.touchCount==1&&mode<=1)
        {
            mode=1;
            FloorSelect(Input.touches[0].position);
        }
        if (Input.touchCount>=2)
        {
            UIFade(false);
            Vector2 m_touchPositions=Vector2.zero;
            int m_count = 0;
            foreach(Touch t in Input.touches)
            {
                m_touchPositions += t.position;
                m_count++;
            }
            m_touchPositions /= m_count;
            CameraMove(m_touchPositions);
            if (!isfloorSelectActive)   return;
            isfloorSelectActive = false;
            activeSelectFloorObj.SetActive(false);
        }else if(mode==2){
            oldMovePos=Vector2.zero;
        }
#endif
    }

    void StageAllViewMode_CameraMove()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        Debug.Log("�ʂ���");
        RaycastHit m_hit;
        Vector3 m_pos = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(m_pos);
        if (!Physics.Raycast(ray, out m_hit))
            return;
        oldCameraPos = new Vector3(m_hit.point.x, 0, m_hit.point.z);
        ViewAllStage();
    }

    /// <summary>
    /// �J�����̈ړ�
    /// </summary>
    /// <param name="pos">�ړ��Ɋ�ƂȂ�ʒu</param>
    void CameraMove(Vector2 pos)
    {
        if (mode > 2) return;
        mode = 2;
        pos /= Mathf.Clamp(10 - (Input.touchCount * 2), 1, 10);
        if (oldMovePos == Vector2.zero)
        {
            oldMovePos = pos;
            return;
        }
        Vector2 m_movePos = oldMovePos - pos;
        oldMovePos = pos;
        mainCamera.transform.position = RangeRimit(mainCamera.transform.position += new Vector3(m_movePos.x, 0, m_movePos.y));

    }

    /// <summary>
    /// �w�̐G��Ă���t���A�������\������B
    /// �w�����ꂽ�ꍇ�A�I�u�W�F�N�g��z�u����B
    /// </summary>
    /// <param name="pos">�^�b�`���W</param>
    void FloorSelect(Vector2 pos)
    {
        RaycastHit m_hit;
        Ray ray = Camera.main.ScreenPointToRay(pos);
        if (!Physics.Raycast(ray, out m_hit))
            return;
        Vector2 m_pos = new Vector2(m_hit.point.x, m_hit.point.z);
        nowTouchPosition = FloorPosCheck(m_pos);

        if (nowTouchPosition == new Vector2(-1, -1))
        {
            activeSelectFloorObj.SetActive(false);
            isfloorSelectActive = false;
            return;
        }
        if (!isfloorSelectActive)
        {
            activeSelectFloorObj.SetActive(true);
            isfloorSelectActive = true;
        }
        activeSelectFloorObj.transform.position = new Vector3(nowTouchPosition.x, -2.25f, nowTouchPosition.y);
    }

    /// <summary>
    /// �J�����̈ړ��͈͐���
    /// </summary>
    Vector3 RangeRimit(Vector3 movePos)
    {
        if (movePos.x < minRange.x)
            movePos = new Vector3(minRange.x, movePos.y, movePos.z);
        if (movePos.z < minRange.y)
            movePos = new Vector3(movePos.x, movePos.y, minRange.y);
        if (movePos.x > maxRange.x)
            movePos = new Vector3(maxRange.x, movePos.y, movePos.z);
        if (movePos.z > maxRange.y)
            movePos = new Vector3(movePos.x, movePos.y, maxRange.y);
        return movePos;
    }

    /// <summary>
    /// �I�����ꂽ�t���A�ɃI�u�W�F�N�g�𐶐�����
    /// </summary>
    /// <param name="pos">����������W</param>
    void FloorSet(Vector2 pos)
    {
        Debug.Log("����");
        GameObject obj = Instantiate(test);
        obj.transform.position = new Vector3(pos.x, 0, pos.y);
    }

    /// <summary>
    /// �I�𒆂̃t���A�̋����\��
    /// </summary>
    /// <param name="hitPos">�I�𒆂̍��W</param>
    /// <returns>�I�������t���A�̍��W</returns>
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
        //0�`5��������2.5
        //����ȊO��������7.5
        float m_firstDigitX = (int)hitPos.x % 10;
        float m_firstDigitY = (int)hitPos.y % 10;
        hitPos = new Vector2((int)hitPos.x - m_firstDigitX, (int)hitPos.y - m_firstDigitY);
        if (m_firstDigitX <= 5)
            m_firstDigitX = floorScale / 2;
        else
            m_firstDigitX = floorScale * 1.5f;
        if (m_firstDigitY <= 5)
            m_firstDigitY = floorScale / 2;
        else
            m_firstDigitY = floorScale * 1.5f;
        hitPos = new Vector2(hitPos.x + m_firstDigitX, hitPos.y + m_firstDigitY);
        return hitPos;
    }

    /// <summary>
    /// UI��G��Ă��邩����
    /// </summary>
    /// <returns>�G��Ă����ꍇtrue��Ԃ�</returns>
    bool IsExist()
    {
        if (!Input.GetMouseButtonDown(0))
            return false;
        EventSystem current = EventSystem.current;
        PointerEventData eventData = new PointerEventData(current)
        {
#if UNITY_EDITOR
            position = Input.mousePosition
#elif UNITY_ANDROID
            position = Input.touches[0].position
#endif
        };
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        current.RaycastAll(eventData, raycastResults);
        bool isExist = 0 < raycastResults.Count;
        return isExist;

    }

    /// <summary>
    /// UI�̃t�F�[�h�C���E�A�E�g���s��
    /// </summary>
    /// <param name="isfadeIn">�t�F�[�h�C��������ꍇtrue</param>
    /// <param name="FadeMode">�t�F�[�h���s��UI(-1�Ȃ炷�ׂ�)</param>
    void UIFade(bool isfadeIn, int FadeMode = -1)
    {
        float m_animation = 0.3f;
        float m_speed = 30;
        if (isfadeIn)
        {
            if (FadeMode == 0 || FadeMode == -1)
            {
                UiWindows[0].SetActive(true);
                UiWindows[0].transform.DOMove(firstUiPositions[0], m_animation);
            }
            if (FadeMode == 1 || FadeMode == -1)
            {
                UiWindows[1].SetActive(true);
                UiWindows[1].transform.DOMove(firstUiPositions[1], m_animation);
            }
        }
        else
        {
            if (FadeMode == 0 || FadeMode == -1)
            {
                UiWindows[0].SetActive(false);
                UiWindows[0].transform.DOMove(Vector3.right * m_speed - UiWindows[0].transform.position, m_animation);
            }
            if (FadeMode == 1 || FadeMode == -1)
            {
                UiWindows[1].SetActive(false);
                UiWindows[1].transform.DOMove(Vector3.up * m_speed - UiWindows[1].transform.position, m_animation);
            }
        }
    }

    public void ViewAllStage()
    {
        const float m_moveSpeed = 0.5f;
        const float m_height = 30;
        Vector3 m_movePos = new Vector3(90, 185, 100);
        if (mode == 100)
        {

            UIFade(true);
            mainCamera.transform.DOMove(RangeRimit(new Vector3(oldCameraPos.x, m_height, oldCameraPos.z)), m_moveSpeed);
            mode = 10;
        }
        else
        {
            mode = 100;
            UIFade(false, 0);
            oldCameraPos = mainCamera.transform.position;
            mainCamera.transform.DOMove(m_movePos, m_moveSpeed);
        }

    }

    public void GimmicButton(int ID)
    {
        setObjID = ID;
    }

    void PositionIDSet(Vector2 pos)
    {
        int m_x, m_y;
        pos -= new Vector2(floorScale / 2, floorScale / 2);
        m_x = (int)Mathf.Abs(pos.x / floorScale);
        m_y = (int)Mathf.Abs(pos.y / floorScale);
        stagespawn.StageObjUpdate(m_x, m_y,setObjID);
    }
}
