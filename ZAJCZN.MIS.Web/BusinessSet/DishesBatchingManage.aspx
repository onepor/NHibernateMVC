<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DishesBatchingManage.aspx.cs" Inherits="ZAJCZN.MIS.Web.DishesBatchingManage" %>

<!DOCTYPE html>
<html>
<head id="Head1" runat="server">
    <title></title>
    <meta name="sourcefiles" content="~/PublicWebForm/GoodSelectDialog.aspx" />
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server" />
        <f:Panel ID="Panel1" runat="server" BodyPadding="5px"
            ShowBorder="false" Layout="VBox" BoxConfigAlign="Stretch" BoxConfigPosition="Start"
            ShowHeader="false" Title="库存物品">
            <Items>
                <f:Grid ID="Grid1" ShowBorder="true" ShowHeader="true" EnableCollapse="true"
                    runat="server" DataKeyNames="ID" AllowCellEditing="true" ClicksToEdit="1"
                    OnPreDataBound="Grid1_PreDataBound" EnableCheckBoxSelect="true" OnRowCommand="Grid1_RowCommand"
                    EnableAfterEditEvent="true" OnAfterEdit="Grid1_AfterEdit">
                    <Toolbars>
                        <f:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <f:Button ID="btnNew" Text="新增配料" Icon="Add" EnablePostBack="false" runat="server">
                                </f:Button>
                                <f:Button ID="btnDeleteSelected" Text="删除选中行" Icon="Delete"
                                    runat="server" OnClick="btnDeleteSelected_Click">
                                </f:Button>
                                <f:ToolbarFill ID="ToolbarFill1" runat="server">
                                </f:ToolbarFill>
                                <f:Button ID="btnReturn" OnClick="btnReturn_Click" runat="server" Icon="arrowleft" Text="返回菜品列表">
                                </f:Button>
                            </Items>
                        </f:Toolbar>
                    </Toolbars>
                    <Columns>
                        <f:BoundField DataField="BatchingName" SortField="BatchingName" Width="180px" HeaderText="物品名称" ExpandUnusedSpace="true" />
                        <f:BoundField DataField="GoodsCode" SortField="GoodsCode" Width="120px" HeaderText="物品编码" />
                        <f:TemplateField Width="60px" HeaderText="单位">
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Width="100px" Text='<%# GetUnitName(Eval("UsingUnit").ToString()) %>'></asp:Label>
                            </ItemTemplate>
                        </f:TemplateField>
                        <f:RenderField Width="100px" ColumnID="UsingCount" DataField="UsingCount" FieldType="Double"
                            HeaderText="消耗数量">
                            <Editor>
                                <f:NumberBox ID="tbxUsingCount" NoDecimal="true" NoNegative="true" MinValue="1"
                                    MaxValue="5000" runat="server">
                                </f:NumberBox>
                            </Editor>
                        </f:RenderField>
                        <f:BoundField DataField="UsingUnitPrice" SortField="UsingUnitPrice" Width="120px" HeaderText="消耗单价(元)" ExpandUnusedSpace="true" />
                        <f:BoundField DataField="CostPrice" SortField="CostPrice" Width="120px" HeaderText="消耗成本(元)" ExpandUnusedSpace="true" />
                        <f:TemplateField Width="100px" HeaderText="冲减库存">
                            <ItemTemplate>
                                <asp:Label ID="Label3" runat="server" Width="100px" Text='<%# GetIsUsed(Eval("IsOffset").ToString()) %>'></asp:Label>
                            </ItemTemplate>
                        </f:TemplateField>
                        <f:LinkButtonField ColumnID="deleteField" HeaderText="删除" TextAlign="Center" Icon="Delete" ToolTip="删除"
                            ConfirmText="确定删除此配料记录？" ConfirmTarget="Top" CommandName="Delete" Width="80px" />
                    </Columns>
                </f:Grid>
                <f:Label ID="labResult" EncodeText="false" runat="server">
                </f:Label>
            </Items>
        </f:Panel>
        <f:Window ID="Window1" runat="server" IsModal="true" Hidden="true" Target="Top" EnableResize="true"
            EnableMaximize="true" EnableIFrame="true" IFrameUrl="about:blank" Width="1000px"
            Height="560px" OnClose="Window1_Close">
        </f:Window>
    </form>
</body>
</html>
