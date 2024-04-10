using Core.GamePlay.Collection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.GamePlay.Collection
{
    public class _CollectionElements
    {

        public _CollectionElements() { }

        public _CollectionElements(Transform rootObject)
        {
            var top = rootObject.Find("Top");
            var puzzlesContainer = rootObject.Find("Mid").GetChild(0);
            _headTitle = top.GetChild(0).GetComponent<TMP_Text>();
            _rewardImage = top.GetChild(2).GetComponent<Image>();
            if (puzzlesContainer.childCount == 6)
            {
                for (int i = 0; i < puzzlesContainer.childCount; i++)
                {
                    _mainImages[i] = puzzlesContainer.GetChild(i).GetComponent<Image>();
                    if (_mainImages[i].transform.childCount == 9)
                    {
                        for (int j = 0; j < _mainImages[i].transform.childCount; j++)
                        {
                            //Debug.Log("Puzzle Piece: " + (i * 9 + j));
                            _puzzlePieces[i * 9 + j] = _mainImages[i].transform.GetChild(j).GetComponent<Image>();
                        }
                    }
                    else
                    {
                        throw new System.Exception("Puzzle Collection Element must have 9 puzzle pieces");
                    }
                }
            }
            else
            {
                throw new System.Exception("Puzzle Collection Element must have 6 main images");
            }
        }

        public _CollectionElements(Image rewadImage, Image[] mainImage, Image[] puzzlePieces, TMP_Text headTitle)
        {
            _rewardImage = rewadImage;
            _mainImages = mainImage;
            _puzzlePieces = puzzlePieces;
            _headTitle = headTitle;
        }

        private Image _rewardImage;
        private Image[] _mainImages = new Image[6];
        private Image[] _puzzlePieces = new Image[54];
        private TMP_Text _headTitle;

        public void SetupPuzzle(_CollectionElementData data)
        {
            _rewardImage.sprite = data.rewardImage;
            _headTitle.text = data.name;
            for (int i = 0; i < data.mainImages.Length; i++)
            {
                _mainImages[i].sprite = data.mainImages[i];
            }
        }

        public void SetupPuzzleState(int puzzlePieceId)
        {
            _puzzlePieces[puzzlePieceId].gameObject.SetActive(false);
        }

        // public void SetupPuzzleState(){

        // }
    }
}