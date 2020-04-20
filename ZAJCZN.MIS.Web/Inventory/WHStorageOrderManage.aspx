<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WHStorageOrderManage.aspx.cs" Inherits="ZAJCZN.MIS.Web.WHStorageOrderManage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server" />
        <f:Panel ID="Panel1" runat="server" BodyPadding="5px"
            ShowBorder="false" Layout="VBox" BoxConfigAlign="Stretch" BoxConfigPosition="Start"
            ShowHeader="false" Title="入库单查询">
            <Items>
                <f:Panel ID="Panel2" ShowHeader="false" ShowBorder="false" Layout="Column" runat="server">
                    <Items>
                        <f:TextBox ID="txtOrderNo" runat="server" Label="入库(业务)单号" Width="300px" LabelAlign="Right" LabelWidth="120px"></f:TextBox>
                        <f:DatePicker ID="dpStartDate" LabelWidth="120px" Label="入库起止时间" Width="260px" runat="server" LabelAlign="Right">
                        </f:DatePicker>
                        <f:DatePicker ID="dpEndDate" LabelWidth="10px" Label="~" Width="150px" runat="server"
                            CompareControl="dpStartDate" CompareOperator="GreaterThan" CompareMessage="结束日期应该大于开始日期！">
                        </f:DatePicker>
                        <f:DropDownList ID="ddlSuplier" Label="供应商" LabelAlign="Right" DataTextField="SupplierName"
                            DataValueField="ID" runat="server" Width="260px" Hidden="true">
                        </f:DropDownList>

                        <f:Button ID="btnSearch" MarginLeft="20px" runat="server" OnClick="btnSearch_Click" Icon="Find" Text="查询"></f:Button>
                    </Items>
                </f:Panel>
                <f:Grid ID="Grid1" runat="server" BoxFlex="1" ShowBorder="true" ShowHeader="false"
                    EnableCheckBoxSelect="false" DataKeyNames="ID,Name" AllowSorting="true" OnSort="Grid1_Sort" SortField="ID"
                    SortDirection="ASC" AllowPaging="true" IsDatabasePaging="true"
                    OnRowCommand="Grid1_RowCommand" OnPageIndexChange="Grid1_PageIndexChange">
                    <Toolbars>
                        <f:Toolbar ID="Toolbar1" runat="server">
                            <Items>
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
                        <f:LinkButtonField HeaderText="查看" TextAlign="Center" Icon="Information" ToolTip="查看入库单详情"
                            CommandName="viewField" Width="80px" />
                        <f:RowNumberField TextAlign="Center" Width="35px" Hidden="true" EnablePagingNumber="true" />
                        <f:BoundField DataField="OrderNO" SortField="OrderNO" Width="200px" HeaderText="入库单号" />
                        <f:BoundField DataField="BOrderNO" SortField="BOrderNO" Width="200px" HeaderText="业务单号" />
                        <f:BoundField DataField="OrderDate" SortField="OrderDate" Width="230px" HeaderText="入库时间" />
                        <f:BoundField DataField="OrderNumber" SortField="OrderNumber" Width="100px" HeaderText="商品数量" />
                        <f:BoundField DataField="OrderAmount" SortField="OrderAmount" Width="120px" HeaderText="入库金额" Hidden="true" />
                        <f:TemplateField Width="100px" HeaderText="入库仓库">
                            <ItemTemplate>
                                <asp:Label ID="Label4" runat="server" Width="100px" Text='<%# GetWHName(Eval("WareHouseID").ToString()) %>'></asp:Label>
                            </ItemTemplate>
                        </f:TemplateField>
                        <f:BoundField DataField="Remark" SortField="Remark" Width="120px" HeaderText="入库备注" />
                        <f:BoundField DataField="Operator" SortField="Operator" Width="100px" HeaderText="操作员" />
                    </Columns>
                </f:Grid>
            </Items>
        </f:Panel>
    </form>
</body>
</html>
