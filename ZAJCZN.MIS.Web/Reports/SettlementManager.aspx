<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SettlementManager.aspx.cs" Inherits="ZAJCZN.MIS.Web.SettlementManager" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server" />
        <f:Panel ID="Panel1" runat="server" BodyPadding="5px"
            ShowBorder="false" Layout="VBox" BoxConfigAlign="Stretch" BoxConfigPosition="Start"
            ShowHeader="false" Title="交班信息列表">
            <Items>
                <f:Panel ID="Panel2" ShowHeader="false" ShowBorder="false" Layout="Column" runat="server">
                    <Items>
                        <f:DatePicker ID="dpStartDate" LabelWidth="120px" Label="营业起止时间" Width="260px" runat="server" LabelAlign="Right">
                        </f:DatePicker>
                        <f:DatePicker ID="dpEndDate" LabelWidth="10px" Label="~" Width="150px" runat="server"
                            CompareControl="dpStartDate" CompareOperator="GreaterThan" CompareMessage="结束日期应该大于开始日期！">
                        </f:DatePicker>
                        <f:Button ID="btnSearch" MarginLeft="20px" runat="server" OnClick="btnSearch_Click" EnableAjax="false" EnablePostBack="true" Icon="SystemSearch" Text="查询"></f:Button>
                        <f:Button runat="server" MarginLeft="20px" ID="btnExcel" OnClick="btnExcel_Click" EnableAjax="false" Icon="PageExcel" Text="导出报表"></f:Button>
                    </Items>
                </f:Panel>
                <f:Grid ID="Grid1" runat="server" BoxFlex="1" ShowBorder="true" ShowHeader="false" 
                    EnableCheckBoxSelect="true" DataKeyNames="ID" AllowSorting="true"  SortField="ID"
                    SortDirection="ASC" AllowPaging="true" IsDatabasePaging="true" 
                    OnPageIndexChange="Grid1_PageIndexChange">
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
                        <f:BoundField DataField="SettlementDate"  Width="150px" HeaderText="结算时间" />
                        <f:BoundField DataField="CustomerCount"  Width="100px" HeaderText="就餐人数" />
                        <f:BoundField DataField="OrderCount"  Width="100px" HeaderText="成交量" />
                        <f:BoundField DataField="AmountReceivable"  Width="150px" HeaderText="营业金额(元)" />
                        <f:BoundField DataField="AmountCollected"  Width="150px" HeaderText="实收金额(元)" /> 
                        <f:WindowField ColumnID="editField" HeaderText="反结算" TextAlign="Center" Icon="Pencil" ToolTip="反结算"
                            WindowID="WindowSettlement" Title="反结算交班报表" DataIFrameUrlFields="ID" DataIFrameUrlFormatString="~/Reports/SettlementEdit.aspx?id={0}&type=1"
                            Width="80px" />
                        <f:WindowField ColumnID="ViewField" HeaderText="查看" TextAlign="Center" Icon="ApplicationViewList" ToolTip="查看"
                            WindowID="WindowSettlement" Title="查看交班报表" DataIFrameUrlFields="ID" DataIFrameUrlFormatString="~/Reports/SettlementEdit.aspx?id={0}&type=0"
                            Width="80px" />
                    </Columns>
                </f:Grid>
            </Items>
        </f:Panel>
        <f:Window ID="WindowSettlement" runat="server" IsModal="true" Hidden="true" Target="Top" EnableResize="true"
            EnableMaximize="true" EnableIFrame="true" IFrameUrl="about:blank" Width="600px"
            Height="600px" OnClose="WindowSettlement_Close">
        </f:Window>
    </form>
</body>
</html>
