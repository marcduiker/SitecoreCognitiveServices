jQuery.noConflict();

//nav
jQuery(document).ready(function () {
    //toggles tabs based on nav clicks
    jQuery(".nav-btn")
        .click(function () {
            var selected = "selected";
            var tab = jQuery(this).attr("rel");
            jQuery(".nav-btn").removeClass(selected);
            jQuery(".tab-content").removeClass(selected);
            jQuery(".tab-content." + tab).addClass(selected);
            jQuery(this).addClass(selected);
        });
});

//reanalyze
jQuery(document).ready(function () {
    //handles reanalyze all form
    var reanlyzeAllForm = ".reanalyze-all-form";
    jQuery(reanlyzeAllForm + " button")
        .click(function(event) {
            event.preventDefault();
            
            var idValue = jQuery(reanlyzeAllForm + " #id").attr("value");
            var langValue = jQuery(reanlyzeAllForm + " #language").attr("value");
            var dbValue = jQuery(reanlyzeAllForm + " #database").attr("value");
            
            jQuery(".form").hide();
            jQuery(".progress-indicator").show();

            jQuery.post(
                jQuery(reanlyzeAllForm).attr("action"),
                {
                    id: idValue,
                    language: langValue,
                    db: dbValue
                }
            ).done(function (r) {
                jQuery(".result-count").text(r.ItemCount);
                jQuery(".progress-indicator").hide();
                jQuery(".result-display").show();
            });
        });
});

//set alt tags
jQuery(document).ready(function () {
    //handles the submit on the set all alts form
    var setAltsAllForm = ".set-alt-all-form";
    jQuery(setAltsAllForm + " button")
        .click(function (event) {
            event.preventDefault();

            var idValue = jQuery(setAltsAllForm + " #id").attr("value");
            var langValue = jQuery(setAltsAllForm + " #language").attr("value");
            var dbValue = jQuery(setAltsAllForm + " #database").attr("value");
            var thresholdValue = jQuery(setAltsAllForm + " #threshold").val();
            var overwriteValue = jQuery(setAltsAllForm + " #overwrite").is(':checked');

            jQuery(".form").hide();
            jQuery(".progress-indicator").show();

            jQuery.post(
                jQuery(setAltsAllForm).attr("action"),
                {
                    id: idValue,
                    language: langValue,
                    db: dbValue,
                    threshold: thresholdValue,
                    overwrite: overwriteValue
                }
            ).done(function (r) {
                jQuery(".result-modified").text(r.ItemsModified);
                jQuery(".result-count").text(r.ItemCount);
                jQuery(".progress-indicator").hide();
                jQuery(".result-display").show();
            }).always(function () {
                jQuery(".progress-indicator").hide();
            });
        });
});

