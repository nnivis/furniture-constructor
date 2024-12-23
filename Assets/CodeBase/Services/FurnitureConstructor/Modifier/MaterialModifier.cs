using System.Collections.Generic;
using System.Linq;
using CodeBase.Services.FurnitureConstructor.Data;
using UnityEngine;

namespace CodeBase.Services.FurnitureConstructor.Modifier
{
    public class MaterialModifier
    {
        private const int I = 4;
        private Material _glassMaterial;
        public MaterialModifier(Material material) => _glassMaterial = material;

        public void InitializeMaterial(FurnitureData data, GameObject prefab)
        {
            if (data == null || prefab == null)
                return;

            foreach (var part in data.Parts.Values)
            {
                foreach (var materialGroup in part.materials.GroupBy(m => m.label))
                {
                    var firstMaterial = materialGroup.FirstOrDefault();
                    if (firstMaterial == null)
                        continue;

                    ApplyMaterialToPrefab(prefab, data, firstMaterial.nameInModel, firstMaterial.texturePath);
                }
            }
        }

        public void SetMaterialByLabel(FurnitureData data, GameObject prefab, string label, string textureName)
        {
            if (data == null || prefab == null)
            {
                return;
            }

            MaterialInfo targetMaterial = data.Parts.Values
                .SelectMany(part => part.materials)
                .FirstOrDefault(m => m.label == label && m.textureLabel == textureName);

            if (targetMaterial == null)
            {
                return;
            }

            string texturePath = targetMaterial.texturePath;
            if (string.IsNullOrEmpty(texturePath))
            {
                return;
            }

            ApplyMaterialToPrefab(prefab, data, targetMaterial.nameInModel, texturePath);
        }

        private void ApplyMaterialToPrefab(GameObject prefab, FurnitureData data, string nameInModel,
            string texturePath)
        {
            var renderers = prefab.GetComponentsInChildren<SkinnedMeshRenderer>(true);
            HashSet<Renderer> processedRenderers = new HashSet<Renderer>();

            Texture2D texture = null;
            if (!string.IsNullOrEmpty(texturePath))
            {
                texture = Resources.Load<Texture2D>(texturePath);
            }

            foreach (var renderer in renderers)
            {
                if (!processedRenderers.Contains(renderer))
                {
                    if (renderer.name.Contains(nameInModel))
                    {
                        if (renderer.sharedMaterial != null)
                        {
                            Material materialCopy = Object.Instantiate(renderer.sharedMaterial);

                            if (texture != null)
                            {
                                materialCopy.SetTexture("baseColorTexture", texture);
                                ApplyTextureTiling(materialCopy, data, texturePath);
                            }

                            renderer.sharedMaterial = materialCopy;
                        }
                    }

                    if (renderer.name.Contains("acrylic") || renderer.name.Contains("glass"))
                    {
                        ApplyAcrylicMaterial(prefab, "acrylic");
                    }

                    processedRenderers.Add(renderer);
                }
            }
        }


        private void ApplyAcrylicMaterial(GameObject prefab, string acrylicObjectName)
        {
            // Получаем все SkinnedMeshRenderer из дочерних объектов
            var renderers = prefab.GetComponentsInChildren<SkinnedMeshRenderer>(true);

            if (_glassMaterial == null)
            {
                Debug.LogWarning("Glass material is not set. Please assign a material to _glassMaterial.");
                return;
            }

            foreach (var renderer in renderers)
            {
                // Проверяем, содержит ли объект в имени искомую строку
                if (!string.IsNullOrEmpty(acrylicObjectName) && renderer.name.Contains(acrylicObjectName))
                {
                    renderer.sharedMaterial = _glassMaterial;
                    Debug.Log($"Applied glass material to: {renderer.name}");
                }
            }
        }

        public void ApplyTextureTiling(Material material, FurnitureData data, string texturePath)
        {
            MaterialInfo materialInfo = data.Parts.Values
                .SelectMany(part => part.materials)
                .FirstOrDefault(m => m.texturePath == texturePath);

            if (materialInfo == null)
            {
                return;
            }

            material.SetTextureScale("baseColorTexture", new Vector2(materialInfo.width / I, materialInfo.height / I));
        }
    }
}