<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrinterEdit.aspx.cs" Inherits="ZAJCZN.MIS.Web.PrinterEdit" %>

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
                                <f:TextBox ID="labPrinterName" runat="server" LabelWidth="110px" Label="打印机名称" Required="true" ShowRedStar="true" LabelAlign="Right"></f:TextBox>
                                <f:DropDownList runat="server" ID="ddlWidth" Label="宽度" LabelWidth="110px" Required="true" ShowRedStar="true" LabelAlign="Right">
                                    <f:ListItem Text="58" Value="1" Selected="true" />
                                    <f:ListItem Text="80" Value="2" />
                                </f:DropDownList>
                            </Items>
                        </f:FormRow>
                        <f:FormRow runat="server">
                            <Items>
                                <f:RadioButtonList ID="radioPrinterType" LabelWidth="110px" runat="server" Label="打印机类型" Required="true" ShowRedStar="true" LabelAlign="Right">
                                    <f:RadioItem Text="58" Value="1" Selected="true" />
                                    <f:RadioItem Text="80" Value="2" />
                                </f:RadioButtonList>
                                <f:RadioButtonList ID="radioIsSinging" LabelWidth="110px" runat="server" Label="是否蜂鸣" Required="true" ShowRedStar="true" LabelAlign="Right">
                                    <f:RadioItem Text="是" Value="1" Selected="true" />
                                    <f:RadioItem Text="否" Value="2" />
                                </f:RadioButtonList>
                            </Items>
                        </f:FormRow>
                        <f:FormRow runat="server">
                            <Items>
                                <f:TextBox ID="txbIP" runat="server" LabelWidth="110px" Label="打印机IP" Required="true" ShowRedStar="true" LabelAlign="Right"></f:TextBox>
                                <f:NumberBox ID="numPort" runat="server" LabelWidth="110px" Label="打印机端口" Required="true" ShowRedStar="true" LabelAlign="Right"></f:NumberBox>
                            </Items>
                        </f:FormRow>
                        <f:FormRow runat="server">
                            <Items>
                                <f:RadioButtonList ID="radioAddress" LabelWidth="110px" runat="server" Label="打印机位置" Required="true" ShowRedStar="true" LabelAlign="Right">
                                    <f:RadioItem Text="前台" Value="1" Selected="true" />
                                    <f:RadioItem Text="厨打" Value="2" />
                                </f:RadioButtonList>
                                <f:RadioButtonList ID="radioIsOpenCashbox" LabelWidth="110px" runat="server" Label="开钱箱" Required="true" ShowRedStar="true" LabelAlign="Right">
                                    <f:RadioItem Text="是" Value="1" Selected="true" />
                                    <f:RadioItem Text="否" Value="2" />
                                </f:RadioButtonList>
                            </Items>
                        </f:FormRow>
                        <f:FormRow runat="server">
                            <Items>
                               <f:RadioButtonList ID="radioIsPrintslabels" LabelWidth="110px" runat="server" Label="标签打印" Required="true" ShowRedStar="true" LabelAlign="Right">
                                    <f:RadioItem Text="是" Value="1" Selected="true" />
                                    <f:RadioItem Text="否" Value="2" />
                                </f:RadioButtonList>
                                <f:TextBox ID="txbSerialNumber" Label="打印机序列号" LabelWidth="110px" ShowRedStar="true" LabelAlign="Right" runat="server">
                                </f:TextBox>
                            </Items>
                        </f:FormRow>
                        <f:FormRow runat="server">
                            <Items>
                                <f:CheckBoxList LabelWidth="110px" runat="server" ID="ckeDeploy" ColumnNumber="3" Label="打印机配置" LabelAlign="Right" Required="true" ShowRedStar="true"
                                    DataTextField="EnumKey" DataValueField="EnumValue" >
                                </f:CheckBoxList>
                            </Items>
                        </f:FormRow>
                    </Rows>
                </f:Form>
            </Items>
        </f:Panel>
    </form>
</body></html>
