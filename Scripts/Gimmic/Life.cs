using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Life : MonoBehaviour {
    public enum Mode
    {
        Enemy,
        Player
    }
    public Mode mode;
    public int lifePoint = 30;
    public int defense = 0;
    private int maxLife;
    public PlayerDead playerDead;

    private bool life50 = false;
    private bool life20 = false;

    [SerializeField]
    private TalkControl talkControl;

    [SerializeField]
    private List<Renderer> renderers;
    [SerializeField]
    private List<Color> emmisionColors;

    private string emmisionColor = "_EmissionColor";

    void Start () {
        GetRenederer();
        maxLife = lifePoint;
    }

    public Mode GetMode()
    {
        return mode;
    }

    public void Damage(int damage) {
        if (damage < 0) return;
        if (damage - defense <= 0) damage = 1;
        lifePoint -= damage;
        StartCoroutine(DamageFlashEffect(Color.white));
        if (lifePoint <= 0){
            lifePoint = 0;
            Dead();
        }
        if (mode != Mode.Player) return;
        LifeAlret();
        Vibration.Vibrate(damage);
    }

    public void Heal(int healPoint) {
        if (healPoint < 0) return;
        lifePoint += healPoint;
        StartCoroutine(DamageFlashEffect(Color.green));
        if (lifePoint >= maxLife) lifePoint = maxLife;
        if (mode != Mode.Player) return;
        LifeAlret();
    }

    public void LifeMaxUP(int setNewMaxLifePoint) {
        int m_difference = maxLife - setNewMaxLifePoint;
        maxLife = setNewMaxLifePoint;
        lifePoint += m_difference;
    }

    /// <summary>
    /// 死亡時の処理
    /// プレイヤーか敵に分岐
    /// </summary>
    void Dead() {
        switch (mode)
        {
            case Mode.Enemy:
                EnmeyDead();
                break;
            case Mode.Player:
                PlayerDead();
                break;
        }
    }

    /// <summary>
    /// エネミー撃破時の処理
    /// </summary>
    void EnmeyDead() {
        GameObject.Find("Player").GetComponent<PlayerController>().KillCount();
        Destroy(gameObject);
    }

    /// <summary>
    /// プレイヤー被撃破時の処理
    /// </summary>
    void PlayerDead() {
        GetComponent<PlayerController>().ControlStop();
        playerDead.DeadEvent();
    }

    /// <summary>
    /// 現在の体力値取得
    /// </summary>
    /// <returns>現在の体力値</returns>
    public int GetLife()
    {
        return lifePoint;
    }

    /// <summary>
    /// 最大体力値の取得
    /// </summary>
    /// <returns>最大体力値</returns>
    public int GetMaxLife()
    {
        return maxLife;
    }

    /// <summary>
    /// 全回復処理
    /// </summary>
    public void Fullcure()
    {
        lifePoint = maxLife;
        life20 = false;
        life50 = false;
    }

    /// <summary>
    /// 最大ライフの設定
    /// </summary>
    /// <param name="maxLifeValue">設定するライフ値</param>
    public void SetMaxLife(int maxLifeValue)
    {
        maxLife = maxLifeValue;
        lifePoint = maxLifeValue;
    }

    void LifeAlret()
    {
        const string m_50alret = "体力値残り半分を切りました。警戒を。";
        const string m_20alret = "警告。体力値残り僅かです。[%p]。";
        if (lifePoint <= maxLife/2)
        {
            if (lifePoint <= maxLife / 3)
            {
                if (life20) return;
                life20 = true;
                talkControl.TalkSet(m_20alret, 1.6f, 1, 1.2f, true);
            }
            else
            {
                life20 = false;
                if (life50) return;
                life50 = true;
                talkControl.TalkSet(m_50alret, 1.4f, 1, 1.1f, true);
            }
        }
        else life50 = false;
    }

    private void GetRenederer()
    {
        foreach (Renderer m_renderer in GetComponentsInChildren<Renderer>())
        {
            if (!m_renderer.material.HasProperty(emmisionColor))
                return;
            renderers.Add(m_renderer);
            emmisionColors.Add(m_renderer.material.GetColor(emmisionColor));
        }
    }

    IEnumerator DamageFlashEffect(Color flashColor)
    {
        foreach(Renderer m_renderer in renderers)
        {
            m_renderer.material.SetColor(emmisionColor,Color.white);
        }   
        yield return new WaitForEndOfFrame();
        for (int i=0;i<renderers.Count;i++)
        {
            renderers[i].material.SetColor(emmisionColor, emmisionColors[i]);
        }
    }
}
