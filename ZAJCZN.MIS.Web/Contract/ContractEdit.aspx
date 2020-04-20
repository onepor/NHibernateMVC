<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ContractEdit.aspx.cs" Inherits="ZAJCZN.MIS.Web.ContractEdit" %>

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
                                        <f:Label ID="lblNo" LabelWidth="120px" LabelAlign="Right" runat="server" Label="合同编号">
                                        </f:Label>
                                        <f:DatePicker ID="dpContractDate" LabelWidth="120px" Label="登记时间" runat="server" LabelAlign="Right"
                                            Required="true" ShowRedStar="true">
                                        </f:DatePicker>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow ID="FormRow13" runat="server">
                                    <Items>
                                        <f:TextBox ID="txtProjectName" LabelAlign="Right" LabelWidth="120px" runat="server" Label="客户地址">
                                        </f:TextBox>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow ID="FormRow1" runat="server">
                                    <Items>
                                        <f:TextBox ID="tbCustomer" LabelAlign="Right" LabelWidth="120px" runat="server" Label="客户名称">
                                        </f:TextBox>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow ID="FormRow2" runat="server">
                                    <Items>
                                        <f:TextBox ID="tbPhone" LabelAlign="Right" LabelWidth="120px" runat="server" Label="联系电话">
                                        </f:TextBox>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow ID="FormRow4" runat="server">
                                    <Items>
                                        <f:DatePicker ID="dpSendDate" LabelWidth="120px" Label="预约送货日期" Width="260px"
                                            runat="server" LabelAlign="Right">
                                        </f:DatePicker>
                                        <f:DatePicker ID="dpInstalDate" LabelWidth="120px" Label="预约安装日期" Width="260px"
                                            runat="server" LabelAlign="Right">
                                        </f:DatePicker>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow ID="FormRow6" runat="server">
                                    <Items>
                                        <f:CheckBox ID="cbIsUrgent" runat="server" LabelWidth="120px" LabelAlign="Right" Label="加急订单" Checked="false"></f:CheckBox>
                                        <f:DropDownList ID="ddlSaler" runat="server" DataTextField="EmployeeName" DataValueField="ID"
                                            LabelWidth="120px" LabelAlign="Right" Label="销售人员">
                                        </f:DropDownList>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow ID="FormRow3" runat="server">
                                    <Items>
                                        <f:TextBox ID="tbxRemark" LabelAlign="Right" LabelWidth="120px" runat="server" Label="备注">
                                        </f:TextBox>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow ID="FormRowState" runat="server">
                                    <Items>
                                        <f:DropDownList ID="ddlState" runat="server" LabelAlign="Right" LabelWidth="120px" Label="生产状态">
                                            <f:ListItem Text="请选择" Value="" Selected="true" />
                                            <f:ListItem Text="售后中" Value="11" />
                                            <f:ListItem Text="质保中" Value="10" />
                                            <f:ListItem Text="质保结束" Value="12" />
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
