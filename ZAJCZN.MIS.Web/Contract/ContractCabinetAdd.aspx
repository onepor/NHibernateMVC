<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ContractCabinetAdd.aspx.cs" Inherits="ZAJCZN.MIS.Web.ContractCabinetAdd" %>

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
                                        <f:TextBox ID="txtAddress" LabelWidth="100px" LabelAlign="Right"
                                            runat="server" Label="柜体位置" Required="true" ShowRedStar="true">
                                        </f:TextBox>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow ID="FormRow1" runat="server">
                                    <Items>
                                        <f:TextBox ID="txtCostName" LabelWidth="100px" LabelAlign="Right"
                                            runat="server" Label="商品名称" Required="true" ShowRedStar="true">
                                        </f:TextBox>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow ID="FormRow3" runat="server">
                                    <Items>
                                        <f:NumberBox ID="nbHeight" LabelWidth="100px" LabelAlign="Right" runat="server" NoDecimal="false"
                                            Label="高度(mm)" Text="0" DecimalPrecision="2">
                                        </f:NumberBox>
                                        <f:NumberBox ID="nbWide" LabelWidth="100px" LabelAlign="Right" runat="server" NoDecimal="false"
                                            Label="宽度(mm)" Text="0" DecimalPrecision="2">
                                        </f:NumberBox>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow ID="FormRow6" runat="server">
                                    <Items>
                                        <f:NumberBox ID="nbCount" LabelWidth="100px" LabelAlign="Right" runat="server" NoDecimal="false"
                                            Label="数量(块)" MinValue="0" Text="1" DecimalPrecision="2">
                                        </f:NumberBox>
                                        <f:NumberBox ID="nbPrice" LabelWidth="100px" LabelAlign="Right" runat="server" NoDecimal="false"
                                            Label="单价(元)" Required="true" ShowRedStar="true" MinValue="0" Text="0" DecimalPrecision="2">
                                        </f:NumberBox>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow ID="FormRow4" runat="server">
                                    <Items>
                                        <f:TextBox ID="tbColor1" LabelWidth="100px" LabelAlign="Right"
                                            runat="server" Label="柜体颜色">
                                        </f:TextBox>
                                        <f:TextBox ID="tbColor2" LabelWidth="100px" LabelAlign="Right"
                                            runat="server" Label="门板颜色">
                                        </f:TextBox>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow ID="FormRow7" runat="server">
                                    <Items>
                                        <f:DropDownList ID="ddlSupply" LabelWidth="100px" ShowRedStar="true" LabelAlign="Right"
                                            Label="供应厂家" DataTextField="SupplierName" DataValueField="ID"
                                            Required="true" runat="server">
                                        </f:DropDownList>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow runat="server">
                                    <Items>
                                        <f:TextBox ID="txtRemark" LabelWidth="100px" LabelAlign="Right" runat="server"
                                            Label="备注">
                                        </f:TextBox>
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
