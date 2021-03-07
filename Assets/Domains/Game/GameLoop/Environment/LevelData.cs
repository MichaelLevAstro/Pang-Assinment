using System;
using Domains.Game.GameLoop.Enemies;

namespace Domains.Game.GameLoop.Environment
{
    [Serializable]
    public class LevelData
    {
        public int Level;
        public string LevelPath;
        public EnemyType[] Enemies;
    }
}
