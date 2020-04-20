<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DinnerOrderEdit.aspx.cs" Inherits="ZAJCZN.MIS.Web.DinnerOrderEdit" %>

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
            ShowHeader="false" Title="餐区管理">
            <Items>
                <f:GroupPanel runat="server" Title="餐台信息">
                    <Toolbars>
                        <f:Toolbar ID="Toolbar1" Position="Top" runat="server">
                            <Items>
                                <f:Label CssStyle="font-size:20px;color: red;margin-left:30px" ID="txbTitle" runat="server"></f:Label>
                                <f:Label ID="txbPopulation" LabelAlign="Right" runat="server" Label="就餐人数"></f:Label>
                                <f:Label ID="txbOpenTime" LabelAlign="Right" runat="server" Label="开台时间"></f:Label>
                                <f:Label ID="txbClearTime" LabelAlign="Right" runat="server" Label="结账时间"></f:Label>
                                <f:Label ID="txtPayType" LabelAlign="Right" runat="server" Label="结算状态"></f:Label>
                                <f:ToolbarFill ID="ToolbarFill1" runat="server">
                                </f:ToolbarFill>
                                <f:Button ID="btnReturn" Width="100px" Height="35px" CssStyle="font-size:18px" OnClick="btnReturn_Click" runat="server" Icon="arrowleft" Text="返 回"></f:Button>
                            </Items>
                        </f:Toolbar>
                    </Toolbars>
                </f:GroupPanel>
                <f:GroupPanel ID="GroupPanel1" runat="server" Title="结算信息">
                    <Items>
                        <f:Form ID="Form2" ShowBorder="false" ShowHeader="false" runat="server">
                            <Items>
                                <f:FormRow>
                                    <Items>
                                        <f:Label ID="txbMoneys" LabelAlign="Right" runat="server" Label="消费金额"></f:Label>
                                        <f:Label ID="txbPrePrice" LabelAlign="Right" runat="server" Label="赠送金额"></f:Label>
                                        <f:Label ID="txbFactPrice" LabelAlign="Right" runat="server" Label="应付金额"></f:Label>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow>
                                    <Items>
                                        <f:Label ID="lblVipCard" LabelAlign="Right" runat="server" Label="会员号"></f:Label>
                                        <f:Label ID="lblGroup" LabelAlign="Right" runat="server" Label="团购信息"></f:Label>
                                        <f:Label ID="lblGroupNO" LabelAlign="Right" runat="server" Label="团购券"></f:Label>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow>
                                    <Items>
                                        <f:Label ID="lblCash" LabelAlign="Right" runat="server" Label="现金支付"></f:Label>
                                        <f:Label ID="lblMember" LabelAlign="Right" runat="server" Label="会员卡"></f:Label>
                                        <f:Label ID="lblCard" LabelAlign="Right" runat="server" Label="刷卡支付"></f:Label>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow>
                                    <Items>
                                        <f:Label ID="lblWX" LabelAlign="Right" runat="server" Label="微信支付"></f:Label>
                                        <f:Label ID="lblZFB" LabelAlign="Right" runat="server" Label="支付宝"></f:Label>
                                        <f:Label ID="lblML" LabelAlign="Right" runat="server" Label="抹零"></f:Label>
                                    </Items>
                                </f:FormRow>
                            </Items>
                        </f:Form>
                    </Items>
                </f:GroupPanel>
                <f:GroupPanel runat="server" Title="点菜详情">
                    <Items>
                        <f:Grid ID="Grid1" runat="server" BoxFlex="1" ShowBorder="true" ShowHeader="false" 
                            EnableCheckBoxSelect="false" EnableCollapse="true" AllowCellEditing="true" ClicksToEdit="2"
                            DataKeyNames="ID,Name" AllowSorting="true" OnSort="Grid1_Sort" SortField="ID"
                            SortDirection="ASC" AllowPaging="false" IsDatabasePaging="true" Height="270px">
                            <PageItems>
                            </PageItems>
                            <Columns>
                                <f:RowNumberField></f:RowNumberField>
                                <f:TemplateField Width="200px" HeaderText="菜品名称" ExpandUnusedSpace="true">
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%# GetDishesNmae(Eval("DishesID").ToString()) %>'></asp:Label>
                                    </ItemTemplate>
                                </f:TemplateField>
                                <f:BoundField DataField="DishesCount" SortField="DishesCount" Width="150px" HeaderText="菜品数量" />
                                <f:BoundField DataField="Price" SortField="Price" Width="150px" HeaderText="菜品单价" />
                                <f:BoundField DataField="Moneys" SortField="Moneys" Width="150px" HeaderText="小计" />
                                <f:TemplateField Width="100px" HeaderText="菜品类型">
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%# Eval("DishesType").ToString()=="1"?"": Eval("DishesType").ToString()=="2"?"退菜":"团购菜品" %>'></asp:Label>
                                    </ItemTemplate>
                                </f:TemplateField>
                                <f:TemplateField Width="100px" HeaderText="赠送">
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%# Eval("IsFree").ToString()=="0"?"":"赠送" %>'></asp:Label>
                                    </ItemTemplate>
                                </f:TemplateField>
                            </Columns>
                        </f:Grid>
                    </Items>
                </f:GroupPanel>
            </Items>
        </f:Panel>
    </form>
</body>
</html>
