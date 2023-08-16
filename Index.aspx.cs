using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using OfficeOpenXml;

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

            bool isAdminLoggedIn = Session["AdminLoggedIn"] != null && (bool)Session["AdminLoggedIn"];
            if (isAdminLoggedIn)
                multiViewTabs.ActiveViewIndex = 1;
            else
                multiViewTabs.ActiveViewIndex = 0;

            ClearManagerView();
        }        
        protected void btnManagerTab_Click(object sender, EventArgs e)
        {
            multiViewTabs.ActiveViewIndex = 2;
            btnAdminTab.CssClass = "nav-link bg-white";
            btnManagerTab.CssClass = "nav-link bg-grey";
            btnReportsTab.CssClass = "nav-link bg-white";
            BindManagerDropdownsInAllTabs();

            ClearAdminLoginForm();
            ClearAdminView();

        }
        protected void btnReportsTab_Click(object sender, EventArgs e)
        {
            multiViewTabs.ActiveViewIndex = 3;
            BindReportsGridView();
            btnAdminTab.CssClass = "nav-link bg-white";
            btnManagerTab.CssClass = "nav-link bg-white";
            btnReportsTab.CssClass = "nav-link bg-grey";

            ClearAdminLoginForm();
            ClearAdminView();
            ClearManagerView();

        }
        #endregion

        #region Login
        protected void btnAdminLogin_Click(object sender, EventArgs e)
        {
            string adminUsername = "admin";
            string adminPassword = "password";

            string enteredUsername = txtAdminUsername.Text.Trim();
            string enteredPassword = txtAdminPassword.Text.Trim();

            if (enteredUsername == adminUsername && enteredPassword == adminPassword)
            {
                Session["AdminLoggedIn"] = true;
                multiViewTabs.ActiveViewIndex = 1;              
            }
            else
            {
                multiViewTabs.ActiveViewIndex = 0;
                lblAdminLoginError.Text = "Invalid username or password.";
            }
        }
        private bool IsValidAdminLogin(string username, string password)
        {
            // Implement your admin login validation logic here
            // Return true if the login is valid, otherwise return false
            return true;
        }
        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            multiViewTabs.ActiveViewIndex = 0;
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

        protected void btnDelManager_Click(object sender, EventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ExpenseTrackerConStr"].ConnectionString;
            ExpenseTrackerDataAccess dataAccess = new ExpenseTrackerDataAccess(connectionString);

            int managerId = Convert.ToInt32(ddlDelMgrName.SelectedValue);
            dataAccess.DeactivateManager(managerId);
            lblMsgA.Text = "Manger deleted successfully!";
            BindManagerDropdownsInAllTabs();
        }

        private void BindManagerDropdownsInAllTabs()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ExpenseTrackerConStr"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("sp_GetManagersForDropdown", connection))
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

                        ddlDelMgrName.DataSource = dt;
                        ddlDelMgrName.DataTextField = "ManagerName";
                        ddlDelMgrName.DataValueField = "ManagerId";
                        ddlDelMgrName.DataBind();
                        ddlDelMgrName.Items.Insert(0, new ListItem("Please select", ""));

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
                    //command.Parameters.AddWithValue("@Balance", balance);
                    //command.Parameters.AddWithValue("@Expenses", 0);
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
            gvReports.PageIndex = e.NewPageIndex;
            BindManagerGrid();
            multiViewTabs.ActiveViewIndex = 2;
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
        protected void btnExcelExport_Click(object sender, EventArgs e)
        {
            ExportToExcel(ReportsResultSet());
        }
        protected void gvReports_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvReports.PageIndex = e.NewPageIndex;
            BindReportsGridView();
            multiViewTabs.ActiveViewIndex = 3;
        }

        private void ExportToExcel(List<ExpenseRecord> expenseRecords)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                var worksheet = excelPackage.Workbook.Worksheets.Add("Sheet1");

                // Write header row
                int colCount = 1;
                foreach (var property in typeof(ExpenseRecord).GetProperties())
                {
                    worksheet.Cells[1, colCount].Value = property.Name;
                    colCount++;
                }

                // Write data rows
                int rowCount = 2;
                foreach (var record in expenseRecords)
                {
                    colCount = 1;
                    foreach (var property in typeof(ExpenseRecord).GetProperties())
                    {
                        worksheet.Cells[rowCount, colCount].Value = property.GetValue(record);
                        colCount++;
                    }
                    rowCount++;
                }

                byte[] excelBytes = excelPackage.GetAsByteArray();

                Response.Clear();
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("Content-Disposition", "attachment; filename=ExpenseTracker.xlsx");
                Response.BinaryWrite(excelBytes);
                Response.End();
            }
        }

        private List<ExpenseRecord> ReportsResultSet()
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

           return filteredExpenseRecords;
            
        }

        private void BindReportsGridView()
        {
            gvReports.DataSource = ReportsResultSet();
            gvReports.DataBind();
        }
        #endregion

        #region ClearOff
        private void ClearAdminLoginForm()
        {
            txtAdminUsername.Text = string.Empty;
            txtAdminPassword.Text = string.Empty;

            rfvAdminUsername.ErrorMessage = string.Empty;
            rfvAdminPassword.ErrorMessage = string.Empty;
            rfvAdminUsername.IsValid = true;
            rfvAdminPassword.IsValid = true;

            lblAdminLoginError.Text = string.Empty;
        }
        private void ClearAdminView()
        {
            // Clear DropDownList selections
            ddlManagerA.SelectedIndex = -1;
            ddlQuarter.SelectedIndex = 0; // Select the "Select" item

            // Clear TextBox values
            txtHc.Text = string.Empty;
            txtBudget.Text = string.Empty;
            txtTotalBudget.Text = string.Empty;
            txtNewManagerName.Text = string.Empty;

            // Clear RequiredFieldValidator errors
            //rfvManagerA.ErrorMessage = string.Empty;
            //rfvQuarter.ErrorMessage = string.Empty;
            //rfvHc.ErrorMessage = string.Empty;
            //rfvBudget.ErrorMessage = string.Empty;
            //rfvNewManagerName.ErrorMessage = string.Empty;

            //rfvManagerA.IsValid = true;
            //rfvQuarter.IsValid = true;
            //rfvHc.IsValid = true;
            //rfvBudget.IsValid = true;
            //rfvNewManagerName.IsValid = true;

            //// Clear RegularExpressionValidator errors
            //revHc.ErrorMessage = string.Empty;
            //revBudget.ErrorMessage = string.Empty;

            //revHc.IsValid = true;
            //revBudget.IsValid = true;

            // Clear Label messages
            lblMsgASave.Text = string.Empty;
            lblMsgA.Text = string.Empty;
        }
        private void ClearManagerView()
        {
            // Clear values and reset DropDownList selections
            ddlManagerM.SelectedIndex = -1;
            ddlQuarterM.SelectedIndex = 0;
            txtBudgetM.Text = "";
            txtHcM.Text = "";
            txtExpense.Text = "";
            gvManager.DataSource = null;
            gvManager.DataBind();

            // Clear validation errors
            //rfvManagerM.IsValid = true;
            //rfvQuarterM.IsValid = true;
            //rfvBudgetM.IsValid = true;
            //rfvHcM.IsValid = true;
            //rfvExpense.IsValid = true;

            // Clear error messages
            lblMsgM.Text = "";
        }

        #endregion
    }
}