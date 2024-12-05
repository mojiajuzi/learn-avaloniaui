using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using MyMoney.Models;

namespace MyMoney.ViewModels;

public partial class WorkViewModel : ViewModelBase
{
    public ObservableCollection<Work> Works { get; set; }

    [ObservableProperty] private Work _workData;

    public WorkViewModel()
    {
        Works = new ObservableCollection<Work>();
        WorkData = new Work();
    }

    public void AddWork()
    {
        Works.Add(WorkData);
    }
}