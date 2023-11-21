using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace MoraleExpenseTracker
{
    public class ExpenseTrackerDataAccess
    {
        private readonly string _connectionString;

        public ExpenseTrackerDataAccess()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ExpenseTrackerConStr"].ConnectionString;

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
        public void SaveExpenseByManager(int managerId, int year, string quarter, decimal budget, decimal expense, int headCount, string description, DateTime expenseDate)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_SaveManagerData", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ManagerId", managerId);
                    command.Parameters.AddWithValue("@Year", year);
                    command.Parameters.AddWithValue("@Quarter", quarter);
                    command.Parameters.AddWithValue("@Budget", budget);
                    command.Parameters.AddWithValue("@HeadCount", headCount);
                    command.Parameters.AddWithValue("@Expenses", expense);
                    command.Parameters.AddWithValue("@Description", description);
                    command.Parameters.AddWithValue("@ExpenseDate", expenseDate);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
        public void AllocatedBudgetByAdmin(int managerId, int year, string quarter, decimal budget, int headCount, string description, DateTime budgetAllocatedDate)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_SaveAdminData", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ManagerId", managerId);
                    command.Parameters.AddWithValue("@Year", year);
                    command.Parameters.AddWithValue("@Quarter", quarter);
                    command.Parameters.AddWithValue("@Budget", budget);
                    command.Parameters.AddWithValue("@HeadCount", headCount);
                    command.Parameters.AddWithValue("@Description", description);
                    command.Parameters.AddWithValue("@BudgetAllocatedDate", budgetAllocatedDate);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}