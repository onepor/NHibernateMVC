﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PaymentManager.aspx.cs" Inherits="ZAJCZN.MIS.Web.PaymentManager" %>

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
            ShowHeader="false" Title="支付方式管理">
            <Items>
                <f:Form ID="Form2" runat="server" Height="36px" BodyPadding="5px" ShowHeader="false"
                    ShowBorder="false" LabelAlign="Right">
                    <Rows>
                        <f:FormRow ID="FormRow1" runat="server">
                            <Items>
                                <f:TwinTriggerBox ID="ttbSearchMessage" runat="server" ShowLabel="false" EmptyText="在支付方式名称中搜索"
                                    Trigger1Icon="Clear" Trigger2Icon="Search" ShowTrigger1="false" OnTrigger2Click="ttbSearchMessage_Trigger2Click"
                                    OnTrigger1Click="ttbSearchMessage_Trigger1Click">
                                </f:TwinTriggerBox>
                                <f:DropDownList ID="ddlIsUsed" Label="是否启用" OnSelectedIndexChanged="rblEnableStatus_SelectedIndexChanged" AutoPostBack="true" runat="server">
                                    <f:ListItem Text="全部" Selected="true" Value="" />
                                    <f:ListItem Text="启用" Value="1" />
                                    <f:ListItem Text="停用" Value="0" />
                                </f:DropDownList>
                                <f:DropDownList ID="ddlIsVip" Hidden="true" Label="是否用于会员充值" LabelWidth="130px" OnSelectedIndexChanged="rblEnableStatus_SelectedIndexChanged" AutoPostBack="true" runat="server">
                                    <f:ListItem Text="全部" Selected="true" Value="" />
                                    <f:ListItem Text="是" Value="1" />
                                    <f:ListItem Text="否" Value="0" />
                                </f:DropDownList>
                                <f:DropDownList ID="ddlIsIntegral" Hidden="true" Label="是否参与积分" OnSelectedIndexChanged="rblEnableStatus_SelectedIndexChanged" AutoPostBack="true" runat="server">
                                    <f:ListItem Text="全部" Selected="true" Value="" />
                                    <f:ListItem Text="是" Value="1" />
                                    <f:ListItem Text="否" Value="0" />
                                </f:DropDownList>
                            </Items>
                        </f:FormRow>
                    </Rows>
                </f:Form>
                <f:Grid ID="Grid1" runat="server" BoxFlex="1" ShowBorder="true" ShowHeader="false"
                    EnableCheckBoxSelect="true"
                    DataKeyNames="ID,Name" AllowSorting="true" OnSort="Grid1_Sort" SortField="ID"
                    SortDirection="ASC" AllowPaging="true" IsDatabasePaging="true" OnDataBinding="Grid1_PreDataBound"
                    OnRowCommand="Grid1_RowCommand" OnPageIndexChange="Grid1_PageIndexChange">
                    <Toolbars>
                        <f:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <f:Button ID="btnDeleteSelected" Hidden="true" Icon="Delete" runat="server" Text="删除选中记录" OnClick="btnDeleteSelected_Click">
                                </f:Button>
                                <f:Button ID="btnChangeEnableUsers" Icon="GroupEdit" EnablePostBack="false" runat="server"
                                    Text="设置启用状态">
                                    <Menu runat="server">
                                        <f:MenuButton ID="btnEnableUsers" OnClick="btnEnableUsers_Click" runat="server" Text="启用选中记录">
                                        </f:MenuButton>
                                        <f:MenuButton ID="btnDisableUsers" OnClick="btnDisableUsers_Click" runat="server" Text="停用选中记录">
                                        </f:MenuButton>
                                    </Menu>
                                </f:Button>
                                <f:ToolbarSeparator runat="server">
                                </f:ToolbarSeparator>
                                <f:Button ID="btnNew" runat="server" Icon="Add" EnablePostBack="false" Text="新增支付方式">
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
                        <f:BoundField DataField="PaymentName" SortField="PaymentName" Width="100px" HeaderText="支付方式名称" />
                        <f:TemplateField Width="130px" HeaderText="是否用于会员充值">
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Width="120px" Text='<%# Eval("IsVip").ToString()=="1"?"是":"否" %>'></asp:Label>
                            </ItemTemplate>
                        </f:TemplateField>
                        <f:TemplateField Width="100px" HeaderText="是否参与积分">
                            <ItemTemplate>
                                <asp:Label ID="Label3" runat="server" Width="120px" Text='<%# Eval("IsIntegral").ToString()=="1"?"是":"否" %>'></asp:Label>
                            </ItemTemplate>
                        </f:TemplateField>
                        <f:TemplateField Width="100px" HeaderText="记录三方券号">
                            <ItemTemplate>
                                <asp:Label ID="Label4" runat="server" Width="120px" Text='<%# Eval("Proportion").ToString()=="1"?"是":"否" %>'></asp:Label>
                            </ItemTemplate>
                        </f:TemplateField>
                        <f:TemplateField Width="80px" HeaderText="是否启用">
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Width="120px" Text='<%# Eval("IsUsed").ToString()=="1"?"启用":"停用" %>'></asp:Label>
                            </ItemTemplate>
                        </f:TemplateField>
                        <f:BoundField DataField="Proportion" Hidden="true" SortField="Proportion" Width="100px" HeaderText="实收占比(%)" />
                        <f:BoundField DataField="Sort" SortField="Sort" Width="70px" HeaderText="排序" />
                        <f:WindowField ColumnID="editField" HeaderText="编辑" TextAlign="Center" Icon="Pencil" ToolTip="编辑"
                            WindowID="Window1" Title="编辑" DataIFrameUrlFields="ID" DataIFrameUrlFormatString="~/BusinessSet/PaymentEdit.aspx?id={0}&action=edit"
                            Width="80px" />
                        <f:LinkButtonField ColumnID="deleteField" HeaderText="删除" Hidden="true" TextAlign="Center" Icon="Delete" ToolTip="删除"
                            ConfirmText="确定删除此记录？" ConfirmTarget="Top" CommandName="Delete" Width="80px" />
                    </Columns>
                </f:Grid>
            </Items>
        </f:Panel>
        <f:Window ID="Window1" runat="server" IsModal="true" Hidden="true" Target="Top" EnableResize="true"
            EnableMaximize="true" EnableIFrame="true" IFrameUrl="about:blank" Width="360px"
            Height="360px" OnClose="Window1_Close">
        </f:Window>
    </form>
</body>
</html>
