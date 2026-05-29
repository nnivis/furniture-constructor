using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace CodeBase.Data.FurnitureConstructor
{
    [Serializable]
    public class Database
    {
        public List<Category> modelsDB;
    }

    [Serializable]
    public class Category
    {
        public string category;
        public List<FurnitureData> objects;
    }

    [Serializable]
    public class FurnitureData
    {
        public string name;
        public float start;
        public List<Morph> morph;
        public List<CustomMaterial> materials;
        public List<Style> styles;

        [NonSerialized] public Dictionary<string, PartData> Parts = new Dictionary<string, PartData>();

        [NonSerialized] public Dictionary<MorphType, Dictionary<string, Vector2[]>> MorphUVs =
            new Dictionary<MorphType, Dictionary<string, Vector2[]>>();

        public float SizeToInfluence(float size, float startValueInMeters)
        {
            float startInches = startValueInMeters * 39.37f;
            return (startInches - size * 39.37f) / (startInches - 300f);
        }

        public void AddUV(MorphType type, string objectName, Vector2[] uv)
        {
            int morphIndex = objectName.IndexOf(FurnitureConstants.MorphSuffix, StringComparison.Ordinal);
            if (morphIndex >= 0)
            {
                objectName = objectName.Substring(0, morphIndex);
            }

            if (!MorphUVs.ContainsKey(type))
                MorphUVs[type] = new Dictionary<string, Vector2[]>();

            MorphUVs[type][objectName] = uv;
        }

        public Vector2[] GetUV(MorphType type, string objectName)
        {
            if (MorphUVs.TryGetValue(type, out var uvDict) && uvDict.TryGetValue(objectName, out var uv))
                return uv;

            if (uvDict == null)
                return null;

            foreach (var key in uvDict.Keys)
            {
                if (key.StartsWith(objectName))
                    return uvDict[key];
            }

            return null;
        }


        public void AddMaterial(string partName, MaterialInfo material)
        {
            if (!Parts.ContainsKey(partName))
                Parts[partName] = new PartData();

            Parts[partName].materials.Add(material);
        }

        public void AddStyle(string partName, string styleKey, StyleInfo style)
        {
            if (!Parts.ContainsKey(partName)) Parts[partName] = new PartData();

            if (!Parts[partName].styles.ContainsKey(styleKey))
                Parts[partName].styles[styleKey] = new List<StyleInfo>();

            if (!Parts[partName].styles[styleKey]
                    .Exists(s => s.label == style.label && s.nameInModel == style.nameInModel))
                Parts[partName].styles[styleKey].Add(style);
        }
    }

    [Serializable]
    public readonly struct Morph
    {
        public readonly string label;
        public readonly float min;
        public readonly float max;

        [JsonConstructor]
        public Morph(string label, float min, float max)
        {
            this.label = label;
            this.min = min;
            this.max = max;
        }
    }

    [Serializable]
    public class CustomMaterial
    {
        public string label;
        public string types;
        [JsonProperty("name_in_model")]
        public string nameInModel;
        public List<TypeInfo> typeInfo;
    }

    [Serializable]
    public class Style
    {
        public string label;
        public string types;
        public List<TypeInfo> typeInfo;
    }

    [Serializable]
    public readonly struct TypeInfo
    {
        public readonly string label;
        [JsonProperty("name_in_model")]
        public readonly string nameInModel;
        public readonly string texture;
        public readonly float width;
        public readonly float height;

        [JsonConstructor]
        public TypeInfo(string label, string nameInModel, string texture, float width, float height)
        {
            this.label = label;
            this.nameInModel = nameInModel;
            this.texture = texture;
            this.width = width;
            this.height = height;
        }
    }

    [Serializable]
    public class PartData
    {
        public Morph? morphInfo;
        public List<MaterialInfo> materials = new List<MaterialInfo>();
        public Dictionary<string, List<StyleInfo>> styles = new Dictionary<string, List<StyleInfo>>();
    }

    [Serializable]
    public class MaterialInfo
    {
        public string label;
        public string nameInModel;
        public string texturePath;
        public string textureLabel;

        public float width;
        public float height;
    }

    [Serializable]
    public readonly struct StyleInfo
    {
        public readonly string label;
        public readonly string nameInModel;

        public StyleInfo(string label, string nameInModel)
        {
            this.label = label;
            this.nameInModel = nameInModel;
        }
    }

    public enum MorphType
    {
        Height,
        Width,
        Depth
    }
}