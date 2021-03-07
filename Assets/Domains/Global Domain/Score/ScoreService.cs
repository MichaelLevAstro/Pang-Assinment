using Domains.Game.GameLoop.Score;
using Domains.Global_Domain.Models.Player;
using Zenject;

namespace Domains.Global_Domain.Score
{
    public class ScoreService
    {
        [Inject] private IPlayerModelRead _playerModelRead;
        [Inject] private IPlayerModelWrite _playerModelWrite;
        [Inject] private SignalBus _signalBus;

        public void AddToPlayerScore(long scoreDelta)
        {
            _playerModelWrite.AddPlayerScore(scoreDelta);
            FireScoreChangedSignal();
        }

        public long GetPlayerScore()
        {
            return _playerModelRead.GetPlayerScore();
        }

        private void FireScoreChangedSignal()
        {
            _signalBus.Fire(new ScoreChangedSignal(GetPlayerScore()));
        }
    }
}