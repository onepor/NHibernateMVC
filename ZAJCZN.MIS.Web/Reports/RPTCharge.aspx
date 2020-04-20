<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RPTCharge.aspx.cs" Inherits="ZAJCZN.MIS.Web.RPTCharge" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server" />
        <f:Panel ID="Panel1" runat="server" BodyPadding="5px"
            ShowBorder="false" Layout="VBox" BoxConfigAlign="Stretch" BoxConfigPosition="Start"
            ShowHeader="false" Title="挂账报表">
            <Items>
                <f:Form ID="Form2" runat="server" Height="36px" BodyPadding="5px" ShowHeader="false"
                    ShowBorder="false" LabelAlign="Right">
                    <Rows>
                        <f:FormRow ID="FormRow1" runat="server">
                            <Items>
                                <f:DropDownList ID="ddlCharge" runat="server"></f:DropDownList>
                                <f:DatePicker runat="server" LabelWidth="25px"  Label="从" ID="dpkStarttime" ></f:DatePicker>
                                <f:DatePicker runat="server" LabelWidth="25px" Label="至" ID="dpkEndtime" ></f:DatePicker>
                                <f:Button runat="server" ID="btnSearch" Icon="SystemSearch" EnablePostBack="true" EnableAjax="false" OnClick="btnSearch_Click" Text="查询"></f:Button>
                                <f:Button runat="server" ID="btnExcel" OnClick="btnExcel_Click" EnableAjax="false" Icon="PageExcel" Text="导出报表"></f:Button>
                            </Items>
                        </f:FormRow>
                    </Rows>
                </f:Form>
                <f:Grid ID="Grid1" runat="server" BoxFlex="1" ShowBorder="true" ShowHeader="false"
                     DataKeyNames="ID,Name" AllowSorting="true" OnSort="Grid1_Sort" SortField="ID"
                    SortDirection="ASC" AllowPaging="true" IsDatabasePaging="true"
                    OnRowCommand="Grid1_RowCommand" OnPageIndexChange="Grid1_PageIndexChange">
                    <PageItems>
                        <f:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                        </f:ToolbarSeparator>
                        <f:ToolbarText ID="ToolbarText1" runat="server" Text="每页记录数：">
                        </f:ToolbarText>
                        <f:DropDownList ID="ddlGridPageSize" Width="80px" AutoPostBack="true" OnSelectedIndexChanged="ddlGridPageSize_SelectedIndexChanged"
                            runat="server">
                            <f:ListItem Text="10" Value="10" />
                            <f:ListItem Text="20" Value="20" Selected="true" />
                            <f:ListItem Text="50" Value="50" />
                            <f:ListItem Text="100" Value="100" />
                        </f:DropDownList>
                    </PageItems>
                    <Columns>
                        <f:RowNumberField TextAlign="Center" Width="90px" EnablePagingNumber="true" HeaderText="挂账金额排名" />
                        <f:BoundField DataField="Charge" SortField="Charge" Width="180px" HeaderText="挂账人" />
                        <f:BoundField DataField="Amount" SortField="Amount" Width="200px" HeaderText="挂账总金额" />
                        <f:LinkButtonField ColumnID="Detail" HeaderText="查看明细" TextAlign="Center" Icon="TABLE" ToolTip="查看明细"
                              CommandName="Detail" Width="80px" />
                    </Columns>
                </f:Grid>
            </Items>
        </f:Panel>
    </form>
</body>
</html>
