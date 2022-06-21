using System.Collections;
using UnityEngine;

public class ReturnAfterDelay : MonoBehaviour, IObjectPoolNotifier
{
    public void OnCreatedOrDequeuedFromPool(bool created)
    {
        Debug.Log("Dequeued from object pool!");

        StartCoroutine(DoReturnAfterDelay());
    }

    public void OnEnqueuedToPool()
    {
        Debug.Log("Enqueued to object pool!");
    }

    IEnumerator DoReturnAfterDelay()
    {
        yield return new WaitForSeconds(1.0f);
        gameObject.ReturnToPool();
    }
}
