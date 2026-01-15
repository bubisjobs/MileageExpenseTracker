namespace MileageExpenseTracker.Models
{
    public enum ClaimStatus
    {
        Draft = 1,

        // Submitted for team lead approval
        SubmittedToTeamLead = 2,

        // Team lead decision
        TeamLeadApproved = 3,
        TeamLeadRejected = 4,

        // Finance stage (after team lead approval)
        SubmittedToFinance = 5,

        // Finance decision
        FinanceApproved = 6,
        FinanceRejected = 7
    }
}

