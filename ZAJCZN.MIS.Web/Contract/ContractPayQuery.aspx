<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ContractPayQuery.aspx.cs" Inherits="ZAJCZN.MIS.Web.ContractPayQuery" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title></title>
    <style>
        .f-grid-row-summary .f-grid-cell-inner {
            font-weight: bold;
            color: red;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server" />
        <f:Panel ID="Panel1" runat="server" BodyPadding="5px" BoxConfigAlign="Stretch" BoxConfigPosition="Start"
            ShowBorder="false" ShowHeader="false" Layout="VBox">
            <Items>
                <f:Panel ID="Panel2" ShowHeader="false" ShowBorder="false" runat="server">
                    <Items>
                        <f:Toolbar ID="Toolbar2" runat="server">
                            <Items>
                                <f:DropDownList ID="ddlType" runat="server" LabelWidth="80px" Label="单据类型" Width="200px"
                                    LabelAlign="Right">
                                    <f:ListItem Text="--全部--" Value="" Selected="true" />
                                    <f:ListItem Text="收款" Value="1" />
                                    <f:ListItem Text="付款" Value="2" />
                                </f:DropDownList>
                                <f:DropDownList ID="ddlPay" runat="server" LabelWidth="80px" Label="审核状态" Width="200px"
                                    LabelAlign="Right">
                                    <f:ListItem Text="--全部--" Value="" Selected="true" />
                                    <f:ListItem Text="待审核" Value="1" />
                                    <f:ListItem Text="通过" Value="2" />
                                    <f:ListItem Text="未通过" Value="3" />
                                </f:DropDownList>
                                <f:DatePicker ID="dpStartDate" LabelWidth="120px" Label="申请时间" Width="260px"
                                    runat="server" LabelAlign="Right">
                                </f:DatePicker>
                                <f:DatePicker ID="dpEndDate" LabelWidth="10px" Label="~" Width="150px" runat="server"
                                    CompareControl="dpStartDate" CompareOperator="GreaterThan" CompareMessage="结束日期应该大于开始日期！">
                                </f:DatePicker>
                                <f:Button ID="btnSearch" MarginLeft="20px" runat="server" OnClick="btnSearch_Click" Icon="Find" Text="查询"></f:Button>
                                <f:ToolbarFill ID="ToolbarFill2" runat="server">
                                </f:ToolbarFill>
                            </Items>
                        </f:Toolbar>
                    </Items>
                </f:Panel>
                <f:Grid ID="Grid1" runat="server" ShowBorder="true" ShowHeader="false"
                    EnableCheckBoxSelect="true" DataKeyNames="ID" EnableSummary="true" SummaryPosition="Bottom"
                    AllowSorting="true" SortField="ID" SortDirection="DESC"
                    AllowPaging="true" IsDatabasePaging="true" OnPageIndexChange="Grid1_PageIndexChange">
                    <Toolbars>
                        <f:Toolbar ID="Toolbar1" Position="Top" runat="server">
                            <Items>
                                <f:Button ID="btnChangeEnableUsers" Icon="GroupEdit" EnablePostBack="false" runat="server"
                                    Text="审核付款单">
                                    <Menu ID="Menu1" runat="server">
                                        <f:MenuButton ID="btnEnableUsers" OnClick="btnEnableUsers_Click" runat="server" Text="通过申请">
                                        </f:MenuButton>
                                        <f:MenuButton ID="btnDisableUsers" OnClick="btnDisableUsers_Click" runat="server"
                                            Text="驳回申请">
                                        </f:MenuButton>
                                    </Menu>
                                </f:Button>
                                <f:ToolbarFill ID="ToolbarFill1" runat="server">
                                </f:ToolbarFill>
                            </Items>
                        </f:Toolbar>
                    </Toolbars>
                    <PageItems>
                        <f:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                        </f:ToolbarSeparator>
                        <f:ToolbarText ID="ToolbarText1" runat="server" Text="每页记录数：">
                        </f:ToolbarText>
                        <f:DropDownList ID="ddlGridPageSize" Width="80px" AutoPostBack="true" OnSelectedIndexChanged="ddlGridPageSize_SelectedIndexChanged"
                            runat="server">
                            <f:ListItem Text="10" Value="10" />
                            <f:ListItem Text="20" Value="20" />
                            <f:ListItem Text="50" Value="50" />
                            <f:ListItem Text="100" Value="100" />
                        </f:DropDownList>
                    </PageItems>
                    <Columns>
                        <f:RowNumberField></f:RowNumberField>
                        <f:TemplateField Width="80px" HeaderText="款项类型">
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Width="60px" Text='<%# Eval("PayType").ToString() =="1"?"收款":"付款" %>' ForeColor='<%# GetColor(Eval("PayType").ToString()) %>'></asp:Label>
                            </ItemTemplate>
                        </f:TemplateField>
                        <f:BoundField DataField="ApplyDate" HeaderText="录入时间" Width="100px" />
                        <f:TemplateField Width="80px" HeaderText="状态">
                            <ItemTemplate>
                                <asp:Label ID="Label4" runat="server" Width="60px" Text='<%# GetOrderState(Eval("ApplyState").ToString()) %>' ForeColor='<%# GetColor1(Eval("ApplyState").ToString()) %>'></asp:Label>
                            </ItemTemplate>
                        </f:TemplateField>
                        <f:BoundField DataField="PayWay" HeaderText="支付方式" Width="150px" />
                        <f:BoundField DataField="PayUser" ColumnID="PayUser"  HeaderText="付款对象" Width="200px" />
                        <f:BoundField DataField="PayMoney" ColumnID="PayMoney" HeaderText="金额(元)" Width="120px" />
                        <f:BoundField DataField="Operator" HeaderText="录入人员" Width="100px" />
                        <f:BoundField DataField="AproveUser" HeaderText="审核人" Width="100px" />
                        <f:BoundField DataField="AproveDate" HeaderText="审核时间" Width="100px" />
                        <f:BoundField DataField="Remark" HeaderText="备注" Width="300px" />
                    </Columns>
                </f:Grid>
            </Items>
        </f:Panel>
    </form>
</body>
</html>
