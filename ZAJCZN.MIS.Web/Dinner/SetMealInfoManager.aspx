<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SetMealInfoManager.aspx.cs" Inherits="ZAJCZN.MIS.Web.SetMealInfoManager" %>

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
            ShowBorder="false" ShowHeader="false" Layout="Fit">
            <Items>
                <f:Grid ID="Grid1" runat="server" ShowBorder="true" ShowHeader="false"
                    EnableCheckBoxSelect="true" DataKeyNames="ID" OnPreDataBound="Grid1_PreDataBound"
                    OnRowCommand="Grid1_RowCommand">
                    <Toolbars>
                        <f:Toolbar ID="Toolbar1" Position="Top" runat="server">
                            <Items>
                                <f:Button ID="btnChangeEnableUsers" Icon="GroupEdit" EnablePostBack="false" runat="server"
                                    Text="设置启用状态">
                                    <Menu ID="Menu1" runat="server">
                                        <f:MenuButton ID="btnEnableUsers" OnClick="btnEnableUsers_Click" runat="server" Text="启用选中记录">
                                        </f:MenuButton>
                                        <f:MenuButton ID="btnDisableUsers" OnClick="btnDisableUsers_Click" runat="server" Text="停用选中记录">
                                        </f:MenuButton>
                                    </Menu>
                                </f:Button>
                                <f:Button ID="btnNew" runat="server" Icon="Add" EnablePostBack="false" Text="新增优惠套餐">
                                </f:Button>
                                <f:ToolbarFill ID="ToolbarFill1" runat="server">
                                </f:ToolbarFill>
                            </Items>
                        </f:Toolbar>
                    </Toolbars>
                    <Columns>
                        <f:RowNumberField />
                        <f:BoundField DataField="SetMealName" SortField="SetMealName" HeaderText="套餐名" Width="300px" />
                        <f:BoundField DataField="Price" SortField="Price" HeaderText="原价(元)" Width="160px" />
                        <f:BoundField DataField="PreferentialPrice" SortField="PreferentialPrice" HeaderText="活动价(元)" Width="160px" />
                        <f:TemplateField Width="100px" HeaderText="是否启用">
                            <ItemTemplate>
                                <asp:Label ID="Label3" runat="server" Width="120px" Text='<%# GetIsUsed(Eval("IsEnabled").ToString()) %>'></asp:Label>
                            </ItemTemplate>
                        </f:TemplateField>
                        <f:WindowField ColumnID="editField" TextAlign="Center" Icon="Pencil" ToolTip="编辑套餐"
                            WindowID="Window1" Title="编辑套餐" DataIFrameUrlFields="ID" DataIFrameUrlFormatString="~/Dinner/SetMealInfoEdit.aspx?id={0}"
                            Width="50px" />
                        <f:LinkButtonField ColumnID="deleteField" Hidden="true" TextAlign="Center" Icon="Delete" ToolTip="删除"
                            ConfirmText="确定删除此记录？" ConfirmTarget="Top" CommandName="Delete" Width="50px" />
                    </Columns>
                </f:Grid>
            </Items>
        </f:Panel>
        <f:Window ID="Window1" CloseAction="Hide" runat="server" IsModal="true" Hidden="true"
            Target="Top" EnableResize="true" EnableMaximize="true" EnableIFrame="true" IFrameUrl="about:blank"
            Width="800px" Height="600px" OnClose="Window1_Close">
        </f:Window>
    </form>
</body>
</html>
