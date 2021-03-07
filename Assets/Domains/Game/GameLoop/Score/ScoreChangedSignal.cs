using Domains.Global_Domain.Signals;

namespace Domains.Game.GameLoop.Score
{
    public class ScoreChangedSignal : Signal
    {
        public long CurrentScore;

        public ScoreChangedSignal(long currentScore)
        {
            CurrentScore = currentScore;
        }
    }
}