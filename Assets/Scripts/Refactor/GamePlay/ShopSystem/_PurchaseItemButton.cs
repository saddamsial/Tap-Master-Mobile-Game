using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.GamePlay.Shop{
    public class _PurchaseItemButton{
        private Button _purchaseButton;
        private TMP_Text _priceText;
        private GameObject _lockMask;
        
        public _PurchaseItemButton(Transform rootObject){
            _purchaseButton = rootObject.GetComponent<Button>();
            _priceText = rootObject.GetChild(1).GetComponent<TMP_Text>();
            _lockMask = rootObject.GetChild(2).gameObject;
        }

        public void SetUpPurchaseItemButton(int price, bool isLock){
            _priceText.text = price.ToString();
            _lockMask.SetActive(isLock);
            _purchaseButton.interactable = !isLock;
        }
    }
}