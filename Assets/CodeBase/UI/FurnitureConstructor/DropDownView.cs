using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace CodeBase.UI.FurnitureConstructor
{
    public class DropDownView : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown _dropdown;
        [SerializeField] private TextMeshProUGUI _label;

        public void SetLabel(string text) { if (_label != null) _label.text = text; }

        public void SetOptions(List<string> options)
        {
            if (_dropdown != null)
            {
                _dropdown.ClearOptions();
                _dropdown.AddOptions(options);
            }
        }

        public void AddOnValueChangedListener(UnityEngine.Events.UnityAction<int> callback)
        {
            if (_dropdown != null)
                _dropdown.onValueChanged.AddListener(callback);
        }

        public void SetSelectedValue(string value)
        {
            if (_dropdown != null)
            {
                var index = _dropdown.options.FindIndex(option => option.text == value);
                if (index >= 0)
                    _dropdown.SetValueWithoutNotify(index);
            }
        }
    }
}
