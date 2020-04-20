<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmployeeManager.aspx.cs" Inherits="ZAJCZN.MIS.Web.EmployeeManager" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server" />
        <f:Panel ID="Panel1" runat="server" BodyPadding="5px"
            ShowBorder="false" Layout="VBox" BoxConfigAlign="Stretch" BoxConfigPosition="Start"
            ShowHeader="false" Title="员工管理">
            <Items>
                <f:Form ID="Form2" runat="server" Height="36px" BodyPadding="5px" ShowHeader="false"
                    ShowBorder="false" LabelAlign="Right">
                    <Rows>
                        <f:FormRow ID="FormRow1" runat="server">
                            <Items>
                                <f:TwinTriggerBox ID="ttbSearchMessage" runat="server" ShowLabel="false" EmptyText="在员工名称、电话号码中搜索"
                                    Trigger1Icon="Clear" Trigger2Icon="Search" ShowTrigger1="false" OnTrigger2Click="ttbSearchMessage_Trigger2Click"
                                    OnTrigger1Click="ttbSearchMessage_Trigger1Click">
                                </f:TwinTriggerBox>
                                <f:DropDownList ID="ddlIsUsed" Label="人员类型" OnSelectedIndexChanged="rblEnableStatus_SelectedIndexChanged" AutoPostBack="true" runat="server">
                                    <f:ListItem Text="全部" Selected="true" Value="0" />
                                    <f:ListItem Text="销售人员" Value="销售人员" />
                                    <f:ListItem Text="测量人员" Value="测量人员" />
                                    <f:ListItem Text="设计人员" Value="设计人员" />
                                    <f:ListItem Text="安装人员" Value="安装人员" />
                                    <f:ListItem Text="售后服务" Value="售后服务" />
                                </f:DropDownList>
                            </Items>
                        </f:FormRow>
                    </Rows>
                </f:Form>
                <f:Grid ID="Grid1" runat="server" BoxFlex="1" ShowBorder="true" ShowHeader="false"
                    EnableCheckBoxSelect="false" DataKeyNames="ID" AllowSorting="true" OnSort="Grid1_Sort" SortField="UserType"
                    SortDirection="ASC" AllowPaging="true" IsDatabasePaging="true" OnDataBinding="Grid1_PreDataBound"
                    OnRowCommand="Grid1_RowCommand" OnPageIndexChange="Grid1_PageIndexChange"
                    EnableRowDoubleClickEvent="true" OnRowDoubleClick="Grid1_RowDoubleClick">
                    <Toolbars>
                        <f:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <f:Button ID="btnNew" runat="server" Icon="Add" EnablePostBack="false" Text="新增员工">
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
                        <f:RowNumberField TextAlign="Center" Width="35px" EnablePagingNumber="true" />
                        <f:BoundField DataField="UserType" SortField="UserType" Width="100px" HeaderText="人员类型" />
                        <f:BoundField DataField="EmployeeName" SortField="EmployeeName" Width="260px" HeaderText="员工名称" />
                        <f:TemplateField Width="60px" HeaderText="性别">
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Width="80px" Text='<%# GetSex(Eval("Sex").ToString()) %>'></asp:Label>
                            </ItemTemplate>
                        </f:TemplateField>
                        <f:BoundField DataField="ContractPhone" SortField="ContractPhone" Width="160px" HeaderText="联系电话" />
                        <f:BoundField DataField="ContractAddress" SortField="ContractAddress" Width="200px" HeaderText="联系地址" />
                        <f:TemplateField Width="60px" HeaderText="状态">
                            <ItemTemplate>
                                <asp:Label ID="Label3" runat="server" Width="80px" Text='<%# GetIsUsed(Eval("IsUsed").ToString()) %>'></asp:Label>
                            </ItemTemplate>
                        </f:TemplateField>
                        <f:BoundField DataField="Remark" SortField="Remark" Width="200px" HeaderText="备注" ExpandUnusedSpace="true" />
                        <f:WindowField ColumnID="editField" HeaderText="编辑" TextAlign="Center" Icon="Pencil" ToolTip="编辑"
                            WindowID="Window1" Title="编辑" DataIFrameUrlFields="ID" DataIFrameUrlFormatString="~/SysSet/EmployeeEdit.aspx?id={0}&action=edit"
                            Width="80px" />
                        <f:LinkButtonField ColumnID="deleteField" HeaderText="" Hidden="true" TextAlign="Center" Icon="Delete" ToolTip="删除"
                            ConfirmText="确定删除此记录？" ConfirmTarget="Top" CommandName="Delete" Width="40px" />
                    </Columns>
                </f:Grid>
            </Items>
        </f:Panel>
        <f:Window ID="Window1" runat="server" IsModal="true" Hidden="true" Target="Top" EnableResize="true"
            EnableMaximize="true" EnableIFrame="true" IFrameUrl="about:blank" Width="460px"
            Height="400px" OnClose="Window1_Close">
        </f:Window>
    </form>
</body>
</html>
