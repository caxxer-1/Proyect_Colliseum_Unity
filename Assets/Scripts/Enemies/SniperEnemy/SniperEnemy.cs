using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class SniperEnemy : Enemy
{
    [SerializeField] GameObject sniperProyectile;
    bool canShoot = true;
    void Start()
    {
        enemyState = EnemyState.Chasing;
        navMeshAgent = GetComponent<NavMeshAgent>();
        health = 2;
        maxHealth = 2;
        damage = 100;
        UpdateHealthBar();
    }
    void Update()
    {
        if (Player.Instance == null) return;
        switch (enemyState)
        {
            case EnemyState.Chasing:
                navMeshAgent.speed = 1.5f;
                navMeshAgent.destination = Player.Instance.transform.position;
            break;
            case EnemyState.Attacking:
                navMeshAgent.speed = 0;
                if (!canShoot) return;
                StartCoroutine(Shoot());
            break;
        }
    }
    IEnumerator Shoot()
    {
        Instantiate(sniperProyectile.transform, transform);
        canShoot = false;
        yield return new WaitForSeconds(1);
        canShoot = true;

    }
    public void SetSniperEnemyState(EnemyState sniperEnemyState)
    {
        enemyState = sniperEnemyState;
    }
    public NoticerSO GetGameNoticer()
    {
        return gameNoticer;
    }
    public float GetSniperDamage()
    {
        return damage;
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
}
