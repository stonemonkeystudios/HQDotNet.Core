using System;
using System.Collections.Generic;
using System.Text;
using HQ.Model;

namespace HQ.View{
    public interface IViewModelListener<TViewModel> : IDispatchListener where TViewModel : HQViewModel{
        void ViewModelUpdated(TViewModel viewModel);
    }
}
