using TMPro;
using UnityEngine;
using Zenject;

namespace Domains.Game.GameLoop.Score
{
    public class ScoreController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _scoreText;

        private SignalBus _signalBus;
        
        [Inject]
        public void Initialize(SignalBus signalBus)
        {
            _signalBus = signalBus;
            _signalBus.Subscribe<ScoreChangedSignal>(OnScoreChanged);
        }

        private void OnScoreChanged(ScoreChangedSignal scoreSignal)
        {
            var currentScore = scoreSignal.CurrentScore;
            _scoreText.text = currentScore.ToString();
        }
    }
}