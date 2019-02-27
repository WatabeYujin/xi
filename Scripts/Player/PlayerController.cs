using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.AI;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.EventSystems;

public class PlayerController : ObjectPool
{
    [SerializeField]
    private Rigidbody playerRigidbody;                  //プレイヤーのRigidBody
    [SerializeField]
    private Transform playerCameraTransform;            //プレイヤーのカメラのTransform情報
    [SerializeField]
    private Life life;                                  //ライフポイント
    [SerializeField]
    private Transform stickWeponSpawn;                  //発射地点
    [SerializeField]
    private GameObject stickWeponBullet;                //スティックウェポンの発射弾
    [SerializeField]
    private GameObject snipeWeponBullet;                //スナイプカノンでの発射弾
    [SerializeField]
    private Animator playerAnimator;                    //プレイヤーのAnimator
    [SerializeField]
    private Transform sniperCamera;                     //スナイパーモードでのカメラ
    [SerializeField]
    private GameObject nomalModeUI;                     //通常モード時のUI
    [SerializeField]
    private GameObject snipeModeUI;                     //スナイパーモード時のUI
    [SerializeField]
    private RimitTime rimitTime;                        //制限時間
    [SerializeField]
    private ParticleSystem dashEffect;                  //フリックドッジ自生成するエフェクト
    [SerializeField]
    private LineRenderer laserSight;                    //レーザーサイトのLineRenderer
    [SerializeField]
    private GameObject flickIconObj;                    //フリックドッジのアイコン
    [SerializeField]
    private GameObject flickIcon;
    [SerializeField]
    private Text snipeBulletCount;                      //スナイパー残弾数text
    [SerializeField]
    private Image snipeRecastImage;                     //スナイパーキャノンのリキャストモード
    [SerializeField]
    private Text[] snipeCountTexts=new Text[2];         //スナイプカノンの残弾
    [SerializeField]
    private TalkControl talkControl;                    //会話システム
    [SerializeField]
    private Text[] lifeText = new Text[2];              //ライフポイントのtext
    [SerializeField]
    private GameObject[] isActiveObject;
    [SerializeField]
    private Status status;                              //ステータス情報
    [SerializeField]
    private bool isActive = true;                       //エネミーが動ける状態
    [SerializeField]
    private FlickEffect flickeffect;                    //フリックエフェクト発生用のプログラム

    public static PlayerController playerController;

    private const string moveHorizontal = "Horizontal1";//移動スティックの水平
    private const string moveVertical = "Vertical1";    //移動スティックの垂直
    private const string shotHorizontal = "Horizontal2";//射撃スティックの水平
    private const string shotVertical = "Vertical2";    //射撃スティックの垂直
    private const string snipeHorizontal = "Horizontal3";//射撃スティックの水平
    private const string snipeVertical = "Vertical3";    //射撃スティックの垂直
    private const string straightShotButton = "StraightShot";           //射撃ボタン
    private const string snipeButton = "SnipeShot";     //狙撃ボタン
    
    [SerializeField]
    private List<Image> cloneFlickIcons = new List<Image>();                //生成したアイコン
    private float lastShotTime;                         //最後にスティックウェポンを撃った時間
    private float[] flickStartTime = new float[10];     //フリックし始めた時間
    private Vector2[] flickStartPos = new Vector2[10];  //フリックし始めた時間
    private bool isFlickDash;                           //フリックダッシュ中か
    private Vector3 flickDashRotate;                    //フリックダッシュの方向
    private Vector2 lastNomalModerotate;
    private bool sniperMode = false;
    private bool isRunning = true;
    private float dashStartTime;
    private int slideFingerID = -1;
    private Vector2 snipeTouchBeginPos=new Vector2(-999,-999);
    private int snipeTouchBeginID = -1;



    void Awake() {
        playerController = this;
    }

    void Start() {
        Application.targetFrameRate = 60;       //目標FPSを60に
        SetAutorotate();
        life.SetMaxLife(status.xiStatus.helth);
        FirstFlickUISetting();
        if (!isActive)
            return;
        FirstTalk();
    }

