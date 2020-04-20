<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DishesEdit.aspx.cs" Inherits="ZAJCZN.MIS.Web.DishesEdit" %>

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
                        <f:FormRow ID="FormRow2" runat="server">
                            <Items>
                                <f:DropDownList ID="ddlPType" Label="所属类别" Required="true" LabelWidth="120px"
                                    ShowRedStar="true" LabelAlign="Right" DataTextField="ClassName" DataValueField="ID"
                                    runat="server">
                                </f:DropDownList>
                                <f:TextBox ID="tbxName" ShowRedStar="true" LabelWidth="120px" LabelAlign="Right" runat="server" Label="菜品名称" Required="true">
                                </f:TextBox>
                            </Items>
                        </f:FormRow>
                        <f:FormRow runat="server">
                            <Items>
                                <f:Label ID="lblCode" LabelWidth="120px" LabelAlign="Right" runat="server" Label="菜品编号">
                                </f:Label>
                                <f:Label ID="lblPY" LabelWidth="120px" LabelAlign="Right" runat="server" Label="拼音助记码">
                                </f:Label>
                            </Items>
                        </f:FormRow>
                        <f:FormRow ID="FormRow4" runat="server">
                            <Items>
                                <f:DropDownList ID="ddlUnit" LabelWidth="120px" LabelAlign="Right" Label="菜品单位"
                                    DataTextField="EnumKey" DataValueField="EnumValue" Required="true" ShowRedStar="true"
                                    runat="server">
                                </f:DropDownList>
                                <f:NumberBox ID="txtIndex" LabelWidth="120px" LabelAlign="Right" runat="server" Label="显示顺序">
                                </f:NumberBox>
                            </Items>
                        </f:FormRow>
                        <f:FormRow ID="FormRow1" runat="server">
                            <Items>
                                <f:NumberBox ID="txtPrice" LabelWidth="120px" LabelAlign="Right" runat="server" Label="售价(元)">
                                </f:NumberBox>
                                <f:NumberBox ID="txtMemberPrice" LabelWidth="120px" LabelAlign="Right" runat="server" Label="会员价(元)">
                                </f:NumberBox>
                            </Items>
                        </f:FormRow>
                        <f:FormRow ID="FormRow6" runat="server">
                            <Items>
                                <f:RadioButtonList LabelWidth="120px" LabelAlign="Right" ID="ddlIsUsed" Label="是否启用" Required="true" ShowRedStar="true" runat="server">
                                    <f:RadioItem Text="是" Value="1" Selected="true" />
                                    <f:RadioItem Text="否" Value="0" />
                                </f:RadioButtonList>
                                <f:DropDownList ID="ddlPrint" LabelWidth="120px" LabelAlign="Right" Label="打印机"
                                    DataTextField="PrinterName" DataValueField="ID" runat="server">
                                </f:DropDownList>
                            </Items>
                        </f:FormRow>
                        <f:FormRow ID="FormRow7" runat="server">
                            <Items>
                                <f:RadioButtonList LabelWidth="120px" LabelAlign="Right" ID="rbtnWeigh" Label="是否称重" Required="true" ShowRedStar="true" runat="server">
                                    <f:RadioItem Text="是" Value="1" />
                                    <f:RadioItem Text="否" Value="0" Selected="true" />
                                </f:RadioButtonList>
                                <f:RadioButtonList LabelWidth="120px" LabelAlign="Right" ID="rbtnRanking" Label="参加排名" Required="true" ShowRedStar="true" runat="server">
                                    <f:RadioItem Text="是" Value="1" Selected="true" />
                                    <f:RadioItem Text="否" Value="0" />
                                </f:RadioButtonList>
                            </Items>
                        </f:FormRow>
                        <f:FormRow ID="FormRow3" runat="server">
                            <Items>

                                <f:RadioButtonList LabelWidth="120px" LabelAlign="Right" ID="rbtnDiscount" Label="整单打折" Required="true" ShowRedStar="true" runat="server">
                                    <f:RadioItem Text="是" Value="1" />
                                    <f:RadioItem Text="否" Value="0" Selected="true" />
                                </f:RadioButtonList>
                                <f:RadioButtonList LabelWidth="120px" LabelAlign="Right" ID="rbntScore" Label="参与积分" Required="true" ShowRedStar="true" runat="server">
                                    <f:RadioItem Text="是" Value="1" Selected="true" />
                                    <f:RadioItem Text="否" Value="0" />
                                </f:RadioButtonList>
                            </Items>
                        </f:FormRow>
                        <f:FormRow ID="FormRow5" runat="server">
                            <Items>
                                <f:FileUpload runat="server" LabelWidth="105px" ID="filePhoto" ShowRedStar="false" ShowEmptyLabel="true"
                                    ButtonText="上传菜品图片" ButtonOnly="true" Required="false" ButtonIcon="ImageAdd" Label="菜品图片"
                                    AutoPostBack="true" OnFileSelected="filePhoto_FileSelected" LabelAlign="Right">
                                </f:FileUpload>
                            </Items>
                        </f:FormRow>
                        <f:FormRow ID="FormRow8" runat="server">
                            <Items>
                                <f:Image LabelAlign="Right" LabelWidth="105px" ID="imgPhoto" ImageWidth="300px" ImageHeight="225px"
                                    ImageUrl="" ShowEmptyLabel="true" runat="server">
                                </f:Image>
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
