<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RPTBie.aspx.cs" Inherits="ZAJCZN.MIS.Web.RPTBie" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body style="height: 500px; margin: 0">
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" AutoSizePanelID="RegionPanel1" runat="server"></f:PageManager>
        <f:RegionPanel runat="server">
            <Toolbars>
                <f:Toolbar runat="server">
                    <Items>
                        <f:DropDownList Hidden="true" ID="ddlSearchWay" LabelWidth="70px" Label="查询方式" runat="server">
                            <f:ListItem Text="按天" Value="1" />
                            <f:ListItem Text="按周" Value="7" />
                            <f:ListItem Text="按月" Value="30" />
                            <f:ListItem Text="按季度" Value="90" />
                            <f:ListItem Text="按年" Value="365" />
                        </f:DropDownList>
                        <f:DatePicker ID="dpkStart" runat="server" LabelAlign="Right" Label="查询日期从"></f:DatePicker>
                        <f:DatePicker ID="dpkEnd" runat="server" LabelWidth="30px" Label="至"></f:DatePicker>
                        <f:Button ID="btnSearch" runat="server" Icon="SystemSearch" EnableAjax="false" EnablePostBack="true" OnClick="btnSearch_Click" Text="查询"></f:Button>
                        <f:ToolbarFill runat="server"></f:ToolbarFill>
                        <f:Button runat="server" ID="btnExcel" OnClick="btnExcel_Click" EnableAjax="false" Icon="PageExcel" Text="导出报表"></f:Button>
                    </Items>
                </f:Toolbar>
            </Toolbars>
        </f:RegionPanel>
        <asp:HiddenField ID="hfBarOptions" runat="server" />
        <asp:HiddenField ID="hfTitle" Value="未查询到数据" runat="server" />
    </form>
    <div id="container" style="height: 100%;"></div>
    <script src="../res/js/echarts.min.js"></script>
    <script>
            var dom = $("#container");
            var myChart = echarts.init(dom[0]);
            var Datas = "[" + $("#hfBarOptions").val() + "]";
            var data = JSON.parse(Datas);
            var title = $("#hfTitle").val();

            var count_num = 0;
            for (var m in data) {
                count_num = count_num + data[m]['value']
            }
            for (var n in data) {
                data[n]['name'] = data[n]['name'] + ' ' + ((data[n]['value'] / count_num) * 100).toFixed(1) + '%'
            }

            option = {
                title: {
                    text: '菜品类别销售占比图',
                    subtext: title,
                    x: 'center'
                },
                tooltip: {
                    trigger: 'item',
                    formatter: "{a} <br/>{b} : {c}"
                },
                legend: {
                    orient: 'vertical',
                    left: 'left',
                    top: 'center',
                    data: data
                },
                series: [
                    {
                        name: '菜品类别',
                        type: 'pie',
                        radius: '55%',
                        center: ['50%', '60%'],
                        data: data,
                        itemStyle: {
                            emphasis: {
                                shadowBlur: 10,
                                shadowOffsetX: 0,
                                shadowColor: 'rgba(0, 0, 0, 0.5)'
                            }
                        }
                    }
                ]
            };

            myChart.setOption(option);
    </script>
</body>
</html>
