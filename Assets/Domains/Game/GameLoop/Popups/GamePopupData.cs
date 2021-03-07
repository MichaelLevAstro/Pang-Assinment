using Domains.Global_Domain.Popups;

namespace Domains.Game.GameLoop.Popups
{
    public class GamePopupData : PopupData
    {
        public bool IsPause;
        public long PlayerScore;
        public bool DidPlayerWinLevel;
    }
}