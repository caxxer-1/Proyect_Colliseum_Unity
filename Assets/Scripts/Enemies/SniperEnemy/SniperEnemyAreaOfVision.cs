using UnityEngine;

public class SniperEnemyAreaOfVision : MonoBehaviour
{
    [SerializeField] SniperEnemy sniperEnemy;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Player>() == null) return;
        sniperEnemy.SetSniperEnemyState(Enemy.EnemyState.Attacking);
        sniperEnemy.CallOnEnemyStateChangedEvent(Enemy.EnemyState.Attacking);
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Player>() == null) return;
        sniperEnemy.SetSniperEnemyState(Enemy.EnemyState.Chasing);
        sniperEnemy.CallOnEnemyStateChangedEvent(Enemy.EnemyState.Chasing);
    }
}
