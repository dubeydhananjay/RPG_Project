using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using RPG.RPGResources;


public class Projectile : MonoBehaviour
{
    private HealthComponent target;
    private float damage;
    [SerializeField] private float speed = 3;
    [SerializeField] private bool homing;
    [SerializeField] private GameObject hitImpactPrefab;
    [SerializeField] private float maxLifeTime = 10;
    [SerializeField] private float lifeAfterImpact = 0.2f;
    [SerializeField] private GameObject[] destroyOnHit;
    [SerializeField] private UnityEvent OnHit;
    private GameObject instigator;

    private void Update()
    {
        if (target)
        {
            if (homing && !target.AlreadyDied)
                transform.LookAt(GetTargetPos());
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }

    public void SetTarget(HealthComponent target, float damage, GameObject instigator)
    {
        this.target = target;
        this.damage = damage;
        this.instigator = instigator;
        transform.LookAt(GetTargetPos());
        Destroy(gameObject, maxLifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<HealthComponent>() == target && !target.AlreadyDied)
        {
            OnHit?.Invoke();
            speed = 0;
            target.TakeDamage(instigator,damage);
            Instantiate(hitImpactPrefab, transform.position, Quaternion.identity);
            foreach (GameObject go in destroyOnHit)
                Destroy(go);
            Destroy(gameObject, lifeAfterImpact);

        }
    }

    private Vector3 GetTargetPos()
    {
        CapsuleCollider targetCollider = target.GetComponent<CapsuleCollider>();
        Vector3 targetPos = target.transform.position;
        if (targetCollider)
            targetPos = target.transform.position + Vector3.up * targetCollider.height / 2;
        return targetPos;
    }


}
