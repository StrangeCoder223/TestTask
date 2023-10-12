using System;
using System.Collections.Generic;
using UnityEngine;

public class SlowdownArea : MonoBehaviour
{
    [SerializeField] private float _slowdownMultiplier;
    private readonly Dictionary<Rigidbody2D, RbInitialData> _rigidbodiesWithData = new();
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Rigidbody2D rb))
        {
            RbInitialData initialData = new RbInitialData(rb);
            _rigidbodiesWithData.Add(rb, initialData);

            float slowTimeScale = 1f / _slowdownMultiplier;
            rb.gravityScale = 0;
            rb.mass /= slowTimeScale;
            rb.velocity *= slowTimeScale;
            rb.angularVelocity *= slowTimeScale;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out Rigidbody2D rb))
        {
            Restore(rb);
            _rigidbodiesWithData.Remove(rb);
        }
    }

    private void Slowdown(Rigidbody2D rb)
    {
        float slowTimeScale = Time.timeScale / _slowdownMultiplier;
        float deltaTime = Time.fixedDeltaTime * slowTimeScale;
        rb.velocity += Physics2D.gravity / rb.mass * deltaTime;
    }

    private void Restore(Rigidbody2D rb)
    {
        float slowTimeScale = 1f / _slowdownMultiplier;
        rb.mass = _rigidbodiesWithData[rb].Mass;
        rb.gravityScale = _rigidbodiesWithData[rb].Gravity;
        rb.velocity /= slowTimeScale;
        rb.angularVelocity /= slowTimeScale;
    }

    private void FixedUpdate()
    {
        foreach (var rb in _rigidbodiesWithData)
        {
            Slowdown(rb.Key);
        }
    }
}

public class RbInitialData
{
    public float Mass { get; private set; }
    public float Gravity { get; private set; }
    
    public RbInitialData(Rigidbody2D rigidbody)
    {
        Mass = rigidbody.mass;
        Gravity = rigidbody.gravityScale;
    }
}