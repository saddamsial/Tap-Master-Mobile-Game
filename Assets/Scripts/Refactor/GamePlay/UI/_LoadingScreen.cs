using System.Collections;
using Core.ResourceGamePlay;
using MyTools.ScreenSystem;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Core.UI
{
    public class _LoadingScreen : _BaseScreen
    {
        [SerializeField] TMPro.TMP_Text _loadingText;
        [SerializeField] private int _percent = 100;
        [SerializeField] private Slider _loadingSlider;
        [SerializeField] private float _delayTime = 0.1f;

        private bool _isLoadedAsset = false;

        private void RunLoading()
        {
            StartCoroutine(CounterTime.CounterUp(_percent, _delayTime, OnCouter, OnCounterComplete));
        }

        private void OnCouter(int value)
        {
            _loadingText.text = $"{value}%";
            _loadingSlider.value = value / 100f;
        }

        private void OnCounterComplete()
        {
            //await Task.Delay(() => AdsManager.Instance.canLoadAds);
            //SceneManager.LoadScene(Const.SCENE_GAME);
            StartCoroutine(LoadScene());
        }

        IEnumerator LoadScene()
        {
            //yield return new WaitUntil( () => AdsManager.Instance.canLoadAds);
            int count = 0;
            if (count == 30)
            {
                LoadAddressables();
                yield return new WaitUntil(() => _isLoadedAsset);
            }
            else
                yield return new WaitForEndOfFrame();
            yield return new WaitUntil(() => AdsManager.Instance.CanLoadAds);
            SceneManager.LoadScene(Const.SCENE_GAMEPLAY);
            count += 1;
        }

        private async void LoadAddressables()
        {
            await AddressablesManager.LoadAssetAsync<GameObject>(_KeyPrefabResources.KeyBlock);
            await AddressablesManager.LoadAssetAsync<Material>(_KeyMaterialResources.KeyMovingMaterial);
            await AddressablesManager.LoadAssetAsync<Material>(_KeyMaterialResources.KeyBlockedMaterial);
            await AddressablesManager.LoadAssetAsync<Material>(_KeyMaterialResources.KeyIdleMaterial);
            _isLoadedAsset = true;
        }

        protected override void OnCompleteShowItSelf()
        {
            base.OnCompleteShowItSelf();
            RunLoading();
        }
    }
}