using System;
using UnityEngine;

namespace Domains.Game.GameLoop.Enemies
{
    // this we could have fun with, setting start size, size division factor
    [Serializable]
    public class EnemyData
    {
        public EnemyType EmemyType;
        public string Path;
        public float Size;
        public float InitialVelocity;
        public int InitialDirection;
        public int MaxSplits;
        public Sprite EnemySprite;
        public long KillScore;

        public EnemyData Copy()
        {
            return new EnemyData
            {
                EmemyType = EmemyType,
                Path = Path,
                Size = Size,
                InitialVelocity = InitialVelocity,
                InitialDirection = InitialDirection,
                MaxSplits = MaxSplits,
                EnemySprite = EnemySprite,
                KillScore = KillScore,
            };
        }

        public void FlipDirection()
        {
            InitialDirection *= -1;
        }
    }
}