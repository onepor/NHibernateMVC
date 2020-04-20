<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ContractPayManage.aspx.cs" Inherits="ZAJCZN.MIS.Web.ContractPayManage" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title></title>
    <style>
        .f-grid-row-summary .f-grid-cell-inner {
            font-weight: bold;
            color: red;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server" />
        <f:Panel ID="Panel1" runat="server" BodyPadding="5px" BoxConfigAlign="Stretch" BoxConfigPosition="Start"
            ShowBorder="false" ShowHeader="false" Layout="VBox">
            <Items>
                <f:Panel ID="Panel2" ShowHeader="false" ShowBorder="false" runat="server">
                    <Items>
                        <f:Toolbar ID="Toolbar2" runat="server">
                            <Items>
                                <f:DropDownList ID="ddlPay" runat="server" LabelWidth="80px" Label="收\付款" Width="200px"
                                    LabelAlign="Right" AutoPostBack="true" OnSelectedIndexChanged="ddlPay_SelectedIndexChanged">
                                    <f:ListItem Text="--全部--" Value="" Selected="true" />
                                    <f:ListItem Text="收款" Value="1" />
                                    <f:ListItem Text="付款" Value="2" />
                                </f:DropDownList>
                                <f:ToolbarFill ID="ToolbarFill2" runat="server">
                                </f:ToolbarFill>
                                <f:Button ID="btnReturn" OnClick="btnReturn_Click" runat="server" Icon="arrowleft"
                                    Text="返回定单列表">
                                </f:Button>
                                <f:HiddenField ID="hdNO" runat="server"></f:HiddenField>
                                <f:HiddenField ID="hdType" runat="server"></f:HiddenField>
                            </Items>
                        </f:Toolbar>
                    </Items>
                </f:Panel>
                <f:Grid ID="Grid1" runat="server" ShowBorder="true" ShowHeader="false"
                    EnableCheckBoxSelect="false" DataKeyNames="ID" OnRowClick="Grid1_RowClick"
                    AllowSorting="true" SortField="ID" SortDirection="DESC" EnableRowClickEvent="true"
                    AllowPaging="false" IsDatabasePaging="true"
                    EnableSummary="true" SummaryPosition="Bottom">
                    <Toolbars>
                        <f:Toolbar ID="Toolbar1" Position="Top" runat="server">
                            <Items>
                                <f:Button ID="btnDeleteHW" Text="删除选中行" Icon="Delete"
                                    runat="server" OnClick="btnDeleteHW_Click" ConfirmText="确定删除选择的记录信息?">
                                </f:Button>
                                <f:ToolbarFill ID="ToolbarFill1" runat="server">
                                </f:ToolbarFill>
                                <f:Button ID="btnNew" Text="新增单据" Icon="Add" EnablePostBack="false" runat="server">
                                </f:Button>
                                <f:Button ID="btnExport" EnableAjax="false" DisableControlBeforePostBack="false"
                                    Icon="PageExcel" runat="server" Text="导出" OnClick="btnExport_Click">
                                </f:Button>
                                <f:Button ID="btnPrint" EnableAjax="false" Icon="Printer" runat="server" Text="打印"
                                    OnClientClick="onPrint();">
                                </f:Button>
                            </Items>
                        </f:Toolbar>
                    </Toolbars>
                    <Columns>
                        <f:RowNumberField></f:RowNumberField>
                        <f:TemplateField Width="80px" HeaderText="款项类型">
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Width="60px" Text='<%# Eval("PayType").ToString() =="1"?"收款":"付款" %>' ForeColor='<%# GetColor(Eval("PayType").ToString()) %>'></asp:Label>
                            </ItemTemplate>
                        </f:TemplateField>
                        <f:TemplateField Width="80px" HeaderText="状态">
                            <ItemTemplate>
                                <asp:Label ID="Label4" runat="server" Width="60px" Text='<%# GetOrderState(Eval("ApplyState").ToString()) %>' ForeColor='<%# GetColor1(Eval("ApplyState").ToString()) %>'></asp:Label>
                            </ItemTemplate>
                        </f:TemplateField>
                        <f:BoundField DataField="ApplyDate" HeaderText="录入时间" Width="100px" />
                        <f:BoundField DataField="PayWay" HeaderText="支付方式" Width="150px" />
                        <f:BoundField DataField="PayUser" ColumnID="PayUser" HeaderText="付款对象" Width="300px" />
                        <f:BoundField DataField="PayMoney" ColumnID="PayMoney" HeaderText="金额(元)" Width="120px" />
                        <f:BoundField DataField="Operator" HeaderText="录入人员" Width="100px" />
                        <f:BoundField DataField="Remark" HeaderText="备注" Width="300px" />
                    </Columns>
                </f:Grid>
            </Items>
        </f:Panel>
        <f:Window ID="Window1" CloseAction="Hide" runat="server" IsModal="true" Hidden="true"
            Target="Top" EnableResize="true" EnableMaximize="true" EnableIFrame="true" IFrameUrl="about:blank"
            Width="600px" Height="500px" OnClose="Window1_Close">
        </f:Window>
    </form>
    <script type="text/javascript">

        function onPrint() {
            var textbox1ID = '<%= hdNO.ClientID %>';
            var textbox2ID = '<%= hdType.ClientID %>';
            var id = F(textbox1ID).getValue();
            var url = 'ContractPayPrint.aspx?printNO=' + F(textbox1ID).getValue();
            if (F(textbox2ID).getValue() == "2") {
                url = 'ContractPayforPrint.aspx?printNO=' + F(textbox1ID).getValue();
            }

            if (id == "") {
                alert("请选择要打印的单据!");
            }
            else {
                window.open(url);
            }
        }

    </script>
</body>
</html>
