<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExamBulletin.aspx.cs" Inherits="ZAJCZN.MIS.Web.ExamBulletin" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <f:PageManager ID="PageManager1" AutoSizePanelID="RegionPanel1" runat="server"></f:PageManager>
        <f:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
            <Regions>
                <f:Region ID="Region2" ShowBorder="false" ShowHeader="false" Position="Center" Layout="VBox" BoxConfigAlign="Stretch" BoxConfigPosition="Left" BodyPadding="5px 5px 5px 0" runat="server">
                    <Items>

                        <f:Panel ID="Panel2" ShowHeader="false" ShowBorder="false" Layout="Column" runat="server">
                            <Items>
                                <%--<f:DropDownList ID="ddlSubject" LabelWidth="100px" LabelAlign="Right" Label="科目" Width="400px"
                                    runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSubject_SelectedIndexChanged">
                                </f:DropDownList>
                                <f:DropDownList ID="ddlExam" LabelWidth="100px" LabelAlign="Right" Label="考试" Width="400px"
                                    runat="server">
                                </f:DropDownList>--%>
                                <f:Button ID="btnSearch" MarginLeft="20px" runat="server" ValidateForms="form1"
                                    OnClick="btnSearch_Click" Icon="Find" Text="查询">
                                </f:Button>
                            </Items>
                        </f:Panel>

                        <f:Grid ID="Grid1" runat="server" BoxFlex="1" ShowBorder="true" ShowHeader="false" PageSize="10"
                            EnableCheckBoxSelect="false" DataKeyNames="DeptID" AllowSorting="true" SortField="ID"
                            SortDirection="DESC" AllowPaging="false" >
                            <Toolbars>
                                <f:Toolbar ID="Toolbar1" Position="Top" runat="server">
                                    <Items>
                                        <f:ToolbarFill ID="ToolbarFill1" runat="server">
                                        </f:ToolbarFill>
                                        <f:Button ID="btnExpor" EnableAjax="false" DisableControlBeforePostBack="false"
                                            Icon="PageExcel" runat="server" Text="导出" OnClick="btnExpor_Click"
                                            Enabled="false">
                                        </f:Button>
                                    </Items>
                                </f:Toolbar>
                            </Toolbars>
                            <Columns>
                                <f:BoundField DataField="SubjectName" HeaderText="科目" Width="200px" TextAlign="Center" />
                                <f:BoundField DataField="TotalCount" HeaderText="考试人数" Width="200px" TextAlign="Center" />
                                <f:BoundField DataField="PassCount" HeaderText="合格人数" Width="200px" TextAlign="Center" />
                                <f:TemplateField ExpandUnusedSpace="true" TextAlign="Center" ColumnID="Gender" HeaderText="合格率">
                                    <ItemTemplate>
                                        <%-- Container.DataItem 的类型是 System.Data.DataRowView 或者用户自定义类型 --%>
                                        <%--<asp:Label ID="Label2" runat="server" Text='<%# GetGender(DataBinder.Eval(Container.DataItem, "Gender")) %>'></asp:Label>--%>
                                        <asp:Label ID="Label3" runat="server" Text='<%# (Convert.ToDecimal(Eval("PassRate"))*100).ToString()+"%" %>'></asp:Label>
                                    </ItemTemplate>
                                </f:TemplateField>
                                <%--<f:BoundField DataField="PassRate" SortField="PassRate" HeaderText="合格率" Width="200px" TextAlign="Center" />--%>
                            </Columns>
                        </f:Grid>
                    </Items>
                </f:Region>
            </Regions>
        </f:RegionPanel>
        <f:Window ID="Window1" CloseAction="Hide" runat="server" IsModal="true"
            Hidden="true" Target="Top" EnableResize="false" EnableMaximize="false"
            EnableIFrame="true" IFrameUrl="about:blank" Width="700px" Height="600px"
            OnClose="Window1_Close">
        </f:Window>
    </form>
</body>
</html>
