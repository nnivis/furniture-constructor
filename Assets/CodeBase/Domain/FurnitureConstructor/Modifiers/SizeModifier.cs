using UnityEngine;
using CodeBase.Data.FurnitureConstructor;
using System.Collections.Generic;

namespace CodeBase.Domain.FurnitureConstructor.Modifiers
{
    public class SizeModifier
    {
        private Dictionary<MorphType, float> _sizes;
        private Dictionary<MorphType, float> _influences;

        private Dictionary<SkinnedMeshRenderer, Vector2[]> _originalUVsDict;
        private BlockStackModifier _blockStackModifier;

        public void InitializeSize(FurnitureData data, GameObject prefab)
        {
            _blockStackModifier = new BlockStackModifier();

            _sizes = new Dictionary<MorphType, float>
            {
                [MorphType.Height] = FindMorph(data, MorphType.Height)?.min ?? 0.72f,
                [MorphType.Width]  = FindMorph(data, MorphType.Width)?.min  ?? 1.52f,
                [MorphType.Depth]  = FindMorph(data, MorphType.Depth)?.min  ?? 0.91f,
            };

            SaveOriginalUVs(prefab);
            UpdateInfluences(data, prefab);
            UpdateUVs(data, prefab);

            _blockStackModifier.UpdateBlocksByHeight(prefab, _sizes[MorphType.Height]);
        }

        public void SetSize(FurnitureData data, GameObject prefab, MorphType type, float newValue)
        {
            if (_sizes == null)
                return;

            _sizes[type] = newValue;
            UpdateInfluences(data, prefab);
            UpdateUVs(data, prefab);

            if (type == MorphType.Height)
                _blockStackModifier.UpdateBlocksByHeight(prefab, newValue);
        }

        private void SaveOriginalUVs(GameObject prefab)
        {
            _originalUVsDict = new Dictionary<SkinnedMeshRenderer, Vector2[]>();
            var renderers = prefab.GetComponentsInChildren<SkinnedMeshRenderer>();
            foreach (var renderer in renderers)
            {
                if (renderer.sharedMesh != null)
                    _originalUVsDict[renderer] = (Vector2[])renderer.sharedMesh.uv.Clone();
            }
        }

        private void UpdateUVs(FurnitureData data, GameObject prefab)
        {
            var renderers = prefab.GetComponentsInChildren<SkinnedMeshRenderer>();
            foreach (var renderer in renderers)
            {
                if (renderer.sharedMesh == null || !_originalUVsDict.TryGetValue(renderer, out var originalUVs))
                    continue;

                Vector2[] newUVs = (Vector2[])originalUVs.Clone();

                foreach (MorphType type in System.Enum.GetValues(typeof(MorphType)))
                {
                    float influence = _influences[type];
                    if (influence == 0f) continue;

                    string objectName = renderer.gameObject.name;
                    Vector2[] morphUVs = data.GetUV(type, objectName);

                    if (morphUVs != null && morphUVs.Length == newUVs.Length)
                    {
                        for (int i = 0; i < newUVs.Length; i++)
                            newUVs[i] += (morphUVs[i] - originalUVs[i]) * influence;
                    }
                }

                Mesh updatedMesh = Object.Instantiate(renderer.sharedMesh);
                updatedMesh.name = renderer.sharedMesh.name + "_UpdatedUVs";
                updatedMesh.uv = newUVs;
                updatedMesh.RecalculateBounds();
                renderer.sharedMesh = updatedMesh;
            }
        }

        private void UpdateInfluences(FurnitureData data, GameObject prefab)
        {
            _influences = new Dictionary<MorphType, float>();
            foreach (MorphType type in System.Enum.GetValues(typeof(MorphType)))
                _influences[type] = data.SizeToInfluence(_sizes[type], data.start);

            UpdateMorphInfluences(prefab);
        }

        private void UpdateMorphInfluences(GameObject prefab)
        {
            var renderers = prefab.GetComponentsInChildren<SkinnedMeshRenderer>(true);
            foreach (var skinnedRenderer in renderers)
            {
                if (skinnedRenderer.sharedMesh == null || skinnedRenderer.sharedMesh.blendShapeCount < 3)
                    continue;

                foreach (MorphType type in System.Enum.GetValues(typeof(MorphType)))
                {
                    int index = MorphTypeToIndex(type);
                    if (index >= 0)
                        skinnedRenderer.SetBlendShapeWeight(index, _influences[type]);
                }
            }
        }

        private Morph? FindMorph(FurnitureData data, MorphType type)
        {
            foreach (var part in data.Parts.Values)
            {
                if (part.morphInfo.HasValue &&
                    part.morphInfo.Value.label.Equals(type.ToString(), System.StringComparison.OrdinalIgnoreCase))
                    return part.morphInfo;
            }

            return null;
        }

        private int MorphTypeToIndex(MorphType type) => type switch
        {
            MorphType.Height => 0,
            MorphType.Width  => 1,
            MorphType.Depth  => 2,
            _                => -1
        };
    }
}
