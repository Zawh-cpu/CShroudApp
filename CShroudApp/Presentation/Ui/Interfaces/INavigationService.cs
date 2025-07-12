﻿using CShroudApp.Presentation.Ui.ViewModels;

namespace CShroudApp.Presentation.Ui.Interfaces;

public interface INavigationService
{
    event EventHandler<ViewModelBase>? ViewModelChanged;
    TViewModel GoTo<TViewModel>(params object[] args) where TViewModel : ViewModelBase;
    bool Back();
}