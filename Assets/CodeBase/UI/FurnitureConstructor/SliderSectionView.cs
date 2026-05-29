using System;
using CodeBase.Data.FurnitureConstructor;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.FurnitureConstructor
{
    public class SliderSectionView : MonoBehaviour
    {
        [SerializeField] private Slider _heightSlider;
        [SerializeField] private Slider _widthSlider;
        [SerializeField] private Slider _depthSlider;

        [SerializeField] private TextMeshProUGUI _heightText;
        [SerializeField] private TextMeshProUGUI _widthText;
        [SerializeField] private TextMeshProUGUI _depthText;

        public event Action<MorphType, float> OnSizeChanged;

        public void InitializeSliders(
            float? heightMin, float? heightMax,
            float? widthMin, float? widthMax,
            float? depthMin, float? depthMax)
        {
            SetupSlider(_heightSlider, heightMin, heightMax, MorphType.Height, _heightText);
            SetupSlider(_widthSlider, widthMin, widthMax, MorphType.Width, _widthText);
            SetupSlider(_depthSlider, depthMin, depthMax, MorphType.Depth, _depthText);
        }

        private void SetupSlider(Slider slider, float? min, float? max, MorphType type, TextMeshProUGUI textElement)
        {
            if (slider == null)
            {
                Debug.LogError($"Slider for {type} is missing.");
                return;
            }

            if (max.HasValue && max.Value > 0)
            {
                slider.minValue = min ?? 0;
                slider.maxValue = max.Value;
                slider.value = min ?? 0;

                slider.onValueChanged.RemoveAllListeners();
                slider.onValueChanged.AddListener(value =>
                {
                    OnSizeChanged?.Invoke(type, value);
                    UpdateText(textElement, value);
                });

                slider.gameObject.SetActive(true);
                UpdateText(textElement, slider.value);
            }
            else
            {
                slider.gameObject.SetActive(false);
                UpdateText(textElement, min ?? 0);
            }
        }

        private void UpdateText(TextMeshProUGUI text, float value)
        {
            if (text != null)
                text.text = $"{Mathf.RoundToInt(value * 100)}";
        }
    }
}
