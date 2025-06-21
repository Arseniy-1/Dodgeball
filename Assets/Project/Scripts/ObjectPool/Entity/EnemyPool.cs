using UnityEngine;

public class EnemyPool : Pool<Enemy>
{
    public EnemyPool(Enemy prefab, int startAmount) : base(prefab, startAmount)
    {
    }

    protected override Enemy Create()
    {
        var enemy = Object.Instantiate(Prefab);
        enemy.gameObject.SetActive(false);

        return enemy;
    }
}