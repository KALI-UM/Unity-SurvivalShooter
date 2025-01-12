using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : LivingEntity
{
    public Image hpBar;
    public GameObject restart;

    private PlayerMovement movement;

    public readonly int hashIsDead = Animator.StringToHash("IsDead");
    private Animator animator;

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();

        restart.SetActive(false);
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

    public void RestartLevel()
    {
        restart.SetActive(true);
    }

    public void OnRestart()
    {
    }
}
