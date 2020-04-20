<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ContractOrderEdit.aspx.cs" Inherits="ZAJCZN.MIS.Web.ContractOrderEdit" %>

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
            BoxConfigPosition="Start" ShowHeader="false" Title="查看发货单">
            <Items>
                <f:Panel ID="GroupPanel3" runat="server" ShowHeader="false">
                    <Items>
                        <f:Toolbar ID="Toolbar2" runat="server">
                            <Items>
                                <f:Button ID="btnReturn" OnClick="btnReturn_Click" runat="server" Icon="arrowleft"
                                    Text="返回发货单列表">
                                </f:Button>
                                <f:ToolbarFill ID="ToolbarFill1" runat="server">
                                </f:ToolbarFill>
                                <f:Button ID="btnSaveClose" ValidateForms="Form2" Icon="SystemSaveClose" OnClick="btnSaveClose_Click"
                                    runat="server" Text="保存发货单">
                                </f:Button>
                            </Items>
                        </f:Toolbar>
                    </Items>
                </f:Panel>
                <f:GroupPanel ID="GroupPanel1" runat="server" Title="发货单基础信息">
                    <Items>
                        <f:Form ID="Form2" ShowBorder="false" ShowHeader="false" runat="server">
                            <Items>
                                <f:FormRow>
                                    <Items>
                                        <f:TextBox ID="tbManualNO" LabelWidth="100px" LabelAlign="Right" runat="server" Label="发货单号"
                                            Required="true" ShowRedStar="true" EmptyText="自编发货单号格式：客户编号-顺序号，如0016-0125">
                                        </f:TextBox>
                                        <f:DatePicker ID="dpStartDate" Label="计价日期" runat="server" Required="true" ShowRedStar="true"
                                            LabelAlign="Right" LabelWidth="100px" DateFormatString="yyyy-MM-dd" EmptyText="请选择计价日期">
                                        </f:DatePicker>
                                        <f:Label ID="lblOrderNo" runat="server" Label="系统单号" LabelAlign="Right" LabelWidth="100px"></f:Label>
                                        <f:Label ID="lblDate" runat="server" Label="建单日期" LabelAlign="Right" LabelWidth="100px"></f:Label>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow>
                                    <Items>
                                        <f:DropDownList ID="ddlContract" LabelWidth="100px" LabelAlign="Right" Label="客户"
                                            DataTextField="ContarctName" DataValueField="ID" Required="true" ShowRedStar="true"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddlContract_SelectedIndexChanged"
                                            runat="server">
                                        </f:DropDownList>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow>
                                    <Items>
                                        <f:DropDownList ID="ddlCar" LabelWidth="100px" LabelAlign="Right" Label="货车"
                                            DataTextField="CarName" DataValueField="ID" Required="true" ShowRedStar="true"
                                            runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCar_SelectedIndexChanged">
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
                <f:TabStrip ID="tsDetail" CssClass="blockpanel" ShowBorder="true" TabPosition="Top" Height="600px"
                    EnableTabCloseMenu="false" ActiveTabIndex="0" runat="server" OnTabIndexChanged="TabStrip1_TabIndexChanged"
                    AutoPostBack="true">
                    <Tabs>
                        <f:Tab ID="tabGoodsDetail" Title="发货明细" BodyPadding="10px" Layout="Fit" runat="server">
                            <Items>
                                <f:Grid ID="Grid1" ShowBorder="true" ShowHeader="false" EnableCollapse="true"
                                    runat="server" DataKeyNames="ID" ClicksToEdit="1" AllowPaging="false" EnableCheckBoxSelect="false"
                                    EnableAfterEditEvent="true" OnAfterEdit="Grid1_AfterEdit" AllowCellEditing="true">
                                    <Toolbars>
                                        <f:Toolbar ID="Toolbar1" runat="server">
                                            <Items>
                                                <f:Label ID="lblNotice1" runat="server" ShowLabel="false" Text="点击调整数量直接填写修改要调整的材料数量" CssStyle="color:red;"></f:Label>
                                                <f:ToolbarFill ID="ToolbarFill2" runat="server"></f:ToolbarFill>
                                            </Items>
                                        </f:Toolbar>
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
                                        <f:BoundField DataField="FormerlyGoodsNumber" SortField="FormerlyGoodsNumber" Width="100px" HeaderText="发货数量" />
                                        <f:RenderField Width="100px" ColumnID="FixGoodsNumber" DataField="FixGoodsNumber" FieldType="Double"
                                            HeaderText="调整数量">
                                            <Editor>
                                                <f:NumberBox ID="tbxUsingCount" NoDecimal="false" NoNegative="false" DecimalPrecision="3" runat="server">
                                                </f:NumberBox>
                                            </Editor>
                                        </f:RenderField>
                                        <f:BoundField DataField="GoodsNumber" SortField="GoodsNumber" Width="100px" HeaderText="实际发货" />
                                        <f:TemplateField Width="90px" HeaderText="计价方式">
                                            <ItemTemplate>
                                                <asp:Label ID="Label7" runat="server" Width="90px" Text='<%# GetPayUnit(Eval("PayUnit").ToString()) %>'></asp:Label>
                                            </ItemTemplate>
                                        </f:TemplateField>
                                        <f:TemplateField Width="80px" HeaderText="计价单位">
                                            <ItemTemplate>
                                                <asp:Label ID="Label6" runat="server" Width="80px" Text='<%# GetUnitName(Eval("GoodsCalcUnit").ToString()) %>'></asp:Label>
                                            </ItemTemplate>
                                        </f:TemplateField>
                                        <f:TemplateField Width="100px" HeaderText="费用支付">
                                            <ItemTemplate>
                                                <asp:Label ID="Label12" runat="server" Width="100px" Text='<%# GetNowPay(Eval("GoodsInfo.IsPayNow").ToString()) %>'></asp:Label>
                                            </ItemTemplate>
                                        </f:TemplateField>
                                        <f:BoundField DataField="GoodCalcPriceNumber" Width="100px" HeaderText="计价数量" />
                                        <f:BoundField DataField="GoodsUnitPrice" Width="180px" HeaderText="日租金/单价(元)" />
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
                                        <f:BoundField DataField="GoodsNumber" Width="120px" HeaderText="辅材数量" />
                                        <f:BoundField DataField="GoodsUnitPrice" Width="100px" HeaderText="单价(元)" />
                                        <f:BoundField DataField="GoodsTotalPrice" Width="160px" HeaderText="总金额(元)" />
                                        <f:TemplateField Width="80px" HeaderText="计算费用">
                                            <ItemTemplate>
                                                <asp:Label ID="Label5" runat="server" Width="80px" Text='<%#  Eval("IsCalcPrice").ToString().Equals ("1")?"是":"否" %>'></asp:Label>
                                            </ItemTemplate>
                                        </f:TemplateField>
                                    </Columns>
                                </f:Grid>
                            </Items>
                        </f:Tab>
                        <f:Tab ID="tabCustomerCost" Title="费用清单" BodyPadding="10px" Layout="Fit" runat="server">
                            <Items>
                                <f:Panel ID="Panel5" runat="server" ShowBorder="false" EnableCollapse="false" Layout="HBox"
                                    ShowHeader="false">
                                    <Items>
                                        <f:Panel ID="Panel2" Title="货物重量统计" BoxFlex="2" runat="server" MarginRight="5px"
                                            BodyPadding="6px" ShowBorder="true" ShowHeader="true" Width="300px">
                                            <Items>
                                                <f:Grid ID="gdWeightInfo" ShowBorder="true" ShowHeader="false" EnableCollapse="true"
                                                    runat="server" DataKeyNames="ID" AllowCellEditing="true" ClicksToEdit="1" AllowPaging="false">
                                                    <Toolbars>
                                                        <f:Toolbar ID="Toolbar4" runat="server">
                                                            <Items>
                                                                <f:Label ID="lblTotalWeight" runat="server" ShowLabel="false" Text=""></f:Label>
                                                                <f:ToolbarFill ID="ToolbarFill4" runat="server"></f:ToolbarFill>
                                                            </Items>
                                                        </f:Toolbar>
                                                    </Toolbars>
                                                    <Columns>
                                                        <f:TemplateField Width="100px" HeaderText="物品分类">
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label15" runat="server" Width="100px" Text='<%# GetType(Eval("GoodTypeID").ToString()) %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </f:TemplateField>
                                                        <f:TemplateField Width="60px" HeaderText="单位">
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label14" runat="server" Width="60px" Text='<%# GetUnitName(Eval("GoodsCalcUnit").ToString()) %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </f:TemplateField>
                                                        <f:BoundField DataField="GoodCalcPriceNumber" Width="100px" HeaderText="数量" />
                                                        <f:BoundField DataField="GoodsCustomerWeight" Width="80px" HeaderText="客户(吨)" />
                                                        <f:BoundField DataField="GoodsStaffWeight" Width="80px" HeaderText="员工(吨)" />
                                                        <f:BoundField DataField="GoodsDriverWeight" Width="80px" HeaderText="司机(吨)" />
                                                    </Columns>
                                                </f:Grid>
                                            </Items>
                                        </f:Panel>
                                        <f:Panel ID="Panel3" Title="费用清单" BoxFlex="3" runat="server" BodyPadding="10px"
                                            ShowBorder="true" ShowHeader="true">
                                            <Items>
                                                <f:Grid ID="gdCostInfo" ShowBorder="true" ShowHeader="false" EnableCollapse="true"
                                                    runat="server" DataKeyNames="ID" AllowCellEditing="true" ClicksToEdit="1" AllowPaging="false"
                                                    EnableCheckBoxSelect="false" EnableAfterEditEvent="true" OnAfterEdit="gdCostInfo_AfterEdit">
                                                    <Toolbars>
                                                        <f:Toolbar ID="Toolbar3" runat="server">
                                                            <Items>
                                                                <f:DropDownList ID="ddlSHCostType" Label="费用类型"
                                                                    AutoPostBack="true" OnSelectedIndexChanged="ddlSHCostType_SelectedIndexChanged"
                                                                    runat="server">
                                                                    <f:ListItem Text="全部类型" Value="0" Selected="true" />
                                                                    <f:ListItem Text="员工费用" Value="1" />
                                                                    <f:ListItem Text="客户费用" Value="2" />
                                                                    <f:ListItem Text="司机费用" Value="3" />

                                                                </f:DropDownList>
                                                                <f:ToolbarFill ID="ToolbarFill3" runat="server"></f:ToolbarFill>
                                                            </Items>
                                                        </f:Toolbar>
                                                    </Toolbars>
                                                    <Columns>
                                                        <f:TemplateField Width="80px" HeaderText="费用类型">
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label3" runat="server" Width="80px" Text='<%# GetCostType(Eval("CostType").ToString()) %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </f:TemplateField>
                                                        <f:BoundField DataField="CostName" Width="120px" HeaderText="费用名称" />
                                                        <f:TemplateField Width="80px" HeaderText="单位">
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label11" runat="server" Width="80px" Text='<%# GetFYUnitName(Eval("PayUnit").ToString()) %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </f:TemplateField>
                                                        <f:RenderField Width="80px" ColumnID="OrderNumber" DataField="OrderNumber" FieldType="Double"
                                                            HeaderText="数量">
                                                            <Editor>
                                                                <f:NumberBox ID="NumberBox1" NoDecimal="false" NoNegative="true" MinValue="0" DecimalPrecision="3"
                                                                    MaxValue="5000" runat="server">
                                                                </f:NumberBox>
                                                            </Editor>
                                                        </f:RenderField>
                                                        <f:RenderField Width="100px" ColumnID="PayPrice" DataField="PayPrice" FieldType="Double"
                                                            HeaderText="单价(元)">
                                                            <Editor>
                                                                <f:NumberBox ID="NumberBox2" NoDecimal="false" NoNegative="true" MinValue="0" DecimalPrecision="3"
                                                                    MaxValue="5000" runat="server">
                                                                </f:NumberBox>
                                                            </Editor>
                                                        </f:RenderField>
                                                        <f:BoundField DataField="CostAmount" Width="100px" HeaderText="总金额(元)" />
                                                        <f:RenderField Width="300px" ColumnID="Remark" DataField="Remark" FieldType="String"
                                                            HeaderText="备注">
                                                            <Editor>
                                                                <f:TextBox ID="tbRemark" runat="server">
                                                                </f:TextBox>
                                                            </Editor>
                                                        </f:RenderField>
                                                    </Columns>
                                                </f:Grid>
                                            </Items>
                                        </f:Panel>
                                    </Items>
                                </f:Panel>
                            </Items>
                        </f:Tab>
                    </Tabs>
                </f:TabStrip>
            </Items>
        </f:Panel>
    </form>
</body>
</html>
