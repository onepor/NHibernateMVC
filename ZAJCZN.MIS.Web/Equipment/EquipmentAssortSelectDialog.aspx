<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EquipmentAssortSelectDialog.aspx.cs" Inherits="ZAJCZN.MIS.Web.EquipmentAssortSelectDialog" %>

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
            ShowHeader="false" Title="库存物品">
            <Items>
                <f:Form ID="Form2" runat="server" Height="36px" BodyPadding="5px" ShowHeader="false"
                    ShowBorder="false" LabelAlign="Right">
                    <Rows>
                        <f:FormRow ID="FormRow2" runat="server">
                            <Items>
                                <f:Label ID="lblTitle" runat="server" ShowLabel="false"></f:Label>
                                <f:DropDownList ID="ddlCostType" Label="物品分类" DataTextField="TypeName" DataValueField="ID" EnableSimulateTree="true"
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
                <f:Grid ID="Grid1" ShowBorder="true" ShowHeader="false" Title="物品物品列表" runat="server"
                    DataKeyNames="ID" EnableCheckBoxSelect="true" AllowPaging="false" PageSize="10" 
                    IsDatabasePaging="true" Height="470px">
                    <Toolbars>
                        <f:Toolbar ID="Toolbar1" runat="server" Position="Top">
                            <Items>
                                <f:Button ID="btnClose" Text="关闭" runat="server" Icon="SystemClose"
                                    OnClick="btnClose_Click">
                                </f:Button>
                                <f:Button ID="btnSaveClose" Text="保存选择的物品" runat="server" OnClick="btnSaveClose_Click"
                                    Icon="SystemSaveClose">
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
                        <f:TemplateField Width="80px" HeaderText="物品分类">
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Width="80px" Text='<%# GetType(Eval("EquipmentTypeID").ToString()) %>'></asp:Label>
                            </ItemTemplate>
                        </f:TemplateField>
                        <f:BoundField DataField="EquipmentCode" SortField="EquipmentCode" Width="100px" HeaderText="物品编码" />
                        <f:BoundField DataField="EquipmentName" SortField="EquipmentName" Width="160px" HeaderText="物品名称" />
                        <f:BoundField DataField="Standard" SortField="Standard" Width="80px" HeaderText="规格" />
                        <f:TemplateField Width="80px" HeaderText="物品单位">
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Width="80px" Text='<%# GetUnitName(Eval("EquipmentUnit").ToString()) %>'></asp:Label>
                            </ItemTemplate>
                        </f:TemplateField>
                    </Columns>
                </f:Grid>
            </Items>
        </f:Panel>
    </form>
</body>
</html>
