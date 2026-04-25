using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Outplay.Core;

namespace Outplay.UI
{
    /// <summary>
    /// Renders any IResourceProvider as a fill bar with optional label.
    /// Bind() can be called any time; sync runs in Start so source.Current
    /// is initialized regardless of script execution order.
    /// </summary>
    [DisallowMultipleComponent]
    public class ResourceBar : MonoBehaviour
    {
        [Header("Visuals")]
        [SerializeField] private Image fill;
        [SerializeField] private TextMeshProUGUI label;

        [Header("Text")]
        [SerializeField] private bool showText = true;
        [SerializeField] private string format = "{0} / {1}";

        private IResourceProvider source;
        private bool started;

        public void Bind(IResourceProvider newSource)
        {
            if (source != null) source.OnChanged -= HandleChanged;
            source = newSource;
            if (source != null) source.OnChanged += HandleChanged;
            if (started) Refresh();
        }

        private void Start()
        {
            started = true;
            Refresh();
        }

        private void OnDisable()
        {
            if (source != null) source.OnChanged -= HandleChanged;
        }

        private void Refresh()
        {
            if (source == null) return;
            HandleChanged(source.Current, source.Max);
        }

        private void HandleChanged(float current, float max)
        {
            fill.fillAmount = max > 0f ? current / max : 0f;
            if (showText) label.text = string.Format(format, Mathf.CeilToInt(current), Mathf.CeilToInt(max));
        }
    }
}
