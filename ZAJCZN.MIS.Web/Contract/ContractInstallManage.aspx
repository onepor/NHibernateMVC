<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ContractInstallManage.aspx.cs" Inherits="ZAJCZN.MIS.Web.ContractInstallManage" %>

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
                        <f:DatePicker ID="dpStartDate" LabelWidth="120px" Label="安装时间" Width="260px"
                            runat="server" LabelAlign="Right">
                        </f:DatePicker>
                        <f:DatePicker ID="dpEndDate" LabelWidth="10px" Label="~" Width="150px" runat="server"
                            CompareControl="dpStartDate" CompareOperator="GreaterThan" CompareMessage="结束日期应该大于开始日期！">
                        </f:DatePicker>
                        <f:DropDownList ID="ddlState" runat="server" LabelWidth="100px" Label="订单状态" Width="200px"
                            LabelAlign="Right">
                            <f:ListItem Text="--全部--" Value="" Selected="true" />
                            <f:ListItem Text="未派工" Value="7" />
                            <f:ListItem Text="已派工" Value="8" />
                            <f:ListItem Text="安装完成" Value="9" />
                            <f:ListItem Text="质保中" Value="10" />
                            <f:ListItem Text="售后中" Value="11" />
                        </f:DropDownList>
                        <f:Button ID="btnSearch" MarginLeft="20px" runat="server" OnClick="btnSearch_Click" Icon="Find" Text="查询"></f:Button>
                    </Items>
                </f:Panel>
                <f:Grid ID="Grid1" runat="server" ShowBorder="true" ShowHeader="false"
                    EnableCheckBoxSelect="true" DataKeyNames="ID" OnPreDataBound="Grid1_PreDataBound"
                    AllowSorting="true" OnSort="Grid1_Sort" SortField="ID" SortDirection="DESC"
                    AllowPaging="false" IsDatabasePaging="true" OnPreRowDataBound="Grid1_PreRowDataBound">
                    <Toolbars>
                        <f:Toolbar ID="Toolbar1" Position="Top" runat="server">
                            <Items>
                                <f:Button ID="btnNew" runat="server" Icon="Accept"
                                    EnablePostBack="true" Text="安装完成" OnClick="btnNew_Click">
                                </f:Button>
                                <f:Button ID="btnChangeEnableUsers" Icon="RecordRed" EnablePostBack="false" runat="server"
                                    Text="设置售后状态">
                                    <Menu ID="Menu1" runat="server">
                                        <f:MenuButton ID="btnEnableUsers" OnClick="btnEnableUsers_Click" runat="server" Text="售后">
                                        </f:MenuButton>
                                        <f:MenuButton ID="btnDisableUsers" OnClick="btnDisableUsers_Click" runat="server"
                                            Text="售后完成">
                                        </f:MenuButton>
                                    </Menu>
                                </f:Button>
                                <f:ToolbarFill ID="ToolbarFill1" runat="server">
                                </f:ToolbarFill>
                            </Items>
                        </f:Toolbar>
                    </Toolbars>
                    <Columns>
                        <f:WindowField ColumnID="editField" HeaderText="派工" TextAlign="Center" Icon="DateEdit" ToolTip="安排安装日期和人员"
                            WindowID="Window1" Title="安排安装日期和人员" DataIFrameUrlFields="ID" DataIFrameUrlFormatString="~/Contract/ContractInstallEdit.aspx?id={0}"
                            Width="50px" />
                        <f:WindowField ColumnID="sheditField" HeaderText="售后成本" TextAlign="Center" Icon="MoneyAdd" ToolTip="售后成本录入"
                            WindowID="Window2" Title="售后成本录入" DataIFrameUrlFields="ID" DataIFrameUrlFormatString="~/Contract/ContractAfterCostManage.aspx?id={0}"
                            Width="80px" />
                        <f:TemplateField Width="80px" HeaderText="状态">
                            <ItemTemplate>
                                <asp:Label ID="Label3" runat="server" Width="80px" Text='<%# GetOrderState(Eval("ContractState").ToString()) %>'
                                    ForeColor='<%# GetColor(Eval("ContractState").ToString()) %>'></asp:Label>
                            </ItemTemplate>
                        </f:TemplateField>
                        <f:TemplateField Width="60px">
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Width="60px" Text='<%# Eval("IsUrgent").ToString() =="1"?"加急":"" %>' ForeColor="Red"></asp:Label>
                            </ItemTemplate>
                        </f:TemplateField>
                        <f:GroupField HeaderText="派工信息" TextAlign="Center">
                            <Columns>
                                <f:BoundField DataField="InstalDate" HeaderText="安装时间" Width="100px" />
                                <f:TemplateField Width="80px" HeaderText="安装人员">
                                    <ItemTemplate>
                                        <asp:Label ID="Label2" runat="server" Width="80px" Text='<%# GetPerson(Eval("InstallPerson").ToString()) %>'></asp:Label>
                                    </ItemTemplate>
                                </f:TemplateField>
                            </Columns>
                        </f:GroupField>
                        <f:GroupField HeaderText="订单信息" TextAlign="Center">
                            <Columns>
                                <f:BoundField DataField="ContractNO" HeaderText="订单号" Width="160px" SortField="ContractNO" />
                                <f:TemplateField Width="110px" HeaderText="售后成本(元)">
                                    <ItemTemplate>
                                        <asp:Label ID="Label4" runat="server" Width="110px" Text='<%# GetAfterCost(Eval("ID").ToString()) %>' ForeColor="Red"></asp:Label>
                                    </ItemTemplate>
                                </f:TemplateField>
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
        <f:Window ID="Window2" CloseAction="Hide" runat="server" IsModal="true" Hidden="true"
            Target="Top" EnableResize="true" EnableMaximize="true" EnableIFrame="true" IFrameUrl="about:blank"
            Width="800px" Height="500px" OnClose="Window1_Close">
        </f:Window>
    </form>
</body>
</html>
