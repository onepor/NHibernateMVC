<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExpiredGoods.aspx.cs" Inherits="KZKJ.IOA.Web.ExpiredGoods" %>

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
            ShowHeader="false" Title="过期商品管理">
            <Items>
                <f:DatePicker ID="dpEndDate" LabelWidth="80px" Label="过期日期" AutoPostBack="true" OnTextChanged="dpEndDate_TextChanged"
                    runat="server">
                </f:DatePicker>
                <f:Grid ID="Grid1" runat="server" BoxFlex="1" ShowBorder="true" ShowHeader="false"
                    EnableCheckBoxSelect="true"
                    DataKeyNames="ID,Name" AllowSorting="true" OnSort="Grid1_Sort" SortField="ID"
                    SortDirection="ASC" AllowPaging="true" IsDatabasePaging="true" OnDataBinding="Grid1_PreDataBound"
                    OnRowCommand="Grid1_RowCommand" OnPageIndexChange="Grid1_PageIndexChange">
                    <Toolbars>
                        <f:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <f:Button runat="server" ID="btnReturns" Icon="cartremove" Text="退货"></f:Button>
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
                        <f:RowNumberField TextAlign="Center" Width="35px" EnablePagingNumber="true" />
                        <f:TemplateField Width="120px" HeaderText="商品名称" SortField="GoodsID">
                            <ItemTemplate>
                                <asp:Label ID="Label4" runat="server" Width="80px" Text='<%# GetGoodsName(Eval("GoodsID").ToString()) %>'></asp:Label>
                            </ItemTemplate>
                        </f:TemplateField>
                        <f:BoundField DataField="GoodsNumber" SortField="GoodsNumber" Width="90px" HeaderText="商品数量" />
                        <f:BoundField DataField="GuaranteeDate" SortField="GuaranteeDate" Width="150px" HeaderText="有效期至" />
                        <f:TemplateField Width="160px" HeaderText="剩余天数">
                            <ItemTemplate>
                                <asp:Label ID="Label3" runat="server" Width="80px" Text='<%# GetDays(Eval("GuaranteeDate").ToString()) %>'></asp:Label>
                            </ItemTemplate>
                        </f:TemplateField>
                        <f:TemplateField Width="100px" HeaderText="所属仓库">
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Width="80px" Text='<%# GetWareHouseID(Eval("OrderNO").ToString()) %>'></asp:Label>
                            </ItemTemplate>
                        </f:TemplateField>
                        <f:TemplateField Width="100px" HeaderText="商品供应商">
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Width="80px" Text='<%# GetSuplierID(Eval("OrderNO").ToString()) %>'></asp:Label>
                            </ItemTemplate>
                        </f:TemplateField>
                        <f:WindowField ColumnID="editField" Hidden="true" HeaderText="编辑" TextAlign="Center" Icon="Pencil" ToolTip="编辑"
                            WindowID="Window1" Title="编辑" DataIFrameUrlFields="ID" DataIFrameUrlFormatString="~/BusinessSet/PrinterEdit.aspx?id={0}&action=edit"
                            Width="80px" />
                        <f:LinkButtonField ColumnID="deleteField" Hidden="true" HeaderText="删除" TextAlign="Center" Icon="Delete" ToolTip="删除"
                            ConfirmText="确定删除此记录？" ConfirmTarget="Top" CommandName="Delete" Width="80px" />
                    </Columns>
                </f:Grid>
            </Items>
        </f:Panel>
    </form>
</body>
</html>
