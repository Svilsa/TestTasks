using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ATagsCounter.Models;

namespace ATagsCounter.Core;

public class ATagsCounterService
{
    private readonly SemaphoreSlim _semaphore = new (5, 5);

    public async Task CountATagsAsync(IEnumerable<UrlItem> urlItems, CancellationToken cancellationToken = default)
    {
        var taskList = new List<Task>();
        
        foreach (var urlItem in urlItems)
        {
            await _semaphore.WaitAsync(cancellationToken);
            taskList.Add(Task.Run(() => CountATagsFromStream(urlItem, cancellationToken), cancellationToken));
        }

        await Task.WhenAll(taskList);
    }

    private async Task CountATagsFromStream(UrlItem urlItem, CancellationToken cancellationToken = default)
    {
        HttpClient? httpClient = null;
        Stream? stream = null;
        StreamReader? sr = null;

        try
        {
            httpClient = new HttpClient();
            stream = await httpClient.GetStreamAsync(urlItem.Uri);
            sr = new StreamReader(stream);

            var counter = 0u;

            while (!sr.EndOfStream)
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (sr.Read() == '<' && sr.Read() == 'a')
                    counter++;
            }
            
            urlItem.ATagsCount = counter;
            urlItem.LoadingStatus = LoadingStatus.Loaded;
        }
        catch (Exception)
        {
            urlItem.LoadingStatus = LoadingStatus.Error;
        }
        finally
        {
            httpClient?.Dispose();
            stream?.Dispose();
            sr?.Dispose();
            _semaphore.Release();
        }
    }
}