<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GoodsTypeManage.aspx.cs" Inherits="ZAJCZN.MIS.Web.GoodsTypeManage" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title></title>

</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" AutoSizePanelID="RegionPanel1" runat="server"></f:PageManager>
        <f:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
            <Regions>
                <f:Region ID="Region1" ShowBorder="false" ShowHeader="false" BodyPadding="5px" Width="200px" Position="Left" Layout="Fit" runat="server">
                    <Items>
                        <f:Grid ID="Grid1" runat="server" ShowBorder="true" ShowHeader="false" EnableCheckBoxSelect="false"
                            DataKeyNames="ID" AllowSorting="false" SortField="TypeName" SortDirection="DESC" 
                            AllowPaging="false" EnableMultiSelect="false" OnRowClick="Grid1_RowClick" EnableRowClickEvent="true">
                            <Columns> 
                                <f:BoundField DataField="TypeName" HeaderText="物品大类名称" Width="200px"></f:BoundField>
                            </Columns>
                        </f:Grid>
                    </Items>
                </f:Region>
                <f:Region ID="Region2" ShowBorder="false" ShowHeader="false" Position="Center" Layout="VBox" BoxConfigAlign="Stretch" BoxConfigPosition="Left" BodyPadding="5px 5px 5px 0" runat="server">
                    <Items>
                        <f:Form ID="Form3" runat="server" Height="36px" BodyPadding="5px" ShowHeader="false" ShowBorder="false" LabelAlign="Right">
                            <Rows>
                                <f:FormRow ID="FormRow2" runat="server">
                                    <Items>
                                        <f:TwinTriggerBox ID="ttbSearchUser" runat="server" ShowLabel="false" EmptyText="在物品类别名称中搜索" Trigger1Icon="Clear" Trigger2Icon="Search" ShowTrigger1="false" OnTrigger2Click="ttbSearchUser_Trigger2Click" OnTrigger1Click="ttbSearchUser_Trigger1Click">
                                        </f:TwinTriggerBox>
                                        <f:Label runat="server">
                                        </f:Label>
                                    </Items>
                                </f:FormRow>
                            </Rows>
                        </f:Form>
                        <f:Grid ID="Grid2" runat="server" BoxFlex="1" ShowBorder="true" ShowHeader="false" EnableCheckBoxSelect="true"
                            DataKeyNames="ID,TypeName" AllowSorting="true" OnSort="Grid2_Sort" SortField="TypeName"
                            SortDirection="DESC" AllowPaging="false" IsDatabasePaging="false"
                            OnPreDataBound="Grid2_PreDataBound" OnRowCommand="Grid2_RowCommand" OnPageIndexChange="Grid2_PageIndexChange">
                            <Toolbars>
                                <f:Toolbar ID="Toolbar1" runat="server">
                                    <Items>
                                        <f:Button ID="btnDeleteSelected" Icon="Delete" Hidden="true" runat="server" Text="移除选中类别" OnClick="btnDeleteSelected_Click">
                                        </f:Button>
                                        <f:Button ID="btnChangeEnableUsers" Icon="GroupEdit" EnablePostBack="false" runat="server"
                                            Text="设置启用状态">
                                            <Menu ID="Menu1" runat="server">
                                                <f:MenuButton ID="btnEnableUsers" OnClick="btnEnableUsers_Click" runat="server" Text="启用选中记录">
                                                </f:MenuButton>
                                                <f:MenuButton ID="btnDisableUsers" OnClick="btnDisableUsers_Click" runat="server" Text="停用选中记录">
                                                </f:MenuButton>
                                            </Menu>
                                        </f:Button>
                                        <f:Button ID="btnChangeEnableCalc" Icon="GroupEdit" EnablePostBack="false" runat="server"
                                            Text="设置财务核算">
                                            <Menu ID="Menu2" runat="server">
                                                <f:MenuButton ID="mbtnEnableCalc" OnClick="btnEnableCalc_Click" runat="server" Text="启用选中记录参与财务核算">
                                                </f:MenuButton>
                                                <f:MenuButton ID="mbtnDisableCalc" OnClick="btnDisableCalc_Click" runat="server" Text="停用选中记录参与财务核算">
                                                </f:MenuButton>
                                            </Menu>
                                        </f:Button>
                                        <f:ToolbarFill ID="ToolbarFill1" runat="server">
                                        </f:ToolbarFill>
                                        <f:Button ID="btnNew" runat="server" Icon="Add" EnablePostBack="true" OnClick="btnNew_Click" Text="新增物品类别">
                                        </f:Button>
                                    </Items>
                                </f:Toolbar>
                            </Toolbars>
                            <PageItems>
                                <f:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                                </f:ToolbarSeparator>
                                <f:ToolbarText ID="ToolbarText1" runat="server" Text="每页记录数：">
                                </f:ToolbarText>
                                <f:DropDownList ID="ddlGridPageSize" Width="80px" AutoPostBack="true" OnSelectedIndexChanged="ddlGridPageSize_SelectedIndexChanged" runat="server">
                                    <f:ListItem Text="10" Value="10"></f:ListItem>
                                    <f:ListItem Text="20" Value="20"></f:ListItem>
                                    <f:ListItem Text="50" Value="50"></f:ListItem>
                                    <f:ListItem Text="100" Value="100"></f:ListItem>
                                </f:DropDownList>
                            </PageItems>
                            <Columns>
                                <f:RowNumberField></f:RowNumberField>
                                <f:BoundField DataField="Typecode" SortField="Typecode" Width="100px" HeaderText="类别编码"></f:BoundField>
                                <f:BoundField DataField="TypeName" SortField="TypeName" Width="200px" HeaderText="类别名称"></f:BoundField>
                                <f:TemplateField Width="100px" HeaderText="上级类别">
                                    <ItemTemplate>
                                        <asp:Label ID="Label2" runat="server" Width="120px" Text='<%# GetTypeName(Eval("ParentID").ToString()) %>'></asp:Label>
                                    </ItemTemplate>
                                </f:TemplateField>
                                <f:TemplateField Width="100px" HeaderText="是否启用">
                                    <ItemTemplate>
                                        <asp:Label ID="Label3" runat="server" Width="120px" Text='<%# GetIsUsed(Eval("IsUsed").ToString()) %>'></asp:Label>
                                    </ItemTemplate>
                                </f:TemplateField>
                                <f:TemplateField Width="120px" HeaderText="参与成本计算">
                                    <ItemTemplate>
                                        <asp:Label ID="Label1" runat="server" Width="120px" Text='<%# GetIsCalc(Eval("IsCalc").ToString()) %>'></asp:Label>
                                    </ItemTemplate>
                                </f:TemplateField>
                                <f:BoundField DataField="TypeName" SortField="Remark" Width="200px" HeaderText="备注" ExpandUnusedSpace="true"></f:BoundField>
                                <f:WindowField ColumnID="editField" HeaderText="编辑" TextAlign="Center" Icon="Pencil" ToolTip="编辑物品类型"
                                    WindowID="Window1" Title="编辑物品类型" DataIFrameUrlFields="ID" DataIFrameUrlFormatString="~/StockSet/GoodsTypeEdit.aspx?id={0}"
                                    Width="80px" />
                                <f:LinkButtonField ColumnID="deleteField" TextAlign="Center" Icon="Delete" ToolTip="从当前部类别中移除此类别户" Hidden="true"
                                    ConfirmText="确定移除此类别？" ConfirmTarget="Top" CommandName="Delete" Width="50px">
                                </f:LinkButtonField>
                            </Columns>
                        </f:Grid>
                    </Items>
                </f:Region>
            </Regions>
        </f:RegionPanel>
        <f:Window ID="Window1" CloseAction="Hide" runat="server" IsModal="true"
            Hidden="true" Target="Top" EnableResize="true" EnableMaximize="true"
            EnableIFrame="true" IFrameUrl="about:blank" Width="600px" Height="400px" OnClose="Window1_Close">
        </f:Window>
    </form>
</body>
</html>
