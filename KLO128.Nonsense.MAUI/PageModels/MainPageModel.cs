using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreshMvvm.Maui;
using KLO128.Nonsense.Common.Abstract;

namespace KLO128.Nonsense.MAUI.PageModels
{
    public class MainPageModel : FreshBasePageModel
    {
        public HomeViewModel HomeViewModel { get; set; }

        public MainPageModel(ISomeAdapter adapter)
        {
            HomeViewModel = new HomeViewModel(adapter);
        }
    }
}
