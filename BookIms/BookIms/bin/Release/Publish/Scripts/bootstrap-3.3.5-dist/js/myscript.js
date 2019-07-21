
$(document).ready(function(){
    var t = 2000,
        p,
        hasStarted = false,
        currentIndex = 0,
        length;
    length = $(".slider2_li").length;
    $(".slider2_li:not(:first)").hide();
    $(".slider2_index:first").addClass("slider2_index_selected");

    $(".slider2_index").hover(function(e)
    {
        stop();
        var preIndex = $(".slider2_index").filter(".slider2_index_selected").index();
        currentIndex = $(this).index();
        play(preIndex, currentIndex);
    },function(){
        start();
    });

    function play(preIndex, currentIndex)
    {
        $(".slider2_li").eq(preIndex).fadeOut(500).parent().children().eq(currentIndex).fadeIn(1000);
        $(".slider2_index").removeClass("slider2_index_selected");
        $(".slider2_index").eq(currentIndex).addClass("slider2_index_selected");

    }
    function next()
    {
        var preIndex = currentIndex;
        currentIndex = ++currentIndex % length;
        play(preIndex, currentIndex);
    }
    function start()
    {
        if(!hasStarted)
        {
            p = setInterval(next, t);
            hasStarted = true;
        }
    }
    function stop()
    {
        clearInterval(p);
        hasStarted = false;
    }
    start();
});

//page
    function theFirst()
    {
        alert("�Ѿ��ǵ�һҳ��");
    }

function theEnd()
{
    alert("�Ѿ������һҳ��");
}

