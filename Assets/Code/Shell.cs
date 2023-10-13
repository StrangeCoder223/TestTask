using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Shell : MonoBehaviour
{
    public Rigidbody2D Rigidbody => _rigidbody;
    
    [SerializeField] private Rigidbody2D _rigidbody;
    private Action _onBecameInvisible;
    private float _initialMass;
    private float _initialGravity;

    private void Awake()
    {
        _initialMass = _rigidbody.mass;
        _initialGravity = _rigidbody.gravityScale;
    }

    private void OnBecameInvisible()
    {
        _onBecameInvisible?.Invoke();
    }

    public void Launch(Vector2 direction, float force, Action onBecameInvisible = null)
    {
        _rigidbody.AddForce(direction * force);
        _onBecameInvisible = onBecameInvisible;
    }

    public void ResetPhysics(float timeScale)
    {
        _rigidbody.mass = _initialMass;
        _rigidbody.gravityScale = _initialGravity;
        _rigidbody.velocity /= timeScale;
        _rigidbody.angularVelocity /= timeScale;
    }
}