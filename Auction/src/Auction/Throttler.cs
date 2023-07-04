using System;
using System.Collections.Concurrent;
using Auction.Models;
using Action = Auction.Models.Action;

namespace Auction;

public class Throttler : ThrottlerBase
{
    private readonly ConcurrentDictionary<string, Updates> _innerDictionary = new();

    public override void PutUpdates(string auctionName, Updates updates)
    {
        while (true)
        {
            if (_innerDictionary.TryGetValue(auctionName, out var oldUpdates))
            {
                // key exists, try to update
                var newUpdates = MergeUpdates(oldUpdates, updates);
                if (_innerDictionary.TryUpdate(auctionName, newUpdates, oldUpdates))
                {
                    updates.Dispose();
                    oldUpdates.Dispose();
                    break;
                }
            }
            else
            {
                // key doesn't exist, try to add
                if (_innerDictionary.TryAdd(auctionName, updates))
                {
                    updates.Sort();
                    break;
                }
            }
        }
    }

    public override Updates? PopUpdates(string auctionName)
    {
        _innerDictionary.TryRemove(auctionName, out var auctionLots);
        return auctionLots;
    }

    private static Updates MergeUpdates(Updates oldUpdates, Updates newUpdates)
    {
        var mergedUpdates = new Updates();
        var i = 0;
        var isUpdatesCountEqual = newUpdates.Count == oldUpdates.Count;
        var isNewUpdatesCountMoreThenOldOnes = newUpdates.Count > oldUpdates.Count;

        newUpdates.Sort();

        for (; i < (isNewUpdatesCountMoreThenOldOnes ? oldUpdates.Count : newUpdates.Count); i++)
        {
            var newUpdate = newUpdates[i];
            var oldUpdate = oldUpdates[i];

            if (newUpdate.Lot.Id == oldUpdate.Lot.Id)
            {
                switch (oldUpdate.Action)
                {
                    case Action.Add:
                        if (newUpdate.Action == Action.Del)
                            break;

                        mergedUpdates.Add(
                            new LotUpdate(Action.Add,
                                new Lot(oldUpdate.Lot.Id, newUpdate.Lot.Price, newUpdate.Lot.Volume)));
                        break;

                    case Action.Change:
                        if (newUpdate.Action == Action.Del)
                        {
                            mergedUpdates.Add(
                                new LotUpdate(Action.Del,
                                    new Lot(oldUpdate.Lot.Id, 0, 0)));
                            break;
                        }

                        mergedUpdates.Add(
                            new LotUpdate(Action.Change,
                                new Lot(oldUpdate.Lot.Id, newUpdate.Lot.Price, newUpdate.Lot.Volume)));
                        break;

                    case Action.Del:
                        if (newUpdate.Action == Action.Del)
                        {
                            mergedUpdates.Add(
                                new LotUpdate(Action.Del,
                                    new Lot(oldUpdate.Lot.Id, 0, 0)));
                            break;
                        }

                        mergedUpdates.Add(
                            new LotUpdate(Action.Change,
                                new Lot(oldUpdate.Lot.Id, newUpdate.Lot.Price, newUpdate.Lot.Volume)));
                        break;

                    default:
                        throw new ArgumentException();
                }

                continue;
            }

            mergedUpdates.Add(oldUpdate);
            mergedUpdates.Add(newUpdate);
        }

        if (!isUpdatesCountEqual)

            if (isNewUpdatesCountMoreThenOldOnes)
                for (; i < newUpdates.Count; i++)
                    mergedUpdates.Add(newUpdates[i]);

            else
                for (; i < oldUpdates.Count; i++)
                    mergedUpdates.Add(oldUpdates[i]);

        mergedUpdates.Sort();
        return mergedUpdates;
    }
}