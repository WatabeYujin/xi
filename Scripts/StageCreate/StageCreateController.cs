using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class StageCreateController : MonoBehaviour
{
    private bool isfloorSelectActive = false;
    [SerializeField]
    private GameObject activeSelectFloorObj;
    [SerializeField]
    private StageSpawn stagespawn;
    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private Vector2 minRange;
    [SerializeField]
    private Vector2 maxRange;
    [SerializeField]
    private GameObject[] UiWindows;
    [SerializeField]
    private Text gimmicText;
    [SerializeField]
    private Image gimmicImage;
    [SerializeField]
    private RectTransform gimmicrotateImage;
    [SerializeField]
    private GimmicData gimmicData;
    [SerializeField]
    private TalkControl talk;
    [SerializeField]
    private GameObject stagemenu;
    [SerializeField]
    private PlayerController plcont;
    [SerializeField]
    private FadeInOut fade;

    private Vector2 nowTouchPosition;
    private Vector2 oldMovePos;
    private Vector2[] firstUiPositions = new Vector2[2];
    private List<Vector2> touchPos;
    private Vector3 oldCameraPos;
    private Touch[] touch;
    private int setObjID = 0;
    private int gimmicRotate = 0;
    private int mode = 0;
    bool isActive = true;

    private const float floorScale = 5;

    void Start()
    {
        firstUiPositions[0] = UiWindows[0].transform.position;
        firstUiPositions[1] = UiWindows[1].transform.position;
        GimmicButton(0);                                        //選択IDを0にするため
    }

    void Update()
    {
        if (!isActive)
            return;
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
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    void PointUp()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonUp(0)|| Input.GetMouseButtonUp(1))
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
            PositionIDSet(nowTouchPosition);
            //FloorSet(nowTouchPosition);
        }
    }

    /// <summary>
    /// タッチ入力を検知し、各種処理に移る
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
        Debug.Log("通った");
        RaycastHit m_hit;
        Vector3 m_pos = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(m_pos);
        if (!Physics.Raycast(ray, out m_hit))
            return;
        oldCameraPos = new Vector3(m_hit.point.x, 0, m_hit.point.z);
        ViewAllStage();
    }

    /// <summary>
    /// カメラの移動
    /// </summary>
    /// <param name="pos">移動に基準となる位置</param>
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
    /// 指の触れているフロアを強調表示する。
    /// 指が離れた場合、オブジェクトを配置する。
    /// </summary>
    /// <param name="pos">タッチ座標</param>
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
    /// UIのフェードイン・アウトを行う
    /// </summary>
    /// <param name="isfadeIn">フェードインさせる場合true</param>
    /// <param name="FadeMode">フェードを行うUI(-1ならすべて)</param>
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

    void PositionIDSet(Vector2 pos)
    {
        int m_x, m_y;
        pos -= new Vector2(floorScale / 2, floorScale / 2);
        m_x = (int)Mathf.Abs(pos.x / floorScale);
        m_y = (int)Mathf.Abs(pos.y / floorScale);
        if (!stagespawn.RequiredCheck(m_x, m_y))
        {
            stagespawn.StageObjUpdate(m_x, m_y, setObjID, gimmicRotate);
        }
        else
            talk.TalkSet("必須ギミックの為、消去・上書きはできません。", 1, 1, 1, true);

    }




    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


    /// <summary>
    /// UIを触れているか判定
    /// </summary>
    /// <returns>触れていた場合trueを返す</returns>
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
    /// 選択中のフロアの強調表示
    /// </summary>
    /// <param name="hitPos">選択中の座標</param>
    /// <returns>選択したフロアの座標</returns>
    Vector2 FloorPosCheck(Vector2 hitPos)
    {
        Vector2 m_minLimitPos = new Vector2(0, 0);
        Vector2 m_maxLimitPos = new Vector2(100, 100);
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
    /// カメラの移動範囲制限
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

    public void StageMenuOpenCloase(bool isopen)
    {
        stagespawn.StageSet();
        isActive = isopen;
        stagemenu.SetActive(isopen);
        mainCamera.gameObject.SetActive(isopen);
        fade.FadeOutEvent(0.2f);
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public void GimmicButton(int ID)
    {
        setObjID = ID;
        gimmicText.text = gimmicData.gimmicList[ID].GetGimmicName + "\n" + gimmicData.gimmicList[ID].GetGimmicDetails;
        gimmicImage.sprite = gimmicData.gimmicList[ID].GetGimmicImage;
    }

    public void ViewAllStage()
    {
        const float m_moveSpeed = 0.5f;
        const float m_height = 30;
        Vector3 m_movePos = new Vector3(45, 95, 50);
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

    public void TestMode()
    {
        plcont.GetSetisActive = !plcont.GetSetisActive;
        StageMenuOpenCloase(!plcont.GetSetisActive);
    }


    public void GimmicRotateButton()
    {
        gimmicRotate += 90;
        if (gimmicRotate >= 360)
            gimmicRotate = 0;
        gimmicrotateImage.DORotate(Vector3.forward * -gimmicRotate, 0.2f);
    }
}
