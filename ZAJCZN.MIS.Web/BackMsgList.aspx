<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BackMsgList.aspx.cs" Inherits="ZAJCZN.MIS.Web.BackMsgList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server" />
        <f:Panel ID="Panel1" runat="server" BodyPadding="5px"
            ShowBorder="false" Layout="VBox" BoxConfigAlign="Stretch" BoxConfigPosition="Start"
            ShowHeader="false" Title="出库单查询">
            <Items>
                <f:Panel ID="Panel2" ShowHeader="false" ShowBorder="false" Layout="Column" runat="server">
                    <Items>
                        <f:DatePicker ID="dpStartDate" LabelWidth="120px" Label="发送起止时间" Width="260px" runat="server" LabelAlign="Right">
                        </f:DatePicker>
                        <f:DatePicker ID="dpEndDate" LabelWidth="10px" Label="~" Width="150px" runat="server"
                            CompareControl="dpStartDate" CompareOperator="GreaterThan" CompareMessage="结束日期应该大于开始日期！">
                        </f:DatePicker>
                        <f:Button ID="btnSearch" MarginLeft="20px" runat="server" OnClick="btnSearch_Click" Icon="Find" Text="查询"></f:Button>
                    </Items>
                </f:Panel>
                <f:Grid ID="Grid1" runat="server" BoxFlex="1" ShowBorder="true" ShowHeader="false"
                    EnableCheckBoxSelect="false" DataKeyNames="ID,Name" AllowSorting="true" OnSort="Grid1_Sort" SortField="ID"
                    SortDirection="ASC" AllowPaging="true" IsDatabasePaging="true" OnPageIndexChange="Grid1_PageIndexChange">
                    <Toolbars>
                        <f:Toolbar ID="Toolbar1" runat="server">
                            <Items>
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
                        <f:BoundField TextAlign="Center" DataField="UserName" SortField="UserName" Width="200px" HeaderText="用户名称" />
                        <f:BoundField TextAlign="Center" DataField="SendTitle" Width="230px" HeaderText="发送标题" />
                        <f:BoundField TextAlign="Center" DataField="SendContent" Width="100px" HeaderText="回复内容" />
                        <f:BoundField TextAlign="Center" ExpandUnusedSpace="true" DataField="SendTime" SortField="SendTime" Width="200px" HeaderText="回复时间" />
                    </Columns>
                </f:Grid>
            </Items>
        </f:Panel>
    </form>
</body>
</html>
