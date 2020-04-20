<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RepairProjectManager.aspx.cs" Inherits="ZAJCZN.MIS.Web.RepairProjectManager" %>

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
            ShowHeader="false" Title="费用项管理">
            <Items>
                <f:Panel ID="Panel2" ShowHeader="false" ShowBorder="false" Layout="Column" runat="server">
                    <Items>
                        <f:DropDownList ID="ddlState" Label="费用类型" LabelAlign="Right"
                            runat="server" LabelWidth="80px" Width="300px" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlState_SelectedIndexChanged">
                            <f:ListItem Text="-全部类型-" Value="" Selected="true" />
                            <f:ListItem Text="员工费用" Value="1" />
                            <f:ListItem Text="客户费用" Value="2" />
                            <f:ListItem Text="司机费用" Value="3" />
                        </f:DropDownList>
                        <f:DropDownList ID="ddlUseRange" Label="使用类型" LabelAlign="Right"
                            runat="server" LabelWidth="80px" Width="300px" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlState_SelectedIndexChanged">
                            <f:ListItem Text="-全部类型-" Value="" Selected="true" />
                            <f:ListItem Text="场内" Value="1" />
                            <f:ListItem Text="发货" Value="2" />
                            <f:ListItem Text="收货" Value="3" />
                        </f:DropDownList>
                    </Items>
                </f:Panel>
                <f:Grid ID="Grid1" runat="server" BoxFlex="1" ShowBorder="true" ShowHeader="false"
                    EnableCheckBoxSelect="false" DataKeyNames="ID" AllowSorting="true" OnSort="Grid1_Sort" SortField="ID"
                    SortDirection="ASC" AllowPaging="true" IsDatabasePaging="true" OnDataBinding="Grid1_PreDataBound"
                    OnRowCommand="Grid1_RowCommand" OnPageIndexChange="Grid1_PageIndexChange"
                    EnableRowDoubleClickEvent="true" OnRowDoubleClick="Grid1_RowDoubleClick">
                    <Toolbars>
                        <f:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <f:Button ID="btnNew" runat="server" Icon="Add" EnablePostBack="false" Text="新增费用项">
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
                        <f:RowNumberField TextAlign="Center" Width="35px" EnablePagingNumber="true" Hidden="true" />
                        <f:TemplateField Width="100px" HeaderText="项目类型">
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Width="100px" Text='<%# GetProjectType(Eval("ProjectType").ToString()) %>'></asp:Label>
                            </ItemTemplate>
                        </f:TemplateField>
                        <f:BoundField DataField="ProjectName" SortField="ProjectName" Width="160px" HeaderText="维修项名称" />
                        <f:TemplateField Width="80px" HeaderText="计费单位" SortField="PayUnit">
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Width="100px" Text='<%# GetUnitName(Eval("PayUnit").ToString()) %>'></asp:Label>
                            </ItemTemplate>
                        </f:TemplateField>
                        <f:TemplateField Width="100px" HeaderText="使用类型" SortField="UsingType">
                            <ItemTemplate>
                                <asp:Label ID="Label4" runat="server" Width="100px" Text='<%# GetUsingType(Eval("UsingType").ToString()) %>'></asp:Label>
                            </ItemTemplate>
                        </f:TemplateField>
                        <f:TemplateField Width="100px" HeaderText="计费类型">
                            <ItemTemplate>
                                <asp:Label ID="Label6" runat="server" Width="100px" Text='<%# GetPriceSourceType(Eval("PriceSourceType").ToString()) %>'></asp:Label>
                            </ItemTemplate>
                        </f:TemplateField>
                        <f:BoundField DataField="PayPrice" SortField="PayPrice" Width="120px" HeaderText="计费单价(元)" />
                        <f:TemplateField Width="90px" HeaderText="固定费用">
                            <ItemTemplate>
                                <asp:Label ID="Label7" runat="server" Width="90px" Text='<%#  Eval("IsRegular").ToString().Equals ("1")?"是":"" %>'></asp:Label>
                            </ItemTemplate>
                        </f:TemplateField>
                         <f:TemplateField Width="90px" HeaderText="生成工单">
                            <ItemTemplate>
                                <asp:Label ID="Label8" runat="server" Width="90px" Text='<%#  Eval("IsCreateJob").ToString().Equals ("1")?"是":"" %>'></asp:Label>
                            </ItemTemplate>
                        </f:TemplateField>
                         <f:TemplateField Width="90px" HeaderText="二次分拣">
                            <ItemTemplate>
                                <asp:Label ID="Label9" runat="server" Width="90px" Text='<%#  Eval("IsSorting").ToString().Equals ("1")?"是":"" %>'></asp:Label>
                            </ItemTemplate>
                        </f:TemplateField>
                        <f:TemplateField Width="60px" HeaderText="状态">
                            <ItemTemplate>
                                <asp:Label ID="Label3" runat="server" Width="60px" Text='<%# GetIsUsed(Eval("IsUsed").ToString()) %>'></asp:Label>
                            </ItemTemplate>
                        </f:TemplateField>
                        <f:TemplateField Width="300px" HeaderText="适用范围">
                            <ItemTemplate>
                                <asp:Label ID="Label5" runat="server" Width="300px" Text='<%# GetGoodsType(Eval("UsingGoods").ToString()) %>'></asp:Label>
                            </ItemTemplate>
                        </f:TemplateField>
                        <f:BoundField DataField="Remark" SortField="Remark" Width="100px" HeaderText="备注" />
                        <f:WindowField ColumnID="editField" HeaderText="编辑" TextAlign="Center" Icon="Pencil" ToolTip="编辑"
                            WindowID="Window1" Title="编辑" DataIFrameUrlFields="ID" DataIFrameUrlFormatString="~/SysSet/RepairProjectEdit.aspx?id={0}&action=edit"
                            Width="80px" />
                        <f:LinkButtonField ColumnID="deleteField" HeaderText="" Hidden="true" TextAlign="Center" Icon="Delete" ToolTip="删除"
                            ConfirmText="确定删除此记录？" ConfirmTarget="Top" CommandName="Delete" Width="40px" />
                    </Columns>
                </f:Grid>
            </Items>
        </f:Panel>
        <f:Window ID="Window1" runat="server" IsModal="true" Hidden="true" Target="Top" EnableResize="true"
            EnableMaximize="true" EnableIFrame="true" IFrameUrl="about:blank" Width="520px"
            Height="650px" OnClose="Window1_Close">
        </f:Window>
    </form>
</body>
</html>
