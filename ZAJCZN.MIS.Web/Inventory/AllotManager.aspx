<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AllotManager.aspx.cs" Inherits="ZAJCZN.MIS.Web.AllotManager" %>

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
            ShowHeader="false" Title="调拨管理">
            <Items>
                <f:Panel ID="Panel2" ShowHeader="false" ShowBorder="false" Layout="Column" runat="server">
                    <Items>

                        <f:TextBox ID="txtOrderNo" runat="server" Label="调拨单号" EnableBlurEvent="true" OnBlur="btnSearch_Click" Width="240px" LabelAlign="Right" LabelWidth="80px"></f:TextBox>
                        <f:DatePicker ID="dpStartDate" AutoPostBack="true" OnTextChanged="btnSearch_Click" LabelWidth="110px"
                            Label="调拨起止时间" Width="210px" runat="server" LabelAlign="Right">
                        </f:DatePicker>
                        <f:DatePicker ID="dpEndDate" LabelWidth="10px" AutoPostBack="true"
                            OnTextChanged="btnSearch_Click" Label="~" Width="110px" runat="server"
                            CompareControl="dpStartDate" CompareOperator="GreaterThan" CompareMessage="结束日期应该大于开始日期！">
                        </f:DatePicker>
                        <f:DropDownList ID="ddlCKWareHouseID" AutoPostBack="true" OnSelectedIndexChanged="btnSearch_Click" Label="调拨出库仓库"
                            LabelAlign="Right" DataTextField="WHName" LabelWidth="110px" DataValueField="ID" runat="server" Width="200px">
                        </f:DropDownList>
                        <f:DropDownList ID="ddlRKWareHouseID" AutoPostBack="true" OnSelectedIndexChanged="btnSearch_Click" Label="调拨入库仓库"
                            LabelAlign="Right" DataTextField="WHName" LabelWidth="110px" DataValueField="ID" runat="server" Width="200px">
                        </f:DropDownList>
                        <f:DropDownList ID="ddlState" Label="订单状态" LabelAlign="Right" runat="server" LabelWidth="80px" Width="170px"
                            AutoPostBack="true" OnSelectedIndexChanged="ddlState_SelectedIndexChanged">
                            <f:ListItem Text="-全部订单-" Value="" Selected="true" />
                            <f:ListItem Text="正式订单" Value="0" />
                            <f:ListItem Text="临时订单" Value="1" />
                        </f:DropDownList>

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
                                <f:Button ID="btnDeleteSelected" Icon="Delete" runat="server" Hidden="true" Text="删除选中记录" OnClick="btnDeleteSelected_Click">
                                </f:Button>
                                <f:ToolbarFill ID="ToolbarFill1" runat="server">
                                </f:ToolbarFill>
                                <f:Button ID="btnNew" runat="server" Icon="Add" OnClick="btnNew_Click" Text="新增调拨">
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
                        <f:BoundField DataField="OrderNO" SortField="OrderNO" Width="140px" HeaderText="调拨单号" />
                        <f:BoundField DataField="OrderDate" SortField="OrderDate" Width="150px" HeaderText="调拨时间" />
                        <f:TemplateField Width="80px" HeaderText="出库仓库">
                            <ItemTemplate>
                                <asp:Label ID="Label4" runat="server" Width="80px" Text='<%# GetWHName(Eval("CKWareHouseID").ToString()) %>'></asp:Label>
                            </ItemTemplate>
                        </f:TemplateField>
                        <f:TemplateField Width="80px" HeaderText="入库仓库">
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Width="80px" Text='<%# GetWHName(Eval("RKWareHouseID").ToString()) %>'></asp:Label>
                            </ItemTemplate>
                        </f:TemplateField>
                        <f:BoundField DataField="AllotCount" SortField="AllotCount" Width="100px" HeaderText="调拨总数" />
                        <f:BoundField DataField="AllotAmount" SortField="AllotAmount" Width="100px" HeaderText="商品总金额" />
                        <f:BoundField DataField="Remark" SortField="Remark" Width="150px" HeaderText="订单备注" />
                        <f:TemplateField Width="80px" HeaderText="订单状态">
                            <ItemTemplate>
                                <asp:Label ID="lblState" runat="server" Width="80px" Text='<%#  Eval("IsTemp").ToString().Equals ("0")?"正式订单":"临时订单" %>'></asp:Label>
                            </ItemTemplate>
                        </f:TemplateField>
                        <f:LinkButtonField HeaderText="查看" TextAlign="Center" Icon="Information" ToolTip="查看进货订单详情"
                            CommandName="viewField" Width="80px" />
                        <f:LinkButtonField HeaderText="编辑" TextAlign="Center" Icon="Pencil" ToolTip="编辑订单详情"
                            CommandName="editField" Width="80px" ID="lbtnEditField" />
                        <f:LinkButtonField ColumnID="deleteField" HeaderText="删除" TextAlign="Center" Icon="Delete" ToolTip="删除"
                            ConfirmText="确定删除此记录？" ConfirmTarget="Top" CommandName="Delete" Width="80px" />
                    </Columns>
                </f:Grid>
            </Items>
        </f:Panel>
    </form>
</body>
</html>
