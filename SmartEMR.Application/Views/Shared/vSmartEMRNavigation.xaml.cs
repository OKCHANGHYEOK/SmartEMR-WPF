using DevExpress.Mvvm;
using SmartEMR.Application.Xpf;
using System.Windows.Controls;
using System.Windows.Input;

namespace SmartEMR.Application.Views.Shared
{
    /// <summary>
    /// vSmartEMRNavigation.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class vSmartEMRNavigation : CustomControl
    {
        public ICommand NavCommand { get; }

        public vSmartEMRNavigation()
        {
            NavCommand = new DelegateCommand<string>(OnNavExecute);
        }

        private void OnNavExecute(string place)
        {
            switch (place)
            {
                case "RES":
                    break;

                case "DSK":
                    break;

                case "CHT":
                    break;

                case "PAY":
                    break;

                case "PAT":
                    break;

                case "CFG":
                    break;
            }
        }
    }
}
