<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VIPPrepaidManage.aspx.cs" Inherits="ZAJCZN.MIS.Web.VIPPrepaidManage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server" />
        <f:Panel ID="Panel1" runat="server" ShowBorder="false" Layout="VBox"
            ShowHeader="false" Title="会员充值管理" BodyPadding="0 10px 0 5px">
            <Items>
                <f:GroupPanel runat="server" ID="GroupPanel2" Title="会员卡充值">
                    <Toolbars>
                        <f:Toolbar runat="server" Position="Bottom" ToolbarAlign="Center">
                            <Items>
                                <f:Button runat="server" Height="30px" CssStyle="font-size:20px" ID="btnSure" OnClick="btnSure_Click" Text="确定充值" Icon="Accept"></f:Button>
                                <f:Button Height="30px" CssStyle="font-size:20px" Hidden="true" Icon="ApplicationViewList" Text="查看充值记录" runat="server" ID="btnSearch" OnClick="btnSearch_Click"></f:Button>
                                <f:SimpleForm ID="SimpleForm1" runat="server" ShowBorder="false" ShowHeader="false" Layout="HBox">
                                    <Items>
                                        <f:Label ID="labPayState" runat="server"></f:Label>
                                        <f:Image ID="imgPayState" runat="server"></f:Image>
                                    </Items>
                                </f:SimpleForm>
                            </Items>
                        </f:Toolbar>
                    </Toolbars>
                    <Items>
                        <f:Form ShowHeader="false" ShowBorder="false" runat="server">
                            <Items>
                                <f:FormRow>
                                    <Items>
                                        <f:RadioButtonList Label="支付方式" LabelWidth="110px"
                                            LabelAlign="Right" ID="cblPayWay" runat="server">
                                            <f:RadioItem Text="刷卡" Value="1" />
                                            <f:RadioItem Text="现金" Value="2" />
                                            <f:RadioItem Text="微信" Value="3" Selected="true" />
                                            <f:RadioItem Text="支付宝" Value="4" />
                                        </f:RadioButtonList>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow>
                                    <Items>
                                        <f:NumberBox runat="server" MaxLength="11" MaxLengthMessage="充值金额超出限额" ID="tbxMoneys" Label="请输入充值金额" LabelAlign="Right" LabelWidth="110px"></f:NumberBox>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow>
                                    <Items>
                                        <f:NumberBox runat="server" ID="tbxFree" Label="赠送金额" LabelAlign="Right" LabelWidth="110px"></f:NumberBox>
                                    </Items>
                                </f:FormRow>
                            </Items>
                        </f:Form>
                    </Items>
                </f:GroupPanel>
            </Items>
        </f:Panel>
        <f:HiddenField runat="server" ID="hfdPayType"></f:HiddenField>
        <f:HiddenField runat="server" ID="hfdPayTime"></f:HiddenField>
        <f:Window ID="Window1" runat="server" IsModal="true" Hidden="true" Target="Top" EnableResize="true"
            EnableMaximize="true" EnableIFrame="true" Title="支付条码" IFrameUrl="about:blank" Width="260px"
            Height="140px" OnClose="Window1_Close">
        </f:Window>
        <f:Window ID="WindowPayRecord" runat="server" IsModal="true" Hidden="true" Target="Top" EnableResize="true"
            EnableMaximize="true" EnableIFrame="true" Title="支付条码" IFrameUrl="about:blank" Width="630px"
            Height="500px">
        </f:Window>
    </form>
</body>
</html>
