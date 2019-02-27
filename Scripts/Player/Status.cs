using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour {

    public SaveScriptableObject2 save;
    public XiStatus xiStatus;
    public StraightWeponStatus straightWeponStatus;
    public FlickDodgeStatus flickDodgeStatus;
    public SnipeCanonStatus snipeCanonStatus;
    void Awake()
    {
        save = Resources.Load("SaveData") as SaveScriptableObject2;
        xiStatus = new XiStatus(save);
        straightWeponStatus = new StraightWeponStatus(save);
        flickDodgeStatus = new FlickDodgeStatus(save);
        snipeCanonStatus = new SnipeCanonStatus(save);
    }

    //本体ステータス
    public class XiStatus
    {
        private SaveScriptableObject2 save;
        public XiStatus(SaveScriptableObject2 value)
        {
             save = value;
        }
        public int killCount = 0;       //キルカウント数
        public float moveSpeed = 6f;    //移動速度
        public float shotermoveSpeed = 3f;    //移動速度
        public float rotateSpeed = 250; //旋回速度
        public Vector3 cameraPosition =
            new Vector3(0, 16, -5);     //カメラ座標
        public int helth = 100;         //体力値
    }

    //ストレートウェポンステータス
    public class StraightWeponStatus
    {
        private SaveScriptableObject2 save;
        public StraightWeponStatus(SaveScriptableObject2 value)
        {
            save = value;
        }
        public float fireRate = 0.4f;   //発射レート
        public float bulletSpeed = 20; //弾速
        public float recoil = 20;        //反動
        public float bulletRange = 27;  //弾の射程
        private int damage = 100;        //弾の攻撃力
        public int GetDamage()
        {
            int m_damage = damage;
            if (save.straightNode[5].GetLevel > 0)
            {
                m_damage = Mathf.FloorToInt(m_damage * ((save.straightNode[5].GetLevel + 1) * 0.2f + 1) / (save.straightNode[5].GetLevel + 1));
            }
            return damage;
        }
    }

    //フリックドッジステータス
    public class FlickDodgeStatus
    {
        private SaveScriptableObject2 save;
        public FlickDodgeStatus(SaveScriptableObject2 value)
        {
            save = value;
        }
        public int dodgeCount = 3;              //使用回数
        public int maxDodgeCount = 3;           //最大使用数
        public float recastTime = 2;            //現在のリキャストタイム
        public float maxRecastTime = 2;         //最大リキャストタイム
        public float usedCoolDownTime = 0.5f;   //使用回数のクールダウン開始までの時間
        public float flickDashRange = 10f;      //最大移動距離
        public int flickAttack = 75;            //通過した敵へのダメージ
        public float flickAttackSize = 1f;      //攻撃判定の広さ
    }

    //スナイプカノンステータス
    public class SnipeCanonStatus
    {
        public SnipeCanonStatus(SaveScriptableObject2 value)
        {
            save = value;
        }
        private SaveScriptableObject2 save;

        public int damage = 50;             //弾の攻撃力
        public float fireRate = 0.5f;       //発射レート
        public int bulletCount = 6;         //残弾
        public int maxBulletCount = 6;      //最大弾数
        public float recastTime = 2;        //現在のリキャストタイム
        public float maxRecastTime = 5;     //最大リキャストタイム
        public float bulletSpeed = 100;     //弾速(固定)
        public float bulletRange = 30;      //弾の射程(固定)
        public Option option = new Option();
        public class Option
        {
            public bool reverseSide = false;
            public bool reverseVertical = false;
            public bool isSnipepushUpmode = true;
        }
    }
}
