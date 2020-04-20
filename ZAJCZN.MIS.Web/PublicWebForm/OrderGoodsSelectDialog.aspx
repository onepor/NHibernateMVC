<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OrderGoodsSelectDialog.aspx.cs" Inherits="ZAJCZN.MIS.Web.OrderGoodsSelectDialog" %>

<!DOCTYPE html>
<html>
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server" />
        <f:Panel ID="Panel1" runat="server" BodyPadding="5px"
            ShowBorder="false" Layout="VBox" BoxConfigAlign="Stretch" BoxConfigPosition="Start"
            ShowHeader="false" Title="五金配件">
            <Items>
                <f:Panel ID="Panel2" ShowHeader="false" ShowBorder="false" Layout="Column" runat="server">
                    <Items>
                        <f:Label ID="lblTitle" runat="server"></f:Label>
                        <f:TwinTriggerBox ID="ttbSearchMessage" runat="server" ShowLabel="false" EmptyText="在物品名称中搜索"
                            Trigger1Icon="Clear" Trigger2Icon="Search" ShowTrigger1="false" OnTrigger2Click="ttbSearchMessage_Trigger2Click"
                            OnTrigger1Click="ttbSearchMessage_Trigger1Click" Width="500px">
                        </f:TwinTriggerBox>
                        <f:DropDownList ID="ddlCostType" Label="配件分类" LabelWidth="90px" LabelAlign="Right"
                            DataTextField="TypeName" DataValueField="ID" EnableSimulateTree="true" Width="260px"
                            DataSimulateTreeLevelField="Level" OnSelectedIndexChanged="ddlCostType_SelectedIndexChanged" AutoPostBack="true"
                            runat="server">
                        </f:DropDownList>
                    </Items>
                </f:Panel>
                <f:Grid ID="Grid1" ShowBorder="true" ShowHeader="false" Title="库存物品列表" runat="server"
                    DataKeyNames="ID" EnableCheckBoxSelect="true" AllowPaging="false" PageSize="10"
                    IsDatabasePaging="true" OnPageIndexChange="Grid1_PageIndexChange">
                    <Toolbars>
                        <f:Toolbar ID="Toolbar1" runat="server" Position="Top">
                            <Items>
                                <f:Button ID="btnSaveClose" Text="保存选择的配件" runat="server" OnClick="btnSaveClose_Click"
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
                        <f:TemplateField Width="100px" HeaderText="配件分类">
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Width="120px" Text='<%# GetType(Eval("PartsTypeID").ToString()) %>'></asp:Label>
                            </ItemTemplate>
                        </f:TemplateField>
                        <f:BoundField DataField="PartsName" Width="200px" HeaderText="配件名称" />
                        <f:TemplateField Width="60px" HeaderText="单位">
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Width="100px" Text='<%# GetUnitName(Eval("PartsUnit").ToString()) %>'></asp:Label>
                            </ItemTemplate>
                        </f:TemplateField>
                        <f:BoundField DataField="UnitPrice" Width="80px" HeaderText="单价" />
                        <f:BoundField DataField="Remark" Width="80px" HeaderText="备注" ExpandUnusedSpace="true" />
                    </Columns>
                </f:Grid>
            </Items>
        </f:Panel>
    </form>
</body>
</html>
