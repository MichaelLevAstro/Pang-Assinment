namespace Domains.Game.GameLoop.Weapons.Models
{
    public class WeaponsModel : IWeaponsModelRead, IWeaponsModelWrite
    {
        private BulletData _currentBulletData; 
        
        public BulletData GetCurrentBulletData()
        {
            return _currentBulletData;
        }

        public void SetCurrentBulletData(BulletData bulletData)
        {
            _currentBulletData = bulletData;
        }
    }
}