﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReceiveOrderEdit.aspx.cs" Inherits="ZAJCZN.MIS.Web.ReceiveOrderEdit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server" />
        <f:Panel ID="Panel1" runat="server" BodyPadding="5px" ShowBorder="false" Layout="VBox" BoxConfigAlign="Stretch"
            BoxConfigPosition="Start" ShowHeader="false" Title="新增领用单">
            <Items>
                <f:Panel ID="GroupPanel3" runat="server" ShowHeader="false">
                    <Items>
                        <f:Toolbar ID="Toolbar2" runat="server">
                            <Items>
                                <f:Button ID="btnReturn" OnClick="btnReturn_Click" runat="server" Icon="Delete" Text="取消领用单">
                                </f:Button>
                                <f:ToolbarFill ID="ToolbarFill1" runat="server">
                                </f:ToolbarFill>
                                <f:Button ID="btnSaveTemp" ValidateForms="SimpleForm1" Icon="SystemSaveNew" OnClick="btnSaveTemp_Click"
                                    runat="server" Text="保存临时订单">
                                </f:Button>
                                <f:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                                </f:ToolbarSeparator>
                                <f:Button ID="btnSaveClose" ValidateForms="SimpleForm1" Icon="SystemSaveClose" OnClick="btnSaveClose_Click"
                                    runat="server" Text="确认提交订单">
                                </f:Button>
                            </Items>
                        </f:Toolbar>
                    </Items>
                </f:Panel>
                <f:GroupPanel ID="GroupPanel1" runat="server" Title="领用单基础信息">
                    <Items>
                        <f:Form ID="Form2" ShowBorder="false" ShowHeader="false" runat="server">
                            <Items>
                                <f:FormRow>
                                    <Items>
                                        <f:Label ID="lblOrderNo" runat="server" Label="领用单号" LabelAlign="Right"></f:Label>
                                        <f:DatePicker ID="dpStartDate" Label="领用日期" runat="server" Required="true" ShowRedStar="true"
                                            LabelAlign="Right">
                                        </f:DatePicker>
                                        <f:Label ID="lblCount" runat="server" LabelAlign="Right" Label="商品数量" Text="0"></f:Label>
                                        <f:Label ID="lblAmount" runat="server" LabelAlign="Right" Label="出库金额(元)" Text="0"></f:Label>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow>
                                    <Items>
                                        <f:DropDownList ID="ddlSuplier" Label="领用人" LabelAlign="Right" DataTextField="EmployeeName"
                                            DataValueField="ID" runat="server" Required="true" ShowRedStar="true">
                                        </f:DropDownList>
                                        <f:DropDownList ID="ddlWH" Label="出库库房" LabelAlign="Right" DataTextField="WHName"
                                            DataValueField="ID" runat="server" Required="true" ShowRedStar="true" AutoPostBack="true"
                                            OnSelectedIndexChanged="ddlWH_SelectedIndexChanged">
                                        </f:DropDownList>
                                        <f:TextBox ID="txtRemark" LabelAlign="Right" runat="server" Label="订单备注"></f:TextBox>
                                    </Items>
                                </f:FormRow>
                            </Items>
                        </f:Form>
                    </Items>
                </f:GroupPanel>
                <f:GroupPanel ID="GroupPanel2" runat="server" Title="领用商品信息">
                    <Items>
                        <f:Grid ID="Grid1" ShowBorder="true" ShowHeader="false" EnableCollapse="true"
                            runat="server" DataKeyNames="ID" AllowCellEditing="true" ClicksToEdit="1"
                            OnPreDataBound="Grid1_PreDataBound" EnableCheckBoxSelect="true" OnRowCommand="Grid1_RowCommand"
                            EnableAfterEditEvent="true" OnAfterEdit="Grid1_AfterEdit">
                            <Toolbars>
                                <f:Toolbar ID="Toolbar1" runat="server">
                                    <Items>
                                        <f:Button ID="btnNew" Text="选择领用商品" Icon="Add" EnablePostBack="false" runat="server">
                                        </f:Button>
                                        <f:Button ID="btnDeleteSelected" Text="删除选中行" Icon="Delete"
                                            runat="server" OnClick="btnDeleteSelected_Click">
                                        </f:Button>
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
                                <f:RenderField Width="120px" ColumnID="GoodsNumber" DataField="GoodsNumber" FieldType="Double"
                                    HeaderText="领用数量">
                                    <Editor>
                                        <f:NumberBox ID="tbxUsingCount" NoDecimal="true" NoNegative="true" MinValue="1"
                                            runat="server">
                                        </f:NumberBox>
                                    </Editor>
                                </f:RenderField>
                                <f:RenderField Width="150px" ColumnID="GoodsUnitPrice" DataField="GoodsUnitPrice" FieldType="Double"
                                    HeaderText="商品单价(元)">
                                    <Editor>
                                        <f:NumberBox ID="txtGoodsUnitPrice" NoDecimal="false" NoNegative="true" MinValue="1"
                                            runat="server">
                                        </f:NumberBox>
                                    </Editor>
                                </f:RenderField>
                                <f:BoundField DataField="GoodTotalPrice" SortField="GoodTotalPrice" Width="200px" HeaderText="商品金额(元)" />
                                <f:LinkButtonField ColumnID="deleteField" HeaderText="删除" TextAlign="Center" Icon="Delete" ToolTip="删除"
                                    ConfirmText="确定删除此商品记录？" ConfirmTarget="Top" CommandName="Delete" Width="80px" />
                            </Columns>
                        </f:Grid>
                        <f:Label ID="labResult" EncodeText="false" runat="server">
                        </f:Label>
                    </Items>
                </f:GroupPanel>
            </Items>
        </f:Panel>
        <f:Window ID="Window1" runat="server" IsModal="true" Hidden="true" Target="Top" EnableResize="true"
            EnableMaximize="true" EnableIFrame="true" IFrameUrl="about:blank" Width="1000px"
            Height="530px" OnClose="Window1_Close">
        </f:Window>
    </form>
</body>
</html>
