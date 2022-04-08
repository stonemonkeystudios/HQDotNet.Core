using System;
using System.Collections.Generic;
using System.Text;
using HQDotNet.Model;

namespace HQDotNet.View{
    public interface IViewModelListener<TViewModel> : IDispatchListener where TViewModel : HQViewModel{
        void ViewModelUpdated(TViewModel viewModel);
    }
}
