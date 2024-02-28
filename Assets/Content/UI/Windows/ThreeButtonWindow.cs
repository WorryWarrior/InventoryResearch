using RSG;
using UnityEngine;
using UnityEngine.UI;

namespace Content.UI.Windows
{
    public class ThreeButtonWindow : TwoButtonWindow
    {
        [SerializeField] private Button cancelButton = null;

        public override Promise<WindowCloseIntent> Show<T>(T data, string titleText = "")
        {
            cancelButton.onClick.AddListener(Cancel);

            return base.Show(data, titleText);
        }

        protected override void Close()
        {
            cancelButton.onClick.RemoveAllListeners();

            base.Close();
        }

        private void Cancel()
        {
            WindowCloseIntent = WindowCloseIntent.Cancelled;
            Close();
        }
    }
}