<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SysSendMsgList.aspx.cs" Inherits="ZAJCZN.MIS.Web.SysSendMsgList" %>

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
                        <f:TextBox ID="txtUserName" runat="server" Label="用户名称" Width="300px" LabelAlign="Right" LabelWidth="120px"></f:TextBox>
                        <f:DatePicker ID="dpStartDate" LabelWidth="120px" Label="发送起止时间" Width="260px" runat="server" LabelAlign="Right">
                        </f:DatePicker>
                        <f:DatePicker ID="dpEndDate" LabelWidth="10px" Label="~" Width="150px" runat="server"
                            CompareControl="dpStartDate" CompareOperator="GreaterThan" CompareMessage="结束日期应该大于开始日期！">
                        </f:DatePicker>
                        <%--<f:DropDownList ID="ddlIsBack" Label="是否回复" LabelAlign="Right" AutoPostBack="False" runat="server" Width="260px">
                        </f:DropDownList>--%>
                        <f:DropDownList runat="server" ID="ddlIsBack" Width="260px" Label="是否回复">
                            <f:ListItem Text="全部" Value="-1" />
                            <f:ListItem Text="否" Value="0" />
                            <f:ListItem Text="是" Value="1" />
                        </f:DropDownList>

                        <f:Button ID="btnSearch" MarginLeft="20px" runat="server" OnClick="btnSearch_Click" Icon="Find" Text="查询"></f:Button>
                    </Items>
                </f:Panel>
                <f:Grid ID="Grid1" runat="server" BoxFlex="1" ShowBorder="true" ShowHeader="false"
                    EnableCheckBoxSelect="false" DataKeyNames="ID,Name" AllowSorting="true" OnSort="Grid1_Sort" SortField="ID"
                    SortDirection="ASC" AllowPaging="true" IsDatabasePaging="true" OnPageIndexChange="Grid1_PageIndexChange"
                    OnRowCommand="Grid1_RowCommand" >
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
                        <f:BoundField TextAlign="Center" DataField="SendContent" Width="100px" HeaderText="发送内容" />
                        <f:BoundField TextAlign="Center" DataField="SendTime" SortField="SendTime" Width="200px" HeaderText="发送时间" />
                        <f:BoundField TextAlign="Center" DataField="IsBack" SortField="IsBack" Width="120px" HeaderText="是否回复" />
                        <%--<f:TemplateField Width="80px" HeaderText="是否回复">
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Eval("IsBack").ToString() == "1"?"是":"否" %>'></asp:Label>
                            </ItemTemplate>
                        </f:TemplateField>--%>
                        <f:LinkButtonField  ExpandUnusedSpace="true" TextAlign="Center" HeaderText="查看" Icon="Information" ToolTip="查看回复列表"
                            CommandName="viewDetail" Width="80px" />
                    </Columns>
                </f:Grid>
            </Items>
        </f:Panel>
        <f:Window ID="Window1" runat="server" IsModal="true" Hidden="true" Target="Top" EnableResize="true"
            EnableMaximize="true" EnableIFrame="true" IFrameUrl="about:blank" Width="1200px"
            Height="600px" OnClose="Window1_Close">
        </f:Window>
    </form>
</body>
</html>
