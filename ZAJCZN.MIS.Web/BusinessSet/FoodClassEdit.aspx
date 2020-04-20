<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FoodClassEdit.aspx.cs" Inherits="ZAJCZN.MIS.Web.FoodClassEdit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
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
                                <f:TextBox ID="tbxName" ShowRedStar="true" LabelWidth="120px" LabelAlign="Right" runat="server" Label="口味类型名称" Required="true">
                                </f:TextBox>
                            </Items>
                        </f:FormRow>
                        <f:FormRow ID="FormRow1" runat="server">
                            <Items>
                                <f:NumberBox ID="tbxSortIndex" Label="排序" Required="true" LabelAlign="Right" LabelWidth="120px"
                                    ShowRedStar="true" runat="server" Text="0">
                                </f:NumberBox>
                            </Items>
                        </f:FormRow>
                        <f:FormRow ID="FormRow2" runat="server">
                            <Items>
                                <f:DropDownList ID="ddlUnit" Label="菜品单位" Required="true" LabelWidth="120px" ShowRedStar="true" LabelAlign="Right"
                                    DataTextField="EnumKey" DataValueField="EnumValue"
                                    runat="server">
                                </f:DropDownList>
                            </Items>
                        </f:FormRow>
                        <f:FormRow runat="server">
                            <Items>
                                <f:RadioButtonList LabelWidth="120px" LabelAlign="Right" ID="ddlIsUsed" Label="是否启用" Required="true" ShowRedStar="true" runat="server">
                                    <f:RadioItem Text="是" Value="2" Selected="true" />
                                    <f:RadioItem Text="否" Value="1" />
                                </f:RadioButtonList>
                            </Items>
                        </f:FormRow>
                        <f:FormRow ID="FormRow4" runat="server">
                            <Items>
                                <f:DropDownList ID="ddlPrint" LabelWidth="120px" LabelAlign="Right" Label="默认打印机"
                                    DataTextField="PrinterName" DataValueField="ID" runat="server">
                                </f:DropDownList>
                            </Items>
                        </f:FormRow>
                        <f:FormRow ID="FormRow5" runat="server" Hidden="true">
                            <Items>
                                <f:DropDownList ID="ddlWH" LabelWidth="120px" LabelAlign="Right" Label="出库库房"
                                    DataTextField="WHName" DataValueField="ID" runat="server">
                                </f:DropDownList>
                            </Items>
                        </f:FormRow>
                        <f:FormRow ID="FormRow3" runat="server">
                            <Items>
                                <f:TextArea ID="tbxRemark" runat="server" LabelWidth="120px" Label="备注" LabelAlign="Right">
                                </f:TextArea>
                            </Items>
                        </f:FormRow>
                    </Rows>
                </f:Form>
            </Items>
        </f:Panel>
        <f:Window ID="Window1" Title="编辑" Hidden="true" EnableIFrame="true" runat="server"
            EnableMaximize="true" EnableResize="true" Target="Top" IsModal="True" Width="550px"
            Height="350px">
        </f:Window>
    </form>
</body>
</html>
