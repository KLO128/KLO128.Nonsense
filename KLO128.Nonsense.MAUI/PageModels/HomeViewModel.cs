using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreshMvvm.Maui;
using KLO128.Nonsense.Common.Abstract;

namespace KLO128.Nonsense.MAUI.PageModels
{
    public class HomeViewModel : FreshBasePageModel
    {
        public string InputCode { get; set; }

        public string Tokens { get; set; }

        public Command GetTokensCommand { get; }

        public HomeViewModel(ISomeAdapter adapter)
        {
            GetTokensCommand = new Command(() =>
            {
                Tokens = string.Join("\r\n", adapter.LoadTokens(InputCode).Select(x => x.ToString()).ToArray());
                OnPropertyChanged(nameof(Tokens));
            });
        }
    }
}
