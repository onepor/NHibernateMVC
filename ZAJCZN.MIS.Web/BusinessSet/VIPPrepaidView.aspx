<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VIPPrepaidView.aspx.cs" Inherits="ZAJCZN.MIS.Web.VIPPrepaidView" %>

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
            ShowHeader="false" Title="会员卡充值记录">
            <Items>
                <f:Form ID="Form2" runat="server" Height="36px" BodyPadding="5px" ShowHeader="false"
                    ShowBorder="false" LabelAlign="Right">
                    <Rows>
                        <f:FormRow ID="FormRow1" runat="server">
                            <Items>
                                <f:DatePicker LabelWidth="50px" Label="从" runat="server" ID="dpkSatrt"></f:DatePicker>
                                <f:DatePicker LabelWidth="50px" Label="至" runat="server" ID="dpkEnd"></f:DatePicker>
                                <f:DropDownList runat="server" ID="ddlPayWay">
                                    <f:ListItem Text="--全部方式--" Value="0"/>
                                    <f:ListItem Text="刷卡" Value="1" />
                                    <f:ListItem Text="现金" Value="2" />
                                    <f:ListItem Text="微信" Value="3"  />
                                    <f:ListItem Text="支付宝" Value="4" />
                                </f:DropDownList>
                                <f:Button runat="server" Icon="SystemSearch" Text="查询" ID="btnSearch" EnablePostBack="true" OnClick="btnSearch_Click"></f:Button>
                            </Items>
                        </f:FormRow>
                    </Rows>
                </f:Form>
                <f:Grid ID="Grid1" runat="server" BoxFlex="1" ShowBorder="true" ShowHeader="false"
                    DataKeyNames="ID,Name" AllowSorting="true" OnSort="Grid1_Sort" SortField="PrepaidDate"
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
                        <f:RowNumberField TextAlign="Center" Width="40px" EnablePagingNumber="true" />
                        <f:TemplateField TextAlign="Center" Width="100px" SortField="VipID" HeaderText="会员">
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Text='<%# GetVipName(Eval("VipID").ToString()) %>'></asp:Label>
                            </ItemTemplate>
                        </f:TemplateField>
                        <f:BoundField TextAlign="Center" DataField="VIPPhone" SortField="VIPPhone" Width="100px" HeaderText="会员电话" />
                        <f:BoundField TextAlign="Center" DataField="PrepaidDate" SortField="PrepaidDate" Width="160px" HeaderText="充值时间" />
                        <f:BoundField TextAlign="Center" DataField="PrepaidAmount" SortField="PrepaidAmount" Width="120px" HeaderText="充值金额" />
                        <f:BoundField TextAlign="Center" DataField="PresentationAmount" SortField="PresentationAmount" Width="120px" HeaderText="赠送金额" />
                        <f:TemplateField TextAlign="Center" Width="100px" SortField="PrepaidWay" HeaderText="支付方式">
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# GetType(Eval("PrepaidWay").ToString()) %>'></asp:Label>
                            </ItemTemplate>
                        </f:TemplateField>
                        <f:BoundField TextAlign="Center" DataField="Operator" SortField="Operator" Width="90px" HeaderText="操作员" />
                    </Columns>
                </f:Grid>
            </Items>
        </f:Panel>
    </form>
</body>
</html>
