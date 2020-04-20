<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PartsEdit.aspx.cs" Inherits="ZAJCZN.MIS.Web.PartsEdit" %>

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
                            runat="server" Text="保存后关闭">
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
                                        <f:DropDownList ID="ddlCostType" LabelWidth="120px" ShowRedStar="true" LabelAlign="Right"
                                            Label="配件分类" DataTextField="TypeName" DataValueField="ID"
                                            DataSimulateTreeLevelField="Level" Required="true" runat="server"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddlCostType_SelectedIndexChanged">
                                        </f:DropDownList>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow ID="FormRow1" runat="server">
                                    <Items>
                                        <f:TextBox ID="txtCostName" LabelWidth="120px" LabelAlign="Right"
                                            runat="server" Label="配件名称" Required="true" ShowRedStar="true">
                                        </f:TextBox>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow ID="FormRow3" runat="server">
                                    <Items>
                                        <f:DropDownList ID="ddlUnit" LabelWidth="120px" LabelAlign="Right" Label="配件单位"
                                            Required="true" ShowRedStar="true" DataTextField="EnumKey" DataValueField="EnumValue"
                                            runat="server">
                                        </f:DropDownList>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow ID="FormRow4" runat="server">
                                    <Items>
                                        <f:NumberBox ID="nbPrice" LabelWidth="120px" LabelAlign="Right" runat="server" NoDecimal="false"
                                            Label="单价(元)" Required="true" ShowRedStar="true" MinValue="0" Text="1" DecimalPrecision="2">
                                        </f:NumberBox>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow ID="FormRow5" runat="server">
                                    <Items>
                                        <f:NumberBox ID="nbCost" LabelWidth="120px" LabelAlign="Right" runat="server" NoDecimal="false"
                                            Label="成本价(元)" Required="true" ShowRedStar="true" MinValue="0" Text="1" DecimalPrecision="2">
                                        </f:NumberBox>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow ID="FormRow8" runat="server">
                                    <Items>
                                        <f:RadioButtonList LabelWidth="120px" LabelAlign="Right" ID="ddlIsUsed" Label="是否启用"
                                            Required="true" ShowRedStar="true" runat="server">
                                            <f:RadioItem Text="是" Value="1" Selected="true" />
                                            <f:RadioItem Text="否" Value="0" />
                                        </f:RadioButtonList>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow runat="server">
                                    <Items>
                                        <f:TextBox ID="txtRemark" LabelWidth="120px" LabelAlign="Right" runat="server"
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
