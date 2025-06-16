using System;

[Serializable]
public class PlayerSpawner : Spawner<Player>
{
    public PlayerSpawner(Player playerPrefab)
    {
        Prefab = playerPrefab;
        Pool = new PlayerPool(Prefab, StartAmount);
    }
}