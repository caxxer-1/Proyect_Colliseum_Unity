using System;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : HaveHealthBar, IKillable
{
    public event EventHandler<EnemyState> OnEnemyStateChanged;
    [SerializeField] protected NoticerSO gameNoticer;
    protected NavMeshAgent navMeshAgent;
    protected EnemyState enemyState;
    protected float damage;
    public void CallOnEnemyStateChangedEvent(EnemyState eventArgs)
    {
        OnEnemyStateChanged?.Invoke(this, eventArgs);
    }
    public void Die()
    {
        Destroy(gameObject);
    }
    public enum EnemyState
    {
        Idle,
        Chasing,
        Attacking
    }
}
