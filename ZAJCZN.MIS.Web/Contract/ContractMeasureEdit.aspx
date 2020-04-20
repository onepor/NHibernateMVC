<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ContractMeasureEdit.aspx.cs" Inherits="ZAJCZN.MIS.Web.ContractMeasureEdit" %>

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
                <f:GroupPanel ID="gpBaseInfo" runat="server" Title="订单基本信息">
                    <Items>
                        <f:Form ID="SimpleForm1" ShowBorder="false" ShowHeader="false" runat="server" BodyPadding="10px"
                            Title="SimpleForm">
                            <Rows>
                                <f:FormRow ID="FormRow9" runat="server">
                                    <Items>
                                        <f:Label ID="lblNo" LabelWidth="100px" LabelAlign="Right" runat="server" Label="合同编号">
                                        </f:Label>
                                        <f:Label ID="lbldate" LabelWidth="100px" LabelAlign="Right" runat="server" Label="登记日期">
                                        </f:Label>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow ID="FormRow5" runat="server">
                                    <Items>
                                        <f:Label ID="lblSend" LabelWidth="100px" LabelAlign="Right" runat="server" Label="预约送货">
                                        </f:Label>
                                        <f:Label ID="lblInstall" LabelWidth="100px" LabelAlign="Right" runat="server" Label="预约安装">
                                        </f:Label>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow ID="FormRow13" runat="server">
                                    <Items>
                                        <f:Label ID="txtProjectName" Readonly="true" LabelAlign="Right" LabelWidth="100px" runat="server" Label="项目名称">
                                        </f:Label>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow ID="FormRow1" runat="server">
                                    <Items>
                                        <f:Label ID="tbCustomer" LabelAlign="Right" Readonly="true" LabelWidth="100px" runat="server" Label="客户名称">
                                        </f:Label>
                                        <f:Label ID="tbPhone" LabelAlign="Right" Readonly="true" LabelWidth="100px" runat="server" Label="联系电话">
                                        </f:Label>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow ID="FormRow3" runat="server">
                                    <Items>
                                        <f:Label ID="tbxRemark" LabelAlign="Right" Readonly="true" LabelWidth="100px" runat="server" Label="备注">
                                        </f:Label>
                                    </Items>
                                </f:FormRow>
                            </Rows>
                        </f:Form>
                    </Items>
                </f:GroupPanel>
                <f:GroupPanel ID="GroupPanel1" runat="server" Title="测量时间">
                    <Items>
                        <f:Form ID="Form2" ShowBorder="false" ShowHeader="false" runat="server" BodyPadding="10px"
                            Title="SimpleForm">
                            <Rows>
                                <f:FormRow ID="FormRow4" runat="server">
                                    <Items>
                                        <f:DatePicker ID="dpContractDate" LabelWidth="100px" Label="测量时间" runat="server" LabelAlign="Right"
                                            Required="true" ShowRedStar="true">
                                        </f:DatePicker>
                                        <f:DropDownList ID="ddlSaler" runat="server" DataTextField="EmployeeName" DataValueField="ID"
                                            LabelWidth="100px" LabelAlign="Right" Label="测量人员">
                                        </f:DropDownList>
                                    </Items>
                                </f:FormRow>
                            </Rows>
                        </f:Form>
                    </Items>
                </f:GroupPanel>
            </Items>
        </f:Panel>
    </form>
</body>
</html>
