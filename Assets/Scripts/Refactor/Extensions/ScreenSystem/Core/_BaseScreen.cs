using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.ResourceProviders.Simulation;

namespace MyTools.ScreenSystem
{
    [RequireComponent(typeof(Animator))]
    public class _BaseScreen : MonoBehaviour
    {

        [SerializeField] private AnimationClip _showAnimation;
        [SerializeField] private AnimationClip _hideAnimation;
        [SerializeField] private Animator _animator;
        [SerializeField] private bool _showWithAnimation = true;
        private bool _isShown = false;
        public _ScreenTypeEnum ScreenType;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Show(Action complete = null)
        {
            if (_showWithAnimation)
            {
                StartCoroutine(ShowByAnimation(complete));
            }
            else
            {
                ShowByAnimation(complete);
            }
        }

        public void Hide(Action completed = null)
        {
            if (_showWithAnimation)
            {
                StartCoroutine(HideByAnimation(completed));
            }
            else
            {
                HideByAnimation(completed);
            }
        }

        private IEnumerator ShowByAnimation(Action completed = null)
        {
            this.gameObject.SetActive(true);
            if (_isShown) throw new System.Exception("Screen is already shown");
            _animator.Play(_showAnimation.name);
            yield return new WaitForSeconds(_showAnimation.length);
            _isShown = true;
            completed?.Invoke();
        }

        private IEnumerator HideByAnimation(Action completed = null)
        {
            if (!_isShown) throw new System.Exception("Screen is already hidden");
            _animator.Play(_hideAnimation.name);
            yield return new WaitForSeconds(_hideAnimation.length);
            _isShown = false;
            completed?.Invoke();
            this.gameObject.SetActive(false);
        }
    }
}