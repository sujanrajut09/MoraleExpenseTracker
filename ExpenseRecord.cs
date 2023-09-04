using System;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace MoraleExpenseTracker
{
    public class ExpenseRecord
    {
        public int ExpenseId { get; set; }
        public string Manager { get; set; }
        public string FY { get; set; }
        public string Quarter { get; set; }
        public int Reportees { get; set; }
        public decimal Budget { get; set; }
        public decimal Expenses { get; set; }
        public decimal Balance { get; set; }
        public string Description { get; set; }
        public DateTime BudgetDate { get; set; }
        public DateTime ExpenseDate { get; set; }
    }
}