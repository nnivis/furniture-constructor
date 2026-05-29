using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.FurnitureConstructor
{
    public class FurnitureVisibilityPanel : MonoBehaviour
    {
        [SerializeField] private Button _sizeButton;
        [SerializeField] private Button _styleButton;
        [SerializeField] private Button _materialButton;

        private SliderSectionView _sizeSection;
        private DropDownSectionView _materialSection;
        private DropDownSectionView _stylesSection;

        private GameObject _activeSection;

        public void Initialize(SliderSectionView sizeSection, DropDownSectionView materialSection,
            DropDownSectionView styleSection)
        {
            _sizeSection = sizeSection;
            _materialSection = materialSection;
            _stylesSection = styleSection;

            DisableSectionPanel();
        }

        public void SetStyleIconVisibility(bool isVisible) => _styleButton.gameObject.SetActive(isVisible);

        private void OnEnable()
        {
            _sizeButton.onClick.AddListener(OnClickSizeButton);
            _styleButton.onClick.AddListener(OnClickStyleButton);
            _materialButton.onClick.AddListener(OnClickMaterial);
        }

        private void OnDisable()
        {
            _sizeButton.onClick.RemoveListener(OnClickSizeButton);
            _styleButton.onClick.RemoveListener(OnClickStyleButton);
            _materialButton.onClick.RemoveListener(OnClickMaterial);
        }

        private void OnClickSizeButton() => ToggleSection(_sizeSection.gameObject);
        private void OnClickStyleButton() => ToggleSection(_stylesSection.gameObject);
        private void OnClickMaterial() => ToggleSection(_materialSection.gameObject);

        private void ToggleSection(GameObject sectionToToggle)
        {
            if (_activeSection == sectionToToggle)
            {
                sectionToToggle.SetActive(false);
                _activeSection = null;
            }
            else
            {
                if (_activeSection != null)
                    _activeSection.SetActive(false);

                sectionToToggle.SetActive(true);
                _activeSection = sectionToToggle;
            }
        }

        private void DisableSectionPanel()
        {
            _sizeSection.gameObject.SetActive(false);
            _materialSection.gameObject.SetActive(false);
            _stylesSection.gameObject.SetActive(false);
        }
    }
}
