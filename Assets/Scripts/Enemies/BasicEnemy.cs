using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BasicEnemy : Enemy
{
    bool idleCooldownCoroutineFired;
    bool chasingCooldownCoroutineFired;
    void Start()
    {
        enemyState = EnemyState.Chasing;
        navMeshAgent = GetComponent<NavMeshAgent>();
        health = 1;
        maxHealth = 1;
        damage = 50;
        UpdateHealthBar();
    }
    void Update()
    {
        if (Player.Instance == null) return;
        switch (enemyState)
        {
            case EnemyState.Idle:
                if (idleCooldownCoroutineFired) return;
                StartCoroutine(IdleCooldown());
                idleCooldownCoroutineFired = true;
            break;
            case EnemyState.Chasing:
                navMeshAgent.speed = 4;
                navMeshAgent.destination = Player.Instance.transform.position;
                if (chasingCooldownCoroutineFired) return;
                StartCoroutine(ChasingCooldown());
                chasingCooldownCoroutineFired = true;
            break;
        }
        
    }
    IEnumerator IdleCooldown()
    {
        yield return new WaitForSeconds(1.5f);
        navMeshAgent.speed = 4;
        enemyState = EnemyState.Chasing;
        CallOnEnemyStateChangedEvent(EnemyState.Chasing);
        idleCooldownCoroutineFired = false;
    }
    IEnumerator ChasingCooldown()
    {
        yield return new WaitForSeconds(5);
        navMeshAgent.speed = 0;
        enemyState = EnemyState.Idle;
        CallOnEnemyStateChangedEvent(EnemyState.Idle);
        chasingCooldownCoroutineFired = false;
    }
    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("PlayerDamageCube")) return;
        health -= Player.Instance.GetPlayerDamage();
        UpdateHealthBar();
        if (health <= 0)
        {
            gameNoticer.Call(new OnMessageSentBasicBuild
            {
                typeOfMessage = TypeOfMessage.EnemyDefeated,
                gameObject = gameObject
            });
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Player>() == null) return;

        gameNoticer.Call(new OnMessageSentBasicBuild
        {
            typeOfMessage = TypeOfMessage.PlayerHitted,
            gameObject = Player.Instance.gameObject,
            damageToDealPlayer = damage * Player.Instance.GetPlayerShield()
        });
    }
}
