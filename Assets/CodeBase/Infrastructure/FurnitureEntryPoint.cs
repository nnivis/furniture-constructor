using System.Collections.Generic;
using CodeBase.Services.FurnitureConstructor;
using CodeBase.UI.FurnitureConstructor;
using UnityEngine;

namespace CodeBase.Infrastructure
{
    public class FurnitureEntryPoint
    {
        private readonly IFurniturePresenter _presenter;
        private readonly FurniturePanel _panel;
        private readonly IReadOnlyList<GameObject> _prefabs;

        public FurnitureEntryPoint(IFurniturePresenter presenter, FurniturePanel panel, IReadOnlyList<GameObject> prefabs)
        {
            _presenter = presenter;
            _panel = panel;
            _prefabs = prefabs;
        }

        public void Initialize()
        {
            _presenter.Initialize();
            _panel.Bind(_presenter);

            foreach (var prefab in _prefabs)
                _presenter.GenerateFurniture(prefab);
        }
    }
}