    void Update () {
        if (!isActive)
            return;
        SnipeBulletCountPrint();
        FlickUIPrint();
        CameraMove();
        if (!isRunning) return;
        if (SniperModeChange()) {
            SniperAction();
            SniperCameraInitialization();
            return;
        }
        NomalCameraInitialization();
        MoveCheck();
        PlayerRotateCheck();
        ShotCheck(stickWeponBullet, status.straightWeponStatus.fireRate- status.save.straightNode[13].GetLevel, 0);
        CoolDown(true,!sniperMode);
        FlickCheck();
        ShotAnimation();
        LifePointPrint();
    }

    /// <summary>
    /// 初期化処理
    /// </summary>
    private void SetAutorotate()
    {
        status.flickDodgeStatus.maxDodgeCount = 3 + status.save.flickDodgeNode[3].GetLevel;
        status.flickDodgeStatus.dodgeCount= status.flickDodgeStatus.maxDodgeCount;
        status.snipeCanonStatus.maxBulletCount= 3 + status.save.snipeCannonNode[0].GetLevel;
        status.snipeCanonStatus.bulletCount = status.snipeCanonStatus.maxBulletCount;
        Input.gyro.enabled = true;
        Screen.autorotateToPortraitUpsideDown = false;
#if UNITY_ANDROID
        Screen.autorotateToPortrait = true;
#endif
        Screen.autorotateToLandscapeRight = true;
        Screen.autorotateToLandscapeLeft = true;

    }

    /// <summary>
    /// ミッション開始時の会話
    /// </summary>
    private void FirstTalk()
    {
        const string startVoice = "アクセス開始。[%p]、お願いします。";
        talkControl.TalkSet(startVoice, 1, 1, 1, true);
    }

    /// <summary>
    /// クールダウンの処理を行う
    /// </summary>
    /// <param name="isDodge"></param>
    /// <param name="isSnipe"></param>
    void CoolDown(bool isDodge,bool isSnipe)
    {
        float m_time = Time.deltaTime;
        if (isDodge &&
            status.flickDodgeStatus.dodgeCount < status.flickDodgeStatus.maxDodgeCount)
        {
            status.flickDodgeStatus.recastTime -= m_time*(status.save.flickDodgeNode[3].GetLevel+1);
            if (status.flickDodgeStatus.recastTime < 0)
            {
                status.flickDodgeStatus.dodgeCount++;
                status.flickDodgeStatus.recastTime = status.flickDodgeStatus.maxRecastTime;
            }
        }
        if (!sniperMode &&
            status.snipeCanonStatus.bulletCount < status.snipeCanonStatus.maxBulletCount &&
            status.snipeCanonStatus.recastTime > 0)
        {
            status.snipeCanonStatus.recastTime -= m_time * (status.save.snipeCannonNode[1].GetLevel + 1);
            ReCastView(snipeRecastImage, status.snipeCanonStatus.recastTime, status.snipeCanonStatus.maxRecastTime);
            if (status.snipeCanonStatus.recastTime <= 0)
            {
                status.snipeCanonStatus.bulletCount++;
                status.snipeCanonStatus.recastTime = status.snipeCanonStatus.maxRecastTime;
            }
        }
    }

    ///////////////////////////////移動・旋回関連///////////////////////////////

    /// <summary>
    /// 移動スティックの入力
    /// </summary>
    /// <returns>移動スティックの入力値</returns>
    Vector2 GetPlayerMoveAxis()
    {
        return 
            new Vector2(CrossPlatformInputManager.GetAxis(moveHorizontal),
                        CrossPlatformInputManager.GetAxis(moveVertical));
    }

    /// <summary>
    /// 射撃スティックの入力
    /// </summary>
    /// <returns>射撃スティックの入力値</returns>
    Vector2 GetPlayerShotAxis()
    {
        return
            new Vector2(CrossPlatformInputManager.GetAxis(shotHorizontal),
                        CrossPlatformInputManager.GetAxis(shotVertical));
    }

