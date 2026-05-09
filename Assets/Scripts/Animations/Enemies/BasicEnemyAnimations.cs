using UnityEngine;

public class BasicEnemyAnimations : MonoBehaviour
{
    Animator basicEnemyAnimator;
    [SerializeField] BasicEnemy basicEnemy;
    void Start()
    {
        basicEnemyAnimator = GetComponent<Animator>();
        basicEnemy.OnEnemyStateChanged += BasicEnemy_OnStateChanged;
    }
    void BasicEnemy_OnStateChanged(object sender, Enemy.EnemyState e)
    {
        if (e == Enemy.EnemyState.Idle)
        {
            basicEnemyAnimator.SetBool("Chasing", false);
            basicEnemyAnimator.SetBool("Idle", true);
        }
        else
        {
            basicEnemyAnimator.SetBool("Idle", false);
            basicEnemyAnimator.SetBool("Chasing", true);
        }
    }
}