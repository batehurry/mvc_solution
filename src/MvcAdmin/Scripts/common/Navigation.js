
/*编辑器IE7bug*/
function FixIe7Bug() {
    var divs = $("div .edui-default");
    var i = 0;
    $.each(divs, function (index, item) {
        if ($(item).css("position") == "relative") {
            $(item).css("position", "static"); i++;
        }
    });
}

$(function () {
    tabClose();
    tabCloseEven();
});

function addTab(title, url) {
    if ($('#tabs').tabs('exists', title)) {
        $('#tabs').tabs('select', title); //选中并刷新
        var currTab = $('#tabs').tabs('getSelected');
        var url = $(currTab.panel('options').content).attr('src');
        if (url != undefined && currTab.panel('options').title != 'Home') {
            $('#tabs').tabs('update', {
                tab: currTab,
                options: {
                    content: createFrame(url)
                }
            })
        }
    } else {
        var content = createFrame(url);
        $('#tabs').tabs('add', {
            title: title,
            content: content,
            closable: true
        });
    }
    tabClose();
}

function addTabById(id, title, url) {
    if ($('#' + id).tabs('exists', title)) {
        $('#' + id).tabs('select', title); //选中并刷新
        var currTab = $('#' + id).tabs('getSelected');
        var url = $(currTab.panel('options').content).attr('src');
        if (url != undefined && currTab.panel('options').title != 'Home') {
            $('#' + id).tabs('update', {
                tab: currTab,
                options: {
                    content: createFrame(url)
                }
            })
        }
    } else {
        var content = createFrame(url);
        $('#' + id).tabs('add', {
            title: title,
            content: content,
            closable: true
        });
    }
    tabClose();
}



function createFrame(url) {
    var s = '<iframe scrolling="auto" frameborder="0"  src="' + url + '" style="width:100%;height:100%;"></iframe>';
    return s;
}

