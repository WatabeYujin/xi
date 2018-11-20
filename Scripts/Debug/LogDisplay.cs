using UnityEngine;
using UnityEngine.UI;

public class LogDisplay : MonoBehaviour
{
    [SerializeField]
    private Text message = null;

    private void Awake()
    {
        Application.logMessageReceived += HandleLog;
    }

    private void OnDestroy()
    {
        Application.logMessageReceived += HandleLog;
    }

    private void HandleLog(string logText, string stackTrace, LogType type)
    {
        message.text = logText;
    }
}