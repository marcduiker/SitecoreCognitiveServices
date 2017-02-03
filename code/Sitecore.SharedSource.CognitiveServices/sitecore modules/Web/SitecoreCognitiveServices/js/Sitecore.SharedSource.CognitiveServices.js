$(document).ready(function () {
    
    $(".nav-btn").click(function () {
        var selected = "selected";
        var tab = $(this).attr("rel");
        $(".nav-btn").removeClass(selected);
        $(".tab-content").removeClass(selected);
        $(".tab-content." + tab).addClass(selected);
        $(this).addClass(selected);
    });

    var reanlyzeAllForm = ".reanalyze-all-form";
    $(reanlyzeAllForm + "button")
        .click(function(event) {
            event.preventDefault();
            
            var idValue = $(reanlyzeAllForm + " #id").attr("value");
            var langValue = $(reanlyzeAllForm + " #language").attr("value");
            var dbValue = $(reanlyzeAllForm + " #database").attr("value");
            
            $(".form").hide();
            $(".progress-indicator").show();

            $.post(
                $(reanlyzeAllForm).attr("action"),
                {
                    id: idValue,
                    language: langValue,
                    db: dbValue
                }
            ).done(function (r) {
                $(".resultCount").text(r.ItemCount);
                $(".progress-indicator").hide();
                $(".result-display").show();
            });
        });

    var setAltsAllForm = ".reanalyze-all-form";

    $(setAltsAllForm + "button")
        .click(function (event) {
            event.preventDefault();

            var thresholdValue = $(setAltsAllForm + " #threshold").attr("value");
            var idValue = $(setAltsAllForm + " #id").attr("value");
            var langValue = $(setAltsAllForm + " #language").attr("value");
            var dbValue = $(setAltsAllForm + " #database").attr("value");

            $(".form").hide();
            $(".progress-indicator").show();

            $.post(
                $(setAltsAllForm).attr("action"),
                {
                    id: idValue,
                    language: langValue,
                    db: dbValue,
                    threshold: thresholdValue
                }
            ).done(function (r) {
                $(".resultCount").text(r.ItemCount);
                $(".progress-indicator").hide();
                $(".result-display").show();
            });
        });
});