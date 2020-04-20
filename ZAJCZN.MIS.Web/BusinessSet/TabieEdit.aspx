<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TabieEdit.aspx.cs" Inherits="ZAJCZN.MIS.Web.TabieEdit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server" />
        <f:Panel ID="Panel1" ShowBorder="false" ShowHeader="false"  runat="server">
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
                                <f:Label ID="labDiningArea" runat="server" Label="所属餐区" LabelAlign="Right"></f:Label>
                            </Items>
                        </f:FormRow>
                        <f:FormRow runat="server">
                            <Items>
                                <f:TextBox ID="txbTabieName" Label="餐台名称" ShowRedStar="true" LabelAlign="Right" runat="server" Required="true">
                                </f:TextBox>
                                <f:DropDownList ID="lstSalesModel" Label="销售模式" ShowRedStar="true" LabelAlign="Right" runat="server"
                                    DataTextField="EnumKey" DataValueField="EnumValue">
                                </f:DropDownList>
                            </Items>
                        </f:FormRow>
                        <f:FormRow runat="server">
                            <Items>
                                <f:NumberBox runat="server" ID="numTabieNumber" Label="餐台编号" LabelAlign="Right" Required="true" ShowRedStar="true"></f:NumberBox>
                                <f:NumberBox runat="server" ID="numSort" Label="排序" LabelAlign="Right" Required="true" ShowRedStar="true"></f:NumberBox>
                            </Items>
                        </f:FormRow>
                    </Rows>
                </f:Form>
            </Items>
        </f:Panel>
    </form>
</body>
</html>
