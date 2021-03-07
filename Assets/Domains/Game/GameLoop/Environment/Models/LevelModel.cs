namespace Domains.Game.GameLoop.Environment.Models
{
    public class LevelModel : ILevelModelWrite, ILevelModelRead
    {
        private int _currentlyLoadedLevel = 1;

        public void SetLevel(int currentLevel)
        {
            _currentlyLoadedLevel = currentLevel;
        }
        
        public int GetLevel()
        {
            return _currentlyLoadedLevel;
        }
    }
}