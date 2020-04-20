<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GoodsEdit.aspx.cs" Inherits="ZAJCZN.MIS.Web.GoodsEdit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server" />
        <f:Panel ID="Panel1" ShowBorder="false" ShowHeader="false" AutoScroll="true" runat="server">
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
                                            Label="物品分类" DataTextField="Name" DataValueField="Id" EnableSimulateTree="true"
                                            DataSimulateTreeLevelField="Level" Required="true"
                                            runat="server">
                                        </f:DropDownList>
                                        <f:TextBox ID="txtCostName" LabelWidth="120px" LabelAlign="Right"
                                            runat="server" Label="物品名称" Required="true" ShowRedStar="true">
                                        </f:TextBox>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow ID="FormRow1" runat="server">
                                    <Items>
                                        <f:Label ID="lblCode" LabelWidth="120px" LabelAlign="Right" runat="server" Label="数字助记码">
                                        </f:Label>
                                        <f:Label ID="lblPY" LabelWidth="120px" LabelAlign="Right" runat="server" Label="拼音助记码">
                                        </f:Label>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow ID="FormRow9" runat="server">
                                    <Items>
                                        <f:TextBox ID="txtGoodsBarCode" LabelWidth="120px" LabelAlign="Right"
                                            runat="server" Label="物品条码">
                                        </f:TextBox>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow ID="FormRow3" runat="server">
                                    <Items>
                                        <f:DropDownList ID="ddlUnit" LabelWidth="120px" LabelAlign="Right" Label="标准单位"
                                            DataTextField="EnumKey" DataValueField="EnumValue" Required="true" ShowRedStar="true"
                                            runat="server" OnSelectedIndexChanged="ddlUnit_SelectedIndexChanged" AutoPostBack="true">
                                        </f:DropDownList>
                                        <f:DropDownList ID="ddlTaxPoint" LabelWidth="120px" LabelAlign="Right" Label="税率"
                                            DataTextField="ParaName" DataValueField="ID" Required="true" ShowRedStar="true"
                                            runat="server">
                                        </f:DropDownList>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow ID="FormRow4" runat="server">
                                    <Items>
                                        <f:NumberBox ID="txtPrice" LabelWidth="120px" LabelAlign="Right" runat="server" Label="物品单价">
                                        </f:NumberBox>
                                        <f:TextBox ID="txtFormat" LabelWidth="120px" LabelAlign="Right" runat="server" Label="规格型号">
                                        </f:TextBox>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow runat="server">
                                    <Items>
                                        <f:DropDownList ID="ddlPD" LabelWidth="120px" LabelAlign="Right" Label="盘点类型"
                                            DataTextField="EnumKey" DataValueField="EnumValue" Required="true" ShowRedStar="true"
                                            runat="server">
                                        </f:DropDownList>
                                        <f:RadioButtonList LabelWidth="120px" LabelAlign="Right" ID="ddlIsUsed" Label="是否启用"
                                            Required="true" ShowRedStar="true" runat="server">
                                            <f:RadioItem Text="是" Value="2" Selected="true" />
                                            <f:RadioItem Text="否" Value="1" />
                                        </f:RadioButtonList>
                                    </Items>
                                </f:FormRow>
                            </Rows>
                        </f:Form>
                    </Items>
                </f:GroupPanel>
                <f:GroupPanel ID="GroupPanel2" runat="server" Title="单位换算">
                    <Items>
                        <f:Form ID="Form2" ShowBorder="false" ShowHeader="false" runat="server" BodyPadding="2px"
                            Title="SimpleForm">
                            <Rows>

                                <f:FormRow ID="FormRow8" runat="server">
                                    <Items>
                                        <f:Label ID="lblUnit" LabelWidth="120px" LabelAlign="Right" runat="server" Label="标准单位">
                                        </f:Label>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow ID="FormRow5" runat="server">
                                    <Items>
                                        <f:DropDownList ID="ddlConsumeUnit" LabelAlign="Right" ShowLabel="true" Label="消耗单位"
                                            DataTextField="EnumKey" DataValueField="EnumValue" Required="true" ShowRedStar="true"
                                            runat="server">
                                        </f:DropDownList>
                                        <f:NumberBox ID="txtConsumNum" LabelAlign="Right" LabelWidth="180px" Text="1" runat="server"
                                            Required="true" Label="1标准单位=消耗单位">
                                        </f:NumberBox>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow ID="FormRow6" runat="server">
                                    <Items>
                                        <f:DropDownList ID="ddlPurchaseUnit" LabelAlign="Right" ShowLabel="true" Label="采购单位"
                                            DataTextField="EnumKey" DataValueField="EnumValue" Required="true" ShowRedStar="true"
                                            runat="server">
                                        </f:DropDownList>
                                        <f:NumberBox ID="txtPurchaseNum" LabelWidth="180px" LabelAlign="Right" Text="1" runat="server"
                                            Required="true" Label="1采购单位=标准单位">
                                        </f:NumberBox>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow ID="FormRow7" runat="server" Hidden="true">
                                    <Items>
                                        <f:NumberBox ID="txtOrderNum" LabelWidth="180px" LabelAlign="Right" Text="1" runat="server"
                                            Required="true" Label="标准与订货换算值=">
                                        </f:NumberBox>
                                        <f:DropDownList ID="ddlOrderUnit" LabelAlign="Right" ShowLabel="false"
                                            DataTextField="EnumKey" DataValueField="EnumValue" Required="true" ShowRedStar="true"
                                            runat="server">
                                        </f:DropDownList>
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
