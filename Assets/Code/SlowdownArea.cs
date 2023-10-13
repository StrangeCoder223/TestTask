using System;
using System.Collections.Generic;
using UnityEngine;

public class SlowdownArea : MonoBehaviour
{
    [SerializeField] private float _slowdownMultiplier;
    private readonly List<Shell> _shells = new();
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Shell shell))
        {
            _shells.Add(shell);

            float slowTimeScale = 1f / _slowdownMultiplier;
            shell.Rigidbody.gravityScale = 0;
            shell.Rigidbody.mass *= slowTimeScale;
            shell.Rigidbody.velocity *= slowTimeScale;
            shell.Rigidbody.angularVelocity *= slowTimeScale;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out Shell shell))
        {
            float slowTimeScale = 1f / _slowdownMultiplier;
            shell.ResetPhysics(slowTimeScale);
            _shells.Remove(shell);
        }
    }

    private void Slowdown(Shell shell)
    {
        float slowTimeScale = Time.timeScale / _slowdownMultiplier;
        float deltaTime = Time.fixedDeltaTime * slowTimeScale;
        shell.Rigidbody.velocity += Physics2D.gravity * (shell.Rigidbody.mass * deltaTime);
    }

    private void FixedUpdate()
    {
        foreach (var shell in _shells)
        {
            Slowdown(shell);
        }
    }
}