using System.Collections;
using UnityEngine;

public class ObjectPoolDemo : MonoBehaviour
{
    [SerializeField] private ObjectPool _pool;

    private IEnumerator Start()
    {
        while (true)
        {
            var obj = _pool.GetObject();

            var position = Random.insideUnitSphere * 4;

            obj.transform.position = position;

            var delay = Random.Range(0.1f, 0.5f);

            yield return new WaitForSeconds(delay);
        }
    }
}
