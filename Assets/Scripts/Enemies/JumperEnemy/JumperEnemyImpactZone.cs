using UnityEngine;

public class JumperEnemyImpactZone : MonoBehaviour
{
    [SerializeField] JumperEnemy jumperEnemy;
    CapsuleCollider capsuleCollider;
    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        capsuleCollider.enabled = false;
    }
    void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<Player>() == null) return;
        jumperEnemy.CallPlayerHitted();
    }
    public void SwitchColliderState(bool enabled)
    {
        if (enabled) capsuleCollider.enabled = true;
        else capsuleCollider.enabled = false;
    }
}
