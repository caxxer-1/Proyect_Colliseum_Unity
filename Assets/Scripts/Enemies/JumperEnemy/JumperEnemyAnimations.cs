using UnityEngine;

public class JumperEnemyAnimations : MonoBehaviour
{
    [SerializeField] JumperEnemy jumperEnemy;
    Animator jumperEnemyAnimator;

    void Start()
    {
        jumperEnemyAnimator = GetComponent<Animator>();
        jumperEnemy.OnJumperEnemyStateChange += JumperEnemy_StateChanged;
    }
    void JumperEnemy_StateChanged(object sender, JumperEnemy.JumperEnemyState e)
    {
        switch (e)
        {
            case JumperEnemy.JumperEnemyState.Chasing:
                jumperEnemyAnimator.SetBool("StuckInFloor", false);
            break;
            case JumperEnemy.JumperEnemyState.PreparingJump:
                jumperEnemyAnimator.SetBool("PrepareJump", true);
            break;
            case JumperEnemy.JumperEnemyState.Jumping:
                jumperEnemyAnimator.SetBool("PrepareJump", false);
                jumperEnemyAnimator.SetBool("Jump", true);
            break;
            case JumperEnemy.JumperEnemyState.StuckedInFloor:
                jumperEnemyAnimator.SetBool("Jump", false);
                jumperEnemyAnimator.SetBool("StuckInFloor", true);
            break;
        }
    }
}
