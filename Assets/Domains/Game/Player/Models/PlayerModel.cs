namespace Domains.Global_Domain.Models.Player
{
    // This data would be saved locally (or server ideally)
    public class PlayerModel : IPlayerModelWrite, IPlayerModelRead
    {
        private int _playerProgress;
        private long _playerScore;
        
        public void SetPlayerProgress(int level)
        {
            _playerProgress = level;
        }
        
        public void SetPlayerScore(long score)
        {
            _playerScore = score;
        }
        
        public void AddPlayerScore(long scoreDelta)
        {
            _playerScore += scoreDelta;
        }
        
        public int GetPlayerProgress()
        {
            return _playerProgress;
        }

        public long GetPlayerScore()
        {
            return _playerScore;
        }
    }
}