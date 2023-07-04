using System.Linq;
using Auction.Models;
using Xunit;
using Action = Auction.Models.Action;

namespace Auction.Tests;

public class AdditionalTests
{
    private const string AuctionName = "au1";
    
    [Fact]
    public void NullResultTest()
    {
        var tr = new Throttler();
        
        var popRes = tr.PopUpdates("au1");
        
        Assert.Null(popRes);
    }
    
    [Fact]
    public void NoMergedUpdates()
    {
        var tr = new Throttler();
            
        var firstUpdates = new Updates();
        firstUpdates.Add(new LotUpdate(Action.Add, new Lot(2, 66, 1000)));
        firstUpdates.Add(new LotUpdate(Action.Add, new Lot(1, 65, 1000)));
        firstUpdates.Add(new LotUpdate(Action.Add, new Lot(3, 67, 1000)));
        tr.PutUpdates(AuctionName, firstUpdates);

        var secondUpdates = new Updates();
        secondUpdates.Add(new LotUpdate(Action.Del, new Lot(5, 0, 0)));
        secondUpdates.Add(new LotUpdate(Action.Add, new Lot(4, 69, 3000)));
        secondUpdates.Add(new LotUpdate(Action.Change, new Lot( 6, 65, 2000)));
        tr.PutUpdates(AuctionName, secondUpdates);

        var actualRes = new Updates();
        actualRes.Add(new LotUpdate(Action.Add, new Lot(1, 65, 1000)));
        actualRes.Add(new LotUpdate(Action.Add, new Lot(2, 66, 1000)));
        actualRes.Add(new LotUpdate(Action.Add, new Lot(3, 67, 1000)));
        actualRes.Add(new LotUpdate(Action.Add, new Lot(4, 69, 3000)));
        actualRes.Add(new LotUpdate(Action.Del, new Lot(5, 0, 0)));
        actualRes.Add(new LotUpdate(Action.Change, new Lot( 6, 65, 2000)));
            
        var popRes = tr.PopUpdates("au1");
            
        Assert.NotNull(popRes);
        Assert.True(popRes.SequenceEqual(actualRes));
    }
    
    [Fact]
    public void AddToAddTest()
    {
        var tr = new Throttler();
            
        var firstUpdates = new Updates();
        firstUpdates.Add(new LotUpdate(Action.Add, new Lot(2, 66, 1000)));
        tr.PutUpdates(AuctionName, firstUpdates);

        var secondUpdates = new Updates();
        secondUpdates.Add(new LotUpdate(Action.Add, new Lot(2, 67, 3000)));
        tr.PutUpdates(AuctionName, secondUpdates);

        var actualRes = new Updates();
        actualRes.Add(new LotUpdate(Action.Add, new Lot(2, 67, 3000)));

        var popRes = tr.PopUpdates("au1");
            
        Assert.NotNull(popRes);
        Assert.True(popRes.SequenceEqual(actualRes));
    }
    
    [Fact]
    public void ChangeToAddTest()
    {
        var tr = new Throttler();
            
        var firstUpdates = new Updates();
        firstUpdates.Add(new LotUpdate(Action.Change, new Lot(3, 40, 1500)));
        tr.PutUpdates(AuctionName, firstUpdates);

        var secondUpdates = new Updates();
        secondUpdates.Add(new LotUpdate(Action.Add, new Lot(3, 42, 2000)));
        tr.PutUpdates(AuctionName, secondUpdates);

        var actualRes = new Updates();
        actualRes.Add(new LotUpdate(Action.Change, new Lot(3, 42, 2000)));

        var popRes = tr.PopUpdates("au1");
            
        Assert.NotNull(popRes);
        Assert.True(popRes.SequenceEqual(actualRes));
    }
    
    [Fact]
    public void ChangeToChangeTest()
    {
        var tr = new Throttler();
            
        var firstUpdates = new Updates();
        firstUpdates.Add(new LotUpdate(Action.Change, new Lot(3, 40, 1500)));
        tr.PutUpdates(AuctionName, firstUpdates);

        var secondUpdates = new Updates();
        secondUpdates.Add(new LotUpdate(Action.Change, new Lot(3, 42, 2000)));
        tr.PutUpdates(AuctionName, secondUpdates);

        var actualRes = new Updates();
        actualRes.Add(new LotUpdate(Action.Change, new Lot(3, 42, 2000)));

        var popRes = tr.PopUpdates("au1");
            
        Assert.NotNull(popRes);
        Assert.True(popRes.SequenceEqual(actualRes));
    }
    
    [Fact]
    public void ChangeToDelTest()
    {
        var tr = new Throttler();
            
        var firstUpdates = new Updates();
        firstUpdates.Add(new LotUpdate(Action.Change, new Lot(3, 40, 1500)));
        tr.PutUpdates(AuctionName, firstUpdates);

        var secondUpdates = new Updates();
        secondUpdates.Add(new LotUpdate(Action.Del, new Lot(3, 0, 0)));
        tr.PutUpdates(AuctionName, secondUpdates);

        var actualRes = new Updates();
        actualRes.Add(new LotUpdate(Action.Del, new Lot(3, 0, 0)));

        var popRes = tr.PopUpdates("au1");
            
        Assert.NotNull(popRes);
        Assert.True(popRes.SequenceEqual(actualRes));
    }
    
    [Fact]
    public void DelToAddTest()
    {
        var tr = new Throttler();
            
        var firstUpdates = new Updates();
        firstUpdates.Add(new LotUpdate(Action.Del, new Lot(3, 0, 0)));
        tr.PutUpdates(AuctionName, firstUpdates);

        var secondUpdates = new Updates();
        secondUpdates.Add(new LotUpdate(Action.Add, new Lot(3, 40, 1500)));
        tr.PutUpdates(AuctionName, secondUpdates);

        var actualRes = new Updates();
        actualRes.Add(new LotUpdate(Action.Change, new Lot(3, 40, 1500)));

        var popRes = tr.PopUpdates("au1");
            
        Assert.NotNull(popRes);
        Assert.True(popRes.SequenceEqual(actualRes));
    }
    
    [Fact]
    public void DelToChangeTest()
    {
        var tr = new Throttler();
            
        var firstUpdates = new Updates();
        firstUpdates.Add(new LotUpdate(Action.Del, new Lot(3, 0, 0)));
        tr.PutUpdates(AuctionName, firstUpdates);

        var secondUpdates = new Updates();
        secondUpdates.Add(new LotUpdate(Action.Change, new Lot(3, 40, 1500)));
        tr.PutUpdates(AuctionName, secondUpdates);

        var actualRes = new Updates();
        actualRes.Add(new LotUpdate(Action.Change, new Lot(3, 40, 1500)));

        var popRes = tr.PopUpdates("au1");
            
        Assert.NotNull(popRes);
        Assert.True(popRes.SequenceEqual(actualRes));
    }
    
    [Fact]
    public void DelToDelTest()
    {
        var tr = new Throttler();
            
        var firstUpdates = new Updates();
        firstUpdates.Add(new LotUpdate(Action.Del, new Lot(3, 0, 0)));
        tr.PutUpdates(AuctionName, firstUpdates);

        var secondUpdates = new Updates();
        secondUpdates.Add(new LotUpdate(Action.Del, new Lot(3, 0, 0)));
        tr.PutUpdates(AuctionName, secondUpdates);

        var actualRes = new Updates();
        actualRes.Add(new LotUpdate(Action.Del, new Lot(3, 0, 0)));

        var popRes = tr.PopUpdates("au1");
            
        Assert.NotNull(popRes);
        Assert.True(popRes.SequenceEqual(actualRes));
    }
}