    /// <summary>
    /// 移動処理
    /// </summary>
    /// <param name="moveRotate">移動方向</param>
    void Move(Vector2 moveRotate)
    {
        float m_magnification = 1;
        if (StraightShotTriggerCheck())
        {
            m_magnification = m_magnification / 2;
            if (status.save.straightNode[11].GetLevel > 0)
                m_magnification *= status.save.straightNode[11].GetLevel / 10;
        }
        Vector3 m_move = new Vector3(moveRotate.x, 0, moveRotate.y);
        if (!StraightShotTriggerCheck())
        {
            playerRigidbody.velocity =
                m_move * status.xiStatus.moveSpeed;
        }else
        {
            playerRigidbody.velocity =
                m_move * status.xiStatus.shotermoveSpeed;
        }
    }

    /// <summary>
    /// カメラの移動処理
    /// </summary>
    void CameraMove() {
        Vector3 m_cameraPos= new Vector3(status.xiStatus.cameraPosition.x + transform.position.x,
                        status.xiStatus.cameraPosition.y,
                        status.xiStatus.cameraPosition.z + transform.position.z);
        playerCameraTransform.DOMove(m_cameraPos, 0.7f).SetEase(Ease.Unset);
        //playerCameraTransform.position = m_cameraPos;
    }

    /// <summary>
    /// 旋回方向の取得（PCでのデバッグ用の旋回処理入り）
    /// </summary>
    public void PlayerRotateCheck()
    {
#if UNITY_EDITOR
        Vector3 m_playerShotAxis = Vector3.zero;
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
            m_playerShotAxis = new Vector3(hit.point.x,0, hit.point.z);
        transform.LookAt(m_playerShotAxis);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, transform.eulerAngles.z);
#elif UNITY_ANDROID
        Vector2 m_playerShotAxis = GetPlayerShotAxis();

