<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SupplierEdit.aspx.cs" Inherits="ZAJCZN.MIS.Web.SupplierEdit" %>

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
                <f:Form ID="SimpleForm1" ShowBorder="false" ShowHeader="false" runat="server" BodyPadding="10px"
                    Title="SimpleForm">
                    <Rows>
                        <f:FormRow runat="server">
                            <Items>
                                <f:TextBox ID="txtSupplierCode" Label="供应商编码" ShowRedStar="true" LabelWidth="110px" LabelAlign="Right" runat="server" Required="true">
                                </f:TextBox>
                                <f:TextBox ID="txtSupplierName" Label="供应商名称" ShowRedStar="true" LabelWidth="110px" LabelAlign="Right" runat="server" Required="true">
                                </f:TextBox>
                            </Items>
                        </f:FormRow>
                        <f:FormRow runat="server">
                            <Items>
                                <f:TextBox ID="txtFullName" Label="供应商全称" LabelWidth="110px" LabelAlign="Right" runat="server">
                                </f:TextBox>
                                <f:TextBox ID="txtRemark" Label="供应商描述" LabelWidth="110px" LabelAlign="Right" runat="server">
                                </f:TextBox>
                            </Items>
                        </f:FormRow>
                        <f:FormRow runat="server">
                            <Items>
                                <f:TextBox ID="txtLinkMan" Label="联系人" ShowRedStar="true" LabelWidth="110px" LabelAlign="Right" runat="server" Required="true">
                                </f:TextBox>
                                <f:TextBox ID="txtPhone" Label="联系电话" ShowRedStar="true" LabelWidth="110px" LabelAlign="Right" runat="server" Required="true">
                                </f:TextBox>
                            </Items>
                        </f:FormRow>
                        <f:FormRow runat="server">
                            <Items>
                                <f:TextBox ID="txtAddress" Label="联系地址" LabelWidth="110px" LabelAlign="Right" runat="server">
                                </f:TextBox>
                                <f:RadioButtonList LabelWidth="120px" LabelAlign="Right" ID="rbtnIsUsed" Label="是否启用"
                                    Required="true" ShowRedStar="true" runat="server">
                                    <f:RadioItem Text="是" Value="2" Selected="true" />
                                    <f:RadioItem Text="否" Value="1" />
                                </f:RadioButtonList>
                            </Items>
                        </f:FormRow>

                    </Rows>
                </f:Form>
            </Items>
        </f:Panel>
    </form>
</body>
</html>