//image search
jQuery(document).ready(function () {
    var config = {
        '.chosen-select': { width: "100%" }
    }
    var results = [];
    for (var selector in config) {
        var elements = jQuery(selector);
        for (var i = 0; i < elements.length; i++) {
            results.push(new Chosen(elements[i], config[selector]));
        }
    }

    jQuery(".slider-range").each(function () {
        jQuery(this).slider({
            range: true,
            min: 0,
            max: 100,
            values: [0, 100],
            slide: function (event, ui) {
                var parent = jQuery(this).parent('.emotion-item');
                var filterValue = jQuery(parent).find(".filter-value");
                var rangeVal = ui.values[0] + " - " + ui.values[1];
                rangeVal += (filterValue.data("field") == "age") ? "" : "%";
                filterValue.html(rangeVal);
                filterValue.data('min', ui.values[0]);
                filterValue.data('max', ui.values[1]);
            }
        });

        var parent = jQuery(this).parent('.emotion-item');
        var filterValue = jQuery(parent).find(".filter-value");
        var htmlVal = "0 - 100";
        htmlVal += (filterValue.data("field") == "age") ? "" : "%";
        filterValue.html(htmlVal);
        filterValue.data('min', 0);
        filterValue.data('max', 100);
    });

    //closes modal and send selected image back to RTE
    var queryObj;
    var imageSearchForm = ".image-search-form";
    var rteSearchForm = ".rte-search-form";
    jQuery(imageSearchForm + " .form-submit")
        .click(function (event) {
            event.preventDefault();

            var img = jQuery(".result-items .selected");
            if (img.length)
                CloseRadWindow(jQuery(".result-items .selected").html());
            else
                alert("You need to select an image.");
        });

    //closes modal on cancel press
    jQuery(imageSearchForm + " .form-cancel")
        .click(function (event) {
            event.preventDefault();

            CloseRadWindow();
        });

    //will perform search if you type and wait
    var imageSearchInput = ".rte-search-input";
    jQuery(imageSearchInput)
        .on('input', function (e) {
            clearTimeout(queryObj);
            queryObj = setTimeout(RunQuery, 500);
        });

    //performs search on 'enter-press' on the form
    jQuery(imageSearchForm + " .search-submit, " + imageSearchForm + " .cognitiveSearchButton")
        .click(function (event) {
            event.preventDefault();
            
            clearTimeout(queryObj);
            RunQuery();
        });

    //performs image search
    function RunQuery() {
        var textValue = jQuery(imageSearchForm + " .rte-search-input").val();
        var langValue = jQuery(imageSearchForm + " #language").attr("value");
        var dbValue = jQuery(imageSearchForm + " #database").attr("value");
        var gender = jQuery(imageSearchForm + " #gender").val();
        var glasses = jQuery(imageSearchForm + " #glasses").val();
        var size = jQuery(imageSearchForm + " #size").val();

        jQuery(rteSearchForm + " .progress-indicator").show();
        jQuery(".search-results").hide();

        var tags = GetTagParameters();
        var ranges = GetRangeParameters();

        jQuery.post(
            jQuery(imageSearchForm).attr("action"),
                {
                    formParameters: [],
                    tagParameters: tags,
                    rangeParameters: ranges,
                    gender: gender,
                    glasses: glasses,
                    size: size,
                    language: langValue,
                    db: dbValue
                }
            ).done(function (r) {
                jQuery(".result-count").text(r.ResultCount);
                jQuery(".result-items").text("");
                jQuery(".search-results").show();
                for (var i = 0; i < r.Results.length; i++) {
                    var d = r.Results[i];
                    if (d.Url != undefined) {
                        jQuery(".result-items").append("<div class='result-img-wrap'><img src=\"" + d.Url + "\" alt=\"" + d.Alt + "\" /></div>");
                    }
                }

                jQuery(".result-img-wrap")
                    .on("click", function () {
                        jQuery(".result-items .selected").removeClass("selected");
                        jQuery(this).addClass("selected");
                    });
            }).always(function () {
                jQuery(rteSearchForm + " .progress-indicator").hide();
            });
    }

    function GetRangeParameters() {
        var params = [];

        //tags
        var filterElements = jQuery('.filter-value');
        if (filterElements.length > 0) {

            filterElements.each(function () {
                var values = [];
                values.push(jQuery(this).data('min'));
                values.push(jQuery(this).data('max'));

                params.push({
                    key: jQuery(this).data('field'),
                    value: values
                });
            });
        }

        return params;
    }

    function GetTagParameters() {
        var params = [];
        params.push({
            key: "tags",
            value: [""]
        });

        //tags
        var tagElements = jQuery('.search-choice span');
        if (tagElements.length > 0) {

            var values = [];

            tagElements.each(function () {
                var text = jQuery(this).text();
                text = text.substring(0, text.indexOf(' ('));
                text = text.toLowerCase();
                values.push(text);
            });

            params.push({
                key: "tags",
                value: values
            });
        }

        return params;
    }

    //get results for the first load
    if (jQuery(imageSearchForm).length)
        RunQuery();
});

//ole chat
jQuery(document).ready(function () {
    //ole chat
    var chatInput = ".chat-input";
    var chatForm = ".chat-form";
    var chatConversation = ".chat-conversation";
    var chatConversationData = {};

    //sends chat text on 'enter-press' on the form
    jQuery(chatForm + " .chat-submit")
        .click(function (event) {
            event.preventDefault();
            var queryValue = jQuery(chatInput).val();
            var langValue = jQuery(".chat-lang").val();
            var dbValue = jQuery(".chat-db").val();
            var idValue = jQuery(".chat-id").val();

            jQuery(chatInput).val("");
            UpdateChatWindow(queryValue, "user");
            
            jQuery
                .post(jQuery(chatForm).attr("action"), GenerateActivity(queryValue, langValue, dbValue, idValue))
                .done(function(r) {
                    chatConversationData = r.conversation;
                    UpdateChatWindow(r.Text, "bot");
                });
        });

    function UpdateChatWindow(text, type) {
        var convoBox = jQuery(chatConversation);
        convoBox.append("<div class='" + type + "'><span class='message'>" + text + "<span class='icon'></span></span></div>");
        convoBox.scrollTop(convoBox[0].scrollHeight - convoBox.height());
    }

    function GenerateActivity(query, langValue, dbValue, idValue) {

        var data = {
            language: "en",
            database: "master",
            id: "{110D559F-DEA5-42EA-9C1C-8A5DF7E70EF9}"
        };

        var activity = {
            type: "message",
            id: GenerateGuid(),
            timestamp: Date.now(),
            channelId: "ole",
            from: {
                id: "2c1c7fa3",
                name: "User1"
            },
            conversation: {
                isGroup: false,
                id: "8a684db8",
                name: "Conv1"
            },
            recipient: {
                id: "56800324",
                name: "Bot1"
            },
            text: query,
            attachments: [],
            entities: [],
            channelData: JSON.stringify(data)
        }

        return activity;
    }

    function GenerateGuid() {
        function s4() {
            return Math.floor((1 + Math.random()) * 0x10000)
              .toString(16)
              .substring(1);
        }
        return s4() + s4() + '-' + s4() + '-' + s4() + '-' + s4() + '-' + s4() + s4() + s4();
    }
});

//closes the search modal and passes value back to the RTE
function CloseRadWindow(value) {

    var radWindow;

    if (window.radWindow)
        radWindow = window.radWindow;
    else if (window.frameElement && window.frameElement.radWindow)
        radWindow = window.frameElement.radWindow;
    else
        window.close();

    radWindow.Close(value);
}