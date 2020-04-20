<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ContractOrderManage.aspx.cs" Inherits="ZAJCZN.MIS.Web.ContractOrderManage" %>

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
            ShowHeader="false" Title="发货管理">
            <Items>
                <f:Panel ID="Panel2" ShowHeader="false" ShowBorder="false" Layout="Column" runat="server">
                    <Items>
                        <f:TextBox ID="txtOrderNo" runat="server" Label="查询单号" Width="350px" LabelAlign="Right" LabelWidth="80px" EmptyText="在客户名称、发货单号、系统单号中搜索"></f:TextBox>
                        <f:DatePicker ID="dpStartDate" LabelWidth="100px" Label="计价时间" Width="220px" runat="server" LabelAlign="Right">
                        </f:DatePicker>
                        <f:DatePicker ID="dpEndDate" LabelWidth="10px" Label="~" Width="130px" runat="server"
                            CompareControl="dpStartDate" CompareOperator="GreaterThan" CompareMessage="结束日期应该大于开始日期！">
                        </f:DatePicker>
                        <f:DropDownList ID="ddlState" Label="结算状态" LabelAlign="Right" runat="server" LabelWidth="80px" Width="180px">
                            <f:ListItem Text="-全部订单-" Value="" Selected="true" />
                            <f:ListItem Text="未结算" Value="1" />
                            <f:ListItem Text="已结算" Value="2" />
                        </f:DropDownList>
                        <f:DropDownList ID="ddlFix" Label="是否调整" LabelAlign="Right" runat="server" LabelWidth="80px" Width="180px">
                            <f:ListItem Text="-全部订单-" Value="" Selected="true" />
                            <f:ListItem Text="有调整" Value="1" />
                            <f:ListItem Text="无调整" Value="0" />
                        </f:DropDownList>
                        <f:Button ID="btnSearch" MarginLeft="20px" runat="server" OnClick="btnSearch_Click" Icon="Find" Text="查询"></f:Button>
                    </Items>
                </f:Panel>
                <f:Grid ID="Grid1" runat="server" BoxFlex="1" ShowBorder="true" ShowHeader="false" 
                    EnableCheckBoxSelect="false" DataKeyNames="ID,Name" AllowSorting="true" OnSort="Grid1_Sort" SortField="ID"
                    SortDirection="ASC" AllowPaging="true" IsDatabasePaging="true" OnPreDataBound="Grid1_PreDataBound"
                    OnRowCommand="Grid1_RowCommand" OnPageIndexChange="Grid1_PageIndexChange" OnPreRowDataBound="Grid1_PreRowDataBound">
                    <Toolbars>
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
                        <f:WindowField ColumnID="editField" HeaderText="查看" Hidden="false" TextAlign="Center" Icon="Zoom" ToolTip="查看发货单明细"
                            WindowID="Window1" Title="查看发货单明细" DataIFrameUrlFields="ID" DataIFrameUrlFormatString="~/Contract/FH/ContractOrderView.aspx?id={0}"
                            Width="50px" />
                        <f:LinkButtonField HeaderText="编辑" TextAlign="Center" Icon="Pencil" ToolTip="编辑发货单" CommandName="editField" Width="50px" ColumnID="lbtnEditField" />
                        <f:BoundField DataField="ContractInfo.ContractNO" SortField="ContractInfo.ContractNO" Width="160px" HeaderText="合同号" />
                        <f:BoundField DataField="ContractInfo.CustomerName" SortField="ContractInfo.CustomerName" Width="200px" HeaderText="客户名称" />
                        <f:BoundField DataField="ManualNO" SortField="ManualNO" Width="120px" HeaderText="发货单号" />
                        <f:BoundField DataField="OrderNO" SortField="OrderNO" Width="180px" HeaderText="系统单号" />
                        <f:TemplateField Width="90px" HeaderText="是否结算">
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Width="90px" Text='<%# Eval("OrderState").ToString().Equals ("2")?"已结算":"" %>'></asp:Label>
                            </ItemTemplate>
                        </f:TemplateField>
                        <f:TemplateField Width="90px" HeaderText="是否调整">
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Width="90px" Text='<%# Eval("IsFix").ToString().Equals ("1")?"是":"" %>'></asp:Label>
                            </ItemTemplate>
                        </f:TemplateField>
                        <f:BoundField DataField="ValuationDate" SortField="ValuationDate" Width="120px" HeaderText="计价日期" DataFormatString="{0:yyyy-MM-dd}" />
                        <f:BoundField DataField="CarNO" SortField="CarNO" Width="100px" HeaderText="送货车牌" />
                        <f:BoundField DataField="Operator" Width="80px" HeaderText="建单人" />
                        <f:BoundField DataField="OrderDate" SortField="OrderDate" Width="120px" HeaderText="建单时间" DataFormatString="{0:yyyy-MM-dd}" />
                        <f:BoundField DataField="Remark" Width="300px" HeaderText="订单备注" />
                    </Columns>
                </f:Grid>
            </Items>
        </f:Panel>
        <f:Window ID="Window1" runat="server" IsModal="true" Hidden="true" Target="Top" EnableResize="true"
            EnableMaximize="true" EnableIFrame="true" IFrameUrl="about:blank" Width="1300px"
            Height="700px" OnClose="Window1_Close">
        </f:Window>
    </form>
</body>
</html>
