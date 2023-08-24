<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="MoraleExpenseTracker.Index" %>

<!DOCTYPE html>
<html>
<head>
    <title>Morale Expense Tracker</title>
    <!-- Add Bootstrap CSS -->
    <link href="https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/css/bootstrap.min.css" rel="stylesheet" />
    <link href="CSS/IndexPageStyleSheet.css" rel="stylesheet" />
    <script src="https://code.jquery.com/jquery-3.5.1.min.js"> </script>
</head>
<body>
    <form id="form2" runat="server">
        <header>
            <nav>
                <div class="logo">
                    <img style="width: 80px; height: 80px; border-radius: 41px; outline: 4px solid red;" src="CSS/Images/Financial-Management.png" />
                </div>
                <div>
                    <h1>DAS - MORALE EXPENSE TARCKER </h1>
                </div>
                <!-- Tab buttons -->
                <div class="nav nav-tabs" id="myTab" role="tablist">
                    <asp:Button ID="btnAdminTab" runat="server" Text="Admin" OnClick="btnAdminTab_Click" CssClass="nav-link" class="btn1" />
                    <asp:Button ID="btnManagerTab" runat="server" Text="Manager" OnClick="btnManagerTab_Click" CssClass="nav-link" class="btn2"/>
                    <asp:Button ID="btnReportsTab" runat="server" Text="Reports" OnClick="btnReportsTab_Click" CssClass="nav-link" class="btn3" />
                </div>
            </nav>  
        </header>

        <!-- Tab navigation using ASP.NET MultiView -->
        <asp:MultiView ID="multiViewTabs" runat="server">
            <asp:View ID="viewAdminLogin" runat="server">
                <div class="backegroundimage">
                    <div class="form-row" style="align-items: center">
                        <div class="form-group col-md-3" style="align-items: center">
                            <asp:Label ID="lblAdminUsername" runat="server" AssociatedControlID="txtAdminUsername">Username:</asp:Label>
                            <asp:TextBox ID="txtAdminUsername" runat="server" CssClass="form-control"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvAdminUsername" runat="server" ControlToValidate="txtAdminUsername"
                                InitialValue="" ErrorMessage="Username is required" CssClass="text-danger" Display="Dynamic" ValidationGroup="AdminLoginValidation" />
                        </div>
                        <div class="form-group col-md-3">
                            <asp:Label ID="lblAdminPassword" runat="server" AssociatedControlID="txtAdminPassword">Password:</asp:Label>
                            <asp:TextBox ID="txtAdminPassword" runat="server" TextMode="Password" CssClass="form-control"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvAdminPassword" runat="server" ControlToValidate="txtAdminPassword"
                                InitialValue="" ErrorMessage="Password is required" CssClass="text-danger" Display="Dynamic" ValidationGroup="AdminLoginValidation" />
                        </div>
                    </div>
                    <div class="form-row">
                        <div class="col-md-3">
                            <asp:Button ID="btnAdminLogin" runat="server" Text="Login" OnClick="btnAdminLogin_Click" CssClass="btn btn-primary btn-same-size" ValidationGroup="AdminLoginValidation" />
                        </div>
                    </div>
                    <div class="form-row">
                        <div class="col-md-6">
                            <asp:Label ID="lblAdminLoginError" runat="server" CssClass="text-danger"></asp:Label>
                        </div>
                    </div>
                </div>
            </asp:View>

            <asp:View ID="viewAdmin" runat="server">
                <!-- Content for Admin tab goes here -->
                <div class="form-row">
                    <div class="form-group col-md-3">
                        <asp:Label ID="lblManagerA" runat="server" AssociatedControlID="ddlManagerA">Manager:</asp:Label>
                        <asp:DropDownList ID="ddlManagerA" runat="server" CssClass="form-control" AutoPostBack="true"></asp:DropDownList>
                    </div>
                    <div class="form-group col-md-3">
                        <asp:Label ID="lblYearA" runat="server" AssociatedControlID="ddlYearA">Financial Year:</asp:Label>
                        <asp:DropDownList ID="ddlYearA" runat="server" CssClass="form-control">
                            <asp:ListItem Value="All">Please select</asp:ListItem>
                            <asp:ListItem Value="2023">2023</asp:ListItem>
                            <asp:ListItem Value="2024">2024</asp:ListItem>
                            <asp:ListItem Value="2025">2025</asp:ListItem>
                            <asp:ListItem Value="2026">2026</asp:ListItem>
                            <asp:ListItem Value="2027">2027</asp:ListItem>
                            <asp:ListItem Value="2028">2028</asp:ListItem>
                            <asp:ListItem Value="2029">2029</asp:ListItem>
                            <asp:ListItem Value="2030">2030</asp:ListItem>
                            <asp:ListItem Value="2020">2031</asp:ListItem>
                            <asp:ListItem Value="2021">2032</asp:ListItem>
                            <asp:ListItem Value="2022">2033</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="form-group col-md-3">
                        <asp:Label ID="lblQuarter" runat="server" AssociatedControlID="ddlQuarter">Quarter:</asp:Label>
                        <asp:DropDownList ID="ddlQuarter" runat="server" CssClass="form-control">
                            <asp:ListItem Value="All">Please select</asp:ListItem>
                            <asp:ListItem Value="Q1">Q1</asp:ListItem>
                            <asp:ListItem Value="Q2">Q2</asp:ListItem>
                            <asp:ListItem Value="Q3">Q3</asp:ListItem>
                            <asp:ListItem Value="Q4">Q4</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="form-group col-md-3 text-right mt-2">
                        <asp:Button ID="btnLogout" runat="server" Text="Logout" OnClick="btnLogout_Click" CssClass="btn btn-danger" />
                    </div>
                </div>
                <asp:RequiredFieldValidator ID="rfvManagerA" runat="server" ControlToValidate="ddlManagerA"
                    InitialValue="" ErrorMessage="Please select a manager" CssClass="text-danger" ValidationGroup="AdminSaveGroup" Display="Dynamic" />
                <asp:RequiredFieldValidator ID="rfvYearA" runat="server" ControlToValidate="ddlYearA"
                    InitialValue="All" ErrorMessage="Please select a year" CssClass="text-danger" ValidationGroup="AdminSaveGroup" Display="Dynamic" />
                <asp:RequiredFieldValidator ID="rfvQuarter" runat="server" ControlToValidate="ddlQuarter"
                    InitialValue="All" ErrorMessage="Please select a quarter" CssClass="text-danger" ValidationGroup="AdminSaveGroup" Display="Dynamic" />
                <div class="form-row">
                    <div class="form-group col-md-3">
                        <asp:Label ID="lblHc" runat="server" AssociatedControlID="txtHcA">No. of Reportees:</asp:Label>
                        <asp:TextBox ID="txtHcA" runat="server" CssClass="form-control" TextMode="Number"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvHcA" runat="server" ControlToValidate="txtHcA"
                            ErrorMessage="Reportees count is required" CssClass="text-danger" ValidationGroup="AdminSaveGroup" Display="Dynamic" />
                        <asp:RegularExpressionValidator ID="revHcA" runat="server" ControlToValidate="txtHcA"
                            ValidationExpression="^\d+$" ErrorMessage="Invalid head count" CssClass="text-danger" ValidationGroup="AdminSaveGroup" Display="Dynamic" />
                    </div>
                    <div class="form-group col-md-3">
                        <asp:Label ID="lblBudget" runat="server" AssociatedControlID="txtBudget">Budget per Reportee:</asp:Label>
                        <asp:TextBox ID="txtBudget" runat="server" CssClass="form-control" TextMode="Number"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvBudget" runat="server" ControlToValidate="txtBudget"
                            ErrorMessage="Budget per Reporteee is required" CssClass="text-danger" ValidationGroup="AdminSaveGroup" Display="Dynamic" />
                        <asp:RegularExpressionValidator ID="revBudget" runat="server" ControlToValidate="txtBudget"
                            ValidationExpression="^\d+(\.\d{1,2})?$" ErrorMessage="Invalid budget format" CssClass="text-danger" ValidationGroup="AdminSaveGroup" Display="Dynamic" />
                    </div>
                    <div class="form-group col-md-3">
                        <asp:Label ID="lblTotalBudget" runat="server" AssociatedControlID="txtTotalBudget">Total Budget:</asp:Label>
                        <asp:TextBox ID="txtTotalBudget" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
                <div class="form-row">
                    <div class="form-group col-md-3">
                        <asp:Label ID="lblDescriptionA" runat="server" AssociatedControlID="txtDescriptionA">Description:</asp:Label>
                        <asp:TextBox ID="txtDescriptionA" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2" Placeholder="Enter Description"></asp:TextBox>
                    </div>
                    <div class="form-group col-md-3 align-self-end">
                        <asp:Button ID="btnSaveA" runat="server" Text="Save" OnClick="btnSaveA_Click" CssClass="btn btn-primary btn-same-size" ValidationGroup="AdminSaveGroup" />
                        <asp:Label ID="lblMsgASave" runat="server" CssClass="text-success"></asp:Label>
                    </div>
                </div>
                <div class="row">
                    <div class="form-group col-md-3">
                        <asp:Label ID="lblNewManagerName" runat="server" AssociatedControlID="txtNewManagerName">Add Manager:</asp:Label>
                    </div>
                    <div class="form-group col-md-3">
                        <asp:TextBox ID="txtNewManagerName" runat="server" CssClass="form-control" Placeholder="Enter Manager Name"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvNewManagerName" runat="server" ControlToValidate="txtNewManagerName"
                            ErrorMessage="Manager name is required" CssClass="text-danger" Display="Dynamic"
                            ValidationGroup="AddManagerGroup" />
                    </div>
                    <div class="form-group col-md-3">
                        <asp:Button ID="btnSaveNewManager" runat="server" Text="Add Manager" OnClick="btnSaveNewManager_Click"
                            CssClass="btn btn-primary btn-same-size" ValidationGroup="AddManagerGroup" />
                    </div>
                </div>
                <div class="row">
                    <div class="form-group col-md-3">
                        <asp:Label ID="lblDelMgrName" runat="server" AssociatedControlID="ddlDelMgrName">Delete Manager:</asp:Label>
                    </div>
                    <div class="form-group col-md-3">
                        <asp:DropDownList ID="ddlDelMgrName" runat="server" CssClass="form-control" AutoPostBack="true">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvManagerName" runat="server" ControlToValidate="ddlDelMgrName"
                            InitialValue="" ErrorMessage="Please select a manager" CssClass="text-danger" Display="Dynamic"
                            ValidationGroup="DeleteManagerGroup" />
                    </div>
                    <div class="form-group col-md-3">
                        <asp:Button ID="btnDelManager" runat="server" Text="Delete Manager" OnClick="btnDelManager_Click"
                            CssClass="btn btn-primary btn-same-size" ValidationGroup="DeleteManagerGroup" />
                    </div>
                </div>
                <div class="row">
                    <div class="form-group col-md-3">
                        <asp:Label ID="lblEditExpenseA" runat="server" AssociatedControlID="txtExpenseIdA">Edit Expense:</asp:Label>
                    </div>
                    <div class="form-group col-md-3">
                        <asp:TextBox ID="txtExpenseIdA" runat="server" CssClass="form-control" TextMode="Number" Placeholder="Enter Expense ID"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvEditExpenseA" runat="server" ControlToValidate="txtExpenseIdA"
                            ErrorMessage="ExpenseId is required" CssClass="text-danger" Display="Dynamic"
                            ValidationGroup="EditExpenseGroup" />
                    </div>
                    <div class="form-group col-md-3">
                        <asp:Button ID="btnEditExpenseA" runat="server" Text="Get Expense" OnClick="btnGetExpenseA_Click" CssClass="btn btn-primary btn-same-size" ValidationGroup="EditExpenseGroup" />
                    </div>
                </div>
                <div class="form-row">
                    <asp:Label ID="lblMsgA" runat="server" CssClass="text-success"></asp:Label>
                </div>
                <div class="form-row">
                    <asp:Repeater ID="rptExpenses" runat="server" OnItemCommand="rptExpenses_ItemCommand">
                        <HeaderTemplate>
                            <table class="table table-striped table-bordered">
                                <thead>
                                    <tr>
                                        <th>ExpenseId</th>
                                        <th>ManagerName</th>
                                        <th>FY</th>
                                        <th>Quarter</th>
                                        <th>Reportees</th>
                                        <th>Budget</th>
                                        <th>Expenses</th>
                                        <th>Balance</th>
                                        <th>Description</th>
                                        <th>BudgetDate</th>
                                        <th>ExpenseDate</th>
                                        <th class="text-center">Update</th>
                                    </tr>
                                </thead>
                                <tbody>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td><%# Eval("ExpenseId") %></td>
                                <td><%# Eval("ManagerName") %></td>
                                <td><%# Eval("FinancialYear") %></td>
                                <td><%# Eval("Quarter") %></td>
                                <td><%# Eval("Reportees") %></td>
                                <td><%# Eval("Budget") %></td>
                                <td>
                                    <asp:TextBox ID="txtRptExpenses" runat="server" CssClass="form-control" TextMode="Number" Text='<%# Eval("Expenses") %>'></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvRptExpenses" runat="server" ControlToValidate="txtRptExpenses" Display="Dynamic" Text="*" ForeColor="Red" ValidationGroup="RptExpenseValidationGroupA"></asp:RequiredFieldValidator>
                                </td>
                                <td><%# Eval("Balance") %></td>
                                <td>
                                    <asp:TextBox ID="txtRptDescription" runat="server" TextMode="MultiLine" Rows="2" CssClass="form-control" Text='<%# Eval("Description") %>'></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvRptDescription" runat="server" ControlToValidate="txtRptDescription" Display="Dynamic" Text="*" ForeColor="Red" ValidationGroup="RptExpenseValidationGroupA"></asp:RequiredFieldValidator>
                                </td>
                                <td><%# Eval("BudgetAllocatedDate") %></td>
                                <td><%# Eval("ExpenseDate") %></td>
                                <td class="text-center">
                                    <asp:Button ID="btnUpdate" runat="server" Text="Update" CommandName="Update" CommandArgument='<%# Eval("ExpenseId") %>' CssClass="btn btn-primary" ValidationGroup="RptExpenseValidationGroupA" />
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </asp:View>

            <asp:View ID="viewManager" runat="server">
                <!-- Content for Manager tab goes here -->
                <div class="form-row">
                    <div class="form-group col-md-3">
                        <asp:Label ID="lblManagerM" runat="server" AssociatedControlID="ddlManagerM">Manager:</asp:Label>
                        <asp:DropDownList ID="ddlManagerM" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlManagerM_SelectedIndexChanged"></asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvManagerM" runat="server" ControlToValidate="ddlManagerM"
                            ValidationGroup="ManagerValidation" InitialValue="" ErrorMessage="Please select a manager" CssClass="text-danger" Display="Dynamic" />
                    </div>
                    <div class="form-group col-md-3">
                        <asp:Label ID="lblYearM" runat="server" AssociatedControlID="ddlYearM">Financial Year:</asp:Label>
                        <asp:DropDownList ID="ddlYearM" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlYearM_SelectedIndexChanged">
                            <asp:ListItem Value="All">Please select</asp:ListItem>
                            <asp:ListItem Value="2023">2023</asp:ListItem>
                            <asp:ListItem Value="2024">2024</asp:ListItem>
                            <asp:ListItem Value="2025">2025</asp:ListItem>
                            <asp:ListItem Value="2026">2026</asp:ListItem>
                            <asp:ListItem Value="2027">2027</asp:ListItem>
                            <asp:ListItem Value="2028">2028</asp:ListItem>
                            <asp:ListItem Value="2029">2029</asp:ListItem>
                            <asp:ListItem Value="2030">2030</asp:ListItem>
                            <asp:ListItem Value="2020">2031</asp:ListItem>
                            <asp:ListItem Value="2021">2032</asp:ListItem>
                            <asp:ListItem Value="2022">2033</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="form-group col-md-3">
                        <asp:Label ID="lblQuarterM" runat="server" AssociatedControlID="ddlQuarterM">Quarter:</asp:Label>
                        <asp:DropDownList ID="ddlQuarterM" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlQuarterM_SelectedIndexChanged">
                            <asp:ListItem Value="All">Please select</asp:ListItem>
                            <asp:ListItem Value="Q1">Q1</asp:ListItem>
                            <asp:ListItem Value="Q2">Q2</asp:ListItem>
                            <asp:ListItem Value="Q3">Q3</asp:ListItem>
                            <asp:ListItem Value="Q4">Q4</asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvQuarterM" runat="server" ControlToValidate="ddlQuarterM"
                            ValidationGroup="ManagerValidation" InitialValue="All" ErrorMessage="Please select a quarter" CssClass="text-danger" Display="Dynamic" />
                    </div>
                </div>
                <div class="form-row">
                    <div class="form-group col-md-3">
                        <asp:Label ID="lblBudgetM" runat="server" AssociatedControlID="txtBudgetM">Budget:</asp:Label>
                        <asp:TextBox ID="txtBudgetM" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvBudgetM" runat="server" ControlToValidate="txtBudgetM"
                            ValidationGroup="ManagerValidation" ErrorMessage="Budget is required" CssClass="text-danger" Display="Dynamic" />
                    </div>
                    <div class="form-group col-md-3">
                        <asp:Label ID="lblHcM" runat="server" AssociatedControlID="txtHcM">No. of Reportees:</asp:Label>
                        <asp:TextBox ID="txtHcM" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvHcM" runat="server" ControlToValidate="txtHcM"
                            ValidationGroup="ManagerValidation" ErrorMessage="Reportees count is required" CssClass="text-danger" Display="Dynamic" />
                    </div>
                    <div class="form-group col-md-3">
                        <asp:Label ID="lblExpense" runat="server" AssociatedControlID="txtExpenseM">Expense:</asp:Label>
                        <asp:TextBox ID="txtExpenseM" runat="server" CssClass="form-control" AutoPostBack="true" TextMode="Number" Placeholder="Enter Expense"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvExpense" runat="server" ControlToValidate="txtExpenseM"
                            ValidationGroup="ManagerValidation" ErrorMessage="Expense amount is required" CssClass="text-danger" Display="Dynamic" />
                        <asp:RegularExpressionValidator ID="revExpense" runat="server" ControlToValidate="txtExpenseM"
                            ValidationGroup="ManagerValidation" ValidationExpression="^\d+(\.\d{1,2})?$" ErrorMessage="Invalid expense format" CssClass="text-danger" Display="Dynamic" />
                    </div>
                </div>
                <div class="form-row">
                    <div class="form-group col-md-3">
                        <asp:Label ID="lblDescriptionM" runat="server" AssociatedControlID="txtDescriptionA">Description:</asp:Label>
                        <asp:TextBox ID="txtDescriptionM" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2" Placeholder="Enter Description"></asp:TextBox>
                    </div>
                </div>
                <div class="form-row">
                    <div class="col-auto">
                        <asp:Button ID="btnSaveM" runat="server" Text="Save" OnClick="btnSaveM_Click" CssClass="btn btn-primary btn-same-size" ValidationGroup="ManagerValidation" />
                    </div>
                    <div class="col-md-6">
                        <asp:Label ID="lblMsgM" runat="server" CssClass="text-success"></asp:Label>
                    </div>
                </div>
                <div class="mt-4">
                    <asp:GridView ID="gvManager" runat="server" CssClass="table table-striped table-bordered"
                        AutoGenerateColumns="true" PageSize="5" AllowPaging="True" OnPageIndexChanging="gvManager_PageIndexChanging">
                    </asp:GridView>
                </div>
            </asp:View>

            <asp:View ID="viewReports" runat="server">
                <!-- Content for Reports tab goes here -->
                <div class="form-row">
                    <div class="form-group col-md-3">
                        <asp:Label ID="lblManagerR" runat="server" AssociatedControlID="ddlManagerR">Manager:</asp:Label>
                        <asp:DropDownList ID="ddlManagerR" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlManagerR_SelectedIndexChanged"></asp:DropDownList>
                    </div>
                    <div class="form-group col-md-3">
                        <asp:Label ID="lblYearR" runat="server" AssociatedControlID="ddlYearR">Financial Year:</asp:Label>
                        <asp:DropDownList ID="ddlYearR" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlYearR_SelectedIndexChanged">
                            <asp:ListItem Value="2023">2023</asp:ListItem>
                            <asp:ListItem Value="2024">2024</asp:ListItem>
                            <asp:ListItem Value="2025">2025</asp:ListItem>
                            <asp:ListItem Value="2026">2026</asp:ListItem>
                            <asp:ListItem Value="2027">2027</asp:ListItem>
                            <asp:ListItem Value="2028">2028</asp:ListItem>
                            <asp:ListItem Value="2029">2029</asp:ListItem>
                            <asp:ListItem Value="2030">2030</asp:ListItem>
                            <asp:ListItem Value="2020">2031</asp:ListItem>
                            <asp:ListItem Value="2021">2032</asp:ListItem>
                            <asp:ListItem Value="2022">2033</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="form-group col-md-3">
                        <asp:Label ID="lblQyarterR" runat="server" AssociatedControlID="ddlQuarterR">Quarter:</asp:Label>
                        <asp:DropDownList ID="ddlQuarterR" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlQuarterR_SelectedIndexChanged">
                            <asp:ListItem Value="Q1">Q1</asp:ListItem>
                            <asp:ListItem Value="Q2">Q2</asp:ListItem>
                            <asp:ListItem Value="Q3">Q3</asp:ListItem>
                            <asp:ListItem Value="Q4">Q4</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="form-row">
                    <div class="form-group col-md-3">
                        <asp:Label ID="lblTotalBudgetR" runat="server" AssociatedControlID="txtTotalBudgetR">Total Budget:</asp:Label>
                        <asp:TextBox ID="txtTotalBudgetR" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                    </div>
                    <div class="form-group col-md-3">
                        <asp:Label ID="lblTotalExpensesR" runat="server" AssociatedControlID="txtTotalExpensesR">Total Expenses:</asp:Label>
                        <asp:TextBox ID="txtTotalExpensesR" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
                <div class="form-row">
                    <div class="col-md-12 text-right">
                        <asp:Button ID="btnExcelExport" runat="server" Text="Export to Excel" OnClick="btnExcelExport_Click" CssClass="btn btn-primary btn-same-size" />
                    </div>
                </div>
                <asp:GridView ID="gvReports" runat="server" CssClass="table table-striped table-bordered"
                    AutoGenerateColumns="true" PageSize="10" AllowPaging="True" OnPageIndexChanging="gvReports_PageIndexChanging">
                </asp:GridView>

                <!-- ... Add other Reports tab content ... -->
            </asp:View>
        </asp:MultiView>

        <div class="Footer">
            <footer>
                MICROSOFT CONFIDENTIAL! FOR INTERNAL USE ONLY.
            </footer>
        </div>

        <!-- Add jQuery and Bootstrap JS -->
        <script src="https://code.jquery.com/jquery-3.5.1.slim.min.js"></script>
        <script src="https://cdn.jsdelivr.net/npm/@popperjs/core@2.9.3/dist/umd/popper.min.js"></script>
        <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/js/bootstrap.min.js"></script>

    </form>

    <script>
        $(document).ready(function () {
            // Calculate and populate txtTotalBudget based on txtHC and txtBudget
            $('#<%= txtHcA.ClientID %>').on('input', function () {
                var hc = parseFloat($(this).val());
                var budget = parseFloat($('#<%= txtBudget.ClientID %>').val());
                var totalBudget = isNaN(hc) || isNaN(budget) ? '' : (hc * budget).toFixed(2);
                $('#<%= txtTotalBudget.ClientID %>').val(totalBudget);
            });

            $('#<%= txtBudget.ClientID %>').on('input', function () {
                var hc = parseFloat($('#<%= txtHcA.ClientID %>').val());
                var budget = parseFloat($(this).val());
                var totalBudget = isNaN(hc) || isNaN(budget) ? '' : (hc * budget).toFixed(2);
                $('#<%= txtTotalBudget.ClientID %>').val(totalBudget);
            });
        });
    </script>

</body>

</html>

