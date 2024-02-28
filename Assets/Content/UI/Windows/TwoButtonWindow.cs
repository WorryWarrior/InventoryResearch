using RSG;
using UnityEngine;
using UnityEngine.UI;

namespace Content.UI.Windows
{
    public class TwoButtonWindow : OneButtonWindow
    {
        [SerializeField] private Button rejectButton = null;

        public override Promise<WindowCloseIntent> Show<T>(T data, string titleText = "")
        {
            rejectButton.onClick.AddListener(Reject);

            return base.Show(data, titleText);
        }

        protected override void Close()
        {
            rejectButton.onClick.RemoveAllListeners();

            base.Close();
        }

        private void Reject()
        {
            WindowCloseIntent = WindowCloseIntent.Rejected;
            Close();
        }
    }
}