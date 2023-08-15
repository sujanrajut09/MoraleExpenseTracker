using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace MoraleExpenseTracker
{
    public class ExpenseRecord
    {
        public int Id { get; set; }
        public string ManagerName { get; set; }
        public string Quarter { get; set; }
        public int HeadCount { get; set; }
        public decimal Budget { get; set; }
        public decimal Expenses { get; set; }
        public decimal Balance { get; set; }
        public DateTime BudgetAllocatedDate { get; set; }
        public DateTime ExpenseDate { get; set; }
    }

    public class ExpenseTrackerDataAccess
    {
        private readonly string _connectionString;

        public ExpenseTrackerDataAccess(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<ExpenseRecord> GetAllExpenseRecords()
        {
            List<ExpenseRecord> expenseRecords = new List<ExpenseRecord>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_GetAllExpenseRecords", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ExpenseRecord expenseRecord = new ExpenseRecord
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                ManagerName = reader["ManagerName"].ToString(),
                                Quarter = reader["Quarter"].ToString(),
                                Budget = Convert.ToDecimal(reader["Budget"]),
                                Balance = Convert.ToDecimal(reader["Balance"]),
                                HeadCount = Convert.ToInt32(reader["HeadCount"]),
                                BudgetAllocatedDate = Convert.ToDateTime(reader["BudgetAllocatedDate"])
                            };

                            if (reader["Expenses"] != DBNull.Value)
                            {
                                expenseRecord.Expenses = Convert.ToDecimal(reader["Expenses"]);
                            }
                            else
                            {
                                expenseRecord.Expenses = 0; // Set a default value if needed
                            }

                            if (reader["ExpenseDate"] != DBNull.Value)
                            {
                                expenseRecord.ExpenseDate = Convert.ToDateTime(reader["ExpenseDate"]);
                            }
                            else
                            {
                                expenseRecord.ExpenseDate = DateTime.MinValue; // Set a default value if needed
                            }

                            expenseRecords.Add(expenseRecord);
                        }
                    }

                }
            }
            return expenseRecords;
        }

        public void InsertManager(string managerName)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("InsertManager", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ManagerName", managerName);
                    command.ExecuteNonQuery();
                }
            }

        }

    }
}