﻿//--------------------------------------
//Admin javascript goes here
//-------------------------------------
$("document").ready(
    function () {
        /*  toggling question menu for admins, make inactive, delete, etc      */
        //mouseOver
        myDiv = $(".question-summary, .question, .question-summary-inactive, .question-inactive");
        myDiv.mouseover(function () {
            $(this).children(".question-report-menu").show();//.css("visibility", "visible");
        })
        //mouseOut
        myDiv.mouseout(function () {
            $(this).children(".question-report-menu").hide();//.css("visibility", "hidden");
        })

        /*  (ugly) Tag summary modigying code toggles invisibility and swaps places between div and textarea     */

        $(".delete-tag-button").click(function () {
            if (confirm('Are you sure?')) {
                var id =  this.id;
                window.location = "/tag/Delete/" + id.substr(9);
            }
        }
             );
        $("[id^=restoresummary]").click(function () {
             var realId = this.id.substr(14);
             $("#textsummary" + realId).toggle();//css("visibility", "hidden");
             $("#divsummary" + realId).toggle();//css("visibility", "visible");
            $("#textsummary" + realId).parent().css("padding-bottom", "");

            //$("#divsummary" + realId).prependTo($("#textsummary" + realId))
        });


        $(".modify-summary-button").click(function () {
            var realId = this.id.substr(9);
            $("#divsummary" + realId).toggle();//css("visibility", "hidden");
            $("#textsummary" + realId).toggle();//css("visibility", "visible");
            $("#textsummary" + realId).parent().css("padding-bottom", "60px");

            //$("#textsummary" + realId).prependTo($("#divsummary" + realId))
        });

     
});