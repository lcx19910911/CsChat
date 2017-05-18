avalon.ready(function() {
    function moveBottom() {
        $('.message-view-box')[0].scrollTop = $('.message-view-box')[0].scrollHeight - $('.message-view-box')[0].clientHeight
    }
    // 全局
    var chat = avalon.define({
        $id: 'chat',
        historyMessage: [],
        imgScaleSrc: "",
        // 上传图片
        imgChange: function() {
            $('#imgview')[0].src = window.URL.createObjectURL(this.files[0]);
            $('#imgview')[0].onload = function() {
                var img = new Image();
                img.src = this.src;
                window.URL.revokeObjectURL(this.src);
                var urlData = $('#submit_form').attr('action') + '?width=' + img.width + '&height=' + img.height;
                console.log(urlData);
                $('#submit_form').attr('action', urlData);
                img.remove();
                $('#submit_form').submit();
            }
        },
        // 加载图片
        imgSubmit: function() {
            var data = $(window.frames['exec_target'].document.body).find('#imgSrc').attr('imgsrc');
            vm.messageArr.push({mId: 1, mStatus: "sended", mType: "fr", contType: "img", cont: data, img: "images/qsmy0.jpg"})
            console.log(data);
            setTimeout(moveBottom, 80);
        },
        // 图片放大、拖拽
        imgMagnify: function() {
            chat.imgScaleSrc = this.src;
            $('.imgScale img').css({'left':'0','top':'0'});
            $('.imgScale').show();
        },
        imgHide: function() {
            $('.imgScale').hide();
        },
        
        clickJudge: true,
        clientName: relationUser.Name,
        // 展开聊天列表
        chatBack: function() {
            // 防止过快重复点击
            if(chat.clickJudge) {
                chat.clickJudge = false;
                $('.chat-page').show();
                $('.chat-box').animate({
                    "left": 0,
                }, 500, function() {
                    chat.clickJudge = true;
                });

                // 排序：未读置顶
                chat.chatArr.forEach(function(el,i) {
                    if(el.message == "unread") {
                        var obj = el;
                        chat.chatArr.removeAt(i);
                        chat.chatArr.unshift(obj);
                    }
                })
            }
        },
        // 收起聊天列表
        maskClick: function() {
            if(chat.clickJudge) {
                chat.clickJudge = false;
                $('.chat-box').animate({
                    "left": "-100%",
                }, 300, function() {
                    $('.chat-page').hide();
                });
                setTimeout(function() {
                    chat.clickJudge = true;
                }, 500)
            }
        },
        // 选择聊天对象
        listClick: function(obj) {
            chat.maskClick();

            // 打开新聊天
            console.log(obj);
            chat.clientName = obj.name;
        },
        chatArr: [
            {cid: 1, status: "online", message: "readed", img: "images/1.jpg", name: "女神1"},
            {cid: 2, status: "offline", message: "readed", img: "images/1.jpg", name: "女神2"},
            {cid: 3, status: "online", message: "unread", img: "images/qsmy0.jpg", name: "秦时明月1"},
            {cid: 4, status: "offline", message: "readed", img: "images/qsmy0.jpg", name: "秦时明月2"},
            {cid: 5, status: "offline", message: "readed", img: "images/1.jpg", name: "女神3"},
            {cid: 6, status: "online", message: "unread", img: "images/1.jpg", name: "女神4"},
        ],
        // 打开历史记录
        kfHistory: function() {
            $('.message-history').fadeIn(300);

            chat.historyMessage = [];
            for(var i = 0; i < 10;i++) {
                chat.historyMessage.push({mId: 1, mStatus: "sended", mType: "fl", contType: "text", cont: "avalonjs test", img: "images/1.jpg"});
                chat.historyMessage.push({mId: null, mStatus: "", mType: "ftime", contType: "text", cont: "2017.02.03 11:23", img: null});
                chat.historyMessage.push({mId: 2, mStatus: "sended", mType: "fr", contType: "text", cont: "avalonjs test", img: "images/qsmy0.jpg"});
            }
            

        },
        // 加载历史消息
        historyMore: function() {
            if(chat.clickJudge) {
                chat.clickJudge = false;
                var that = $('.history-message-box');
                var oldTop = that[0].scrollHeight;
                console.log(oldTop);
                for(var i = 0; i < 10; i++) {
                    chat.historyMessage.unshift({mId: 4, mStatus: "sended", mType: "fl", contType: "text", cont: "随机数：" + Math.floor(Math.random() * 10), img: "images/1.jpg"});
                }
                setTimeout(function() {
                    that.scrollTop(that[0].scrollHeight - oldTop - that.height() * 0.5);
                    console.log(that[0].scrollHeight);
                    chat.clickJudge = true;
                }, 300);
            }

        },
        // 返回
        historyBack: function() {
            $('.message-history').fadeOut(300);
        },
    })

    // 消息栏
    var vm = avalon.define({
        $id: "mView",
        messageArr: [
            {mId: 1, mStatus: "sended", mType: "fl", contType: "text", cont: "message_test1", img: "images/1.jpg"},
            {mId: 2, mStatus: "sending", mType: "fr", contType: "text", cont: "message_test2", img: "images/qsmy0.jpg"},
            {mId: 3, mStatus: "sendfail", mType: "fr", contType: "text", cont: "message_test3", img: "images/qsmy0.jpg"},
            // null,
            {mId: null, mStatus: "", mType: "ftime", contType: "text", cont: "2017.02.03 11:23", img: null},
            {mId: 4, mStatus: "sended", mType: "fr", contType: "text", cont: "lzhong", img: "images/qsmy0.jpg"},
            {mId: 5, mStatus: "sended", mType: "fl", contType: "text", cont: "wsad", img: "images/1.jpg"},
        ],
        scrollJudge: 0,
        oldTop: 0,
        // 下拉加载
        addMessage: function() {
            if($(this).scrollTop() < 20) {
                var that = $('.message-view-box');
                // console.log(that);
                if($(this).hasClass('more-message')) {
                    vm.oldTop = that[0].scrollHeight;
                    vm.scrollJudge = 2;
                    // console.log(vm.oldTop)
                }
                if(vm.scrollJudge == 0) {
                    vm.scrollJudge = 1;
                    setTimeout(function() {
                        that.scrollTop(20);
                        vm.scrollJudge = 2;
                        vm.oldTop = that[0].scrollHeight;
                        console.log(vm.oldTop)
                    }, 500)
                } else if(vm.scrollJudge == 2) {
                    vm.scrollJudge = 3;


                    //获取聊天记录
                    $.ajax("/chat/GetRecord", { time: searchTime, relationId: relationUser.RelationID }, function (data) {
                        if (data.Code == 0) {
                            $(data.Result).each(function (index, item) {
                                //两分钟未回复，增加时间
                                if ((searchTime - new Date(item.SendTime)) / 1000 > 120) {
                                    vm.messageArr.unshift({ mType: 'ftime', cont: item.SendTime });
                                }
                                else {
                                    searchTime = new Date(item.SendTime);
                                }
                                //判断发送方
                                if (item.UserID == userObj.UserID) {
                                    vm.messageArr.unshift({ mType: 'fl',mStatus: "sended", cont: item.Content, img: userObj.Img });
                                }
                                else {
                                    vm.messageArr.unshift({ mType: 'fr',mStatus: "sended", cont: item.Content, img: relationUser.Img });
                                }
                            })
                        }
                        else {
                            alert(data.Append);
                        }
                    });

                    // 设置滚动条当前位置
                    setTimeout(function() {
                        // console.log(that[0].scrollHeight - vm.oldTop);
                        that.scrollTop(that[0].scrollHeight - vm.oldTop - that.height() * 0.5);
                        if(that.scrollTop() == 0) {
                            that.scrollTop(20);
                        }
                    }, 300);
                    setTimeout(function() {
                        vm.scrollJudge = 0;
                    }, 1000)
                }
            }
        }
        
    })
    // 编辑栏
    var vm1 = avalon.define({
        $id: "em",
        mmm: '',
        keyFn: function(a, e) {
            if(e.key == "Enter") {
                vm1.sendMs(a);
            }
        },
        sendMs: function(a) {
            if(a.length <= 0) return false;
            for(var i = 0; i < a.length; i++) {
                if(a[i] !== ' ') {

                    // 发送消息
                    vm.messageArr.push({mId: 4, mStatus: "sended", mType: "fr", contType: "text", cont: a, img: "images/qsmy0.jpg"});

                    vm1.mmm = "";
                    setTimeout(moveBottom, 80);


                    $.ajax("/chat/addText", { id: guid, message: text, relationId: relationUser.RelationID }, function (data) {
                        if (data.Result) {
                            //修改消息状态
                            $(vm.messageArr).each(function (i, item) {
                                if (item.RecordId == guid) {
                                    item.State = '';
                                }
                            })
                            //发送消息
                            chatHub.server.SendMessage(guid, text, relationUser.RelationID);
                        }
                        else {
                            //修改状态
                            alert(data.Append);
                        }
                    });

                    return false;
                } else {
                    if(i == a.length - 1) {
                        vm1.mmm = "";
                        return false;
                    }
                }
            }
        }
    })

    avalon.scan();
    setTimeout(moveBottom, 80);
})