using CommunityToolkit.Mvvm.ComponentModel;
using MyMoney.Models;

namespace MyMoney.ViewModels;

public partial class TagViewModel:ViewModelBase
{
    [ObservableProperty] private string _tag;
    [ObservableProperty] private string _namel;
    [ObservableProperty] private TagStatus _status;
}