function tabClose() {
    /*双击关闭TAB选项卡*/
    $(".tabs-inner").dblclick(function () {
        var subtitle = $(this).children(".tabs-closable").text();
        $('#tabs').tabs('close', subtitle);
    })
    /*为选项卡绑定右键*/
    $(".tabs-inner").bind('contextmenu', function (e) {
        $('#mm').menu('show', {
            left: e.pageX,
            top: e.pageY
        });

        var subtitle = $(this).children(".tabs-closable").text();

        $('#mm').data("currtab", subtitle);
        $('#tabs').tabs('select', subtitle);
        return false;
    });
}
//绑定右键菜单事件
function tabCloseEven() {
    //刷新
    $('#mm-tabupdate').click(function () {
        var currTab = $('#tabs').tabs('getSelected');
        var url = $(currTab.panel('options').content).attr('src');
        var home = currTab.panel('options').title;
        if (home == "我的桌面") {
            //location.href = "Default.aspx";
            var iframe = $('#tabs iframe');
            for (var i = 0; i < iframe.length; i++) {
                var if3 = $(iframe[i]).attr("id");
                var src = $("#" + if3 + "").attr("src");
                if (src != undefined) {
                    var tempSrc = src.substr(0, src.lastIndexOf("&"));
                    var date = "&date=" + new Date();
                    src = tempSrc != "" ? (tempSrc + date) : (src + date);
                    $("#" + if3 + "").attr("src", src);
                }
            }
        }
        if (url != undefined && home != '我的桌面') {
            $('#tabs').tabs('update', {
                tab: currTab,
                options: {
                    content: createFrame(url)
                }
            })
        }
    })
    //关闭当前
    $('#mm-tabclose').click(function () {
        var currtab_title = $('#mm').data("currtab");
        $('#tabs').tabs('close', currtab_title);
    })
    //全部关闭
    $('#mm-tabcloseall').click(function () {
        $('.tabs-inner span').each(function (i, n) {
            var t = $(n).text();
            if (t != '我的桌面') {
                $('#tabs').tabs('close', t);
            }
        });
    });
    //关闭除当前之外的TAB
    $('#mm-tabcloseother').click(function () {
        var prevall = $('.tabs-selected').prevAll();
        var nextall = $('.tabs-selected').nextAll();
        if (prevall.length > 0) {
            prevall.each(function (i, n) {
                var t = $('a:eq(0) span', $(n)).text();
                if (t != '我的桌面') {
                    $('#tabs').tabs('close', t);
                }
            });
        }
        if (nextall.length > 0) {
            nextall.each(function (i, n) {
                var t = $('a:eq(0) span', $(n)).text();
                if (t != 'Home') {
                    $('#tabs').tabs('close', t);
                }
            });
        }
        return false;
    });
    //关闭当前右侧的TAB
    $('#mm-tabcloseright').click(function () {
        var nextall = $('.tabs-selected').nextAll();
        if (nextall.length == 0) {
            //msgShow('系统提示','后边没有啦~~','error');
            alert('后边没有啦~~');
            return false;
        }
        nextall.each(function (i, n) {
            var t = $('a:eq(0) span', $(n)).text();
            $('#tabs').tabs('close', t);
        });
        return false;
    });
    //关闭当前左侧的TAB
    $('#mm-tabcloseleft').click(function () {
        var prevall = $('.tabs-selected').prevAll();
        if (prevall.length == 0) {
            alert('到头了，前边没有啦~~');
            return false;
        }
        prevall.each(function (i, n) {
            var t = $('a:eq(0) span', $(n)).text();
            $('#tabs').tabs('close', t);
        });
        return false;
    });

    //退出
    $("#mm-exit").click(function () {
        $('#mm').menu('hide');
    })
}
$(function () {
    $(".cs-navi-tab").click(function () {
        var $this = $(this);
        var href = $this.attr('src');
        var title = $this.text();
        addTab(title, href);
    });

    var themes = {
        'gray': 'Content/easyui-1.3.4/themes/gray/easyui.css',
        'black': 'Content/easyui-1.3.4/themes/black/easyui.css',
        'bootstrap': 'Content/easyui-1.3.4/themes/bootstrap/easyui.css',
        'default': 'Content/easyui-1.3.4/themes/default/easyui.css',
        'metro': 'Content/easyui-1.3.4/themes/metro/easyui.css'
    };

    var skins = $('.li-skinitem span').click(function () {
        var $this = $(this);
        if ($this.hasClass('cs-skin-on')) return;
        skins.removeClass('cs-skin-on');
        $this.addClass('cs-skin-on');
        var skin = $this.attr('rel');
        $('#swicth-style').attr('href', themes[skin]);
        setCookie('cs-skin', skin);
        skin == 'dark-hive' ? $('.cs-north-logo').css('color', '#FFFFFF') : $('.cs-north-logo').css('color', '#000000');
    });

    if (getCookie('cs-skin')) {
        var skin = getCookie('cs-skin');
        $('#swicth-style').attr('href', themes[skin]);
        $this = $('.li-skinitem span[rel=' + skin + ']');
        $this.addClass('cs-skin-on');
        skin == 'dark-hive' ? $('.cs-north-logo').css('color', '#FFFFFF') : $('.cs-north-logo').css('color', '#000000');
    }
    //此处是扩展tree的两个方法.
    //$.extend($.fn.tree.methods, {
    //    getCheckedExt: function (jq) {//扩展getChecked方法,使其能实心节点也一起返回
    //        var checked = $(jq).tree("getChecked");
    //        var checkbox2 = $(jq).find("span.tree-checkbox2").parent();
    //        $.each(checkbox2, function () {
    //            var node = $.extend({}, $.data(this, "tree-node"), {
    //                target: this
    //            });
    //            checked.push(node);
    //        });
    //        return checked;
    //    },
    //    getSolidExt: function (jq) {//扩展一个能返回实心节点的方法
    //        var checked = [];
    //        var checkbox2 = $(jq).find("span.tree-checkbox2").parent();
    //        $.each(checkbox2, function () {
    //            var node = $.extend({}, $.data(this, "tree-node"), {
    //                target: this
    //            });
    //            checked.push(node);
    //        });
    //        return checked;
    //    }
    //});
    //easyui 释放iframe内存
    //$.fn.panel.defaults = $.extend({}, $.fn.panel.defaults, {
    //    onBeforeDestroy: function () {
    //        var frame = $('iframe', this);
    //        if (frame.length > 0) {
    //            frame[0].contentWindow.document.write('');
    //            frame[0].contentWindow.close();
    //            frame.remove();

    //            if ($.browser.msie) {
    //                CollectGarbage();
    //            }
    //        }
    //    }
    //});
});

