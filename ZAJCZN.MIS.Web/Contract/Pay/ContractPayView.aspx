<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ContractPayView.aspx.cs" Inherits="ZAJCZN.MIS.Web.ContractPayView" %>

<!DOCTYPE html>
<html>
<head id="Head1" runat="server">
    <title></title> 
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server" />
        <f:Panel ID="Panel1" runat="server" BodyPadding="5px" ShowBorder="false" Layout="VBox" BoxConfigAlign="Stretch"
            BoxConfigPosition="Start" ShowHeader="false" Title="查看买赔单">
            <Items>
                <f:Panel ID="GroupPanel3" runat="server" ShowHeader="false" Hidden="true">
                    <Items>
                        <f:Toolbar ID="Toolbar2" runat="server">
                            <Items>
                                <f:ToolbarFill ID="ToolbarFill1" runat="server">
                                </f:ToolbarFill>
                                <f:Button ID="btnClose" Icon="SystemClose" EnablePostBack="false" runat="server"
                                    Text="关闭">
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
                                        <f:Label ID="lblContract" runat="server" Label="客户" LabelAlign="Right" LabelWidth="100px"></f:Label>
                                        <f:Label ID="lblOrderNo" runat="server" Label="系统单号" LabelAlign="Right" LabelWidth="100px"></f:Label>
                                        <f:Label ID="lblManualNO" runat="server" Label="买赔单号" LabelAlign="Right" LabelWidth="100px"></f:Label>
                                        <f:Label ID="lblDate" runat="server" Label="买赔日期" LabelAlign="Right" LabelWidth="100px" Readonly="true"></f:Label>
                                        <f:Label ID="lblAmount" runat="server" LabelAlign="Right" Label="总金额(元)" Text="0" LabelWidth="100px"></f:Label>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow>
                                    <Items>
                                        <f:TextBox ID="txtRemark" LabelAlign="Right" runat="server" Label="订单备注"></f:TextBox>
                                    </Items>
                                </f:FormRow>
                            </Items>
                        </f:Form>
                    </Items>
                </f:GroupPanel>
                <f:GroupPanel ID="GroupPanel2" runat="server" Title="买赔单明细信息">
                    <Items>
                        <f:Grid ID="gdGoodsDetail" ShowBorder="true" ShowHeader="false" EnableCollapse="true"
                            runat="server" DataKeyNames="ID" ClicksToEdit="1" AllowPaging="false" EnableCheckBoxSelect="false">
                            <Toolbars>
                            </Toolbars>
                            <Columns>
                                <f:BoundField DataField="GoodsTypeInfo.TypeName" Width="200px" HeaderText="商品名称" />
                                <f:TemplateField Width="80px" HeaderText="单位">
                                    <ItemTemplate>
                                        <asp:Label ID="Label4" runat="server" Width="80px" Text='<%# GetUnitName(Eval("GoodsUnit").ToString()) %>'></asp:Label>
                                    </ItemTemplate>
                                </f:TemplateField>
                                <f:BoundField DataField="GoodsNumber" Width="100px" HeaderText="买赔数量" />
                                <f:BoundField DataField="GoodsUnitPrice" Width="100px" HeaderText="买赔单价" />
                                <f:BoundField DataField="PayAmount" Width="100px" HeaderText="买赔金额" />
                            </Columns>
                        </f:Grid>
                    </Items>
                </f:GroupPanel>
            </Items>
        </f:Panel>
    </form>
</body>
</html>
