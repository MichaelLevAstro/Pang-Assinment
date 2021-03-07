using System;
using System.Collections;
using UnityEngine;

namespace Global_Domain
{
    public class CoroutineService : MonoBehaviour
    {
        public Coroutine RunCoroutine(IEnumerator coroutine)
        {
            return StartCoroutine(coroutine);
        }

        public void StopCoroutine(Coroutine coroutine)
        {
            StopCoroutine(coroutine);
        }
        
        public void WaitForSeconds(int seconds, Action callback)
        {
            StartCoroutine(WaitForSecondsRoutine(seconds, callback));
        }

        private IEnumerator WaitForSecondsRoutine(int seconds, Action callback)
        {
            // I wish wait for seconds had the ability to re set the time so i wouldn't have to create a new object everytime...
            yield return new WaitForSeconds(seconds);
            callback?.Invoke();
        }
    }
}