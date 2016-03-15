//获得文件后缀
function getFileExt(obj) {
    //isExist(obj);
    var ext = obj.substring(obj.lastIndexOf(".")).toLowerCase(); //获得文件后缀名
    //alert(ext);
    return ext;
}
//是否是文档
function isDoc(obj) {
    var ext = getFileExt(obj);
    if (ext == "" || ext.length == 0) {
        return false; 
    }
    if ((ext == ".xls" || ext == ".doc" || ext == ".docx" || ext == ".xlsx")) {    
       return true;
    }       
    return false;
}

//判断文件是否存在(IE)
function isExist(fileUrl) {
    var xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
    xmlhttp.open("GET", fileUrl, false);
    xmlhttp.send();
    var s = '';
    if (xmlhttp.readyState == 4) {
        if (xmlhttp.status == 200) {
            s += " 存在."; //url存在 
            return true;
        }
        else if (xmlhttp.status == 404) s += " 不存在."; //url不存在 
        else s += "";  //其他状态 
    }
    alert(s);
    return false;
}

//获得请求参数
function getQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return decodeURI(r[2]); return null;
}

//删除
function doDelete(action,idField) {
    var rows = $('#grid').datagrid('getSelections');
    if (rows.length <= 0) {
        alert('请选择要删除的记录'); return;
    }
    $.messager.confirm('确认', '您确认想要删除记录吗？', function (r) {
        if (r) {
            var id = '';
            $.each(rows, function (i, row) {
                id += row[idField] + ',';
            })
            id = id.substring(0, id.length - 1);
            //alert(id); return;
            $.ajax({
                url: action,
                type: 'post',
                data: { idList: id },
                success: function (data) {
                    if (data.result) {
                        $.messager.alert('提示', '删除成功！', 'info');
                        getGrid();
                    } else {
                        var info = '操作失败！';
                        if (data.info.length > 0) {
                            info = data.info;
                        }
                        $.messager.alert('提示', info, 'info');
                    }
                }
            })
        }
    });
}

//文本域字数
function textCounter(field, maxlimit) {
    // 参数：表单名字，限制字符；
    if (field.value.length > maxlimit)
        //如果元素区字符数大于最大字符数，按照最大字符数截断；
        field.value = field.value.substring(0, maxlimit);
    //else
    //    //在记数区文本框内显示剩余的字符数；
    //    countfield.value = maxlimit - field.value.length;
}

//计算时间差
function GetDateDiff(startTime, endTime, diffType) {
    //将xxxx-xx-xx的时间格式，转换为 xxxx/xx/xx的格式 
    startTime = startTime.replace(/\-/g, "/");
    endTime = endTime.replace(/\-/g, "/");

    //将计算间隔类性字符转换为小写
    diffType = diffType.toLowerCase();
    if (startTime == '') {
        var sTime = new Date();  //开始时间
    }
    else {
        var sTime = new Date(startTime);      //开始时间
    }
    if (endTime == '') {
        var eTime = new Date();  //结束时间
    } else {
        var eTime = new Date(endTime);  //结束时间
    }
    //作为除数的数字
    var divNum = 1;
    switch (diffType) {
        case "second":
            divNum = 1000;
            break;
        case "minute":
            divNum = 1000 * 60;
            break;
        case "hour":
            divNum = 1000 * 3600;
            break;
        case "day":
            divNum = 1000 * 3600 * 24;
            break;
        default:
            break;
    }
    return parseInt((eTime.getTime() - sTime.getTime()) / parseInt(divNum));
}
//扩展Date,日期格式化
Date.prototype.Format = function (format) {
    var o = {
        "M+": this.getMonth() + 1, //month
        "d+": this.getDate(), //day
        "h+": this.getHours(), //hour
        "m+": this.getMinutes(), //minute
        "s+": this.getSeconds(), //second
        "q+": Math.floor((this.getMonth() + 3) / 3), //quarter
        "S": this.getMilliseconds() //millisecond
    }

    if (/(y+)/.test(format)) {
        format = format.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    }

    for (var k in o) {
        if (new RegExp("(" + k + ")").test(format)) {
            format = format.replace(RegExp.$1, RegExp.$1.length == 1 ? o[k] : ("00" + o[k]).substr(("" + o[k]).length));
        }
    }
    return format;
}

function dateFormat(obj) {
    if (obj == null) {
        return '';
    }
    //var obj = "/Date(1437965679667)/";
    var date = eval(obj.replace(/\/Date\((\d+)\)\//gi, "new Date($1)"));
    return date.toLocaleString();
    //return date.Format('yyyy-MM-dd hh:mm:ss');
}