using UnityEngine;

public class SniperEnemyAnimations : MonoBehaviour
{
    Animator sniperEnemyAnimator;
    [SerializeField] SniperEnemy sniperEnemy;
    void Start()
    {
        sniperEnemyAnimator = GetComponent<Animator>();
        sniperEnemy.OnEnemyStateChanged += BasicEnemy_OnStateChanged;
    }
    void BasicEnemy_OnStateChanged(object sender, Enemy.EnemyState e)
    {
        if (e == Enemy.EnemyState.Attacking)
        {
            sniperEnemyAnimator.SetBool("Chasing", false);
            sniperEnemyAnimator.SetBool("Attacking", true);
        }
        else
        {
            sniperEnemyAnimator.SetBool("Attacking", false);
            sniperEnemyAnimator.SetBool("Chasing", true);
        }
    }
}
