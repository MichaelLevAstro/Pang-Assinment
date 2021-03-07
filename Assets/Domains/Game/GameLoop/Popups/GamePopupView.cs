using Domains.Global_Domain.Popups;
using Domains.Global_Domain.Signals;
using TMPro;
using UnityEngine;

namespace Domains.Game.GameLoop.Popups
{
    public class GamePopupView : BasePopupView
    {
        private const string PAUSED_TITLE = "Paused";
        private const string PLAYER_WON_TITLE = "You Won!";
        private const string PLAYER_LONS_TITLE = "Game over";
        
        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private TextMeshProUGUI _playerScoreText;
        [SerializeField] private GameObject _nextLevelButton;
        [SerializeField] private GameObject _resumeButton;

        public override void SetupPopup(PopupData popupData)
        {
            if (!(popupData is GamePopupData gameEndPopupData))
            {
                return;
            }
            _playerScoreText.text = gameEndPopupData.PlayerScore.ToString();
            _nextLevelButton.SetActive(gameEndPopupData.DidPlayerWinLevel && !gameEndPopupData.IsPause);
            _resumeButton.SetActive(gameEndPopupData.IsPause);
            if (gameEndPopupData.IsPause)
            {
                _titleText.text = PAUSED_TITLE;
            }
            else
            {
                _titleText.text = gameEndPopupData.DidPlayerWinLevel ? PLAYER_WON_TITLE : PLAYER_LONS_TITLE;   
            }
        }
        
        public void OnNextLevel()
        {
            _signalBus.Fire<StartNextLevelSignal>();
            ClosePopup();
        }
        
        public void OnPlayerReTry()
        {
            _signalBus.Fire<RetryLevelSignal>();
            ClosePopup();
        }
        
        public void OnPlayerResume()
        {
            _signalBus.Fire<ResumeGameSignal>();
            ClosePopup();
        }
        
        public void OnPlayerQuit()
        {
            _signalBus.Fire<QuitToMainMenuSignal>();
            ClosePopup();
        }
    }
}