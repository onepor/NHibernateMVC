<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ContractFHManage.aspx.cs" Inherits="ZAJCZN.MIS.Web.ContractFHManage" %>

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
                        <f:TextBox ID="txtOrderNo" runat="server" Label="查询单号" Width="300px" LabelAlign="Right" LabelWidth="80px" EmptyText="在客户名称、发货单号中搜索"></f:TextBox>
                        <f:DatePicker ID="dpStartDate" LabelWidth="120px" Label="发货起止时间" Width="260px" runat="server" LabelAlign="Right">
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
                    EnableCheckBoxSelect="false" DataKeyNames="ID,Name" AllowSorting="true" OnSort="Grid1_Sort" SortField="ID"
                    SortDirection="ASC" AllowPaging="true" IsDatabasePaging="true" 
                    OnPreDataBound="Grid1_PreDataBound" OnPreRowDataBound="Grid1_PreRowDataBound"
                    OnRowCommand="Grid1_RowCommand" OnPageIndexChange="Grid1_PageIndexChange">
                    <Toolbars>
                        <f:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <f:Button ID="btnDeleteSelected" Icon="Delete" runat="server" Hidden="true" Text="删除选中记录" OnClick="btnDeleteSelected_Click">
                                </f:Button>
                                <f:Button ID="btnNew" runat="server" Icon="Add" OnClick="btnNew_Click" Text="新增发货单">
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
                        <f:WindowField ColumnID="editField" HeaderText="查看" Hidden="false" TextAlign="Center" Icon="Zoom" ToolTip="查看发货单明细"
                            WindowID="Window1" Title="查看发货单明细" DataIFrameUrlFields="ID" DataIFrameUrlFormatString="~/Contract/FH/ContractFHView.aspx?id={0}"
                            Width="50px" />
                        <f:LinkButtonField HeaderText="编辑" TextAlign="Center" Icon="Pencil" ToolTip="编辑发货单"
                            CommandName="editField" Width="50px" ColumnID="lbtnEditField" />
                        <f:LinkButtonField ColumnID="deleteField" HeaderText="删除" TextAlign="Center" Icon="Delete"
                            ToolTip="删除临时单" ConfirmText="确定删除此记录？" ConfirmTarget="Top" CommandName="Delete" Width="50px" />
                        <f:TemplateField Width="80px" HeaderText="订单类型" SortField="IsTemp">
                            <ItemTemplate>
                                <asp:Label ID="lblState" runat="server" Width="80px" Text='<%#  Eval("IsTemp").ToString().Equals ("0")?"正式订单":"临时订单" %>'></asp:Label>
                            </ItemTemplate>
                        </f:TemplateField>
                        <f:BoundField DataField="ContractInfo.CustomerName" SortField="ContractInfo.CustomerName" Width="200px" HeaderText="客户名称" />
                        <f:BoundField DataField="ManualNO" SortField="ManualNO" Width="160px" HeaderText="发货单号" />
                        <f:BoundField DataField="OrderDate" SortField="OrderDate" Width="140px" HeaderText="发货时间" DataFormatString="{0:yyyy-MM-dd}" />
                        <f:BoundField DataField="CarNO" SortField="CarNO" Width="100px" HeaderText="送货车牌" />
                        <f:BoundField DataField="Operator" Width="100px" HeaderText="建单人" />
                        <f:BoundField DataField="Remark" Width="300px" HeaderText="订单备注" ExpandUnusedSpace="true" />
                    </Columns>
                </f:Grid>
            </Items>
        </f:Panel>
        <f:Window ID="Window1" runat="server" IsModal="true" Hidden="true" Target="Top" EnableResize="true"
            EnableMaximize="true" EnableIFrame="true" IFrameUrl="about:blank" Width="1200px"
            Height="700px" OnClose="Window1_Close">
        </f:Window>
    </form>
</body>
</html>
