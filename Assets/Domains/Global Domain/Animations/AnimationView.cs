using System;
using DG.Tweening;
using RSG;
using UnityEngine;
using Zenject;

namespace Global_Domain.Animations
{
    public class AnimationView : MonoBehaviour
    {
        [SerializeField] private Animation _animation;

        public IPromise Play(string clipName)
        {
            var foundClip = _animation.GetClip(clipName);
            if (foundClip == null)
            {
                return Promise.Rejected(new Exception("Clip not found"));
            }

            var promise = new Promise();
            _animation.clip = foundClip;
            _animation.Play();
            DOVirtual.DelayedCall(_animation.clip.length, promise.Resolve);
            return promise;
        }
    }
}