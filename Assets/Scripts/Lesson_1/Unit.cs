using System.Collections;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private float _health = 80;

    private float _maxHealth = 100; 
    private float _duration = 3f;
    private float _interval = 0.5f;
    private int _healingAmount = 5;
    private float _elapsedTime = 0f;

    private Coroutine _healingCoroutine;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            ReceiveHealing();
        }
    }

    public void ReceiveHealing()
    {
        if (_healingCoroutine != null)
        {
            StopCoroutine(_healingCoroutine);
        }

        _healingCoroutine = StartCoroutine(HealingCoroutine());
    }

    private IEnumerator HealingCoroutine()
    {
        while (_health < _maxHealth && _elapsedTime < _duration)
        {
            _health += _healingAmount;
            yield return new WaitForSeconds(_interval);
            _elapsedTime += _interval;
            Debug.Log($"Health + {_health}");
        }

        if (_health >= _maxHealth)
        {
            Debug.Log("Health reached 100");
        }

        else
        {
            Debug.Log("Health duration expired");
        }
    }
}
