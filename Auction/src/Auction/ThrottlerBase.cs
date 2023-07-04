namespace Auction;

public abstract class ThrottlerBase
{
    public abstract void PutUpdates(string auctionName, Updates updates);
    public abstract Updates? PopUpdates(string auctionName);
}