using System.Text;
using CodeBase.Data.FurnitureConstructor;
using CodeBase.Domain.FurnitureConstructor.Modifiers;
using UnityEngine;

namespace CodeBase.Domain.FurnitureConstructor
{
    public class Furniture : MonoBehaviour
    {
        public GameObject Prefab { get; private set; }
        public FurnitureData Data { get; private set; }

        private Modifier _modifier;

        public void Initialize(GameObject prefab, FurnitureData data, Modifier modifier)
        {
            Prefab = prefab;
            Data = data;
            _modifier = modifier;

            _modifier.SetStartModifier(Data, gameObject);

           // DebugFurnitureData();
        }


        public void ApplyNewMaterial(string label, string textureName) =>
            _modifier.SetMaterialByLabel(Data, Prefab, label, textureName);

        public void ApplyNewStyle(string key, string label) =>
            _modifier.SetStyleByKeyAndLabel(Data, Prefab, key, label);

        public void ApplyNewSize(MorphType type, float value) => _modifier.SetSize(Data, Prefab, type, value);

        private void DebugFurnitureData()
        {
            if (Data == null)
            {
                Debug.LogError("FurnitureData is null");
                return;
            }

            var sb = new StringBuilder();
            sb.AppendLine($"Furniture Name: {Data.name}");
            sb.AppendLine($"Start Value: {Data.start}");
            sb.AppendLine("Parts:");

            foreach (var part in Data.Parts)
            {
                sb.AppendLine($"  Part Name: {part.Key}");
                if (part.Value.morphInfo.HasValue)
                {
                    var morph = part.Value.morphInfo.Value;
                    sb.AppendLine($"    Morph Info: Label={morph.label}, Min={morph.min}, Max={morph.max}");
                }

                sb.AppendLine("    Materials:");
                foreach (var material in part.Value.materials)
                {
                    sb.AppendLine(
                        $"      Material: Label={material.label}, NameInModel={material.nameInModel}, TexturePath={material.texturePath}, TextureLabel={material.textureLabel}, Width={material.width}, Height={material.height}");
                }

                sb.AppendLine("    Styles:");
                foreach (var style in part.Value.styles)
                {
                    sb.AppendLine($"      Style Key: {style.Key}");
                    foreach (var styleInfo in style.Value)
                    {
                        sb.AppendLine($"        Label: {styleInfo.label}, NameInModel: {styleInfo.nameInModel}");
                    }
                }
            }

            sb.AppendLine("Morph UVs:");
            foreach (var morphType in Data.MorphUVs)
            {
                sb.AppendLine($"  Morph Type: {morphType.Key}");
                foreach (var uv in morphType.Value)
                {
                    sb.AppendLine($"    Object: {uv.Key}, UV Count: {uv.Value.Length}");
                }
            }

            Debug.Log(sb.ToString());
        }
    }
}