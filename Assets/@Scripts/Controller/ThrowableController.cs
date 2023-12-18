using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableController : MonoBehaviour
{
    private bool hasCollided = false;

    private void OnCollisionEnter(Collision collision)
    {
        // 뭔가에 충돌하면 5초 후에 Destroy
        hasCollided = true;
        StartCoroutine(DestroyAfterDelay(5f));
    }

    private void Update()
    {
        // 만약 충돌이 없다면(바닥 밖으로 떨어지는 경우) 10초 후에 Destroy
        if (!hasCollided)
        {
            StartCoroutine(DestroyAfterDelay(10f));
        }
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
