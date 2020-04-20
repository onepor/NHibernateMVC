<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EquipmentManage.aspx.cs" Inherits="ZAJCZN.MIS.Web.EquipmentManage" %>

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
                <f:Region ID="Region1" ShowBorder="false" ShowHeader="false" BodyPadding="5px" Width="240px" Position="Left" Layout="Fit" runat="server">
                    <Items>
                        <f:Grid ID="Grid2" runat="server" ShowBorder="true" ShowHeader="false" EnableCheckBoxSelect="false"
                            DataKeyNames="ID" AllowSorting="false" SortField="TypeName" SortDirection="DESC"
                            AllowPaging="false" EnableMultiSelect="false" OnRowClick="Grid2_RowClick" EnableRowClickEvent="true">
                            <Columns>
                                <f:BoundField DataField="TypeName" HeaderText="商品分类" Width="236px"></f:BoundField>
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
                                        <f:TwinTriggerBox ID="ttbSearchMessage" runat="server" ShowLabel="false" EmptyText="在商品名称中搜索"
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
                                        <f:Button ID="btnNew" runat="server" Icon="Add" EnablePostBack="false" Text="新增商品">
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
                                <f:WindowField ColumnID="editField" HeaderText="" Hidden="false" TextAlign="Center" Icon="Pencil" ToolTip="编辑商品"
                                    WindowID="Window1" Title="编辑商品商品" DataIFrameUrlFields="ID" DataIFrameUrlFormatString="~/Equipment/EquipmentEdit.aspx?id={0}"
                                    Width="40px" />
                                <f:LinkButtonField ColumnID="deleteField" HeaderText="" Hidden="true" TextAlign="Center" Icon="Delete" ToolTip="删除"
                                    ConfirmText="确定删除此记录？" ConfirmTarget="Top" CommandName="Delete" Width="40px" />
                                <f:TemplateField Width="100px" HeaderText="商品分类" SortField="EquipmentTypeID">
                                    <ItemTemplate>
                                        <asp:Label ID="Label1" runat="server" Width="100px" Text='<%# GetType(Eval("EquipmentTypeID").ToString()) %>'></asp:Label>
                                    </ItemTemplate>
                                </f:TemplateField>
                                <f:BoundField DataField="EquipmentName" SortField="EquipmentName" Width="150px" HeaderText="商品名称" />
                                <f:TemplateField Width="80px" HeaderText="计价单位">
                                    <ItemTemplate>
                                        <asp:Label ID="Label2" runat="server" Width="80px" Text='<%# GetUnitName(Eval("EquipmentUnit").ToString()) %>'></asp:Label>
                                    </ItemTemplate>
                                </f:TemplateField>
                                <f:TemplateField Width="80px" HeaderText="计价方式">
                                    <ItemTemplate>
                                        <asp:Label ID="Label4" runat="server" Width="80px" Text='<%# GetCalcUnitName(Eval("CalcUnitType").ToString()) %>'></asp:Label>
                                    </ItemTemplate>
                                </f:TemplateField>
                                <f:BoundField DataField="UnitPrice" SortField="UnitPrice" Width="100px" HeaderText="单价(元)" />
                                <f:BoundField DataField="InstallCost" SortField="InstallCost" Width="100px" HeaderText="装运费(元)" />
                                <f:BoundField DataField="LineName" Width="100px" HeaderText="配套线条" />
                                <f:GroupField HeaderText="标准尺寸" TextAlign="Center">
                                    <Columns>
                                        <f:BoundField DataField="EHeight" SortField="EHeight" Width="80px" HeaderText="标准高" />
                                        <f:BoundField DataField="EWide" SortField="EWide" Width="80px" HeaderText="标准宽" />
                                        <f:BoundField DataField="EThickness" SortField="EThickness" Width="80px" HeaderText="标准厚" />
                                    </Columns>
                                </f:GroupField>
                                <f:GroupField HeaderText="超标计算" TextAlign="Center">
                                    <Columns>
                                        <f:TemplateField Width="80px" HeaderText="超标计算">
                                            <ItemTemplate>
                                                <asp:Label ID="Label5" runat="server" Width="80px" Text='<%# GetPassUnit(Eval("PassCalcType").ToString()) %>'></asp:Label>
                                            </ItemTemplate>
                                        </f:TemplateField>
                                        <f:BoundField DataField="PassHeight" SortField="PassHeight" Width="80px" HeaderText="超高(元)" />
                                        <f:BoundField DataField="PassWide" SortField="PassWide" Width="80px" HeaderText="超宽(元)" />
                                        <f:BoundField DataField="PassThckness" SortField="PassThckness" Width="80px" HeaderText="超厚(元)" />
                                        <f:BoundField DataField="PassArea" SortField="PassArea" Width="90px" HeaderText="超面积(元)" />
                                    </Columns>
                                </f:GroupField>
                                <f:TemplateField Width="60px" HeaderText="状态">
                                    <ItemTemplate>
                                        <asp:Label ID="Label3" runat="server" Width="80px" Text='<%# GetIsUsed(Eval("IsUsed").ToString()) %>'></asp:Label>
                                    </ItemTemplate>
                                </f:TemplateField>
                                <f:BoundField DataField="Remark" Width="200px" HeaderText="备注" />
                            </Columns>
                        </f:Grid>
                    </Items>
                </f:Region>
            </Regions>
        </f:RegionPanel>
        <f:Window ID="Window1" runat="server" IsModal="true" Hidden="true" Target="Top" EnableResize="true"
            EnableMaximize="true" EnableIFrame="true" IFrameUrl="about:blank" Width="700px"
            Height="500px" OnClose="Window1_Close">
        </f:Window>
    </form>
</body>
</html>
