namespace Domains.Global_Domain.Models.Player
{
    public interface IPlayerModelRead
    {
        int GetPlayerProgress();
        long GetPlayerScore();
    }
}