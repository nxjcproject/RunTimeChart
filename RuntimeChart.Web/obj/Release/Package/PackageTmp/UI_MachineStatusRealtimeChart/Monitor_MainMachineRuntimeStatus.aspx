<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Monitor_MainMachineRuntimeStatus.aspx.cs" Inherits="RuntimeChart.Web.UI_MachineStatusRealtimeChart.Monitor_MainMachineRuntimeStatus" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link rel="stylesheet" type="text/css" href="/lib/ealib/themes/gray/easyui.css" />
    <link rel="stylesheet" type="text/css" href="/lib/ealib/themes/icon.css" />
    <link rel="stylesheet" type="text/css" href="/lib/extlib/themes/syExtIcon.css" />
    <link rel="stylesheet" type="text/css" href="/lib/extlib/themes/syExtCss.css" />

    <link type="text/css" rel="stylesheet" href="/css/common/NormalPage.css" />
    <link type="text/css" rel="stylesheet" href="css/page/Monitor_MainMachineRuntimeStatus.css" />

    <script type="text/javascript" src="/lib/ealib/jquery.min.js" charset="utf-8"></script>
    <script type="text/javascript" src="/lib/ealib/jquery.easyui.min.js" charset="utf-8"></script>
    <script type="text/javascript" src="/lib/ealib/easyui-lang-zh_CN.js" charset="utf-8"></script>

    <!--[if gt IE 8]><script type="text/javascript" src="/lib/ealib/extend/easyUI.WindowsOverrange.js" charset="utf-8"></script><![endif]-->
    <!--[if !IE]>
    <script type="text/javascript" src="/lib/ealib/extend/easyUI.WindowsOverrange.js" charset="utf-8"></script>
    <!--<![endif]-->
    <!--[if lt IE 8 ]><script type="text/javascript" src="/js/common/json2.min.js"></script><![endif]-->
    <script type="text/javascript" src="/js/common/format/DateTimeFormat.js" charset="utf-8"></script>
    <script type="text/javascript" src="js/page/Monitor_MainMachineRuntimeStatus.js" charset="utf-8"></script>

</head>
<body>
    <div id="Mainlayout" class="easyui-layout" data-options="fit:true,border:false">
        <div class="easyui-panel" data-options="region:'center',border:false,onResize:function(width,height){SetDivPosization(width);}" style="overflow: auto; background-color: #ececec">
            <div id ="TableContentDiv">
                <table id="ChartContentTable">
                    
                </table>
            </div>
        </div>
        <div id="ab_ba_de2_fdaf_erf_fda" style ="width:300px;"></div>
    </div>
    <!--设备停机记录-->
    <div id="dlg_MachineHaltRecord" class="easyui-dialog">
        <table id="grid_MachineHaltRecord" class="easyui-datagrid" data-options="fit:true, rownumbers: true,striped:true, singleSelect:true, border:false">
            <thead>
                <tr>
                    <th data-options="field:'MachineHaltLogID',width:60, hidden:true">设备ID</th>
                    <th data-options="field:'HaltTime',width:120">停机时间</th>
                    <th data-options="field:'RecoverTime',width:120">重新开机时间</th>
                    <th data-options="field:'HaltLong',width:80">停机时长</th>
                    <th data-options="field:'ReasonText',width:150">停机原因</th>
                </tr>
            </thead>
        </table>
    </div>
    <form id="form1" runat="server">
        <div>
        </div>
    </form>
</body>
</html>
