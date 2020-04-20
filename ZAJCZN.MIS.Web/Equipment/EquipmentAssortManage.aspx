<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EquipmentAssortManage.aspx.cs" Inherits="ZAJCZN.MIS.Web.EquipmentAssortManage" %>

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
            ShowHeader="false" Title="配套材料物品">
            <Items>
                <f:Grid ID="Grid1" ShowBorder="true" ShowHeader="true" EnableCollapse="true"
                    runat="server" DataKeyNames="ID" AllowCellEditing="true" ClicksToEdit="1"
                    OnPreDataBound="Grid1_PreDataBound" EnableCheckBoxSelect="true" OnRowCommand="Grid1_RowCommand"
                    EnableAfterEditEvent="true" OnAfterEdit="Grid1_AfterEdit">
                    <Toolbars>
                        <f:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <f:Button ID="btnDeleteSelected" Text="删除选中行" Icon="Delete" Hidden="true"
                                    runat="server" OnClick="btnDeleteSelected_Click">
                                </f:Button>
                                <f:Button ID="btnUpdate" Text="同步更新物品规格配套材料信息" Icon="TableRefresh" runat="server" OnClick="btnUpdate_Click">
                                </f:Button>
                                <f:ToolbarFill ID="ToolbarFill1" runat="server">
                                </f:ToolbarFill>
                                <f:Button ID="btnNew" Text="新增配套材料" Icon="Add" EnablePostBack="false" runat="server">
                                </f:Button>
                                <f:Button ID="btnReturn" OnClick="btnReturn_Click" runat="server" Icon="arrowleft" Text="返回物品列表">
                                </f:Button>
                            </Items>
                        </f:Toolbar>
                    </Toolbars>
                    <Columns>
                        <f:BoundField DataField="EquipmentInfo.EquipmentCode" SortField="EquipmentInfo.EquipmentCode" Width="100px" HeaderText="物品编码" />
                        <f:BoundField DataField="EquipmentInfo.EquipmentName" SortField="EquipmentInfo.EquipmentName" Width="200px" HeaderText="物品名称" />
                        <f:BoundField DataField="EquipmentInfo.Standard" SortField="EquipmentInfo.Standard" Width="100px" HeaderText="规格" />
                        <f:TemplateField Width="100px" HeaderText="物品单位">
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Width="100px" Text='<%# GetUnitName(Eval("EquipmentInfo.EquipmentUnit").ToString()) %>'></asp:Label>
                            </ItemTemplate>
                        </f:TemplateField>
                        <f:TemplateField Width="120px" HeaderText="物品分类">
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Width="120px" Text='<%# GetType(Eval("EquipmentInfo.EquipmentTypeID").ToString()) %>'></asp:Label>
                            </ItemTemplate>
                        </f:TemplateField>
                        <f:RenderField Width="90px" ColumnID="EquipmentCount" DataField="EquipmentCount" FieldType="Double"
                            HeaderText="单位数量">
                            <Editor>
                                <f:NumberBox ID="txtCount" NoDecimal="true" NoNegative="true" MinValue="1" Width="90px"
                                    MaxValue="5000" runat="server">
                                </f:NumberBox>
                            </Editor>
                        </f:RenderField>
                        <f:RenderField Width="90px" ColumnID="AssortCount" DataField="AssortCount" FieldType="Double"
                            HeaderText="配套数量">
                            <Editor>
                                <f:NumberBox ID="tbxUsingCount" NoDecimal="true" NoNegative="true" MinValue="1" Width="90px"
                                    MaxValue="5000" runat="server">
                                </f:NumberBox>
                            </Editor>
                        </f:RenderField>
                        <f:RenderCheckField Width="80px" ColumnID="IsOutCalcNumber" DataField="IsOutCalcNumber" HeaderText="出库算量" />
                        <f:RenderCheckField Width="80px" ColumnID="IsOutCalcPrice" DataField="IsOutCalcPrice" HeaderText="出库算价" />
                        <f:RenderCheckField Width="80px" ColumnID="IsInCalcNumber" DataField="IsInCalcNumber" HeaderText="入库算量" />
                        <f:RenderCheckField Width="80px" ColumnID="IsInCalcPrice" DataField="IsInCalcPrice" HeaderText="入库算价" />
                        <f:LinkButtonField ColumnID="deleteField" HeaderText="删除" TextAlign="Center" Icon="Delete" ToolTip="删除"
                            ConfirmText="确定删除此配套材料记录？" ConfirmTarget="Top" CommandName="Delete" Width="80px" />
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
