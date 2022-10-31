[System.Serializable]
public partial class ClaimableDailyReward : IClaimableDailyReward
{
    public bool isClaimed;
    public bool IsClaimed { get { return isClaimed; } set { isClaimed = value; } }
    public bool canClaim;
    public bool CanClaim { get { return canClaim; } set { canClaim = value; } }
    public RewardData reward;
    public RewardData Reward { get { return reward; } set { reward = value; } }
}
