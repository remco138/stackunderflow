﻿function upvote(id) {
    var prev = parseInt($("#totalvote_" + id).text()) + 1;
    $("#totalvote_" + id).text(prev);
    $.ajax({
        type: "POST",
        url: "vote?id=" + id + "&type=up",
        success: function (result) {
        }
    });
    return false;
}
function downvote(id) {
    var prev = parseInt($("#totalvote_" + id).text()) - 1;
    $("#totalvote_" + id).text(prev);
    $.ajax({
        type: "POST",
        url: "vote?id=" + id + "&type=down",
        success: function (result) {
        }
    });
    return false;
}
function notloggedin() {
    alert("You must be logged in to vote");
}