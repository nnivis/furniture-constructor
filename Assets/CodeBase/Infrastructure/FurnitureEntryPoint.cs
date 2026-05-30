using CodeBase.Infrastructure.DataProvider;
using CodeBase.Services.FurnitureConstructor;
using CodeBase.UI.FurnitureConstructor;
using UnityEngine;

namespace CodeBase.Infrastructure
{
    public class FurnitureEntryPoint
    {
        private readonly IFurniturePresenter _presenter;
        private readonly FurniturePanel _panel;
        private readonly Material _glassMaterial;

        public FurnitureEntryPoint(IFurniturePresenter presenter, FurniturePanel panel, Material glassMaterial)
        {
            _presenter = presenter;
            _panel = panel;
            _glassMaterial = glassMaterial;
        }

        public void Initialize()
        {
            IFurnitureLoader loader = new FurnitureLoader();
            loader.LoadDatabase();
            IFurnitureFactory factory = new FurnitureFactory(loader, _glassMaterial);

            _presenter.Initialize(factory);
            _panel.Bind(_presenter);
            _presenter.LoadCatalog();
        }
    }
}
