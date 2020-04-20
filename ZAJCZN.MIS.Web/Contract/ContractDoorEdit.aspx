<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ContractDoorEdit.aspx.cs" Inherits="ZAJCZN.MIS.Web.ContractDoorEdit" %>

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
                                        <f:TextBox ID="txtAddress" LabelWidth="100px" LabelAlign="Right"
                                            runat="server" Label="门位置" Required="true" ShowRedStar="true">
                                        </f:TextBox>
                                        <f:DropDownList ID="ddlDirection" LabelWidth="100px" LabelAlign="Right"
                                            Label="开启方向" runat="server">
                                            <f:ListItem Text="" Value="" Selected="true" />
                                            <f:ListItem Text="左内开右锁" Value="左内开右锁" />
                                            <f:ListItem Text="左外开右锁" Value="左外开右锁" />
                                            <f:ListItem Text="右内开左锁" Value="右内开左锁" />
                                            <f:ListItem Text="右内开左锁" Value="右内开左锁" />
                                        </f:DropDownList>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow ID="FormRow5" runat="server">
                                    <Items>
                                        <f:DropDownList ID="ddlGoodType" LabelWidth="100px" ShowRedStar="true" LabelAlign="Right"
                                            Label="商品分类" DataTextField="TypeName" DataValueField="ID"
                                            DataSimulateTreeLevelField="Level" Required="true" runat="server"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddlGoodType_SelectedIndexChanged">
                                        </f:DropDownList>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow ID="FormRow1" runat="server">
                                    <Items>
                                        <f:DropDownList ID="ddlGood" LabelWidth="100px" ShowRedStar="true" LabelAlign="Right"
                                            Label="商品" DataTextField="EquipmentName" DataValueField="ID"
                                            DataSimulateTreeLevelField="Level" Required="true" runat="server"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddlGood_SelectedIndexChanged">
                                        </f:DropDownList>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow ID="FormRow8" runat="server">
                                    <Items>
                                        <f:TextBox ID="tbColor" LabelWidth="100px" LabelAlign="Right"
                                            runat="server" Label="颜色" Required="true" ShowRedStar="true">
                                        </f:TextBox>
                                        <f:TextBox ID="tbModel" LabelWidth="100px" LabelAlign="Right"
                                            runat="server" Label="款式型号">
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
                                            Label="厚度(mm)" MinValue="0" Text="0" DecimalPrecision="2">
                                        </f:NumberBox>
                                        <f:NumberBox ID="nbOtherAmount" LabelWidth="100px" LabelAlign="Right" runat="server" NoDecimal="false"
                                            Label="其他金额(元)" MinValue="0" Text="0" DecimalPrecision="2">
                                        </f:NumberBox>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow ID="FormRow4" runat="server">
                                    <Items>
                                        <f:TextBox ID="tbLine" LabelWidth="100px" LabelAlign="Right"
                                            runat="server" Label="线条">
                                        </f:TextBox>
                                        <f:TextBox ID="tbGlass" LabelWidth="100px" LabelAlign="Right"
                                            runat="server" Label="玻璃款式">
                                        </f:TextBox>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow ID="FormRow7" runat="server">
                                    <Items>
                                        <f:DropDownList ID="ddlParts" LabelWidth="100px" LabelAlign="Right"
                                            Label="锁具型号" DataTextField="PartsName" DataValueField="ID" runat="server">
                                        </f:DropDownList>
                                        <f:DropDownList ID="ddlSupply" LabelWidth="100px" ShowRedStar="true" LabelAlign="Right"
                                            Label="供应厂家" DataTextField="SupplierName" DataValueField="ID"
                                            Required="true" runat="server">
                                        </f:DropDownList>
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
