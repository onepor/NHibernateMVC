﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ContractFHEdit.aspx.cs" Inherits="ZAJCZN.MIS.Web.ContractFHEdit" %>

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
            BoxConfigPosition="Start" ShowHeader="false" Title="新增合同发货单">
            <Items>
                <f:Panel ID="GroupPanel3" runat="server" ShowHeader="false">
                    <Items>
                        <f:Toolbar ID="Toolbar2" runat="server">
                            <Items>
                                <f:Button ID="btnReturn" OnClick="btnReturn_Click" runat="server" Icon="PageBack" Text="返回发货单列表">
                                </f:Button>
                                <f:Button ID="btnCancel" OnClick="btnCancel_Click" runat="server" Icon="SystemClose" Text="取消发货单">
                                </f:Button>
                                <f:ToolbarFill ID="ToolbarFill1" runat="server">
                                </f:ToolbarFill>
                                <f:Button ID="btnSaveTemp" ValidateForms="Form2" Icon="SystemSave" OnClick="btnSaveTemp_Click"
                                    runat="server" Text="保存临时订单">
                                </f:Button>
                                <f:ToolbarSeparator ID="ToolbarSeparator2" runat="server">
                                </f:ToolbarSeparator>
                                <f:Button ID="btnSaveClose" ValidateForms="Form2" Icon="Accept" OnClick="btnSaveClose_Click"
                                    runat="server" Text="确认提交订单">
                                </f:Button>
                            </Items>
                        </f:Toolbar>
                    </Items>
                </f:Panel>
                <f:GroupPanel ID="GroupPanel1" runat="server" Title="发货单基础信息">
                    <Items>
                        <f:Form ID="Form2" ShowBorder="false" ShowHeader="false" runat="server">
                            <Items>
                                <f:FormRow Hidden="true">
                                    <Items>
                                        <f:Label ID="lblOrderNo" runat="server" Label="系统单号" LabelAlign="Right" LabelWidth="100px"></f:Label>
                                        <f:Label ID="lblAmount" runat="server" LabelAlign="Right" Label="总金额(元)" Text="0" LabelWidth="100px" Hidden="true"></f:Label>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow>
                                    <Items>
                                        <f:DropDownList ID="ddlContract" LabelWidth="100px" LabelAlign="Right" Label="客户" Width="300px"
                                            DataTextField="ContarctName" DataValueField="ID" Required="true" ShowRedStar="true"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddlContract_SelectedIndexChanged"
                                            runat="server">
                                        </f:DropDownList>
                                        <f:DropDownList ID="ddlCar" LabelWidth="100px" LabelAlign="Right" Label="货车"
                                            DataTextField="CarName" DataValueField="ID" Required="true" ShowRedStar="true"
                                            runat="server">
                                        </f:DropDownList>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow>
                                    <Items>
                                        <f:TextBox ID="tbManualNO" LabelWidth="100px" LabelAlign="Right" runat="server" Label="发货单号"
                                            Required="true" ShowRedStar="true" EmptyText="自编发货单号格式：客户编号-顺序号，如0016-0125">
                                        </f:TextBox>
                                        <f:DatePicker ID="dpStartDate" Label="发货日期" runat="server" Required="true" ShowRedStar="true"
                                            LabelAlign="Right" LabelWidth="100px" DateFormatString="yyyy-MM-dd" EmptyText="请选择发货日期">
                                        </f:DatePicker>

                                    </Items>
                                </f:FormRow>
                                <f:FormRow>
                                    <Items>
                                        <f:TextBox ID="txtRemark" LabelAlign="Right" runat="server" Label="订单备注" LabelWidth="100px"></f:TextBox>
                                    </Items>
                                </f:FormRow>
                            </Items>
                        </f:Form>
                    </Items>
                </f:GroupPanel>
                <f:TabStrip ID="tsDetail" CssClass="blockpanel" ShowBorder="true" TabPosition="Top" Height="360px"
                    EnableTabCloseMenu="false" ActiveTabIndex="0" runat="server">
                    <Tabs>
                        <f:Tab ID="tabGoodsDetail" Title="发货明细" BodyPadding="10px" Layout="Fit" runat="server">
                            <Items>
                                <f:Grid ID="Grid1" ShowBorder="true" ShowHeader="false" EnableCollapse="true"
                                    runat="server" DataKeyNames="ID" AllowCellEditing="true" ClicksToEdit="1" AllowPaging="false"
                                    OnPreDataBound="Grid1_PreDataBound" EnableCheckBoxSelect="true" OnRowCommand="Grid1_RowCommand"
                                    EnableAfterEditEvent="true" OnAfterEdit="Grid1_AfterEdit">
                                    <Toolbars>
                                        <f:Toolbar ID="Toolbar1" runat="server">
                                            <Items>
                                                <f:Button ID="btnNew" Text="选择发货物品" Icon="Add" EnablePostBack="false" runat="server">
                                                </f:Button>
                                                <f:Button ID="btnDeleteSelected" Text="删除选中行" Icon="Delete"
                                                    runat="server" OnClick="btnDeleteSelected_Click">
                                                </f:Button>
                                                <f:Label ID="lblNotice1" runat="server" ShowLabel="false" Text="点击发货数量直接填写修改实际出库数量" CssStyle="color:red;"></f:Label>
                                                <f:ToolbarFill ID="ToolbarFill2" runat="server"></f:ToolbarFill>
                                            </Items>
                                        </f:Toolbar>
                                    </Toolbars>
                                    <Columns>
                                        <f:LinkButtonField ColumnID="deleteField" HeaderText="删除" TextAlign="Center" Icon="Delete" ToolTip="删除"
                                            ConfirmText="确定删除此物品记录？" ConfirmTarget="Top" CommandName="Delete" Width="60px" />
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
                                        <f:BoundField DataField="GoodsInfo.EquipmentCode" SortField="GoodsInfo.EquipmentCode" Width="120px" HeaderText="商品编号" />
                                        <f:BoundField DataField="GoodsInfo.Standard" SortField="GoodsInfo.Standard" Width="100px" HeaderText="规格" />
                                        <f:TemplateField Width="80px" HeaderText="单位">
                                            <ItemTemplate>
                                                <asp:Label ID="Label4" runat="server" Width="80px" Text='<%# GetUnitName(Eval("GoodsInfo.EquipmentUnit").ToString()) %>'></asp:Label>
                                            </ItemTemplate>
                                        </f:TemplateField>
                                        <f:RenderField Width="100px" ColumnID="GoodsNumber" DataField="GoodsNumber" FieldType="Double"
                                            HeaderText="发货数量">
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
                                                <asp:Label ID="Label6" runat="server" Width="80px" Text='<%# GetUnitName(Eval("GoodsCalcUnit").ToString()) %>'></asp:Label>
                                            </ItemTemplate>
                                        </f:TemplateField>
                                        <f:BoundField DataField="GoodCalcPriceNumber" Width="100px" HeaderText="计价数量" />
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
                                    </Columns>
                                </f:Grid>
                            </Items>
                        </f:Tab>
                    </Tabs>
                </f:TabStrip>
            </Items>
        </f:Panel>
        <f:Window ID="Window1" runat="server" IsModal="true" Hidden="true" Target="Top" EnableResize="true"
            EnableMaximize="true" EnableIFrame="true" IFrameUrl="about:blank" Width="1000px"
            Height="530px" OnClose="Window1_Close">
        </f:Window>
    </form>
</body>
</html>
