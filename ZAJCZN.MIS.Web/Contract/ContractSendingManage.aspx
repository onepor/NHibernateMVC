<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ContractSendingManage.aspx.cs" Inherits="ZAJCZN.MIS.Web.ContractSendingManage" %>

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
                <f:Grid ID="Grid1" runat="server" ShowBorder="true" ShowHeader="false"
                    EnableCheckBoxSelect="true" DataKeyNames="ID" OnPreDataBound="Grid1_PreDataBound"
                    OnRowDoubleClick="Grid1_RowDoubleClick" EnableRowDoubleClickEvent="true"
                    AllowSorting="true" OnSort="Grid1_Sort" SortField="ID" SortDirection="DESC"
                    AllowPaging="false" IsDatabasePaging="true">
                    <Toolbars>
                        <f:Toolbar ID="Toolbar1" Position="Top" runat="server">
                            <Items>
                                <f:Button ID="btnNew" runat="server" Icon="Accept" Hidden="true"
                                    EnablePostBack="true" Text="送货完成" OnClick="btnNew_Click">
                                </f:Button>
                                <f:ToolbarFill ID="ToolbarFill1" runat="server">
                                </f:ToolbarFill>
                                <f:Button ID="btnQuery" runat="server" Icon="Reload" Text="查询刷新"
                                    OnClick="btnQuery_Click">
                                </f:Button>
                            </Items>
                        </f:Toolbar>
                    </Toolbars>
                    <Columns>
                        <f:WindowField ColumnID="editField" HeaderText="派工" TextAlign="Center" Icon="DateEdit" ToolTip="安排送货日期和人员"
                            WindowID="Window1" Title="安排送货日期和人员" DataIFrameUrlFields="ID" DataIFrameUrlFormatString="~/Contract/ContractSendingEdit.aspx?id={0}"
                            Width="50px" />
                        <f:TemplateField Width="60px">
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Width="60px" Text='<%# Eval("IsUrgent").ToString() =="1"?"加急":"" %>' ForeColor="Red"></asp:Label>
                            </ItemTemplate>
                        </f:TemplateField>
                        <f:TemplateField Width="60px" HeaderText="状态">
                            <ItemTemplate>
                                <asp:Label ID="Label3" runat="server" Width="60px" Text='<%# GetOrderState(Eval("ContractState").ToString()) %>'
                                    ForeColor='<%# GetColor(Eval("ContractState").ToString()) %>'></asp:Label>
                            </ItemTemplate>
                        </f:TemplateField>
                        <f:GroupField HeaderText="派工信息" TextAlign="Center">
                            <Columns>
                                <f:BoundField DataField="SendDate" HeaderText="送货时间" Width="100px" />
                                <f:TemplateField Width="80px" HeaderText="送货人员">
                                    <ItemTemplate>
                                        <asp:Label ID="Label2" runat="server" Width="80px" Text='<%# GetPerson(Eval("SendPerson").ToString()) %>'></asp:Label>
                                    </ItemTemplate>
                                </f:TemplateField>
                            </Columns>
                        </f:GroupField>
                        <f:GroupField HeaderText="订单信息" TextAlign="Center">
                            <Columns>
                                <f:BoundField DataField="ContractNO" HeaderText="订单号" Width="160px" SortField="ContractNO" />
                                <f:BoundField DataField="PerSendDate" HeaderText="预约送货日期" Width="130px" />
                                <f:BoundField DataField="PerInstalDate" HeaderText="预约安装日期" Width="130px" />
                                <f:BoundField DataField="ProjectName" HeaderText="客户地址" Width="200px" />
                                <f:BoundField DataField="CustomerName" HeaderText="客户名称" Width="180px" />
                                <f:BoundField DataField="ContactPhone" HeaderText="联系电话" Width="180px" />
                                <f:BoundField DataField="ContractDate" HeaderText="登记时间" Width="100px" />
                                <f:BoundField DataField="Remark" HeaderText="备注" Width="300px" />
                            </Columns>
                        </f:GroupField>
                    </Columns>
                </f:Grid>
            </Items>
        </f:Panel>
        <f:Window ID="Window1" CloseAction="Hide" runat="server" IsModal="true" Hidden="true"
            Target="Top" EnableResize="true" EnableMaximize="true" EnableIFrame="true" IFrameUrl="about:blank"
            Width="800px" Height="800px" OnClose="Window1_Close">
        </f:Window>
    </form>
</body>
</html>
