<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RPTBar.aspx.cs" Inherits="ZAJCZN.MIS.Web.RPTBar" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body style="height: 470px; margin: 0">
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" AutoSizePanelID="RegionPanel1" runat="server"></f:PageManager>
        <f:RegionPanel runat="server">
            <Toolbars>
                <f:Toolbar runat="server">
                    <Items>
                        <f:DropDownList ID="ddlSearchWay" LabelWidth="70px" Label="查询方式" runat="server">
                            <f:ListItem Text="按天" Value="1" Selected="true" />
                            <f:ListItem Text="按月" Value="30" />
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
        <asp:HiddenField ID="hfBarOptionsDates" runat="server" />
        <asp:HiddenField ID="hfBarOptionsGrowth" runat="server" />
        <asp:HiddenField ID="hfBarOptionsMoneys" runat="server" />
        <asp:HiddenField ID="hfBarOptionsFactpre" runat="server" />
        <asp:HiddenField ID="hfBarOptionsMax" runat="server" />
        <asp:HiddenField ID="hfBarOptionsUnit" runat="server" />
        <asp:HiddenField ID="hfBarOptionsyGrowthMax" runat="server" />
        <asp:HiddenField ID="hfBarOptionsyGrowthMin" runat="server" />
        <asp:HiddenField ID="hfTitle" Value="未查询到数据" runat="server" />
    </form>
    <div id="container" style="height: 100%;"></div>
    <script src="../res/js/echarts.min.js"></script>
    <script>
        var dom = $("#container");
        var myChart = echarts.init(dom[0]);
        var Dates = "[" + $("#hfBarOptionsDates").val() + "]";
        var Growth = $("#hfBarOptionsGrowth").val().split(",");
        var Moneys = $("#hfBarOptionsMoneys").val().split(",");
        var Factpre = $("#hfBarOptionsFactpre").val().split(",");
        var Max = $("#hfBarOptionsMax").val();
        var Unit = $("#hfBarOptionsUnit").val();
        var yGrowthMax = $("#hfBarOptionsyGrowthMax").val();
        var yGrowthMin = $("#hfBarOptionsyGrowthMin").val();

        var dates = JSON.parse(Dates);
        var growth = JSON.stringify(Growth);
        var moneys = JSON.stringify(Moneys);
        var factpre = JSON.stringify(Factpre);
        var title = $("#hfTitle").val();

        option = {
            title: {
                text: '销售金额统计图',
                subtext: title,
                x: 'center',
            },
            tooltip: {
                trigger: 'axis'
            },
            grid: {
                containLabel: true,
                left: '1%',
                right: '2%',
                top: '15%',
                x: 30
            },
            legend: {
                data: ['同比增涨', '营业金额', '实收金额'],
                top: 25,
                left: 150
            },
            xAxis: [{
                type: 'category',
                axisTick: {
                    alignWithLabel: true
                },
                data: dates
            }],
            yAxis: [{
                type: 'value',
                name: '增涨幅度',
                min: yGrowthMin,
                max: yGrowthMax,
                position: 'right',
                axisLabel: {
                    formatter: '{value} %'
                }
            }, {
                type: 'value',
                name:  Unit ,
                min: 0,
                max: Max ,
                position: 'left'
            }],
            //dataZoom: {
            //    show: true,
            //    start: 0,
            //    end: 50,
            //    maxSpan: 50
            //    // zoomLock: true
            //},
            series: [{
                name: '同比增涨',
                type: 'line',
                stack: '总量',
                label: {
                    normal: {
                        show: true,
                        position: 'top',
                    }
                },
                lineStyle: {
                    normal: {
                        width: 3,
                        shadowColor: 'rgba(0,0,0,0.4)',
                        shadowBlur: 10,
                        shadowOffsetY: 10
                    }
                },
                data: Growth
            }, {
                name: '营业金额',
                type: 'bar',
                yAxisIndex: 1,
                stack: '营业金额',
                label: {
                    normal: {
                        show: true,
                        position: 'top'
                    }
                },
                data: Moneys
            }, {
                name: '实收金额',
                type: 'bar',
                yAxisIndex: 1,
                stack: '实收金额',
                label: {
                    normal: {
                        show: true,
                        position: 'top'
                    }
                },
                data: Factpre
            }
            ]
        };
        myChart.setOption(option);
    </script>
</body>
</html>
