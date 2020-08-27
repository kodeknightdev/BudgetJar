namespace BudgetJar.Api.Dtos
{
    public class CategoryTotalDto
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public decimal Total { get; set; }
    }
}
