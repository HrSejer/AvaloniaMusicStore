using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Windows.Input;
using ReactiveUI;
using System.Reactive.Concurrency;
using Avalonia.MusicStore.Models;

namespace Avalonia.MusicStore.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            ShowDialog = new Interaction<MusicStoreViewModel, AlbumViewModel?>();

            BuyMusicCommand = ReactiveCommand.CreateFromTask(async() =>
            {
                var store = new MusicStoreViewModel();
                var result = await ShowDialog.Handle(store);
                if (result != null) 
                {
                    Albums.Add(result);
                    await result.SaveToDiskAsync();
                }
            });

            RxApp.MainThreadScheduler.Schedule(LoadAlbums);
        }
        public ICommand BuyMusicCommand { get; }
        public Interaction<MusicStoreViewModel, AlbumViewModel?> ShowDialog { get; }
        public ObservableCollection<AlbumViewModel> Albums { get; } = new();

        private async void LoadAlbums()
        {
            var albums = (await Album.LoadCachedAsync()).Select(X => new AlbumViewModel(X));

            foreach (var album in albums)
            {
                Albums.Add(album);
            }

            foreach (var album in Albums.ToList())
            {
                await album.LoadCover();
            }
        }

    }
}