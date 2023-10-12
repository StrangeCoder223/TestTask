using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

public class ShellsPool : MonoBehaviour, IObjectPool<Shell>
{
    public int CountInactive => _pooledShells.Count(x => !x.isActiveAndEnabled);
    
    [SerializeField] private int _poolSize;
    [SerializeField] private Shell _shellPrefab;
    
    private readonly List<Shell> _pooledShells = new();

    private void Awake()
    {
        for (int i = 0; i < _poolSize; i++)
        {
            _pooledShells.Add(CreateShell());
        }
    }

    private Shell CreateShell()
    {
        Shell shell = Instantiate(_shellPrefab);
        Release(shell);

        return shell;
    }

    public Shell Get()
    {
        Shell pooledShell = _pooledShells.FirstOrDefault(x => !x.isActiveAndEnabled);
        if (pooledShell == null)
        {
            Shell newShell = CreateShell();
            _pooledShells.Add(newShell);
            
            return newShell;
        }
        
        return pooledShell;
    }

    public PooledObject<Shell> Get(out Shell value)
    {
        throw new NotImplementedException();
    }

    public void Release(Shell shell)
    {
        shell.gameObject.SetActive(false);
    }

    public void Clear()
    {
        foreach (var shell in _pooledShells)
        {
            _pooledShells.Remove(shell);
            Destroy(shell);
        }
    }
}