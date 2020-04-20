<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReceivingOrderView.aspx.cs" Inherits="ZAJCZN.MIS.Web.ReceivingOrderView" %>

<!DOCTYPE html>
<html>
<head id="Head1" runat="server">
    <title></title>
    <meta name="sourcefiles" content="~/PublicWebForm/OrderGoodsSelectDialog.aspx" />
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server" />
        <f:Panel ID="Panel1" runat="server" BodyPadding="5px" ShowBorder="false" Layout="VBox" BoxConfigAlign="Stretch"
            BoxConfigPosition="Start" ShowHeader="false" Title="查看合同收货单">
            <Items>
                <f:Panel ID="GroupPanel3" runat="server" ShowHeader="false"  Hidden="true">
                    <Items>
                        <f:Toolbar ID="Toolbar2" runat="server">
                            <Items>
                                <f:ToolbarFill ID="ToolbarFill1" runat="server">
                                </f:ToolbarFill>
                                <f:Button ID="btnClose" Icon="SystemClose" EnablePostBack="false" runat="server"
                                    Text="关闭">
                                </f:Button>
                            </Items>
                        </f:Toolbar>
                    </Items>
                </f:Panel>
                <f:GroupPanel ID="GroupPanel1" runat="server" Title="收货单基础信息">
                    <Items>
                        <f:Form ID="Form2" ShowBorder="false" ShowHeader="false" runat="server">
                            <Items>
                                <f:FormRow>
                                    <Items>
                                        <f:Label ID="lblAmount" runat="server" LabelAlign="Right" Label="收货单号" Text="0" LabelWidth="100px"></f:Label>
                                        <f:Label ID="lblOrderNo" runat="server" Label="系统单号" LabelAlign="Right" LabelWidth="100px"></f:Label>
                                        <f:Label ID="lblDate" runat="server" Label="收货日期" LabelAlign="Right" LabelWidth="100px" Readonly="true"></f:Label>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow>
                                    <Items>
                                        <f:DropDownList ID="ddlCar" LabelWidth="100px" LabelAlign="Right" Label="货车"
                                            DataTextField="CarName" DataValueField="ID" runat="server" Readonly="true">
                                        </f:DropDownList>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow>
                                    <Items>
                                        <f:DropDownList ID="ddlContract" LabelWidth="100px" LabelAlign="Right" Label="合同号"
                                            DataTextField="ContarctName" DataValueField="ID" Readonly="true" runat="server">
                                        </f:DropDownList>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow>
                                    <Items>
                                        <f:TextBox ID="txtRemark" LabelAlign="Right" runat="server" Label="订单备注"></f:TextBox>
                                    </Items>
                                </f:FormRow>
                            </Items>
                        </f:Form>
                    </Items>
                </f:GroupPanel>
                <f:TabStrip ID="tsDetail" CssClass="blockpanel" ShowBorder="true" TabPosition="Top"
                    EnableTabCloseMenu="false" ActiveTabIndex="0" runat="server">
                    <Tabs>
                        <f:Tab ID="tabGoodsDetail" Title="收货明细" BodyPadding="10px" Layout="Fit" runat="server">
                            <Items>
                                <f:Grid ID="Grid1" ShowBorder="true" ShowHeader="false" EnableCollapse="true"
                                    runat="server" DataKeyNames="ID" ClicksToEdit="1" AllowPaging="false" EnableCheckBoxSelect="false">
                                    <Toolbars>
                                    </Toolbars>
                                    <Columns>
                                        <f:TemplateField Width="100px" HeaderText="出库仓库">
                                            <ItemTemplate>
                                                <asp:Label ID="Label2" runat="server" Width="100px" Text='<%# GetWHName(Eval("WareHouseID").ToString()) %>'></asp:Label>
                                            </ItemTemplate>
                                        </f:TemplateField>
                                        <f:TemplateField Width="100px" HeaderText="物品分类">
                                            <ItemTemplate>
                                                <asp:Label ID="Label1" runat="server" Width="100px" Text='<%# GetType(Eval("GoodTypeID").ToString()) %>'></asp:Label>
                                            </ItemTemplate>
                                        </f:TemplateField>
                                        <f:BoundField DataField="GoodsInfo.EquipmentName" SortField="GoodsInfo.EquipmentName" Width="200px" HeaderText="商品名称" />
                                        <f:BoundField DataField="GoodsInfo.EquipmentCode" SortField="GoodsInfo.EquipmentCode" Width="100px" HeaderText="商品编号" />
                                        <f:BoundField DataField="GoodsInfo.Standard" SortField="GoodsInfo.Standard" Width="100px" HeaderText="规格" />
                                        <f:TemplateField Width="80px" HeaderText="单位">
                                            <ItemTemplate>
                                                <asp:Label ID="Label4" runat="server" Width="80px" Text='<%# GetUnitName(Eval("GoodsInfo.EquipmentUnit").ToString()) %>'></asp:Label>
                                            </ItemTemplate>
                                        </f:TemplateField>
                                        <f:RenderField Width="100px" ColumnID="GoodsNumber" DataField="GoodsNumber" FieldType="Double"
                                            HeaderText="收货数量">
                                            <Editor>
                                                <f:NumberBox ID="tbxUsingCount" NoDecimal="false" NoNegative="true" MinValue="1" DecimalPrecision="3"
                                                    MaxValue="5000" runat="server">
                                                </f:NumberBox>
                                            </Editor>
                                        </f:RenderField>
                                        <f:TemplateField Width="90px" HeaderText="计价方式">
                                            <ItemTemplate>
                                                <asp:Label ID="Label7" runat="server" Width="90px" Text='<%# GetPayUnit(Eval("PayUnit").ToString()) %>'></asp:Label>
                                            </ItemTemplate>
                                        </f:TemplateField>
                                        <f:TemplateField Width="80px" HeaderText="计价单位">
                                            <ItemTemplate>
                                                <asp:Label ID="Label6" runat="server" Width="80px" Text='<%# GetFYUnitName(Eval("GoodsCalcUnit").ToString()) %>'></asp:Label>
                                            </ItemTemplate>
                                        </f:TemplateField>
                                        <f:BoundField DataField="GoodCalcPriceNumber" Width="100px" HeaderText="计价数量" />
                                        <f:RenderField Width="120px" ColumnID="GoodsUnitPrice" DataField="GoodsUnitPrice" FieldType="Double"
                                            HeaderText="日租金(元)">
                                            <Editor>
                                                <f:NumberBox ID="txtGoodsUnitPrice" NoDecimal="false" NoNegative="true" MinValue="1" DecimalPrecision="3"
                                                    MaxValue="5000" runat="server">
                                                </f:NumberBox>
                                            </Editor>
                                        </f:RenderField>
                                    </Columns>
                                </f:Grid>
                            </Items>
                        </f:Tab>
                        <f:Tab ID="tabOtherGoods" Title="辅材明细" BodyPadding="10px" Layout="Fit" runat="server">
                            <Items>
                                <f:Grid ID="GridSecondDetail" ShowBorder="true" ShowHeader="false" EnableCollapse="true"
                                    runat="server" DataKeyNames="ID" ClicksToEdit="1" EnableCheckBoxSelect="false" AllowPaging="false">
                                    <Columns>
                                        <f:TemplateField Width="100px" HeaderText="出库仓库">
                                            <ItemTemplate>
                                                <asp:Label ID="Label8" runat="server" Width="100px" Text='<%# GetWHName(Eval("WareHouseID").ToString()) %>'></asp:Label>
                                            </ItemTemplate>
                                        </f:TemplateField>
                                        <f:TemplateField Width="100px" HeaderText="主材分类">
                                            <ItemTemplate>
                                                <asp:Label ID="Label9" runat="server" Width="100px" Text='<%# GetType(Eval("MainGoodsInfo.EquipmentTypeID").ToString()) %>'></asp:Label>
                                            </ItemTemplate>
                                        </f:TemplateField>
                                        <f:BoundField DataField="MainGoodsInfo.EquipmentName" SortField="MainGoodsInfo.EquipmentName" Width="200px" HeaderText="主材名称" />
                                        <f:BoundField DataField="MainGoodsOrderInfo.GoodsNumber" Width="120px" HeaderText="主材数量" />
                                        <f:BoundField DataField="GoodsInfo.EquipmentName" SortField="GoodsInfo.EquipmentName" Width="200px" HeaderText="辅材名称" />
                                        <f:TemplateField Width="80px" HeaderText="辅材单位">
                                            <ItemTemplate>
                                                <asp:Label ID="Label10" runat="server" Width="80px" Text='<%# GetUnitName(Eval("GoodsUnit").ToString()) %>'></asp:Label>
                                            </ItemTemplate>
                                        </f:TemplateField>
                                        <f:BoundField DataField="FormerlyGoodsNumber" Width="120px" HeaderText="应收数量" />
                                        <f:BoundField DataField="PayForNumber" Width="120px" HeaderText="赔偿数量" />
                                        <f:BoundField DataField="GoodsNumber" Width="120px" HeaderText="入库数量" />
                                        <f:BoundField DataField="GoodsUnitPrice" Width="100px" HeaderText="单价(元)" />
                                        <f:BoundField DataField="GoodsTotalPrice" Width="160px" HeaderText="赔偿金额(元)" />
                                    </Columns>
                                </f:Grid>
                            </Items>
                        </f:Tab>
                    </Tabs>
                </f:TabStrip>
            </Items>
        </f:Panel>
    </form>
</body>
</html>
