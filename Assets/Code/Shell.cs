using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Shell : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody;
    private Action _onBecameInvisible;
    
    private void OnBecameInvisible()
    {
        _onBecameInvisible?.Invoke();
    }

    public void Launch(Vector2 direction, float force, Action onBecameInvisible = null)
    {
        _rigidbody.AddForce(direction * force);
        _onBecameInvisible = onBecameInvisible;
    }
}