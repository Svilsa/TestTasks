namespace Auction.Models;

public record struct Lot(uint Id, double Price, double Volume)
{
    public readonly uint Id = Id;
    public readonly double Price = Price;
    public readonly double Volume = Volume;
}