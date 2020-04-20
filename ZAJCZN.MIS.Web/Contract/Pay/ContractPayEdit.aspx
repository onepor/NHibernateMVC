<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ContractPayEdit.aspx.cs" Inherits="ZAJCZN.MIS.Web.ContractPayEdit" %>

<!DOCTYPE html>
<html>
<head id="Head1" runat="server">
    <title></title>
    <meta name="sourcefiles" content="~/PublicWebForm/OrderGoodsSelectDialog.aspx" />
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server" />
        <f:Panel ID="Panel1" runat="server" BodyPadding="5px" ShowBorder="false" Layout="VBox" BoxConfigAlign="Stretch"
            BoxConfigPosition="Start" ShowHeader="false" Title="新增买赔单">
            <Items>
                <f:Panel ID="GroupPanel3" runat="server" ShowHeader="false">
                    <Items>
                        <f:Toolbar ID="Toolbar2" runat="server">
                            <Items>
                                <f:Button ID="btnReturn" OnClick="btnReturn_Click" runat="server" Icon="PageBack" Hidden="true" Text="返回买赔单列表">
                                </f:Button>
                                <f:Button ID="btnCancel" OnClick="btnCancel_Click" runat="server" Icon="SystemClose" Text="取消买赔单">
                                </f:Button>
                                <f:ToolbarFill ID="ToolbarFill1" runat="server">
                                </f:ToolbarFill>
                                <f:Button ID="btnSaveClose" ValidateForms="Form2" Icon="Accept" OnClick="btnSaveClose_Click"
                                    runat="server" Text="保存买赔单">
                                </f:Button>
                            </Items>
                        </f:Toolbar>
                    </Items>
                </f:Panel>
                <f:GroupPanel ID="GroupPanel1" runat="server" Title="买赔单基础信息">
                    <Items>
                        <f:Form ID="Form2" ShowBorder="false" ShowHeader="false" runat="server">
                            <Items>
                                <f:FormRow>
                                    <Items>
                                        <f:DropDownList ID="ddlContract" LabelWidth="100px" LabelAlign="Right" Label="客户" Width="300px"
                                            DataTextField="ContarctName" DataValueField="ID" Required="true" ShowRedStar="true"
                                            runat="server">
                                        </f:DropDownList>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow>
                                    <Items>
                                        <f:Label ID="lblOrderNo" runat="server" Label="系统单号" LabelAlign="Right" LabelWidth="100px"></f:Label>
                                        <f:TextBox ID="tbManualNO" LabelWidth="100px" LabelAlign="Right" runat="server" Label="买赔单号"
                                            Required="true" ShowRedStar="true" EmptyText="自编格式：客户编号-顺序号，如0016-0125">
                                        </f:TextBox>
                                        <f:DatePicker ID="dpStartDate" Label="买赔日期" runat="server" Required="true" ShowRedStar="true"
                                            LabelAlign="Right" LabelWidth="100px" DateFormatString="yyyy-MM-dd" EmptyText="请选择买赔日期">
                                        </f:DatePicker>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow>
                                    <Items>
                                        <f:TextBox ID="txtRemark" LabelAlign="Right" runat="server" Label="备注" LabelWidth="100px"></f:TextBox>
                                    </Items>
                                </f:FormRow>
                            </Items>
                        </f:Form>
                    </Items>
                </f:GroupPanel>
                <f:GroupPanel ID="GroupPanel2" runat="server" Title="买赔单明细信息">
                    <Items>
                        <f:Grid ID="gdGoodsDetail" ShowBorder="true" ShowHeader="false" EnableCollapse="true"
                            runat="server" DataKeyNames="ID" AllowCellEditing="true" ClicksToEdit="1" AllowPaging="false"
                            EnableCheckBoxSelect="false" EnableAfterEditEvent="true" OnAfterEdit="gdGoodsDetail_AfterEdit">
                            <Toolbars>
                                <f:Toolbar ID="Toolbar1" runat="server">
                                    <Items>
                                        <f:Label ID="lblNotice1" runat="server" ShowLabel="false" Text="点击买赔数量或单价直接填写修改" CssStyle="color:red;"></f:Label>
                                        <f:ToolbarFill ID="ToolbarFill2" runat="server"></f:ToolbarFill>
                                    </Items>
                                </f:Toolbar>
                            </Toolbars>
                            <Columns>
                                <f:BoundField DataField="GoodsTypeInfo.TypeName" SortField="GoodsTypeInfo.TypeName" Width="200px" HeaderText="商品名称" />
                                <f:TemplateField Width="80px" HeaderText="单位">
                                    <ItemTemplate>
                                        <asp:Label ID="Label4" runat="server" Width="80px" Text='<%# GetUnitName(Eval("GoodsUnit").ToString()) %>'></asp:Label>
                                    </ItemTemplate>
                                </f:TemplateField>
                                <f:RenderField Width="100px" ColumnID="GoodsNumber" DataField="GoodsNumber" FieldType="Double"
                                    HeaderText="买赔数量">
                                    <Editor>
                                        <f:NumberBox ID="tbxUsingCount" NoDecimal="false" NoNegative="true" MinValue="1" DecimalPrecision="3"
                                            MaxValue="5000" runat="server">
                                        </f:NumberBox>
                                    </Editor>
                                </f:RenderField>
                                <f:RenderField Width="100px" ColumnID="GoodsUnitPrice" DataField="GoodsUnitPrice" FieldType="Double"
                                    HeaderText="买赔单价">
                                    <Editor>
                                        <f:NumberBox ID="tbxUnitPrice" NoDecimal="false" NoNegative="true" MinValue="1" DecimalPrecision="3"
                                            MaxValue="5000" runat="server">
                                        </f:NumberBox>
                                    </Editor>
                                </f:RenderField>
                                <f:BoundField DataField="PayAmount" Width="160px" HeaderText="买赔金额" />
                            </Columns>
                        </f:Grid>
                    </Items>
                </f:GroupPanel>
            </Items>
        </f:Panel>
        <f:Window ID="Window1" runat="server" IsModal="true" Hidden="true" Target="Top" EnableResize="true"
            EnableMaximize="true" EnableIFrame="true" IFrameUrl="about:blank" Width="1000px"
            Height="530px" OnClose="Window1_Close">
        </f:Window>
    </form>
</body>
</html>
