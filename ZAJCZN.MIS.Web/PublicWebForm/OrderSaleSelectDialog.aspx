<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OrderSaleSelectDialog.aspx.cs" Inherits="ZAJCZN.MIS.Web.OrderSaleSelectDialog" %>

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
            ShowHeader="false" Title="库存物品">
            <Items>
                <f:Form ID="Form2" runat="server" Height="36px" BodyPadding="5px" ShowHeader="false"
                    ShowBorder="false" LabelAlign="Right">
                    <Rows>
                        <f:FormRow ID="FormRow2" runat="server">
                            <Items>
                                <f:Label ID="lblTitle" runat="server"></f:Label>
                                <f:DropDownList ID="ddlWH" Label="库房" LabelAlign="Right" DataTextField="WHName"
                                    DataValueField="ID" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlWH_SelectedIndexChanged">
                                </f:DropDownList>
                                <f:DropDownList ID="ddlCostType" Label="物品分类" DataTextField="Name" DataValueField="Id" EnableSimulateTree="true"
                                    DataSimulateTreeLevelField="Level" OnSelectedIndexChanged="ddlCostType_SelectedIndexChanged" AutoPostBack="true"
                                    runat="server">
                                </f:DropDownList>
                                <f:TwinTriggerBox ID="ttbSearchMessage" runat="server" ShowLabel="false" EmptyText="在物品名称或物品编码中搜索"
                                    Trigger1Icon="Clear" Trigger2Icon="Search" ShowTrigger1="false" OnTrigger2Click="ttbSearchMessage_Trigger2Click"
                                    OnTrigger1Click="ttbSearchMessage_Trigger1Click">
                                </f:TwinTriggerBox>
                            </Items>
                        </f:FormRow>
                    </Rows>
                </f:Form>
                <f:Grid ID="Grid1" ShowBorder="true" ShowHeader="false" Title="库存物品列表" runat="server"
                    DataKeyNames="ID" EnableCheckBoxSelect="true" AllowPaging="false" PageSize="10" MarginTop="10px"
                    IsDatabasePaging="true" OnPageIndexChange="Grid1_PageIndexChange" Height="440px"
                    AllowSorting="true" OnSort="Grid1_Sort" SortField="ID" SortDirection="DESC">
                    <Toolbars>
                        <f:Toolbar ID="Toolbar1" runat="server" Position="Top">
                            <Items>

                                <f:Button ID="btnSaveClose" Text="保存选择的商品" runat="server" OnClick="btnSaveClose_Click"
                                    Icon="SystemSaveClose">
                                </f:Button>
                                <f:ToolbarFill ID="fill" runat="server"></f:ToolbarFill>
                                <f:Button ID="btnClose" Text="关闭返回" runat="server" Icon="SystemClose"
                                    OnClick="btnClose_Click">
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
                            <f:ListItem Text="10" Value="10" Selected="true" />
                            <f:ListItem Text="20" Value="20" />
                            <f:ListItem Text="50" Value="50" />
                            <f:ListItem Text="100" Value="100" />
                        </f:DropDownList>
                    </PageItems>
                    <Columns>
                        <f:TemplateField Width="140px" HeaderText="分类名称">
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Width="140px" Text='<%# GetType(Eval("GoodsTypeID").ToString()) %>'></asp:Label>
                            </ItemTemplate>
                        </f:TemplateField>
                        <f:BoundField DataField="GoodsCode" SortField="GoodsCode" Width="120px" HeaderText="物品编码" />
                        <f:BoundField DataField="GoodsName" SortField="GoodsName" Width="200px" HeaderText="物品名称" ExpandUnusedSpace="true" />
                        <f:BoundField DataField="GoodsInfo.Standard" SortField="GoodsInfo.Standard" Width="100px" HeaderText="规格" />
                        <f:TemplateField Width="60px" HeaderText="单位">
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Width="100px" Text='<%# GetUnitName(Eval("InventoryUnit").ToString()) %>'></asp:Label>
                            </ItemTemplate>
                        </f:TemplateField>
                        <f:BoundField DataField="GoodsInfo.DailyRents" Width="100px" HeaderText="日租金(元)" Hidden="true" />
                        <f:BoundField DataField="InventoryCount" SortField="InventoryCount" Width="100px" HeaderText="库存数" />

                    </Columns>
                </f:Grid>
            </Items>
        </f:Panel>
    </form>
</body>
</html>
