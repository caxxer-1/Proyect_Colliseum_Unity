using System;
using System.Collections;
using UnityEngine;

public class PlayerAttackAnimations : MonoBehaviour
{
    Animator playerAttackAnimator;
    void Start()
    {
        playerAttackAnimator = GetComponent<Animator>();
        InputManager.Instance.OnAttackPerformed += InputManager_PlayerAttacked;
    }
    void InputManager_PlayerAttacked(object sender, EventArgs e)
    {
        if (playerAttackAnimator == null) return;
        playerAttackAnimator.SetBool("Attacked", true);
        StartCoroutine(AttackCooldown());
    }
    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(.1f);
        playerAttackAnimator.SetBool("Attacked", false);
    }
}
