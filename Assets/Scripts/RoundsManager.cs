using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RoundsManager : MonoBehaviour
{
    public static RoundsManager Instance {get; private set;}
    public event EventHandler<int> OnRoundStarted;
    [SerializeField] GameUI gameUI;
    [SerializeField] NoticerSO gameNoticer;
    [SerializeField] Transform basicEnemyPrefab;
    [SerializeField] Transform sniperEnemyPrefab;
    [SerializeField] Transform jumperEnemyPrefab;
    List<Enemy> enemiesList = new List<Enemy>();
    System.Random random = new System.Random();
    int cuantityOfEnemiesCurrentRound = 0;
    int currentRound = 89;
    bool roundFinished = false;
    float cooldownForNextRound = 5;
    float cooldownForNextRoundMax = 5;
    int previousRoundNumOfEnemies = 0;
    int enemiesDefeatedCount = 0;
    int aliveEnemiesCount = 0;
    float percentageOfBasicEnemies;
    float percentageOfSniperEnemies;
    float percentageOfJumperEnemies;
    //BOOSTS SPAWNING
    [SerializeField] Transform speedBoostPrefab;
    [SerializeField] Transform shieldBoostPrefab;
    [SerializeField] Transform healthBoostPrefab;
    [SerializeField] Transform damageBoostPrefab;
    List<GameObject> currentBoosts = new List<GameObject>();
    bool spawningBoost = false;
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        Instance = this;
    }
    void Start()
    {
        gameNoticer.OnMessageSent += NoticerSO_ManageMessage;
        gameUI.ChangeGameStateInfoTextEnabledState(false);
        StartNewRound();
    }
    void Update()
    {
        if (!roundFinished)
        {
            if (spawningBoost) return;
            StartCoroutine(SpawnBoost());
        }
        else
        {
            gameUI.ChangeGameStateInfoTextEnabledState(true);
            cooldownForNextRound -= Time.deltaTime;
            gameUI.RefreshCooldownTimerText((int)Math.Round(cooldownForNextRound));
            if (cooldownForNextRound <= 0)
            {
                roundFinished = false;
                gameUI.ChangeGameStateInfoTextEnabledState(false);
                cooldownForNextRound = cooldownForNextRoundMax;
                if (currentRound == 100)
                {
                    StartCoroutine(LoadMainMenu("YOU WIN!!! :D"));
                }
                else
                {
                    StartNewRound();
                }
            }
    }    }
    void StartNewRound()
    {
        currentRound++;
        if (currentRound >= 1 && currentRound <= 10)
        {
            cuantityOfEnemiesCurrentRound = previousRoundNumOfEnemies + Mathf.CeilToInt(currentRound * 2);
            percentageOfBasicEnemies = .8f;
            percentageOfSniperEnemies = .2f;
            percentageOfJumperEnemies = 0;
        }
        else if (currentRound > 10 && currentRound <= 20)
        {
            cuantityOfEnemiesCurrentRound = previousRoundNumOfEnemies + currentRound;
            percentageOfBasicEnemies = .5f;
            percentageOfSniperEnemies = .3f;
            percentageOfJumperEnemies = .2f;
        }
        else if (currentRound > 20 && currentRound <= 30)
        {
            cuantityOfEnemiesCurrentRound = previousRoundNumOfEnemies + Mathf.CeilToInt(currentRound / 2);
            percentageOfBasicEnemies = .3f;
            percentageOfSniperEnemies = .4f;
            percentageOfJumperEnemies = .3f;
        }
        else if (currentRound > 30 && currentRound <= 40)
        {
            cuantityOfEnemiesCurrentRound = previousRoundNumOfEnemies + 5;
            percentageOfBasicEnemies = .2f;
            percentageOfSniperEnemies = .5f;
            percentageOfJumperEnemies = .3f;
        }
        else
        {
            cuantityOfEnemiesCurrentRound = previousRoundNumOfEnemies + 2;
            percentageOfBasicEnemies = .1f;
            percentageOfSniperEnemies = .5f;
            percentageOfJumperEnemies = .4f;
        }
        int cuantityOfBasicEnemies = Mathf.RoundToInt(percentageOfBasicEnemies * cuantityOfEnemiesCurrentRound);
        int cuantityOfSniperEnemies = Mathf.RoundToInt(percentageOfSniperEnemies * cuantityOfEnemiesCurrentRound);
        int cuantityOfJumperEnemies = Mathf.RoundToInt(percentageOfJumperEnemies * cuantityOfEnemiesCurrentRound);
        for (int i = 0; i < cuantityOfBasicEnemies; i++)
        {
            Vector3 spawnPos = GetValidSpawnPos();
            Transform basicEnemyTransform = Instantiate(basicEnemyPrefab, spawnPos, Quaternion.identity);
            basicEnemyTransform.GetComponent<NavMeshAgent>().Warp(spawnPos);
            enemiesList.Add(basicEnemyTransform.GetComponent<BasicEnemy>());
        }
        for (int i = 0; i < cuantityOfSniperEnemies; i++)
        {
            Vector3 spawnPos = GetValidSpawnPos();
            Transform sniperEnemyTransform = Instantiate(sniperEnemyPrefab, spawnPos, Quaternion.identity);
            sniperEnemyTransform.GetComponent<NavMeshAgent>().Warp(spawnPos);
            enemiesList.Add(sniperEnemyTransform.GetComponent<SniperEnemy>());
        }
        for (int i = 0; i < cuantityOfJumperEnemies; i++)
        {
            Vector3 spawnPos = GetValidSpawnPos();
            Transform jumperEnemyTransform = Instantiate(jumperEnemyPrefab, spawnPos, Quaternion.identity);
            jumperEnemyTransform.GetComponent<NavMeshAgent>().Warp(spawnPos);
            enemiesList.Add(jumperEnemyTransform.GetComponent<JumperEnemy>());
        }
        previousRoundNumOfEnemies = cuantityOfBasicEnemies + cuantityOfSniperEnemies + cuantityOfJumperEnemies;
        aliveEnemiesCount = cuantityOfEnemiesCurrentRound;
        OnRoundStarted?.Invoke(this, currentRound);
    }
    public int GetEnemiesDefeatedCount()
    {
        return enemiesDefeatedCount;
    }
    public int GetAliveEnemiesCount()
    {
        return aliveEnemiesCount;
    }
    IEnumerator LoadMainMenu(string gameStateText)
    {
        gameUI.ChangeGameStateInfoTextEnabledState(true);
        gameUI.SetGameFinalStateText(gameStateText);
        yield return new WaitForSeconds(5);
        SceneLoader.LoadScene(SceneLoader.Scene.MainMenu);
    }
    IEnumerator SpawnBoost()
    {
        spawningBoost = true;
        float spawnBoostCooldown;
        if (currentRound >= 1 && currentRound <= 10)
        {
            spawnBoostCooldown = 12.5f;
        }
        else if (currentRound > 10 && currentRound <= 20)
        {
            spawnBoostCooldown = 10;
        }
        else if (currentRound > 20 && currentRound <= 30)
        {
            spawnBoostCooldown = 7.5f;
        }
        else if (currentRound > 30 && currentRound <= 40)
        {
            spawnBoostCooldown = 5;
        }
        else
        {
            spawnBoostCooldown = 2.5f;
        }
        yield return new WaitForSeconds(spawnBoostCooldown);
        Vector3 spawnPos = GetValidSpawnPos();
        switch (random.Next(0, 4))
        {
            case 0:
                Transform speedBoostTransform = Instantiate(speedBoostPrefab, new Vector3(spawnPos.x, speedBoostPrefab.transform.position.y, spawnPos.z), Quaternion.identity);
                currentBoosts.Add(speedBoostTransform.gameObject);
            break;
            case 1:
                Transform shieldBoostTransform = Instantiate(shieldBoostPrefab, new Vector3(spawnPos.x, shieldBoostPrefab.transform.position.y, spawnPos.z), Quaternion.identity);
                currentBoosts.Add(shieldBoostTransform.gameObject);
            break;
            case 2:
                Transform healthBoostTransform = Instantiate(healthBoostPrefab, new Vector3(spawnPos.x, healthBoostPrefab.transform.position.y, spawnPos.z), Quaternion.identity);
                currentBoosts.Add(healthBoostTransform.gameObject);
            break;
            case 3:
                Transform damageBoostTransform = Instantiate(damageBoostPrefab, new Vector3(spawnPos.x, damageBoostPrefab.transform.position.y, spawnPos.z), Quaternion.identity);
                currentBoosts.Add(damageBoostTransform.gameObject);
            break;
        }
        spawningBoost = false;
    }
    void NoticerSO_ManageMessage(object sender, OnMessageSentBasicBuild e)
    {
        switch (e.typeOfMessage)
        {
            case TypeOfMessage.EnemyDefeated:
                EnemyDefeated(e.gameObject.GetComponent<IKillable>());
            break;
            case TypeOfMessage.PlayerHitted:
                PlayerHitted(e.gameObject.GetComponent<Player>(), e.damageToDealPlayer);
            break;
            case TypeOfMessage.PlayerDefeated:
                PlayerDefeated(e.gameObject.GetComponent<IKillable>());
            return;
            case TypeOfMessage.BoostPickedUp:
                BoostPickedUp(e.gameObject);
            return;
        }
    }
    void BoostPickedUp(GameObject boost)
    {
        currentBoosts.Remove(boost);
        Destroy(boost);
    }
    void EnemyDefeated(IKillable enemy)
    {
        enemiesDefeatedCount++;
        aliveEnemiesCount--;
        enemiesList.Remove((Enemy)enemy);
        enemy.Die();
        if (aliveEnemiesCount == 0) roundFinished = true;
    }
    void PlayerHitted(Player player, float damage)
    {
        player.TakeDamage(damage);
        if (player.GetPlayerHealth() <= 0) PlayerDefeated(player);
    }
    void PlayerDefeated(IKillable player)
    {
        foreach (Enemy enemy in enemiesList)
        {
            enemy.Die();
        }
        player.Die();
        StartCoroutine(LoadMainMenu("GAME OVER :("));
    }
    Vector3 GetValidSpawnPos()
    {
        for (int i = 0; i < 30; i++)
        {
            if (NavMesh.SamplePosition(new Vector3(random.Next(-48, 48), 0, random.Next(-48, 48)), out NavMeshHit hit, 3, NavMesh.AllAreas))
            {
                return hit.position;
            }
        }
        return Vector3.zero;
    }
    void OnDestroy()
    {
        gameNoticer.OnMessageSent -= NoticerSO_ManageMessage;
    }
}
