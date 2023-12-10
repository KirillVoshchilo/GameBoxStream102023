using App.Simples;
using UnityEngine;

namespace SimpleComponents.UI
{
    /// <summary>
    /// Класс для выделения UI. 
    /// Имеется 2 состояния у UI: исходное и выделелнное.
    /// </summary>
    public sealed class SCUIHighlighter : MonoBehaviour
    {
        [SerializeField] private GameObject[] _diactivatedObjects;
        [SerializeField] private GameObject[] _activatedObjects;
        [SerializeField] private SImageColor[] _changedColorImages;
        [SerializeField] private STMPColor[] _changedTMPColors;
        [SerializeField] private SRectTransformsScale[] _changedScaleImages;

        private Color[] _defaultImagesColors;
        private Color[] _defaultTMPColors;
        private Vector2[] _defaultScales;
        private bool _isConstructed;

        private void Awake()
        {
            if (!_isConstructed)
                Construct();
        }

        public void Construct()
        {
            if (_isConstructed)
                return;
            int count = _changedColorImages.Length;
            _defaultImagesColors = new Color[count];
            for (int i = 0; i < count; i++)
                _defaultImagesColors[i] = _changedColorImages[i].Image.color;
            count = _changedTMPColors.Length;
            _defaultTMPColors = new Color[count];
            for (int i = 0; i < count; i++)
                _defaultTMPColors[i] = _changedTMPColors[i].TMP.color;
            count = _changedScaleImages.Length;
            _defaultScales = new Vector2[count];
            for (int i = 0; i < count; i++)
                _defaultScales[i] = _changedScaleImages[i].RectTransform.localScale;
            _isConstructed = true;
        }
        public void Highlight()
        {
            SetActiveObjects(_diactivatedObjects, false);
            SetActiveObjects(_activatedObjects, true);
            int count = _changedColorImages.Length;
            for (int i = 0; i < count; i++)
                _changedColorImages[i].Image.color = _changedColorImages[i].TargetColor;
            count = _changedTMPColors.Length;
            for (int i = 0; i < count; i++)
                _changedTMPColors[i].TMP.color = _changedTMPColors[i].TargetColor;
            count = _changedScaleImages.Length;
            for (int i = 0; i < count; i++)
            {
                float x = _changedScaleImages[i].Scale.x;
                float y = _changedScaleImages[i].Scale.y;
                float z = _changedScaleImages[i].RectTransform.localScale.z;
                Vector3 scale = new(x, y, z);
                _changedScaleImages[i].RectTransform.localScale = scale;
            }
        }
        public void TurnOffHighlight()
        {
            SetActiveObjects(_diactivatedObjects, true);
            SetActiveObjects(_activatedObjects, false);
            int count = _changedColorImages.Length;
            for (int i = 0; i < count; i++)
                _changedColorImages[i].Image.color = _defaultImagesColors[i];
            count = _changedTMPColors.Length;
            for (int i = 0; i < count; i++)
                _changedTMPColors[i].TMP.color = _defaultTMPColors[i];
            count = _changedScaleImages.Length;
            for (int i = 0; i < count; i++)
            {
                float x = _defaultScales[i].x;
                float y = _defaultScales[i].y;
                float z = _changedScaleImages[i].RectTransform.localScale.z;
                Vector3 scale = new(x, y, z);
                _changedScaleImages[i].RectTransform.localScale = scale;
            }
        }

        private void SetActiveObjects(GameObject[] gameObjects, bool state)
        {
            int count = gameObjects.Length;
            for (int i = 0; i < count; i++)
                gameObjects[i].SetActive(state);
        }
    }
}