        if (axisRunningCheck(m_playerShotAxis))
            Rotate(m_playerShotAxis);
#endif
    }

    /// <summary>
    /// 旋回処理
    /// </summary>
    /// <param name="playerRotate"></param>
    void Rotate(Vector2 playerRotate)
    {
        Vector3 m_targetRotate = new Vector3(playerRotate.x, 0, playerRotate.y);
        Quaternion m_targetQuaternion = Quaternion.LookRotation(m_targetRotate);
        transform.eulerAngles =
            Vector3.up * Mathf.MoveTowardsAngle(
                transform.eulerAngles.y, m_targetQuaternion.eulerAngles.y,
                status.xiStatus.rotateSpeed * Time.deltaTime
            );
    }

    /// <summary>
    /// スティックの移動許容範囲
    /// </summary>
    /// <param name="checkAxis">調べるスティックの入力</param>
    /// <returns>指定以上の移動値が出ていればtrue、そうでなければfalse</returns>
    bool axisRunningCheck(Vector2 checkAxis)
    {
        float m_runnningRange = 0.4f;
        if (checkAxis.x > m_runnningRange) return true;
        if (checkAxis.x < -m_runnningRange) return true;
        if (checkAxis.y > m_runnningRange) return true;
        if (checkAxis.y < -m_runnningRange) return true;
        return false;
    }

    ///////////////////////////////射撃関連///////////////////////////////

    /// <summary>
    /// 通常・狙撃両方の射撃入力の確認を行う
    /// </summary>
    /// <param name="shotbullet">発射時の弾丸</param>
    /// <param name="fireRate">発射レート</param>
    /// <param name="vibrateTime">バイブレーションの長さ</param>
    void ShotCheck(GameObject shotbullet, float fireRate, long vibrateTime)
    {
        if (!ShotRateCheck(fireRate)) return;
        if (sniperMode)
        {
            //狙撃モード
            if (!SnipeShotTriggerCheck()) return;
            if (status.snipeCanonStatus.bulletCount <= 0) return;
            status.snipeCanonStatus.recastTime = status.snipeCanonStatus.maxRecastTime;
            status.snipeCanonStatus.bulletCount--;
        }
        else
        {
            //通常モード
            if (!StraightShotTriggerCheck()) return;
        }
        lastShotTime = Time.time;
        Vibration.Vibrate(vibrateTime);     //バイブレーション
        if (sniperMode)
        {
            Transform m_bullet = Instantiate(snipeWeponBullet).transform;
            m_bullet.localScale = Vector3.one * (status.save.snipeCannonNode[1].GetLevel*0.2f);
            m_bullet.position = stickWeponSpawn.position;
            m_bullet.forward = transform.forward;
            m_bullet.GetComponent<SnipeBullet>().SetStatus(status.snipeCanonStatus);
        }
        else
        {
            for(int i = -1;i<Pellet();i++)
            {
                Transform m_bullet = Objectspawn(stickWeponBullet, stickWeponSpawn.position, transform.rotation).transform;
                m_bullet.GetComponent<StickWeponBullet>().SetStatus(status.straightWeponStatus);
            }
        }
    }


    /// <summary>
    /// 射撃のレートの確認
    /// </summary>
    /// <param name="firelate">射撃レート</param>
    /// <returns>射撃可能か</returns>
    bool ShotRateCheck(float firelate)
    {
        if (Time.time - lastShotTime >= firelate)
            return true;
        else
            return false;
    }

    ///////////////////////////////ストレートウェポン関係///////////////////////////////

    /// <summary>
    /// 射撃入力の検知
    /// </summary>
    /// <returns>入力があった場合true</returns>
    bool StraightShotTriggerCheck(){
#if UNITY_EDITOR
        if (Input.GetMouseButton(0)) return true;
#endif
        if (CrossPlatformInputManager.GetButton(straightShotButton)) return true;
        else return false;
    }

    //散弾ノード用の処理
    int Pellet()
    {
        return status.save.straightNode[5].GetLevel;
    }

    ///////////////////////////////フリック入力関連///////////////////////////////

    /// <summary>
    /// フリック入力の取得
    /// </summary>
    void FlickCheck() {
        if (isFlickDash){
            FlickAction();
            return;
        }
#if UNITY_EDITOR
        if (!Input.GetKeyDown(KeyCode.LeftShift)) return;
        if (isFlickDash) return;
        if (!FlickCountCheck()) return;
        status.flickDodgeStatus.recastTime += status.flickDodgeStatus.usedCoolDownTime;
        status.flickDodgeStatus.dodgeCount--;
        isFlickDash = true;
        dashStartTime = Time.time;
        DashEffect();
#elif UNITY_ANDROID
        foreach (Touch t in Input.touches)
        {
            switch (t.phase)
            {
                case TouchPhase.Began:
                    FlickStart(t);
                    break;
                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    if (isFlickDash) return;
                    if (!FlickTimeCheck(t.fingerId)) return;
                    if (!FlickRangeCheck(t.fingerId)) return;
                    if (!FlickCountCheck()) return;
                    status.flickDodgeStatus.recastTime += status.flickDodgeStatus.usedCoolDownTime;
                    status.flickDodgeStatus.dodgeCount--;
                    isFlickDash = true;
                    dashStartTime = Time.time;
                    FlickRotateCheck(t.fingerId);
                    DashEffect();
                    break;
                default:

                    break;
            }
        }
#endif
    }

    /// <summary>
    /// フリックドッジ時のエフェクトの生成
    /// </summary>
    void DashEffect()
    {
        dashEffect.Play();
        dashEffect.gameObject.transform.LookAt(flickDashRotate+transform.position);
        flickeffect.EffectSpawn();
    }

    void FlickStart(Touch t) {
        flickStartPos[t.fingerId] = t.position;
        flickStartTime[t.fingerId] = Time.time;
    }

    bool FlickTimeCheck(int fingerID) {
        const float m_flickRimitTime = 0.2f;
        
        if (Time.time - flickStartTime[fingerID] >= m_flickRimitTime) return false;
        else return true;
    }   

    //フリックドッジの使用回数の確認を行う
    bool FlickCountCheck()
    {
        if (status.flickDodgeStatus.dodgeCount <= 0) return false;
        return true;
    }
    
    bool FlickRangeCheck(int fingerID) {
        const float m_flickRange = 100;
        if ((flickStartPos[fingerID] -Input.touches[fingerID].position).magnitude < m_flickRange) return false;
        else return true;
    }

    void FlickRotateCheck(int fingerID) {
        flickDashRotate = (Input.touches[fingerID].position - flickStartPos[fingerID]).normalized;
        flickDashRotate = 
            new Vector3(
                Mathf.Clamp(flickDashRotate.x, -1, 1),
                0,
                Mathf.Clamp(flickDashRotate.y, -1, 1)
                );
    }

    void FlickAction() {
        
        RaycastHit hit;
        Ray m_ray = new Ray(transform.position, flickDashRotate);
        Vector3 m_dashPos = Vector3.zero;
        float m_range = status.flickDodgeStatus.flickDashRange;
        float m_attackSize= status.flickDodgeStatus.flickAttackSize;
        m_range += 1f * status.save.flickDodgeNode[0].GetLevel;
        m_attackSize += 0.2f * status.save.flickDodgeNode[4].GetLevel;
        if (Physics.Raycast(m_ray, out hit, m_range, ~(1 << 10)))
        {
            m_dashPos = hit.point;
        }
        else { 
            m_dashPos = transform.position + (flickDashRotate * m_range);
        }
        FlickAttack(Physics.SphereCastAll(m_ray,m_attackSize, m_range, 1 << 10));

        transform.position = m_dashPos;
        
        isFlickDash = false;
    }

    /// <summary>
    /// フリックダッシュ時の攻撃処理
    /// </summary>
    /// <param name="hitsEnemy">命中した敵の配列</param>
    void FlickAttack(RaycastHit[] hitsEnemy)
    {
        int m_damage = status.flickDodgeStatus.flickAttack;
        m_damage += (int)(0.2f * status.save.flickDodgeNode[1].GetLevel * m_damage);
        foreach (RaycastHit enemy in hitsEnemy)
        {
            Life m_life = enemy.transform.GetComponent<Life>();
            if (m_life != null)
                m_life.Damage(m_damage);
        }

    }

    //スナイパーモード用
    bool SniperModeChange() {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.LeftControl))
            return !sniperMode;
        else
            return sniperMode;
