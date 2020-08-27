namespace BudgetJar.Api.Dtos
{
    public class MonthlyTotalDto
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal Income { get; set; }
        public decimal Expense { get; set; }
        public decimal Net => Income - Expense;
    }
}
