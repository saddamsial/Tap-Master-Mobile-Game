using System.Collections;
using Core.Data;
using Core.GamePlay;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using PopupSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI.ExtendPopup{
    public class _CollectRewardGoldPopup : BasePopup{
        [SerializeField] private TMPro.TMP_Text _coinText;
        [SerializeField] private TMP_Text _multiCoinText;
        [SerializeField] private TMP_Text _finalCoinText;
        [SerializeField] private Image _multipleBarImage;
        [SerializeField] private RectTransform _cursor;
        [SerializeField] private GameObject _watchAdButton;

        private float _barWidth;
        private float _pivotPos;
        private int _coin;
        private bool _isWinGame;

        // public override void Awake(){
        //     _barWidth = _multipleBarImage.rectTransform.rect.width;
        //     _pivotPos = _multipleBarImage.rectTransform.position.x - _barWidth / 2;
        // }

        public void Show(int coin , bool isWinGame = false){
            base.Show();
            _GameManager.Instance.GamePlayManager.IsGameplayInteractable = false;
            _watchAdButton.SetActive(true);
            _barWidth = _multipleBarImage.rectTransform.rect.width;
            _pivotPos = _multipleBarImage.rectTransform.localPosition.x - _barWidth / 2;
            _coinText.text = "+" + coin.ToString();
            _finalCoinText.text = ( coin).ToString();
            _multiCoinText.text = ( coin * 5).ToString();
            _coin = coin;
            _cursor.GetComponent<RectTransform>().localPosition = new Vector3(_pivotPos, _cursor.localPosition.y, _cursor.localPosition.z);
            StartMovingCursor();
            _isWinGame = isWinGame;
        }

        public void OnClickWatchAd(){
            _MySoundManager.Instance.PlaySound(SoundType.ClickUIButton);
            _cursor.DOKill();
            AdsManager.Instance.ShowRewarded(
                (x) => {
                    GlobalEventManager.Instance.OnRewardedComplete(_PlayerData.UserData.CurrentLevel, "goldBlock_get_multi_coin");
                    if(x)
                        OnCompleteWatchAds();
                }, null, location: "goldBlock_get_multi_coin"
            );
        }

        private void OnCompleteWatchAds(){
            float tmpX = _cursor.localPosition.x;
            float value = tmpX - _pivotPos;
            float dis = _barWidth / 7;
            int val = Mathf.FloorToInt(value / dis);
            int coin = _coin;
            switch (val){
                case 0:
                    coin = _coin * 2;
                    break;
                case 1:
                    coin =  _coin * 3;
                    break;
                case 2:
                    coin = _coin * 4;
                    break;
                case 3:
                    coin = _coin *5;
                    break;
                case 4:
                    coin = _coin * 4;
                    break;
                case 5:
                    coin = _coin * 3;
                    break;
                case 6:
                    coin = _coin * 2;
                    break;
            }
            _coinText.text = "+" + coin.ToString();
            //_PlayerData.UserData.Coin += coin - _coin;
            _PlayerData.UserData.CurrentCollectCoin += coin - _coin;
            //_GameEvent.OnReceivedRewardByAds?.Invoke(GamePlay.Block._BlockTypeEnum.GoldReward ,coin - _coin);
            _finalCoinText.text = (coin).ToString();
            _watchAdButton.SetActive(false);
        }

        public void OnClickClose(){
            _MySoundManager.Instance.PlaySound(SoundType.ClickUIButton);
            _cursor.DOKill();
            if(_isWinGame){
                _GameManager.Instance.GamePlayManager.IsGameplayInteractable = false;
                this.gameObject.SetActive(false);
                PopupManager.CreateNewInstance<_WinGamePopup>().Show();
            }
            else{
                base.Hide(
                    () => {
                        _GameManager.Instance.GamePlayManager.IsGameplayInteractable = true;
                    }
                );
            }
        }

        private IEnumerator DelayShowWinGamePopup(float delayTime){
            yield return new WaitForSeconds(delayTime);
            PopupManager.CreateNewInstance<_WinGamePopup>().Show();
        }

        private void StartMovingCursor(){
            _cursor.DOLocalMoveX(_cursor.localPosition.x + _barWidth, 1.25f).SetEase(Ease.InOutCubic).SetLoops(-1, LoopType.Yoyo);
        }
    }
}