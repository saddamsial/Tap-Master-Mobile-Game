using System;
using System.Collections;
using Coffee.UIExtensions;
using UnityEngine;
using UnityEngine.UI.Extensions.CasualGame;

namespace MyTools.ParticleSystem{
    public class _BaseMyParticles : UIParticleSystem{
        public _ParticleTypeEnum ParticleType;
        public RectTransform RectTransform;

        public void Play(Action closeCallBack = null){
            //base.StartParticleEmission();
            this.GetComponent<UnityEngine.ParticleSystem>().Play();
            float duration = (this.GetComponent<UnityEngine.ParticleSystem>().main.duration);
            StartCoroutine(Close(duration, closeCallBack));
        }

        private IEnumerator Close(float duration, Action closeCallBack){
//            Debug.Log("Complete Particle");
            yield return new WaitForSeconds(duration);
            closeCallBack?.Invoke();
        }
    }
}