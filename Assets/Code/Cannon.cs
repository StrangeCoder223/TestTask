using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Cannon : MonoBehaviour
{
    [SerializeField] private float _cooldown;
    [SerializeField] private float _force;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private ShellsPool _pool;
    
    private Coroutine _shootCoroutine;

    private void Awake()
    {
        _shootCoroutine = StartCoroutine(ShootCoroutine());
    }

    private IEnumerator ShootCoroutine()
    {
        while (gameObject.activeInHierarchy)
        {
            Shoot();
            yield return new WaitForSeconds(_cooldown);
        }
    }

    private void Shoot()
    {
        Shell shell = _pool.Get();
        shell.transform.position = _spawnPoint.position;
        shell.gameObject.SetActive(true);
        shell.Launch(_spawnPoint.right, _force, () => _pool.Release(shell));
    }
}