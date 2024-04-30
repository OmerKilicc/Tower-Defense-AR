using System;
using System.Collections;
using UnityEngine;

public class Ball : MonoBehaviour, IProjectile
{
    [SerializeField] private int _bulletDamage = 40;
    private Vector3 _targetPosition;

    private float _speed = 20.0f; // Adjusted for better control
    private float _firingAngle = 45.0f; // Launch angle
    private float _gravity = 9.8f; // Gravity force

    private bool isLaunched = false; // Ensure that we don't fire this projectile multiple times

    public static event Action<int> OnEnemyDamaged;

    // Called from the tower to initialize the projectile
    public void Initialize(Vector3 targetPos)
    {
        _targetPosition = targetPos;
        if (!isLaunched)
        {
            isLaunched = true;
            StartCoroutine(SimulateProjectile());
        }
    }

    IEnumerator SimulateProjectile()
    {
        // Short delay added before the projectile is fired
        yield return new WaitForSeconds(0.1f);

        // Calculate the initial trajectory and firing details
        float distance = Vector3.Distance(transform.position, _targetPosition);
        float velocity = distance / (Mathf.Sin(2 * _firingAngle * Mathf.Deg2Rad) / _gravity);
        float Vx = Mathf.Sqrt(velocity) * Mathf.Cos(_firingAngle * Mathf.Deg2Rad);
        float Vy = Mathf.Sqrt(velocity) * Mathf.Sin(_firingAngle * Mathf.Deg2Rad);
        float flightDuration = distance / Vx;

        // Rotate to face the target direction
        transform.rotation = Quaternion.LookRotation(_targetPosition - transform.position);

        float elapsedTime = 0;
        while (elapsedTime < flightDuration)
        {
            transform.Translate(0, (Vy - (_gravity * elapsedTime)) * Time.deltaTime, Vx * 4 * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            OnEnemyDamaged.Invoke(_bulletDamage);
            Destroy(gameObject);  // Destroys this projectile
        }
    }
}