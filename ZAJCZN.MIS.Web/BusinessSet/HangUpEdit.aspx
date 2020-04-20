<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HangUpEdit.aspx.cs" Inherits="ZAJCZN.MIS.Web.HangUpEdit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server" />
        <f:Panel ID="Panel1" ShowBorder="false" ShowHeader="false"  runat="server">
            <Toolbars>
                <f:Toolbar ID="Toolbar1" runat="server">
                    <Items>
                        <f:Button ID="btnClose" Icon="SystemClose" EnablePostBack="false" runat="server"
                            Text="关闭">
                        </f:Button>
                        <f:ToolbarSeparator ID="ToolbarSeparator2" runat="server">
                        </f:ToolbarSeparator>
                        <f:Button ID="btnSaveClose" ValidateForms="SimpleForm1" Icon="SystemSaveClose" OnClick="btnSaveClose_Click"
                            runat="server" Text="保存后关闭">
                        </f:Button>
                    </Items>
                </f:Toolbar>
            </Toolbars>
            <Items>
                <f:Form ID="SimpleForm1" ShowBorder="false" ShowHeader="false" runat="server" BodyPadding="10px"
                    Title="SimpleForm">
                    <Rows>
                        <f:FormRow runat="server">
                            <Items>
                                <f:TextBox ID="txbPaymentName" runat="server" Label="支付方式名称" LabelWidth="110px" LabelAlign="Right" Required="true" ShowRedStar="true"></f:TextBox>
                                <f:NumberBox runat="server" ID="numProportion" Label="收入占比(%)" Hidden="true" LabelWidth="110px" LabelAlign="Right" ShowRedStar="true"></f:NumberBox>
                            </Items>
                        </f:FormRow>
                        <f:FormRow runat="server">
                            <Items>
                                <f:NumberBox runat="server" ID="numSort" Label="排序" LabelAlign="Right" LabelWidth="110px" Required="true" ShowRedStar="true"></f:NumberBox>
                            </Items>
                        </f:FormRow>
                        <f:FormRow ID="FormRow1" runat="server">
                            <Items>
                                <f:RadioButtonList ID="rbtnRecord" Label="是否记录券号" ShowRedStar="true" LabelWidth="110px" LabelAlign="Right" runat="server" Required="true">
                                    <f:RadioItem Text="是" Value="1" Selected="true" />
                                    <f:RadioItem Text="否" Value="0" />
                                </f:RadioButtonList>
                            </Items>
                        </f:FormRow>
                        <f:FormRow ID="FormRow2" runat="server">
                            <Items>
                                <f:RadioButtonList ID="radioIsVip" Label="用于会员充值" ShowRedStar="true" LabelWidth="110px" LabelAlign="Right" runat="server" Required="true">
                                    <f:RadioItem Text="是" Value="1" Selected="true" />
                                    <f:RadioItem Text="否" Value="0" />
                                </f:RadioButtonList>
                            </Items>
                        </f:FormRow>
                        <f:FormRow runat="server">
                            <Items>
                                <f:RadioButtonList ID="radioIsIntegral" Label="参与积分" ShowRedStar="true" LabelWidth="110px" LabelAlign="Right" runat="server" Required="true">
                                    <f:RadioItem Text="是" Value="1" Selected="true" />
                                    <f:RadioItem Text="否" Value="0" />
                                </f:RadioButtonList>
                            </Items>
                        </f:FormRow>
                        <f:FormRow ID="FormRow3" runat="server">
                            <Items>
                                <f:RadioButtonList ID="radioIsUsed" Label="是否启用" ShowRedStar="true" LabelWidth="110px" LabelAlign="Right" runat="server" Required="true">
                                    <f:RadioItem Text="是" Value="1" Selected="true" />
                                    <f:RadioItem Text="否" Value="0" />
                                </f:RadioButtonList>
                            </Items>
                        </f:FormRow>
                        <f:FormRow Hidden="true" runat="server">
                            <Items>
                                <f:NumberBox runat="server" Label="抵用规则"></f:NumberBox>
                                <f:NumberBox runat="server" Label="抵用规则"></f:NumberBox>
                            </Items>
                        </f:FormRow>
                    </Rows>
                </f:Form>
            </Items>
        </f:Panel>
    </form>
</body>
</html>
