<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SettlementEdit.aspx.cs" Inherits="ZAJCZN.MIS.Web.SettlementEdit" %>

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
            ShowHeader="false" Title="修改交班信息">
            <Items>
                <f:GroupPanel ID="GroupPanel" Title="主要信息" runat="server">
                    <Items>
                        <f:Form ID="SimpleForm1" ShowBorder="false" ShowHeader="false" runat="server" BodyPadding="10px"
                            Title="SimpleForm">
                            <Rows>
                                <f:FormRow>
                                    <Items>
                                        <f:Label ID="labDate" LabelWidth="70px" Label="营业日期" runat="server"></f:Label>
                                        <f:Label ID="labSettlementDate" LabelWidth="70px" Label="结算时间" runat="server"></f:Label>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow>
                                    <Items>
                                        <f:Label ID="labCustomerCount" LabelWidth="70px" Label="全天客数" runat="server"></f:Label>
                                        <f:Label ID="labOrderCount" LabelWidth="70px" Label="全天接单" runat="server"></f:Label>
                                    </Items>
                                </f:FormRow>
                            </Rows>
                        </f:Form>
                    </Items>
                </f:GroupPanel>
                <f:GroupPanel ID="GPvip" Title="金额信息" runat="server">
                    <Items>
                        <f:Form ID="Form2" ShowBorder="false" ShowHeader="false" runat="server" BodyPadding="10px"
                            Title="SimpleForm">
                            <Rows>
                                <f:FormRow>
                                    <Items>
                                        <f:Label ID="labAmountReceivable" LabelWidth="70px" Label="应收金额" runat="server"></f:Label>
                                        <f:Label ID="labAmountCollected" LabelWidth="70px" Label="实付金额" runat="server"></f:Label>
                                        <f:Label ID="labDonationAmount" LabelWidth="70px" Label="赠送金额" runat="server"></f:Label>
                                        <f:Label ID="labDiscountAmount" LabelWidth="70px" Label="抹零金额" runat="server"></f:Label>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow>
                                    <Items>
                                        <f:Label ID="labDisCount" LabelWidth="70px" Label="折让金额" runat="server"></f:Label>
                                        <f:Label ID="labBackAmount" LabelWidth="70px" Label="退菜金额" runat="server"></f:Label>
                                        <f:Label ID="labSingleAmount" LabelWidth="70px" Label="免单金额" runat="server"></f:Label>
                                        <f:Label ID="labCharge" LabelWidth="70px" Label="挂账金额" runat="server"></f:Label>
                                    </Items>
                                </f:FormRow>
                            </Rows>
                        </f:Form>
                    </Items>
                </f:GroupPanel>
                <f:GroupPanel ID="GroupPanel1" Title="支付方式" runat="server">
                    <Items>
                        <f:Form runat="server" ShowBorder="false" ShowHeader="false" BodyPadding="10px"
                            Title="SimpleForm">
                            <Rows>
                                <f:FormRow>
                                    <Items>
                                        <f:Label ID="labPayWay" Text="付款方式" runat="server"></f:Label>
                                        <f:Label ID="labAmount" Text="票面金额" runat="server"></f:Label>
                                        <f:Label ID="labFactAmount" Text="实盘金额" runat="server"></f:Label>
                                        <f:Label ID="labLost" Text="溢缺" runat="server"></f:Label>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow>
                                    <Items>
                                        <f:Label ID="labCash" Text="现金金额" runat="server"></f:Label>
                                        <f:Label ID="labCashAmount" Text="0.00" runat="server"></f:Label>
                                        <f:TextBox ID="tbxCashFact" Text="0.00" OnTextChanged="tbxCashFact_TextChanged" runat="server"></f:TextBox>
                                        <f:Label ID="labCashLost" Text="0.00" runat="server"></f:Label>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow>
                                    <Items>
                                        <f:Label ID="labWX" Text="微信金额" runat="server"></f:Label>
                                        <f:Label ID="labWXAmount" Text="0.00" runat="server"></f:Label>
                                        <f:TextBox ID="tbxWXFact" Text="0.00" OnTextChanged="tbxWXFact_TextChanged" runat="server"></f:TextBox>
                                        <f:Label ID="labWXLost" Text="0.00" runat="server"></f:Label>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow>
                                    <Items>
                                        <f:Label ID="labZFB" Text="支付宝金额" runat="server"></f:Label>
                                        <f:Label ID="labZFBAmount" Text="0.00" runat="server"></f:Label>
                                        <f:TextBox ID="tbxZFBFact" Text="0.00" OnTextChanged="tbxZFBFact_TextChanged" runat="server"></f:TextBox>
                                        <f:Label ID="labZFBLost" Text="0.00" runat="server"></f:Label>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow>
                                    <Items>
                                        <f:Label ID="labCredit" Text="刷卡金额" runat="server"></f:Label>
                                        <f:Label ID="labCreditAmount" Text="0.00" runat="server"></f:Label>
                                        <f:TextBox ID="tbxCreditFact" Text="0.00" OnTextChanged="tbxCreditFact_TextChanged" runat="server"></f:TextBox>
                                        <f:Label ID="labCreditLost" Text="0.00" runat="server"></f:Label>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow>
                                    <Items>
                                        <f:Label ID="labVip" Text="会员抵扣" runat="server"></f:Label>
                                        <f:Label ID="labVipAmount" Text="0.00" runat="server"></f:Label>
                                        <f:TextBox ID="tbxVipFact" Text="0.00" OnTextChanged="tbxVipFact_TextChanged" runat="server"></f:TextBox>
                                        <f:Label ID="labVipLost" Text="0.00" runat="server"></f:Label>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow>
                                    <Items>
                                        <f:Label ID="labGroup" Text="团购卷金额" runat="server"></f:Label>
                                        <f:Label ID="labGroupAmount" Text="0.00" runat="server"></f:Label>
                                        <f:TextBox ID="tbxGroupFact" Text="0.00" OnTextChanged="tbxGroupFact_TextChanged" runat="server"></f:TextBox>
                                        <f:Label ID="labGroupLost" Text="0.00" runat="server"></f:Label>
                                    </Items>
                                </f:FormRow>
                            </Rows>
                        </f:Form>
                    </Items>
                </f:GroupPanel>
                <f:Form runat="server" ShowBorder="false" ShowHeader="false" MarginTop="5px">
                    <Toolbars>
                        <f:Toolbar ToolbarAlign="Center" Position="Bottom" runat="server">
                            <Items>
                                <f:Button ID="btnAccept" Width="100px" Height="35px" CssStyle="font-size:17px" Icon="accept" runat="server" Text="确认" OnClick="btnAccept_Click"></f:Button>
                            </Items>
                        </f:Toolbar>
                    </Toolbars>
                </f:Form>
            </Items>
        </f:Panel>
    </form>
</body>
</html>
