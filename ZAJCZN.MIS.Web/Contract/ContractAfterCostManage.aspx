<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ContractAfterCostManage.aspx.cs" Inherits="ZAJCZN.MIS.Web.ContractAfterCostManage" %>

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
                <f:Grid ID="Grid1" runat="server" ShowBorder="true" ShowHeader="false"
                    EnableCheckBoxSelect="true" DataKeyNames="ID"
                    AllowSorting="true" SortField="ID" SortDirection="DESC"
                    AllowPaging="false" IsDatabasePaging="true">
                    <Toolbars>
                        <f:Toolbar ID="Toolbar1" Position="Top" runat="server">
                            <Items>
                                <f:Button ID="btnDeleteHW" Text="删除选中行" Icon="Delete"
                                    runat="server" OnClick="btnDeleteHW_Click" ConfirmText="确定删除选择的记录信息?">
                                </f:Button>
                                <f:ToolbarFill ID="ToolbarFill1" runat="server">
                                </f:ToolbarFill>
                                <f:Button ID="btnNew" Text="新增费用" Icon="Add" EnablePostBack="false" runat="server">
                                </f:Button>
                            </Items>
                        </f:Toolbar>
                    </Toolbars>
                    <Columns>
                        <f:RowNumberField></f:RowNumberField>
                        <f:BoundField DataField="ApplyDate" HeaderText="录入时间" Width="100px" />
                        <f:BoundField DataField="PayMoney" ColumnID ="PayMoney" HeaderText="金额(元)" Width="120px" />
                        <f:BoundField DataField="Operator" HeaderText="录入人员" Width="100px" />
                        <f:BoundField DataField="Remark" HeaderText="备注" Width="400px" ExpandUnusedSpace="true" />
                    </Columns>
                </f:Grid>
            </Items>
        </f:Panel>
        <f:Window ID="Window1" CloseAction="Hide" runat="server" IsModal="true" Hidden="true"
            Target="Top" EnableResize="true" EnableMaximize="true" EnableIFrame="true" IFrameUrl="about:blank"
            Width="400px" Height="300px" OnClose="Window1_Close">
        </f:Window>
    </form>
</body>
</html>
