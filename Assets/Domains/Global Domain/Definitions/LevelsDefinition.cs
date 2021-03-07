using System.Collections.Generic;
using Domains.Game.GameLoop.Environment;
using JM.LinqFaster;
using UnityEngine;

namespace Domains.Global_Domain.Definitions
{
    [CreateAssetMenu(fileName = "LevelDefinitions", menuName = "Level", order = 0)]
    public class LevelsDefinition : ScriptableObject
    {
        [SerializeField] private List<LevelData> _levelDefinitions;

        public LevelData GetLevelByLevel(int level)
        {
            return _levelDefinitions.FirstOrDefaultF(lvl => lvl.Level == level);
        }
    }
}