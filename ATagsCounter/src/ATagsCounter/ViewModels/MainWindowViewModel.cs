using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using ATagsCounter.Core;
using ATagsCounter.Infrastructure;
using ATagsCounter.Models;

namespace ATagsCounter.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private bool _isProcessed;
        private string _startAndPauseButtonContent = "Start";

        private readonly ATagsCounterService _counterService = new();
        private CancellationTokenSource _cancellationTokenSource = new();

        private IEnumerable<UrlItem>? _urlItems;
        private string _urlFilePath = "./UrlsSample.txt";
        private UrlItem? _maxItem;

        public MainWindowViewModel()
        {
            StartAndPauseCommand = new DelegateCommand(StartOrCancelIfStared);
        }

        public UrlItem? MaxItem
        {
            get => _maxItem;
            set
            {
                if (Equals(value, _maxItem)) return;
                _maxItem = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable<UrlItem>? UrlItems
        {
            get => _urlItems;
            set
            {
                if (Equals(value, _urlItems)) return;
                _urlItems = value;
                OnPropertyChanged();
            }
        }

        public string UrlFilePath
        {
            get => _urlFilePath;
            set
            {
                if (value == _urlFilePath) return;
                _urlFilePath = value;
                OnPropertyChanged();
            }
        }

        public string StartAndPauseButtonContent
        {
            get => _startAndPauseButtonContent;
            set
            {
                _startAndPauseButtonContent = value;
                OnPropertyChanged();
            }
        }

        public DelegateCommand StartAndPauseCommand { get; set; }

        private async void StartOrCancelIfStared()
        {
            try
            {
                switch (_isProcessed)
                {
                    case false:
                    {
                        _isProcessed = true;
                        StartAndPauseButtonContent = "Cancel";

                        UrlItems = await Task.Run(() => FileParser.ParseUriFromTxt(UrlFilePath));
                        await _counterService.CountATagsAsync(UrlItems, _cancellationTokenSource.Token);
                        MaxItem = UrlItems.OrderByDescending(url => url.ATagsCount).First();

                        break;
                    }
                    case true:
                    {
                        _cancellationTokenSource.Cancel();
                        _cancellationTokenSource = new CancellationTokenSource();

                        break;
                    }
                }

                StartAndPauseButtonContent = "Start";
                _isProcessed = false;
            }
            catch (OperationCanceledException)
            {
                UrlItems = null;
                MaxItem = null;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler? PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}