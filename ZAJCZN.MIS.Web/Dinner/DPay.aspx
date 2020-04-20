<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DPay.aspx.cs" Inherits="ZAJCZN.MIS.Web.DPay" %>

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
            ShowHeader="false" Title="付款">
            <Items>
                <f:RadioButtonList ID="rblPayType" AutoPostBack="true" OnSelectedIndexChanged="rblPayType_SelectedIndexChanged"
                    runat="server" Label="付款方式" LabelWidth="70">
                    <f:RadioItem Text="正常支付" Value="1" Selected="true" />
                    <f:RadioItem Text="消费免单" Value="2" />
                    <f:RadioItem Text="消费挂账" Value="3" />
                </f:RadioButtonList>
                <f:DatePicker ID="dltCloseTabie" runat="server" Label="付款时间" LabelWidth="70"
                    DateFormatString="yyyy-MM-dd HH:mm:ss" Width="230px" AutoPostBack="true"
                    OnTextChanged="dltCloseTabie_TextChanged">
                </f:DatePicker>
                <f:GroupPanel ID="GPconsume" Title="消费信息" runat="server">
                    <Items>
                        <f:Form ID="SimpleForm1" ShowBorder="false" ShowHeader="false" runat="server"
                            Title="SimpleForm">
                            <Rows>
                                <f:FormRow>
                                    <Items>
                                        <f:Label ID="labMoneys" LabelWidth="70px" Label="账单金额" runat="server"></f:Label>
                                        <f:Label ID="labPrePrice" LabelWidth="70px" Label="赠送金额" runat="server"></f:Label>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow>
                                    <Items>
                                        <f:DropDownList ID="ddlDiscount" Label="折扣" LabelWidth="45px" runat="server">
                                            <f:ListItem Text="无折扣" Value="1" Selected="true" />
                                            <f:ListItem Text="95折" Value="0.95" />
                                            <f:ListItem Text="88折" Value="0.88" />
                                            <f:ListItem Text="85折" Value="0.85" />
                                            <f:ListItem Text="75折" Value="0.75" />
                                        </f:DropDownList>
                                        <f:TextBox ID="tbxKey" runat="server" EnableBlurEvent="true" OnBlur="tbxKey_Blur" LabelWidth="60px" Label="授权码"></f:TextBox>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow>
                                    <Items>
                                        <f:NumberBox ID="txtDiscount" runat="server" EnableBlurEvent="true" OnBlur="tbxDiscount_Blur" Text="0" LabelWidth="70px" Label="折让金额"></f:NumberBox>
                                        <f:Label ID="labFactPrice" CssStyle="color: red" LabelWidth="70px" Label="应付金额" runat="server"></f:Label>
                                    </Items>
                                </f:FormRow>
                            </Rows>
                        </f:Form>
                    </Items>
                </f:GroupPanel>
                <f:GroupPanel ID="GPvip" Title="会员卡信息" runat="server">
                    <Items>
                        <f:TextBox ID="txtVip" runat="server" EnableBlurEvent="true" OnBlur="txtVip_Blur"
                            LabelWidth="90px" Label="会员手机号">
                        </f:TextBox>
                        <f:Label runat="server" ID="lblVipMoney" Label="可用余额"></f:Label>
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
                                        <f:NumberBox runat="server" ID="nbWX" LabelWidth="70px" EnableBlurEvent="true" OnBlur="nbWX_Blur" Label="微信" Text="0"></f:NumberBox>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow>
                                    <Items>
                                        <f:NumberBox runat="server" ID="nbZFB" LabelWidth="70px" EnableBlurEvent="true" OnBlur="nbZFB_Blur" Label="支付宝" Text="0"></f:NumberBox>
                                        <f:NumberBox runat="server" ID="nbML" LabelWidth="70px" Label="抹零" Text="0"></f:NumberBox>
                                    </Items>
                                </f:FormRow>
                            </Rows>
                        </f:Form>
                        <f:Label CssStyle="font-size:17px;color: red" Hidden="true" runat="server" ID="labChange" Label="抹零"></f:Label>
                    </Items>
                </f:GroupPanel>
                <f:GroupPanel ID="GPFreeReason" Title="免单" runat="server">
                    <Items>
                        <f:DropDownList runat="server" Label="原因" LabelWidth="44px" DataTextField="AbnormalName" DataValueField="AbnormalName" ID="ddlFreeReason"></f:DropDownList>
                    </Items>
                </f:GroupPanel>
                <f:GroupPanel ID="GPChargeReason" Title="挂账" runat="server">
                    <Items>
                        <f:DropDownList runat="server" Label="挂账人" LabelWidth="60px" DataTextField="ChargeName" DataValueField="ID" ID="ddlChargeReason">
                        </f:DropDownList>
                        <f:TextBox runat="server" Label="挂账人" LabelWidth="60px" ID="txbChargeReason"></f:TextBox>
                    </Items>
                </f:GroupPanel>
                <f:Form runat="server" ShowBorder="false" ShowHeader="false" MarginTop="10px">
                    <Toolbars>
                        <f:Toolbar ToolbarAlign="Center" Position="Bottom" runat="server">
                            <Items>
                                <f:Button ID="btnSave" Width="160px"  Height="50px" CssStyle="font-size:17px" Icon="Money" runat="server" Text="确认付款信息" OnClick="btnSave_Click"></f:Button>
                                <f:Label Hidden="true" ID="labPayState" runat="server" Text="在线支付成功"></f:Label>
                                <f:Image Hidden="true" ID="imgPayState" runat="server"></f:Image>
                            </Items>
                        </f:Toolbar>
                    </Toolbars>
                </f:Form>
            </Items>
        </f:Panel>
        <f:HiddenField runat="server" ID="hfdPayType"></f:HiddenField>
        <f:HiddenField runat="server" ID="hfdPayTime"></f:HiddenField>
        <f:Window ID="WindowOnlinePay" runat="server" IsModal="true" Hidden="true" Target="Top" EnableResize="true"
            EnableMaximize="true" EnableIFrame="true" IFrameUrl="about:blank" Title="支付条码" Width="260px"
            Height="140px" OnClose="WindowOnlinePay_Close">
        </f:Window>
    </form>
</body>
</html>
