<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OnlinePayInfoList.aspx.cs" Inherits="ZAJCZN.MIS.Web.OnlinePayInfoList" %>

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
            ShowHeader="false" Title="在线支付记录">
            <Items>
                <f:Panel ID="Panel2" ShowHeader="false" ShowBorder="false" Layout="Column" runat="server">
                    <Items>
                        <f:DatePicker ID="dpStartDate" LabelWidth="70px" Label="起止时间" Width="260px" runat="server" LabelAlign="Right">
                        </f:DatePicker>
                        <f:DatePicker ID="dpEndDate" LabelWidth="10px" Label="~" Width="150px" runat="server"
                            CompareControl="dpStartDate" CompareOperator="GreaterThan" CompareMessage="结束日期应该大于开始日期！">
                        </f:DatePicker>
                        <f:DropDownList MarginLeft="10px" runat="server" ID="ddlPayType">
                            <f:ListItem Text="--请选择--" Value="0" />
                            <f:ListItem Text="微信" Value="010"/>
                            <f:ListItem Text="支付宝" Value="020" />
                        </f:DropDownList>
                        <f:Button ID="btnSearch" MarginLeft="20px" runat="server" OnClick="btnSearch_Click" EnableAjax="false" EnablePostBack="true" Icon="SystemSearch" Text="查询"></f:Button>
                    </Items>
                </f:Panel>
                <f:Grid ID="Grid1" runat="server" BoxFlex="1" ShowBorder="true" ShowHeader="false" 
                    EnableCheckBoxSelect="false" DataKeyNames="ID" AllowSorting="true"  SortField="end_time"
                    SortDirection="ASC" AllowPaging="true" IsDatabasePaging="true" 
                    OnPageIndexChange="Grid1_PageIndexChange">
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
                        <f:TemplateField TextAlign="Center" Width="75px" HeaderText="支付方式">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# GetPayType(Eval("pay_type").ToString()) %>'></asp:Label>
                            </ItemTemplate>
                        </f:TemplateField>
                        <f:TemplateField TextAlign="Center" Width="100px" HeaderText="支付金额(元)">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# decimal.Parse(Eval("total_fee").ToString())/100 %>'></asp:Label>
                            </ItemTemplate>
                        </f:TemplateField>
                        <f:TemplateField TextAlign="Center" Width="140px" HeaderText="支付时间">
                            <ItemTemplate>
                                <asp:Label runat="server" Text='<%# GetTime(Eval("end_time").ToString()) %>'></asp:Label>
                            </ItemTemplate>
                        </f:TemplateField>
                        <f:BoundField TextAlign="Center" DataField="terminal_trace"  Width="150px" HeaderText="用餐订单号" />
                        <f:BoundField TextAlign="Center" DataField="out_trade_no"  Width="150px" HeaderText="利楚单号" />
                        <f:BoundField TextAlign="Center" DataField="channel_trade_no"  Width="150px" HeaderText="通道订单号" />
                        <f:BoundField TextAlign="Center" DataField="channel_order_no"  Width="150px" HeaderText="银行渠道订单号" />
                    </Columns>
                </f:Grid>
            </Items>
        </f:Panel>
    </form>
</body>
</html>
