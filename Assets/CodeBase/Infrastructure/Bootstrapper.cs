using System.Collections.Generic;
using CodeBase.Services.FurnitureConstructor;
using CodeBase.UI.FurnitureConstructor;
using UnityEngine;

namespace CodeBase.Infrastructure
{
    public class Bootstrapper : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _furniturePrefabs;
        [SerializeField] private FurniturePresenter _furniturePresenter;
        [SerializeField] private FurniturePanel _furniturePanel;

        private void Start()
        {
            var entryPoint = new FurnitureEntryPoint(_furniturePresenter, _furniturePanel, _furniturePrefabs);
            entryPoint.Initialize();
        }
    }
}
