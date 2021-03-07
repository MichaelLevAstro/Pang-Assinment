using System;

namespace Domains.Game.GameLoop.Weapons
{
    [Serializable]
    public class BulletData
    {
        public float Speed;
        public string Path;
        public int Level;
        public float FireRate;
    }
}