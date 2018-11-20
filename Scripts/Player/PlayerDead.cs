using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDead : MonoBehaviour {
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private Image fadeImage;
    [SerializeField]
    private Color fadeColor;
    [SerializeField]
    private RimitTime rimitTime;
    [SerializeField]
    private PlayerController playerController;
    [SerializeField]
    private TalkControl talkControl;

    private Vector3 respawnPosition = new Vector3(0, 0, 0);

    public void DeadEvent()
    {
        StartCoroutine(EnumeratorDeadEvent());
    }

    IEnumerator EnumeratorDeadEvent()
    {
        const string m_deadText = "撃破されました。再アクセスを試みます。";
        talkControl.TalkSet(m_deadText,0.8f,0.7f,1,true);
        playerController.ControlStop();
        fadeImage.color = fadeColor;
        transform.tag = "Untagged";
        yield return StartCoroutine(DeadAnimation());
        yield return StartCoroutine(FadeIn(0.5f));
        rimitTime.TimeSecrease();
        transform.position = respawnPosition;
        GetComponent<Life>().Fullcure();
        yield return StartCoroutine(FadeOut(0.5f));
        transform.tag = "Player";
        playerController.ControlStart();
    }

    IEnumerator DeadAnimation() {
        const float m_deadAnimWait =0.7f;
        animator.SetFloat("X", 0);
        animator.SetFloat("Z", 0);
        animator.Play("Dead");
        yield return new WaitForSeconds(m_deadAnimWait);

    }
    

    void RespawnPositionSet(Vector3 position)
    {
        Vector3 m_position = new Vector3(position.x,position.y+1,position.z);
        respawnPosition = m_position;
    }


    IEnumerator FadeIn(float time)
    {
        fadeImage.enabled = true;
        Material m_material = fadeImage.material;
        float current = 0;
        while (current < time)
        {
            m_material.SetFloat("_Alpha", current / time);
            yield return new WaitForEndOfFrame();
            current += Time.deltaTime;
        }
        m_material.SetFloat("_Alpha", 1);
    }

    IEnumerator FadeOut(float time)
    {
        fadeImage.enabled = true;
        Material m_material = fadeImage.material;
        float current = 0;
        while (current < time)
        {
            m_material.SetFloat("_Alpha", 1 - current / time);
            yield return new WaitForEndOfFrame();
            current += Time.deltaTime;
        }
        m_material.SetFloat("_Alpha", 0);
        fadeImage.enabled = false;
    }
}
