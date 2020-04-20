<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmployeeEdit.aspx.cs" Inherits="ZAJCZN.MIS.Web.EmployeeEdit" %>

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
                                <f:TextBox ID="txtVipName" Label="员工姓名" ShowRedStar="true" LabelAlign="Right" runat="server" Required="true">
                                </f:TextBox>
                            </Items>
                        </f:FormRow>
                        <f:FormRow runat="server">
                            <Items>
                                <f:TextBox ID="txbVipPhone" Label="联系电话" LabelAlign="Right" runat="server">
                                </f:TextBox>
                            </Items>
                        </f:FormRow>
                        <f:FormRow ID="FormRow2" runat="server">
                            <Items>
                                <f:TextBox ID="txtAddress" Label="联系地址" LabelAlign="Right" runat="server">
                                </f:TextBox>
                            </Items>
                        </f:FormRow>
                        <f:FormRow ID="FormRow1" runat="server">
                            <Items>
                                <f:RadioButtonList LabelAlign="Right" ID="rbtnSex" Label="性别"
                                    Required="true" ShowRedStar="true" runat="server">
                                    <f:RadioItem Text="男" Value="1" Selected="true" />
                                    <f:RadioItem Text="女" Value="2" />
                                </f:RadioButtonList>
                            </Items>
                        </f:FormRow>
                        <f:FormRow ID="FormRow8" runat="server">
                            <Items>
                                <f:RadioButtonList LabelAlign="Right" ID="ddlIsUsed" Label="是否启用"
                                    Required="true" ShowRedStar="true" runat="server">
                                    <f:RadioItem Text="是" Value="1" Selected="true" />
                                    <f:RadioItem Text="否" Value="0" />
                                </f:RadioButtonList>
                            </Items>
                        </f:FormRow>
                        <f:FormRow ID="FormRow3" runat="server">
                            <Items>
                                <f:DropDownList ID="ddlType" Label="人员类型" runat="server" LabelAlign="Right" ShowRedStar="true" Required="true">
                                    <f:ListItem Text="销售人员" Value="销售人员" Selected="true" />
                                    <f:ListItem Text="测量人员" Value="测量人员" />
                                    <f:ListItem Text="设计人员" Value="设计人员" />
                                    <f:ListItem Text="送货人员" Value="送货人员" />
                                    <f:ListItem Text="安装人员" Value="安装人员" />
                                    <f:ListItem Text="售后服务" Value="售后服务" />
                                </f:DropDownList>
                            </Items>
                        </f:FormRow>
                        <f:FormRow runat="server">
                            <Items>
                                <f:TextBox ID="txtRemark" Label="备注" LabelAlign="Right" runat="server">
                                </f:TextBox>
                            </Items>
                        </f:FormRow>
                    </Rows>
                </f:Form>
            </Items>
        </f:Panel>
    </form>
</body>
</html>
