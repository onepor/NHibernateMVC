<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PartsTypeManage.aspx.cs" Inherits="ZAJCZN.MIS.Web.PartsTypeManage" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server" />
        <f:Panel ID="Panel1" runat="server" BodyPadding="5px"
            ShowBorder="false" ShowHeader="false" Layout="Fit">
            <Items>
                <f:Grid ID="Grid1" runat="server" ShowBorder="true" ShowHeader="false"
                    OnSort="Grid1_Sort" SortField="ID" SortDirection="ASC"
                    EnableCheckBoxSelect="true" DataKeyNames="ID" OnPreDataBound="Grid1_PreDataBound"
                    OnRowCommand="Grid1_RowCommand" AllowSorting="true" 
                    EnableRowDoubleClickEvent="true" OnRowDoubleClick="Grid1_RowDoubleClick">
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
                                <f:Button ID="btnNew" runat="server" Icon="Add" EnablePostBack="false" Text="新增配件类别">
                                </f:Button>
                                <f:ToolbarFill ID="ToolbarFill1" runat="server">
                                </f:ToolbarFill>
                            </Items>
                        </f:Toolbar>
                    </Toolbars>
                    <Columns>
                        <f:WindowField ColumnID="editField" TextAlign="Center" Icon="Pencil" ToolTip="编辑配件类别"
                            WindowID="Window1" Title="编辑配件类别" DataIFrameUrlFields="ID" DataIFrameUrlFormatString="~/Parts/PartsTypeEdit.aspx?id={0}"
                            Width="50px" />
                        <f:BoundField DataField="TypeName" SortField="TypeName" HeaderText="配件类别名称" Width="300px" />
                        <f:TemplateField Width="60px" HeaderText="状态">
                            <ItemTemplate>
                                <asp:Label ID="Label3" runat="server" Width="60px" Text='<%# GetIsUsed(Eval("IsUsed").ToString()) %>'></asp:Label>
                            </ItemTemplate>
                        </f:TemplateField>
                        <f:BoundField DataField="Remark" HeaderText="备注" Width="300px" ExpandUnusedSpace="true"  />
                    </Columns>
                </f:Grid>
            </Items>
        </f:Panel>
        <f:Window ID="Window1" CloseAction="Hide" runat="server" IsModal="true" Hidden="true"
            Target="Top" EnableResize="true" EnableMaximize="true" EnableIFrame="true" IFrameUrl="about:blank"
            Width="500px" Height="400px" OnClose="Window1_Close">
        </f:Window>
    </form>
</body>
</html>
