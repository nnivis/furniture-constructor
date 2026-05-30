using CodeBase.Services.FurnitureConstructor;
using CodeBase.UI.FurnitureConstructor;

namespace CodeBase.Infrastructure
{
    public class FurnitureEntryPoint
    {
        private readonly IFurniturePresenter _presenter;
        private readonly FurniturePanel _panel;

        public FurnitureEntryPoint(IFurniturePresenter presenter, FurniturePanel panel)
        {
            _presenter = presenter;
            _panel = panel;
        }

        public void Initialize()
        {
            _presenter.Initialize();
            _panel.Bind(_presenter);
            _presenter.LoadCatalog();
        }
    }
}
