<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EquipmentEdit.aspx.cs" Inherits="ZAJCZN.MIS.Web.EquipmentEdit" %>

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
                                            Label="商品分类" DataTextField="TypeName" DataValueField="ID"
                                            DataSimulateTreeLevelField="Level" Required="true" runat="server"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddlCostType_SelectedIndexChanged">
                                        </f:DropDownList>
                                        <f:TextBox ID="txtCostName" LabelWidth="120px" LabelAlign="Right"
                                            runat="server" Label="商品名称" Required="true" ShowRedStar="true">
                                        </f:TextBox>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow ID="FormRow3" runat="server">
                                    <Items>
                                        <f:DropDownList ID="ddlUnit" LabelWidth="120px" LabelAlign="Right" Label="商品单位"
                                            Required="true" ShowRedStar="true"
                                            runat="server">
                                            <f:ListItem Text="樘" Value="1" Selected="true" />
                                            <f:ListItem Text="米" Value="2" />
                                            <f:ListItem Text="平方米" Value="3" />
                                        </f:DropDownList>
                                        <f:DropDownList ID="ddlCalcUnitType" LabelWidth="120px" LabelAlign="Right" Label="计算方式"
                                            Required="true" ShowRedStar="true"
                                            runat="server">
                                            <f:ListItem Text="单位数量" Value="1" Selected="true" />
                                            <f:ListItem Text="三方周长" Value="2" />
                                            <f:ListItem Text="四方周长" Value="3" />
                                            <f:ListItem Text="面积" Value="4" />
                                        </f:DropDownList>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow ID="FormRow1" runat="server">
                                    <Items>
                                        <f:NumberBox ID="nbPrice" LabelWidth="120px" LabelAlign="Right" runat="server" NoDecimal="false"
                                            Label="单价(元)" Required="true" ShowRedStar="true" MinValue="0" Text="1" DecimalPrecision="2">
                                        </f:NumberBox>
                                        <f:NumberBox ID="nbInstall" LabelWidth="120px" LabelAlign="Right" runat="server" NoDecimal="false"
                                            Label="安装运费(元)" Required="true" ShowRedStar="true" MinValue="0" Text="1" DecimalPrecision="2">
                                        </f:NumberBox>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow ID="FormRow8" runat="server">
                                    <Items>
                                        <f:DropDownList ID="ddlCalcUnit" LabelWidth="120px" LabelAlign="Right" Label="超标计算"
                                            Required="true" ShowRedStar="true"
                                            runat="server">
                                            <f:ListItem Text="按公分计算" Value="1" Selected="true" />
                                            <f:ListItem Text="单价加价" Value="2" />
                                            <f:ListItem Text="面积加价" Value="3" />
                                        </f:DropDownList>
                                        <f:RadioButtonList LabelWidth="120px" LabelAlign="Right" ID="ddlIsUsed" Label="是否启用"
                                            Required="true" ShowRedStar="true" runat="server">
                                            <f:RadioItem Text="是" Value="1" Selected="true" />
                                            <f:RadioItem Text="否" Value="0" />
                                        </f:RadioButtonList>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow runat="server">
                                    <Items>
                                        <f:TextBox ID="txtLine" LabelWidth="120px" LabelAlign="Right" runat="server"
                                            Label="配套线条">
                                        </f:TextBox>
                                    </Items>
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
                <f:GroupPanel ID="GroupPanel2" runat="server" Title="尺寸规格">
                    <Items>
                        <f:Form ID="Form2" ShowBorder="false" ShowHeader="false" runat="server" BodyPadding="2px"
                            Title="SimpleForm">
                            <Rows>
                                <f:FormRow ID="FormRow4" runat="server">
                                    <Items>
                                        <f:NumberBox ID="nbHeght" LabelWidth="120px" LabelAlign="Right" runat="server"
                                            Label="标准高(MM)" Required="true" ShowRedStar="true" MinValue="0" Text="0">
                                        </f:NumberBox>
                                        <f:NumberBox ID="nbWide" LabelWidth="120px" LabelAlign="Right" runat="server"
                                            Label="标准宽(MM)" Required="true" ShowRedStar="true" MinValue="0" Text="0">
                                        </f:NumberBox>
                                        <f:NumberBox ID="nbThckness" LabelWidth="120px" LabelAlign="Right" runat="server"
                                            Label="标准厚(MM)" Required="true" ShowRedStar="true" MinValue="0" Text="0">
                                        </f:NumberBox>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow ID="FormRow5" runat="server">
                                    <Items>
                                        <f:NumberBox ID="nbPassHeght" LabelWidth="120px" LabelAlign="Right" runat="server" NoDecimal="false"
                                            Label="超高(元)" Required="true" ShowRedStar="true" MinValue="0" Text="0" DecimalPrecision="2">
                                        </f:NumberBox>
                                        <f:NumberBox ID="nbPassWide" LabelWidth="120px" LabelAlign="Right" runat="server" NoDecimal="false"
                                            Label="超宽(元)" Required="true" ShowRedStar="true" MinValue="0" Text="0" DecimalPrecision="2">
                                        </f:NumberBox>
                                        <f:NumberBox ID="nbPassTK" LabelWidth="120px" LabelAlign="Right" runat="server" NoDecimal="false"
                                            Label="超厚(元)" Required="true" ShowRedStar="true" MinValue="0" Text="0" DecimalPrecision="2">
                                        </f:NumberBox>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow ID="FormRow6" runat="server">
                                    <Items>
                                        <f:NumberBox ID="nbPassArea" LabelWidth="120px" LabelAlign="Right" runat="server" NoDecimal="false"
                                            Label="超面积(元)" Required="true" ShowRedStar="true" MinValue="0" Text="0" DecimalPrecision="2">
                                        </f:NumberBox>
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
