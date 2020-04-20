<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RepairProjectEdit.aspx.cs" Inherits="ZAJCZN.MIS.Web.RepairProjectEdit" %>

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
                <f:Form ID="SimpleForm1" ShowBorder="false" ShowHeader="false" runat="server" BodyPadding="10px"
                    Title="SimpleForm">
                    <Rows>
                        <f:FormRow runat="server">
                            <Items>
                                <f:TextBox ID="txtVipName" Label="费用项名称" LabelWidth="120px" ShowRedStar="true" LabelAlign="Right" runat="server" Required="true">
                                </f:TextBox>
                            </Items>
                        </f:FormRow>
                        <f:FormRow ID="FormRow3" runat="server">
                            <Items>
                                <f:DropDownList ID="ddlUnit" LabelWidth="120px" LabelAlign="Right" Label="计费单位"
                                    DataTextField="EnumKey" DataValueField="EnumValue" Required="true" ShowRedStar="true"
                                    runat="server">
                                </f:DropDownList>
                            </Items>
                        </f:FormRow>
                        <f:FormRow runat="server">
                            <Items>
                                <f:NumberBox ID="txtDailyRents" LabelWidth="120px" LabelAlign="Right" runat="server" NoDecimal="false"
                                    Label="单价(元)" Required="true" ShowRedStar="true" MinValue="0" Text="1" DecimalPrecision="3">
                                </f:NumberBox>
                            </Items>
                        </f:FormRow>
                        <f:FormRow ID="FormRow5" runat="server">
                            <Items>
                                <f:DropDownList ID="ddlContractPrice" runat="server" LabelWidth="120px" LabelAlign="Right" Label="合同价格"
                                    Required="true" ShowRedStar="true" AutoPostBack="true" OnSelectedIndexChanged="ddlContractPrice_SelectedIndexChanged">
                                    <f:ListItem Text="不参考合同" Value="0" Selected="true" />
                                    <f:ListItem Text="合同客户运费" Value="1" />
                                    <f:ListItem Text="合同司机运费" Value="2" />
                                    <f:ListItem Text="合同物品单价" Value="3" />
                                    <f:ListItem Text="合同维修单价" Value="4" />
                                </f:DropDownList>
                            </Items>
                        </f:FormRow>
                        <f:FormRow ID="FormRow1" runat="server">
                            <Items>
                                <f:DropDownList ID="ddlType" runat="server" LabelWidth="120px" LabelAlign="Right" Label="费用类型"
                                    Required="true" ShowRedStar="true">
                                    <f:ListItem Text="员工费用" Value="1" Selected="true" />
                                    <f:ListItem Text="客户费用" Value="2" />
                                    <f:ListItem Text="司机费用" Value="3" />
                                </f:DropDownList>
                            </Items>
                        </f:FormRow>
                        <f:FormRow ID="FormRow2" runat="server">
                            <Items>
                                <f:DropDownList ID="ddlUsingType" runat="server" LabelWidth="120px" LabelAlign="Right" Label="使用类型"
                                    Required="true" ShowRedStar="true">
                                    <f:ListItem Text="场内" Value="1" Selected="true" />
                                    <f:ListItem Text="发货" Value="2" />
                                    <f:ListItem Text="收货" Value="3" />
                                </f:DropDownList>
                            </Items>
                        </f:FormRow>
                        <f:FormRow ID="FormRow8" runat="server">
                            <Items>
                                <f:RadioButtonList LabelWidth="120px" LabelAlign="Right" ID="rbtnIsCreateJob" Label="创建工单 "
                                    Required="true" ShowRedStar="true" runat="server">
                                    <f:RadioItem Text="是" Value="1" Selected="true" />
                                    <f:RadioItem Text="否" Value="0" />
                                </f:RadioButtonList>
                                <f:RadioButtonList LabelWidth="120px" LabelAlign="Right" ID="tbtnIsSorting" Label="二次分拣"
                                    Required="true" ShowRedStar="true" runat="server" AutoPostBack="true" OnSelectedIndexChanged="tbtnIsSorting_SelectedIndexChanged">
                                    <f:RadioItem Text="是" Value="1" Selected="true" />
                                    <f:RadioItem Text="否" Value="0" />
                                </f:RadioButtonList>
                            </Items>
                        </f:FormRow>
                        <f:FormRow ID="FormRow6" runat="server">
                            <Items>
                                <f:RadioButtonList LabelWidth="120px" LabelAlign="Right" ID="ddlIsRegular" Label="固定费用 "
                                    Required="true" ShowRedStar="true" runat="server">
                                    <f:RadioItem Text="是" Value="1" Selected="true" />
                                    <f:RadioItem Text="否" Value="0" />
                                </f:RadioButtonList>
                                <f:RadioButtonList LabelWidth="120px" LabelAlign="Right" ID="ddlIsUsed" Label="是否启用"
                                    Required="true" ShowRedStar="true" runat="server">
                                    <f:RadioItem Text="是" Value="1" Selected="true" />
                                    <f:RadioItem Text="否" Value="0" />
                                </f:RadioButtonList>
                            </Items>
                        </f:FormRow>
                        <f:FormRow runat="server">
                            <Items>
                                <f:TextBox ID="txtRemark" Label="备注" LabelAlign="Right" LabelWidth="120px" runat="server">
                                </f:TextBox>
                            </Items>
                        </f:FormRow>
                        <f:FormRow ID="FormRow4" runat="server">
                            <Items>
                                <f:CheckBoxList ID="cblGoods" Label="适用范围" ColumnNumber="3" runat="server">
                                </f:CheckBoxList>
                            </Items>
                        </f:FormRow>
                    </Rows>
                </f:Form>
            </Items>
        </f:Panel>
    </form>
</body>
</html>
