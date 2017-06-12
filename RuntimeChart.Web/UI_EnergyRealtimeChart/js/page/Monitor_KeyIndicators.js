var OrganizationTreeFlag = true;
var SelectOrganizationId = "";
$(function () {
    $('#Mainlayout').layout('panel', 'center').scroll(function (myScroll) {
        //alert(myScroll.target.scrollLeft + "||" + myScroll.target.scrollTop);
        if (ChartObject.length > 0) {
            for (var i = 0; i < ChartObject.length; i++) {
                var m_ChartLabelLeft = $('#ChartContent_' + i).position().left;
                var m_ChartLabelTop = $('#ChartContent_' + i).position().top;
                $('#ChartValueTip_' + i).css('left', m_ChartLabelLeft + 15);
                $('#ChartValueTip_' + i).css('top', m_ChartLabelTop + 15);
            }
        }
    })
    InitComponent();

    //GetChartData();
    $.parser.parse($('#Mainlayout').layout('panel', 'center'));
});
function InitComponent() {
    $.ajax({
        type: "POST",
        url: "Monitor_KeyIndicators.aspx/GetOrganizationTree",
        data: "",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var m_MsgData = jQuery.parseJSON(msg.d);
            if (m_MsgData != null && m_MsgData != undefined) {
                if (OrganizationTreeFlag == true) {
                    ///////////自动生成Html/////////  
                    initializeOrganisationTree(m_MsgData);
                    OrganizationTreeFlag = false;
                }
                else {
                    $('#ComboTree_OrganizationIdF').combotree('loadData', m_MsgData);
                }
            }
        },
        error: function (msg) {
        }
    });
}
// 初始化组织结构树
function initializeOrganisationTree(jsonData) {
    $('#ComboTree_OrganizationIdF').combotree({
        data: jsonData,
        animate: true,
        lines: true,
        id: 'id',
        text: 'text',
        required: false,
        panelHeight: 330,
        onLoadSuccess: function (node, data) {
            var m_ChieldrenNode = $(this).tree("getChildren");
            if (m_ChieldrenNode != null && m_ChieldrenNode != undefined) {
                for (var i = 0; i < m_ChieldrenNode.length; i++) {
                    if ($(this).tree('isLeaf', m_ChieldrenNode[i].target)) {
                        $('#ComboTree_OrganizationIdF').combotree('setValue', m_ChieldrenNode[i].id);
                        SelectOrganizationId = m_ChieldrenNode[i].id;
                        InitChartHtml(4, 180, 20, 300000);
                        break;
                    }
                }
            }
        },
        onBeforeSelect: function (node) {
            if (!$(this).tree('isLeaf', node.target)) {
                alert("请选择到最下层节点!");
                return false;
            }
        },
        onSelect: function (node) {
            SelectOrganizationId = node.id;
            InitChartHtml(4, 180, 20, 300000);
        },
    });
}