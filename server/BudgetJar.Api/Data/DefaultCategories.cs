using System.Collections.Generic;
using BudgetJar.Api.Models;

namespace BudgetJar.Api.Data
{
    public static class DefaultCategories
    {
        public static IEnumerable<Category> For(User user)
        {
            yield return Create(user, "Salary", TransactionType.Income, "#59a14f");
            yield return Create(user, "Other income", TransactionType.Income, "#8cd17d");
            yield return Create(user, "Rent", TransactionType.Expense, "#4e79a7");
            yield return Create(user, "Groceries", TransactionType.Expense, "#f28e2b");
            yield return Create(user, "Transport", TransactionType.Expense, "#76b7b2");
            yield return Create(user, "Utilities", TransactionType.Expense, "#edc948");
            yield return Create(user, "Eating out", TransactionType.Expense, "#e15759");
            yield return Create(user, "Fun", TransactionType.Expense, "#b07aa1");
            yield return Create(user, "Other", TransactionType.Expense, "#bab0ac");
        }

        private static Category Create(User user, string name, TransactionType type, string color)
        {
            return new Category { User = user, Name = name, Type = type, Color = color };
        }
    }
}
