using UnityEngine;
using UnityEngine.EventSystems;

namespace Components.UI
{
    /// <summary>
    /// Класс выделитель при наведении курсора.
    /// </summary>
    public sealed class SCHoverSelection : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private GameObject[] _diactivatedObjects;
        [SerializeField] private GameObject[] _activatedObjects;
        [SerializeField] private SImageColor[] _changedColorImages;
        [SerializeField] private STMPColor[] _changedTMPColors;
        [SerializeField] private SRectTransformsScale[] _changedScaleImages;

        private Color[] _defaultImagesColors;
        private Color[] _defaultTMPColors;
        private Vector2[] _defaultScales;

        private void Awake() 
            => Construct();

        private void Construct()
        {
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
        }
        public void OnPointerEnter(PointerEventData eventData)
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
        public void OnPointerExit(PointerEventData eventData)
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