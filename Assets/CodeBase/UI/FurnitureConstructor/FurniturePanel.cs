using System.Collections.Generic;
using System.Linq;
using CodeBase.Data.FurnitureConstructor;
using CodeBase.Domain.FurnitureConstructor;
using CodeBase.Services.FurnitureConstructor;
using UnityEngine;

namespace CodeBase.UI.FurnitureConstructor
{
    public class FurniturePanel : MonoBehaviour
    {
        [Header("Section")]
        [SerializeField] private FurnitureVisibilityPanel _furnitureVisibilityPanel;
        [SerializeField] private DropDownSectionView _modelSection;
        [SerializeField] private SliderSectionView _sizeSection;
        [SerializeField] private DropDownSectionView _materialSection;
        [SerializeField] private DropDownSectionView _stylesSection;

        [Header("Prefab")]
        [SerializeField] private DropDownView _dropDownViewPrefab;

        private IFurniturePresenter _presenter;
        private FurnitureData _currentFurniture;

        public void Bind(IFurniturePresenter presenter)
        {
            _presenter = presenter;
            _presenter.FurnitureCreated += OnFurnitureCreated;
            _presenter.FurnitureSelected += OnFurnitureSelected;
            _furnitureVisibilityPanel.Initialize(_sizeSection, _materialSection, _stylesSection);
        }

        public void Unbind()
        {
            _presenter.FurnitureCreated -= OnFurnitureCreated;
            _presenter.FurnitureSelected -= OnFurnitureSelected;
            _presenter = null;
        }

        private void OnFurnitureCreated(Furniture furniture)
        {
            _currentFurniture = furniture.Data;
            UpdateModelSection();
            UpdateModifierSection();
        }

        private void OnFurnitureSelected(Furniture furniture)
        {
            _currentFurniture = furniture.Data;
            UpdateModifierSection();
        }

        private void UpdateModifierSection()
        {
            UpdateSizeSection();
            UpdateMaterialSection();
            UpdateStyleSection();
            _furnitureVisibilityPanel.SetStyleIconVisibility(HasStyles());
        }

        private bool HasStyles()
        {
            if (_currentFurniture?.Parts == null) return false;
            foreach (var part in _currentFurniture.Parts.Values)
                if (part.styles?.Count > 0) return true;
            return false;
        }

        private void UpdateModelSection()
        {
            _modelSection.Clear();
            if (_presenter?.Furnitures == null || !_presenter.Furnitures.Any()) return;

            var names = _presenter.Furnitures.Select(f => f.Prefab.name).ToList();
            _modelSection.AddDropDownView(_dropDownViewPrefab, "Select Model", names, OnModelChanged);
            _modelSection.SetSelectedValue(0, names.Last());
        }

        private void UpdateSizeSection()
        {
            if (_currentFurniture == null) return;

            var morphes = _currentFurniture.Parts.Values.Select(p => p.morphInfo).ToList();
            var heightMorph = morphes.FirstOrDefault(m => m?.label == "Height");
            var widthMorph = morphes.FirstOrDefault(m => m?.label == "Width");
            var depthMorph = morphes.FirstOrDefault(m => m?.label == "Depth");

            _sizeSection.InitializeSliders(
                heightMorph?.min ?? 0.5f, heightMorph?.max ?? 2f,
                widthMorph?.min ?? 0.5f, widthMorph?.max ?? 2f,
                depthMorph?.min ?? 0.5f, depthMorph?.max ?? 2f
            );

            _sizeSection.OnSizeChanged -= OnSizeChanged;
            _sizeSection.OnSizeChanged += OnSizeChanged;
        }

        private void UpdateMaterialSection()
        {
            _materialSection.Clear();
            if (_currentFurniture?.Parts == null) return;

            foreach (var part in _currentFurniture.Parts)
            {
                if (part.Value.materials?.Any() != true) continue;

                var options = part.Value.materials.Select(m => m.textureLabel ?? "None").ToList();
                _materialSection.AddDropDownView(_dropDownViewPrefab, part.Key, options,
                    selected => OnMaterialChanged(part.Key, selected));
                _materialSection.SetSelectedValue(_materialSection.DropDownViews.Count - 1, options.First());
            }
        }

        private void UpdateStyleSection()
        {
            _stylesSection.Clear();
            if (_currentFurniture?.Parts == null) return;

            foreach (var part in _currentFurniture.Parts)
            {
                foreach (var styleEntry in part.Value.styles)
                {
                    var styleKey = styleEntry.Key;
                    var styleOptions = styleEntry.Value.Select(s => s.label).ToList();

                    _stylesSection.AddDropDownView(_dropDownViewPrefab, styleKey, styleOptions,
                        selected => OnStyleChanged(styleKey, selected));

                    if (styleOptions.Any())
                        _stylesSection.SetSelectedValue(_stylesSection.DropDownCount - 1, styleOptions.First());
                }
            }
        }

        private void OnModelChanged(string selectedModelName)
        {
            var selected = _presenter?.Furnitures?.FirstOrDefault(f => f.Prefab.name == selectedModelName);
            if (selected != null)
                _presenter.SelectFurniture(selected);
        }

        private void OnMaterialChanged(string partName, string textureName) =>
            _presenter?.ApplyMaterial(partName, textureName);

        private void OnStyleChanged(string styleKey, string styleLabel) =>
            _presenter?.ApplyStyle(styleKey, styleLabel);

        private void OnSizeChanged(MorphType type, float value) =>
            _presenter?.ApplySize(type, value);
    }
}
