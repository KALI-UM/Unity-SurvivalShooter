using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public ParticleSystem fireEffect;
    public AudioClip fireSfx;

    private AudioSource audioSource;

    public int damage = 30;
    public Transform firePoint;
    public float fireDistance = 20f;
    public float fireInterval = 2f;

    private LineRenderer lineRenderer;

    public static readonly string fireButtonName = "Fire1";
    private float lastFireTime = 0;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (Time.time > lastFireTime + fireInterval && Input.GetButtonDown(fireButtonName))
        {
            lastFireTime = Time.time;
            Fire();
        }

    }

    private void Fire()
    {
        Vector3 endPos;
        var ray = new Ray(firePoint.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, fireDistance))
        {
            endPos = hit.point;

            var damageable = hit.collider.GetComponent<LivingEntity>();
            if (damageable != null&&!damageable.IsDead)
            {
                damageable.OnDamage(damage, hit.point, hit.normal);
            }
        }
        else
        {
            endPos = firePoint.position + firePoint.forward * fireDistance;
        }

        StartCoroutine(FireEffect(endPos));

        IEnumerator FireEffect(Vector3 hitPoint)
        {
            audioSource.PlayOneShot(fireSfx);
            fireEffect.Play();

            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, firePoint.position); // 시작점
            lineRenderer.SetPosition(1, endPos); // 끝점


            yield return new WaitForSeconds(0.03f);
            lineRenderer.enabled = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Ray ray = new(firePoint.position, firePoint.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, fireDistance))
        {
            Gizmos.DrawLine(firePoint.position, hit.point);
        }
    }
}
