using UnityEngine;
using UnityEngine.Pool;
using System.Collections.Generic;

public class ProjectileSpawner : Singleton<ProjectileSpawner>
{
    [SerializeField] private BananaProjectile _bananaProjectile;
    [SerializeField] private ObjectPool<BananaProjectile> _projectilePool;

    private void Start()
    {
        _projectilePool = new ObjectPool<BananaProjectile>(CreateArrowPoolObject, OnTakeArrowFromPool, OnReturnArrowToPool, OnDestroyArrow, false, 50, 200);        
    }

    private BananaProjectile CreateArrowPoolObject()
    {
        BananaProjectile projectile = Instantiate(_bananaProjectile, transform);
        projectile.DestroyArrow += ReleaseArrowToPool;
        projectile.gameObject.SetActive(false);
        return projectile;
    }

    private void OnTakeArrowFromPool(BananaProjectile projectile)
    {
        projectile.gameObject.SetActive(true);        
    }

    private void OnReturnArrowToPool(BananaProjectile projectile)
    {
        projectile.gameObject.SetActive(false);
    }

    private void OnDestroyArrow(BananaProjectile projectile)
    {
        Destroy(projectile.gameObject);
    }

    private void ReleaseArrowToPool(BananaProjectile projectile)
    {
        _projectilePool.Release(projectile);
    }

    public void SummonArrow(Vector3 summonPos, Enemy enemy, float projectileSpeed, float projectileDamage)
    {
        BananaProjectile projectile = _projectilePool.Get();
        projectile.SummonArrow(summonPos, enemy, projectileSpeed, projectileDamage);
    }

    /*private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 200, 30), $"Total Pool Size: {_projectilePool.CountAll}");
        GUI.Label(new Rect(10, 40, 200, 30), $"Active Objects: {_projectilePool.CountActive}");
    }*/
}
