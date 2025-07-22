using Project.Scripts.Entities;
using UnityEngine;

namespace Project.Scripts.ObjectPool.Entity
{
    public class PlayerPool : Pool<Player>
    {
        public PlayerPool(Player prefab, int startAmount) 
            : base(prefab, startAmount)
        {
        }

        protected override Player Create()
        {
            var player = Object.Instantiate(Prefab);
            player.gameObject.SetActive(false);

            return player;
        }
    }
}