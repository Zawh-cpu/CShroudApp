using Avalonia.Controls;
using Avalonia.Controls.Templates;
using CShroudApp.Presentation.Ui.ViewModels;
using CShroudApp.Presentation.Ui.Views;
using CShroudApp.Presentation.Ui.Views.Auth;

namespace CShroudApp.Presentation.Ui;

public class ViewLocator : IDataTemplate
{
    private static Dictionary<string, Type> _viewTypes = new()
    {
        ["DashboardView"] = typeof(DashboardView),
        ["LoginView"] = typeof(LoginView),
        ["QuickLoginView"] = typeof(QuickLoginView),
    };
    
    public Control Build(object? data)
    {
        if (data is null)
        {
            return new TextBlock { Text = "data was null" };
        }
            
        //var name = data.GetType().FullName!.Replace("ViewModel", "View").Split(".").Last();
        //var type = _viewTypes.GetValueOrDefault(name);
        
        var name = data.GetType().FullName!.Replace("ViewModel", "View");
        var type = Type.GetType(name);

        if (type != null)
        {
            // ReSharper disable once HeapView.ObjectAllocation
            var instance = (Control)Activator.CreateInstance(type)!;
            instance.DataContext = data;
            return instance;
        }
        else
        {
            return new TextBlock { Text = "Not Found ---: " + name };
        }
    }

    public bool Match(object? data)
    {
        return data is ViewModelBase;
    }
}