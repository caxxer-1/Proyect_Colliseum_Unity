using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    [SerializeField] NoticerSO gameNoticer;
    [SerializeField] TMP_Text roundText;
    [SerializeField] TMP_Text gameStateInfoText;
    [SerializeField] TMP_Text enemiesDefeatedCountText;
    [SerializeField] TMP_Text aliveEnemiesCountText;
    void Start()
    {
        gameNoticer.OnMessageSent += GameNoticer_ManageMessages;
        if (GameManager.Instance == null || Player.Instance == null)
        {
            Debug.LogError("GameManager.Instance o Player.Instance no estan disponibles para GameUI.");
            enabled = false;
            return;
        }
        RoundsManager.Instance.OnRoundStarted += GameManager_RoundStarted;
        roundText.text = "Round 1";
        enemiesDefeatedCountText.text = $"Enemies defeated: {RoundsManager.Instance.GetEnemiesDefeatedCount()}";
        aliveEnemiesCountText.text = $"Alive enemies left: {RoundsManager.Instance.GetAliveEnemiesCount()}";
    }
    void GameNoticer_ManageMessages(object sender, OnMessageSentBasicBuild e)
    {
        switch (e.typeOfMessage)
        {
            case TypeOfMessage.EnemyDefeated:
                enemiesDefeatedCountText.text = $"Enemies defeated: {RoundsManager.Instance.GetEnemiesDefeatedCount()}";
                aliveEnemiesCountText.text = $"Alive enemies left: {RoundsManager.Instance.GetAliveEnemiesCount()}";
            break;
        }
    }
    void GameManager_RoundStarted(object sender, int e)
    {
        roundText.text = $"Round {e}";
    }
    public void ChangeGameStateInfoTextEnabledState(bool enabled)
    {
        if (enabled) gameStateInfoText.enabled = true;
        else gameStateInfoText.enabled = false;
    }
    public void SetGameFinalStateText(string finalStateText)
    {
        gameStateInfoText.text = finalStateText;
    }
    public void RefreshCooldownTimerText(int cooldownTimer)
    {
        gameStateInfoText.text = cooldownTimer.ToString();
    }

    void OnDestroy()
    {
        gameNoticer.OnMessageSent -= GameNoticer_ManageMessages;

        if (RoundsManager.Instance == null) return;
        RoundsManager.Instance.OnRoundStarted -= GameManager_RoundStarted;
    }
}
