using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : LivingEntity
{
    public Image hpBar;

    private PlayerMovement movement;

    public readonly int hashIsDead = Animator.StringToHash("IsDead");
    private Animator animator;

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();
    }

    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        base.OnDamage(damage, hitPoint, hitNormal);
        hpBar.fillAmount = Hp / maxHp;
    }


    public override void Die()
    {
        base.Die();
        movement.enabled = false;
        animator.SetTrigger(hashIsDead);
    }

}
