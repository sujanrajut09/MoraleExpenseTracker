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
                                ExpenseId = Convert.ToInt32(reader["Id"]),
                                Manager = reader["ManagerName"].ToString(),
                                FY = reader["Year"].ToString(),
                                Quarter = reader["Quarter"].ToString(),
                                Budget = Convert.ToDecimal(reader["Budget"]),
                                Balance = Convert.ToDecimal(reader["Balance"]),
                                Reportees = Convert.ToInt32(reader["HeadCount"]),
                                Description = reader["Description"].ToString(),
                                BudgetDate = Convert.ToDateTime(reader["BudgetAllocatedDate"])
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

                using (SqlCommand command = new SqlCommand("sp_InsertManager", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ManagerName", managerName);
                    command.ExecuteNonQuery();
                }
            }

        }
        public void UpdateExpense(int expenseId, decimal expenses, string description, DateTime expenseDate)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_UpdateExpense", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add("@ExpenseId", SqlDbType.Int).Value = expenseId;
                    command.Parameters.Add("@Expenses", SqlDbType.Decimal).Value = expenses;
                    command.Parameters.Add("@Description", SqlDbType.VarChar, -1).Value = description;
                    command.Parameters.AddWithValue("@ExpenseDate", expenseDate);

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("An error occurred while updating expense: " + ex.Message);
                    }
                }
            }
        }

        public void DeleteExpense(int expenseId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("sp_DeleteExpense", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Add parameters to the stored procedure
                    command.Parameters.Add(new SqlParameter("@ExpenseId", SqlDbType.Int));
                    command.Parameters["@ExpenseId"].Value = expenseId;

                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error deleting expense: " + ex.Message);
                        // Handle the exception as needed
                    }
                }
            }
        }

        public void DeactivateManager(int managerId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("sp_DeactivateManager", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ManagerId", managerId);
                    command.ExecuteNonQuery();
                }
            }

        }

    }
}