using CodeBase.Services.FurnitureConstructor;
using CodeBase.UI.FurnitureConstructor;
using UnityEngine;

namespace CodeBase.Infrastructure
{
    public class Bootstrapper : MonoBehaviour
    {
        [SerializeField] private FurniturePresenter _furniturePresenter;
        [SerializeField] private FurniturePanel _furniturePanel;

        private void Start()
        {
            var entryPoint = new FurnitureEntryPoint(_furniturePresenter, _furniturePanel);
            entryPoint.Initialize();
        }
    }
}
