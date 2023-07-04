using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ATagsCounter.Infrastructure;

namespace ATagsCounter.Models;

public enum LoadingStatus
{
    Processing,
    Loaded,
    Error
}

public sealed class UrlItem : INotifyPropertyChanged
{
    private LoadingStatus _loadingStatus = LoadingStatus.Processing;
    private uint _aTagsCount;

    public UrlItem(Uri uri)
    {
        Uri = uri;
    }

    public Uri Uri { get; }

    public LoadingStatus LoadingStatus
    {
        get => _loadingStatus;
        set
        {
            if (value == _loadingStatus) return;
            _loadingStatus = value;
            OnPropertyChanged();
        }
    }

    public uint ATagsCount
    {
        get => _aTagsCount;
        set
        {
            if (value == _aTagsCount) return;
            _aTagsCount = value;
            OnPropertyChanged();
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