﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DinnerOrderManage.aspx.cs" Inherits="ZAJCZN.MIS.Web.DinnerOrderManage" %>

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
            ShowHeader="false" Title="餐台消费查询">
            <Items>
                <f:Panel ID="Panel2" ShowHeader="false" ShowBorder="false" Layout="Column" runat="server">
                    <Items>
                        <f:TextBox ID="txtOrderNo" runat="server" Label="订单号" Width="300px" LabelAlign="Right" LabelWidth="80px"></f:TextBox>
                        <f:DatePicker ID="dpStartDate" LabelWidth="120px" Label="营业起止时间" Width="260px" runat="server" LabelAlign="Right">
                        </f:DatePicker>
                        <f:DatePicker ID="dpEndDate" LabelWidth="10px" Label="~" Width="150px" runat="server"
                            CompareControl="dpStartDate" CompareOperator="GreaterThan" CompareMessage="结束日期应该大于开始日期！">
                        </f:DatePicker>
                        <f:Button ID="btnSearch" MarginLeft="20px" runat="server" OnClick="btnSearch_Click" Icon="Find" Text="查询"></f:Button>
                    </Items>
                </f:Panel>
                <f:Grid ID="Grid1" runat="server" BoxFlex="1" ShowBorder="true" ShowHeader="false"
                    EnableCheckBoxSelect="false" DataKeyNames="ID" AllowSorting="true" OnSort="Grid1_Sort" SortField="ID"
                    SortDirection="ASC" AllowPaging="true" IsDatabasePaging="true"
                    OnRowCommand="Grid1_RowCommand" OnPageIndexChange="Grid1_PageIndexChange">
                    <Toolbars>
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
                        <f:LinkButtonField HeaderText="库存" TextAlign="Center" Icon="Information" ToolTip="库存" ID="stockField"
                            CommandName="stockField" Width="70px" Hidden="true" ConfirmText="确定做出库处理？" />
                        <f:LinkButtonField HeaderText="反结算" TextAlign="Center" Icon="Information" ToolTip="反结算" ID="stockBack"
                            CommandName="stockBack" Width="70px"  ConfirmText="确定做反结算处理？" />
                        <f:LinkButtonField HeaderText="查看" TextAlign="Center" Icon="Report" ToolTip="查看消费详情"
                            CommandName="viewField" Width="60px" ID="viewField" />
                        <f:TemplateField Width="70px" HeaderText="台号">
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Width="70px" Text='<%# GetTabieName(Eval("TabieID").ToString()) %>'></asp:Label>
                            </ItemTemplate>
                        </f:TemplateField>
                        <f:BoundField DataField="OrderNO" SortField="OrderNO" Width="150px" HeaderText="业务单号" />
                        <f:BoundField DataField="OpenTime" SortField="OpenTime" Width="150px" HeaderText="开台时间" />
                        <f:BoundField DataField="ClearTime" SortField="ClearTime" Width="150px" HeaderText="结算时间" />
                        <f:BoundField DataField="Population" SortField="Population" Width="80px" HeaderText="就餐人数" />
                        <f:BoundField DataField="Moneys" SortField="Moneys" Width="100px" HeaderText="消费金额(元)" />
                        <f:BoundField DataField="PrePrice" SortField="PrePrice" Width="100px" HeaderText="赠送金额(元)" />
                        <f:BoundField DataField="FactPrice" SortField="FactPrice" Width="100px" HeaderText="实付金额(元)" />
                        <f:TemplateField Width="190px" HeaderText="订单状态">
                            <ItemTemplate>
                                <asp:Label ID="lblState" runat="server" Width="190px" Text='<%# GetPayType(Eval("ID").ToString()) %>'></asp:Label>
                            </ItemTemplate>
                        </f:TemplateField>
                    </Columns>
                </f:Grid>
            </Items>
        </f:Panel>
    </form>
</body>
</html>
