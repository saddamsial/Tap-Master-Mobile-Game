using UnityEngine;
using MyTools.ScreenSystem;
using UnityEngine.EventSystems;

namespace Core.UI
{
    public class _GameplayTutorialScreen : _BaseScreen, IPointerDownHandler
    {
        [SerializeField] private GameObject _tutorialsObject;

        public void OnPointerDown(PointerEventData data)
        {
            if(_isShown)
                _tutorialsObject.SetActive(false);
        }

        public void OnMouseDown()
        {

        }
    }
}