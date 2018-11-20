using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour {

    public SavaScriptableObject2 save;

    //本体ステータス
    protected XiStatus xiStatus = new XiStatus();
    public class XiStatus
    {
        public int killCount = 0;       //キルカウント数
        public float moveSpeed = 10;    //移動速度
        public float rotateSpeed = 270; //旋回速度
        public Vector3 cameraPosition =
            new Vector3(0, 14, -5);     //カメラ座標
        public int helth = 100;         //体力値
        
    }

    //ストレートウェポンステータス
    protected StraightWeponStatus straightWeponStatus = new StraightWeponStatus();
    public class StraightWeponStatus
    {
        public float fireRate = 0.4f;   //発射レート
        public float bulletSpeed = 20; //弾速
        public float recoil = 20;        //反動
        public float bulletRange = 27;  //弾の射程
        public int damage = 5;          //弾の攻撃力
        public NodeLevel level = new NodeLevel();
        public class NodeLevel
        {
            public int bounce = 0;
            public int through = 0;
            public int bulletSize = 0;
            public int pellet = 0;
            public int spinUp = 0;
            public int missedSplit = 0;
            public int tracking = 0;
            public int range = 0;
            public int bomb = 0;
            public int recoil = 0;
            public int moveload = 0;
            public int rotate = 0;
        }
    }

    //フリックドッジステータス
    protected FlickDodgeStatus flickDodgeStatus = new FlickDodgeStatus();
    public class FlickDodgeStatus
    {
        public int dodgeCount = 3;          //使用回数
        public int maxDodgeCount = 3;       //最大使用数
        public float recastTime = 2;        //現在のリキャストタイム
        public float maxRecastTime = 2;     //最大リキャストタイム
        public float usedCoolDownTime = 0.5f;//使用回数のクールダウン開始までの時間
        public NodeLevel level = new NodeLevel();
        public class NodeLevel
        {
            public int moveRange = 0;
            public int dodgeSpec = 0;
            public int dodgeCount = 0;
            public int coolDown = 0;
            public int attackRange = 0;
            public int drain = 0;
            public int afterImage = 0;
            public int attackimpact = 0;
        }
    }

    //スナイプカノンステータス
    protected SnipeCanonStatus snipeCanonStatus = new SnipeCanonStatus();
    public class SnipeCanonStatus
    {
        public int damage = 50;             //弾の攻撃力
        public float fireRate = 0.5f;       //発射レート
        public int bulletCount = 6;         //残弾
        public int maxBulletCount = 6;      //最大弾数
        public float recastTime = 2;        //現在のリキャストタイム
        public float maxRecastTime = 5;     //最大リキャストタイム
        public float bulletSpeed = 100;     //弾速(固定)
        public float bulletRange = 30;      //弾の射程(固定)

        public NodeLevel level = new NodeLevel();
        public class NodeLevel
        {
            public int bulletCount = 0;
            public int relordTime = 0;
            public int bulletSize = 0;
            public int zoomSize = 0;
            public int areaBomb = 0;
            public int weekBullet = 0;
        }
        public Option option = new Option();
        public class Option
        {
            public bool reverseSide = false;
            public bool reverseVertical = false;
            public bool isSnipepushUpmode = true;
        }
    }

    void Awake()
    {
        save=Resources.Load("SaveData") as SavaScriptableObject2;
    }
}
