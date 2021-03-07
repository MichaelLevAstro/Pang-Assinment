using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Global_Domain
{
    /// <summary>
    /// Very simple scene service
    /// </summary>
    public class SceneService
    {
        [Inject] private CoroutineService _coroutineService;
        
        public AsyncOperation LoadScene(string sceneName, bool additively = false)
        {
            return SceneManager.LoadSceneAsync(sceneName, additively ? LoadSceneMode.Additive : LoadSceneMode.Single);
        }

        public AsyncOperation UnloadScene(string sceneName)
        {
            return SceneManager.UnloadSceneAsync(sceneName);
        }

        public void SwapScenes(string fromScene, string toScene, bool additively = false, int unloadDelay = 0)
        {
            var asyncOperation = LoadScene(toScene, additively);
            asyncOperation.allowSceneActivation = false;
            asyncOperation.completed += operation => UnloadScene(fromScene);
            _coroutineService.RunCoroutine(WaitToEndSceneLoadRoutine(asyncOperation, unloadDelay));
        }

        private void OnSceneLoaded(AsyncOperation obj, int unloadDelay)
        {
            _coroutineService.WaitForSeconds(unloadDelay, () =>
            {
                obj.allowSceneActivation = true;
            });
        }

        private IEnumerator WaitToEndSceneLoadRoutine(AsyncOperation operation, int unloadDelay)
        {
            yield return null;
            while (!operation.isDone)
            {
                if (operation.progress >= 0.88f)
                {
                    break;
                }
            }

            OnSceneLoaded(operation, unloadDelay);
        }
    }
}