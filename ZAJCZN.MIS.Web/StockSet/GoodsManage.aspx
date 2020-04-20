<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GoodsManage.aspx.cs" Inherits="ZAJCZN.MIS.Web.GoodsManage" %>

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
            ShowHeader="false" Title="库存物品管理">
            <Items>
                <f:Form ID="Form2" runat="server" Height="36px" BodyPadding="5px" ShowHeader="false"
                    ShowBorder="false" LabelAlign="Right">
                    <Rows>
                        <f:FormRow ID="FormRow1" runat="server">
                            <Items>
                                <f:TwinTriggerBox ID="ttbSearchMessage" runat="server" ShowLabel="false" EmptyText="在物品名称、物品拼音或物品编码中搜索"
                                    Trigger1Icon="Clear" Trigger2Icon="Search" ShowTrigger1="false" OnTrigger2Click="ttbSearchMessage_Trigger2Click"
                                    OnTrigger1Click="ttbSearchMessage_Trigger1Click">
                                </f:TwinTriggerBox>
                                <f:DropDownList ID="ddlCostType" Label="物品分类" DataTextField="Name" DataValueField="Id" EnableSimulateTree="true"
                                    DataSimulateTreeLevelField="Level" OnSelectedIndexChanged="rblEnableStatus_SelectedIndexChanged" AutoPostBack="true"
                                    runat="server">
                                </f:DropDownList>
                                <f:DropDownList ID="ddlIsUsed" Label="是否启用" OnSelectedIndexChanged="rblEnableStatus_SelectedIndexChanged" AutoPostBack="true" runat="server">
                                    <f:ListItem Text="全部" Selected="true" Value="0" />
                                    <f:ListItem Text="停用" Value="1" />
                                    <f:ListItem Text="启用" Value="2" />
                                </f:DropDownList>
                            </Items>
                        </f:FormRow>
                    </Rows>
                </f:Form>
                <f:Grid ID="Grid1" runat="server" BoxFlex="1" ShowBorder="true" ShowHeader="false"
                    EnableCheckBoxSelect="true" PageSize="10"
                    DataKeyNames="ID,Name" AllowSorting="true" OnSort="Grid1_Sort" SortField="ID"
                    SortDirection="ASC" AllowPaging="true" IsDatabasePaging="true" OnPreDataBound="Grid1_PreDataBound"
                    OnRowCommand="Grid1_RowCommand" OnPageIndexChange="Grid1_PageIndexChange">
                    <Toolbars>
                        <f:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <f:Button ID="btnDeleteSelected" Icon="Delete" runat="server" Hidden="false" Text="删除选中记录" OnClick="btnDeleteSelected_Click">
                                </f:Button>
                                <f:ToolbarSeparator runat="server">
                                </f:ToolbarSeparator>
                                <f:Button ID="btnChangeEnableUsers" Icon="GroupEdit" EnablePostBack="false" runat="server"
                                    Text="设置启用状态">
                                    <Menu runat="server">
                                        <f:MenuButton ID="btnEnableUsers" OnClick="btnEnableUsers_Click" runat="server" Text="启用选中记录">
                                        </f:MenuButton>
                                        <f:MenuButton ID="btnDisableUsers" OnClick="btnDisableUsers_Click" runat="server" Text="停用选中记录">
                                        </f:MenuButton>
                                    </Menu>
                                </f:Button>
                                <f:ToolbarFill ID="ToolbarFill1" runat="server">
                                </f:ToolbarFill>
                                <f:Button ID="btnNew" runat="server" Icon="Add" EnablePostBack="false" Text="新增物品">
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
                        <f:BoundField DataField="GoodsCode" SortField="GoodsCode" Width="90px" HeaderText="物品编码" />
                        <f:BoundField DataField="GoodsBarCode" SortField="GoodsBarCode" Width="90px" HeaderText="物品条码" />
                        <f:BoundField DataField="GoodsName" SortField="GoodsName" Width="200px" HeaderText="物品名称" />
                        <f:BoundField DataField="GoodsPY" SortField="GoodsPY" Width="130px" HeaderText="拼音助记码" />
                        <f:BoundField DataField="GoodsFormat" SortField="GoodsFormat" Width="150px" HeaderText="规格" />
                        <f:TemplateField Width="60px" HeaderText="单位">
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Width="60px" Text='<%# GetUnitName(Eval("GoodsUnit").ToString()) %>'></asp:Label>
                            </ItemTemplate>
                        </f:TemplateField>
                        <f:BoundField DataField="GoodsPrice" SortField="GoodsPrice" Width="100px" HeaderText="单价" />
                        <f:TemplateField Width="100px" HeaderText="税率">
                            <ItemTemplate>
                                <asp:Label ID="Label4" runat="server" Width="100px" Text='<%# GetTaxName(Eval("TaxPoint").ToString()) %>'></asp:Label>
                            </ItemTemplate>
                        </f:TemplateField>
                        <f:TemplateField Width="100px" HeaderText="物品分类">
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Width="100px" Text='<%# GetType(Eval("GoodsTypeID").ToString()) %>'></asp:Label>
                            </ItemTemplate>
                        </f:TemplateField>
                        <f:TemplateField Width="80px" HeaderText="是否启用">
                            <ItemTemplate>
                                <asp:Label ID="Label3" runat="server" Width="80px" Text='<%# GetIsUsed(Eval("IsUsed").ToString()) %>'></asp:Label>
                            </ItemTemplate>
                        </f:TemplateField>
                        <f:WindowField ColumnID="editField" HeaderText="编辑" Hidden="false" TextAlign="Center" Icon="Pencil" ToolTip="编辑库存物品"
                            WindowID="Window1" Title="编辑库存物品" DataIFrameUrlFields="ID" DataIFrameUrlFormatString="~/StockSet/GoodsEdit.aspx?id={0}"
                            Width="80px" />
                        <f:WindowField ColumnID="viewField" HeaderText="查看" TextAlign="Center" Icon="Information" ToolTip="查看库存物品"
                            WindowID="Window1" Title="查看库存物品" DataIFrameUrlFields="ID" DataIFrameUrlFormatString="~/StockSet/GoodsEdit.aspx?id={0}&isview=1"
                            Width="80px" />
                        <f:LinkButtonField ColumnID="deleteField" HeaderText="删除" Hidden="false" TextAlign="Center" Icon="Delete" ToolTip="删除"
                            ConfirmText="确定删除此记录？" ConfirmTarget="Top" CommandName="Delete" Width="80px" />
                    </Columns>
                </f:Grid>
            </Items>
        </f:Panel>
        <f:Window ID="Window1" runat="server" IsModal="true" Hidden="true" Target="Top" EnableResize="true"
            EnableMaximize="true" EnableIFrame="true" IFrameUrl="about:blank" Width="800px"
            Height="460px" OnClose="Window1_Close">
        </f:Window>
    </form>
</body>
</html>
