global using Updates = Auction.Infrastructure.ListPool<Auction.Models.LotUpdate>;
using System;

namespace Auction.Models;

public enum Action
{
    Add,
    Change,
    Del
}

public struct LotUpdate : IComparable<LotUpdate>
{
    public readonly Action Action;
    public Lot Lot;

    public LotUpdate(Action action, Lot lot)
    {
        Action = action;
        Lot = lot;
    }
        
    public int CompareTo(LotUpdate other)
    {
        if (Lot.Id < other.Lot.Id) return -1;
        if (Lot.Id > other.Lot.Id) return 1;
        return 0;
    }
}