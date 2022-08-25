namespace Service.Outputs
{
    public class StatisticsOutput
    {
        public UserStatistics MostActiveSeller { get; set; }
        public UserStatistics MostActiveBuyer { get; set; }
        public AuctionDetails MostExpensiveItem { get; set; }
        public AuctionDetails MostExpensiveItemSoldByCurrentUser { get; set; }
        public AuctionDetails MostExpensiveItemBoughtByCurrentUser { get; set; }
    }
}