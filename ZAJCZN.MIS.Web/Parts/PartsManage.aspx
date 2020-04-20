<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PartsManage.aspx.cs" Inherits="ZAJCZN.MIS.Web.PartsManage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" AutoSizePanelID="RegionPanel1" runat="server" />
        <f:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
            <Regions>
                <f:Region ID="Region1" ShowBorder="false" ShowHeader="false" BodyPadding="5px" Width="160px" Position="Left" Layout="Fit" runat="server">
                    <Items>
                        <f:Grid ID="Grid2" runat="server" ShowBorder="true" ShowHeader="false" EnableCheckBoxSelect="false"
                            DataKeyNames="ID" AllowSorting="false" SortField="TypeName" SortDirection="DESC"
                            AllowPaging="false" EnableMultiSelect="false" OnRowClick="Grid2_RowClick" EnableRowClickEvent="true">
                            <Columns>
                                <f:BoundField DataField="TypeName" HeaderText="配件分类" Width="156px"></f:BoundField>
                            </Columns>
                        </f:Grid>
                    </Items>
                </f:Region>
                <f:Region ID="Region2" ShowBorder="false" ShowHeader="false" Position="Center" Layout="VBox" BoxConfigAlign="Stretch" BoxConfigPosition="Left" BodyPadding="5px 5px 5px 0" runat="server">
                    <Items>
                        <f:Form ID="Form2" runat="server" Height="36px" BodyPadding="5px" ShowHeader="false"
                            ShowBorder="false" LabelAlign="Right">
                            <Rows>
                                <f:FormRow ID="FormRow1" runat="server">
                                    <Items>
                                        <f:TwinTriggerBox ID="ttbSearchMessage" runat="server" ShowLabel="false" EmptyText="在配件名称中搜索"
                                            Trigger1Icon="Clear" Trigger2Icon="Search" ShowTrigger1="false" OnTrigger2Click="ttbSearchMessage_Trigger2Click"
                                            OnTrigger1Click="ttbSearchMessage_Trigger1Click">
                                        </f:TwinTriggerBox>
                                        <f:DropDownList ID="ddlIsUsed" Label="是否启用" OnSelectedIndexChanged="rblEnableStatus_SelectedIndexChanged" AutoPostBack="true" runat="server">
                                            <f:ListItem Text="全部" Selected="true" Value="0" />
                                            <f:ListItem Text="启用" Value="1" />
                                            <f:ListItem Text="停用" Value="0" />
                                        </f:DropDownList>
                                    </Items>
                                </f:FormRow>
                            </Rows>
                        </f:Form>
                        <f:Grid ID="Grid1" runat="server" BoxFlex="1" ShowBorder="true" ShowHeader="false" DataKeyNames="ID,Name"
                            EnableCheckBoxSelect="true" PageSize="10" IsDatabasePaging="true" AllowPaging="true"
                            AllowSorting="true" OnSort="Grid1_Sort" SortField="ID" SortDirection="ASC"
                            OnPreDataBound="Grid1_PreDataBound" OnRowCommand="Grid1_RowCommand"
                            OnPageIndexChange="Grid1_PageIndexChange"
                            EnableRowDoubleClickEvent="true" OnRowDoubleClick="Grid1_RowDoubleClick">
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
                                        <f:Button ID="btnNew" runat="server" Icon="Add" EnablePostBack="false" Text="新增配件">
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
                                <f:WindowField ColumnID="editField" HeaderText="" Hidden="false" TextAlign="Center" Icon="Pencil" ToolTip="编辑配件"
                                    WindowID="Window1" Title="编辑配件配件" DataIFrameUrlFields="ID" DataIFrameUrlFormatString="~/Parts/PartsEdit.aspx?id={0}"
                                    Width="40px" />
                                <f:LinkButtonField ColumnID="deleteField" HeaderText="" Hidden="true" TextAlign="Center" Icon="Delete" ToolTip="删除"
                                    ConfirmText="确定删除此记录？" ConfirmTarget="Top" CommandName="Delete" Width="40px" />
                                <f:TemplateField Width="100px" HeaderText="配件分类" SortField="PartsTypeID">
                                    <ItemTemplate>
                                        <asp:Label ID="Label1" runat="server" Width="100px" Text='<%# GetType(Eval("PartsTypeID").ToString()) %>'></asp:Label>
                                    </ItemTemplate>
                                </f:TemplateField>
                                <f:BoundField DataField="PartsName" SortField="PartsName" Width="150px" HeaderText="配件名称" />
                                <f:TemplateField Width="80px" HeaderText="单位">
                                    <ItemTemplate>
                                        <asp:Label ID="Label2" runat="server" Width="80px" Text='<%# GetUnitName(Eval("PartsUnit").ToString()) %>'></asp:Label>
                                    </ItemTemplate>
                                </f:TemplateField>
                                <f:BoundField DataField="UnitPrice" SortField="UnitPrice" Width="100px" HeaderText="单价(元)" />
                                <f:BoundField DataField="CostPrice" Width="100px" HeaderText="成本价(元)" />
                                <f:TemplateField Width="60px" HeaderText="状态">
                                    <ItemTemplate>
                                        <asp:Label ID="Label3" runat="server" Width="80px" Text='<%# GetIsUsed(Eval("IsUsed").ToString()) %>'></asp:Label>
                                    </ItemTemplate>
                                </f:TemplateField>
                                <f:BoundField DataField="Remark" SortField="Remark" ExpandUnusedSpace="true" HeaderText="备注" />

                            </Columns>
                        </f:Grid>
                    </Items>
                </f:Region>
            </Regions>
        </f:RegionPanel>
        <f:Window ID="Window1" runat="server" IsModal="true" Hidden="true" Target="Top" EnableResize="true"
            EnableMaximize="true" EnableIFrame="true" IFrameUrl="about:blank" Width="500px"
            Height="400px" OnClose="Window1_Close">
        </f:Window>
    </form>
</body>
</html>
