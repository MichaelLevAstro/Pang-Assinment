namespace Domains.Global_Domain.Models.Player
{
    public interface IPlayerModelWrite
    {
        void SetPlayerProgress(int level);
        void SetPlayerScore(long score);
        void AddPlayerScore(long scoreDelta);
    }
}