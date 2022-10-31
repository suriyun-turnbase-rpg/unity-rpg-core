public partial interface IClaimableDailyReward
{
    bool IsClaimed { get; set; }
    bool CanClaim { get; set; }
    RewardData Reward { get; set; }
}
