using UnityEngine;

public class JumperEnemyAreaOfVision : MonoBehaviour
{
    JumperEnemy jumperEnemy;
    void Start()
    {
        jumperEnemy = transform.parent.GetComponent<JumperEnemy>();
    }
    void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<Player>() == null || jumperEnemy.GetJumperState() != JumperEnemy.JumperEnemyState.Chasing) return;
        jumperEnemy.SetJumpState(JumperEnemy.JumperEnemyState.PreparingJump);
    }
}
