global using Updates = Auction.Infrastructure.ListPool<Auction.Models.LotUpdate>;
using System.Linq;
using Auction.Models;
using Xunit;
using Action = Auction.Models.Action;

namespace Auction.Tests
{
    public class TestsFromDescription
    {
        private const string AuctionName = "au1";
        
        [Fact]
        // ADD -> Change; ADD -> Del
        public void TestFromDescription1()
        {
            var tr = new Throttler();
            
            var firstUpdates = new Updates();
            firstUpdates.Add(new LotUpdate(Action.Add, new Lot(2, 66, 1000)));
            firstUpdates.Add(new LotUpdate(Action.Add, new Lot(1, 65, 1000)));
            firstUpdates.Add(new LotUpdate(Action.Add, new Lot(3, 67, 1000)));
            tr.PutUpdates(AuctionName, firstUpdates);

            var secondUpdates = new Updates();
            secondUpdates.Add(new LotUpdate(Action.Del, new Lot(2, 0, 0)));
            secondUpdates.Add(new LotUpdate(Action.Add, new Lot(4, 69, 3000)));
            secondUpdates.Add(new LotUpdate(Action.Change, new Lot( 1, 65, 2000)));
            tr.PutUpdates(AuctionName, secondUpdates);

            var actualRes = new Updates();
            actualRes.Add(new LotUpdate(Action.Add, new Lot(1, 65, 2000)));
            actualRes.Add(new LotUpdate(Action.Add, new Lot(3, 67, 1000)));
            actualRes.Add(new LotUpdate(Action.Add, new Lot(4, 69, 3000)));
            
            var popRes = tr.PopUpdates("au1");
            
            Assert.NotNull(popRes);
            Assert.True(popRes.SequenceEqual(actualRes));
        }
        
        [Fact]
        // Del -> ADD
        public void TestFromDescription2()
        {
            var tr = new Throttler();
            
            var firstUpdates = new Updates();
            firstUpdates.Add(new LotUpdate(Action.Del, new Lot(1, 0, 0 )));
            tr.PutUpdates(AuctionName, firstUpdates);

            var secondUpdates = new Updates();
            secondUpdates.Add(new LotUpdate(Action.Add, new Lot(1, 70, 5000)));
            secondUpdates.Add(new LotUpdate(Action.Del, new Lot(4, 0, 0)));
            tr.PutUpdates(AuctionName, secondUpdates);

            var actualRes = new Updates();
            actualRes.Add(new LotUpdate(Action.Change, new Lot(1, 70, 5000 )));
            actualRes.Add(new LotUpdate(Action.Del, new Lot(4, 0, 0)));

            var popRes = tr.PopUpdates("au1");
            
            Assert.NotNull(popRes);
            Assert.True(popRes.SequenceEqual(actualRes));
        }
        
        [Fact]
        public void TestFromDescription3()
        {
            var tr = new Throttler();
            
            var firstUpdates = new Updates();
            firstUpdates.Add(new LotUpdate(Action.Add, new Lot(5, 55, 5500 )));
            tr.PutUpdates(AuctionName, firstUpdates);

            var secondUpdates = new Updates();
            secondUpdates.Add(new LotUpdate(Action.Del, new Lot(5, 0, 0 )));
            tr.PutUpdates(AuctionName, secondUpdates);
            
            var thirdUpdates = new Updates();
            thirdUpdates.Add(new LotUpdate(Action.Add, new Lot(5, 56, 5600 )));
            tr.PutUpdates(AuctionName, thirdUpdates);

            var actualRes = new Updates();
            actualRes.Add(new LotUpdate(Action.Add, new Lot(5, 56, 5600 )));

            var popRes = tr.PopUpdates("au1");
            
            Assert.NotNull(popRes);
            Assert.True(popRes.SequenceEqual(actualRes));
        }
    }
}