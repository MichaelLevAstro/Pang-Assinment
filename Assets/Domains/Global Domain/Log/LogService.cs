using UnityEngine;

namespace Domains.Global_Domain.Log
{
    public class LogService
    {
        public void Log(string message)
        {
            Debug.Log(message);
        }
        
        public void LogError(string message)
        {
            Debug.LogError(message);
        }
        
        public void LogWarning(string message)
        {
            Debug.LogWarning(message);
        }
    }
}
