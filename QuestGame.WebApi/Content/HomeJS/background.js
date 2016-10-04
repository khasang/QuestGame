$(document).ready(function () {
    var heightR = $(window).height() - 175;// высота экрана
    $('#backgroundPicture').css({ 'height': heightR });
    $("#playGame").mouseover(function () {
        $("#playGame").css('opacity', '1');
    });
    $("#playGame").mouseout(function () {
        $("#playGame").css('opacity', '0.65');
    });
    $("#news").mouseover(function () {
        $("#news").css('opacity', '1');
    });
    $("#news").mouseout(function () {
        $("#news").css('opacity', '0.65');
    });
    $("#topQuests").mouseover(function () {
        $("#topQuests").css('opacity', '1');
    });
    $("#topQuests").mouseout(function () {
        $("#topQuests").css('opacity', '0.65');
    });
    $("#topPlayers").mouseover(function () {
        $("#topPlayers").css('opacity', '1');
    });
    $("#topPlayers").mouseout(function () {
        $("#topPlayers").css('opacity', '0.65');
    });
    $("#developers").mouseover(function () {
        $("#developers").css('opacity', '1');
    });
    $("#developers").mouseout(function () {
        $("#developers").css('opacity', '0.65');
    });
})