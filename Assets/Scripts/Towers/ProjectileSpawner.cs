using UnityEngine;
using UnityEngine.Pool;
using System.Collections.Generic;

public class ProjectileSpawner : Singleton<ProjectileSpawner>
{
    [SerializeField] private BananaProjectile _bananaProjectile;
    [SerializeField] private ObjectPool<BananaProjectile> _projectilePool;

    private void Start()
    {
        _projectilePool = new ObjectPool<BananaProjectile>(CreateBananaPoolObject, OnTakeBananaFromPool, OnReturnBananaToPool, OnDestroyBanana, false, 50, 200);        
    }

    private BananaProjectile CreateBananaPoolObject()
    {
        BananaProjectile projectile = Instantiate(_bananaProjectile, transform);
        projectile.DestroyBanana += ReleaseBananaToPool;
        projectile.gameObject.SetActive(false);
        return projectile;
    }

    private void OnTakeBananaFromPool(BananaProjectile projectile)
    {
        projectile.gameObject.SetActive(true);        
    }

    private void OnReturnBananaToPool(BananaProjectile projectile)
    {
        projectile.gameObject.SetActive(false);
    }

    private void OnDestroyBanana(BananaProjectile projectile)
    {
        Destroy(projectile.gameObject);
    }

    private void ReleaseBananaToPool(BananaProjectile projectile)
    {
        _projectilePool.Release(projectile);
    }

    public void SummonBanana(Vector3 summonPos, Vector3 targetPos, Enemy targetEnemy, float projectileSpeed, float projectileDamage)
    {
        BananaProjectile projectile = _projectilePool.Get();
        projectile.SummonBanana(summonPos, targetPos, targetEnemy, projectileSpeed, projectileDamage);
    }   
}
