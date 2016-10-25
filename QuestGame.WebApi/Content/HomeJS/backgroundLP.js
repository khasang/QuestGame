$(document).ready(function () {
    var widthR = $(window).width();
    $('.fullPageSlide').css({ 'width': widthR });

    var slides = $(".slider .slides").children(".slide");
    var i = slides.length;
    var offset = i * widthR;
    i--;

    $(".slider .slides").css('width', offset);

    offset = 0;
    $(".slider .next").click(function () {
        if (offset < widthR * i) {
            offset += widthR;
            $(".slider .slides").css("transform", "translate3d(-" + offset + "px, 0px, 0px)");
        }
        else
        {
            offset -= 2*widthR;
            $(".slider .slides").css("transform", "translate3d(-" + offset + "px, 0px, 0px)");
        }
    });

    $(".slider .prev").click(function () {
        if (offset > 0) {
            offset -= widthR;
            $(".slider .slides").css("transform", "translate3d(-" + offset + "px, 0px, 0px)");
        }
        else {
            offset += 2*widthR;
            $(".slider .slides").css("transform", "translate3d(-" + offset + "px, 0px, 0px)");
        }
    });
});