function setCookie(name, value) {//两个参数，一个是cookie的名子，一个是值
    var Days = 30; //此 cookie 将被保存 30 天
    var exp = new Date();    //new Date("December 31, 9998");
    exp.setTime(exp.getTime() + Days * 24 * 60 * 60 * 1000);
    document.cookie = name + "=" + escape(value) + ";expires=" + exp.toGMTString();
}

function getCookie(name) {//取cookies函数        
    var arr = document.cookie.match(new RegExp("(^| )" + name + "=([^;]*)(;|$)"));
    if (arr != null) return unescape(arr[2]); return null;
}

//新建选项卡
function newTabs(title, url) {
    var jq = top.jQuery;
    if (jq("#tabs").tabs('exists', title)) {
        jq("#tabs").tabs('select', title);
        var tab = jq("#tabs").tabs('getTab', title);
        //var url = $(tab.panel('options').content).attr('src');
        jq("#tabs").tabs('update', {
            tab: tab,
            options: {
                content: createFrame(url)
            }
        });
    } else {
        var tab = jq("#tabs").tabs('getSelected');
        var content = '<iframe scrolling="auto" frameborder="0" parentName=' + tab.panel('options').title + '  src=' + url + ' style="width:100%;height:100%;"></iframe>';
        jq("#tabs").tabs('add', {
            title: title,
            content: content,
            //iconCls: 'icon-reload',
            closable: true
        });
    }
    var tab = jq('#tabs').tabs('getTab', title);
    tab.panel('panel').css('padding', '1px');
}
/**操作成功 关闭当前选项卡 并且刷新特定的选项卡**/
function tabCloaseLoad() {
    var jq = top.jQuery;
    var selectTab = jq('#tabs').tabs('getSelected'); //获取选中tabs
    var parentName = $(selectTab.panel('options').content).attr('parentName');
    //根据名称 获取特定的选项卡
    var tab = jq("#tabs").tabs('getTab', parentName);
    var url = $(tab.panel('options').content).attr('src');
    jq("#tabs").tabs('update', {
        tab: tab,
        options: {
            content: createFrame(url)
        }
    });
    //关闭当前选项卡
    jq("#tabs").tabs('close', selectTab.panel('options').title);
}
//点击返回按钮关闭选项卡
function tabBtnCloas() {
    var jq = top.jQuery;
    var tab = jq('#tabs').tabs('getSelected');
    jq("#tabs").tabs('close', tab.panel('options').title);
}
///顶部左右居中 消息提示
function topCenterMsg(title, msg, timeout, showType) {
    $.messager.show({
        title: title,
        msg: msg,
        timeout: timeout,
        showType: showType,
        style: {
            right: '',
            top: 2,
            bottom: ''
        }
    });
}
function tempTest() {

    var jq = top.jQuery;
    var selectTab = jq('#tabs').tabs('getSelected'); //获取选中tabs
    //var parentName = $(selectTab.panel('options').content).attr('parentName');
    //根据名称 获取特定的选项卡
    var tab = jq("#tabs").tabs('getTab', '外出登记');
    var url = "/DMTask/OutRecordList.aspx?menuId=81";
    jq("#tabs").tabs('update', {
        tab: tab,
        options: {
            content: createFrame(url)
        }
    });
    //关闭当前选项卡
    jq("#tabs").tabs('close', selectTab.panel('options').title);

}
