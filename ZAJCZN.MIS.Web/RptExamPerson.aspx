<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RptExamPerson.aspx.cs" Inherits="XGZDAPP.Web.Exam.Report.RptExamPerson" %>

<!DOCTYPE html>


<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>考试成绩报表</title>
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
                                <f:DropDownList ID="ddlSubject" LabelWidth="100px" LabelAlign="Right" Label="科目" Width="400px"
                                    runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSubject_SelectedIndexChanged">
                                </f:DropDownList>
                                <f:DropDownList ID="ddlExam" LabelWidth="100px" LabelAlign="Right" Label="考试" Width="400px"
                                    runat="server">
                                </f:DropDownList>
                                <f:Button ID="btnSearch" MarginLeft="20px" runat="server" ValidateForms="form1"
                                    OnClick="btnSearch_Click" Icon="Find" Text="查询">
                                </f:Button>
                            </Items>
                        </f:Panel>
                        <f:Grid ID="Grid1" runat="server" BoxFlex="1" ShowBorder="true" ShowHeader="false" PageSize="10"
                            EnableCheckBoxSelect="false" DataKeyNames="DeptID" AllowSorting="true" SortField="ID"
                            SortDirection="DESC" AllowPaging="false" OnRowCommand="Grid1_RowCommand">
                            <Toolbars>
                                <f:Toolbar ID="Toolbar1" Position="Top" runat="server">
                                    <Items>
                                        <f:ToolbarFill ID="ToolbarFill1" runat="server">
                                        </f:ToolbarFill>
                                        <f:Button ID="btnExpor" EnableAjax="false" DisableControlBeforePostBack="false"
                                            Icon="PageExcel" runat="server" Text="导出部门汇总" OnClick="btnExpor_Click"
                                            Enabled="false">
                                        </f:Button>
                                    </Items>
                                </f:Toolbar>
                            </Toolbars>
                            <Columns>
                                <f:RowNumberField TextAlign="Center" Width="60px" HeaderText="序号" EnablePagingNumber="true" />
                                <f:BoundField DataField="Name" SortField="Name" HeaderText="姓名" Width="160px" TextAlign="Center" />
                                <f:BoundField DataField="WorkPlace" SortField="WorkPlace" HeaderText="工作单位" Width="200px" TextAlign="Center" />
                                <f:BoundField DataField="Position" SortField="Position" HeaderText="职务" Width="200px" TextAlign="Center" />
                                <f:BoundField DataField="ExamName" SortField="ExamName" HeaderText="考试计划表" Width="210px" TextAlign="Center" />
                                <f:BoundField DataField="SubjectName" SortField="SubjectName" HeaderText="科目" Width="200px" TextAlign="Center" />
                                <f:TemplateField Width="150px" HeaderText="成绩" SortField="Score">
                                    <ItemTemplate>
                                        <asp:Label ID="Label1" runat="server" Width="80px" Text='<%# Eval("ExamState").ToString()=="4"?"缺考":decimal.Parse(Eval("Score").ToString()).ToString("0.00") %>'></asp:Label>
                                    </ItemTemplate>
                                </f:TemplateField>
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
