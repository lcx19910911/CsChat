/*
 * 功能：弹窗提示
 * str: 提示内容,
 * obj: {
 *   //DIY颜色搭配: bgColor, textColor, contColor, contBgColor,
 *   //确认触发函数：sure: function() {},
 *   //取消触发函数：cancel: function() {},
 * }
 */
function alertVerify(str,obj) {
	var body = document.getElementsByTagName('html')[0];
	body.style.cssText = "overflow:hidden;height:100%;";
	if(document.getElementById('lVerify')) {
		document.getElementById('lVerifyCont').innerText = str;
		var ele = document.getElementById('lVerify');
		ele.style.display = 'block';
	} else {
		var alertBox = document.createElement('div');
		var box = document.createElement('div');
		var title = document.createElement('h2');
		var contBox = document.createElement('div');
		var btnBox = document.createElement('div');
		var sureBtn = document.createElement('span');
		var cancelBtn = document.createElement('span');
		var mask = document.createElement('div');

		var bgColor = obj.bgColor || "#85bdff",
			textColor = obj.textColor || "#fff",
			contColor = obj.contColor || "#333",
			contBgColor = obj.contBgColor || "#fff";

		alertBox.id = 'lVerify';
		alertBox.style.cssText = "position:fixed;top:0;width:100%;height:100%;z-index:1000;border:1px solid #ccc";
		box.style.cssText = "position:absolute;top:20%;width:" + document.body.offsetWidth*0.7 + "px;text-align:center;background:" + contBgColor + ";font-size:1.2rem;border-radius:0.3em 0.3em 0 0;overflow:hidden;";
		// console.log(document.body.offsetWidth * 0.7);

		title.style.cssText = "font-size:1em;background-color:" + bgColor + ";color:" + textColor + ";padding:0.4em 1em;box-sizing:border-box;text-align:left;";
		title.innerText = '提示！';
		box.appendChild(title);

		contBox.style.cssText = "color:#000;padding: 1.5em 1em;";
		contBox.id = 'lVerifyCont';
		box.appendChild(contBox);
		
		sureBtn.innerText = '确定';
		cancelBtn.innerText = '取消';
		sureBtn.style.cssText = "float:right;width:50%;background:" + bgColor + ";";
		cancelBtn.style.cssText = "float:left;width:50%;background:#ccc;";
		btnBox.appendChild(sureBtn);
		btnBox.appendChild(cancelBtn);
		btnBox.style.cssText = "overflow:hidden;padding:0;color:" + textColor + ";border-top:1px solid #eee;line-height:2.5em;";
		box.appendChild(btnBox);

		mask.style.cssText = "position:absolute;top:0;left:0;width:" + document.body.offsetWidth + "px;height:100%;background:rgba(0,0,0,0.6);";

		alertBox.appendChild(mask);
		alertBox.appendChild(box);

		body.appendChild(alertBox);
		contBox.style.color = contColor;
		contBox.innerText = str;

		box.style.left = (document.body.offsetWidth - box.offsetWidth) / 2 + 'px';
		box.style.top  = (window.screen.height - box.offsetHeight) * 0.9 / 2 + 'px';

		sureBtn.onclick = function() {
			alertBox.style.display = 'none';
			body.style.cssText = "overflow:visible;height:auto;";
			obj.sure();
		};
		cancelBtn.onclick = function() {
			alertBox.style.display = 'none';
			body.style.cssText = "overflow:visible;height:auto;";
			if(obj.cancel != null) {
				obj.cancel();
			}
		};
	}
}


/*
 * 功能：移动端图片放大、拖拽
 * selector: 通过 id选择器 获取到的 img标签！
 */ 
var touchScale = function(selector) {
    var startX, endX, scale, x1, x2, y1, y2, imgLeft, imgTop, imgWidth, imgHeight, dragLeft, dragTop,
        one = false,
        $touch = $(selector),
        originalWidth = $touch.width(),
        originalHeight = $touch.height(),
        baseScale = parseFloat(originalWidth/originalHeight),
        imgData = [];
    function siteData(name) {
        imgLeft = parseInt(name.css('left'));
        imgTop = parseInt(name.css('top'));
        imgWidth = name.width();
        imgHeight = name.height();
    }
    $(document).on('touchstart touchmove touchend', '#' + selector.id, function(event){
        event.preventDefault();
        var $me = $(selector),
            touch1 = event.originalEvent.targetTouches[0],  // 第一根手指touch事件
            touch2 = event.originalEvent.targetTouches[1],  // 第二根手指touch事件
            fingers = event.originalEvent.touches.length;   // 屏幕上手指数量
        //手指放到屏幕上的时候，还没有进行其他操作
        if (event.type == 'touchstart') {
            if (fingers == 2) {
                // 缩放图片的时候X坐标起始值
                startX = Math.abs(touch1.pageX - touch2.pageX);
                one = false;
            }
            else if (fingers == 1) {
                // 预存手指刚点击时的位置
                x1 = touch1.pageX;
                y1 = touch1.pageY;
                // 获取图片当前的位置
                imgLeft = $me.css('left');
                imgTop = $me.css('top');
                one = true;
            }
            siteData($me);
        }
        //手指在屏幕上滑动
        else if (event.type == 'touchmove') {
            if (fingers == 2) {
                // 缩放图片的时候X坐标滑动变化值
                endX = Math.abs(touch1.pageX - touch2.pageX);
                scale = endX - startX;
                $me.css({
                    'width' : originalWidth + scale,
                    'height' : (originalWidth + scale)/baseScale,
                    'left' : imgLeft,
                    'top' : imgTop
                });

            }else if (fingers == 1) {
                // 手指拖动图片实时的位置
                x2 = touch1.pageX;
                y2 = touch1.pageY;
                if (one) {
                    dragLeft = imgLeft + (x2 - x1);
                    dragTop = imgTop + (y2 - y1);
                    $me.css({
                        'left' : dragLeft,
                        'top' : dragTop,
                    });
                }

            }
        }
        //手指移开屏幕
        else if (event.type == 'touchend') {
            // 手指移开后保存图片的宽
            originalWidth = $touch.width();
            siteData($me);
        }
    });
};

function newGuid() {
    var guid = "";
    for (var i = 1; i <= 32; i++) {
        var n = Math.floor(Math.random() * 16.0).toString(16);
        guid += n;
    }
    return guid;
}

//获取Json的时间
function RenderTime(data) {
    var da = eval('new ' + data.replace('/', '', 'g').replace('/', '', 'g'));
    var minute=da.getMinutes();
    var second=da.getMinutes();
    return da.getFullYear() + "-" + (da.getMonth() + 1) + "-" + da.getDate() + " " + da.getHours() + ":" + (minute > 9 ? minute : "0" + minute) + ":" + (second > 9 ? second : "0" + second);
}

function CurentTime() {
    var now = new Date();
    var year = now.getFullYear();
    var month = now.getMonth() + 1;
    var day = now.getDate();
    var hh = now.getHours();
    var mm = now.getMinutes();
    var clock = year + "-";
    if (month < 10)
        clock += "0";
    clock += month + "-";
    if (day < 10)
        clock += "0";
    clock += day + " ";
    if (hh < 10)
        clock += "0";
    clock += hh + ":";
    if (mm < 10) clock += '0';
    clock += mm;
    return (clock);
}
function GetJsonTimeSpan(data)
{
    return parseInt(data.replace("/Date(", "").replace(")/", ""));
}