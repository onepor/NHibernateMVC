<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PersonExam.aspx.cs" Inherits="ZAJCZN.MIS.Web.PersonExam" %>

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
            ShowHeader="false" Title="成绩">
            <Items>

                <f:Form ID="Form2" runat="server" Height="36px" BodyPadding="5px" ShowHeader="false"
                    ShowBorder="false" LabelAlign="Right">
                    <Rows>
                        <f:FormRow ID="FormRow1" runat="server">
                            <Items>
                                <f:TwinTriggerBox ID="ttbSearchMessage" runat="server" ShowLabel="false" EmptyText="通过姓名、工作单位、职务、考试计划、科目搜索"
                                    Trigger1Icon="Clear" Trigger2Icon="Search" ShowTrigger1="false" OnTrigger2Click="ttbSearchMessage_Trigger2Click"
                                    OnTrigger1Click="ttbSearchMessage_Trigger1Click">
                                </f:TwinTriggerBox>
                            </Items>
                        </f:FormRow>
                    </Rows>
                </f:Form>

                <f:Grid ID="Grid1" runat="server" BoxFlex="1" ShowBorder="true" ShowHeader="false"
                    EnableCheckBoxSelect="false" DataKeyNames="ID" AllowSorting="true"
                    OnSort="Grid1_Sort" SortField="ID" SortDirection="ASC" AllowPaging="true" IsDatabasePaging="true"
                    OnDataBinding="Grid1_PreDataBound"
                    OnPageIndexChange="Grid1_PageIndexChange"
                    EnableRowDoubleClickEvent="false">

                    <Toolbars>
                        <f:Toolbar ID="ToolbarExpor" runat="server">
                            <Items>
                                <f:ToolbarFill ID="ToolbarFill1" runat="server">
                                </f:ToolbarFill>
                                <f:Button ID="btnExpor" runat="server" EnableAjax="false" DisableControlBeforePostBack="false" Icon="ApplicationGet" Text="导出" OnClick="btnExpor_Click">
                                </f:Button>
                            </Items>
                        </f:Toolbar>
                    </Toolbars>
                    <PageItems>
                        <f:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                        </f:ToolbarSeparator>
                        <f:ToolbarText ID="ToolbarText1" runat="server" Text="每页记录数：">
                        </f:ToolbarText>
                        <f:DropDownList ID="ddlGridPageSize" Width="90px" AutoPostBack="true" OnSelectedIndexChanged="ddlGridPageSize_SelectedIndexChanged"
                            runat="server">
                            <f:ListItem Text="10" Value="10" />
                            <f:ListItem Text="20" Value="20" />
                            <f:ListItem Text="50" Value="50" />
                            <f:ListItem Text="100" Value="100" />
                        </f:DropDownList>
                    </PageItems>

                    <Columns>
                        <f:RowNumberField TextAlign="Center" Width="40px" HeaderText="序号" EnablePagingNumber="true" />
                        <f:BoundField DataField="Name" SortField="Name" HeaderText="姓名" Width="60px" TextAlign="Center" />
                        <f:BoundField DataField="WorkPlace" SortField="WorkPlace" HeaderText="工作单位" Width="200px" TextAlign="Center" />
                        <f:BoundField DataField="Position" SortField="Position" HeaderText="职务" Width="100px" TextAlign="Center" />
                        <f:BoundField DataField="ExamName" SortField="ExamName" HeaderText="考试计划表" Width="100px" TextAlign="Center" />
                        <f:BoundField DataField="SubjectName" SortField="SubjectName" HeaderText="科目" Width="100px" TextAlign="Center" />
                        <f:BoundField DataField="Score" SortField="Score" HeaderText="成绩" Width="140px" TextAlign="Center" />
                        <f:BoundField DataField="ExamState" SortField="ExamState" HeaderText="考试状态" Width="160px" TextAlign="Center" />
                    </Columns>
                </f:Grid>
            </Items>
        </f:Panel>
        <f:Window ID="Window1" runat="server" IsModal="true" Hidden="true" Target="Top" EnableResize="true"
            EnableMaximize="true" EnableIFrame="true" IFrameUrl="about:blank" Width="460px"
            Height="350px" OnClose="Window1_Close">
        </f:Window>
    </form>
</body>
</html>
