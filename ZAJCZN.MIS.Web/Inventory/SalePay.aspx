<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SalePay.aspx.cs" Inherits="ZAJCZN.MIS.Web.SalePay" %>

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
            ShowHeader="false" Title="付款">
            <Items>
                <f:GroupPanel ID="GPconsume" Title="消费信息" runat="server">
                    <Items>
                        <f:Form ID="SimpleForm1" ShowBorder="false" ShowHeader="false" runat="server" BodyPadding="10px"
                            Title="SimpleForm">
                            <Rows>
                                <f:FormRow>
                                    <Items>
                                        <f:Label ID="labMoneys" LabelWidth="70px" Label="账单金额" runat="server"></f:Label>
                                        <f:Label ID="labFactPrice" CssStyle="color: red" LabelWidth="70px" Label="应付金额" runat="server"></f:Label>
                                    </Items>
                                </f:FormRow>
                            </Rows>
                        </f:Form>
                    </Items>
                </f:GroupPanel>
                <f:GroupPanel ID="GPvip" Title="会员卡信息" runat="server">
                    <Items>
                        <f:Label runat="server" ID="lblvipID" Label="会员卡号" Text="15615615155"></f:Label>
                        <f:Label runat="server" ID="lblMoney" Label="可用余额"></f:Label>
                    </Items>
                </f:GroupPanel>
                <f:GroupPanel ID="GPPayWay" Title="支付方式" runat="server">
                    <Items>
                        <f:Form ShowBorder="false" ShowHeader="false" runat="server">
                            <Rows>
                                <f:FormRow>
                                    <Items>
                                        <f:NumberBox runat="server" ID="nbxVip" LabelWidth="70px" Label="会员卡" Text="0"></f:NumberBox>
                                        <f:NumberBox runat="server" ID="nbCash" LabelWidth="70px" Label="现金" Text="0"></f:NumberBox>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow>
                                    <Items>
                                        <f:NumberBox runat="server" ID="nbCard" LabelWidth="70px" Label="刷卡" Text="0"></f:NumberBox>
                                        <f:NumberBox runat="server" ID="nbWX" LabelWidth="70px" Label="微信" Text="0"></f:NumberBox>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow>
                                    <Items>
                                        <f:NumberBox runat="server" ID="nbZFB" LabelWidth="70px" Label="支付宝" Text="0"></f:NumberBox>
                                        <f:NumberBox runat="server" ID="nbML" LabelWidth="70px" Label="抹零" Text="0"></f:NumberBox>
                                    </Items>
                                </f:FormRow>
                            </Rows>
                        </f:Form>
                        <f:Label CssStyle="font-size:17px;color: red" Hidden="true" runat="server" ID="labChange" Label="抹零"></f:Label>
                    </Items>
                </f:GroupPanel>
                <f:Form runat="server" ShowBorder="false" ShowHeader="false" MarginTop="5px">
                    <Toolbars>
                        <f:Toolbar ToolbarAlign="Center" Position="Bottom" runat="server">
                            <Items>
                                <f:Button ID="Accept" Width="100px" Height="35px" CssStyle="font-size:17px" Icon="accept" runat="server" Text="确认" OnClick="Accept_Click"></f:Button>
                            </Items>
                        </f:Toolbar>
                    </Toolbars>
                </f:Form>
            </Items>
        </f:Panel>
    </form>
    <script>

</script>
</body>
</html>
