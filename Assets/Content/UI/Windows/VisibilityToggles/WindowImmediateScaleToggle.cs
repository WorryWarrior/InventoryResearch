using RSG;
using RSG.Extensions;
using UnityEngine;

namespace Content.UI.Windows.VisibilityToggles
{
    public class WindowsImmediateScaleToggle : WindowVisibilityToggleBase
    {
        [SerializeField] private RectTransform windowPanel = null;

        public override Promise SetVisibility(bool value)
        {
            base.SetVisibility(value);

            Promise animationPromise = new Promise();

            windowPanel.localScale = value ? Vector3.one : Vector3.zero;

            animationPromise.ResolveIfPending();
            return animationPromise;
        }

        public override void SetInitialVisibility()
        {
            windowPanel.localScale = Vector3.zero;
        }
    }
}