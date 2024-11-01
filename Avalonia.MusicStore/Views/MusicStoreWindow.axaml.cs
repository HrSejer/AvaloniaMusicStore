using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.MusicStore.ViewModels;
using System;
using Avalonia.ReactiveUI;
using ReactiveUI;


namespace Avalonia.MusicStore;

public partial class MusicStoreWindow : ReactiveWindow<MusicStoreViewModel>
{
    public MusicStoreWindow()
    {
        InitializeComponent();

        if (Design.IsDesignMode) return;

        this.WhenActivated(action => action(ViewModel!.BuyMusicCommand.Subscribe(Close)));
    }
}