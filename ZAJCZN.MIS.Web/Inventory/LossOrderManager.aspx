<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LossOrderManager.aspx.cs" Inherits="ZAJCZN.MIS.Web.LossOrderManager" %>

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
            ShowHeader="false" Title="损耗管理">
            <Items>
                <f:Panel ID="Panel2" ShowHeader="false" ShowBorder="false" Layout="Column" runat="server">
                    <Items>
                        <f:TextBox ID="txtOrderNo" runat="server" Label="损耗单号" Width="400px" LabelAlign="Right" LabelWidth="80px" EmptyText="输入损耗单号或报损人查询"></f:TextBox>
                        <f:DatePicker ID="dpStartDate" LabelWidth="120px" Label=" 报损时间" Width="260px" runat="server" LabelAlign="Right">
                        </f:DatePicker>
                        <f:DatePicker ID="dpEndDate" LabelWidth="10px" Label="~" Width="150px" runat="server"
                            CompareControl="dpStartDate" CompareOperator="GreaterThan" CompareMessage="结束日期应该大于开始日期！">
                        </f:DatePicker>
                        <f:DropDownList ID="ddlState" Label="订单状态" LabelAlign="Right" runat="server" LabelWidth="80px" Width="180px">
                            <f:ListItem Text="-全部订单-" Value="" Selected="true" />
                            <f:ListItem Text="正式订单" Value="0" />
                            <f:ListItem Text="临时订单" Value="1" />
                        </f:DropDownList>
                        <f:Button ID="btnSearch" MarginLeft="20px" runat="server" OnClick="btnSearch_Click" Icon="Find" Text="查询"></f:Button>
                    </Items>
                </f:Panel>
                <f:Grid ID="Grid1" runat="server" BoxFlex="1" ShowBorder="true" ShowHeader="false"
                    EnableCheckBoxSelect="true"
                    DataKeyNames="ID,Name" AllowSorting="true" OnSort="Grid1_Sort" SortField="ID"
                    SortDirection="ASC" AllowPaging="true" IsDatabasePaging="true" OnPreDataBound="Grid1_PreDataBound"
                    OnRowCommand="Grid1_RowCommand" OnPageIndexChange="Grid1_PageIndexChange">
                    <Toolbars>
                        <f:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <f:ToolbarFill ID="ToolbarFill1" runat="server">
                                </f:ToolbarFill>
                                <f:Button ID="btnNew" runat="server" Icon="Add" OnClick="btnNew_Click" Text="新增损耗单">
                                </f:Button>
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
                        <f:RowNumberField TextAlign="Center" Width="35px" Hidden="true" EnablePagingNumber="true" />
                        <f:BoundField DataField="OrderNO" SortField="OrderNO" Width="160px" HeaderText="损耗单号" />
                        <f:BoundField DataField="OrderDate" SortField="OrderDate" Width="160px" HeaderText="出单时间" />
                        <f:BoundField DataField="UserName" SortField="UserName" Width="100px" HeaderText="报损人" />
                        <f:BoundField DataField="OrderNumber" SortField="OrderNumber" Width="100px" HeaderText="报损数量" />
                        <f:BoundField DataField="OrderAmount" SortField="OrderAmount" Width="120px" HeaderText="出库金额(元)" />
                        <f:TemplateField Width="110px" HeaderText="出库仓库">
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Width="110px" Text='<%# GetWareHouseID(Eval("WareHouseID").ToString()) %>'></asp:Label>
                            </ItemTemplate>
                        </f:TemplateField>
                        <f:TemplateField Width="80px" HeaderText="订单类型" SortField="IsTemp">
                            <ItemTemplate>
                                <asp:Label ID="lblState" runat="server" Width="80px" Text='<%#  Eval("IsTemp").ToString().Equals ("0")?"正式订单":"临时订单" %>'></asp:Label>
                            </ItemTemplate>
                        </f:TemplateField>
                        <f:BoundField DataField="Remark" SortField="Remark" Width="150px" HeaderText="领用备注" ExpandUnusedSpace="true" />
                        <f:LinkButtonField HeaderText="查看" TextAlign="Center" Icon="Information" ToolTip="查看损耗单详情"
                            CommandName="viewField" Width="60px" />
                        <f:LinkButtonField HeaderText="编辑" TextAlign="Center" Icon="Pencil" ToolTip="编辑领用单"
                            CommandName="editField" Width="50px" ID="lbtnEditField" />
                        <f:LinkButtonField ColumnID="deleteField" HeaderText="删除" TextAlign="Center" Icon="Delete" ToolTip="删除"
                            ConfirmText="确定删除此记录？" ConfirmTarget="Top" CommandName="Delete" Width="60px" />
                    </Columns>
                </f:Grid>
            </Items>
        </f:Panel>
    </form>
</body>
</html>
