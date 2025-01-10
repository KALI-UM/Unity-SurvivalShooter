using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : LivingEntity
{
    public ParticleSystem hitEffect;
    public AudioClip hitSfx;

    private AudioSource audioSource;
    private EnemyMovement movement;
    public readonly int hashIsDead = Animator.StringToHash("IsDead");
    private Animator animator;
    public float sinkingTime = 10f;

    public Action onReturn;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        movement = GetComponent<EnemyMovement>();
        animator = GetComponent<Animator>();

    }

    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        base.OnDamage(damage, hitPoint, hitNormal);

        hitEffect.transform.position = hitPoint;
        hitEffect.transform.rotation = Quaternion.LookRotation(hitNormal);
        hitEffect.Play();
        audioSource.PlayOneShot(hitSfx);
    }

    public override void Die()
    {
        base.Die();
        movement.enabled = false;
        animator.SetTrigger(hashIsDead);
    }

    public void StartSinking()
    {
        var sinkingPosition = transform.position + new Vector3(0, -5, 0);

        StartCoroutine(Sinking(sinkingPosition));

        IEnumerator Sinking(Vector3 newPosition)
        {
            Vector3 startPosition = transform.position;
            float elapsedTime = 0;

            while (elapsedTime < sinkingTime)
            {
                transform.position = Vector3.Lerp(startPosition, newPosition, elapsedTime / sinkingTime);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.position = newPosition;
            
            onReturn?.Invoke();
        }
    }
}