#elif UNITY_ANDROID
        if (Screen.orientation == ScreenOrientation.Portrait)
            return true;
        else
            return false;
#endif
    }

    void SniperCameraInitialization() { 
        if (sniperMode) return;
        laserSight.enabled = false;
        nomalModeUI.SetActive(false);
        snipeModeUI.SetActive(true);
        playerAnimator.speed = 0;
        sniperMode = true;
        lastNomalModerotate = transform.eulerAngles;
        playerCameraTransform.gameObject.SetActive(false);
        sniperCamera.gameObject.SetActive(true);
    }

    void NomalCameraInitialization() {
        if (!sniperMode) return;
        laserSight.enabled = true;
        playerAnimator.speed = 1;
        snipeModeUI.SetActive(false);
        nomalModeUI.SetActive(true);
        sniperMode = false;
        transform.eulerAngles = lastNomalModerotate;
        playerCameraTransform.gameObject.SetActive(true);
        sniperCamera.gameObject.SetActive(false);
    }

    void SniperAction() {
        if (!Input.gyro.enabled) return;
        const long m_vibrationLength = 150;
        transform.eulerAngles += GyroEulerAngles();
        transform.eulerAngles += SnipeRotateSlideControl();
        ShotCheck(snipeWeponBullet, status.snipeCanonStatus.fireRate, m_vibrationLength);
        
    }

    Vector3 GyroEulerAngles() {
        Vector3 m_gyroEulerAngles = Input.gyro.rotationRateUnbiased;
        m_gyroEulerAngles =
            new Vector3(-m_gyroEulerAngles.x,
                        -m_gyroEulerAngles.y
                        );
        return m_gyroEulerAngles/1.2f;
    }

    Vector3 SnipeRotateSlideControl()
    {
        Vector2 m_move = Vector2.zero;
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
            snipeTouchBeginPos = Input.mousePosition;
        if (Input.GetMouseButton(0))
        {
            m_move = snipeTouchBeginPos - new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }
#elif UNITY_ANDROID
        //タッチ開始の情報がなかった場合
        if (snipeTouchBeginID == -1)
        {
            //タッチ開始の検知へ
            foreach (Touch touch in Input.touches)
            {
                int m_touchID = touch.fingerId;
                //トリガーボタンをタッチしていない場合、カメラ操作として
                //タッチ開始とフィンガーIDを取得する。
                if (touch.phase == TouchPhase.Began &&
                    !CrossPlatformInputManager.GetButtonDown(straightShotButton))
                {
                    snipeTouchBeginID = touch.fingerId;
                    snipeTouchBeginPos = Input.touches[m_touchID].position;
                    break;
                }
            }
        }
        if (snipeTouchBeginID != -1)
        {
            switch (Input.touches[snipeTouchBeginID].phase)
            {
                case TouchPhase.Moved:
                case TouchPhase.Stationary:
                    m_move = snipeTouchBeginPos - Input.touches[snipeTouchBeginID].position;
                    break;
                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    snipeTouchBeginID = -1;
                    break;
            }
        }
#endif
        m_move /= 200;
        m_move = new Vector3(-Mathf.Clamp(m_move.y,-30,30),-Mathf.Clamp(m_move.x, -30, 30));
        if (status.snipeCanonStatus.option.reverseSide)
            m_move = new Vector3(m_move.x, -m_move.y);
        if (!status.snipeCanonStatus.option.reverseVertical)
            m_move = new Vector3(-m_move.x, m_move.y);
        return m_move;
    }

    public void FlickUIPrint()
    {
        for (int i = 0; i < status.flickDodgeStatus.maxDodgeCount; i++)
        {
            cloneFlickIcons[i].enabled = i < status.flickDodgeStatus.dodgeCount + 1;
        }
        if (status.flickDodgeStatus.dodgeCount < status.flickDodgeStatus.maxDodgeCount)
            ReCastView(cloneFlickIcons[status.flickDodgeStatus.dodgeCount], status.flickDodgeStatus.recastTime, status.flickDodgeStatus.maxRecastTime);
    }

    //狙撃ボタンを押したかの確認
    bool SnipeShotTriggerCheck()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonUp(0))
            return true;
        else return false;
