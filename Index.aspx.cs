using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MoraleExpenseTracker
{
    public partial class Index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindManagerDropdownsInAllTabs();
            }

        }

        #region Tabs
        protected void btnAdminTab_Click(object sender, EventArgs e)
        {
            multiViewTabs.ActiveViewIndex = 0;
            btnAdminTab.CssClass = "nav-link bg-grey";
            btnManagerTab.CssClass = "nav-link bg-white";
            btnReportsTab.CssClass = "nav-link bg-white";
        }
        protected void btnManagerTab_Click(object sender, EventArgs e)
        {
            multiViewTabs.ActiveViewIndex = 1;
            btnAdminTab.CssClass = "nav-link bg-white";
            btnManagerTab.CssClass = "nav-link bg-grey";
            btnReportsTab.CssClass = "nav-link bg-white";
            BindManagerDropdownsInAllTabs();

        }
        protected void btnReportsTab_Click(object sender, EventArgs e)
        {
            multiViewTabs.ActiveViewIndex = 2;
            BindReportsGridView();
            btnAdminTab.CssClass = "nav-link bg-white";
            btnManagerTab.CssClass = "nav-link bg-white";
            btnReportsTab.CssClass = "nav-link bg-grey";
        }
        #endregion

        #region AdminTab
        protected void btnSaveA_Click(object sender, EventArgs e)
        {
            SaveAdminData();
        }
        protected void btnSaveNewManager_Click(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ExpenseTrackerConStr"].ConnectionString;
            ExpenseTrackerDataAccess dataAccess = new ExpenseTrackerDataAccess(connectionString);

            string newManagerName = txtNewManagerName.Text.Trim();
            dataAccess.InsertManager(newManagerName);
            lblMsgA.Text = "Manger added successfully!";
            BindManagerDropdownsInAllTabs();

        }       
        private void BindManagerDropdownsInAllTabs()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ExpenseTrackerConStr"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("GetManagersForDropdown", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        ddlManagerA.DataSource = dt;
                        ddlManagerA.DataTextField = "ManagerName";
                        ddlManagerA.DataValueField = "ManagerId";
                        ddlManagerA.DataBind();
                        ddlManagerA.Items.Insert(0, new ListItem("Please select", ""));

                        ddlManagerM.DataSource = dt;
                        ddlManagerM.DataTextField = "ManagerName";
                        ddlManagerM.DataValueField = "ManagerId";
                        ddlManagerM.DataBind();
                        ddlManagerM.Items.Insert(0, new ListItem("Please select", ""));

                        ddlManagerR.DataSource = dt;
                        ddlManagerR.DataTextField = "ManagerName";
                        ddlManagerR.DataValueField = "ManagerId";
                        ddlManagerR.DataBind();
                        ddlManagerR.Items.Insert(0, new ListItem("All Managers", ""));

                        if (ddlQuarterR.Items.Count > 0)
                        {
                            if (ddlQuarterR.Items[0].Text == "All Quarters")
                            {
                                ddlQuarterR.Items.RemoveAt(0);
                            }
                        }
                        ddlQuarterR.Items.Insert(0, new ListItem("All Quarters", ""));
                    }
                }
            }
        }
        private void SaveAdminData()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ExpenseTrackerConStr"].ConnectionString;

            int managerId = Convert.ToInt32(ddlManagerA.SelectedValue);
            string quarter = ddlQuarter.SelectedValue;
            decimal budget = Convert.ToInt32(txtHc.Text) * Convert.ToInt32(txtBudget.Text);
            decimal balance = budget;
            int headCount = Convert.ToInt32(txtHc.Text);
            DateTime budgetAllocatedDate = DateTime.Now;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_SaveAdminData", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ManagerId", managerId);
                    command.Parameters.AddWithValue("@Quarter", quarter);
                    command.Parameters.AddWithValue("@Budget", budget);
                    command.Parameters.AddWithValue("@HeadCount", headCount);
                    command.Parameters.AddWithValue("@Balance", balance);
                    command.Parameters.AddWithValue("@Expenses", 0);
                    command.Parameters.AddWithValue("@BudgetAllocatedDate", budgetAllocatedDate);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
                lblMsgASave.Text = "Data saved successfully!";
            }

        }
        #endregion

        #region ManagerTab
        protected void btnSaveM_Click(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ExpenseTrackerConStr"].ConnectionString;

            int managerId = Convert.ToInt32(ddlManagerM.SelectedValue);
            string quarter = ddlQuarterM.SelectedValue;
            decimal budget = Convert.ToDecimal(txtBudgetM.Text);
            decimal expense = Convert.ToDecimal(txtExpense.Text);
            int headCount = Convert.ToInt32(txtHcM.Text);
            DateTime expenseDate = DateTime.Now;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_SaveManagerData", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ManagerId", managerId);
                    command.Parameters.AddWithValue("@Quarter", quarter);
                    command.Parameters.AddWithValue("@Budget", budget);
                    command.Parameters.AddWithValue("@HeadCount", headCount);
                    command.Parameters.AddWithValue("@Expenses", expense);
                    command.Parameters.AddWithValue("@ExpenseDate", expenseDate);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
                lblMsgM.Text = "Data saved successfully!";
                BindManagerGrid();
            }
        }
        protected void ddlManagerM_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindManagerGrid();
            lblMsgM.Text = string.Empty;
        }
        protected void ddlQuarterM_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindManagerGrid();
            lblMsgM.Text = string.Empty;
        }
        protected void gvManager_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //gvReports.PageIndex = e.NewPageIndex;
            //BindGridView();
            //multiViewTabs.ActiveViewIndex = 2;
        }
        private void BindManagerGrid()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ExpenseTrackerConStr"].ConnectionString;
            ExpenseTrackerDataAccess dataAccess = new ExpenseTrackerDataAccess(connectionString);

            string selectedManager = ddlManagerM.SelectedItem.ToString();
            string selectedQuarter = ddlQuarterM.SelectedValue;

            List<ExpenseRecord> expenseRecords = dataAccess.GetAllExpenseRecords();

            List<ExpenseRecord> filteredRecords = expenseRecords
                .Where(r => r.ManagerName == selectedManager && r.Quarter == selectedQuarter)
                .ToList();

            gvManager.DataSource = filteredRecords;
            gvManager.DataBind();

            decimal sumOfExpenses = filteredRecords.Sum(r => r.Expenses);

            string selectedMgr = ddlManagerM.SelectedItem.ToString();
            ExpenseRecord managerRecord = expenseRecords.FirstOrDefault(r => r.ManagerName == selectedMgr && r.Quarter == ddlQuarterM.SelectedValue);

            if (managerRecord != null)
            {
                txtBudgetM.Text = managerRecord.Budget.ToString();
                txtHcM.Text = managerRecord.HeadCount.ToString();
            }
            else
            {
                txtBudgetM.Text = string.Empty;
                txtHcM.Text = string.Empty;
                txtExpense.Text = string.Empty;
            }
        }

        #endregion

        #region ReportsTab
        protected void ddlManagerR_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindReportsGridView();
        }
        protected void ddlQuarterR_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindReportsGridView();
        }
        protected void btnGetExpenseReports_Click(object sender, EventArgs e)
        {
            BindReportsGridView();
        }
        protected void gvReports_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvReports.PageIndex = e.NewPageIndex;
            BindReportsGridView();
            multiViewTabs.ActiveViewIndex = 2;
        }

        private void BindReportsGridView()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ExpenseTrackerConStr"].ConnectionString;
            ExpenseTrackerDataAccess dataAccess = new ExpenseTrackerDataAccess(connectionString);

            int selectedManagerIndex = ddlManagerR.SelectedIndex;
            int selectedQuarterIndex = ddlQuarterR.SelectedIndex;

            List<ExpenseRecord> expenseRecords = dataAccess.GetAllExpenseRecords();
            List<ExpenseRecord> filteredExpenseRecords = expenseRecords; // Initialize with all records

            if (selectedManagerIndex != 0)
            {
                string selectedManager = ddlManagerR.SelectedItem.ToString();

                if (selectedQuarterIndex != 0)
                {
                    string selectedQuarter = ddlQuarterR.SelectedItem.ToString();
                    filteredExpenseRecords = expenseRecords
                        .Where(r => r.ManagerName == selectedManager && r.Quarter == selectedQuarter)
                        .ToList();

                    decimal selectedQuarterBudget = filteredExpenseRecords
                        .Select(r => r.Budget)
                        .FirstOrDefault();

                    decimal sumOfFilteredExpenses = filteredExpenseRecords
                        .Select(r => r.Expenses)
                        .Sum();

                    txtTotalBudgetR.Text = selectedQuarterBudget.ToString();
                    txtTotalExpensesR.Text = sumOfFilteredExpenses.ToString();
                }
                else
                {
                    filteredExpenseRecords = expenseRecords
                        .Where(r => r.ManagerName == selectedManager)
                        .ToList();

                    decimal sumOfFilteredBudget = filteredExpenseRecords
                        .Select(r => r.Budget)
                        .Distinct()
                        .Sum();

                    decimal sumOfFilteredExpenses = filteredExpenseRecords
                        .Select(r => r.Expenses)
                        .Sum();

                    txtTotalBudgetR.Text = sumOfFilteredBudget.ToString();
                    txtTotalExpensesR.Text = sumOfFilteredExpenses.ToString();
                }
            }
            else if (selectedQuarterIndex != 0)
            {
                string selectedQuarter = ddlQuarterR.SelectedItem.ToString();
                filteredExpenseRecords = expenseRecords
                    .Where(r => r.Quarter == selectedQuarter)
                    .ToList();

                decimal sumOfFilteredBudget = filteredExpenseRecords
                    .Select(r => r.Budget)
                    .Distinct()
                    .Sum();

                decimal sumOfFilteredExpenses = filteredExpenseRecords
                    .Select(r => r.Expenses)
                    .Sum();

                txtTotalBudgetR.Text = sumOfFilteredBudget.ToString();
                txtTotalExpensesR.Text = sumOfFilteredExpenses.ToString();
            }
            else
            {
                decimal sumOfFilteredBudget = expenseRecords
                    .Select(r => r.Budget)
                    .Distinct()
                    .Sum();

                decimal sumOfFilteredExpenses = expenseRecords
                    .Select(r => r.Expenses)
                    .Sum();

                txtTotalBudgetR.Text = sumOfFilteredBudget.ToString();
                txtTotalExpensesR.Text = sumOfFilteredExpenses.ToString();
            }

            gvReports.DataSource = filteredExpenseRecords;
            gvReports.DataBind();
        }


        #endregion
    }
}