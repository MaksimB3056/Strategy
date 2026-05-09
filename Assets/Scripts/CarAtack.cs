using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CarAtack : MonoBehaviour
{
    [NonSerialized] public int _health = 100;
    
    public float radiuss = 70f;
    public GameObject bullet;
    private Coroutine _coroutine = null;
    private void Update()
    {
        DetectCollision();
    }

    private void DetectCollision()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radiuss);

        if (hitColliders.Length == 0 && _coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
            if (gameObject.CompareTag("Enemy"))
            {
                GetComponent<NavMeshAgent>().SetDestination(gameObject.transform.position);
            }
        }
        foreach(var el in hitColliders)
        {
            if((gameObject.CompareTag("Player") && el.CompareTag("Enemy")) ||
               (gameObject.CompareTag("Enemy") && el.CompareTag("Player")))
            {
                if (gameObject.CompareTag("Enemy"))
                {
                    GetComponent<NavMeshAgent>().SetDestination(el.transform.position);
                }
                if(_coroutine == null)
                {
                    _coroutine = StartCoroutine(StartAtack(el));
                }
            }
        }
    }

    IEnumerator StartAtack(Collider enemyPos)
    { 
        GameObject obj = Instantiate(bullet, transform.GetChild(1).position, Quaternion.identity);
        obj.GetComponent<BulletControl>().position = enemyPos.transform.position;
        yield return new WaitForSeconds(1f);
        StopCoroutine(_coroutine);
        _coroutine = null;
    }
}
