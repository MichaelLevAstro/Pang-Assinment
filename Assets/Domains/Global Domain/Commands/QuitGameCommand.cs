using Domains.Global_Domain.Signals;
using Global_Domain.Commands;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Domains.Global_Domain.Commands
{
    public class QuitGameCommand : BaseCommand<QuitGameSignal>
    {
        protected override void OnCallbackAction(QuitGameSignal quitGameSignal)
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif   
        }
    }
}