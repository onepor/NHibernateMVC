<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ContractManage.aspx.cs" Inherits="ZAJCZN.MIS.Web.ContractManage" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server" />
        <f:Panel ID="Panel1" runat="server" BodyPadding="5px" BoxConfigAlign="Stretch" BoxConfigPosition="Start"
            ShowBorder="false" ShowHeader="false" Layout="VBox">
            <Items>
                <f:Panel ID="Panel2" ShowHeader="false" ShowBorder="false" Layout="Column" runat="server">
                    <Items>
                        <f:TextBox ID="txtSearch" runat="server" LabelWidth="80px" Label="关键字" LabelAlign="Right"
                            EmptyText="在客户名称、电话号码、订单编号、客户地址中搜索" Width="500px">
                        </f:TextBox>
                        <f:DatePicker ID="dpStartDate" LabelWidth="120px" Label="订单登记时间" Width="260px"
                            runat="server" LabelAlign="Right">
                        </f:DatePicker>
                        <f:DatePicker ID="dpEndDate" LabelWidth="10px" Label="~" Width="150px" runat="server"
                            CompareControl="dpStartDate" CompareOperator="GreaterThan" CompareMessage="结束日期应该大于开始日期！">
                        </f:DatePicker>
                        <f:Button ID="btnSearch" MarginLeft="20px" runat="server" OnClick="btnSearch_Click" Icon="Find" Text="查询"></f:Button>
                        <f:HiddenField ID="hdNO" runat="server"></f:HiddenField>
                    </Items>
                </f:Panel>
                <f:Grid ID="Grid1" runat="server" ShowBorder="true" ShowHeader="false"
                    EnableCheckBoxSelect="false" DataKeyNames="ID" OnPreDataBound="Grid1_PreDataBound"
                    OnRowCommand="Grid1_RowCommand" OnRowDoubleClick="Grid1_RowDoubleClick" EnableRowDoubleClickEvent="true"
                    AllowSorting="true" OnSort="Grid1_Sort" SortField="ID" SortDirection="DESC"
                    AllowPaging="true" IsDatabasePaging="true" OnPageIndexChange="Grid1_PageIndexChange"
                    OnRowClick="Grid1_RowClick" EnableRowClickEvent="true">
                    <Toolbars>
                        <f:Toolbar ID="Toolbar1" Position="Top" runat="server">
                            <Items>
                                <f:Button ID="btnNew" runat="server" Icon="BookAdd" EnablePostBack="false" Text="订单登记">
                                </f:Button>
                                <f:ToolbarFill ID="ToolbarFill1" runat="server">
                                </f:ToolbarFill>
                                <f:Button ID="btnPrint" EnableAjax="false" Icon="Printer" runat="server" Text="打印定金"
                                    OnClientClick="onPrint();">
                                </f:Button>
                            </Items>
                        </f:Toolbar>
                    </Toolbars>
                    <PageItems>
                        <f:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                        </f:ToolbarSeparator>
                        <f:ToolbarText ID="ToolbarText1" runat="server" Text="每页记录数：">
                        </f:ToolbarText>
                        <f:DropDownList ID="ddlGridPageSize" Width="80px" AutoPostBack="true" OnSelectedIndexChanged="ddlGridPageSize_SelectedIndexChanged"
                            runat="server">
                            <f:ListItem Text="10" Value="10" />
                            <f:ListItem Text="20" Value="20" />
                            <f:ListItem Text="50" Value="50" />
                            <f:ListItem Text="100" Value="100" />
                        </f:DropDownList>
                    </PageItems>
                    <Columns>
                        <f:LinkButtonField ColumnID="deleteField" HeaderText="" TextAlign="Center" Icon="Delete" ToolTip="删除"
                            ConfirmText="确定删除此记录？" ConfirmTarget="Top" CommandName="Delete" Width="40px" />
                        <f:WindowField ColumnID="editField" HeaderText="编辑" TextAlign="Center" Icon="Pencil" ToolTip="编辑订单信息"
                            WindowID="Window1" Title="编辑订单信息" DataIFrameUrlFields="ID" DataIFrameUrlFormatString="~/Contract/ContractEdit.aspx?id={0}&type=1"
                            Width="50px" />
                        <f:WindowField ColumnID="lbtnEditMoneyField" HeaderText="定金" TextAlign="Center" Icon="MoneyAdd" ToolTip="订单收定金登记"
                            WindowID="Window1" Title="订单收定金登记" DataIFrameUrlFields="ID" DataIFrameUrlFormatString="~/Contract/ContractPaySure.aspx?id={0}"
                            Width="50px" />
                        <f:TemplateField Width="60px">
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Width="60px" Text='<%# Eval("IsUrgent").ToString() =="1"?"加急":"" %>' ForeColor="Red"></asp:Label>
                            </ItemTemplate>
                        </f:TemplateField>
                        <f:TemplateField Width="80px" HeaderText="状态">
                            <ItemTemplate>
                                <asp:Label ID="Label3" runat="server" Width="80px" Text='<%# GetOrderState(Eval("ContractState").ToString()) %>'></asp:Label>
                            </ItemTemplate>
                        </f:TemplateField>
                        <f:BoundField DataField="ContractNO" HeaderText="订单号" Width="160px" SortField="ContractNO" />
                        <f:BoundField DataField="ProjectName" HeaderText="客户地址" Width="200px" />
                        <f:BoundField DataField="CustomerName" HeaderText="客户名称" Width="200px" />
                        <f:BoundField DataField="ContactPhone" HeaderText="联系电话" Width="180px" />
                        <f:TemplateField Width="80px" HeaderText="销售人员">
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Width="80px" Text='<%# GetPerson(Eval("SalePerson").ToString()) %>'></asp:Label>
                            </ItemTemplate>
                        </f:TemplateField>
                        <f:BoundField DataField="ContractDate" HeaderText="登记时间" Width="100px" />
                        <f:BoundField DataField="PerSendDate" HeaderText="预约送货日期" Width="130px" />
                        <f:BoundField DataField="PerInstalDate" HeaderText="预约安装日期" Width="130px" />
                        <f:BoundField DataField="Remark" HeaderText="备注" Width="300px" />
                    </Columns>
                </f:Grid>
            </Items>
        </f:Panel>
        <f:Window ID="Window1" CloseAction="Hide" runat="server" IsModal="true" Hidden="true"
            Target="Top" EnableResize="true" EnableMaximize="true" EnableIFrame="true" IFrameUrl="about:blank"
            Width="600px" Height="480px" OnClose="Window1_Close">
        </f:Window>
    </form>
    <script type="text/javascript">

        function onPrint() {
            var textbox1ID = '<%= hdNO.ClientID %>';
            var id = F(textbox1ID).getValue();
            if (id != "0")
            {
                var url = 'ContractPayPrint.aspx?printNO=' + id;
                window.open(url);
            }
        }

    </script>
</body>
</html>
