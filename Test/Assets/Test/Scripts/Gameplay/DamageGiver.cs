using UnityEngine;

public class DamageGiver : MonoBehaviour
{
    [SerializeField] private int _damageAmount = 1;

    private void OnCollisionEnter(Collision collision)
    {
        var otherDamageReceiver = collision.gameObject.GetComponent<DamageReceiver>();

        if (otherDamageReceiver != null)
        {
            otherDamageReceiver.TakeDamage(_damageAmount);
        }

        Destroy(gameObject);
    }

}
