using JM.LinqFaster;
using UnityEngine;

namespace Domains.Game.GameLoop.Weapons
{
    [CreateAssetMenu(fileName = "BulletDefinitions", menuName = "Bullets/Bullet Definitions")]
    public class BulletDefinitions : ScriptableObject
    {
        [SerializeField] private BulletData[] _bullets;

        public BulletData GetBulletByLevel(int level)
        {
            return _bullets.FirstOrDefaultF(lvl => lvl.Level == level);
        }
    }
}