#elif UNITY_ANDROID
        //押した瞬間か離れた瞬間かオプションで変更可能に
        if (status.snipeCanonStatus.option.isSnipepushUpmode)
            return CrossPlatformInputManager.GetButtonUp(snipeButton);
        else
            return CrossPlatformInputManager.GetButtonDown(snipeButton);
#endif
    }

    //アニメーション関係
    void MoveAnimation(Vector2 stickMove) {
        float m_playerEulerAnglesY = transform.eulerAngles.y;
        if (m_playerEulerAnglesY > 90 && m_playerEulerAnglesY < 270) stickMove.y *= -1;
        if (m_playerEulerAnglesY > 180) stickMove.x *= -1;
        if (stickMove.y < 0) stickMove *= -1;
        playerAnimator.SetFloat("X", MoveLocalRotateCheck().x);
        playerAnimator.SetFloat("Z", MoveLocalRotateCheck().y);
    }

    private Vector2 MoveLocalRotateCheck()
    {
        if (GetPlayerMoveAxis() == Vector2.zero)
            return Vector2.zero;
        float m_playerEulerAngleBefore = transform.eulerAngles.y;
        float m_playerEulerAngleAfter = Quaternion.LookRotation(new Vector3(GetPlayerMoveAxis().x, 0, GetPlayerMoveAxis().y)).eulerAngles.y;
        float m_playerEulerAngles = m_playerEulerAngleBefore - m_playerEulerAngleAfter;
        m_playerEulerAngles = m_playerEulerAngles <= 0 ? m_playerEulerAngles + 360 : m_playerEulerAngles;//ここで向きと移動方向の差が求まる
        Vector2 m_MoveRotate;
        float m_differenceRotateY;
        int[] m_mainus = new int[2];
        if (m_playerEulerAngles <= 90)
        {
            m_mainus[0] = -1;
            m_mainus[1] = 1;
            m_differenceRotateY = m_playerEulerAngles;
        }
        else if (m_playerEulerAngles > 90 && m_playerEulerAngles <= 180)
        {
            m_differenceRotateY = 90 - (m_playerEulerAngles - 90);
            m_mainus[0] = -1;
            m_mainus[1] = -1;

        }
        else if (m_playerEulerAngles > 180 && m_playerEulerAngles <= 270)
        {
            m_differenceRotateY = m_playerEulerAngles - 180;
            m_mainus[0] = 1;
            m_mainus[1] = -1;

        }
        else
        {
            m_differenceRotateY = 90 - (m_playerEulerAngles - 270);
            m_mainus[0] = 1;
            m_mainus[1] = 1;

        }
        m_MoveRotate.x = m_differenceRotateY / 90 * m_mainus[0];

        m_MoveRotate.y = (90 - m_differenceRotateY) / 90 * m_mainus[1];

        return m_MoveRotate;
    }

    void ShotAnimation() {
        bool m_trigger = StraightShotTriggerCheck();
        if (!m_trigger) m_trigger = sniperMode;
        playerAnimator.SetBool("Shot", m_trigger);
    }

    void SnipeBulletCountPrint()
    {
        if (sniperMode) snipeCountTexts[1].text = status.snipeCanonStatus.bulletCount.ToString();
        else snipeCountTexts[0].text = status.snipeCanonStatus.bulletCount.ToString();
    }

    void ReCastView(Image viewImage, float nowRecastTime, float maxRecastTime)
    {
        if (nowRecastTime == 0)
            viewImage.fillAmount = 1;
        else
            viewImage.fillAmount = 1 - nowRecastTime / maxRecastTime;
    }

    //////////////////////////////////////////////////////////////////////

    public void AnimatorStop()
    {
        playerAnimator.SetFloat("X", 0);
        playerAnimator.SetFloat("Z", 0);
    }

    //UIコントロール
    public void FirstFlickUISetting()
    {
        cloneFlickIcons.Add(flickIcon.GetComponent<Image>());
        for (int i = 1; i < status.flickDodgeStatus.maxDodgeCount; i++)
        {
            GameObject m_icon = Instantiate(flickIcon, flickIconObj.transform);
            cloneFlickIcons.Add(m_icon.GetComponent<Image>());
        }
    }

    /// <summary>
    /// 移動関連
    /// </summary>
    public void MoveCheck()
    {
#if UNITY_EDITOR
        Vector2 m_moveRotate = Vector2.zero;
        if (Input.GetKey(KeyCode.W))
            m_moveRotate += Vector2.up;
        else if (Input.GetKey(KeyCode.S))
            m_moveRotate -= Vector2.up;
        if (Input.GetKey(KeyCode.D))
            m_moveRotate += Vector2.right;
        else if (Input.GetKey(KeyCode.A))
            m_moveRotate -= Vector2.right;
        flickDashRotate = new Vector3(m_moveRotate.x,0, m_moveRotate.y);
#elif UNITY_ANDROID
        Vector2 m_moveRotate = GetPlayerMoveAxis();
#endif
        MoveAnimation(m_moveRotate);
        Move(m_moveRotate);
    }

    /// <summary>
    /// ライフポイントの描画
    /// </summary>
    void LifePointPrint()
    {
        if (sniperMode)
            lifeText[1].text = life.lifePoint.ToString();
        else
            lifeText[0].text = life.lifePoint.ToString();
    }
    //その他コントロール関係

    public void ControlStop()
    {
        isRunning = false;
        playerRigidbody.velocity = Vector3.zero;
        AnimatorStop();
        rimitTime.IsTime(false);
    }

    public void ControlStart()
    {
        isRunning = true;
        rimitTime.IsTime(true);
    }

    public void KillCount() {
        status.xiStatus.killCount++;
    }

    /// <summary>
    /// メインゲームのアクティブ化
    /// </summary>
    public bool GetSetisActive
    {
        get{
            return isActive;
        }
        set
        {
            isActive = value;
            foreach(GameObject obj in isActiveObject)
            {
                obj.SetActive(isActive);
            }
        }
    }

}
