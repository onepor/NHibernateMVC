<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ContractSendingEdit.aspx.cs" Inherits="ZAJCZN.MIS.Web.ContractSendingEdit" %>

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
                <f:GroupPanel ID="GroupPanel1" runat="server" Title="送货安装时间">
                    <Items>
                        <f:Form ID="Form2" ShowBorder="false" ShowHeader="false" runat="server" BodyPadding="10px"
                            Title="SimpleForm">
                            <Rows>
                                <f:FormRow ID="FormRow4" runat="server">
                                    <Items>
                                        <f:DatePicker ID="dpContractDate" LabelWidth="120px" Label="送货时间" runat="server" LabelAlign="Right"
                                            Required="true" ShowRedStar="true">
                                        </f:DatePicker>
                                        <f:DropDownList ID="ddlSaler" runat="server" DataTextField="EmployeeName" DataValueField="ID"
                                            LabelWidth="100px" LabelAlign="Right" Label="送货人员">
                                        </f:DropDownList>
                                        <f:CheckBox ID="ckHandWare" runat="server" LabelWidth="100px" LabelAlign="Right" Label="五金齐备" Checked="true">
                                        </f:CheckBox>
                                    </Items>
                                </f:FormRow>
                            </Rows>
                        </f:Form>
                    </Items>
                </f:GroupPanel>
                <f:GroupPanel ID="gpBaseInfo" runat="server" Title="订单基本信息">
                    <Items>
                        <f:Form ID="SimpleForm1" ShowBorder="false" ShowHeader="false" runat="server" BodyPadding="10px"
                            Title="SimpleForm">
                            <Rows>
                                <f:FormRow ID="FormRow9" runat="server">
                                    <Items>
                                        <f:Label ID="lblNo" LabelWidth="120px" LabelAlign="Right" runat="server" Label="合同编号">
                                        </f:Label>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow ID="FormRow13" runat="server">
                                    <Items>
                                        <f:TextBox ID="txtProjectName" Readonly="true" LabelAlign="Right" LabelWidth="120px" runat="server" Label="项目名称">
                                        </f:TextBox>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow ID="FormRow5" runat="server">
                                    <Items>
                                        <f:Label ID="lbldate" LabelWidth="120px" LabelAlign="Right" runat="server" Label="登记日期">
                                        </f:Label>
                                        <f:Label ID="lblMdate" LabelWidth="120px" LabelAlign="Right" runat="server" Label="测量日期">
                                        </f:Label>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow ID="FormRow1" runat="server">
                                    <Items>
                                        <f:TextBox ID="tbCustomer" LabelAlign="Right" Readonly="true" LabelWidth="120px" runat="server" Label="客户名称">
                                        </f:TextBox>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow ID="FormRow2" runat="server">
                                    <Items>
                                        <f:TextBox ID="tbPhone" LabelAlign="Right" Readonly="true" LabelWidth="120px" runat="server" Label="联系电话">
                                        </f:TextBox>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow ID="FormRow3" runat="server">
                                    <Items>
                                        <f:TextBox ID="tbxRemark" LabelAlign="Right" Readonly="true" LabelWidth="120px" runat="server" Label="备注">
                                        </f:TextBox>
                                    </Items>
                                </f:FormRow>
                            </Rows>
                        </f:Form>
                    </Items>
                </f:GroupPanel>
                <f:GroupPanel ID="GroupPanel2" runat="server" Title="生产信息">
                    <Items>
                        <f:Grid ID="gdHandWare" ShowBorder="true" ShowHeader="false" EnableCollapse="true"
                            runat="server" DataKeyNames="ID" EnableCheckBoxSelect="false">
                            <Columns>
                                <f:RowNumberField></f:RowNumberField>
                                <f:BoundField DataField="SuppilerName" Width="260px" HeaderText="厂商名称" />
                                <f:TemplateField Width="100px" HeaderText="厂商类型">
                                    <ItemTemplate>
                                        <asp:Label ID="Label2" runat="server" Width="100px" Text='<%#  Eval("CostType").ToString().Equals ("1")?"门厂商":"柜子厂商" %>'></asp:Label>
                                    </ItemTemplate>
                                </f:TemplateField>
                            </Columns>
                        </f:Grid>
                    </Items>
                </f:GroupPanel>
            </Items>
        </f:Panel>
    </form>
</body>
</html>
