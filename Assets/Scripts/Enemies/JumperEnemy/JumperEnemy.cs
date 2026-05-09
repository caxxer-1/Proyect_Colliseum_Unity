using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class JumperEnemy : Enemy
{
    public event EventHandler<JumperEnemyState> OnJumperEnemyStateChange;
    [SerializeField] JumperEnemyImpactZone jumpImpactZone;
    JumperEnemyState jumpState;
    bool preparingJump = false;
    bool jumping = false;
    bool stucked = false;
    void Start()
    {
        jumpState = JumperEnemyState.Chasing;
        navMeshAgent = GetComponent<NavMeshAgent>();
        health = 50;
        maxHealth = 50;
        damage = 300;
        UpdateHealthBar();
    }
    void Update()
    {
        if (Player.Instance == null) return;
        switch (jumpState)
        {
            case JumperEnemyState.Chasing:
                navMeshAgent.destination = Player.Instance.transform.position;
            break;
            case JumperEnemyState.PreparingJump:
                jumpImpactZone.gameObject.SetActive(true);
                jumpImpactZone.transform.position = Player.Instance.transform.position;
                if (preparingJump) return;
                StartCoroutine(PreparingJump());
            break;
            case JumperEnemyState.Jumping:
                if (jumping) return;
                StartCoroutine(Jump());
            break;
            case JumperEnemyState.StuckedInFloor:
                if (stucked) return;
                jumpImpactZone.transform.position = transform.position;
                StartCoroutine(StuckedInFloor());
            break;
        }
    }
    public void SetJumpState(JumperEnemyState newJumpState)
    {
        jumpState = newJumpState;
        OnJumperEnemyStateChange?.Invoke(this, newJumpState);
    }
    IEnumerator PreparingJump()
    {
        preparingJump = true;
        navMeshAgent.speed = 0;
        yield return new WaitForSeconds(4.2f);
        jumpState = JumperEnemyState.Jumping;
        OnJumperEnemyStateChange?.Invoke(this, JumperEnemyState.Jumping);
        preparingJump = false;
    }
    IEnumerator Jump()
    {
        jumping = true;
        yield return new WaitForSeconds(.8f);
        jumpImpactZone.SwitchColliderState(true);
        transform.position = jumpImpactZone.transform.position;
        jumpState = JumperEnemyState.StuckedInFloor;
        OnJumperEnemyStateChange?.Invoke(this, JumperEnemyState.StuckedInFloor);
        jumping = false;
    }
    IEnumerator StuckedInFloor()
    {
        stucked = true;
        StartCoroutine(DisableImpactZoneColliderCooldown());
        yield return new WaitForSeconds(3);
        navMeshAgent.speed = 3;
        jumpState = JumperEnemyState.Chasing;
        OnJumperEnemyStateChange?.Invoke(this, JumperEnemyState.Chasing);
        stucked = false;
    }
    IEnumerator DisableImpactZoneColliderCooldown()
    {
        yield return new WaitForSeconds(.01f);
        jumpImpactZone.SwitchColliderState(false);
        jumpImpactZone.gameObject.SetActive(false);
    }
    public void CallPlayerHitted()
    {
        gameNoticer.Call(new OnMessageSentBasicBuild
        {
            typeOfMessage = TypeOfMessage.PlayerHitted,
            gameObject = Player.Instance.gameObject,
            damageToDealPlayer = damage * Player.Instance.GetPlayerShield()
        });
    }
    public JumperEnemyState GetJumperState()
    {
        return jumpState;
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
    public enum JumperEnemyState
    {
        Chasing,
        PreparingJump,
        Jumping,
        StuckedInFloor
    }
}
