<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WHOutBoundOrderView.aspx.cs" Inherits="ZAJCZN.MIS.Web.WHOutBoundOrderView" %>

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
            BoxConfigPosition="Start" ShowHeader="false" Title="出库单明细">
            <Items>
                <f:Panel ID="GroupPanel3" runat="server" ShowHeader="false">
                    <Items>
                        <f:Toolbar ID="Toolbar2" runat="server">
                            <Items>
                                <f:ToolbarFill ID="ToolbarFill1" runat="server">
                                </f:ToolbarFill>
                                <f:Button ID="btnReturn" OnClick="btnReturn_Click" runat="server" Icon="arrowleft" Text="返回出库单列表">
                                </f:Button>
                            </Items>
                        </f:Toolbar>
                    </Items>
                </f:Panel>
                <f:GroupPanel ID="GroupPanel1" runat="server" Title="出库单基础信息">
                    <Items>
                        <f:Form ID="Form2" ShowBorder="false" ShowHeader="false" runat="server">
                            <Items>
                                <f:FormRow>
                                    <Items>
                                        <f:Label ID="lblOrderNo" runat="server" Label="出库单号" LabelAlign="Right"></f:Label>
                                        <f:Label ID="lblStartDate" runat="server" LabelAlign="Right" Label="出库时间" Text="0"></f:Label>
                                        <f:DropDownList ID="ddlWH" Label="出库库房" LabelAlign="Right" DataTextField="WHName"
                                            DataValueField="ID" runat="server" Readonly="true">
                                        </f:DropDownList>
                                        <f:Label ID="Label2" runat="server" ShowLabel="false" LabelAlign="Right"></f:Label>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow>
                                    <Items>
                                        <f:Label ID="lblBussinessOrder" runat="server" Label="业务单号" LabelAlign="Right"></f:Label>
                                        <f:Label ID="lblTypeName" runat="server" Label="出库类型" LabelAlign="Right"></f:Label>
                                        <f:Label ID="lblCount" runat="server" LabelAlign="Right" Label="商品数量" Text="0"></f:Label>
                                        <f:Label ID="lblAmount" runat="server" LabelAlign="Right" Label="出库金额(元)" Text="0"></f:Label>
                                    </Items>
                                </f:FormRow>
                            </Items>
                        </f:Form>
                    </Items>
                </f:GroupPanel>
                <f:GroupPanel ID="GroupPanel2" runat="server" Title="出库商品信息">
                    <Items>
                        <f:Grid ID="Grid1" ShowBorder="true" ShowHeader="false" EnableCollapse="true"
                            runat="server" DataKeyNames="ID" AllowCellEditing="true" ClicksToEdit="1">
                            <Toolbars>
                                <f:Toolbar ID="Toolbar1" runat="server">
                                    <Items>
                                    </Items>
                                </f:Toolbar>
                            </Toolbars>
                            <Columns>
                                <f:TemplateField Width="100px" HeaderText="物品分类">
                                    <ItemTemplate>
                                        <asp:Label ID="Label1" runat="server" Width="100px" Text='<%# GetType(Eval("GoodsInfo.EquipmentTypeID").ToString()) %>'></asp:Label>
                                    </ItemTemplate>
                                </f:TemplateField>
                                <f:BoundField DataField="GoodsInfo.EquipmentName" SortField="GoodsInfo.EquipmentName" Width="200px" HeaderText="商品名称" ExpandUnusedSpace="true" />
                                <f:BoundField DataField="GoodsInfo.EquipmentCode" SortField="GoodsInfo.EquipmentCode" Width="80px" HeaderText="商品编号" />
                                <f:BoundField DataField="GoodsInfo.Standard" SortField="GoodsInfo.Standard" Width="150px" HeaderText="规格" />
                                <f:TemplateField Width="80px" HeaderText="单位">
                                    <ItemTemplate>
                                        <asp:Label ID="Label4" runat="server" Width="80px" Text='<%# GetUnitName(Eval("GoodsInfo.EquipmentUnit").ToString()) %>'></asp:Label>
                                    </ItemTemplate>
                                </f:TemplateField>
                                <f:BoundField DataField="GoodsNumber" SortField="GoodsNumber" Width="120px" HeaderText="出库数量" />
                                <f:RenderField Width="150px" ColumnID="GoodsUnitPrice" DataField="GoodsUnitPrice" FieldType="Double"
                                    HeaderText="商品单价(元)" Hidden="true">
                                    <Editor>
                                        <f:NumberBox ID="txtGoodsUnitPrice" NoDecimal="true" NoNegative="true" MinValue="1"
                                            runat="server">
                                        </f:NumberBox>
                                    </Editor>
                                </f:RenderField>
                                <f:BoundField DataField="GoodTotalPrice" SortField="GoodTotalPrice" Width="200px" HeaderText="商品金额(元)" Hidden="true" />
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
