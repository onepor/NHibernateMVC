<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ContractPaySure.aspx.cs" Inherits="ZAJCZN.MIS.Web.ContractPaySure" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server" />
        <f:Panel ID="Panel1" ShowBorder="false" ShowHeader="false" runat="server">
            <Toolbars>
                <f:Toolbar ID="Toolbar1" runat="server">
                    <Items>
                        <f:Button ID="btnClose" Icon="SystemClose" EnablePostBack="false" runat="server"
                            Text="关闭">
                        </f:Button>
                        <f:ToolbarSeparator ID="ToolbarSeparator2" runat="server">
                        </f:ToolbarSeparator>
                        <f:Button ID="btnSaveClose" ValidateForms="SimpleForm1" Icon="SystemSaveClose" OnClick="btnSaveClose_Click"
                            runat="server" Text="保存">
                        </f:Button>                       
                    </Items>
                </f:Toolbar>
            </Toolbars>
            <Items>
                <f:GroupPanel ID="GroupPanel1" runat="server" Title="基本信息">
                    <Items>
                        <f:Form ID="SimpleForm1" ShowBorder="false" ShowHeader="false" runat="server" BodyPadding="2px"
                            Title="SimpleForm">
                            <Rows>
                                <f:FormRow ID="FormRow2" runat="server">
                                    <Items>
                                        <f:DropDownList ID="ddlDirection" LabelWidth="100px" LabelAlign="Right"
                                            Label="单据类型" runat="server" Required="true" ShowRedStar="true" Readonly="true">
                                            <f:ListItem Text="收款" Value="1" Selected="true" />
                                            <f:ListItem Text="付款" Value="2" />
                                        </f:DropDownList>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow ID="FormRow4" runat="server">
                                    <Items>
                                        <f:DatePicker ID="dpStartDate" LabelWidth="100px" Label="支付时间"
                                            runat="server" LabelAlign="Right">
                                        </f:DatePicker>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow ID="FormRow1" runat="server">
                                    <Items>
                                        <f:DropDownList ID="ddlPayType" LabelWidth="100px" ShowRedStar="true" LabelAlign="Right"
                                            Label="付款方式" DataTextField="EnumKey" DataValueField="EnumKey"
                                            DataSimulateTreeLevelField="Level" Required="true" runat="server">
                                        </f:DropDownList>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow ID="FormRow3" runat="server">
                                    <Items>
                                        <f:NumberBox ID="nbHeight" LabelWidth="100px" LabelAlign="Right" runat="server" NoDecimal="false"
                                            Label="金额(元)" Text="0" DecimalPrecision="2" MinValue="0">
                                        </f:NumberBox>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow runat="server">
                                    <Items>
                                        <f:TextArea ID="taRemark" LabelWidth="100px" LabelAlign="Right" runat="server" Label="备注"
                                            AutoGrowHeight="true" AutoGrowHeightMin="100" AutoGrowHeightMax="600">
                                        </f:TextArea>
                                    </Items>
                                </f:FormRow>
                            </Rows>
                        </f:Form>
                    </Items>
                </f:GroupPanel>
            </Items>
        </f:Panel>
        <f:Window ID="Window1" Title="编辑" Hidden="true" EnableIFrame="true" runat="server"
            EnableMaximize="true" EnableResize="true" Target="Top" IsModal="True" Width="550px"
            Height="350px">
        </f:Window>
    </form>
</body>
</html>
