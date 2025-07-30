using Avalonia.Controls;
using Avalonia.Controls.Templates;
using CShroudApp.Desktop.ViewModels;

namespace CShroudApp.Desktop;

public class ViewLocator : IDataTemplate
{
    private static Dictionary<string, Type> _viewTypes = new()
    {
        ["LoginViewModel"] = typeof(LoginViewModel),
        ["MainWindowViewModel"] = typeof(MainWindowViewModel),
        ["AppViewModel"] = typeof(AppViewModel)
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