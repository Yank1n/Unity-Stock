using UnityEngine;

public class DamageReceiver : MonoBehaviour
{
    private readonly UnityEngine.Events.UnityEvent _onDeath;

    [SerializeField] private int _hitPoints = 5;

    private int _currentHitPoints;

    private void Awake()
    {
        _currentHitPoints = _hitPoints;
    }

    public void TakeDamage(int damageAmount)
    {
        _currentHitPoints -= damageAmount;

        if (_currentHitPoints <= 0)
        {
            if (_onDeath != null)
            {
                _onDeath.Invoke();
            }

            Destroy(gameObject);
        }
    }
}
