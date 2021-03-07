using UnityEngine;

namespace Domains.Global_Domain
{
    public class DestroyService : MonoBehaviour
    {
        public void DestroyObject(GameObject obj)
        {
            Destroy(obj);
        }
        
        public void DestroyComponent(MonoBehaviour mono)
        {
            Destroy(mono);
        }
    }
}