using System;

[Serializable]
public class EnemySpawner : Spawner<Enemy>
{
    public EnemySpawner(Enemy enenmyPrefab)
    {
        Prefab = enenmyPrefab;
        Pool = new EnemyPool(Prefab, StartAmount);
    }
}