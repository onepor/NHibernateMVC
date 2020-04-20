﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VIPManager.aspx.cs" Inherits="ZAJCZN.MIS.Web.VIPManager" %>

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
            ShowHeader="false" Title="会员管理">
            <Items>
                <f:Form ID="Form2" runat="server" Height="36px" BodyPadding="5px" ShowHeader="false"
                    ShowBorder="false" LabelAlign="Right">
                    <Rows>
                        <f:FormRow ID="FormRow1" runat="server">
                            <Items>
                                <f:TwinTriggerBox ID="ttbSearchMessage" runat="server" ShowLabel="false" EmptyText="在会员名称、电话号码中搜索"
                                    Trigger1Icon="Clear" Trigger2Icon="Search" ShowTrigger1="false" OnTrigger2Click="ttbSearchMessage_Trigger2Click"
                                    OnTrigger1Click="ttbSearchMessage_Trigger1Click">
                                </f:TwinTriggerBox>
                            </Items>
                        </f:FormRow>
                    </Rows>
                </f:Form>
                <f:Grid ID="Grid1" runat="server" BoxFlex="1" ShowBorder="true" ShowHeader="false"
                    EnableCheckBoxSelect="true"
                    DataKeyNames="ID,Name" AllowSorting="true" OnSort="Grid1_Sort" SortField="RegisterDate"
                    SortDirection="ASC" AllowPaging="true" IsDatabasePaging="true" OnDataBinding="Grid1_PreDataBound"
                    OnRowCommand="Grid1_RowCommand" OnPageIndexChange="Grid1_PageIndexChange">
                    <Toolbars>
                        <f:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <f:Button ID="btnNew" runat="server" Icon="Add" EnablePostBack="false" Text="新增会员">
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
                        <f:RowNumberField TextAlign="Center" Width="35px" EnablePagingNumber="true" />
                        <f:BoundField DataField="VIPName" SortField="VIPName" Width="160px" HeaderText="会员名称" />
                        <f:BoundField DataField="VIPPhone" SortField="VIPPhone" Width="160px" HeaderText="电话号码" />
                        <f:BoundField DataField="VIPCount" SortField="VIPCount" Width="100px" HeaderText="账户余额" />
                        <f:BoundField DataField="VIPScore" SortField="VIPScore" Width="100px" HeaderText="消费积分" />
                        <f:BoundField DataField="RegisterDate" SortField="RegisterDate" Width="200px" HeaderText="注册时间" />
                        <f:BoundField DataField="Remark" SortField="Remark" Width="200px" HeaderText="备注" ExpandUnusedSpace="true" />
                        <f:WindowField ColumnID="editField" HeaderText="编辑" TextAlign="Center" Icon="Pencil" ToolTip="编辑"
                            WindowID="Window1" Title="编辑" DataIFrameUrlFields="ID" DataIFrameUrlFormatString="~/BusinessSet/VIPEdit.aspx?id={0}&action=edit"
                            Width="80px" />
                        <f:WindowField ColumnID="rechargeField" HeaderText="充值" TextAlign="Center" Icon="MoneyAdd" ToolTip="充值"
                            WindowID="Window2" Title="充值" DataIFrameUrlFields="ID" DataIFrameUrlFormatString="~/BusinessSet/VIPPrepaidManage.aspx?id={0}"
                            Width="80px" />
                    </Columns>
                </f:Grid>
            </Items>
        </f:Panel>
        <f:Window ID="Window1" runat="server" IsModal="true" Hidden="true" Target="Top" EnableResize="true"
            EnableMaximize="true" EnableIFrame="true" IFrameUrl="about:blank" Width="360px"
            Height="250px" OnClose="Window1_Close">
        </f:Window>
        <f:Window ID="Window2" runat="server" IsModal="true" Hidden="true" Target="Top" EnableResize="true"
            EnableMaximize="true" EnableIFrame="true" IFrameUrl="about:blank" Width="400px"
            Height="215px" OnClose="Window1_Close">
        </f:Window>
    </form>
</body>
</html>
