<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AllotView.aspx.cs" Inherits="ZAJCZN.MIS.Web.AllotView" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server" />
        <f:Panel ID="Panel1" runat="server" BodyPadding="5px" ShowBorder="false" Layout="VBox" BoxConfigAlign="Stretch"
            BoxConfigPosition="Start" ShowHeader="false" Title="查看销售单">
            <Items>
                <f:Panel ID="GroupPanel3" runat="server" ShowHeader="false">
                    <Items>
                        <f:Toolbar ID="Toolbar2" runat="server">
                            <Items>
                                <f:ToolbarFill ID="ToolbarFill1" runat="server">
                                </f:ToolbarFill>
                                <f:Button ID="btnReturn" OnClick="btnReturn_Click" runat="server" Icon="arrowleft" Text="返回调拨单列表">
                                </f:Button>
                            </Items>
                        </f:Toolbar>
                    </Items>
                </f:Panel>
                <f:GroupPanel ID="GroupPanel1" runat="server" Title="调拨单基础信息">
                    <Items>
                        <f:Form ID="Form2" ShowBorder="false" ShowHeader="false" runat="server">
                            <Items>
                                <f:FormRow>
                                    <Items>
                                        <f:Label ID="lblOrderNo" runat="server" Label="调拨单号" LabelAlign="Right"></f:Label>
                                        <f:Label ID="lblStartDate" runat="server" LabelAlign="Right" Width="300px" Label="调拨日期"></f:Label>
                                        <f:Label ID="lblCount" runat="server" LabelAlign="Right" Label="商品数量" Text="0"></f:Label>
                                        <f:Label ID="lblAmount" runat="server" LabelAlign="Right" Label="订单金额(元)" Text="0"></f:Label>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow>
                                    <Items>
                                        <f:DropDownList ID="ddlCKWareHouseID" Readonly="true" Label="调拨出库仓库" LabelAlign="Right" DataTextField="WHName"
                                            DataValueField="ID" runat="server" Width="300px">
                                        </f:DropDownList>
                                        <f:DropDownList ID="ddlRKWareHouseID" Readonly="true" Label="调拨入库仓库" LabelAlign="Right" DataTextField="WHName"
                                            DataValueField="ID" runat="server" Width="300px">
                                        </f:DropDownList>
                                    </Items>
                                </f:FormRow>
                            </Items>
                        </f:Form>
                    </Items>
                </f:GroupPanel>
                <f:GroupPanel ID="GroupPanel2" runat="server" Title="订单商品信息">
                    <Items>
                        <f:Grid ID="Grid1" ShowBorder="true" ShowHeader="false" EnableCollapse="true"
                            runat="server" DataKeyNames="ID" AllowCellEditing="true" ClicksToEdit="1">
                            <Columns>
                                <f:TemplateField Width="150px" HeaderText="商品名称">
                                    <ItemTemplate>
                                        <asp:Label ID="Label2" runat="server" Width="200px" Text='<%# GetGoodsName(Eval("GoodsID").ToString()) %>'></asp:Label>
                                    </ItemTemplate>
                                </f:TemplateField>
                                <f:TemplateField Width="100px" HeaderText="商品编号">
                                    <ItemTemplate>
                                        <asp:Label ID="Label1" runat="server" Width="150px" Text='<%# GetGoodCode(Eval("GoodsID").ToString()) %>'></asp:Label>
                                    </ItemTemplate>
                                </f:TemplateField>
                                <f:TemplateField Width="60px" HeaderText="单位">
                                    <ItemTemplate>
                                        <asp:Label ID="Label4" runat="server" Width="80px" Text='<%# GetUnitName(Eval("GoodsID").ToString()) %>'></asp:Label>
                                    </ItemTemplate>
                                </f:TemplateField>
                                <f:BoundField DataField="GoodsNumber" SortField="GoodsNumber" Width="120px" HeaderText="调拨数量" />
                                <f:BoundField DataField="GoodsPrice" SortField="GoodsPrice" Width="120px" HeaderText="商品单价(元)" />
                                <f:BoundField DataField="GoodsAmount" SortField="GoodsAmount" Width="200px" HeaderText="商品金额(元)" />
                            </Columns>
                        </f:Grid>
                        <f:Label ID="labResult" EncodeText="false" runat="server">
                        </f:Label>
                    </Items>
                </f:GroupPanel>
            </Items>
        </f:Panel>
    </form>
</body>
</html>
